using Grpc.Core;
using Grpc.Core.Interceptors;

namespace AF.ECT.WebClient.Interceptors;

/// <summary>
/// gRPC client-side interceptor for military-grade audit logging and monitoring.
/// Captures client-side method invocations, performance metrics, and error tracking
/// to complement server-side audit trails for complete end-to-end observability.
/// </summary>
public class ClientAuditInterceptor : Interceptor
{
    private readonly ILogger<ClientAuditInterceptor> _logger;

    /// <summary>
    /// Initializes a new instance of the ClientAuditInterceptor.
    /// </summary>
    /// <param name="logger">The logger for client-side audit trail recording.</param>
    public ClientAuditInterceptor(ILogger<ClientAuditInterceptor> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Intercepts unary client calls to log audit information and performance metrics.
    /// </summary>
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var startTime = DateTime.UtcNow;
        var methodName = context.Method.FullName;
        var correlationId = Guid.NewGuid().ToString();

        _logger.LogInformation(
            "gRPC Client Call Start: CorrelationId={CorrelationId}, Method={Method}, Timestamp={Timestamp}",
            correlationId, methodName, startTime.ToString("O"));

        try
        {
            var call = continuation(request, context);

            // Wrap the response to capture completion
            var responseAsync = call.ResponseAsync.ContinueWith(task =>
            {
                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;

                if (task.IsFaulted)
                {
                    var exception = task.Exception?.InnerException;
                    _logger.LogWarning(exception,
                        "gRPC Client Call Failed: CorrelationId={CorrelationId}, Method={Method}, Duration={Duration}ms, ErrorType={ErrorType}",
                        correlationId, methodName, duration.TotalMilliseconds, exception?.GetType().Name ?? "Unknown");
                }
                else
                {
                    _logger.LogInformation(
                        "gRPC Client Call Success: CorrelationId={CorrelationId}, Method={Method}, Duration={Duration}ms",
                        correlationId, methodName, duration.TotalMilliseconds);
                }

                return task.Result;
            });

            return new AsyncUnaryCall<TResponse>(
                responseAsync,
                call.ResponseHeadersAsync,
                call.GetStatus,
                call.GetTrailers,
                call.Dispose);
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            _logger.LogError(ex,
                "gRPC Client Call Exception: CorrelationId={CorrelationId}, Method={Method}, Duration={Duration}ms, ErrorType={ErrorType}",
                correlationId, methodName, duration.TotalMilliseconds, ex.GetType().Name);
            throw;
        }
    }

    /// <summary>
    /// Intercepts client streaming calls to log audit information.
    /// </summary>
    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var startTime = DateTime.UtcNow;
        var methodName = context.Method.FullName;
        var correlationId = Guid.NewGuid().ToString();

        _logger.LogInformation(
            "gRPC Client Streaming Call Start: CorrelationId={CorrelationId}, Method={Method}, Timestamp={Timestamp}",
            correlationId, methodName, startTime.ToString("O"));

        try
        {
            var call = continuation(context);

            // Wrap the response to capture completion
            var responseAsync = call.ResponseAsync.ContinueWith(task =>
            {
                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;

                if (task.IsFaulted)
                {
                    var exception = task.Exception?.InnerException;
                    _logger.LogWarning(exception,
                        "gRPC Client Streaming Call Failed: CorrelationId={CorrelationId}, Method={Method}, Duration={Duration}ms, ErrorType={ErrorType}",
                        correlationId, methodName, duration.TotalMilliseconds, exception?.GetType().Name ?? "Unknown");
                }
                else
                {
                    _logger.LogInformation(
                        "gRPC Client Streaming Call Success: CorrelationId={CorrelationId}, Method={Method}, Duration={Duration}ms",
                        correlationId, methodName, duration.TotalMilliseconds);
                }

                return task.Result;
            });

            return new AsyncClientStreamingCall<TRequest, TResponse>(
                call.RequestStream,
                responseAsync,
                call.ResponseHeadersAsync,
                call.GetStatus,
                call.GetTrailers,
                call.Dispose);
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            _logger.LogError(ex,
                "gRPC Client Streaming Call Exception: CorrelationId={CorrelationId}, Method={Method}, Duration={Duration}ms, ErrorType={ErrorType}",
                correlationId, methodName, duration.TotalMilliseconds, ex.GetType().Name);
            throw;
        }
    }

    /// <summary>
    /// Intercepts server streaming calls to log audit information.
    /// </summary>
    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var startTime = DateTime.UtcNow;
        var methodName = context.Method.FullName;
        var correlationId = Guid.NewGuid().ToString();

        _logger.LogInformation(
            "gRPC Client Server Streaming Call Start: CorrelationId={CorrelationId}, Method={Method}, Timestamp={Timestamp}",
            correlationId, methodName, startTime.ToString("O"));

        try
        {
            var call = continuation(request, context);

            // Wrap the response headers to capture when streaming starts
            var headersAsync = call.ResponseHeadersAsync.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    var exception = task.Exception?.InnerException;
                    var duration = DateTime.UtcNow - startTime;
                    _logger.LogWarning(exception,
                        "gRPC Client Server Streaming Headers Failed: CorrelationId={CorrelationId}, Method={Method}, Duration={Duration}ms, ErrorType={ErrorType}",
                        correlationId, methodName, duration.TotalMilliseconds, exception?.GetType().Name ?? "Unknown");
                }
                else
                {
                    _logger.LogDebug(
                        "gRPC Client Server Streaming Headers Received: CorrelationId={CorrelationId}, Method={Method}",
                        correlationId, methodName);
                }

                return task.Result;
            });

            // Create a wrapper for the response stream to track completion
            var wrappedCall = new AsyncServerStreamingCall<TResponse>(
                call.ResponseStream,
                headersAsync,
                call.GetStatus,
                call.GetTrailers,
                call.Dispose);

            return wrappedCall;
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            _logger.LogError(ex,
                "gRPC Client Server Streaming Call Exception: CorrelationId={CorrelationId}, Method={Method}, Duration={Duration}ms, ErrorType={ErrorType}",
                correlationId, methodName, duration.TotalMilliseconds, ex.GetType().Name);
            throw;
        }
    }

    /// <summary>
    /// Intercepts duplex streaming calls to log audit information.
    /// </summary>
    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var startTime = DateTime.UtcNow;
        var methodName = context.Method.FullName;
        var correlationId = Guid.NewGuid().ToString();

        _logger.LogInformation(
            "gRPC Client Duplex Streaming Call Start: CorrelationId={CorrelationId}, Method={Method}, Timestamp={Timestamp}",
            correlationId, methodName, startTime.ToString("O"));

        try
        {
            var call = continuation(context);

            // Wrap the response headers to capture when streaming starts
            var headersAsync = call.ResponseHeadersAsync.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    var exception = task.Exception?.InnerException;
                    var duration = DateTime.UtcNow - startTime;
                    _logger.LogWarning(exception,
                        "gRPC Client Duplex Streaming Headers Failed: CorrelationId={CorrelationId}, Method={Method}, Duration={Duration}ms, ErrorType={ErrorType}",
                        correlationId, methodName, duration.TotalMilliseconds, exception?.GetType().Name ?? "Unknown");
                }
                else
                {
                    _logger.LogDebug(
                        "gRPC Client Duplex Streaming Headers Received: CorrelationId={CorrelationId}, Method={Method}",
                        correlationId, methodName);
                }

                return task.Result;
            });

            return new AsyncDuplexStreamingCall<TRequest, TResponse>(
                call.RequestStream,
                call.ResponseStream,
                headersAsync,
                call.GetStatus,
                call.GetTrailers,
                call.Dispose);
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            _logger.LogError(ex,
                "gRPC Client Duplex Streaming Call Exception: CorrelationId={CorrelationId}, Method={Method}, Duration={Duration}ms, ErrorType={ErrorType}",
                correlationId, methodName, duration.TotalMilliseconds, ex.GetType().Name);
            throw;
        }
    }
}