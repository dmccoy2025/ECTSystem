using System.Collections;

namespace AF.ECT.Tests.Data;

/// <summary>
/// Contains test data classes for DataService parameterized tests.
/// </summary>
public static class DataServiceTestData
{
    /// <summary>
    /// Test data for DataService parameter combinations
    /// </summary>
    public class DataServiceParameterCombinationData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, true };
            yield return new object[] { 5, false };
            yield return new object[] { null!, true };
            yield return new object[] { 10, null! };
            yield return new object[] { null!, null! };
            yield return new object[] { 0, true }; // Edge case: zero user ID
            yield return new object[] { -1, false }; // Edge case: negative user ID
            yield return new object[] { int.MaxValue, true }; // Edge case: maximum integer
            yield return new object[] { int.MinValue, false }; // Edge case: minimum integer
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for DataService exception types
    /// </summary>
    public class DataServiceExceptionTypeData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { typeof(InvalidOperationException), "Invalid operation" };
            yield return new object[] { typeof(TimeoutException), "Operation timed out" };
            yield return new object[] { typeof(ArgumentException), "Invalid argument" };
            yield return new object[] { typeof(UnauthorizedAccessException), "Access denied" };
            yield return new object[] { typeof(OperationCanceledException), "Operation was canceled" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}