using AF.ECT.Data.Interfaces;
using AF.ECT.Data.Models;
using AF.ECT.Data.Services;
using AF.ECT.Server.Interceptors;
using AF.ECT.Server.Services;
using AF.ECT.Server.Services.Interfaces;
using AspNetCoreRateLimit;
using Audit.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Radzen;

namespace AF.ECT.Server.Extensions;

/// <summary>
/// Extension methods for configuring dependency injection services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds web components and related services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="builder">The web application builder to configure Kestrel options.</param>
    /// <returns>The service collection with web components configured.</returns>
    public static IServiceCollection AddWebComponents(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ConfigureEndpointDefaults(listenOptions =>
            {
                listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
            });
        });

        services
            .AddRadzenComponents()
            .AddRazorComponents()
            .AddInteractiveServerComponents();

        return services;
    }

    /// <summary>
    /// Adds data access services including database context and repositories.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration containing database connection strings.</param>
    /// <returns>The service collection with data access services configured.</returns>
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure and validate database options
        services.AddValidatedOptions<DatabaseOptions>(configuration);
        
        // Get database options for connection setup
        var databaseOptions = new DatabaseOptions();
        configuration.GetSection("DatabaseOptions").Bind(databaseOptions);

        var connectionString = configuration.GetConnectionString("ALODConnection");

        services.AddDbContextFactory<ALODContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: databaseOptions.MaxRetryCount,
                    maxRetryDelay: TimeSpan.FromSeconds(databaseOptions.MaxRetryDelaySeconds),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(databaseOptions.CommandTimeoutSeconds);
            });
        
            // Add Audit.NET interceptor for EF Core
            options.AddInterceptors(new Audit.EntityFramework.AuditSaveChangesInterceptor());
        
            // Add detailed logging in development
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                options.LogTo(Console.WriteLine, LogLevel.Information);
            }
        }, ServiceLifetime.Scoped);

        services.AddScoped<IDataService, DataService>();

        // Configure Audit.NET for Entity Framework Core
        Audit.Core.Configuration.Setup()
            .UseSqlServer(config => config
                .ConnectionString(connectionString)
                .TableName("AuditLogs")
                .IdColumnName("AuditId")
            );

        return services;
    }

    /// <summary>
    /// Adds theme-related services for Radzen components.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection with theme services configured.</returns>
    public static IServiceCollection AddThemeServices(this IServiceCollection services)
    {
        services.AddRadzenCookieThemeService(options =>
        {
            options.Name = "ECT-Theme";
            options.Duration = TimeSpan.FromDays(365);
        });

        return services;
    }

    /// <summary>
    /// Adds CORS (Cross-Origin Resource Sharing) configuration to the application.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration containing CORS settings.</param>
    /// <returns>The service collection with CORS configured.</returns>
    public static IServiceCollection AddApplicationCors(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure and validate CORS options
        services.AddValidatedOptions<CorsOptions>(configuration);
        
        // Get CORS options for policy setup
        var corsOptions = new CorsOptions();
        configuration.GetSection("CorsOptions").Bind(corsOptions);
    
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(corsOptions.AllowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }

    /// <summary>
    /// Adds gRPC services with JSON transcoding, reflection, and global exception handling.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection with gRPC services configured.</returns>
    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services
            .AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionInterceptor>();
                options.Interceptors.Add<AuditInterceptor>();
                options.EnableDetailedErrors = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            })
            .AddJsonTranscoding();

        services.AddGrpcReflection();

        return services;
    }

    /// <summary>
    /// Adds health check services for monitoring application health.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration containing database connection strings.</param>
    /// <returns>The service collection with health checks configured.</returns>
    public static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
    {
        var healthChecksBuilder = services.AddHealthChecks();
    
        // Add EF Core DbContext health check for SQL Server
        healthChecksBuilder.AddDbContextCheck<ALODContext>();
    
        healthChecksBuilder.AddCheck("Self", () => HealthCheckResult.Healthy());

        return services;
    }

    /// <summary>
    /// Adds antiforgery services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection with antiforgery configured.</returns>
    public static IServiceCollection AddAntiforgeryServices(this IServiceCollection services)
    {
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-CSRF-TOKEN";
            options.Cookie.Name = "CSRF-TOKEN";
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });

        return services;
    }

    /// <summary>
    /// Adds resilience services for fault tolerance and recovery.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection with resilience services configured.</returns>
    public static IServiceCollection AddResilienceServices(this IServiceCollection services)
    {
        services.AddSingleton<IResilienceService, ResilienceService>();

        return services;
    }

    /// <summary>
    /// Adds caching services to improve performance and reduce load.   
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration containing caching settings.</param>
    /// <returns>The service collection with caching services configured.</returns>
    public static IServiceCollection AddCachingServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add memory cache for rate limiting stores
        services.AddMemoryCache();

        return services;
    }

    /// <summary>
    /// Adds rate limiting services to protect against abuse and ensure fair resource usage.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration containing rate limiting settings.</param>
    /// <returns>The service collection with rate limiting configured.</returns>
    public static IServiceCollection AddRateLimitingServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Load rate limiting configuration from appsettings.json
        services.AddValidatedOptions<IpRateLimitOptions>(configuration);

        // Register rate limiting stores
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

        // Register rate limiting services
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        return services;
    }

    /// <summary>
    /// Adds OpenAPI and Swagger documentation services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection with documentation services configured.</returns>
    public static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddSwaggerGen();

        return services;
    }

    /// <summary>
    /// Adds logging services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration containing logging settings.</param>
    /// <returns>The service collection with logging services configured.</returns>
    public static IServiceCollection AddLoggingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(logging =>
        {
            logging.AddConfiguration(configuration.GetSection("Logging"));
            logging.AddConsole();
            logging.AddDebug();
        });

        return services;
    }

    /// <summary>
    /// Adds OpenTelemetry telemetry services for tracing and metrics.  
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection with telemetry services configured.</returns>
    /// <remarks>
    /// Uses centralized OpenTelemetry configuration from AF.ECT.ServiceDefaults
    /// with ASP.NET Core, Entity Framework Core, and runtime instrumentation.
    /// </remarks>
    public static IServiceCollection AddTelemetry(this IServiceCollection services)
    {
        return services.AddServerTelemetry();
    }
}