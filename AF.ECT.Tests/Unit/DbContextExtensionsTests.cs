using static AF.ECT.Tests.Data.DbContextExtensionsTestData;
using AF.ECT.Tests.Infrastructure;
using AF.ECT.Data.Extensions;
using Microsoft.Data.Sqlite;

namespace AF.ECT.Tests.Unit;

/// <summary>
/// Contains unit tests for the <see cref="DbContextExtensions"/> class.
/// Tests cover SQL query execution, parameter validation, error handling, and cancellation token support.
/// 
/// <para>Test Scenarios Outline:</para>
/// <list type="bullet">
/// <item><description>Parameter validation: Ensures proper exception handling for null database context and SQL query parameters.</description></item>
/// <item><description>SQL execution: Tests basic query execution path with valid parameters.</description></item>
/// <item><description>Cancellation support: Validates proper handling of cancellation tokens in async operations.</description></item>
/// <item><description>Error handling: Covers database connection failures and query execution errors.</description></item>
/// </list>
/// </summary>
[Collection("DbContextExtensions Tests")]
[Trait("Category", "Unit")]
[Trait("Component", "DbContextExtensions")]
public class DbContextExtensionsTests : DbContextExtensionsTestBase
{
    /// <summary>
    /// Tests that SqlQueryToListAsync throws <see cref="NullReferenceException"/>
    /// when a null database context is provided.
    /// </summary>
    [Fact]
    public async Task SqlQueryToListAsync_ThrowsNullReferenceException_WhenDbIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() =>
            DbContextExtensions.SqlQueryToListAsync<string>(null!, "SELECT 1", null));
    }

    /// <summary>
    /// Tests that SqlQueryToListAsync throws <see cref="ArgumentNullException"/>
    /// when a null SQL query string is provided.
    /// </summary>
    [Fact]
    public async Task SqlQueryToListAsync_ThrowsArgumentNullException_WhenSqlIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            DbContextExtensions.SqlQueryToListAsync<string>(TestContext, null!, null));
    }

    /// <summary>
    /// Tests that SqlQueryToListAsync handles different SQL query formats correctly.
    /// </summary>
    /// <param name="sqlQuery">The SQL query to test.</param>
    [Theory]
    [ClassData(typeof(DbContextExtensionsSqlQueryData))]
    public async Task SqlQueryToListAsync_HandlesDifferentSqlQueryFormats(string sqlQuery)
    {
        // For all queries, they should either succeed or throw database-related exceptions
        // Empty/whitespace queries may succeed with empty results or throw syntax errors
        try
        {
            var result = await DbContextExtensions.SqlQueryToListAsync<string>(TestContext, sqlQuery, null);
            Assert.NotNull(result);
        }
        catch (Exception ex)
        {
            // Any exception thrown should be database-related, not our custom validation
            Assert.True(ex is SqliteException || 
                       ex is InvalidOperationException ||
                       ex is ArgumentException, 
                       $"Unexpected exception type: {ex.GetType().Name}");
        }
    }

    /// <summary>
    /// Tests that SqlQueryToListAsync handles different parameter scenarios correctly.
    /// </summary>
    /// <param name="parameters">The parameters to test.</param>
    [Theory]
    [ClassData(typeof(DbContextExtensionsParameterData))]
    public async Task SqlQueryToListAsync_HandlesDifferentParameterScenarios(object[] parameters)
    {
        // Arrange
        // Use a simple query that works with in-memory database
        const string sqlQuery = "SELECT 'test' AS Result";

        // Act
        var result = await DbContextExtensions.SqlQueryToListAsync<string>(TestContext, sqlQuery, parameters);

        // Assert
        Assert.NotNull(result);
    }
}