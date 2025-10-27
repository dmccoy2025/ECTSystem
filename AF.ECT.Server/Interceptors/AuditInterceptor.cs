using AF.ECT.Server.Utilities;
using Grpc.Core.Interceptors;

namespace AF.ECT.Server.Interceptors;

/// <summary>
/// gRPC interceptor for military-grade audit logging of all service calls.
/// Captures method invocations, user context, timestamps, and operation details
/// for compliance and security auditing requirements.
/// </summary>
public class AuditInterceptor : Interceptor
{
    private readonly ILogger<AuditInterceptor> _logger;

    /// <summary>
    /// Initializes a new instance of the AuditInterceptor.
    /// </summary>
    /// <param name="logger">The logger for audit trail recording.</param>
    public AuditInterceptor(ILogger<AuditInterceptor> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Intercepts unary server calls to log audit information.
    /// </summary>
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var startTime = DateTime.UtcNow;
        var methodName = context.Method;
        var userId = GrpcContextHelper.GetUserId(context);
        var clientIp = GrpcContextHelper.GetClientIpAddress(context);

        try
        {
            var response = await continuation(request, context);
            var duration = DateTime.UtcNow - startTime;

            _logger.LogInformation(
                "gRPC Audit: Method={Method}, UserId={UserId}, ClientIP={ClientIP}, Duration={Duration}ms, Status=Success",
                methodName, userId, clientIp, duration.TotalMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;

            _logger.LogWarning(ex,
                "gRPC Audit: Method={Method}, UserId={UserId}, ClientIP={ClientIP}, Duration={Duration}ms, Status=Error, ErrorType={ErrorType}",
                methodName, userId, clientIp, duration.TotalMilliseconds, ex.GetType().Name);

            throw;
        }
    }

    /// <summary>
    /// Intercepts server streaming calls to log audit information.
    /// </summary>
    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        var startTime = DateTime.UtcNow;
        var methodName = context.Method;
        var userId = GrpcContextHelper.GetUserId(context);
        var clientIp = GrpcContextHelper.GetClientIpAddress(context);

        try
        {
            await continuation(request, responseStream, context);
            var duration = DateTime.UtcNow - startTime;

            _logger.LogInformation(
                "gRPC Audit: Method={Method}, UserId={UserId}, ClientIP={ClientIP}, Duration={Duration}ms, Status=Success, Type=Streaming",
                methodName, userId, clientIp, duration.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;

            _logger.LogWarning(ex,
                "gRPC Audit: Method={Method}, UserId={UserId}, ClientIP={ClientIP}, Duration={Duration}ms, Status=Error, Type=Streaming, ErrorType={ErrorType}",
                methodName, userId, clientIp, duration.TotalMilliseconds, ex.GetType().Name);

            throw;
        }
    }

}