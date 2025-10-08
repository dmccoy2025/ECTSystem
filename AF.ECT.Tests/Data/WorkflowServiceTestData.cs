using System.Collections;

namespace AF.ECT.Tests.Data;

/// <summary>
/// Contains test data classes for WorkflowManagementService parameterized tests.
/// </summary>
public static class WorkflowServiceTestData
{
    /// <summary>
    /// Test data for WorkflowManagementService constructor parameter validation
    /// </summary>
    public class WorkflowServiceConstructorNullParameterData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { true, false };  // null logger, valid dataService
            yield return new object[] { false, true };  // valid logger, null dataService
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for different request count scenarios in WorkflowManagementService
    /// </summary>
    public class WorkflowServiceRequestCountData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0 };
            yield return new object[] { 1 };
            yield return new object[] { 5 };
            yield return new object[] { 100 };
            yield return new object[] { 1000 };
            yield return new object[] { int.MaxValue }; // Edge case: maximum integer value
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for various name format scenarios in WorkflowManagementService
    /// </summary>
    public class WorkflowServiceNameFormatData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "", " - CASE001 (Active)" };
            yield return new object[] { "   ", "    - CASE001 (Active)" };
            yield return new object[] { "Test<User>@Domain", "Test<User>@Domain - CASE001 (Active)" };
            yield return new object[] { "José_Müller_张三_محمد", "José_Müller_张三_محمد - CASE001 (Active)" };
            yield return new object[] { null!, "Unknown - CASE001 (Active)" }; // Edge case: null name
            yield return new object[] { "Name\tWith\tTabs", "Name\tWith\tTabs - CASE001 (Active)" }; // Edge case: tabs
            yield return new object[] { "Name\nWith\nNewlines", "Name\nWith\nNewlines - CASE001 (Active)" }; // Edge case: newlines
            yield return new object[] { "Name\r\nWith\r\nCRLF", "Name\r\nWith\r\nCRLF - CASE001 (Active)" }; // Edge case: CRLF
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for different exception types in WorkflowManagementService
    /// </summary>
    public class WorkflowServiceExceptionTypeData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { typeof(InvalidOperationException), "Invalid operation" };
            yield return new object[] { typeof(ArgumentException), "Invalid argument" };
            yield return new object[] { typeof(TimeoutException), "Operation timed out" };
            yield return new object[] { typeof(UnauthorizedAccessException), "Access denied" };
            yield return new object[] { typeof(IOException), "I/O error occurred" };
            yield return new object[] { typeof(HttpRequestException), "Network request failed" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for WorkflowManagementService request scenarios
    /// </summary>
    public class WorkflowServiceRequestScenariosData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0 };
            yield return new object[] { 1 };
            yield return new object[] { 5 };
            yield return new object[] { 100 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for WorkflowManagementService null/empty response scenarios
    /// </summary>
    public class WorkflowServiceNullResponseData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null!, "Null response from data service" };
            yield return new object[] { new List<object>(), "Empty list response" };
            yield return new object[] { new List<object> { null! }, "List with null item" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}