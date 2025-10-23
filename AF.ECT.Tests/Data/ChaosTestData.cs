using System.Collections;

namespace AF.ECT.Tests.Data;

/// <summary>
/// Contains test data classes for ChaosTests parameterized tests.
/// </summary>
public static class ChaosTestData
{
    /// <summary>
    /// Test data for network failure scenarios with different parameters
    /// </summary>
    public class NetworkFailureScenariosData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 10, 0.2, 0.3, 0.5 }; // 10 iterations, 20% failure, 30% delay, 50% success
            yield return new object[] { 20, 0.4, 0.3, 0.3 }; // 20 iterations, 40% failure, 30% delay, 30% success
            yield return new object[] { 50, 0.1, 0.2, 0.7 }; // 50 iterations, 10% failure, 20% delay, 70% success
            yield return new object[] { 5, 0.8, 0.1, 0.1 };  // 5 iterations, 80% failure, 10% delay, 10% success
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for database failure scenarios
    /// </summary>
    public class DatabaseFailureScenariosData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 6, 50, 200 }; // 6 failures, delay 50-200ms
            yield return new object[] { 3, 100, 300 }; // 3 failures, delay 100-300ms
            yield return new object[] { 10, 25, 150 }; // 10 failures, delay 25-150ms
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for mixed failure scenarios
    /// </summary>
    public class MixedFailureScenariosData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 10, 0.5, 0.3, 0.2 }; // 10 iterations, 50% success, 30% timeout, 20% server error
            yield return new object[] { 15, 0.4, 0.4, 0.2 }; // 15 iterations, 40% success, 40% timeout, 20% server error
            yield return new object[] { 8, 0.6, 0.2, 0.2 };  // 8 iterations, 60% success, 20% timeout, 20% server error
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for high load scenarios
    /// </summary>
    public class HighLoadScenariosData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 5, 5, 0.2 }; // 5 tasks, 5 operations each, 20% failure rate
            yield return new object[] { 10, 3, 0.3 }; // 10 tasks, 3 operations each, 30% failure rate
            yield return new object[] { 3, 10, 0.1 }; // 3 tasks, 10 operations each, 10% failure rate
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for circuit breaker recovery scenarios
    /// </summary>
    public class CircuitBreakerRecoveryScenariosData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0.6 }; // 60% failure rate initially
            yield return new object[] { 0.8 }; // 80% failure rate initially
            yield return new object[] { 0.4 }; // 40% failure rate initially
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Test data for performance under failure scenarios
    /// </summary>
    public class PerformanceUnderFailureScenariosData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 5 };  // 5 executions
            yield return new object[] { 10 }; // 10 executions
            yield return new object[] { 15 }; // 15 executions
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}