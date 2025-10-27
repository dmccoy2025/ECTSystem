using AF.ECT.Server.Services.Interfaces;
using AF.ECT.Data.Interfaces;
using Google.Protobuf.Collections;

namespace AF.ECT.Server.Services;

/// <summary>
/// Partial class containing Core Workflow Methods
/// </summary>
public partial class WorkflowServiceImpl : WorkflowService.WorkflowServiceBase
{
    #region Core Workflow Methods


    /// <summary>
    /// Handles the AddSignature gRPC request.
    /// </summary>
    /// <param name="request">The request containing signature addition parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the signature addition Response.</returns>
    public async override Task<AddSignatureResponse> AddSignature(AddSignatureRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Adding signature");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.AddSignatureAsync(request, context?.CancellationToken ?? CancellationToken.None));

            return new AddSignatureResponse
            {
                Items = { results?.Select(r => new SignatureItem { SignatureId = 0, RefId = 0, UserId = 0, SignatureDate = "2023-01-01" }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding signature");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the CopyActions gRPC request.
    /// </summary>
    /// <param name="request">The request containing action copy parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the action copy Response.</returns>
    public async override Task<CopyActionsResponse> CopyActions(CopyActionsRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Copying actions");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.CopyActionsAsync(request.DestWsoid, request.SrcWsoid, context?.CancellationToken ?? CancellationToken.None));

            return new CopyActionsResponse
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
            _logger.LogWarning(ex, "Operation was cancelled");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while copying actions");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the CopyRules gRPC request.
    /// </summary>
    /// <param name="request">The request containing rule copy parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the rule copy Response.</returns>
    public async override Task<CopyRulesResponse> CopyRules(CopyRulesRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Copying rules");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.CopyRulesAsync(request.DestWsoid, request.SrcWsoid, context?.CancellationToken ?? CancellationToken.None));

            return new CopyRulesResponse
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
            _logger.LogWarning(ex, "Operation was cancelled");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while copying rules");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the CopyWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow copy parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow copy Response.</returns>
    public async override Task<CopyWorkflowResponse> CopyWorkflow(CopyWorkflowRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Copying workflow");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.CopyWorkflowAsync(request.FromId, request.ToId, context?.CancellationToken ?? CancellationToken.None));

            return new CopyWorkflowResponse
            {
                Items = { results?.Select(r => new WorkflowCopyItem { WorkflowId = 0, WorkflowName = "Workflow", CopySuccess = true }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while copying workflow");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the DeleteStatusCode gRPC request.
    /// </summary>
    /// <param name="request">The request containing status code deletion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status code deletion Response.</returns>
    public async override Task<DeleteStatusCodeResponse> DeleteStatusCode(DeleteStatusCodeRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Deleting status code");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.DeleteStatusCodeAsync(request.StatusId, context?.CancellationToken ?? CancellationToken.None));

            return new DeleteStatusCodeResponse
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
            _logger.LogWarning(ex, "Operation was cancelled");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting status code");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetActionsByStep gRPC request.
    /// </summary>
    /// <param name="request">The request containing actions by step parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the actions by step Response.</returns>
    public async override Task<GetActionsByStepResponse> GetActionsByStep(GetActionsByStepRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting actions by step");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetActionsByStepAsync(request.StepId, context?.CancellationToken ?? CancellationToken.None));

            return new GetActionsByStepResponse
            {
                Items = { results?.Select(r => new ActionByStepItem { ActionId = r.wsa_id, StepId = r.wso_id, ActionType = r.actionType.ToString(), ActionDescription = r.text ?? string.Empty }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting actions by step");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetActiveCases gRPC request.
    /// </summary>
    /// <param name="request">The request containing active cases parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the active cases Response.</returns>
    public async override Task<GetActiveCasesResponse> GetActiveCases(GetActiveCasesRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting active cases");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetActiveCasesAsync(request.RefId, (short?)request.GroupId, context?.CancellationToken ?? CancellationToken.None));

            return new GetActiveCasesResponse
            {
                Items = { results?.Select(r => new ActiveCaseItem { CaseId = 0, RefId = 0, GroupId = 0, Status = "Status" }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting active cases");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetAllFindingByReasonOf gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the all findings by reason of Response.</returns>
    public async override Task<GetAllFindingByReasonOfResponse> GetAllFindingByReasonOf(EmptyRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting all findings by reason of");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetAllFindingByReasonOfAsync(context?.CancellationToken ?? CancellationToken.None));

            return new GetAllFindingByReasonOfResponse
            {
                Items = { results?.Select(r => new FindingByReasonOfItem { FindingId = r.Id, Reason = string.Empty, Description = r.Description ?? string.Empty }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all findings by reason of");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetAllLocks gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the all locks Response.</returns>
    public async override Task<GetAllLocksResponse> GetAllLocks(EmptyRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting all locks");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetAllLocksAsync(context?.CancellationToken ?? CancellationToken.None));

            return new GetAllLocksResponse
            {
                Items = { results?.Select(r => new LockItem { LockId = r.lockId, UserId = r.userId, LockType = r.moduleName ?? string.Empty, LockTime = r.lockTime.ToString("yyyy-MM-dd") }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all locks");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetCancelReasons gRPC request.
    /// </summary>
    /// <param name="request">The request containing cancel reasons parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the cancel reasons Response.</returns>
    public async override Task<GetCancelReasonsResponse> GetCancelReasons(GetCancelReasonsRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting cancel reasons");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetCancelReasonsAsync((byte?)request.WorkflowId, request.IsFormal, context?.CancellationToken ?? CancellationToken.None));

            return new GetCancelReasonsResponse
            {
                Items = { results?.Select(r => new CancelReasonItem { ReasonId = r.Id, ReasonText = r.Description ?? string.Empty, IsFormal = request.IsFormal }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting cancel reasons");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetCreatableByGroup gRPC request.
    /// </summary>
    /// <param name="request">The request containing creatable by group parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the creatable by group Response.</returns>
    public async override Task<GetCreatableByGroupResponse> GetCreatableByGroup(GetCreatableByGroupRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting creatable by group");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetCreatableByGroupAsync(request.Compo, (byte?)request.Module, (byte?)request.GroupId, context?.CancellationToken ?? CancellationToken.None));

            return new GetCreatableByGroupResponse
            {
                Items = { results?.Select(r => new CreatableByGroupItem { WorkflowId = r.workFlowId, WorkflowName = r.title ?? string.Empty, GroupId = request.GroupId }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting creatable by group");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting creatable by group");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetFindingByReasonOfById gRPC request.
    /// </summary>
    /// <param name="request">The request containing finding by reason of by ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the finding by reason of by ID Response.</returns>
    public async override Task<GetFindingByReasonOfByIdResponse> GetFindingByReasonOfById(GetFindingByReasonOfByIdRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting finding by reason of by ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetFindingByReasonOfByIdAsync(request.Id, context?.CancellationToken ?? CancellationToken.None));

            return new GetFindingByReasonOfByIdResponse
            {
                Items = { results?.Select(r => new FindingByReasonOfByIdItem { FindingId = r.Id, Reason = string.Empty, Description = r.Description ?? string.Empty, Id = r.Id }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting finding by reason of by ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting finding by reason of by ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetFindings gRPC request.
    /// </summary>
    /// <param name="request">The request containing findings parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the findings Response.</returns>
    public async override Task<GetFindingsResponse> GetFindings(GetFindingsRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting findings");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetFindingsAsync((byte?)request.WorkflowId, request.GroupId, context?.CancellationToken ?? CancellationToken.None));

            return new GetFindingsResponse
            {
                Items = { results?.Select(r => new FindingItem { FindingId = r.Id ?? 0, WorkflowId = request.WorkflowId, GroupId = request.GroupId, FindingText = r.Description ?? string.Empty }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting findings");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting findings");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetModuleFromWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing module from workflow parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the module from workflow Response.</returns>
    public async override Task<GetModuleFromWorkflowResponse> GetModuleFromWorkflow(GetModuleFromWorkflowRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting module from workflow");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetModuleFromWorkflowAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            return new GetModuleFromWorkflowResponse
            {
                Items = { results?.Select(r => new ModuleFromWorkflowItem { ModuleId = 0, ModuleName = "Module", WorkflowId = 0 }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting module from workflow");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting module from workflow");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetPageAccessByGroup gRPC request.
    /// </summary>
    /// <param name="request">The request containing page access by group parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the page access by group Response.</returns>
    public async override Task<GetPageAccessByGroupResponse> GetPageAccessByGroup(GetPageAccessByGroupRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting page access by group");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPageAccessByGroupAsync((byte?)request.Workflow, request.Status, (byte?)request.Group, context?.CancellationToken ?? CancellationToken.None));

            return new GetPageAccessByGroupResponse
            {
                Items = { results?.Select(r => new PageAccessByGroupItem { PageId = r.PageId, PageName = r.PageTitle ?? string.Empty, HasAccess = r.Access != 0, GroupId = r.GroupId ?? 0 }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting page access by group");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting page access by group");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetPageAccessByWorkflowView gRPC request.
    /// </summary>
    /// <param name="request">The request containing page access by workflow view parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the page access by workflow view Response.</returns>
    public async override Task<GetPageAccessByWorkflowViewResponse> GetPageAccessByWorkflowView(GetPageAccessByWorkflowViewRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting page access by workflow view");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPageAccessByWorkflowViewAsync(request.Compo, (byte?)request.Workflow, request.Status, context?.CancellationToken ?? CancellationToken.None));

            return new GetPageAccessByWorkflowViewResponse
            {
                Items = { results?.Select(r => new PageAccessByWorkflowViewItem { PageId = r.PageId, PageName = r.PageTitle ?? string.Empty, HasAccess = r.Access != 0, Component = request.Compo }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting page access by workflow view");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting page access by workflow view");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetPagesByWorkflowId gRPC request.
    /// </summary>
    /// <param name="request">The request containing pages by workflow ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the pages by workflow ID Response.</returns>
    public async override Task<GetPagesByWorkflowIdResponse> GetPagesByWorkflowId(GetPagesByWorkflowIdRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting pages by workflow ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPagesByWorkflowIdAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            return new GetPagesByWorkflowIdResponse
            {
                Items = { results?.Select(r => new PageByWorkflowItem { PageId = r.pageId, PageName = r.title ?? string.Empty, WorkflowId = request.WorkflowId, PageUrl = "/page" }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting pages by workflow ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting pages by workflow ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetPermissions gRPC request.
    /// </summary>
    /// <param name="request">The request containing permissions parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the permissions Response.</returns>
    public async override Task<GetPermissionsResponse> GetPermissions(GetPermissionsRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting permissions");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPermissionsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            return new GetPermissionsResponse
            {
                Items = { results?.Select(r => new PermissionItem { PermissionId = r.groupId, PermissionName = r.name ?? string.Empty, WorkflowId = request.WorkflowId, IsGranted = r.canView ?? false }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting permissions");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting permissions");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetPermissionsByCompo gRPC request.
    /// </summary>
    /// <param name="request">The request containing permissions by component parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the permissions by component Response.</returns>
    public async override Task<GetPermissionsByCompoResponse> GetPermissionsByCompo(GetPermissionsByCompoRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting permissions by component");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPermissionsByCompoAsync((byte?)request.WorkflowId, request.Compo, context?.CancellationToken ?? CancellationToken.None));

            return new GetPermissionsByCompoResponse
            {
                Items = { results?.Select(r => new PermissionByCompoItem { PermissionId = r.groupId, PermissionName = r.name ?? string.Empty, Component = request.Compo, IsGranted = r.canView ?? false }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting permissions by component");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting permissions by component");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetReturnReasons gRPC request.
    /// </summary>
    /// <param name="request">The request containing return reasons parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the return reasons Response.</returns>
    public async override Task<GetReturnReasonsResponse> GetReturnReasons(GetReturnReasonsRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting return reasons");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetReturnReasonsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            return new GetReturnReasonsResponse
            {
                Items = { results?.Select(r => new ReturnReasonItem { ReasonId = r.Id, ReasonText = r.Description ?? string.Empty, WorkflowId = request.WorkflowId }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting return reasons");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting return reasons");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetRwoaReasons gRPC request.
    /// </summary>
    /// <param name="request">The request containing RWOA reasons parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the RWOA reasons Response.</returns>
    public async override Task<GetRwoaReasonsResponse> GetRwoaReasons(GetRwoaReasonsRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting RWOA reasons");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetRwoaReasonsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            return new GetRwoaReasonsResponse
            {
                Items = { results?.Select(r => new RwoaReasonItem { ReasonId = r.Id, ReasonText = r.Description ?? string.Empty, WorkflowId = request.WorkflowId }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting RWOA reasons");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting RWOA reasons");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByCompo gRPC request.
    /// </summary>
    /// <param name="request">The request containing status codes by component parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by component Response.</returns>
    public async override Task<GetStatusCodesByCompoResponse> GetStatusCodesByCompo(GetStatusCodesByCompoRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting status codes by component");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByCompoAsync(request.Compo, context?.CancellationToken ?? CancellationToken.None));

            return new GetStatusCodesByCompoResponse
            {
                Items = { results?.Select(r => new StatusCodeByCompoItem { StatusId = r.statusId, StatusName = r.description ?? string.Empty, Component = r.compo ?? string.Empty }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting status codes by component");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting status codes by component");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByCompoAndModule gRPC request.
    /// </summary>
    /// <param name="request">The request containing status codes by component and module parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by component and module Response.</returns>
    public async override Task<GetStatusCodesByCompoAndModuleResponse> GetStatusCodesByCompoAndModule(GetStatusCodesByCompoAndModuleRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting status codes by component and module");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByCompoAndModuleAsync(request.Compo, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

            return new GetStatusCodesByCompoAndModuleResponse
            {
                Items = { results?.Select(r => new StatusCodeByCompoAndModuleItem { StatusId = r.statusId, StatusName = r.description ?? string.Empty, Component = r.compo ?? string.Empty, ModuleId = r.moduleId }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting status codes by component and module");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting status codes by component and module");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesBySignCode gRPC request.
    /// </summary>
    /// <param name="request">The request containing status codes by sign code parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by sign code Response.</returns>
    public async override Task<GetStatusCodesBySignCodeResponse> GetStatusCodesBySignCode(GetStatusCodesBySignCodeRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting status codes by sign code");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesBySignCodeAsync((short?)request.GroupId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

            return new GetStatusCodesBySignCodeResponse
            {
                Items = { results?.Select(r => new StatusCodeBySignCodeItem { StatusId = r.statusId, StatusName = r.description ?? string.Empty, GroupId = request.GroupId, ModuleId = request.Module }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting status codes by sign code");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting status codes by sign code");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing status codes by workflow parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by workflow Response.</returns>
    public async override Task<GetStatusCodesByWorkflowResponse> GetStatusCodesByWorkflow(GetStatusCodesByWorkflowRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting status codes by workflow");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByWorkflowAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            return new GetStatusCodesByWorkflowResponse
            {
                Items = { results?.Select(r => new StatusCodeByWorkflowItem { StatusId = 0, StatusName = "Status", WorkflowId = 0 }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting status codes by workflow");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting status codes by workflow");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByWorkflowAndAccessScope gRPC request.
    /// </summary>
    /// <param name="request">The request containing status codes by workflow and access scope parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by workflow and access scope Response.</returns>
    public async override Task<GetStatusCodesByWorkflowAndAccessScopeResponse> GetStatusCodesByWorkflowAndAccessScope(GetStatusCodesByWorkflowAndAccessScopeRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting status codes by workflow and access scope");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByWorkflowAndAccessScopeAsync((byte?)request.WorkflowId, (byte?)request.AccessScope, context?.CancellationToken ?? CancellationToken.None));

            return new GetStatusCodesByWorkflowAndAccessScopeResponse
            {
                Items = { results?.Select(r => new StatusCodeByWorkflowAndAccessScopeItem { StatusId = 0, StatusName = "Status", WorkflowId = 0, AccessScope = 0 }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting status codes by workflow and access scope");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting status codes by workflow and access scope");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodeScope gRPC request.
    /// </summary>
    /// <param name="request">The request containing status code scope parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status code scope Response.</returns>
    public async override Task<GetStatusCodeScopeResponse> GetStatusCodeScope(GetStatusCodeScopeRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting status code scope");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodeScopeAsync((byte?)request.StatusId, context?.CancellationToken ?? CancellationToken.None));

            return new GetStatusCodeScopeResponse
            {
                Items = { results?.Select(r => new StatusCodeScopeItem { StatusId = request.StatusId, ScopeName = r.accessScope.ToString(), Description = "Access Scope" }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting status code scope");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting status code scope");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStepsByWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing steps by workflow parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the steps by workflow Response.</returns>
    public async override Task<GetStepsByWorkflowResponse> GetStepsByWorkflow(GetStepsByWorkflowRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting steps by workflow");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStepsByWorkflowAsync((byte?)request.Workflow, context?.CancellationToken ?? CancellationToken.None));

            return new GetStepsByWorkflowResponse
            {
                Items = { results?.Select(r => new StepByWorkflowItem { StepId = 0, StepName = "Step", WorkflowId = 0, StepOrder = 0 }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting steps by workflow");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting steps by workflow");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStepsByWorkflowAndStatus gRPC request.
    /// </summary>
    /// <param name="request">The request containing steps by workflow and status parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the steps by workflow and status Response.</returns>
    public async override Task<GetStepsByWorkflowAndStatusResponse> GetStepsByWorkflowAndStatus(GetStepsByWorkflowAndStatusRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting steps by workflow and status");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStepsByWorkflowAndStatusAsync((byte?)request.Workflow, (byte?)request.Status, request.DeathStatus, context?.CancellationToken ?? CancellationToken.None));

            return new GetStepsByWorkflowAndStatusResponse
            {
                Items = { results?.Select(r => new StepByWorkflowAndStatusItem { StepId = 0, StepName = "Step", WorkflowId = 0, StatusId = 0, DeathStatus = "Status" }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting steps by workflow and status");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting steps by workflow and status");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetViewableByGroup gRPC request.
    /// </summary>
    /// <param name="request">The request containing viewable by group parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the viewable by group Response.</returns>
    public async override Task<GetViewableByGroupResponse> GetViewableByGroup(GetViewableByGroupRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting viewable by group");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetViewableByGroupAsync((byte?)request.GroupId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

            return new GetViewableByGroupResponse
            {
                Items = { results?.Select(r => new ViewableByGroupItem { WorkflowId = r.workFlowId, WorkflowName = r.title, GroupId = request.GroupId, IsViewable = r.active }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting viewable by group");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting viewable by group");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowByCompo gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow by component parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow by component Response.</returns>
    public async override Task<GetWorkflowByCompoResponse> GetWorkflowByCompo(GetWorkflowByCompoRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workflow by component");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowByCompoAsync(request.Compo, request.UserId, context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkflowByCompoResponse
            {
                Items = { results?.Select(r => new WorkflowByCompoItem { WorkflowId = r.workflowId, WorkflowName = r.title ?? string.Empty, Component = request.Compo, UserId = request.UserId }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workflow by component");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workflow by component");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowFromModule gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow from module parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow from module Response.</returns>
    public async override Task<GetWorkflowFromModuleResponse> GetWorkflowFromModule(GetWorkflowFromModuleRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workflow from module");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowFromModuleAsync(request.ModuleId, context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkflowFromModuleResponse
            {
                Items = { results?.Select(r => new WorkflowFromModuleItem { WorkflowId = r.workflowId ?? 0, WorkflowName = "Workflow", ModuleId = request.ModuleId }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workflow from module");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workflow from module");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowInitialStatusCode gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow initial status code parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow initial status code Response.</returns>
    public async override Task<GetWorkflowInitialStatusCodeResponse> GetWorkflowInitialStatusCode(GetWorkflowInitialStatusCodeRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workflow initial status code");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowInitialStatusCodeAsync(request.Compo, request.Module, request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkflowInitialStatusCodeResponse
            {
                Items = { results?.Select(r => new WorkflowInitialStatusCodeItem { WorkflowId = request.WorkflowId, InitialStatusId = r.statusId ?? 0, StatusName = "Status" }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workflow initial status code");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workflow initial status code");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowTitle gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow title parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow title Response.</returns>
    public async override Task<GetWorkflowTitleResponse> GetWorkflowTitle(GetWorkflowTitleRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workflow title");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTitleAsync(request.ModuleId, request.SubCase, context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkflowTitleResponse
            {
                Items = { results?.Select(r => new WorkflowTitleItem { WorkflowId = 0, Title = "Title", ModuleId = 0, SubCase = 0 }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workflow title");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workflow title");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowTitleByWorkStatusId gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow title by work status ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow title by work status ID Response.</returns>
    public async override Task<GetWorkflowTitleByWorkStatusIdResponse> GetWorkflowTitleByWorkStatusId(GetWorkflowTitleByWorkStatusIdRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting workflow title by work status ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTitleByWorkStatusIdAsync(request.WorkflowId, request.SubCase, context?.CancellationToken ?? CancellationToken.None));

            return new GetWorkflowTitleByWorkStatusIdResponse
            {
                Items = { results?.Select(r => new WorkflowTitleByWorkStatusIdItem { WorkflowId = 0, Title = "Title", WorkStatusId = 0, SubCase = 0 }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while getting workflow title by work status ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting workflow title by work status ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the InsertAction gRPC request.
    /// </summary>
    /// <param name="request">The request containing action insertion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the action insertion Response.</returns>
    public async override Task<InsertActionResponse> InsertAction(InsertActionRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Inserting action");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertActionAsync(request, context?.CancellationToken ?? CancellationToken.None));

            return new InsertActionResponse
            {
                Items = { results?.Select(r => new InsertActionItem { ActionId = 0, Type = 0, StepId = 0, ResultMessage = "Success" }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while inserting action");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while inserting action");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the InsertOptionAction gRPC request.
    /// </summary>
    /// <param name="request">The request containing option action insertion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the option action insertion Response.</returns>
    public async override Task<InsertOptionActionResponse> InsertOptionAction(InsertOptionActionRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Inserting option action");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertOptionActionAsync(request, context?.CancellationToken ?? CancellationToken.None));

            return new InsertOptionActionResponse
            {
                Items = { results?.Select(r => new InsertOptionActionItem { ActionId = 0, Type = 0, Wsoid = 0, ResultMessage = "Success" }) ?? [] }
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while inserting option action");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while inserting option action");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the AddSignatureStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing signature addition parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task AddSignatureStream(AddSignatureRequest request, IServerStreamWriter<SignatureItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming signature addition");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.AddSignatureAsync(request, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new SignatureItem { SignatureId = 0, RefId = 0, UserId = 0, SignatureDate = "2023-01-01" });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming signature addition");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming signature addition");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the CopyWorkflowStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow copy parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task CopyWorkflowStream(CopyWorkflowRequest request, IServerStreamWriter<WorkflowCopyItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workflow copy");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.CopyWorkflowAsync(request.FromId, request.ToId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkflowCopyItem { WorkflowId = 0, WorkflowName = "Workflow", CopySuccess = true });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming workflow copy");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workflow copy");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetActionsByStepStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing actions by step parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetActionsByStepStream(GetActionsByStepRequest request, IServerStreamWriter<ActionByStepItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming actions by step");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetActionsByStepAsync(request.StepId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new ActionByStepItem { ActionId = item.wsa_id, StepId = item.wso_id, ActionType = item.actionType.ToString(), ActionDescription = item.text ?? string.Empty });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming actions by step");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming actions by step");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetActiveCasesStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing active cases parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetActiveCasesStream(GetActiveCasesRequest request, IServerStreamWriter<ActiveCaseItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming active cases");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetActiveCasesAsync(request.RefId, (short?)request.GroupId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new ActiveCaseItem { CaseId = 0, RefId = 0, GroupId = 0, Status = "Status" });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming active cases");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming active cases");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetAllFindingByReasonOfStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetAllFindingByReasonOfStream(EmptyRequest request, IServerStreamWriter<FindingByReasonOfItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming all findings by reason of");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetAllFindingByReasonOfAsync(context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new FindingByReasonOfItem { FindingId = item.Id, Reason = string.Empty, Description = item.Description ?? string.Empty });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming all findings by reason of");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming all findings by reason of");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetAllLocksStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetAllLocksStream(EmptyRequest request, IServerStreamWriter<LockItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming all locks");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetAllLocksAsync(context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new LockItem { LockId = item.lockId, UserId = item.userId, LockType = item.moduleName ?? string.Empty, LockTime = item.lockTime.ToString("yyyy-MM-dd") });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming all locks");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming all locks");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetCancelReasonsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing cancel reasons parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetCancelReasonsStream(GetCancelReasonsRequest request, IServerStreamWriter<CancelReasonItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming cancel reasons");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetCancelReasonsAsync((byte?)request.WorkflowId, request.IsFormal, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new CancelReasonItem { ReasonId = item.Id, ReasonText = item.Description ?? string.Empty, IsFormal = request.IsFormal });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming cancel reasons");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming cancel reasons");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetCreatableByGroupStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing creatable by group parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetCreatableByGroupStream(GetCreatableByGroupRequest request, IServerStreamWriter<CreatableByGroupItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming creatable by group");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetCreatableByGroupAsync(request.Compo, (byte?)request.Module, (byte?)request.GroupId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new CreatableByGroupItem { WorkflowId = item.workFlowId, WorkflowName = item.title ?? string.Empty, GroupId = request.GroupId });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming creatable by group");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming creatable by group");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetFindingByReasonOfByIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing finding by reason of by ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetFindingByReasonOfByIdStream(GetFindingByReasonOfByIdRequest request, IServerStreamWriter<FindingByReasonOfByIdItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming finding by reason of by ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetFindingByReasonOfByIdAsync(request.Id, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new FindingByReasonOfByIdItem { FindingId = item.Id, Reason = string.Empty, Description = item.Description ?? string.Empty, Id = item.Id });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming finding by reason of by ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming finding by reason of by ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetFindingsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing findings parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetFindingsStream(GetFindingsRequest request, IServerStreamWriter<FindingItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming findings");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetFindingsAsync((byte?)request.WorkflowId, request.GroupId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new FindingItem { FindingId = item.Id ?? 0, WorkflowId = request.WorkflowId, GroupId = request.GroupId, FindingText = item.Description ?? string.Empty });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming findings");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming findings");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetModuleFromWorkflowStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing module from workflow parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetModuleFromWorkflowStream(GetModuleFromWorkflowRequest request, IServerStreamWriter<ModuleFromWorkflowItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming module from workflow");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetModuleFromWorkflowAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new ModuleFromWorkflowItem { ModuleId = 0, ModuleName = "Module", WorkflowId = 0 });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming module from workflow");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming module from workflow");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetPageAccessByGroupStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing page access by group parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetPageAccessByGroupStream(GetPageAccessByGroupRequest request, IServerStreamWriter<PageAccessByGroupItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming page access by group");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPageAccessByGroupAsync((byte?)request.Workflow, request.Status, (byte?)request.Group, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new PageAccessByGroupItem { PageId = item.PageId, PageName = item.PageTitle ?? string.Empty, HasAccess = item.Access != 0, GroupId = item.GroupId ?? 0 });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming page access by group");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming page access by group");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetPageAccessByWorkflowViewStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing page access by workflow view parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetPageAccessByWorkflowViewStream(GetPageAccessByWorkflowViewRequest request, IServerStreamWriter<PageAccessByWorkflowViewItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming page access by workflow view");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPageAccessByWorkflowViewAsync(request.Compo, (byte?)request.Workflow, request.Status, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new PageAccessByWorkflowViewItem { PageId = item.PageId, PageName = item.PageTitle ?? string.Empty, HasAccess = item.Access != 0, Component = request.Compo });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming page access by workflow view");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming page access by workflow view");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetPagesByWorkflowIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing pages by workflow ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetPagesByWorkflowIdStream(GetPagesByWorkflowIdRequest request, IServerStreamWriter<PageByWorkflowItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming pages by workflow ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPagesByWorkflowIdAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new PageByWorkflowItem { PageId = item.pageId, PageName = item.title ?? string.Empty, WorkflowId = request.WorkflowId, PageUrl = "/page" });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming pages by workflow ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming pages by workflow ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetPermissionsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing permissions parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetPermissionsStream(GetPermissionsRequest request, IServerStreamWriter<PermissionItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming permissions");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPermissionsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new PermissionItem { PermissionId = item.groupId, PermissionName = item.name ?? string.Empty, WorkflowId = request.WorkflowId, IsGranted = item.canView ?? false });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming permissions");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming permissions");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetPermissionsByCompoStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing permissions by component parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetPermissionsByCompoStream(GetPermissionsByCompoRequest request, IServerStreamWriter<PermissionByCompoItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming permissions by component");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPermissionsByCompoAsync((byte?)request.WorkflowId, request.Compo, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new PermissionByCompoItem { PermissionId = item.groupId, PermissionName = item.name ?? string.Empty, Component = request.Compo, IsGranted = item.canView ?? false });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming permissions by component");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming permissions by component");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetReturnReasonsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing return reasons parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetReturnReasonsStream(GetReturnReasonsRequest request, IServerStreamWriter<ReturnReasonItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming return reasons");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetReturnReasonsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new ReturnReasonItem { ReasonId = item.Id, ReasonText = item.Description ?? string.Empty, WorkflowId = request.WorkflowId });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming return reasons");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming return reasons");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetRwoaReasonsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing RWOA reasons parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetRwoaReasonsStream(GetRwoaReasonsRequest request, IServerStreamWriter<RwoaReasonItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming RWOA reasons");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetRwoaReasonsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new RwoaReasonItem { ReasonId = item.Id, ReasonText = item.Description ?? string.Empty, WorkflowId = request.WorkflowId });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming RWOA reasons");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming RWOA reasons");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByCompoStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status codes by component parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodesByCompoStream(GetStatusCodesByCompoRequest request, IServerStreamWriter<StatusCodeByCompoItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming status codes by component");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByCompoAsync(request.Compo, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new StatusCodeByCompoItem { StatusId = item.statusId, StatusName = item.description ?? string.Empty, Component = item.compo });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming status codes by component");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming status codes by component");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByCompoAndModuleStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status codes by component and module parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodesByCompoAndModuleStream(GetStatusCodesByCompoAndModuleRequest request, IServerStreamWriter<StatusCodeByCompoAndModuleItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming status codes by component and module");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByCompoAndModuleAsync(request.Compo, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new StatusCodeByCompoAndModuleItem { StatusId = item.statusId, StatusName = item.description ?? string.Empty, Component = item.compo, ModuleId = item.moduleId });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming status codes by component and module");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming status codes by component and module");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesBySignCodeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status codes by sign code parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodesBySignCodeStream(GetStatusCodesBySignCodeRequest request, IServerStreamWriter<StatusCodeBySignCodeItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming status codes by sign code");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesBySignCodeAsync((short?)request.GroupId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new StatusCodeBySignCodeItem { StatusId = item.statusId, StatusName = item.description ?? string.Empty, GroupId = request.GroupId, ModuleId = request.Module });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming status codes by sign code");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming status codes by sign code");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByWorkflowStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status codes by workflow parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodesByWorkflowStream(GetStatusCodesByWorkflowRequest request, IServerStreamWriter<StatusCodeByWorkflowItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming status codes by workflow");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByWorkflowAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new StatusCodeByWorkflowItem { StatusId = 0, StatusName = "Status", WorkflowId = 0 });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming status codes by workflow");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming status codes by workflow");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByWorkflowAndAccessScopeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status codes by workflow and access scope parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodesByWorkflowAndAccessScopeStream(GetStatusCodesByWorkflowAndAccessScopeRequest request, IServerStreamWriter<StatusCodeByWorkflowAndAccessScopeItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming status codes by workflow and access scope");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByWorkflowAndAccessScopeAsync((byte?)request.WorkflowId, (byte?)request.AccessScope, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new StatusCodeByWorkflowAndAccessScopeItem { StatusId = 0, StatusName = "Status", WorkflowId = 0, AccessScope = 0 });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming status codes by workflow and access scope");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming status codes by workflow and access scope");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStatusCodeScopeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status code scope parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodeScopeStream(GetStatusCodeScopeRequest request, IServerStreamWriter<StatusCodeScopeItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming status code scope");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodeScopeAsync((byte?)request.StatusId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new StatusCodeScopeItem { StatusId = request.StatusId, ScopeName = item.accessScope.ToString(), Description = "Access Scope" });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming status code scope");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming status code scope");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStepsByWorkflowStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing steps by workflow parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStepsByWorkflowStream(GetStepsByWorkflowRequest request, IServerStreamWriter<StepByWorkflowItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming steps by workflow");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStepsByWorkflowAsync((byte?)request.Workflow, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new StepByWorkflowItem { StepId = 0, StepName = "Step", WorkflowId = 0, StepOrder = 0 });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming steps by workflow");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming steps by workflow");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetStepsByWorkflowAndStatusStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing steps by workflow and status parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStepsByWorkflowAndStatusStream(GetStepsByWorkflowAndStatusRequest request, IServerStreamWriter<StepByWorkflowAndStatusItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming steps by workflow and status");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStepsByWorkflowAndStatusAsync((byte?)request.Workflow, (byte?)request.Status, request.DeathStatus, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new StepByWorkflowAndStatusItem { StepId = 0, StepName = "Step", WorkflowId = 0, StatusId = 0, DeathStatus = "Status" });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming steps by workflow and status");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming steps by workflow and status");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetViewableByGroupStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing viewable by group parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetViewableByGroupStream(GetViewableByGroupRequest request, IServerStreamWriter<ViewableByGroupItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming viewable by group");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetViewableByGroupAsync((byte?)request.GroupId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new ViewableByGroupItem { WorkflowId = item.workFlowId, WorkflowName = item.title ?? string.Empty, GroupId = request.GroupId, IsViewable = item.active });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming viewable by group");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming viewable by group");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowByCompoStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow by component parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowByCompoStream(GetWorkflowByCompoRequest request, IServerStreamWriter<WorkflowByCompoItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workflow by component");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowByCompoAsync(request.Compo, request.UserId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkflowByCompoItem { WorkflowId = item.workflowId, WorkflowName = item.title, Component = request.Compo, UserId = request.UserId });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming workflow by component");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workflow by component");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowFromModuleStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow from module parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowFromModuleStream(GetWorkflowFromModuleRequest request, IServerStreamWriter<WorkflowFromModuleItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workflow from module");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowFromModuleAsync(request.ModuleId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkflowFromModuleItem { WorkflowId = item.workflowId ?? 0, WorkflowName = "Workflow", ModuleId = request.ModuleId });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming workflow from module");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workflow from module");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowInitialStatusCodeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow initial status code parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowInitialStatusCodeStream(GetWorkflowInitialStatusCodeRequest request, IServerStreamWriter<WorkflowInitialStatusCodeItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workflow initial status code");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowInitialStatusCodeAsync(request.Compo, request.Module, request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkflowInitialStatusCodeItem { WorkflowId = request.WorkflowId, InitialStatusId = item.statusId ?? 0, StatusName = "Status" });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming workflow initial status code");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workflow initial status code");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowTitleStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow title parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowTitleStream(GetWorkflowTitleRequest request, IServerStreamWriter<WorkflowTitleItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workflow title");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTitleAsync(request.ModuleId, request.SubCase, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkflowTitleItem { WorkflowId = 0, Title = "Title", ModuleId = 0, SubCase = 0 });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming workflow title");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workflow title");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWorkflowTitleByWorkStatusIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow title by work status ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowTitleByWorkStatusIdStream(GetWorkflowTitleByWorkStatusIdRequest request, IServerStreamWriter<WorkflowTitleByWorkStatusIdItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming workflow title by work status ID");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTitleByWorkStatusIdAsync(request.WorkflowId, request.SubCase, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new WorkflowTitleByWorkStatusIdItem { WorkflowId = 0, Title = "Title", WorkStatusId = 0, SubCase = 0 });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming workflow title by work status ID");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming workflow title by work status ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the InsertActionStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing action insertion parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task InsertActionStream(InsertActionRequest request, IServerStreamWriter<InsertActionItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming action insertion");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertActionAsync(request, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new InsertActionItem { ActionId = 0, Type = 0, StepId = 0, ResultMessage = "Success" });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming action insertion");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming action insertion");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the InsertOptionActionStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing option action insertion parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task InsertOptionActionStream(InsertOptionActionRequest request, IServerStreamWriter<InsertOptionActionItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming option action insertion");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertOptionActionAsync(request, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    await responseStream.WriteAsync(new InsertOptionActionItem { ActionId = 0, Type = 0, Wsoid = 0, ResultMessage = "Success" });
                }
            }
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled while streaming option action insertion");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while streaming option action insertion");
            throw CreateInternalErrorException();
        }
    }


    #endregion
}
