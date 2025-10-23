using System.Collections;

namespace AF.ECT.Tests.Data;

/// <summary>
/// Contains test data classes for ResilienceServiceTests parameterized tests.
/// </summary>
public static class ResilienceServiceTestData
{
    /// <summary>
    /// Test data for retry scenarios with different failure patterns
    /// </summary>
    public class RetryScenariosData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 2, 42 }; // Fail 2 times, succeed on 3rd, return 42
            yield return new object[] { 1, 100 }; // Fail 1 time, succeed on 2nd, return 100
            yield return new object[] { 3, 0 };   // Fail 3 times, succeed on 4th, return 0
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for circuit breaker state transitions
    /// </summary>
    public class CircuitBreakerStateTransitionsData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 5 }; // 5 failures to trigger open
            yield return new object[] { 6 }; // 6 failures to trigger open
            yield return new object[] { 4 }; // 4 failures (should not trigger if threshold is 5)
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for exponential backoff scenarios
    /// </summary>
    public class ExponentialBackoffScenariosData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 3, 42 }; // Fail 3 times, succeed on 4th, return 42
            yield return new object[] { 2, 99 }; // Fail 2 times, succeed on 3rd, return 99
            yield return new object[] { 4, 1 };  // Fail 4 times, succeed on 5th, return 1
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for timeout scenarios
    /// </summary>
    public class TimeoutScenariosData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 5000 }; // 5 second timeout
            yield return new object[] { 10000 }; // 10 second timeout
            yield return new object[] { 15000 }; // 15 second timeout
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for database operation scenarios
    /// </summary>
    public class DatabaseOperationScenariosData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 42 }; // Fail 1 time, succeed on 2nd, return 42
            yield return new object[] { 2, 100 }; // Fail 2 times, succeed on 3rd, return 100
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}