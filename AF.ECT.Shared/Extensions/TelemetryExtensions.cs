using OpenTelemetry.Trace;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for configuring OpenTelemetry telemetry for client applications.
/// </summary>
public static class TelemetryExtensions
{
    /// <summary>
    /// Configures OpenTelemetry for browser-based client applications (Blazor WebAssembly).
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The service collection for method chaining.</returns>
    /// <remarks>
    /// This method configures OpenTelemetry optimized for client-side applications with:
    /// 
    /// <para><b>Distributed Tracing:</b></para>
    /// <list type="bullet">
    /// <item><description>gRPC client instrumentation for tracking remote procedure calls</description></item>
    /// <item><description>Automatic context propagation for distributed traces</description></item>
    /// <item><description>OTLP exporter for sending traces to observability backends</description></item>
    /// </list>
    /// 
    /// This lightweight configuration is suitable for browser-based applications where
    /// full server-side instrumentation (ASP.NET Core, EF Core, runtime metrics) is not available.
    /// </remarks>
    /// <example>
    /// <code>
    /// // In Blazor WebAssembly client
    /// builder.Services.AddClientTelemetry();
    /// </code>
    /// </example>
    public static IServiceCollection AddClientTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .AddGrpcClientInstrumentation()
                .AddOtlpExporter());

        return services;
    }
}
