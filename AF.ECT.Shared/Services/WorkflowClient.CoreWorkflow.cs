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
    #region Core Workflow Methods

/// <summary>
    /// Adds a signature to a workflow.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="moduleType">The module type.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="actionId">The action ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <param name="statusIn">The status in.</param>
    /// <param name="statusOut">The status out.</param>
    /// <returns>A task representing the asynchronous operation, containing the signature addition response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<AddSignatureResponse> AddSignatureAsync(int refId, int moduleType, int userId, int actionId, int groupId, int statusIn, int statusOut)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.AddSignatureAsync(new AddSignatureRequest
                {
                    RefId = refId,
                    ModuleType = moduleType,
                    UserId = userId,
                    ActionId = actionId,
                    GroupId = groupId,
                    StatusIn = statusIn,
                    StatusOut = statusOut
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(AddSignatureAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { RefId = refId, ModuleType = moduleType, UserId = userId, ActionId = actionId, GroupId = groupId, StatusIn = statusIn, StatusOut = statusOut });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(AddSignatureAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { RefId = refId, ModuleType = moduleType, UserId = userId, ActionId = actionId, GroupId = groupId, StatusIn = statusIn, StatusOut = statusOut });
            throw;
        }
    }

    /// <summary>
    /// Copies actions from one workflow to another.
    /// </summary>
    /// <param name="destWsoid">The destination WSOID.</param>
    /// <param name="srcWsoid">The source WSOID.</param>
    /// <returns>A task representing the asynchronous operation, containing the action copy response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<CopyActionsResponse> CopyActionsAsync(int destWsoid, int srcWsoid)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.CopyActionsAsync(new CopyActionsRequest
                {
                    DestWsoid = destWsoid,
                    SrcWsoid = srcWsoid
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyActionsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { DestWsoid = destWsoid, SrcWsoid = srcWsoid });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyActionsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { DestWsoid = destWsoid, SrcWsoid = srcWsoid });
            throw;
        }
    }

    /// <summary>
    /// Copies rules from one workflow to another.
    /// </summary>
    /// <param name="destWsoid">The destination WSOID.</param>
    /// <param name="srcWsoid">The source WSOID.</param>
    /// <returns>A task representing the asynchronous operation, containing the rule copy response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<CopyRulesResponse> CopyRulesAsync(int destWsoid, int srcWsoid)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.CopyRulesAsync(new CopyRulesRequest
                {
                    DestWsoid = destWsoid,
                    SrcWsoid = srcWsoid
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyRulesAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { DestWsoid = destWsoid, SrcWsoid = srcWsoid });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyRulesAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { DestWsoid = destWsoid, SrcWsoid = srcWsoid });
            throw;
        }
    }

    /// <summary>
    /// Copies a workflow from one ID to another.
    /// </summary>
    /// <param name="fromId">The source workflow ID.</param>
    /// <param name="toId">The destination workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow copy response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<CopyWorkflowResponse> CopyWorkflowAsync(int fromId, int toId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.CopyWorkflowAsync(new CopyWorkflowRequest
                {
                    FromId = fromId,
                    ToId = toId
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { FromId = fromId, ToId = toId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { FromId = fromId, ToId = toId });
            throw;
        }
    }

    /// <summary>
    /// Deletes a status code.
    /// </summary>
    /// <param name="statusId">The status ID to delete.</param>
    /// <returns>A task representing the asynchronous operation, containing the status code deletion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<DeleteStatusCodeResponse> DeleteStatusCodeAsync(int statusId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.DeleteStatusCodeAsync(new DeleteStatusCodeRequest { StatusId = statusId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(DeleteStatusCodeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { StatusId = statusId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(DeleteStatusCodeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { StatusId = statusId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves actions by step.
    /// </summary>
    /// <param name="stepId">The step ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the actions by step response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetActionsByStepResponse> GetActionsByStepAsync(int stepId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetActionsByStepAsync(new GetActionsByStepRequest { StepId = stepId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetActionsByStepAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { StepId = stepId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetActionsByStepAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { StepId = stepId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves active cases with specified parameters.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the active cases response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetActiveCasesResponse> GetActiveCasesAsync(int refId, int groupId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetActiveCasesAsync(new GetActiveCasesRequest
                {
                    RefId = refId,
                    GroupId = groupId
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetActiveCasesAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { RefId = refId, GroupId = groupId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetActiveCasesAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { RefId = refId, GroupId = groupId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves all findings by reason of.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the all findings by reason of response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetAllFindingByReasonOfResponse> GetAllFindingByReasonOfAsync()
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetAllFindingByReasonOfAsync(new EmptyRequest()).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllFindingByReasonOfAsync), correlationId, startTime, _stopwatch.Elapsed, true);

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllFindingByReasonOfAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all locks.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the all locks response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetAllLocksResponse> GetAllLocksAsync()
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetAllLocksAsync(new EmptyRequest()).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllLocksAsync), correlationId, startTime, _stopwatch.Elapsed, true);

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllLocksAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves cancel reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="isFormal">Whether it's formal.</param>
    /// <returns>A task representing the asynchronous operation, containing the cancel reasons response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetCancelReasonsResponse> GetCancelReasonsAsync(int workflowId, bool isFormal)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetCancelReasonsAsync(new GetCancelReasonsRequest
                {
                    WorkflowId = workflowId,
                    IsFormal = isFormal
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetCancelReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId, IsFormal = isFormal });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetCancelReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId, IsFormal = isFormal });
            throw;
        }
    }

    /// <summary>
    /// Retrieves creatable workflows by group.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the creatable by group response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetCreatableByGroupResponse> GetCreatableByGroupAsync(string compo, int module, int groupId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetCreatableByGroupAsync(new GetCreatableByGroupRequest
                {
                    Compo = compo,
                    Module = module,
                    GroupId = groupId
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetCreatableByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo, Module = module, GroupId = groupId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetCreatableByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo, Module = module, GroupId = groupId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves finding by reason of by ID.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the finding by reason of by ID response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetFindingByReasonOfByIdResponse> GetFindingByReasonOfByIdAsync(int id)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetFindingByReasonOfByIdAsync(new GetFindingByReasonOfByIdRequest { Id = id }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetFindingByReasonOfByIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Id = id });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetFindingByReasonOfByIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Id = id });
            throw;
        }
    }

    /// <summary>
    /// Retrieves findings for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the findings response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetFindingsResponse> GetFindingsAsync(int workflowId, int groupId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetFindingsAsync(new GetFindingsRequest
                {
                    WorkflowId = workflowId,
                    GroupId = groupId
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetFindingsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId, GroupId = groupId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetFindingsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId, GroupId = groupId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves module from workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the module from workflow response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetModuleFromWorkflowResponse> GetModuleFromWorkflowAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetModuleFromWorkflowAsync(new GetModuleFromWorkflowRequest { WorkflowId = workflowId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetModuleFromWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetModuleFromWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves page access by group.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="group">The group.</param>
    /// <returns>A task representing the asynchronous operation, containing the page access by group response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetPageAccessByGroupResponse> GetPageAccessByGroupAsync(int workflow, int status, int group)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetPageAccessByGroupAsync(new GetPageAccessByGroupRequest
                {
                    Workflow = workflow,
                    Status = status,
                    Group = group
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPageAccessByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Workflow = workflow, Status = status, Group = group });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPageAccessByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Workflow = workflow, Status = status, Group = group });
            throw;
        }
    }

    /// <summary>
    /// Retrieves page access by workflow view.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <returns>A task representing the asynchronous operation, containing the page access by workflow view response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetPageAccessByWorkflowViewResponse> GetPageAccessByWorkflowViewAsync(string compo, int workflow, int status)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetPageAccessByWorkflowViewAsync(new GetPageAccessByWorkflowViewRequest
                {
                    Compo = compo,
                    Workflow = workflow,
                    Status = status
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPageAccessByWorkflowViewAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo, Workflow = workflow, Status = status });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPageAccessByWorkflowViewAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo, Workflow = workflow, Status = status });
            throw;
        }
    }

    /// <summary>
    /// Retrieves pages by workflow ID.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the pages by workflow ID response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetPagesByWorkflowIdResponse> GetPagesByWorkflowIdAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetPagesByWorkflowIdAsync(new GetPagesByWorkflowIdRequest { WorkflowId = workflowId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPagesByWorkflowIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPagesByWorkflowIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves permissions for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the permissions response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetPermissionsResponse> GetPermissionsAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetPermissionsAsync(new GetPermissionsRequest { WorkflowId = workflowId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPermissionsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPermissionsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves permissions by component.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="compo">The component.</param>
    /// <returns>A task representing the asynchronous operation, containing the permissions by component response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetPermissionsByCompoResponse> GetPermissionsByCompoAsync(int workflowId, string compo)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetPermissionsByCompoAsync(new GetPermissionsByCompoRequest
                {
                    WorkflowId = workflowId,
                    Compo = compo
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPermissionsByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId, Compo = compo });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPermissionsByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId, Compo = compo });
            throw;
        }
    }

    /// <summary>
    /// Retrieves return reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the return reasons response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetReturnReasonsResponse> GetReturnReasonsAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetReturnReasonsAsync(new GetReturnReasonsRequest { WorkflowId = workflowId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetReturnReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetReturnReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves RWOA reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the RWOA reasons response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetRwoaReasonsResponse> GetRwoaReasonsAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetRwoaReasonsAsync(new GetRwoaReasonsRequest { WorkflowId = workflowId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetRwoaReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetRwoaReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status codes by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by component response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodesByCompoResponse> GetStatusCodesByCompoAsync(string compo)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetStatusCodesByCompoAsync(new GetStatusCodesByCompoRequest { Compo = compo }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status codes by component and module.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by component and module response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodesByCompoAndModuleResponse> GetStatusCodesByCompoAndModuleAsync(string compo, int module)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetStatusCodesByCompoAndModuleAsync(new GetStatusCodesByCompoAndModuleRequest
                {
                    Compo = compo,
                    Module = module
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByCompoAndModuleAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo, Module = module });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByCompoAndModuleAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo, Module = module });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status codes by sign code.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by sign code response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodesBySignCodeResponse> GetStatusCodesBySignCodeAsync(int groupId, int module)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetStatusCodesBySignCodeAsync(new GetStatusCodesBySignCodeRequest
                {
                    GroupId = groupId,
                    Module = module
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesBySignCodeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { GroupId = groupId, Module = module });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesBySignCodeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { GroupId = groupId, Module = module });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status codes by workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by workflow response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodesByWorkflowResponse> GetStatusCodesByWorkflowAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetStatusCodesByWorkflowAsync(new GetStatusCodesByWorkflowRequest { WorkflowId = workflowId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status codes by workflow and access scope.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="accessScope">The access scope.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by workflow and access scope response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodesByWorkflowAndAccessScopeResponse> GetStatusCodesByWorkflowAndAccessScopeAsync(int workflowId, int accessScope)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetStatusCodesByWorkflowAndAccessScopeAsync(new GetStatusCodesByWorkflowAndAccessScopeRequest
                {
                    WorkflowId = workflowId,
                    AccessScope = accessScope
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByWorkflowAndAccessScopeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId, AccessScope = accessScope });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByWorkflowAndAccessScopeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId, AccessScope = accessScope });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status code scope.
    /// </summary>
    /// <param name="statusId">The status ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the status code scope response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodeScopeResponse> GetStatusCodeScopeAsync(int statusId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetStatusCodeScopeAsync(new GetStatusCodeScopeRequest { StatusId = statusId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodeScopeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { StatusId = statusId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodeScopeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { StatusId = statusId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves steps by workflow.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <returns>A task representing the asynchronous operation, containing the steps by workflow response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStepsByWorkflowResponse> GetStepsByWorkflowAsync(int workflow)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetStepsByWorkflowAsync(new GetStepsByWorkflowRequest { Workflow = workflow }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStepsByWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Workflow = workflow });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStepsByWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Workflow = workflow });
            throw;
        }
    }

    /// <summary>
    /// Retrieves steps by workflow and status.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="deathStatus">The death status.</param>
    /// <returns>A task representing the asynchronous operation, containing the steps by workflow and status response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStepsByWorkflowAndStatusResponse> GetStepsByWorkflowAndStatusAsync(int workflow, int status, string deathStatus)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetStepsByWorkflowAndStatusAsync(new GetStepsByWorkflowAndStatusRequest
                {
                    Workflow = workflow,
                    Status = status,
                    DeathStatus = deathStatus
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStepsByWorkflowAndStatusAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Workflow = workflow, Status = status, DeathStatus = deathStatus });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStepsByWorkflowAndStatusAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Workflow = workflow, Status = status, DeathStatus = deathStatus });
            throw;
        }
    }

    /// <summary>
    /// Retrieves viewable workflows by group.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the viewable by group response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetViewableByGroupResponse> GetViewableByGroupAsync(int groupId, int module)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetViewableByGroupAsync(new GetViewableByGroupRequest
                {
                    GroupId = groupId,
                    Module = module
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetViewableByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { GroupId = groupId, Module = module });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetViewableByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { GroupId = groupId, Module = module });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflow by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow by component response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowByCompoResponse> GetWorkflowByCompoAsync(string compo, int userId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkflowByCompoAsync(new GetWorkflowByCompoRequest
                {
                    Compo = compo,
                    UserId = userId
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo, UserId = userId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo, UserId = userId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflow from module.
    /// </summary>
    /// <param name="moduleId">The module ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow from module response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowFromModuleResponse> GetWorkflowFromModuleAsync(int moduleId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkflowFromModuleAsync(new GetWorkflowFromModuleRequest { ModuleId = moduleId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowFromModuleAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { ModuleId = moduleId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowFromModuleAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { ModuleId = moduleId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflow initial status code.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow initial status code response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowInitialStatusCodeResponse> GetWorkflowInitialStatusCodeAsync(int compo, int module, int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkflowInitialStatusCodeAsync(new GetWorkflowInitialStatusCodeRequest
                {
                    Compo = compo,
                    Module = module,
                    WorkflowId = workflowId
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowInitialStatusCodeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo, Module = module, WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowInitialStatusCodeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo, Module = module, WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflow title.
    /// </summary>
    /// <param name="moduleId">The module ID.</param>
    /// <param name="subCase">The sub case.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow title response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowTitleResponse> GetWorkflowTitleAsync(int moduleId, int subCase)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkflowTitleAsync(new GetWorkflowTitleRequest
                {
                    ModuleId = moduleId,
                    SubCase = subCase
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowTitleAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { ModuleId = moduleId, SubCase = subCase });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowTitleAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { ModuleId = moduleId, SubCase = subCase });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflow title by work status ID.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="subCase">The sub case.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow title by work status ID response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowTitleByWorkStatusIdResponse> GetWorkflowTitleByWorkStatusIdAsync(int workflowId, int subCase)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkflowTitleByWorkStatusIdAsync(new GetWorkflowTitleByWorkStatusIdRequest
                {
                    WorkflowId = workflowId,
                    SubCase = subCase
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowTitleByWorkStatusIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId, SubCase = subCase });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowTitleByWorkStatusIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId, SubCase = subCase });
            throw;
        }
    }

    /// <summary>
    /// Inserts an action.
    /// </summary>
    /// <param name="type">The action type.</param>
    /// <param name="stepId">The step ID.</param>
    /// <param name="target">The target.</param>
    /// <param name="data">The data.</param>
    /// <returns>A task representing the asynchronous operation, containing the action insertion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<InsertActionResponse> InsertActionAsync(int type, int stepId, int target, int data)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.InsertActionAsync(new InsertActionRequest
                {
                    Type = type,
                    StepId = stepId,
                    Target = target,
                    Data = data
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertActionAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Type = type, StepId = stepId, Target = target, Data = data });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertActionAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Type = type, StepId = stepId, Target = target, Data = data });
            throw;
        }
    }

    /// <summary>
    /// Inserts an option action.
    /// </summary>
    /// <param name="type">The action type.</param>
    /// <param name="wsoid">The WSOID.</param>
    /// <param name="target">The target.</param>
    /// <param name="data">The data.</param>
    /// <returns>A task representing the asynchronous operation, containing the option action insertion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<InsertOptionActionResponse> InsertOptionActionAsync(int type, int wsoid, int target, int data)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.InsertOptionActionAsync(new InsertOptionActionRequest
                {
                    Type = type,
                    Wsoid = wsoid,
                    Target = target,
                    Data = data
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertOptionActionAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Type = type, Wsoid = wsoid, Target = target, Data = data });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertOptionActionAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Type = type, Wsoid = wsoid, Target = target, Data = data });
            throw;
        }
    }

    #endregion
}
