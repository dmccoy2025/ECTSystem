using AF.ECT.Server.Services.Interfaces;
using AF.ECT.Data.Interfaces;
using Google.Protobuf.Collections;

namespace AF.ECT.Server.Services;

/// <summary>
/// Partial class containing Application Warmup Process Methods
/// </summary>
public partial class WorkflowServiceImpl : WorkflowService.WorkflowServiceBase
{
    #region Application Warmup Process Methods


    /// <summary>
    /// Handles the DeleteLogById gRPC request.
    /// </summary>
    /// <param name="request">The request containing log deletion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the log deletion Response.</returns>
    public async override Task<DeleteLogByIdResponse> DeleteLogById(DeleteLogByIdRequest request, ServerCallContext context)
    {
        try
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Request was cancelled"));
            }

            // Validate input
            if (!request.HasLogId || request.LogId <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "LogId must be provided and greater than 0"));
            }

            _logger.LogInformation($"Deleting log by ID: {request.LogId}");

            var result = await _resilienceService.ExecuteWithRetryAsync(() =>
            {
                return _dataService.DeleteLogByIdAsync(request.LogId, context?.CancellationToken ?? CancellationToken.None);
            });

            return new DeleteLogByIdResponse
            {
                Result = result
            };
        }
        catch (RpcException)
        {
            // Re-throw RpcException as-is
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Operation cancelled in DeleteLogById");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in DeleteLogById for LogId {LogId}: {Message}", request.LogId, ex.Message);
            throw new RpcException(new Status(StatusCode.Internal, "An internal error occurred"), new Metadata { { "error-id", Guid.NewGuid().ToString() } });
        }
    }

    /// <summary>
    /// Handles the FindProcessLastExecutionDate gRPC request.
    /// </summary>
    /// <param name="request">The request containing process last execution date parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the process last execution date Response.</returns>
    public async override Task<FindProcessLastExecutionDateResponse> FindProcessLastExecutionDate(FindProcessLastExecutionDateRequest request, ServerCallContext context)
    {
        try
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Request was cancelled"));
            }

            _logger.LogInformation("Finding process last execution date");

            var results = await _resilienceService.ExecuteWithRetryAsync(() =>
            {
                return _dataService.FindProcessLastExecutionDateAsync(request.ProcessName, context?.CancellationToken ?? CancellationToken.None);
            });

            return new FindProcessLastExecutionDateResponse
            {
                Items =
                {
                    results?.Select(r => new ProcessLastExecutionDateItem
                    {
                        ProcessName = request.ProcessName ?? string.Empty,
                        LastExecutionDate = r.ExecutionDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty,
                        Message = string.Empty
                    }) ?? []
                }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Operation cancelled in FindProcessLastExecutionDate");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in FindProcessLastExecutionDate: {Message}", ex.Message);
            throw CreateInternalErrorException();
        }
    }    

    /// <summary>
    /// Handles the FindProcessLastExecutionDateStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing process last execution date parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task FindProcessLastExecutionDateStream(FindProcessLastExecutionDateRequest request, IServerStreamWriter<ProcessLastExecutionDateItem> responseStream, ServerCallContext context)
    {
        try
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Request was cancelled"));
            }

            _logger.LogInformation("Streaming process last execution date");

            var results = await _resilienceService.ExecuteWithRetryAsync(() =>
            {
                return _dataService.FindProcessLastExecutionDateAsync(request.ProcessName, context?.CancellationToken ?? CancellationToken.None);
            });

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new ProcessLastExecutionDateItem
                    {
                        ProcessName = request.ProcessName ?? string.Empty,
                        LastExecutionDate = item.ExecutionDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty,
                        Message = string.Empty
                    });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Operation cancelled in FindProcessLastExecutionDateStream");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in FindProcessLastExecutionDateStream: {Message}", ex.Message);
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetAllLogs gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the all logs Response.</returns>
    public async override Task<GetAllLogsResponse> GetAllLogs(EmptyRequest request, ServerCallContext context)
    {
        try
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Request was cancelled"));
            }

            _logger.LogInformation("Getting all logs");

            var results = await _resilienceService.ExecuteWithRetryAsync(() =>
            {
                return _dataService.GetAllLogsAsync(context?.CancellationToken ?? CancellationToken.None);
            });

            return new GetAllLogsResponse
            {
                Items =
                {
                    results?.Select(r =>
                    {
                        return new LogItem
                        {
                            LogId = r.Id,
                            ProcessName = r.Name ?? string.Empty,
                            ExecutionDate = r.ExecutionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            Message = r.Message ?? string.Empty
                        };
                    }) ?? []
                }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Operation cancelled in GetAllLogs");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetAllLogs: {Message}", ex.Message);
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetAllLogsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetAllLogsStream(EmptyRequest request, IServerStreamWriter<LogItem> responseStream, ServerCallContext context)
    {
        try
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Request was cancelled"));
            }

            _logger.LogInformation("Streaming all logs");

            var results = await _resilienceService.ExecuteWithRetryAsync(() =>
            {
                return _dataService.GetAllLogsAsync(context?.CancellationToken ?? CancellationToken.None);
            });

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new LogItem
                    {
                        LogId = item.Id,
                        ProcessName = item.Name ?? string.Empty,
                        ExecutionDate = item.ExecutionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        Message = item.Message ?? string.Empty
                    });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Operation cancelled in GetAllLogsStream");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetAllLogsStream: {Message}", ex.Message);
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the InsertLog gRPC request.
    /// </summary>
    /// <param name="request">The request containing log insertion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the log insertion Response.</returns>
    public async override Task<InsertLogResponse> InsertLog(InsertLogRequest request, ServerCallContext context)
    {
        try
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Request was cancelled"));
            }

            // Validate input
            if (string.IsNullOrWhiteSpace(request.ProcessName))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "ProcessName is required"));
            }

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Message is required"));
            }

            _logger.LogInformation("Inserting log");

            var result = await _resilienceService.ExecuteWithRetryAsync(() =>
            {
                DateTime executionDate = DateTime.TryParse(request.ExecutionDate, out var parsedDate) ? parsedDate : DateTime.Now;
                return _dataService.InsertLogAsync(request.ProcessName, executionDate, request.Message, context?.CancellationToken ?? CancellationToken.None);
            });

            return new InsertLogResponse
            {
                Result = result
            };
        }
        catch (FormatException ex)
        {
            _logger.LogWarning(ex, "Invalid date format in InsertLog: {ExecutionDate}", request.ExecutionDate);
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid date format"));
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Operation cancelled in InsertLog");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in InsertLog: {Message}", ex.Message);
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the IsProcessActive gRPC request.
    /// </summary>
    /// <param name="request">The request containing process active check parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the process active Response.</returns>
    public async override Task<IsProcessActiveResponse> IsProcessActive(IsProcessActiveRequest request, ServerCallContext context)
    {
        try
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Request was cancelled"));
            }

            _logger.LogInformation("Checking if process is active");

            var results = await _resilienceService.ExecuteWithRetryAsync(() =>
            {
                return _dataService.IsProcessActiveAsync(request.ProcessName, context?.CancellationToken ?? CancellationToken.None);
            });

            return new IsProcessActiveResponse
            {
                Items = { results?.Select(r => new ProcessActiveItem { ProcessName = request.ProcessName ?? string.Empty, IsActive = results.Count != 0 }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Operation cancelled in IsProcessActive");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in IsProcessActive: {Message}", ex.Message);
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the IsProcessActiveStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing process active check parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task IsProcessActiveStream(IsProcessActiveRequest request, IServerStreamWriter<ProcessActiveItem> responseStream, ServerCallContext context)
    {
        try
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Request was cancelled"));
            }

            _logger.LogInformation("Streaming process active check");

            var results = await _resilienceService.ExecuteWithRetryAsync(() =>
            {
                return _dataService.IsProcessActiveAsync(request.ProcessName, context?.CancellationToken ?? CancellationToken.None);
            });

            if (results != null)
            {
                foreach (var result in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new ProcessActiveItem
                    {
                        ProcessName = request.ProcessName ?? string.Empty,
                        IsActive = true
                    });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Operation cancelled in IsProcessActiveStream");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in IsProcessActiveStream: {Message}", ex.Message);
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetAllLogsPagination gRPC request.
    /// </summary>
    /// <param name="request">The request containing pagination, filtering, and sorting parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated logs Response.</returns>
    public async override Task<GetAllLogsPaginationResponse> GetAllLogsPagination(GetAllLogsPaginationRequest request, ServerCallContext context)
    {
        try
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Request was cancelled"));
            }

            _logger.LogInformation("Getting all logs with pagination, filtering, and sorting");

            var results = await _resilienceService.ExecuteWithRetryAsync(() =>
            {
                return _dataService.GetAllLogsPaginationAsync(
                    request.HasPageNumber ? request.PageNumber : 1,
                    request.HasPageSize ? request.PageSize : 10,
                    request.ProcessName,
                    string.IsNullOrEmpty(request.StartDate) ? null : DateTime.Parse(request.StartDate),
                    string.IsNullOrEmpty(request.EndDate) ? null : DateTime.Parse(request.EndDate),
                    request.MessageFilter,
                    request.SortBy ?? "ExecutionDate",
                    request.SortOrder ?? "DESC", context?.CancellationToken ?? CancellationToken.None);
            });

            return new GetAllLogsPaginationResponse
            {
                Items =
                {
                    results?.Data?.Select(r => new LogItem
                    {
                        LogId = r.Id,
                        ProcessName = r.Name ?? string.Empty,
                        ExecutionDate = r.ExecutionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        Message = r.Message ?? string.Empty }) ?? []
                },
                TotalCount = results?.TotalCount ?? 0
            };
        }
        catch (FormatException ex)
        {
            _logger.LogWarning(ex, "Invalid date format in GetAllLogsPagination: {Message}", ex.Message);
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid date format in request parameters"));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogWarning(ex, "Invalid pagination parameters in GetAllLogsPagination: {Message}", ex.Message);
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid pagination parameters"));
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Operation cancelled in GetAllLogsPagination");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetAllLogsPagination: {Message}", ex.Message);
            throw CreateInternalErrorException();
        }
    }


    #endregion
}
