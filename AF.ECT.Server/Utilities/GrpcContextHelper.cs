using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AF.ECT.Server.Utilities;

/// <summary>
/// Provides helper methods for extracting contextual information from gRPC server calls.
/// </summary>
/// <remarks>
/// This utility class centralizes common logic for extracting user identity, client IP addresses,
/// and other contextual information from gRPC ServerCallContext instances. These methods are
/// designed to be reused across interceptors, middleware, and service implementations.
/// </remarks>
public static class GrpcContextHelper
{
    /// <summary>
    /// Extracts the user ID from the gRPC server call context.
    /// </summary>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>The user ID if found; otherwise "anonymous" or "unknown" if an error occurs.</returns>
    /// <remarks>
    /// This method attempts to extract the user ID using multiple strategies in the following order:
    /// <list type="number">
    /// <item><description>Claims from authenticated user (NameIdentifier, sub, user_id)</description></item>
    /// <item><description>Name claim if no ID claim is found</description></item>
    /// <item><description>Custom header 'x-user-id'</description></item>
    /// <item><description>Returns "anonymous" if no user information is found</description></item>
    /// <item><description>Returns "unknown" if an exception occurs during extraction</description></item>
    /// </list>
    /// 
    /// This method is safe to call from any context and will not throw exceptions.
    /// </remarks>
    /// <example>
    /// <code>
    /// var userId = GrpcContextHelper.GetUserId(context);
    /// _logger.LogInformation("Request from user: {UserId}", userId);
    /// </code>
    /// </example>
    public static string GetUserId(ServerCallContext context)
    {
        if (context == null)
        {
            return "unknown";
        }

        try
        {
            // Try to get user ID from claims
            var user = context.GetHttpContext()?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                // Try standard claim types for user ID
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
    /// Extracts the client IP address from the gRPC server call context.
    /// </summary>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>The client IP address if found; otherwise "unknown".</returns>
    /// <remarks>
    /// This method attempts to extract the client IP address using multiple strategies in the following order:
    /// <list type="number">
    /// <item><description>X-Forwarded-For header (for requests through proxies/load balancers)</description></item>
    /// <item><description>X-Real-IP header (alternative proxy header)</description></item>
    /// <item><description>Direct connection remote IP address</description></item>
    /// <item><description>Returns "unknown" if no IP information is found or an exception occurs</description></item>
    /// </list>
    /// 
    /// When X-Forwarded-For contains multiple IPs (comma-separated), the first IP is returned
    /// as it represents the original client IP in a proxy chain.
    /// 
    /// This method is safe to call from any context and will not throw exceptions.
    /// </remarks>
    /// <example>
    /// <code>
    /// var clientIp = GrpcContextHelper.GetClientIpAddress(context);
    /// _logger.LogInformation("Request from IP: {ClientIP}", clientIp);
    /// </code>
    /// </example>
    public static string GetClientIpAddress(ServerCallContext context)
    {
        if (context == null)
        {
            return "unknown";
        }

        try
        {
            var httpContext = context.GetHttpContext();
            if (httpContext != null)
            {
                // Check for forwarded headers (common in proxy/load balancer scenarios)
                var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    // X-Forwarded-For can contain multiple IPs (client, proxy1, proxy2, ...)
                    // The first IP is the original client
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

    /// <summary>
    /// Extracts the user agent string from the gRPC server call context.
    /// </summary>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>The user agent string if found; otherwise "unknown".</returns>
    /// <remarks>
    /// This method extracts the User-Agent header from the HTTP context, which identifies
    /// the client application, browser, or tool making the request. This information is
    /// useful for analytics, debugging, and security monitoring.
    /// 
    /// This method is safe to call from any context and will not throw exceptions.
    /// </remarks>
    /// <example>
    /// <code>
    /// var userAgent = GrpcContextHelper.GetUserAgent(context);
    /// _logger.LogInformation("Request from: {UserAgent}", userAgent);
    /// </code>
    /// </example>
    public static string GetUserAgent(ServerCallContext context)
    {
        if (context == null)
        {
            return "unknown";
        }

        try
        {
            var httpContext = context.GetHttpContext();
            if (httpContext != null)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault();
                if (!string.IsNullOrEmpty(userAgent))
                {
                    return userAgent;
                }
            }

            return "unknown";
        }
        catch
        {
            return "unknown";
        }
    }

    /// <summary>
    /// Extracts the correlation ID from the gRPC server call context.
    /// </summary>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>The correlation ID if found; otherwise an empty string.</returns>
    /// <remarks>
    /// This method looks for correlation IDs in the following headers (in order):
    /// <list type="bullet">
    /// <item><description>x-correlation-id</description></item>
    /// <item><description>x-request-id</description></item>
    /// <item><description>traceparent (W3C trace context)</description></item>
    /// </list>
    /// 
    /// Correlation IDs are essential for distributed tracing and linking related operations
    /// across multiple services in a microservices architecture.
    /// 
    /// This method is safe to call from any context and will not throw exceptions.
    /// </remarks>
    /// <example>
    /// <code>
    /// var correlationId = GrpcContextHelper.GetCorrelationId(context);
    /// _logger.LogInformation("CorrelationId: {CorrelationId}", correlationId);
    /// </code>
    /// </example>
    public static string GetCorrelationId(ServerCallContext context)
    {
        if (context == null)
        {
            return string.Empty;
        }

        try
        {
            var httpContext = context.GetHttpContext();
            if (httpContext != null)
            {
                // Try standard correlation ID headers
                var correlationId = httpContext.Request.Headers["x-correlation-id"].FirstOrDefault() ??
                                   httpContext.Request.Headers["x-request-id"].FirstOrDefault() ??
                                   httpContext.Request.Headers["traceparent"].FirstOrDefault();

                if (!string.IsNullOrEmpty(correlationId))
                {
                    return correlationId;
                }
            }

            return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }
}
