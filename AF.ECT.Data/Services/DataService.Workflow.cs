using AF.ECT.Data.Models;
using AF.ECT.Data.Extensions;
using AF.ECT.Data.ResultTypes;

#nullable enable

namespace AF.ECT.Data.Services;

/// <summary>
/// Partial class containing Workflow Methods.
/// </summary>
public partial class DataService
{
    #region Workflow Methods


    /// <summary>
    /// Asynchronously retrieves a workflow by its identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow results.</returns>
    public async Task<List<workflow_sp_GetWorkflowByIdResult>> GetWorkflowByIdAsync(int? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow by id {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_GetWorkflowByIdAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow by id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow by id {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflows by reference identifier.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows by reference.</returns>
    public async Task<List<workflow_sp_GetWorkflowsByRefIdResult>> GetWorkflowsByRefIdAsync(int? refId, byte? module, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflows by ref id {RefId}, module {Module}", refId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_GetWorkflowsByRefIdAsync(refId, module, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflows by ref id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflows by ref id {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflows by reference identifier and type.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowType">The workflow type.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows by reference and type.</returns>
    public async Task<List<workflow_sp_GetWorkflowsByRefIdAndTypeResult>> GetWorkflowsByRefIdAndTypeAsync(int? refId, byte? module, int? workflowType, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflows by ref id and type for refId {RefId}, module {Module}, workflowType {WorkflowType}", refId, module, workflowType);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_GetWorkflowsByRefIdAndTypeAsync(refId, module, workflowType, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflows by ref id and type", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflows by ref id and type for refId {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow types.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow types.</returns>
    public async Task<List<workflow_sp_GetWorkflowTypesResult>> GetWorkflowTypesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow types");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_GetWorkflowTypesAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow types", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow types");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously inserts a workflow.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowType">The workflow type.</param>
    /// <param name="workflowText">The workflow text.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> InsertWorkflowAsync(int? refId, byte? module, int? workflowType, string? workflowText, int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting workflow for refId {RefId}, module {Module}", refId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_InsertWorkflowAsync(refId, module, workflowType, workflowText, userId, cancellationToken: cancellationToken);
            _logger.LogInformation("Workflow inserted successfully");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting workflow for refId {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously updates a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="workflowText">The workflow text.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> UpdateWorkflowAsync(int? workflowId, string? workflowText, int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating workflow {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_UpdateWorkflowAsync(workflowId, workflowText, userId, cancellationToken: cancellationToken);
            _logger.LogInformation("Workflow {WorkflowId} updated successfully", workflowId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating workflow {WorkflowId}", workflowId);
            throw;
        }
    }


    #endregion
}
