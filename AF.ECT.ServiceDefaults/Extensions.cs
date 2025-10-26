using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// Provides extension methods for configuring .NET Aspire service defaults.
/// </summary>
/// <remarks>
/// This class contains extension methods that add common .NET Aspire services to an application,
/// including service discovery, resilience patterns, health checks, and OpenTelemetry observability.
/// These defaults should be applied to each service in a distributed .NET Aspire application to ensure
/// consistent configuration, monitoring, and inter-service communication patterns.
/// 
/// For more information, see: https://aka.ms/dotnet/aspire/service-defaults
/// </remarks>
public static class Extensions
{
    /// <summary>
    /// The endpoint path for comprehensive health checks.
    /// </summary>
    /// <remarks>
    /// This endpoint returns the aggregated health status of all registered health checks.
    /// </remarks>
    private const string HealthEndpointPath = "/health";
    
    /// <summary>
    /// The endpoint path for basic aliveness checks.
    /// </summary>
    /// <remarks>
    /// This lightweight endpoint confirms the service is responsive without detailed health checks.
    /// </remarks>
    private const string AlivenessEndpointPath = "/alive";

    /// <summary>
    /// Adds .NET Aspire service defaults to the application builder.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the host application builder.</typeparam>
    /// <param name="builder">The host application builder to configure.</param>
    /// <returns>The configured builder instance for method chaining.</returns>
    /// <remarks>
    /// This method configures the following .NET Aspire defaults:
    /// <list type="bullet">
    /// <item><description><b>OpenTelemetry</b>: Enables distributed tracing, metrics collection, and structured logging for observability.</description></item>
    /// <item><description><b>Health Checks</b>: Registers default health check endpoints for Kubernetes readiness and liveness probes.</description></item>
    /// <item><description><b>Service Discovery</b>: Enables automatic service-to-service discovery in the Aspire environment.</description></item>
    /// <item><description><b>Resilience Patterns</b>: Configures retry policies, circuit breakers, and timeouts for all HTTP clients.</description></item>
    /// </list>
    /// 
    /// These defaults ensure consistent behavior across all services in a distributed .NET Aspire application,
    /// providing production-ready observability, reliability, and inter-service communication patterns.
    /// </remarks>
    /// <example>
    /// <code>
    /// var builder = WebApplication.CreateBuilder(args);
    /// builder.AddServiceDefaults();
    /// </code>
    /// </example>
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        // Configure comprehensive observability with OpenTelemetry (logging, metrics, tracing)
        builder.ConfigureOpenTelemetry();

        // Register health check endpoints for container orchestration (Kubernetes, Docker)
        builder.AddDefaultHealthChecks();

        // Enable automatic service discovery for inter-service communication
        builder.Services.AddServiceDiscovery();

        // Configure all HTTP clients with resilience patterns and service discovery
        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default (retry, circuit breaker, timeout policies)
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default (resolve service names to endpoints)
            http.AddServiceDiscovery();
        });

        // Uncomment the following to restrict the allowed schemes for service discovery.
        // This is useful in production to enforce HTTPS-only communication.
        // builder.Services.Configure<ServiceDiscoveryOptions>(options =>
        // {
        //     options.AllowedSchemes = ["https"];
        // });

        return builder;
    }

    /// <summary>
    /// Configures OpenTelemetry for comprehensive application observability.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the host application builder.</typeparam>
    /// <param name="builder">The host application builder to configure.</param>
    /// <returns>The configured builder instance for method chaining.</returns>
    /// <remarks>
    /// This method configures OpenTelemetry with three pillars of observability:
    /// 
    /// <para><b>1. Logging:</b></para>
    /// <list type="bullet">
    /// <item><description>Structured logging with formatted messages and scope information</description></item>
    /// <item><description>Logs are exported to configured backends (OTLP, Azure Monitor)</description></item>
    /// </list>
    /// 
    /// <para><b>2. Metrics:</b></para>
    /// <list type="bullet">
    /// <item><description>ASP.NET Core instrumentation (request counts, durations, error rates)</description></item>
    /// <item><description>HTTP client instrumentation (outbound request metrics)</description></item>
    /// <item><description>Runtime instrumentation (GC, thread pool, exception metrics)</description></item>
    /// </list>
    /// 
    /// <para><b>3. Distributed Tracing:</b></para>
    /// <list type="bullet">
    /// <item><description>ASP.NET Core request traces with automatic context propagation</description></item>
    /// <item><description>HTTP client traces for outbound calls</description></item>
    /// <item><description>Health check endpoints excluded to reduce noise</description></item>
    /// <item><description>Optional gRPC client instrumentation available</description></item>
    /// </list>
    /// 
    /// The data is exported to backends configured via environment variables or appsettings.json.
    /// In .NET Aspire, this typically connects to the Aspire dashboard for local development.
    /// </remarks>
    /// <example>
    /// <code>
    /// var builder = WebApplication.CreateBuilder(args);
    /// builder.ConfigureOpenTelemetry();
    /// </code>
    /// </example>
    public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        // Configure structured logging with OpenTelemetry
        builder.Logging.AddOpenTelemetry(logging =>
        {
            // Include formatted log messages for better readability
            logging.IncludeFormattedMessage = true;
            // Include log scopes for contextual information (e.g., correlation IDs)
            logging.IncludeScopes = true;
        });

        // Configure OpenTelemetry with metrics and tracing
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                // Collect ASP.NET Core metrics (HTTP request rates, durations, status codes)
                metrics.AddAspNetCoreInstrumentation()
                    // Collect HTTP client metrics (outbound request telemetry)
                    .AddHttpClientInstrumentation()
                    // Collect .NET runtime metrics (GC, memory, thread pool, exceptions)
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                // Create traces for this application (identified by application name)
                tracing.AddSource(builder.Environment.ApplicationName)
                    // Trace ASP.NET Core requests automatically
                    .AddAspNetCoreInstrumentation(tracing =>
                        // Exclude health check requests from tracing to reduce noise
                        tracing.Filter = context =>
                            !context.Request.Path.StartsWithSegments(HealthEndpointPath)
                            && !context.Request.Path.StartsWithSegments(AlivenessEndpointPath)
                    )
                    // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                    //.AddGrpcClientInstrumentation()
                    // Trace outbound HTTP client calls for distributed tracing
                    .AddHttpClientInstrumentation();
            });

        // Configure exporters (OTLP, Azure Monitor) based on environment configuration
        builder.AddOpenTelemetryExporters();

        return builder;
    }

    /// <summary>
    /// Configures OpenTelemetry exporters based on environment configuration.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the host application builder.</typeparam>
    /// <param name="builder">The host application builder to configure.</param>
    /// <returns>The configured builder instance for method chaining.</returns>
    /// <remarks>
    /// This method enables OpenTelemetry data exporters based on configuration:
    /// 
    /// <para><b>OTLP Exporter (OpenTelemetry Protocol):</b></para>
    /// <list type="bullet">
    /// <item><description>Enabled when <c>OTEL_EXPORTER_OTLP_ENDPOINT</c> environment variable is set</description></item>
    /// <item><description>Used by .NET Aspire dashboard in local development</description></item>
    /// <item><description>Can export to any OTLP-compatible backend (Jaeger, Prometheus, Grafana)</description></item>
    /// </list>
    /// 
    /// <para><b>Azure Monitor Exporter (Optional):</b></para>
    /// <list type="bullet">
    /// <item><description>Enabled when <c>APPLICATIONINSIGHTS_CONNECTION_STRING</c> is configured</description></item>
    /// <item><description>Requires <c>Azure.Monitor.OpenTelemetry.AspNetCore</c> NuGet package</description></item>
    /// <item><description>Sends telemetry data to Azure Application Insights for production monitoring</description></item>
    /// </list>
    /// 
    /// If no exporters are configured, telemetry data is collected but not exported.
    /// </remarks>
    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        // Check if OTLP exporter endpoint is configured (typically set by .NET Aspire)
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            // Enable OTLP exporter to send telemetry to Aspire dashboard or other OTLP-compatible backends
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
        // This sends telemetry data to Azure Application Insights for production monitoring and analytics
        //if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        //{
        //    builder.Services.AddOpenTelemetry()
        //       .UseAzureMonitor();
        //}

        return builder;
    }

    /// <summary>
    /// Adds default health check endpoints for container orchestration and monitoring.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the host application builder.</typeparam>
    /// <param name="builder">The host application builder to configure.</param>
    /// <returns>The configured builder instance for method chaining.</returns>
    /// <remarks>
    /// This method registers default health check endpoints that are essential for container orchestration
    /// platforms like Kubernetes and Docker. The health checks help determine when a service is ready to
    /// accept traffic and when it should be restarted.
    /// 
    /// <para><b>Registered Health Checks:</b></para>
    /// <list type="bullet">
    /// <item><description><b>"self" (liveness)</b>: A basic check that always returns healthy, confirming the process is running and responsive.</description></item>
    /// </list>
    /// 
    /// <para><b>Kubernetes Integration:</b></para>
    /// <list type="bullet">
    /// <item><description><b>Liveness Probe</b>: Checks if the application process is alive (uses "live" tag)</description></item>
    /// <item><description><b>Readiness Probe</b>: Checks if the application is ready to serve traffic (all health checks)</description></item>
    /// </list>
    /// 
    /// Additional health checks can be added in service-specific code, such as:
    /// - Database connectivity checks
    /// - External service dependency checks
    /// - Custom application-specific health indicators
    /// </remarks>
    /// <example>
    /// <code>
    /// // In your service-specific Program.cs, you can add additional health checks:
    /// builder.Services.AddHealthChecks()
    ///     .AddSqlServer(connectionString, name: "database")
    ///     .AddUrlGroup(new Uri("https://api.example.com/health"), name: "external-api");
    /// </code>
    /// </example>
    public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            // This check always returns healthy and is used by Kubernetes liveness probes
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }
}
