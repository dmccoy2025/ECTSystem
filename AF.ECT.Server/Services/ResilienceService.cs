using AF.ECT.Server.Services.Interfaces;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

#nullable enable

namespace AF.ECT.Server.Services;

/// <summary>
/// Service for implementing resilience patterns using Polly policies
/// </summary>
public class ResilienceService : IResilienceService
{
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
    private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;
    private readonly AsyncTimeoutPolicy<HttpResponseMessage> _timeoutPolicy;
    private readonly AsyncPolicy<HttpResponseMessage> _combinedPolicy;

    public ResilienceService()
    {
        // Retry policy: Retry up to 3 times with exponential backoff
        _retryPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .OrResult(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (outcome, timespan, retryAttempt, context) =>
                {
                    // Log retry attempt
                    Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds}s due to: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
                });

        // Circuit breaker policy: Break after 5 failures within 30 seconds
        _circuitBreakerPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .OrResult(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30),
                onBreak: (outcome, timespan) =>
                {
                    Console.WriteLine($"Circuit breaker opened for {timespan.TotalSeconds}s due to: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
                },
                onReset: () =>
                {
                    Console.WriteLine("Circuit breaker reset");
                },
                onHalfOpen: () =>
                {
                    Console.WriteLine("Circuit breaker half-open, testing next call");
                });

        // Timeout policy: Timeout after 3 seconds
        _timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(3), TimeoutStrategy.Pessimistic);

        // Combined policy: Timeout -> Retry -> Circuit Breaker
        _combinedPolicy = Policy.WrapAsync(_circuitBreakerPolicy, _retryPolicy, _timeoutPolicy);
    }

    /// <summary>
    /// Executes an HTTP request with resilience policies applied
    /// </summary>
    public async Task<HttpResponseMessage> ExecuteResilientHttpRequestAsync(Func<Task<HttpResponseMessage>> action)
    {
        return await _combinedPolicy.ExecuteAsync(action);
    }

    /// <summary>
    /// Executes a generic action with retry policy only
    /// </summary>
    public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action)
    {
        var retryPolicy = Policy<T>
            .Handle<Exception>()
            .WaitAndRetryAsync(4, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        return await retryPolicy.ExecuteAsync(action);
    }

    /// <summary>
    /// Executes a database operation with timeout and retry
    /// </summary>
    public async Task<T> ExecuteDatabaseOperationAsync<T>(Func<Task<T>> action)
    {
        var dbPolicy = Policy<T>
            .Handle<Exception>() // Could be more specific for database exceptions
            .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromMilliseconds(100 * retryAttempt))
            .WrapAsync(Policy.TimeoutAsync<T>(TimeSpan.FromSeconds(5)));

        return await dbPolicy.ExecuteAsync(action);
    }

    /// <summary>
    /// Gets the current circuit breaker state
    /// </summary>
    public CircuitState CircuitBreakerState => _circuitBreakerPolicy.CircuitState;

    /// <summary>
    /// Gets the last exception that caused the circuit breaker to open
    /// </summary>
    public Exception? LastException => _circuitBreakerPolicy.LastException;

    /// <summary>
    /// Manually resets the circuit breaker
    /// </summary>
    public void ResetCircuitBreaker()
    {
        _circuitBreakerPolicy.Reset();
    }
}