using AF.ECT.Server.Services.Interfaces;
using AF.ECT.Data.Interfaces;
using Google.Protobuf.Collections;

namespace AF.ECT.Server.Services;

/// <summary>
/// Partial class containing Workflow Methods
/// </summary>
public partial class WorkflowServiceImpl : WorkflowService.WorkflowServiceBase
{
    #region Workflow Methods


    /// <summary>
    /// Handles the GetWorkflowById gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow by ID Response.</returns>
    public async override Task<GetWorkflowByIdResponse> GetWorkflowById(GetWorkflowByIdRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workflow by ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(() =>
            {
                return _dataService.GetWorkflowByIdAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None);
            });

            return new GetWorkflowByIdResponse
            {
                Items =
                {
                    results?.Select(r => new WorkflowByIdItem
                    {
                        WorkflowId = r.workflowId,
                        WorkflowText = r.title ?? string.Empty
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
            _logger.LogWarning(ex, "Operation was cancelled while getting workflow by ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workflow by ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowByIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowByIdStream(GetWorkflowByIdRequest request, IServerStreamWriter<WorkflowByIdItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workflow by ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowByIdAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkflowByIdItem
                    {
                        WorkflowId = (int)item.workflowId,
                        WorkflowText = item.title ?? string.Empty
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
            _logger.LogWarning(ex, "Operation was cancelled while streaming workflow by ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workflow by ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowsByRefId gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflows by ref ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflows by ref ID Response.</returns>
    public async override Task<GetWorkflowsByRefIdResponse> GetWorkflowsByRefId(GetWorkflowsByRefIdRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workflows by ref ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowsByRefIdAsync(request.RefId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkflowsByRefIdResponse
            {
                Items = { results?.Select(r => new WorkflowByRefIdItem { WorkflowId = (int)r.workflowId, RefId = r.refId ?? 0, Module = (int)request.Module }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workflows by ref ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workflows by ref ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowsByRefIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflows by ref ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowsByRefIdStream(GetWorkflowsByRefIdRequest request, IServerStreamWriter<WorkflowByRefIdItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workflows by ref ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowsByRefIdAsync(request.RefId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkflowByRefIdItem
                    {
                        WorkflowId = (int)item.workflowId,
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
            _logger.LogWarning(ex, "Operation was cancelled while streaming workflows by ref ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workflows by ref ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowsByRefIdAndType gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflows by ref ID and type parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflows by ref ID and type Response.</returns>
    public async override Task<GetWorkflowsByRefIdAndTypeResponse> GetWorkflowsByRefIdAndType(GetWorkflowsByRefIdAndTypeRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workflows by ref ID and type");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowsByRefIdAndTypeAsync(request.RefId, (byte?)request.Module, request.WorkflowType, context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkflowsByRefIdAndTypeResponse
            {
                Items = { results?.Select(r => new WorkflowByRefIdAndTypeItem { WorkflowId = (int)r.workflowId, RefId = r.refId ?? 0, Module = (int)request.Module, WorkflowType = r.workflowType ?? 0 }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workflows by ref ID and type");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workflows by ref ID and type");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowsByRefIdAndTypeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflows by ref ID and type parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowsByRefIdAndTypeStream(GetWorkflowsByRefIdAndTypeRequest request, IServerStreamWriter<WorkflowByRefIdAndTypeItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workflows by ref ID and type");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowsByRefIdAndTypeAsync(request.RefId, (byte?)request.Module, request.WorkflowType, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkflowByRefIdAndTypeItem
                    {
                        WorkflowId = (int)item.workflowId,
                        RefId = item.refId ?? 0,
                        Module = (int)request.Module,
                        WorkflowType = item.workflowType ?? 0
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
            _logger.LogWarning(ex, "Operation was cancelled while streaming workflows by ref ID and type");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workflows by ref ID and type");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowTypes gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow types Response.</returns>
    public async override Task<GetWorkflowTypesResponse> GetWorkflowTypes(EmptyRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workflow types");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTypesAsync(context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkflowTypesResponse
            {
                Items = { results?.Select(r => new WorkflowTypeItem { WorkflowTypeId = r.workflowType ?? 0, TypeName = r.typeName ?? string.Empty }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workflow types");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workflow types");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowTypesStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowTypesStream(EmptyRequest request, IServerStreamWriter<WorkflowTypeItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workflow types");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTypesAsync(context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkflowTypeItem
                    {
                        WorkflowTypeId = item.workflowType ?? 0,
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
            _logger.LogWarning(ex, "Operation was cancelled while streaming workflow types");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workflow types");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the InsertWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow insertion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow insertion Response.</returns>
    public async override Task<InsertWorkflowResponse> InsertWorkflow(InsertWorkflowRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Inserting workflow");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertWorkflowAsync(request.RefId, (byte?)request.Module, request.WorkflowType, request.WorkflowText, request.UserId, context?.CancellationToken ?? CancellationToken.None));

            return new InsertWorkflowResponse
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
            _logger.LogWarning(ex, "Operation was cancelled while inserting workflow");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while inserting workflow");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the UpdateWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow update Response.</returns>
    public async override Task<UpdateWorkflowResponse> UpdateWorkflow(UpdateWorkflowRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Updating workflow");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateWorkflowAsync(request.WorkflowId, request.WorkflowText, request.UserId, context?.CancellationToken ?? CancellationToken.None));

            return new UpdateWorkflowResponse
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
            _logger.LogWarning(ex, "Operation was cancelled while updating workflow");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating workflow");
            throw CreateInternalErrorException();
        }
    }


    #endregion
}
