using Polly.CircuitBreaker;

namespace AF.ECT.Server.Services.Interfaces;

/// <summary>
/// Interface for resilience service providing fault tolerance patterns
/// </summary>
public interface IResilienceService
{
    /// <summary>
    /// Executes an HTTP request with resilience policies applied
    /// </summary>
    /// <param name="action">The HTTP request action to execute</param>
    /// <returns>The HTTP response message</returns>
    Task<HttpResponseMessage> ExecuteResilientHttpRequestAsync(Func<Task<HttpResponseMessage>> action);

    /// <summary>
    /// Executes a generic action with retry policy only
    /// </summary>
    /// <typeparam name="T">The return type of the action</typeparam>
    /// <param name="action">The action to execute with retry</param>
    /// <returns>The result of the action</returns>
    Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action);

    /// <summary>
    /// Executes a database operation with timeout and retry
    /// </summary>
    /// <typeparam name="T">The return type of the operation</typeparam>
    /// <param name="action">The database operation to execute</param>
    /// <returns>The result of the database operation</returns>
    Task<T> ExecuteDatabaseOperationAsync<T>(Func<Task<T>> action);

    /// <summary>
    /// Gets the current circuit breaker state
    /// </summary>
    CircuitState CircuitBreakerState { get; }

    /// <summary>
    /// Gets the last exception that caused the circuit breaker to open
    /// </summary>
    Exception? LastException { get; }

    /// <summary>
    /// Manually resets the circuit breaker
    /// </summary>
    void ResetCircuitBreaker();
}
