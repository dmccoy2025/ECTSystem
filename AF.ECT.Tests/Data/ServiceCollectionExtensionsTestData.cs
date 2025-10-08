using System.Collections;

namespace AF.ECT.Tests.Data;

/// <summary>
/// Contains test data classes for ServiceCollectionExtensions parameterized tests.
/// </summary>
public static class ServiceCollectionExtensionsTestData
{
    /// <summary>
    /// Test data for ServiceCollectionExtensions connection string scenarios
    /// </summary>
    public class ServiceCollectionExtensionsConnectionStringData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Server=localhost;Database=TestDB;Trusted_Connection=True;" };
            yield return new object[] { "Server=localhost,1433;Database=TestDB;User Id=sa;Password=TestPassword123!;" };
            yield return new object[] { "Server=(localdb)\\MSSQLLocalDB;Database=TestDB;Trusted_Connection=True;" };
            yield return new object[] { "Server=192.168.1.100;Database=TestDB;User Id=testuser;Password=testpass;" };
            yield return new object[] { "Server=localhost;Database=TestDB;Trusted_Connection=True;MultipleActiveResultSets=true;" };
            yield return new object[] { "Server=localhost;Database=TestDB;User Id=sa;Password=Complex!@#$%^&*()Password;" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}