using System.Collections;

namespace AF.ECT.Tests.Data;

/// <summary>
/// Contains test data classes for GreeterClient parameterized tests.
/// </summary>
public static class WorkflowClientTestData
{
    /// <summary>
    /// Test data for GreeterClient invalid name scenarios
    /// </summary>
    public class WorkflowClientInvalidNameData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null! };
            yield return new object[] { "" };
            yield return new object[] { "   " };
            yield return new object[] { "\t" };
            yield return new object[] { "\n" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for GreeterClient valid name scenarios
    /// </summary>
    public class WorkflowClientValidNameData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "John" };
            yield return new object[] { "José_Müller_张三" };
            yield return new object[] { "Name With Spaces" };
            yield return new object[] { "Name-With-Hyphens" };
            yield return new object[] { "Name123" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for GreeterClient user ID scenarios
    /// </summary>
    public class WorkflowClientUserIdData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1 };
            yield return new object[] { 123 };
            yield return new object[] { int.MaxValue };
            yield return new object[] { -1 }; // Edge case: negative ID
            yield return new object[] { 0 }; // Edge case: zero ID
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for GreeterClient string parameter scenarios
    /// </summary>
    public class WorkflowClientStringParameterData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "TestValue" };
            yield return new object[] { "" };
            yield return new object[] { "   " };
            yield return new object[] { new string('A', 1000) }; // Very long string
            yield return new object[] { "Special@Chars#$%" };
            yield return new object[] { "Unicode: José_张三_محمد" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for GreeterClient nullable parameter combinations
    /// </summary>
    public class WorkflowClientNullableParameterData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null!, null!, null!, null!, null!, null!, null! };
            yield return new object[] { 1, "test", "name", 1, 1, 1, true };
            yield return new object[] { 0, "", "", 0, 0, 0, false };
            yield return new object[] { int.MaxValue, new string('A', 100), new string('B', 100), int.MaxValue, int.MaxValue, int.MaxValue, true };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for WorkflowClient large integer values
    /// </summary>
    public class WorkflowClientLargeIntegerData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { int.MaxValue };
            yield return new object[] { int.MinValue };
            yield return new object[] { 0 };
            yield return new object[] { -1 };
            yield return new object[] { 1000000 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for WorkflowClient workflow parameter combinations
    /// </summary>
    public class WorkflowClientWorkflowParameterData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 2, 3 };
            yield return new object[] { 0, 0, 0 };
            yield return new object[] { int.MaxValue, int.MaxValue, int.MaxValue };
            yield return new object[] { 100, 200, 300 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for WorkflowClient request parameter combinations
    /// </summary>
    public class WorkflowClientRequestParameterData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 2, 3, "test" };
            yield return new object[] { 0, 0, 0, "" };
            yield return new object[] { int.MaxValue, int.MaxValue, int.MaxValue, new string('A', 100) };
            yield return new object[] { 100, 200, 300, "service" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}