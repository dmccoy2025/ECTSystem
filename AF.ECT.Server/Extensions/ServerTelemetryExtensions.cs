using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for configuring OpenTelemetry telemetry for server applications.
/// </summary>
public static class ServerTelemetryExtensions
{
    /// <summary>
    /// Configures OpenTelemetry for server applications with comprehensive instrumentation.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The service collection for method chaining.</returns>
    /// <remarks>
    /// This method configures OpenTelemetry optimized for server-side applications with:
    /// 
    /// <para><b>Distributed Tracing:</b></para>
    /// <list type="bullet">
    /// <item><description>ASP.NET Core instrumentation for HTTP request tracing</description></item>
    /// <item><description>Entity Framework Core instrumentation for database query tracing</description></item>
    /// <item><description>OTLP exporter for sending traces to observability backends</description></item>
    /// </list>
    /// 
    /// <para><b>Metrics:</b></para>
    /// <list type="bullet">
    /// <item><description>ASP.NET Core metrics (request counts, durations, status codes)</description></item>
    /// <item><description>.NET runtime metrics (GC, memory, thread pool, exceptions)</description></item>
    /// <item><description>OTLP exporter for sending metrics to observability backends</description></item>
    /// </list>
    /// 
    /// This comprehensive configuration provides production-ready observability for
    /// server-side applications with database access and HTTP request handling.
    /// </remarks>
    /// <example>
    /// <code>
    /// // In ASP.NET Core server
    /// builder.Services.AddServerTelemetry();
    /// </code>
    /// </example>
    public static IServiceCollection AddServerTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter());

        return services;
    }
}
