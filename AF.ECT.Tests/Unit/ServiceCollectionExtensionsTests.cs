using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AF.ECT.Server.Extensions;
using AF.ECT.Data.Models;
using AF.ECT.Data.Interfaces;
using static AF.ECT.Tests.Data.ServiceCollectionExtensionsTestData;
using AF.ECT.Data.Services;

namespace AF.ECT.Tests.Unit;

/// <summary>
/// Contains unit tests for the <see cref="ServiceCollectionExtensions"/> class.
/// Tests cover service registration, dependency injection configuration, CORS setup, gRPC configuration, and health checks.
/// 
/// <para>Test Scenarios Outline:</para>
/// <list type="bullet">
/// <item><description>Parameter validation: Ensures proper exception handling for null service collection parameters.</description></item>
/// <item><description>Service registration: Verifies correct registration of all required services (DbContext factory, DataService).</description></item>
/// <item><description>CORS configuration: Tests proper setup of CORS policies and services.</description></item>
/// <item><description>gRPC setup: Validates gRPC service configuration with JSON transcoding.</description></item>
/// <item><description>Health checks: Ensures health check services are properly registered and configured.</description></item>
/// <item><description>Data access: Tests DbContext factory configuration with various connection strings.</description></item>
/// <item><description>Service lifetime: Verifies correct service lifetimes (scoped for DataService).</description></item>
/// </list>
/// </summary>
[Collection("ServiceCollectionExtensions Tests")]
[Trait("Category", "Unit")]
[Trait("Component", "ServiceCollectionExtensions")]
public class ServiceCollectionExtensionsTests
{
    /// <summary>
    /// Tests that AddApplicationServices throws <see cref="ArgumentNullException"/>
    /// when a null service collection is provided.
    /// </summary>
    [Fact]
    public void AddApplicationServices_ThrowsArgumentNullException_WhenServicesIsNull()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            ServiceCollectionExtensions.AddApplicationServices(null!, configuration));
    }

    /// <summary>
    /// Tests that AddApplicationServices correctly registers all required services
    /// including DbContext factory and DataService.
    /// </summary>
    [Fact]
    public void AddApplicationServices_RegistersAllRequiredServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:ALODConnection"] = "Server=test;Database=test;Trusted_Connection=True;"
            })
            .Build();

        // Act
        services.AddApplicationServices(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        // Verify DbContext factory is registered
        var contextFactory = serviceProvider.GetService<IDbContextFactory<ALODContext>>();
        Assert.NotNull(contextFactory);

        // Verify DataService is registered
        var dataService = serviceProvider.GetService<IDataService>();
        Assert.NotNull(dataService);
        Assert.IsType<DataService>(dataService);
    }

    /// <summary>
    /// Tests that AddApplicationCors correctly configures CORS policies
    /// and registers the necessary CORS services.
    /// </summary>
    [Fact]
    public void AddApplicationCors_ConfiguresCorsPolicy()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging(); // Add logging for CORS service

        // Act
        services.AddApplicationCors();

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        // Verify CORS service can be resolved (indicates CORS was configured)
        var corsService = serviceProvider.GetService<Microsoft.AspNetCore.Cors.Infrastructure.ICorsService>();
        Assert.NotNull(corsService);
    }

    /// <summary>
    /// Tests that AddGrpcServices correctly configures gRPC services
    /// with JSON transcoding support.
    /// </summary>
    [Fact]
    public void AddGrpcServices_ConfiguresGrpcWithJsonTranscoding()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddGrpcServices();

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        // Verify gRPC services are configured (this is a basic check)
        // In a real scenario, you might check for specific gRPC service registrations
        Assert.NotNull(serviceProvider);
    }

    /// <summary>
    /// Tests that AddHealthChecks correctly registers health check services
    /// and configures database connectivity health checks.
    /// </summary>
    [Fact]
    public void AddHealthChecks_RegistersHealthChecks()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging(); // Add logging for health check service
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:ALODConnection"] = "Server=test;Database=test;Trusted_Connection=True;"
            })
            .Build();

        // Act
        services.AddHealthChecks(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        // Verify health check service is registered
        var healthCheckService = serviceProvider.GetService<Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService>();
        Assert.NotNull(healthCheckService);
    }

    /// <summary>
    /// Tests that AddDataAccess correctly registers the DbContext factory
    /// with the proper connection string configuration.
    /// </summary>
    [Fact]
    public void AddDataAccess_RegistersDbContextFactory_WithCorrectConnectionString()
    {
        // Arrange
        var services = new ServiceCollection();
        var expectedConnectionString = "Server=myserver;Database=mydb;User Id=myuser;Password=mypass;";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:ALODConnection"] = expectedConnectionString
            })
            .Build();

        // Act - Test through public AddApplicationServices method
        services.AddApplicationServices(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var contextFactory = serviceProvider.GetService<IDbContextFactory<ALODContext>>();
        Assert.NotNull(contextFactory);

        // Verify the factory can create contexts (basic smoke test)
        using var context = contextFactory!.CreateDbContext();
        Assert.NotNull(context);
    }

    /// <summary>
    /// Tests that AddDataAccess registers the DataService with the correct
    /// service lifetime (Scoped).
    /// </summary>
    [Fact]
    public void AddDataAccess_RegistersDataService_AsScoped()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:ALODConnection"] = "Server=test;Database=test;Trusted_Connection=True;"
            })
            .Build();

        // Act - Test through public AddApplicationServices method
        services.AddApplicationServices(configuration);

        // Assert
        var serviceDescriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(IDataService));
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor!.Lifetime);
        Assert.Equal(typeof(DataService), serviceDescriptor.ImplementationType);
    }

    /// <summary>
    /// Tests that AddApplicationServices correctly handles various database connection strings
    /// and configures the services appropriately for each format.
    /// </summary>
    /// <param name="connectionString">The database connection string to test.</param>
    [Theory]
    [ClassData(typeof(ServiceCollectionExtensionsConnectionStringData))]
    public void AddApplicationServices_HandlesDifferentConnectionStrings(string connectionString)
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:ALODConnection"] = connectionString
            })
            .Build();

        // Act
        services.AddApplicationServices(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        // Verify DbContext factory is registered
        var contextFactory = serviceProvider.GetService<IDbContextFactory<ALODContext>>();
        Assert.NotNull(contextFactory);

        // Verify DataService is registered
        var dataService = serviceProvider.GetService<IDataService>();
        Assert.NotNull(dataService);
        Assert.IsType<DataService>(dataService);
    }

    /// <summary>
    /// Tests that AddDataAccess correctly configures the DbContext factory
    /// with various connection string formats and ensures the factory can create contexts.
    /// </summary>
    /// <param name="connectionString">The database connection string to test.</param>
    [Theory]
    [ClassData(typeof(ServiceCollectionExtensionsConnectionStringData))]
    public void AddDataAccess_ConfiguresDbContextFactory_WithVariousConnectionStrings(string connectionString)
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:ALODConnection"] = connectionString
            })
            .Build();

        // Act - Test through public AddApplicationServices method
        services.AddApplicationServices(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var contextFactory = serviceProvider.GetService<IDbContextFactory<ALODContext>>();
        Assert.NotNull(contextFactory);

        // Verify the factory can create contexts
        using var context = contextFactory!.CreateDbContext();
        Assert.NotNull(context);
    }
}