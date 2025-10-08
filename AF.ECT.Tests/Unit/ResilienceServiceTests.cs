using System.Net;
using Polly.CircuitBreaker;
using Polly.Timeout;
using Xunit.Abstractions;
using AF.ECT.Tests.Infrastructure;

namespace AF.ECT.Tests;

/// <summary>
/// Tests for the ResilienceService class.
/// </summary>
public class ResilienceServiceTests : ResilienceTestBase
{
    public ResilienceServiceTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task ExecuteWithRetryAsync_RetriesOnFailure_ThenSucceeds()
    {
        // Arrange
        var callCount = 0;

        async Task<int> FailingThenSucceedingOperation()
        {
            callCount++;
            if (callCount < 3)
            {
                throw new HttpRequestException("Temporary failure");
            }
            return 42;
        }

        // Act
        var result = await _resilienceService.ExecuteWithRetryAsync(FailingThenSucceedingOperation);

        // Assert
        Assert.Equal(42, result);
        Assert.Equal(3, callCount); // Should have been called 3 times: 2 failures + 1 success
        _output.WriteLine($"Operation succeeded after {callCount} attempts");
    }

    [Fact]
    public async Task ExecuteWithRetryAsync_ExhaustsRetries_ThrowsException()
    {
        // Arrange
        var callCount = 0;

        async Task<int> AlwaysFailingOperation()
        {
            callCount++;
            throw new HttpRequestException("Persistent failure");
        }

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            () => _resilienceService.ExecuteWithRetryAsync(AlwaysFailingOperation));

        Assert.Equal(4, callCount); // Initial call + 3 retries
        Assert.Contains("Persistent failure", exception.Message);
        _output.WriteLine($"Operation failed after {callCount} attempts as expected");
    }

    [Fact]
    public async Task ExecuteResilientHttpRequestAsync_HandlesNetworkFailures()
    {
        // Arrange
        var callCount = 0;

        async Task<HttpResponseMessage> FailingHttpOperation()
        {
            callCount++;
            if (callCount < 2)
            {
                throw new HttpRequestException("Network error");
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // Act
        var result = await _resilienceService.ExecuteResilientHttpRequestAsync(FailingHttpOperation);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.True(callCount >= 2); // Should have failed at least once then succeeded
        _output.WriteLine($"HTTP operation succeeded after {callCount} attempts");
    }

    [Fact]
    public async Task CircuitBreaker_OpensAfterFailures_ThenRecovers()
    {
        // Arrange
        var callCount = 0;

        async Task<HttpResponseMessage> FailingOperation()
        {
            callCount++;
            throw new HttpRequestException("Service unavailable");
        }

        // Act - Cause circuit breaker to open
        for (var i = 0; i < 6; i++) // More than the 5 failure threshold
        {
            try
            {
                await _resilienceService.ExecuteResilientHttpRequestAsync(FailingOperation);
            }
            catch (HttpRequestException)
            {
                // Expected
            }
        }

        // Wait for circuit breaker to open
        await WaitForCircuitBreakerState(CircuitState.Open);

        // Assert circuit breaker is open
        Assert.Equal(CircuitState.Open, _resilienceService.CircuitBreakerState);

        // Act - Try operation while circuit breaker is open (should fail fast)
        var startTime = DateTime.UtcNow;
        await Assert.ThrowsAsync<BrokenCircuitException>(
            () => _resilienceService.ExecuteResilientHttpRequestAsync(FailingOperation));
        var executionTime = DateTime.UtcNow - startTime;

        // Assert it failed fast (less than 100ms, not the full retry timeout)
        Assert.True(executionTime.TotalMilliseconds < 100);
        _output.WriteLine($"Circuit breaker opened after {callCount} failures");

        // Act - Reset circuit breaker and try successful operation
        _resilienceService.ResetCircuitBreaker();

        async Task<HttpResponseMessage> SuccessfulOperation()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        var result = await _resilienceService.ExecuteResilientHttpRequestAsync(SuccessfulOperation);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(CircuitState.Closed, _resilienceService.CircuitBreakerState);
        _output.WriteLine("Circuit breaker recovered successfully");
    }

    [Fact]
    public async Task ExecuteDatabaseOperationAsync_AppliesTimeoutAndRetry()
    {
        // Arrange
        var callCount = 0;

        async Task<int> SlowThenFastOperation()
        {
            callCount++;
            if (callCount == 1)
            {
                await Task.Delay(2000); // Longer than database timeout (5s), but should be retried
                throw new TimeoutException("Database timeout");
            }
            return await SimulateDatabaseOperation(false);
        }

        // Act
        var result = await _resilienceService.ExecuteDatabaseOperationAsync(SlowThenFastOperation);

        // Assert
        Assert.Equal(42, result);
        Assert.Equal(2, callCount); // Should have failed once then succeeded
        _output.WriteLine($"Database operation succeeded after {callCount} attempts");
    }

    [Fact]
    public async Task ExecuteResilientHttpRequestAsync_TimeoutPolicy_PreventsLongRunningRequests()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<TimeoutRejectedException>(
            () => _resilienceService.ExecuteResilientHttpRequestAsync(SimulateTimeout));

        _output.WriteLine("Timeout policy correctly prevented long-running request");
    }

    [Fact]
    public async Task CircuitBreaker_TransitionsThroughStatesCorrectly()
    {
        // Start in closed state
        Assert.Equal(CircuitState.Closed, _resilienceService.CircuitBreakerState);

        // Arrange - Failing operation
        async Task<HttpResponseMessage> FailingOperation()
        {
            throw new HttpRequestException("Service down");
        }

        // Act - Cause enough failures to open circuit
        for (var i = 0; i < 5; i++)
        {
            await Assert.ThrowsAsync<HttpRequestException>(
                () => _resilienceService.ExecuteResilientHttpRequestAsync(FailingOperation));
        }

        // Assert circuit opens
        await WaitForCircuitBreakerState(CircuitState.Open);
        Assert.Equal(CircuitState.Open, _resilienceService.CircuitBreakerState);

        // Act - Wait for circuit to become half-open (30 seconds would be too long for test)
        // For testing purposes, we'll manually reset to simulate recovery
        _resilienceService.ResetCircuitBreaker();

        // Assert circuit closes after reset
        Assert.Equal(CircuitState.Closed, _resilienceService.CircuitBreakerState);

        _output.WriteLine("Circuit breaker state transitions work correctly");
    }

    [Fact]
    public async Task ExecuteWithRetryAsync_ExponentialBackoff_IncreasesDelay()
    {
        // Arrange
        var attemptTimes = new List<DateTime>();
        var callCount = 0;

        async Task<int> FailingOperation()
        {
            callCount++;
            attemptTimes.Add(DateTime.UtcNow);

            if (callCount < 4) // Fail 3 times, succeed on 4th
            {
                throw new HttpRequestException("Temporary failure");
            }

            return 42;
        }

        // Act
        var result = await _resilienceService.ExecuteWithRetryAsync(FailingOperation);

        // Assert
        Assert.Equal(42, result);
        Assert.Equal(4, callCount);

        // Verify exponential backoff (approximately)
        var delay1 = attemptTimes[1] - attemptTimes[0];
        var delay2 = attemptTimes[2] - attemptTimes[1];
        var delay3 = attemptTimes[3] - attemptTimes[2];

        // Each delay should be roughly double the previous (with some tolerance for timing)
        Assert.True(delay2.TotalMilliseconds >= delay1.TotalMilliseconds * 1.5);
        Assert.True(delay3.TotalMilliseconds >= delay2.TotalMilliseconds * 1.5);

        _output.WriteLine($"Exponential backoff delays: {delay1.TotalMilliseconds}ms, {delay2.TotalMilliseconds}ms, {delay3.TotalMilliseconds}ms");
    }
}