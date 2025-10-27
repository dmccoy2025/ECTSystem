using AF.ECT.Data.Models;
using AF.ECT.Data.Extensions;
using AF.ECT.Data.ResultTypes;

#nullable enable

namespace AF.ECT.Data.Services;

/// <summary>
/// Partial class containing Core Workflow Methods.
/// </summary>
public partial class DataService
{
    #region Core Workflow Methods


    /// <summary>
    /// Asynchronously adds a signature to a workflow.
    /// </summary>
    /// <param name="request">The add signature request containing signature details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of signature results.</returns>
    public async Task<List<core_workflow_sp_AddSignatureResult>> AddSignatureAsync(AddSignatureRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding signature for refId {RefId}, userId {UserId}", request.RefId, request.UserId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_AddSignatureAsync(request.RefId, request.ModuleType, request.UserId, request.ActionId, (byte?)request.GroupId, (byte?)request.StatusIn, (byte?)request.StatusOut, cancellationToken: cancellationToken);
            _logger.LogInformation("Signature added successfully, {Count} results", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding signature for refId {RefId}", request.RefId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously copies actions from one workflow step to another.
    /// </summary>
    /// <param name="destWsoid">The destination workflow step object identifier.</param>
    /// <param name="srcWsoid">The source workflow step object identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> CopyActionsAsync(int? destWsoid, int? srcWsoid, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Copying actions from srcWsoid {SrcWsoid} to destWsoid {DestWsoid}", srcWsoid, destWsoid);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_CopyActionsAsync(destWsoid, srcWsoid, cancellationToken: cancellationToken);
            _logger.LogInformation("Actions copied successfully, result {Result}", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error copying actions from srcWsoid {SrcWsoid}", srcWsoid);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously copies rules from one workflow step to another.
    /// </summary>
    /// <param name="destWsoid">The destination workflow step object identifier.</param>
    /// <param name="srcWsoid">The source workflow step object identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> CopyRulesAsync(int? destWsoid, int? srcWsoid, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Copying rules from srcWsoid {SrcWsoid} to destWsoid {DestWsoid}", srcWsoid, destWsoid);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_CopyRulesAsync(destWsoid, srcWsoid, cancellationToken: cancellationToken);
            _logger.LogInformation("Rules copied successfully, result {Result}", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error copying rules from srcWsoid {SrcWsoid}", srcWsoid);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously copies a workflow from one ID to another.
    /// </summary>
    /// <param name="fromId">The source workflow identifier.</param>
    /// <param name="toId">The destination workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow copy results.</returns>
    public async Task<List<core_workflow_sp_CopyWorkflowResult>> CopyWorkflowAsync(int? fromId, int? toId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Copying workflow from {FromId} to {ToId}", fromId, toId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_CopyWorkflowAsync(fromId, toId, cancellationToken: cancellationToken);
            _logger.LogInformation("Workflow copied successfully, {Count} results", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error copying workflow from {FromId}", fromId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously deletes a status code.
    /// </summary>
    /// <param name="statusId">The status identifier to delete.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> DeleteStatusCodeAsync(int? statusId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting status code {StatusId}", statusId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_DeleteStatusCodeAsync(statusId, cancellationToken: cancellationToken);
            _logger.LogInformation("Status code {StatusId} deleted successfully", statusId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting status code {StatusId}", statusId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves actions by step identifier.
    /// </summary>
    /// <param name="stepId">The step identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of actions by step.</returns>
    public async Task<List<core_workflow_sp_GetActionsByStepResult>> GetActionsByStepAsync(int? stepId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving actions by step for stepId {StepId}", stepId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetActionsByStepAsync(stepId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} actions by step", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actions by step for stepId {StepId}", stepId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves active cases for a reference and group.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of active cases.</returns>
    public async Task<List<core_workflow_sp_GetActiveCasesResult>> GetActiveCasesAsync(int? refId, short? groupId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving active cases for refId {RefId}, groupId {GroupId}", refId, groupId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetActiveCasesAsync(refId, groupId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} active cases", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active cases for refId {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves all findings by reason of.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of all findings by reason of.</returns>
    public async Task<List<core_workflow_sp_GetAllFindingByReasonOfResult>> GetAllFindingByReasonOfAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all findings by reason of");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetAllFindingByReasonOfAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} all findings by reason of", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all findings by reason of");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves all locks.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of all locks.</returns>
    public async Task<List<core_workflow_sp_GetAllLocksResult>> GetAllLocksAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all locks");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetAllLocksAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} all locks", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all locks");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves cancel reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="isFormal">Whether the workflow is formal.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of cancel reasons.</returns>
    public async Task<List<core_workflow_sp_GetCancelReasonsResult>> GetCancelReasonsAsync(byte? workflowId, bool? isFormal, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving cancel reasons for workflowId {WorkflowId}, isFormal {IsFormal}", workflowId, isFormal);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetCancelReasonsAsync(workflowId, isFormal, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} cancel reasons", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cancel reasons for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves creatable workflows by group.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of creatable workflows by group.</returns>
    public async Task<List<core_workflow_sp_GetCreatableByGroupResult>> GetCreatableByGroupAsync(string? compo, byte? module, byte? groupId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving creatable by group for compo {Compo}, module {Module}, groupId {GroupId}", compo, module, groupId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetCreatableByGroupAsync(compo, module, groupId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} creatable by group", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving creatable by group for compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves finding by reason of by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of findings by reason of.</returns>
    public async Task<List<core_workflow_sp_GetFindingByReasonOfByIdResult>> GetFindingByReasonOfByIdAsync(int? id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving finding by reason of by id {Id}", id);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetFindingByReasonOfByIdAsync(id, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} findings by reason of by id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving finding by reason of by id {Id}", id);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves findings for a workflow and group.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of findings.</returns>
    public async Task<List<core_workflow_sp_GetFindingsResult>> GetFindingsAsync(byte? workflowId, int? groupId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving findings for workflowId {WorkflowId}, groupId {GroupId}", workflowId, groupId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetFindingsAsync(workflowId, groupId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} findings", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving findings for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves module from workflow identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of modules from workflow.</returns>
    public async Task<List<core_workflow_sp_GetModuleFromWorkflowResult>> GetModuleFromWorkflowAsync(int? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving module from workflow for workflowId {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetModuleFromWorkflowAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} modules from workflow", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving module from workflow for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves page access by group.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="group">The group.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of page access by group.</returns>
    public async Task<List<core_workflow_sp_GetPageAccessByGroupResult>> GetPageAccessByGroupAsync(byte? workflow, int? status, byte? group, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving page access by group for workflow {Workflow}, status {Status}, group {Group}", workflow, status, group);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetPageAccessByGroupAsync(workflow, status, group, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} page access by group", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving page access by group for workflow {Workflow}", workflow);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves page access by workflow view.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of page access by workflow view.</returns>
    public async Task<List<core_workflow_sp_GetPageAccessByWorkflowViewResult>> GetPageAccessByWorkflowViewAsync(string? compo, byte? workflow, int? status, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving page access by workflow view for compo {Compo}, workflow {Workflow}, status {Status}", compo, workflow, status);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetPageAccessByWorkflowViewAsync(compo, workflow, status, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} page access by workflow view", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving page access by workflow view for compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves pages by workflow identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of pages by workflow.</returns>
    public async Task<List<core_workflow_sp_GetPagesByWorkflowIdResult>> GetPagesByWorkflowIdAsync(int? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving pages by workflow id {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetPagesByWorkflowIdAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} pages by workflow id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pages by workflow id {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves permissions for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of permissions.</returns>
    public async Task<List<core_Workflow_sp_GetPermissionsResult>> GetPermissionsAsync(byte? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving permissions for workflowId {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_Workflow_sp_GetPermissionsAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} permissions", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves permissions by component for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="compo">The component.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of permissions by component.</returns>
    public async Task<List<core_Workflow_sp_GetPermissionsByCompoResult>> GetPermissionsByCompoAsync(byte? workflowId, string? compo, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving permissions by compo for workflowId {WorkflowId}, compo {Compo}", workflowId, compo);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_Workflow_sp_GetPermissionsByCompoAsync(workflowId, compo, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} permissions by compo", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions by compo for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves return reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of return reasons.</returns>
    public async Task<List<core_workflow_sp_GetReturnReasonsResult>> GetReturnReasonsAsync(byte? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving return reasons for workflowId {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetReturnReasonsAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} return reasons", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving return reasons for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves RWOA reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of RWOA reasons.</returns>
    public async Task<List<core_workflow_sp_GetRwoaReasonsResult>> GetRwoaReasonsAsync(byte? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving RWOA reasons for workflowId {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetRwoaReasonsAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} RWOA reasons", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving RWOA reasons for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves status codes by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by component.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodesByCompoResult>> GetStatusCodesByCompoAsync(string? compo, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving status codes by compo {Compo}", compo);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStatusCodesByCompoAsync(compo, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} status codes by compo", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving status codes by compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves status codes by component and module.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by component and module.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodesByCompoAndModuleResult>> GetStatusCodesByCompoAndModuleAsync(string? compo, byte? module, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving status codes by compo and module for compo {Compo}, module {Module}", compo, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStatusCodesByCompoAndModuleAsync(compo, module, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} status codes by compo and module", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving status codes by compo and module for compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves status codes by sign code.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by sign code.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodesBySignCodeResult>> GetStatusCodesBySignCodeAsync(short? groupId, byte? module, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving status codes by sign code for groupId {GroupId}, module {Module}", groupId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStatusCodesBySignCodeAsync(groupId, module, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} status codes by sign code", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving status codes by sign code for groupId {GroupId}", groupId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves status codes by workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by workflow.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodesByWorkflowResult>> GetStatusCodesByWorkflowAsync(byte? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving status codes by workflow for workflowId {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStatusCodesByWorkflowAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} status codes by workflow", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving status codes by workflow for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves status codes by workflow and access scope.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="accessScope">The access scope.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by workflow and access scope.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodesByWorkflowAndAccessScopeResult>> GetStatusCodesByWorkflowAndAccessScopeAsync(byte? workflowId, byte? accessScope, CancellationToken cancellationToken = default)
    {
        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Procedures.core_workflow_sp_GetStatusCodesByWorkflowAndAccessScopeAsync(workflowId, accessScope, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves status code scope.
    /// </summary>
    /// <param name="statusID">The status identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status code scopes.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodeScopeResult>> GetStatusCodeScopeAsync(byte? statusID, CancellationToken cancellationToken = default)
    {
        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Procedures.core_workflow_sp_GetStatusCodeScopeAsync(statusID, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves steps by workflow.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of steps by workflow.</returns>
    public async Task<List<core_workflow_sp_GetStepsByWorkflowResult>> GetStepsByWorkflowAsync(byte? workflow, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving steps by workflow for workflow {Workflow}", workflow);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStepsByWorkflowAsync(workflow, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} steps by workflow", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving steps by workflow for workflow {Workflow}", workflow);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves steps by workflow and status.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="deathStatus">The death status.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of steps by workflow and status.</returns>
    public async Task<List<core_workflow_sp_GetStepsByWorkflowAndStatusResult>> GetStepsByWorkflowAndStatusAsync(byte? workflow, byte? status, string? deathStatus, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving steps by workflow and status for workflow {Workflow}, status {Status}, deathStatus {DeathStatus}", workflow, status, deathStatus);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStepsByWorkflowAndStatusAsync(workflow, status, deathStatus, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} steps by workflow and status", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving steps by workflow and status for workflow {Workflow}", workflow);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves viewable workflows by group.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of viewable workflows by group.</returns>
    public async Task<List<core_workflow_sp_GetViewableByGroupResult>> GetViewableByGroupAsync(byte? groupId, byte? module, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving viewable by group for groupId {GroupId}, module {Module}", groupId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetViewableByGroupAsync(groupId, module, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} viewable by group", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving viewable by group for groupId {GroupId}", groupId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows by component.</returns>
    public async Task<List<core_workflow_sp_GetWorkflowByCompoResult>> GetWorkflowByCompoAsync(string? compo, int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow by compo for compo {Compo}, userId {UserId}", compo, userId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetWorkflowByCompoAsync(compo, userId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow by compo", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow by compo for compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow from module.
    /// </summary>
    /// <param name="moduleId">The module identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows from module.</returns>
    public async Task<List<core_workflow_sp_GetWorkflowFromModuleResult>> GetWorkflowFromModuleAsync(int? moduleId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow from module for moduleId {ModuleId}", moduleId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetWorkflowFromModuleAsync(moduleId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow from module", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow from module for moduleId {ModuleId}", moduleId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow initial status code.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow initial status codes.</returns>
    public async Task<List<core_workflow_sp_GetWorkflowInitialStatusCodeResult>> GetWorkflowInitialStatusCodeAsync(int? compo, int? module, int? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow initial status code for compo {Compo}, module {Module}, workflowId {WorkflowId}", compo, module, workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetWorkflowInitialStatusCodeAsync(compo, module, workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow initial status code", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow initial status code for compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow title.
    /// </summary>
    /// <param name="moduleId">The module identifier.</param>
    /// <param name="subCase">The sub case.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow titles.</returns>
    public async virtual Task<List<core_workflow_sp_GetWorkflowTitleResult>> GetWorkflowTitleAsync(int? moduleId, int? subCase, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow title for moduleId {ModuleId}, subCase {SubCase}", moduleId, subCase);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetWorkflowTitleAsync(moduleId, subCase, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow title", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow title for moduleId {ModuleId}", moduleId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow title by work status identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="subCase">The sub case.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow titles by work status.</returns>
    public async Task<List<core_workflow_sp_GetWorkflowTitleByWorkStatusIdResult>> GetWorkflowTitleByWorkStatusIdAsync(int? workflowId, int? subCase, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow title by work status id for workflowId {WorkflowId}, subCase {SubCase}", workflowId, subCase);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetWorkflowTitleByWorkStatusIdAsync(workflowId, subCase, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow title by work status id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow title by work status id for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously inserts an action.
    /// </summary>
    /// <param name="request">The insert action request containing action details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of insert action results.</returns>
    public async Task<List<core_workflow_sp_InsertActionResult>> InsertActionAsync(InsertActionRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting action with type {Type}, stepId {StepId}", request.Type, request.StepId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_InsertActionAsync((byte?)request.Type, (short?)request.StepId, request.Target, request.Data, cancellationToken: cancellationToken);
            _logger.LogInformation("Inserted {Count} action", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting action with type {Type}", request.Type);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously inserts an option action.
    /// </summary>
    /// <param name="request">The insert option action request containing action details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of insert option action results.</returns>
    public async Task<List<core_workflow_sp_InsertOptionActionResult>> InsertOptionActionAsync(InsertOptionActionRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting option action with type {Type}, wsoid {Wsoid}", request.Type, request.Wsoid);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_InsertOptionActionAsync((byte?)request.Type, request.Wsoid, request.Target, request.Data, cancellationToken: cancellationToken);
            _logger.LogInformation("Inserted {Count} option action", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting option action with type {Type}", request.Type);
            throw;
        }
    }


    #endregion
}
