using AF.ECT.Data.Models;
using AF.ECT.Data.Extensions;
using AF.ECT.Data.ResultTypes;
using AF.ECT.Data.Entities;

#nullable enable

namespace AF.ECT.Data.Services;

/// <summary>
/// Partial class containing Application Warmup Process Methods.
/// </summary>
public partial class DataService
{
    #region Application Warmup Process Methods


    /// <summary>
    /// Asynchronously deletes a log entry by its identifier.
    /// </summary>
    /// <param name="logId">The log entry identifier to delete.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> DeleteLogByIdAsync(int? logId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting log entry with id {LogId}", logId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.ApplicationWarmupProcess_sp_DeleteLogByIdAsync(logId, cancellationToken: cancellationToken);
            _logger.LogInformation("Log entry {LogId} deleted successfully", logId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting log entry with id {LogId}", logId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously finds the last execution date for a process.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of process last execution date results.</returns>
    public async Task<List<ApplicationWarmupProcess_sp_FindProcessLastExecutionDateResult>> FindProcessLastExecutionDateAsync(string? processName, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Finding last execution date for process {ProcessName}", processName);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await ((ALODContextProcedures)context.Procedures).ApplicationWarmupProcess_sp_FindProcessLastExecutionDateAsync(processName, cancellationToken: cancellationToken);
            _logger.LogInformation("Found {Count} last execution date results for process {ProcessName}", result.Count, processName);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding last execution date for process {ProcessName}", processName);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves all log entries.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of all log entries.</returns>
    public async Task<List<ApplicationWarmupProcess_sp_GetAllLogsResult>> GetAllLogsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all log entries");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.ApplicationWarmupProcess_sp_GetAllLogsAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} log entries", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all log entries");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves all log entries with pagination, filtering, and sorting.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="processName">Optional filter by process name.</param>
    /// <param name="startDate">Optional filter for execution date from this date.</param>
    /// <param name="endDate">Optional filter for execution date up to this date.</param>
    /// <param name="messageFilter">Optional filter by message content.</param>
    /// <param name="sortBy">Column to sort by ('Id', 'Name', 'ExecutionDate', 'Message').</param>
    /// <param name="sortOrder">Sort order ('ASC' or 'DESC').</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing both the total count and paginated log entries.</returns>
    public async Task<ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result> GetAllLogsPaginationAsync(int? pageNumber = 1, int? pageSize = 10, string? processName = null, DateTime? startDate = null, DateTime? endDate = null, string? messageFilter = null, string? sortBy = "ExecutionDate", string? sortOrder = "DESC", CancellationToken cancellationToken = default)
    {
        // Validate input parameters
        if ((pageNumber ?? 1) <= 0)
        {
            throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
        }

        if ((pageSize ?? 10) <= 0 || (pageSize ?? 10) > 1000)
        {
            throw new ArgumentException("Page size must be between 1 and 1000", nameof(pageSize));
        }

        var validSortColumns = new[] { "Id", "Name", "ExecutionDate", "Message" };
        if (!validSortColumns.Contains(sortBy ?? "ExecutionDate"))
        {
            throw new ArgumentException($"Invalid sortBy parameter. Valid values are: {string.Join(", ", validSortColumns)}", nameof(sortBy));
        }

        var validSortOrders = new[] { "ASC", "DESC" };
        if (!validSortOrders.Contains(sortOrder ?? "DESC"))
        {
            throw new ArgumentException("Invalid sortOrder parameter. Valid values are: ASC, DESC", nameof(sortOrder));
        }

        if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
        {
            throw new ArgumentException("Start date cannot be after end date", nameof(startDate));
        }

        // Check for cancellation before proceeding
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Retrieving all log entries with pagination, filtering, and sorting, page {PageNumber}, size {PageSize}", pageNumber, pageSize);

        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.ApplicationWarmupProcess_sp_GetAllLogs_paginationAsync(
                pageNumber,
                pageSize,
                processName,
                startDate,
                endDate,
                messageFilter,
                sortBy,
                sortOrder,
                cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} log entries (total: {TotalCount}) for page {PageNumber}", result.Data.Count, result.TotalCount, pageNumber);
            return result;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Operation was cancelled while retrieving log entries with pagination for page {PageNumber}", pageNumber);
            throw;
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            _logger.LogError(ex, "Database error occurred while retrieving log entries with pagination for page {PageNumber}: {Message}", pageNumber, ex.Message);
            throw new InvalidOperationException("A database error occurred while retrieving log entries. Please try again later.", ex);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            _logger.LogError(ex, "Entity Framework update error occurred while retrieving log entries with pagination for page {PageNumber}: {Message}", pageNumber, ex.Message);
            throw new InvalidOperationException("An error occurred while accessing the database. Please try again later.", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Timeout occurred while retrieving log entries with pagination for page {PageNumber}: {Message}", pageNumber, ex.Message);
            throw new InvalidOperationException("The operation timed out. Please try again later.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while retrieving log entries with pagination for page {PageNumber}: {Message}", pageNumber, ex.Message);
            throw new InvalidOperationException("An unexpected error occurred while retrieving log entries. Please contact support if the problem persists.", ex);
        }
    }

    /// <summary>
    /// Asynchronously retrieves all log entries with pagination, filtering, and sorting using LINQ.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="processName">Optional filter by process name.</param>
    /// <param name="startDate">Optional filter for execution date from this date.</param>
    /// <param name="endDate">Optional filter for execution date up to this date.</param>
    /// <param name="messageFilter">Optional filter by message content.</param>
    /// <param name="sortBy">Column to sort by ('Id', 'Name', 'ExecutionDate', 'Message').</param>
    /// <param name="sortOrder">Sort order ('ASC' or 'DESC').</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing both the total count and paginated log entries.</returns>
    public async Task<ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result> GetAllLogsPaginationAsync1(int? pageNumber = 1, int? pageSize = 10, string? processName = null, DateTime? startDate = null, DateTime? endDate = null, string? messageFilter = null, string? sortBy = "ExecutionDate", string? sortOrder = "DESC", CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all log entries with pagination, filtering, and sorting using LINQ, page {PageNumber}, size {PageSize}", pageNumber, pageSize);
        try
        {
            // Validate sort parameters
            var validSortColumns = new[] { "Id", "Name", "ExecutionDate", "Message" };
            if (!validSortColumns.Contains(sortBy ?? "ExecutionDate"))
            {
                throw new ArgumentException($"Invalid sortBy parameter. Valid values are: {string.Join(", ", validSortColumns)}", nameof(sortBy));
            }

            var validSortOrders = new[] { "ASC", "DESC" };
            if (!validSortOrders.Contains(sortOrder ?? "DESC"))
            {
                throw new ArgumentException("Invalid sortOrder parameter. Valid values are: ASC, DESC", nameof(sortOrder));
            }

            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            // Build base query with filters
            var query = from l in context.Set<ApplicationWarmupProcessLog>()
                        join p in context.Set<ApplicationWarmupProcess>() on l.ProcessId equals p.Id
                        where (processName == null || p.Name.Contains(processName)) &&
                              (startDate == null || l.ExecutionDate >= startDate) &&
                              (endDate == null || l.ExecutionDate <= endDate) &&
                              (messageFilter == null || (l.Message != null && l.Message.Contains(messageFilter)))
                        select new { Log = l, Process = p };

            // Get total count
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply dynamic sorting
            var sortedQuery = sortBy switch
            {
                "Id" => sortOrder == "ASC" 
                    ? query.OrderBy(x => x.Log.Id) 
                    : query.OrderByDescending(x => x.Log.Id),
                "Name" => sortOrder == "ASC" 
                    ? query.OrderBy(x => x.Process.Name) 
                    : query.OrderByDescending(x => x.Process.Name),
                "ExecutionDate" => sortOrder == "ASC" 
                    ? query.OrderBy(x => x.Log.ExecutionDate) 
                    : query.OrderByDescending(x => x.Log.ExecutionDate),
                "Message" => sortOrder == "ASC" 
                    ? query.OrderBy(x => x.Log.Message) 
                    : query.OrderByDescending(x => x.Log.Message),
                _ => query.OrderByDescending(x => x.Log.ExecutionDate)
            };

            // Apply pagination and project to result type
            var data = await sortedQuery
                .Skip(((pageNumber ?? 1) - 1) * (pageSize ?? 10))
                .Take(pageSize ?? 10)
                .Select(x => new ApplicationWarmupProcess_sp_GetAllLogsResult
                {
                    Id = x.Log.Id,
                    Name = x.Process.Name,
                    ExecutionDate = x.Log.ExecutionDate,
                    Message = x.Log.Message
                })
                .ToListAsync(cancellationToken);

            var result = new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result
            {
                TotalCount = totalCount,
                Data = data
            };

            _logger.LogInformation("Retrieved {Count} log entries (total: {TotalCount}) for page {PageNumber} using LINQ", result.Data.Count, result.TotalCount, pageNumber);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all log entries with pagination, filtering, and sorting using LINQ, page {PageNumber}", pageNumber);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously inserts a new log entry.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <param name="executionDate">The execution date.</param>
    /// <param name="message">The log message.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> InsertLogAsync(string? processName, DateTime? executionDate, string? message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting log entry for process {ProcessName}", processName);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.ApplicationWarmupProcess_sp_InsertLogAsync(processName, executionDate, message, cancellationToken: cancellationToken);
            _logger.LogInformation("Log entry inserted successfully for process {ProcessName}", processName);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting log entry for process {ProcessName}", processName);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously checks if a process is active.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of process active status results.</returns>
    public async Task<List<ApplicationWarmupProcess_sp_IsProcessActiveResult>> IsProcessActiveAsync(string? processName, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking if process {ProcessName} is active", processName);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.ApplicationWarmupProcess_sp_IsProcessActiveAsync(processName, cancellationToken: cancellationToken);
            _logger.LogInformation("Checked process {ProcessName} active status, found {Count} results", processName, result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if process {ProcessName} is active", processName);
            throw;
        }
    }


    #endregion
}
