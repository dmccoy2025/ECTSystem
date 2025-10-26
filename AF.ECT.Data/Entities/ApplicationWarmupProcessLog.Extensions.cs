namespace AF.ECT.Data.Entities;

/// <summary>
/// Represents a log entry for an application warmup process execution.
/// </summary>
/// <remarks>
/// <para>
/// The ApplicationWarmupProcessLog entity tracks individual executions of warmup processes,
/// providing a complete audit trail of when processes ran, how long they took, and whether
/// they completed successfully. This is essential for troubleshooting performance issues
/// and ensuring the warmup schedule is functioning correctly.
/// </para>
/// 
/// <para><b>Use Cases:</b></para>
/// <list type="bullet">
/// <item><description>Track warmup process execution history</description></item>
/// <item><description>Identify failing or slow warmup processes</description></item>
/// <item><description>Monitor application health and readiness</description></item>
/// <item><description>Audit background process activity for compliance</description></item>
/// </list>
/// 
/// <para><b>Database Schema:</b></para>
/// <list type="bullet">
/// <item><description>Table: dbo.ApplicationWarmupProcessLog</description></item>
/// <item><description>Primary Key: Id (int, IDENTITY)</description></item>
/// <item><description>Foreign Key: ProcessId → ApplicationWarmupProcess.Id</description></item>
/// </list>
/// 
/// <para><b>Related Entities:</b></para>
/// <list type="bullet">
/// <item><description>Process (ApplicationWarmupProcess): The warmup process configuration that generated this log</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// // Query recent warmup execution logs
/// var recentLogs = await context.ApplicationWarmupProcessLogs
///     .Include(l => l.Process)
///     .Where(l => l.ExecutionDate >= DateTime.UtcNow.AddHours(-24))
///     .OrderByDescending(l => l.ExecutionDate)
///     .ToListAsync();
/// 
/// foreach (var log in recentLogs)
/// {
///     Console.WriteLine($"{log.Process.Name} executed at {log.ExecutionDate}: {log.Message}");
/// }
/// </code>
/// </example>
public partial class ApplicationWarmupProcessLog
{
    /// <summary>
    /// Gets a value indicating whether this log entry represents a successful execution.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the message is null or empty (indicating no errors);
    /// <c>false</c> if there is an error message.
    /// </returns>
    /// <remarks>
    /// By convention, log entries with null or empty messages indicate successful executions.
    /// Entries with messages typically indicate warnings, errors, or notable events during execution.
    /// </remarks>
    public bool IsSuccessful() => string.IsNullOrWhiteSpace(Message);

    /// <summary>
    /// Gets a value indicating whether this log entry represents an error or failure.
    /// </summary>
    /// <returns>
    /// <c>true</c> if there is an error message present;
    /// <c>false</c> if the message is null or empty.
    /// </returns>
    /// <remarks>
    /// This is the inverse of IsSuccessful(). Use this method when checking for failures
    /// in warmup process executions.
    /// </remarks>
    public bool HasError() => !string.IsNullOrWhiteSpace(Message);

    /// <summary>
    /// Gets the age of this log entry in hours.
    /// </summary>
    /// <returns>The number of hours since this log entry was created.</returns>
    /// <remarks>
    /// Useful for determining if a warmup process has run recently or identifying stale logs.
    /// </remarks>
    public double GetAgeInHours() => (DateTime.UtcNow - ExecutionDate).TotalHours;

    /// <summary>
    /// Gets a formatted display string for this log entry.
    /// </summary>
    /// <returns>A string containing the execution date and message (if any).</returns>
    /// <remarks>
    /// Provides a human-readable summary of the log entry suitable for UI display or logging.
    /// </remarks>
    public string GetDisplayString()
    {
        var status = IsSuccessful() ? "Success" : "Error";
        var message = string.IsNullOrWhiteSpace(Message) ? "" : $" - {Message}";
        return $"{ExecutionDate:yyyy-MM-dd HH:mm:ss} [{status}]{message}";
    }
}
