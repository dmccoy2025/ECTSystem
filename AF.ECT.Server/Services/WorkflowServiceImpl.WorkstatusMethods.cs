using AF.ECT.Server.Services.Interfaces;
using AF.ECT.Data.Interfaces;
using Google.Protobuf.Collections;

namespace AF.ECT.Server.Services;

/// <summary>
/// Partial class containing Workstatus Methods
/// </summary>
public partial class WorkflowServiceImpl : WorkflowService.WorkflowServiceBase
{
    #region Workstatus Methods


    /// <summary>
    /// Handles the GetWorkstatusById gRPC request.
    /// </summary>
    /// <param name="request">The request containing workstatus ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus by ID Response.</returns>
    public async override Task<GetWorkstatusByIdResponse> GetWorkstatusById(GetWorkstatusByIdRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workstatus by ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusByIdAsync(request.WorkstatusId, context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkstatusByIdResponse
            {
                Items = { results?.Select(r => new WorkstatusByIdItem { WorkstatusId = r.workstatusId ?? 0, WorkstatusText = r.name ?? string.Empty }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workstatus by ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workstatus by ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkstatusByIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workstatus ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkstatusByIdStream(GetWorkstatusByIdRequest request, IServerStreamWriter<WorkstatusByIdItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workstatus by ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusByIdAsync(request.WorkstatusId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkstatusByIdItem
                    {
                        WorkstatusId = item.workstatusId ?? 0,
                        WorkstatusText = item.name ?? string.Empty
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
            _logger.LogWarning(ex, "Operation was cancelled while streaming workstatus by ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workstatus by ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkstatusesByRefId gRPC request.
    /// </summary>
    /// <param name="request">The request containing workstatuses by ref ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatuses by ref ID Response.</returns>
    public async override Task<GetWorkstatusesByRefIdResponse> GetWorkstatusesByRefId(GetWorkstatusesByRefIdRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workstatuses by ref ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusesByRefIdAsync(request.RefId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkstatusesByRefIdResponse
            {
                Items = { results?.Select(r => new WorkstatusByRefIdItem { WorkstatusId = r.workstatusId ?? 0, RefId = r.refId ?? 0, Module = (int)request.Module }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workstatuses by ref ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workstatuses by ref ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkstatusesByRefIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workstatuses by ref ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkstatusesByRefIdStream(GetWorkstatusesByRefIdRequest request, IServerStreamWriter<WorkstatusByRefIdItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workstatuses by ref ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusesByRefIdAsync(request.RefId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkstatusByRefIdItem
                    {
                        WorkstatusId = item.workstatusId ?? 0,
                        RefId = item.refId ?? 0,
                        Module = (int)request.Module
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
            _logger.LogWarning(ex, "Operation was cancelled while streaming workstatuses by ref ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workstatuses by ref ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkstatusesByRefIdAndType gRPC request.
    /// </summary>
    /// <param name="request">The request containing workstatuses by ref ID and type parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatuses by ref ID and type Response.</returns>
    public async override Task<GetWorkstatusesByRefIdAndTypeResponse> GetWorkstatusesByRefIdAndType(GetWorkstatusesByRefIdAndTypeRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workstatuses by ref ID and type");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusesByRefIdAndTypeAsync(request.RefId, (byte?)request.Module, request.WorkstatusType, context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkstatusesByRefIdAndTypeResponse
            {
                Items = { results?.Select(r => new WorkstatusByRefIdAndTypeItem { WorkstatusId = r.workstatusId ?? 0, RefId = r.refId ?? 0, Module = (int)request.Module, WorkstatusType = r.workstatusType ?? 0 }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workstatuses by ref ID and type");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workstatuses by ref ID and type");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkstatusesByRefIdAndTypeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workstatuses by ref ID and type parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkstatusesByRefIdAndTypeStream(GetWorkstatusesByRefIdAndTypeRequest request, IServerStreamWriter<WorkstatusByRefIdAndTypeItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workstatuses by ref ID and type");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusesByRefIdAndTypeAsync(request.RefId, (byte?)request.Module, request.WorkstatusType, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkstatusByRefIdAndTypeItem
                    {
                        WorkstatusId = item.workstatusId ?? 0,
                        RefId = item.refId ?? 0,
                        Module = (int)request.Module,
                        WorkstatusType = item.workstatusType ?? 0
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
            _logger.LogWarning(ex, "Operation was cancelled while streaming workstatuses by ref ID and type");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workstatuses by ref ID and type");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkstatusTypes gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus types Response.</returns>
    public async override Task<GetWorkstatusTypesResponse> GetWorkstatusTypes(EmptyRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workstatus types");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusTypesAsync(context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkstatusTypesResponse
            {
                Items = { results?.Select(r => new WorkstatusTypeItem { WorkstatusTypeId = r.workstatusType ?? 0, TypeName = r.typeName ?? string.Empty }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workstatus types");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workstatus types");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkstatusTypesStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkstatusTypesStream(EmptyRequest request, IServerStreamWriter<WorkstatusTypeItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workstatus types");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusTypesAsync(context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkstatusTypeItem
                    {
                        WorkstatusTypeId = item.workstatusType ?? 0,
                        TypeName = item.typeName ?? string.Empty
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
            _logger.LogWarning(ex, "Operation was cancelled while streaming workstatus types");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workstatus types");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the InsertWorkstatus gRPC request.
    /// </summary>
    /// <param name="request">The request containing workstatus insertion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus insertion Response.</returns>
    public async override Task<InsertWorkstatusResponse> InsertWorkstatus(InsertWorkstatusRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Inserting workstatus");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertWorkstatusAsync(request.RefId, (byte?)request.Module, request.WorkstatusType, request.WorkstatusText, request.UserId, context?.CancellationToken ?? CancellationToken.None));

            return new InsertWorkstatusResponse
            {
                Result = result
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while inserting workstatus");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while inserting workstatus");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the UpdateWorkstatus gRPC request.
    /// </summary>
    /// <param name="request">The request containing workstatus update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus update Response.</returns>
    public async override Task<UpdateWorkstatusResponse> UpdateWorkstatus(UpdateWorkstatusRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Updating workstatus");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateWorkstatusAsync(request.WorkstatusId, request.WorkstatusText, request.UserId, context?.CancellationToken ?? CancellationToken.None));

            return new UpdateWorkstatusResponse
            {
                Result = result
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while updating workstatus");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating workstatus");
            throw CreateInternalErrorException();
        }
    }


    #endregion
}
