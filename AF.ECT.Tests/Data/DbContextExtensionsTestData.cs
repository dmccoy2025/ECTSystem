using System.Collections;

namespace AF.ECT.Tests.Data;

/// <summary>
/// Contains test data classes for DbContextExtensions parameterized tests.
/// </summary>
public static class DbContextExtensionsTestData
{
    /// <summary>
    /// Test data for DbContextExtensions SQL queries
    /// </summary>
    public class DbContextExtensionsSqlQueryData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "SELECT 1 AS Result" };
            yield return new object[] { "SELECT 'test' AS Result" };
            yield return new object[] { "SELECT COUNT(*) AS Result FROM sqlite_master" };
            yield return new object[] { "SELECT * FROM sqlite_master LIMIT 10" };
            yield return new object[] { "" };
            yield return new object[] { "   " };
            yield return new object[] { new string('A', 10000) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for DbContextExtensions parameter scenarios
    /// </summary>
    public class DbContextExtensionsParameterData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null! };
            yield return new object[] { new object[] { } };
            yield return new object[] { new object[] { 1 } };
            yield return new object[] { new object[] { "test", 123, DateTime.Now } };
            yield return new object[] { new object[] { DBNull.Value } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}