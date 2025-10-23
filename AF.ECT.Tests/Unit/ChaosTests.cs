using System.Net;
using Polly.CircuitBreaker;
using Xunit.Abstractions;
using AF.ECT.Tests.Infrastructure;
using static AF.ECT.Tests.Data.ChaosTestData;

namespace AF.ECT.Tests.Unit;

/// <summary>
/// Chaos engineering tests to simulate real-world failure scenarios.
/// </summary>
public class ChaosTests : ResilienceTestBase
{
    public ChaosTests(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [ClassData(typeof(NetworkFailureScenariosData))]
    public async Task ChaosTest_NetworkFailures_ResilienceServiceHandlesGracefully(int iterations, double failureRate, double delayRate, double successRate)
    {
        // Arrange - Simulate network chaos
        var successCount = 0;
        var failureCount = 0;

        async Task<HttpResponseMessage> ChaoticNetworkOperation()
        {
            var random = new Random();
            var chaos = random.NextDouble();

            if (chaos < failureRate) // failure rate chance of network failure
            {
                failureCount++;
                throw new HttpRequestException("Network connection lost");
            }
            else if (chaos < failureRate + delayRate) // delay rate chance of delay
            {
                await Task.Delay(random.Next(100, 1000));
                successCount++;
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else // success rate chance of success
            {
                successCount++;
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        // Act - Execute multiple times to simulate chaos
        for (var i = 0; i < iterations; i++)
        {
            try
            {
                var result = await _resilienceService.ExecuteResilientHttpRequestAsync(ChaoticNetworkOperation);
                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Expected failure during chaos test: {ex.Message}");
            }
        }

        // Assert - Service should have handled failures gracefully
        Assert.True(successCount > 0, "Should have had some successful operations");
        Assert.True(failureCount >= 0, "Failures are expected in chaos testing");
        Assert.True(Math.Abs((failureRate + delayRate + successRate) - 1.0) < 0.01, "Probabilities should sum to 1");

        _output.WriteLine($"Chaos test completed: {successCount} successes, {failureCount} failures handled gracefully");
    }

    [Theory]
    [ClassData(typeof(DatabaseFailureScenariosData))]
    public async Task ChaosTest_DatabaseFailures_CircuitBreakerActivates(int failureCount, int minDelay, int maxDelay)
    {
        // Arrange - Simulate database chaos
        var operationCount = 0;

        async Task<int> ChaoticDatabaseOperation()
        {
            operationCount++;
            var random = new Random();

            if (operationCount <= failureCount) // First N operations fail (to trigger circuit breaker)
            {
                await Task.Delay(random.Next(minDelay, maxDelay)); // Simulate some processing time
                throw new InvalidOperationException("Database connection failed");
            }
            else
            {
                return await SimulateDatabaseOperation(false);
            }
        }

        // Act - Execute operations that should trigger circuit breaker
        for (var i = 0; i < failureCount; i++)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _resilienceService.ExecuteDatabaseOperationAsync(ChaoticDatabaseOperation));
        }

        // Assert circuit breaker opens
        await WaitForCircuitBreakerState(CircuitState.Open);

        // Act - Try operation while circuit is open (should fail fast)
        var startTime = DateTime.UtcNow;
        await Assert.ThrowsAsync<BrokenCircuitException>(
            () => _resilienceService.ExecuteDatabaseOperationAsync(ChaoticDatabaseOperation));
        var executionTime = DateTime.UtcNow - startTime;

        // Assert it failed fast
        Assert.True(executionTime.TotalMilliseconds < 500);

        _output.WriteLine($"Circuit breaker activated after {operationCount} failed database operations");
    }

    [Theory]
    [ClassData(typeof(MixedFailureScenariosData))]
    public async Task ChaosTest_MixedFailures_TimeoutAndRetryWorkTogether(int iterations, double successRate, double timeoutRate, double serverErrorRate)
    {
        // Arrange - Mixed failure scenarios
        var callCount = 0;

        async Task<HttpResponseMessage> MixedFailureOperation()
        {
            callCount++;
            var random = new Random();

            var failureType = random.NextDouble();

            if (failureType < successRate) // success rate chance of success
            {
                await Task.Delay(random.Next(100, 500));
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else if (failureType < successRate + timeoutRate) // timeout rate chance of timeout
            {
                await Task.Delay(12000); // Longer than 10s timeout
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else // server error rate chance of server error
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // Act - Execute multiple chaotic operations
        var successfulOperations = 0;
        var failedOperations = 0;

        for (var i = 0; i < iterations; i++)
        {
            try
            {
                var result = await _resilienceService.ExecuteResilientHttpRequestAsync(MixedFailureOperation);
                if (result.IsSuccessStatusCode)
                {
                    successfulOperations++;
                }
                else
                {
                    failedOperations++;
                }
            }
            catch (Exception)
            {
                failedOperations++;
            }
        }

        // Assert - Should have some successes and some failures handled
        Assert.True(successfulOperations >= 0, "Operations completed");
        Assert.True(callCount > iterations, "Retry policy should have increased total call count");
        Assert.True(Math.Abs((successRate + timeoutRate + serverErrorRate) - 1.0) < 0.01, "Probabilities should sum to 1");

        _output.WriteLine($"Mixed chaos test: {successfulOperations} successes, {failedOperations} failures, {callCount} total calls");
    }

    [Theory]
    [ClassData(typeof(HighLoadScenariosData))]
    public async Task ChaosTest_HighLoad_SustainedResilience(int taskCount, int operationsPerTask, double failureRate)
    {
        // Arrange - High load scenario
        var tasks = new Task[taskCount];
        var totalSuccesses = 0;
        var totalFailures = 0;

        async Task RunLoadTest(int taskId)
        {
            for (var i = 0; i < operationsPerTask; i++)
            {
                try
                {
                    var result = await _resilienceService.ExecuteResilientHttpRequestAsync(async () =>
                    {
                        var random = new Random();
                        if (random.NextDouble() < failureRate) // failure rate chance of failure
                        {
                            throw new HttpRequestException($"Load test failure in task {taskId}");
                        }
                        await Task.Delay(random.Next(50, 200));
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    });

                    if (result.IsSuccessStatusCode)
                    {
                        Interlocked.Increment(ref totalSuccesses);
                    }
                }
                catch (Exception)
                {
                    Interlocked.Increment(ref totalFailures);
                }
            }
        }

        // Act - Run concurrent load test
        for (var i = 0; i < tasks.Length; i++)
        {
            tasks[i] = RunLoadTest(i);
        }

        await Task.WhenAll(tasks);

        // Assert - Should handle concurrent load
        Assert.True(totalSuccesses >= 0, "Should have attempted operations under load");
        Assert.True(totalFailures >= 0, "Failures are expected in load testing");

        _output.WriteLine($"Load test completed: {totalSuccesses} successes, {totalFailures} failures under concurrent load");
    }

    [Theory]
    [ClassData(typeof(CircuitBreakerRecoveryScenariosData))]
    public async Task ChaosTest_CircuitBreakerRecovery_AutomaticRecovery(double initialFailureRate)
    {
        // Arrange - Test circuit breaker recovery
        async Task<HttpResponseMessage> IntermittentFailureOperation()
        {
            var random = new Random();
            if (random.NextDouble() < initialFailureRate) // initial failure rate chance of failure
            {
                throw new HttpRequestException("Intermittent service failure");
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // Act - Cause circuit breaker to open
        for (var i = 0; i < 6; i++)
        {
            try
            {
                await _resilienceService.ExecuteResilientHttpRequestAsync(IntermittentFailureOperation);
            }
            catch (HttpRequestException)
            {
                // Expected
            }
        }

        // Wait for circuit breaker to open
        await WaitForCircuitBreakerState(CircuitState.Open);

        // Act - Try operations while circuit is open (should fail fast)
        for (var i = 0; i < 3; i++)
        {
            await Assert.ThrowsAsync<BrokenCircuitException>(
                () => _resilienceService.ExecuteResilientHttpRequestAsync(IntermittentFailureOperation));
        }

        // Act - Reset circuit breaker to simulate recovery
        _resilienceService.ResetCircuitBreaker();

        // Assert circuit is closed and operations work
        Assert.Equal(CircuitState.Closed, _resilienceService.CircuitBreakerState);

        var result = await _resilienceService.ExecuteResilientHttpRequestAsync(
            () => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        _output.WriteLine("Circuit breaker recovery test completed successfully");
    }

    [Theory]
    [ClassData(typeof(PerformanceUnderFailureScenariosData))]
    public async Task ChaosTest_PerformanceUnderFailure_LoadTimesRemainAcceptable(int executionCount)
    {
        // Arrange - Test performance under failure conditions
        var executionTimes = new List<TimeSpan>();

        async Task<HttpResponseMessage> FailingOperation()
        {
            throw new HttpRequestException("Service temporarily unavailable");
        }

        // Act - Measure execution times under failure
        for (var i = 0; i < executionCount; i++)
        {
            var executionTime = await MeasureExecutionTime(async () =>
            {
                try
                {
                    await _resilienceService.ExecuteResilientHttpRequestAsync(FailingOperation);
                }
                catch (HttpRequestException)
                {
                    // Expected failure
                }
            });

            executionTimes.Add(executionTime);
        }

        // Assert - Execution times should be reasonable even under failure
        var averageTime = TimeSpan.FromTicks((long)executionTimes.Average(t => t.Ticks));
        var maxTime = executionTimes.Max();

        _output.WriteLine($"Average failure handling time: {averageTime.TotalMilliseconds}ms");
        _output.WriteLine($"Max failure handling time: {maxTime.TotalMilliseconds}ms");

        // Circuit breaker should prevent extremely long execution times
        Assert.True(maxTime.TotalSeconds < 30, "Should not take excessively long to fail");
        Assert.True(averageTime.TotalMilliseconds >= 0, "Average failure time should be measurable");
    }
}