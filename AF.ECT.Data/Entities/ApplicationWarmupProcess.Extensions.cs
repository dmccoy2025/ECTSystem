namespace AF.ECT.Data.Entities;

/// <summary>
/// Represents an application warmup process configuration in the Electronic Case Tracking (ECT) system.
/// </summary>
/// <remarks>
/// <para>
/// The ApplicationWarmupProcess entity manages scheduled background processes that keep the application
/// responsive by periodically executing warmup operations. These processes prevent cold starts and ensure
/// consistent performance for military users.
/// </para>
/// 
/// <para><b>Purpose:</b></para>
/// <list type="bullet">
/// <item><description>Maintain application responsiveness during low-traffic periods</description></item>
/// <item><description>Prevent database connection pool timeouts</description></item>
/// <item><description>Keep cached data fresh for military operations</description></item>
/// <item><description>Track execution history via ApplicationWarmupProcessLog relationship</description></item>
/// </list>
/// 
/// <para><b>Common Warmup Processes:</b></para>
/// <list type="bullet">
/// <item><description>Database connection validation</description></item>
/// <item><description>Cache warmup for frequently accessed data</description></item>
/// <item><description>Service health check pings</description></item>
/// <item><description>Background data synchronization</description></item>
/// </list>
/// 
/// <para><b>Database Schema:</b></para>
/// <list type="bullet">
/// <item><description>Table: dbo.ApplicationWarmupProcess</description></item>
/// <item><description>Primary Key: Id (int, IDENTITY)</description></item>
/// <item><description>Related Logs: ApplicationWarmupProcessLogs (1:many)</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// // Create a new warmup process configuration
/// var warmupProcess = new ApplicationWarmupProcess
/// {
///     Name = "DatabaseConnectionWarmup",
///     Active = true
/// };
/// 
/// // Check if the process is active before executing
/// if (warmupProcess.Active)
/// {
///     // Execute warmup logic
/// }
/// </code>
/// </example>
public partial class ApplicationWarmupProcess
{
    /// <summary>
    /// Gets a value indicating whether this warmup process should be executed.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the process is active and should be executed during warmup cycles;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method provides a semantic way to check if the warmup process is enabled.
    /// Use this instead of directly checking the Active property for better code readability.
    /// </remarks>
    public bool IsEnabled() => Active;

    /// <summary>
    /// Gets a display-friendly name for this warmup process.
    /// </summary>
    /// <returns>The name of the warmup process formatted for display.</returns>
    /// <remarks>
    /// Returns the process name as-is. This method exists to provide a consistent
    /// API for retrieving display names across all entity types.
    /// </remarks>
    public string GetDisplayName() => Name;

    /// <summary>
    /// Creates a log entry for this warmup process execution.
    /// </summary>
    /// <param name="executionDate">The date and time when the process was executed.</param>
    /// <param name="message">An optional message describing the execution result or any issues encountered.</param>
    /// <returns>A new ApplicationWarmupProcessLog instance associated with this process.</returns>
    /// <remarks>
    /// This helper method simplifies the creation of execution log entries. The log is not
    /// automatically saved to the database; you must add it to the context and call SaveChanges.
    /// </remarks>
    /// <example>
    /// <code>
    /// var process = await context.ApplicationWarmupProcesses.FindAsync(processId);
    /// var log = process.CreateLogEntry(DateTime.UtcNow, "Warmup completed successfully");
    /// context.ApplicationWarmupProcessLogs.Add(log);
    /// await context.SaveChangesAsync();
    /// </code>
    /// </example>
    public ApplicationWarmupProcessLog CreateLogEntry(DateTime executionDate, string? message = null)
    {
        return new ApplicationWarmupProcessLog
        {
            ProcessId = Id,
            ExecutionDate = executionDate,
            Message = message,
            Process = this
        };
    }
}
