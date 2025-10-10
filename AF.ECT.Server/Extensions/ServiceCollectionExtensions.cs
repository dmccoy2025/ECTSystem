using Microsoft.EntityFrameworkCore;
using AF.ECT.Data.Models;
using AF.ECT.Data.Interfaces;
using AF.ECT.Server.Services;
using AF.ECT.Server.Services.Interfaces;
using Radzen;
using AspNetCoreRateLimit;
using AF.ECT.Server.Interceptors;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AF.ECT.Server.Extensions;

/// <summary>
/// Extension methods for configuring dependency injection services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all application services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration for connection strings and settings.</param>
    /// <returns>The service collection with all application services configured.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services or configuration is null.</exception>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddWebComponents()
            .AddDataAccess(configuration)
            .AddThemeServices()
            .AddHttpClient()
            .AddResilienceServices();
    }

    /// <summary>
    /// Adds web components and related services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection with web components configured.</returns>
    private static IServiceCollection AddWebComponents(this IServiceCollection services)
    {
        services
            .AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddRadzenComponents();

        return services;
    }

    /// <summary>
    /// Adds data access services including database context and repositories.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration containing database connection strings.</param>
    /// <returns>The service collection with data access services configured.</returns>
    /// <exception cref="ArgumentNullException">Thrown when configuration is null.</exception>
    private static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ALODConnection");
        
        services.AddDbContextFactory<ALODContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(30);
            });
            
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

        return services;
    }

    /// <summary>
    /// Adds theme-related services for Radzen components.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection with theme services configured.</returns>
    private static IServiceCollection AddThemeServices(this IServiceCollection services)
    {
        services.AddRadzenCookieThemeService(options =>
        {
            options.Name = "RadzenBlazorApp1Theme";
            options.Duration = TimeSpan.FromDays(365);
        });

        return services;
    }

    /// <summary>
    /// Adds CORS (Cross-Origin Resource Sharing) configuration to the application.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection with CORS configured.</returns>
    /// <remarks>
    /// This configuration allows any origin, header, and method.
    /// In production, this should be restricted to specific origins.
    /// </remarks>
    public static IServiceCollection AddApplicationCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
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
                options.EnableDetailedErrors = true;
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
    /// <exception cref="ArgumentNullException">Thrown when configuration is null.</exception>
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
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
    private static IServiceCollection AddResilienceServices(this IServiceCollection services)
    {
        services.AddSingleton<IResilienceService, ResilienceService>();

        return services;
    }

    /// <summary>
    /// Adds rate limiting services to protect against abuse and ensure fair resource usage.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration containing rate limiting settings.</param>
    /// <returns>The service collection with rate limiting configured.</returns>
    /// <exception cref="ArgumentNullException">Thrown when configuration is null.</exception>
    public static IServiceCollection AddRateLimitingServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add memory cache for rate limiting stores
        services.AddMemoryCache();

        // Load rate limiting configuration from appsettings.json
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

        // Register rate limiting stores
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

        // Register rate limiting services
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        return services;
    }
}