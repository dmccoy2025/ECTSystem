using AF.ECT.Server.Extensions;
using AF.ECT.Server.Services;
using AspNetCoreRateLimit;

namespace AF.ECT.Server;

/// <summary>
/// Main entry point for the ECTSystem gRPC server application.
/// </summary>
/// <remarks>
/// This server provides:
/// - gRPC API for workflow management and case tracking
/// - RESTful JSON endpoints via gRPC JSON Transcoding
/// - Entity Framework Core data access to SQL Server
/// - Audit logging with Audit.NET
/// - Rate limiting and CORS protection
/// - OpenTelemetry observability
/// - Health monitoring endpoints
/// </remarks>
public partial class Program 
{
    /// <summary>
    /// Application entry point that configures and starts the gRPC server.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// Service Configuration Order:
    /// 1. Web Components - Kestrel, Razor, Blazor Server (for admin UI)
    /// 2. Data Access - EF Core, DbContext factory, repositories
    /// 3. Theme Services - Radzen UI components
    /// 4. CORS - Cross-origin resource sharing for web clients
    /// 5. gRPC Services - Core business logic services
    /// 6. Health Checks - Database, external services monitoring
    /// 7. Logging - Structured logging with Serilog
    /// 8. Antiforgery - CSRF protection for forms
    /// 9. Resilience - Polly policies for retries and circuit breakers
    /// 10. Caching - Redis distributed caching
    /// 11. Rate Limiting - IP-based throttling
    /// 12. Documentation - OpenAPI/Swagger for API docs
    /// 13. Telemetry - OpenTelemetry for metrics and tracing
    /// 
    /// Middleware Pipeline Order (Critical):
    /// 1. HTTPS Redirection - Force secure connections
    /// 2. CORS - Must be early for preflight requests
    /// 3. gRPC-Web - Protocol translation for browser clients
    /// 4. Routing - Endpoint matching
    /// 5. Antiforgery - Token validation
    /// 6. Rate Limiting - Throttle before processing
    /// 7. Endpoints - gRPC services and health checks
    /// </remarks>
    private static async Task Main(string[] args)
    {
        // Create the web application builder with default configuration
        // Loads appsettings.json, environment variables, and command-line args
        var builder = WebApplication.CreateBuilder(args);

        // === SERVICE REGISTRATION ===
        
        // Register web components: Kestrel with HTTP/1.1 and HTTP/2 support
        // Configures Razor Components and Blazor Server for interactive UI
        builder.Services.AddWebComponents(builder);
        
        // Register data access layer: EF Core with SQL Server
        // Includes DbContext factory, retry policies, and Audit.NET interceptors
        builder.Services.AddDataAccess(builder.Configuration);
        
        // Register Radzen theme services for UI components
        builder.Services.AddThemeServices();
        
        // Configure CORS to allow requests from trusted origins
        // Origins specified in appsettings.json CorsOptions section
        builder.Services.AddApplicationCors(builder.Configuration);
        
        // Register gRPC services including WorkflowServiceImpl
        // Enables JSON transcoding for REST-like HTTP access
        builder.Services.AddGrpcServices();
        
        // Configure health checks for database and application monitoring
        // Exposes /healthz endpoint for load balancers and orchestrators
        builder.Services.AddHealthCheckServices(builder.Configuration);
        
        // Configure structured logging with appropriate log levels
        builder.Services.AddLoggingServices(builder.Configuration);
        
        // Enable antiforgery tokens for CSRF protection
        builder.Services.AddAntiforgeryServices();
        
        // Register Polly resilience policies for fault tolerance
        // Includes retry, circuit breaker, and timeout policies
        builder.Services.AddResilienceServices();
        
        // Configure distributed caching with Redis (if available)
        // Falls back to in-memory caching in development
        builder.Services.AddCachingServices(builder.Configuration);
        
        // Configure IP-based rate limiting to prevent abuse
        // Limits specified in appsettings.json IpRateLimitOptions section
        builder.Services.AddRateLimitingServices(builder.Configuration);
        
        // Enable OpenAPI/Swagger for API documentation
        // Available at /swagger in development mode
        builder.Services.AddDocumentation();
        
        // Configure OpenTelemetry for metrics, traces, and logs
        // Integrates with .NET Aspire dashboard
        builder.Services.AddTelemetry();

        // Build the application with all configured services
        var app = builder.Build();

        // === MIDDLEWARE PIPELINE ===
        
        // Development-only: Enable OpenAPI and Swagger UI
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();      // Expose OpenAPI spec at /openapi/v1.json
            app.UseSwagger();       // Enable Swagger JSON endpoint
            app.UseSwaggerUI();     // Enable Swagger UI at /swagger
        }

        // Redirect HTTP requests to HTTPS for security
        app.UseHttpsRedirection();
        
        // Enable CORS based on configured policies
        // Must be before UseRouting for preflight requests
        app.UseCors();
        
        // Enable gRPC-Web protocol for browser-based gRPC calls
        // Required for Blazor WebAssembly client communication
        app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
        
        // Enable endpoint routing
        app.UseRouting();
        
        // Validate antiforgery tokens for state-changing operations
        app.UseAntiforgery();
        
        // Apply rate limiting rules to throttle excessive requests
        app.UseIpRateLimiting();
        
        // Map health check endpoint for monitoring
        // Returns HTTP 200 (Healthy) or 503 (Unhealthy)
        app.MapHealthChecks("/healthz");
        
        // Map the primary gRPC service with gRPC-Web enabled
        // Handles all workflow and case management operations
        app.MapGrpcService<WorkflowServiceImpl>().EnableGrpcWeb();

        // Development-only: Enable gRPC reflection for tools like grpcurl
        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }

        // Fallback to serve index.html for client-side routing
        // Required for Blazor WebAssembly SPA functionality
        app.MapFallbackToFile("index.html");

        // Start the application and listen for requests
        // Runs until application shutdown (Ctrl+C or host termination)
        await app.RunAsync();
    }
}

// Make Program class public for testing
public partial class Program { }
