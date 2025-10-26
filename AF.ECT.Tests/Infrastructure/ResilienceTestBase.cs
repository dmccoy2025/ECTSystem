using System.Net;
using Polly.CircuitBreaker;
using AF.ECT.Server.Services;
using AF.ECT.Server.Services.Interfaces;
using Xunit.Abstractions;

namespace AF.ECT.Tests.Infrastructure;

/// <summary>
/// Base class for resilience testing with fault injection capabilities.
/// </summary>
public abstract class ResilienceTestBase : IDisposable
{
    protected readonly IResilienceService _resilienceService;
    protected readonly Mock<ILogger<ResilienceService>> _loggerMock;
    protected readonly ITestOutputHelper _output;

    protected ResilienceTestBase(ITestOutputHelper output)
    {
        _output = output;
        _loggerMock = new Mock<ILogger<ResilienceService>>();
        _resilienceService = new ResilienceService();
    }

    /// <summary>
    /// Simulates a network failure by throwing HttpRequestException.
    /// </summary>
    protected static async Task<HttpResponseMessage> SimulateNetworkFailure()
    {
        await Task.Delay(100); // Simulate network delay
        throw new HttpRequestException("Network connection failed");
    }

    /// <summary>
    /// Simulates a timeout by delaying execution.
    /// </summary>
    protected static async Task<HttpResponseMessage> SimulateTimeout()
    {
        await Task.Delay(15000); // Delay longer than timeout policy (10s)
        return new HttpResponseMessage(HttpStatusCode.OK);
    }

    /// <summary>
    /// Simulates a server error response.
    /// </summary>
    protected static Task<HttpResponseMessage> SimulateServerError()
    {
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
    }

    /// <summary>
    /// Simulates a successful response after some delay.
    /// </summary>
    protected static async Task<HttpResponseMessage> SimulateSuccessWithDelay(int delayMs = 500)
    {
        await Task.Delay(delayMs);
        return new HttpResponseMessage(HttpStatusCode.OK);
    }

    /// <summary>
    /// Simulates a database operation that may fail.
    /// </summary>
    protected static async Task<int> SimulateDatabaseOperation(bool shouldFail = false)
    {
        if (shouldFail)
        {
            throw new InvalidOperationException("Database operation failed");
        }

        await Task.Delay(100); // Simulate database operation time
        return 42; // Return some data
    }

    /// <summary>
    /// Waits for the circuit breaker to transition to a specific state.
    /// </summary>
    protected async Task WaitForCircuitBreakerState(CircuitState expectedState, int maxWaitMs = 5000)
    {
        var startTime = DateTime.UtcNow;

        while (_resilienceService.CircuitBreakerState != expectedState)
        {
            if ((DateTime.UtcNow - startTime).TotalMilliseconds > maxWaitMs)
            {
                throw new TimeoutException($"Circuit breaker did not reach state {expectedState} within {maxWaitMs}ms");
            }

            await Task.Delay(100);
        }

        _output.WriteLine($"Circuit breaker reached state: {expectedState}");
    }

    /// <summary>
    /// Measures the execution time of an action.
    /// </summary>
    protected static async Task<TimeSpan> MeasureExecutionTime(Func<Task> action)
    {
        var startTime = DateTime.UtcNow;
        await action();
        return DateTime.UtcNow - startTime;
    }

    /// <summary>
    /// Asserts that an action completes within a specified time range.
    /// </summary>
    protected async Task AssertExecutionTime(Func<Task> action, TimeSpan minTime, TimeSpan maxTime, string operationName = "operation")
    {
        var executionTime = await MeasureExecutionTime(action);

        _output.WriteLine($"{operationName} took {executionTime.TotalMilliseconds}ms");

        Assert.InRange(executionTime, minTime, maxTime);
    }

    public void Dispose()
    {
        // Reset circuit breaker if it's in a broken state
        if (_resilienceService.CircuitBreakerState == CircuitState.Open)
        {
            _resilienceService.ResetCircuitBreaker();
        }
    }
}

/// <summary>
/// Extension methods for resilience testing.
/// </summary>
public static class ResilienceTestExtensions
{
    /// <summary>
    /// Retries an assertion until it passes or times out.
    /// Useful for testing eventual consistency or async operations.
    /// </summary>
    public static async Task AssertEventually(this ResilienceTestBase testBase,
        Func<Task> assertion,
        TimeSpan? timeout = null,
        TimeSpan? checkInterval = null,
        string failureMessage = "Assertion did not pass within timeout")
    {
        timeout ??= TimeSpan.FromSeconds(5);
        checkInterval ??= TimeSpan.FromMilliseconds(100);

        var startTime = DateTime.UtcNow;
        Exception? lastException = null;

        while (DateTime.UtcNow - startTime < timeout)
        {
            try
            {
                await assertion();
                return; // Assertion passed
            }
            catch (Exception ex)
            {
                lastException = ex;
                await Task.Delay(checkInterval.Value);
            }
        }

        if (lastException != null)
        {
            throw new Xunit.Sdk.XunitException($"{failureMessage}. Last error: {lastException.Message}", lastException);
        }

        throw new TimeoutException(failureMessage);
    }
}