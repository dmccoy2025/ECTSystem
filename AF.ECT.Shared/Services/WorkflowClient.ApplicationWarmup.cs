using Audit.Core;
using AF.ECT.Shared.Options;

namespace AF.ECT.Shared.Services;

/// <summary>
/// Provides gRPC client services for communicating with the WorkflowManagementService.
/// </summary>
/// <remarks>
/// This client handles all gRPC communication with the server-side WorkflowManagementService,
/// including basic greeting operations and comprehensive data access methods.
/// It uses gRPC-Web for browser compatibility and provides both synchronous
/// and asynchronous method calls.
/// 
/// Performance Optimizations:
/// - Connection pooling with HttpClientHandler
/// - Retry policy for transient failures
/// - Performance logging for monitoring
/// - Configurable timeouts
/// - Channel reuse for efficiency
/// </remarks>
public partial class WorkflowClient
{
    #region Application Warmup Process Methods

/// <summary>
    /// Deletes a log by ID.
    /// </summary>
    /// <param name="logId">The log ID to delete.</param>
    /// <returns>A task representing the asynchronous operation, containing the log deletion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<DeleteLogByIdResponse> DeleteLogByIdAsync(int logId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.DeleteLogByIdAsync(new DeleteLogByIdRequest { LogId = logId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(DeleteLogByIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { LogId = logId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(DeleteLogByIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { LogId = logId });
            throw;
        }
    }

    /// <summary>
    /// Finds the last execution date of a process.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>A task representing the asynchronous operation, containing the process last execution date response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<FindProcessLastExecutionDateResponse> FindProcessLastExecutionDateAsync(string processName)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.FindProcessLastExecutionDateAsync(new FindProcessLastExecutionDateRequest { ProcessName = processName }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(FindProcessLastExecutionDateAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { ProcessName = processName });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(FindProcessLastExecutionDateAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { ProcessName = processName });
            throw;
        }
    }

    /// <summary>
    /// Finds the last execution date of a process as a streaming response.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>An asynchronous enumerable of process last execution date items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<ProcessLastExecutionDateItem> FindProcessLastExecutionDateStream(string processName)
    {
        using var call = _client.FindProcessLastExecutionDateStream(new FindProcessLastExecutionDateRequest { ProcessName = processName });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves all logs.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the all logs response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetAllLogsResponse> GetAllLogsAsync()
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetAllLogsAsync(new EmptyRequest()).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllLogsAsync), correlationId, startTime, _stopwatch.Elapsed, true);

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllLogsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all logs as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of log items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<LogItem> GetAllLogsStream()
    {
        using var call = _client.GetAllLogsStream(new EmptyRequest());
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves all logs with pagination, filtering, and sorting.
    /// </summary>
    /// <param name="pageNumber">The page number (optional, defaults to 1).</param>
    /// <param name="pageSize">The page size (optional, defaults to 10).</param>
    /// <param name="processName">The process name filter (optional).</param>
    /// <param name="startDate">The start date filter (optional).</param>
    /// <param name="endDate">The end date filter (optional).</param>
    /// <param name="messageFilter">The message filter (optional).</param>
    /// <param name="sortBy">The sort by field (optional, defaults to "ExecutionDate").</param>
    /// <param name="sortOrder">The sort order (optional, defaults to "DESC").</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated logs response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetAllLogsPaginationResponse> GetAllLogsPaginationAsync(int? pageNumber = null, int? pageSize = null, string? processName = null, string? startDate = null, string? endDate = null, string? messageFilter = null, string? sortBy = null, string? sortOrder = null)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                var request = new GetAllLogsPaginationRequest();
                
                if (pageNumber.HasValue)
                    request.PageNumber = pageNumber.Value;
                
                if (pageSize.HasValue)
                    request.PageSize = pageSize.Value;
                
                if (!string.IsNullOrEmpty(processName))
                    request.ProcessName = processName;
                
                if (!string.IsNullOrEmpty(startDate))
                    request.StartDate = startDate;
                
                if (!string.IsNullOrEmpty(endDate))
                    request.EndDate = endDate;
                
                if (!string.IsNullOrEmpty(messageFilter))
                    request.MessageFilter = messageFilter;
                
                if (!string.IsNullOrEmpty(sortBy))
                    request.SortBy = sortBy;
                
                if (!string.IsNullOrEmpty(sortOrder))
                    request.SortOrder = sortOrder;

                return await _client.GetAllLogsPaginationAsync(request);
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllLogsPaginationAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { PageNumber = pageNumber, PageSize = pageSize, ProcessName = processName, StartDate = startDate, EndDate = endDate, MessageFilter = messageFilter, SortBy = sortBy, SortOrder = sortOrder });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllLogsPaginationAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { PageNumber = pageNumber, PageSize = pageSize, ProcessName = processName, StartDate = startDate, EndDate = endDate, MessageFilter = messageFilter, SortBy = sortBy, SortOrder = sortOrder });
            throw;
        }
    }

    /// <summary>
    /// Inserts a log entry.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <param name="executionDate">The execution date.</param>
    /// <param name="message">The log message.</param>
    /// <returns>A task representing the asynchronous operation, containing the log insertion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<InsertLogResponse> InsertLogAsync(string processName, string executionDate, string message)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.InsertLogAsync(new InsertLogRequest
                {
                    ProcessName = processName,
                    ExecutionDate = executionDate,
                    Message = message
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertLogAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { ProcessName = processName, ExecutionDate = executionDate, Message = message });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertLogAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { ProcessName = processName, ExecutionDate = executionDate, Message = message });
            throw;
        }
    }

    /// <summary>
    /// Checks if a process is active.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>A task representing the asynchronous operation, containing the process active response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<IsProcessActiveResponse> IsProcessActiveAsync(string processName)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.IsProcessActiveAsync(new IsProcessActiveRequest { ProcessName = processName }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(IsProcessActiveAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { ProcessName = processName });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(IsProcessActiveAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { ProcessName = processName });
            throw;
        }
    }

    /// <summary>
    /// Checks if a process is active as a streaming response.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>An asynchronous enumerable of process active items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<ProcessActiveItem> IsProcessActiveStream(string processName)
    {
        using var call = _client.IsProcessActiveStream(new IsProcessActiveRequest { ProcessName = processName });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    #endregion
}
