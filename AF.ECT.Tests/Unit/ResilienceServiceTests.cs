using System.Net;
using Polly.CircuitBreaker;
using Polly.Timeout;
using Xunit.Abstractions;
using AF.ECT.Tests.Infrastructure;
using static AF.ECT.Tests.Data.ResilienceServiceTestData;

namespace AF.ECT.Tests.Unit;

/// <summary>
/// Tests for the ResilienceService class.
/// </summary>
public class ResilienceServiceTests : ResilienceTestBase
{
    public ResilienceServiceTests(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [ClassData(typeof(RetryScenariosData))]
    public async Task ExecuteWithRetryAsync_RetriesOnFailure_ThenSucceeds(int failuresBeforeSuccess, int expectedResult)
    {
        // Arrange
        var callCount = 0;

        async Task<int> FailingThenSucceedingOperation()
        {
            callCount++;
            if (callCount <= failuresBeforeSuccess)
            {
                throw new HttpRequestException("Temporary failure");
            }
            return expectedResult;
        }

        // Act
        var result = await _resilienceService.ExecuteWithRetryAsync(FailingThenSucceedingOperation);

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(failuresBeforeSuccess + 1, callCount); // Should have been called failures + 1 times
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

        Assert.Equal(5, callCount); // Initial call + 4 retries
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

    [Theory]
    [ClassData(typeof(CircuitBreakerStateTransitionsData))]
    public async Task CircuitBreaker_OpensAfterFailures_ThenRecovers(int failureThreshold)
    {
        // Arrange
        var callCount = 0;

        async Task<HttpResponseMessage> FailingOperation()
        {
            callCount++;
            throw new HttpRequestException("Service unavailable");
        }

        // Act - Cause circuit breaker to open
        for (var i = 0; i < failureThreshold; i++) // failure threshold failures to trigger circuit breaker
        {
            try
            {
                await _resilienceService.ExecuteResilientHttpRequestAsync(FailingOperation);
            }
            catch (Exception)
            {
                // Expected any exception during failure induction
            }
        }

        // Assert circuit state
        if (failureThreshold >= 5)
        {
            await WaitForCircuitBreakerState(CircuitState.Open);
            Assert.Equal(CircuitState.Open, _resilienceService.CircuitBreakerState);
        }
        else
        {
            Assert.Equal(CircuitState.Closed, _resilienceService.CircuitBreakerState);
        }

        // Act - Try operation
        var startTime = DateTime.UtcNow;
        var circuitStateBeforeTestCall = _resilienceService.CircuitBreakerState;
        try
        {
            await _resilienceService.ExecuteResilientHttpRequestAsync(FailingOperation);
            Assert.Fail("Expected exception");
        }
        catch (BrokenCircuitException)
        {
            Assert.Equal(CircuitState.Open, circuitStateBeforeTestCall);
            var executionTime = DateTime.UtcNow - startTime;
            // Assert it failed fast
            Assert.True(executionTime.TotalMilliseconds < 100);
        }
        catch (HttpRequestException)
        {
            Assert.Equal(CircuitState.Closed, circuitStateBeforeTestCall);
        }

        _output.WriteLine($"Circuit breaker test with {failureThreshold} failures completed");

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

    [Theory]
    [ClassData(typeof(DatabaseOperationScenariosData))]
    public async Task ExecuteDatabaseOperationAsync_AppliesTimeoutAndRetry(int failuresBeforeSuccess, int expectedResult)
    {
        // Arrange
        var callCount = 0;

        async Task<int> SlowThenFastOperation()
        {
            callCount++;
            if (callCount <= failuresBeforeSuccess)
            {
                await Task.Delay(2000); // Longer than database timeout (5s), but should be retried
                throw new TimeoutException("Database timeout");
            }
            return expectedResult;
        }

        // Act
        var result = await _resilienceService.ExecuteDatabaseOperationAsync(SlowThenFastOperation);

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(failuresBeforeSuccess + 1, callCount); // Should have failed failures times then succeeded
        _output.WriteLine($"Database operation succeeded after {callCount} attempts");
    }

    [Theory]
    [ClassData(typeof(TimeoutScenariosData))]
    public async Task ExecuteResilientHttpRequestAsync_TimeoutPolicy_PreventsLongRunningRequests(int timeoutMs)
    {
        // Arrange
        async Task<HttpResponseMessage> LongRunningOperation()
        {
            await Task.Delay(timeoutMs); // Delay longer than timeout
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // Act & Assert
        var exception = await Assert.ThrowsAsync<TimeoutRejectedException>(
            () => _resilienceService.ExecuteResilientHttpRequestAsync(LongRunningOperation));

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

    [Theory]
    [ClassData(typeof(ExponentialBackoffScenariosData))]
    public async Task ExecuteWithRetryAsync_ExponentialBackoff_IncreasesDelay(int failuresBeforeSuccess, int expectedResult)
    {
        // Arrange
        var attemptTimes = new List<DateTime>();
        var callCount = 0;

        async Task<int> FailingOperation()
        {
            callCount++;
            attemptTimes.Add(DateTime.UtcNow);

            if (callCount <= failuresBeforeSuccess) // Fail N times, succeed on N+1th
            {
                throw new HttpRequestException("Temporary failure");
            }

            return expectedResult;
        }

        // Act
        var result = await _resilienceService.ExecuteWithRetryAsync(FailingOperation);

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(failuresBeforeSuccess + 1, callCount);

        // Verify exponential backoff (approximately)
        if (callCount > 2)
        {
            var delay1 = attemptTimes[1] - attemptTimes[0];
            var delay2 = attemptTimes[2] - attemptTimes[1];

            // Each delay should be roughly double the previous (with some tolerance for timing)
            Assert.True(delay2.TotalMilliseconds >= delay1.TotalMilliseconds * 1.5);
        }

        _output.WriteLine($"Exponential backoff test completed with {callCount} attempts");
    }
}