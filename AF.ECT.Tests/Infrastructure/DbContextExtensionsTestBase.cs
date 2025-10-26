
namespace AF.ECT.Tests.Infrastructure;

/// <summary>
/// Base class for DbContextExtensions tests providing common test database setup.
/// </summary>
public abstract class DbContextExtensionsTestBase : IDisposable
{
    protected readonly DbContext TestContext;

    protected DbContextExtensionsTestBase()
    {
        // Create a SQLite in-memory database for testing (supports SQL queries)
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        TestContext = new DbContext(options);
        TestContext.Database.OpenConnection();
        TestContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        TestContext.Database.CloseConnection();
        TestContext.Dispose();
    }
}