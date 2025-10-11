using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

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
        var userId = GetUserId(context);
        var clientIp = GetClientIpAddress(context);

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
        var userId = GetUserId(context);
        var clientIp = GetClientIpAddress(context);

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

    /// <summary>
    /// Extracts the user ID from the request context.
    /// </summary>
    private static string GetUserId(ServerCallContext context)
    {
        try
        {
            // Try to get user ID from claims
            var user = context.GetHttpContext()?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ??
                                 user.FindFirst("sub") ??
                                 user.FindFirst("user_id");
                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }

                // Fallback to name claim
                var nameClaim = user.FindFirst(ClaimTypes.Name);
                if (nameClaim != null)
                {
                    return nameClaim.Value;
                }
            }

            // Try to get from custom headers
            var userIdHeader = context.RequestHeaders.Get("x-user-id")?.Value;
            if (!string.IsNullOrEmpty(userIdHeader))
            {
                return userIdHeader;
            }

            return "anonymous";
        }
        catch
        {
            return "unknown";
        }
    }

    /// <summary>
    /// Extracts the client IP address from the request context.
    /// </summary>
    private static string GetClientIpAddress(ServerCallContext context)
    {
        try
        {
            var httpContext = context.GetHttpContext();
            if (httpContext != null)
            {
                // Check for forwarded headers (common in proxy/load balancer scenarios)
                var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    return forwardedFor.Split(',').First().Trim();
                }

                var realIp = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
                if (!string.IsNullOrEmpty(realIp))
                {
                    return realIp;
                }

                // Fallback to connection remote IP
                return httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            }

            return "unknown";
        }
        catch
        {
            return "unknown";
        }
    }
}