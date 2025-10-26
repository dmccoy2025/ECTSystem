namespace AF.ECT.AppHost;

/// <summary>
/// Main entry point for the .NET Aspire orchestration host.
/// </summary>
/// <remarks>
/// This AppHost orchestrates the ECTSystem distributed application using .NET Aspire,
/// providing service discovery, health monitoring, and observability across all services.
/// 
/// Architecture:
/// - Client: Blazor WebAssembly UI (AF.ECT.WebClient)
/// - Server: ASP.NET Core gRPC API (AF.ECT.Server)
/// - Wiki: Blazor Server documentation (AF.ECT.Wiki)
/// 
/// .NET Aspire Features:
/// - Automatic service discovery between projects
/// - Integrated OpenTelemetry for logging, metrics, and tracing
/// - Dashboard for monitoring at http://localhost:15888
/// - Automatic health checks and restart policies
/// - Environment variable management
/// 
/// To run the application:
/// 1. dotnet run --project AF.ECT.AppHost
/// 2. Navigate to the Aspire dashboard URL shown in console
/// 3. Access individual services through the dashboard links
/// </remarks>
internal class Program
{
    /// <summary>
    /// Main entry point for the application host.
    /// </summary>
    /// <param name="args">Command-line arguments passed to the application.</param>
    /// <remarks>
    /// Configures and starts the distributed application with the following services:
    /// - Blazor WebAssembly client for user interface
    /// - ASP.NET Core gRPC server for business logic and data access
    /// - Blazor Server wiki for documentation and help content
    /// 
    /// Service Communication:
    /// - Client communicates with Server via gRPC-Web over HTTP
    /// - All services are registered with Aspire's service discovery
    /// - Automatic DNS resolution for service-to-service calls
    /// 
    /// Monitoring:
    /// - Aspire dashboard available at http://localhost:15888 (default)
    /// - OpenTelemetry metrics, logs, and traces collected automatically
    /// - Health check endpoints exposed at /health for each service
    /// </remarks>
    private static void Main(string[] args)
    {
        // Create the distributed application builder
        // This initializes .NET Aspire with default configuration including:
        // - Service discovery
        // - OpenTelemetry integration
        // - Dashboard hosting
        // - Resource management
        var builder = DistributedApplication.CreateBuilder(args);

        // Add the Blazor WebAssembly client project
        // This serves the UI and communicates with the server via gRPC-Web
        // The client is automatically configured to discover the server via Aspire service discovery
        var client = builder.AddProject<Projects.AF_ECT_WebClient>("client");

        // Add the ASP.NET Core gRPC server project
        // This hosts the gRPC services, business logic, and data access layer
        // Exposed via HTTP/2 for gRPC and HTTP/1.1 for gRPC-Web (Blazor compatibility)
        var server = builder.AddProject<Projects.AF_ECT_Server>("server");

        // Add the Blazor Server wiki project
        // This provides documentation, help content, and architectural guidance
        // Runs as a separate service to allow independent scaling and updates
        var wiki = builder.AddProject<Projects.AF_ECT_Wiki>("wiki");

        // Build and run the distributed application
        // This starts all services, the Aspire dashboard, and monitoring infrastructure
        // Services are started in dependency order with automatic health monitoring
        builder.Build().Run();
    }
}