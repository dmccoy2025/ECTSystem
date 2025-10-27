using System.Data.Common;
using AF.ECT.Data.Interfaces;
using AF.ECT.Data.Models;
using AF.ECT.Server;
using AF.ECT.Shared.Extensions;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AF.ECT.Tests.Infrastructure;

/// <summary>
/// Enhanced integration test base that uses SQLite in-memory database
/// for realistic database integration testing.
/// </summary>
public class DatabaseIntegrationTestBase : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _databaseName = Guid.NewGuid().ToString();
    private DbConnection _dbConnection = null!;

    public async Task InitializeAsync()
    {
        // Ensure database is created and seeded
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ALODContext>();
        await dbContext.Database.EnsureCreatedAsync();
        _dbConnection = dbContext.Database.GetDbConnection();
        await SeedTestDataAsync(dbContext);
    }

    public async new Task DisposeAsync()
    {
        // Clean up database
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ALODContext>();
        await dbContext.Database.EnsureDeletedAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set the content root to the test project directory
        var testProjectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        builder.UseContentRoot(testProjectPath);

        builder.ConfigureServices(services =>
        {
            // Remove ALL Entity Framework related services
            var descriptorsToRemove = services.Where(d =>
                d.ServiceType.Namespace?.StartsWith("Microsoft.EntityFrameworkCore") == true ||
                d.ServiceType == typeof(DbContextOptions<ALODContext>) ||
                d.ServiceType == typeof(IDbContextFactory<ALODContext>) ||
                d.ServiceType == typeof(ALODContext) ||
                (d.ServiceType.IsGenericType &&
                 d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>) &&
                 d.ServiceType.GetGenericArguments()[0] == typeof(ALODContext)) ||
                d.ServiceType == typeof(IEnumerable<IDbContextOptionsConfiguration<ALODContext>>)
            ).ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            // Register SQLite in-memory database with scoped lifetime
            services.AddDbContextFactory<ALODContext>(options =>
            {
                options.UseSqlite($"Data Source={_databaseName};Mode=Memory;Cache=Shared");
            }, ServiceLifetime.Scoped);

            services.AddDbContext<ALODContext>(options =>
            {
                options.UseSqlite($"Data Source={_databaseName};Mode=Memory;Cache=Shared");
            }, ServiceLifetime.Scoped);

            // Replace the real DataService with a mock for integration testing
            // This allows testing the gRPC layer without database complications
            var mockDataService = new Mock<IDataService>();
            mockDataService.Setup(x => x.GetReinvestigationRequestsAsync(It.IsAny<int?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);
            mockDataService.Setup(x => x.GetUserNameAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);
            mockDataService.Setup(x => x.GetWorkflowTitleAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            services.AddSingleton(mockDataService.Object);
        });
    }

    /// <summary>
    /// Seeds the database with test data required for integration tests.
    /// </summary>
    protected async virtual Task SeedTestDataAsync(ALODContext context)
    {
        // Base implementation - override in derived classes for specific test data
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Resets the database to a clean state between tests.
    /// Note: For SQLite in-memory, we recreate the database schema.
    /// </summary>
    protected async Task ResetDatabaseAsync()
    {
        // For SQLite in-memory, we need to recreate the database
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ALODContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
        await SeedTestDataAsync(dbContext);
    }

    /// <summary>
    /// Creates a gRPC channel to the test server with real database.
    /// </summary>
    protected GrpcChannel CreateGrpcChannel()
    {
        var client = base.CreateClient();
        return GrpcChannelFactory.CreateForTesting(client.BaseAddress!, client);
    }

    /// <summary>
    /// Gets a service instance from the test container.
    /// </summary>
    protected T GetService<T>() where T : notnull
    {
        using var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
}
