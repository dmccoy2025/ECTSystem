namespace AF.ECT.Tests.Infrastructure;

/// <summary>
/// Test class representing the result structure for SQL queries.
/// Used in tests to verify query result mapping.
/// </summary>
public class TestResult
{
    /// <summary>
    /// Gets or sets the ID field from the query result.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Name field from the query result.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}