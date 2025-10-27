using Audit.Core;
using AF.ECT.Shared.Options;

namespace AF.ECT.Shared.Services;

/// <summary>
/// Provides gRPC client services for communicating with the WorkflowManagementService.
/// </summary>
/// <remarks>
/// This client handles all gRPC communication with the server-side WorkflowManagementService,
/// including basic greeting operations and comprehensive data access methods.
/// It uses gRPC-Web for browser compatibility and provides both synchronous
/// and asynchronous method calls.
/// 
/// Performance Optimizations:
/// - Connection pooling with HttpClientHandler
/// - Retry policy for transient failures
/// - Performance logging for monitoring
/// - Configurable timeouts
/// - Channel reuse for efficiency
/// </remarks>
public partial class WorkflowClient
{
    #region Audit Logging Methods

/// <summary>
    /// Logs audit information for gRPC calls with correlation ID and performance metrics.
    /// </summary>
    /// <param name="methodName">The name of the method being called.</param>
    /// <param name="correlationId">The correlation ID for tracing.</param>
    /// <param name="startTime">The start time of the operation.</param>
    /// <param name="duration">The duration of the operation.</param>
    /// <param name="success">Whether the operation was successful.</param>
    /// <param name="errorMessage">The error message if the operation failed.</param>
    /// <param name="additionalData">Additional audit data.</param>
    private void LogAuditEvent(string methodName, string correlationId, DateTime startTime, TimeSpan duration, bool success, string? errorMessage = null, object? additionalData = null)
    {
        using (var scope = AuditScope.Create($"gRPC:{methodName}", () => additionalData))
        {
            scope.Event.StartDate = startTime;
            scope.Event.EndDate = startTime + duration;
            scope.Event.Duration = (int)duration.TotalMilliseconds;
            scope.SetCustomField("CorrelationId", correlationId);
            scope.SetCustomField("Success", success);
            if (!success && errorMessage != null)
            {
                scope.SetCustomField("ErrorMessage", errorMessage);
            }
            scope.SetCustomField("ClientInfo", new
            {
                UserAgent = "ECTSystem-BlazorClient",
                Version = "1.0.0",
                Environment = "Production"
            });
        }

        // Keep existing logger calls for immediate logging
        if (success)
        {
            _logger?.LogInformation("gRPC Audit: {MethodName} completed successfully. CorrelationId: {CorrelationId}, Duration: {DurationMs}ms",
                methodName, correlationId, duration.TotalMilliseconds);
        }
        else
        {
            _logger?.LogError("gRPC Audit: {MethodName} failed. CorrelationId: {CorrelationId}, Duration: {DurationMs}ms, Error: {ErrorMessage}",
                methodName, correlationId, duration.TotalMilliseconds, errorMessage);
        }
    }

    /// <summary>
    /// Generates a unique correlation ID for request tracing.
    /// </summary>
    /// <returns>A unique correlation ID.</returns>
    private static string GenerateCorrelationId() => Guid.NewGuid().ToString();

    #endregion
}
