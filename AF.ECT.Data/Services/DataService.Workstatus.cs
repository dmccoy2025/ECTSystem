using AF.ECT.Data.Models;
using AF.ECT.Data.Extensions;
using AF.ECT.Data.ResultTypes;

#nullable enable

namespace AF.ECT.Data.Services;

/// <summary>
/// Partial class containing Workstatus Methods.
/// </summary>
public partial class DataService
{
    #region Workstatus Methods


    /// <summary>
    /// Asynchronously retrieves a workstatus by its identifier.
    /// </summary>
    /// <param name="workstatusId">The workstatus identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatus results.</returns>
    public async Task<List<workstatus_sp_GetWorkstatusByIdResult>> GetWorkstatusByIdAsync(int? workstatusId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workstatus by id {WorkstatusId}", workstatusId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_GetWorkstatusByIdAsync(workstatusId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workstatus by id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workstatus by id {WorkstatusId}", workstatusId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workstatuses by reference identifier.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatuses by reference.</returns>
    public async Task<List<workstatus_sp_GetWorkstatusesByRefIdResult>> GetWorkstatusesByRefIdAsync(int? refId, byte? module, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workstatuses by ref id {RefId}, module {Module}", refId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_GetWorkstatusesByRefIdAsync(refId, module, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workstatuses by ref id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workstatuses by ref id {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workstatuses by reference identifier and type.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="workstatusType">The workstatus type.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatuses by reference and type.</returns>
    public async Task<List<workstatus_sp_GetWorkstatusesByRefIdAndTypeResult>> GetWorkstatusesByRefIdAndTypeAsync(int? refId, byte? module, int? workstatusType, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workstatuses by ref id and type for refId {RefId}, module {Module}, workstatusType {WorkstatusType}", refId, module, workstatusType);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_GetWorkstatusesByRefIdAndTypeAsync(refId, module, workstatusType, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workstatuses by ref id and type", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workstatuses by ref id and type for refId {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workstatus types.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatus types.</returns>
    public async Task<List<workstatus_sp_GetWorkstatusTypesResult>> GetWorkstatusTypesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workstatus types");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_GetWorkstatusTypesAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workstatus types", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workstatus types");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously inserts a workstatus.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="workstatusType">The workstatus type.</param>
    /// <param name="workstatusText">The workstatus text.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> InsertWorkstatusAsync(int? refId, byte? module, int? workstatusType, string? workstatusText, int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting workstatus for refId {RefId}, module {Module}", refId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_InsertWorkstatusAsync(refId, module, workstatusType, workstatusText, userId, cancellationToken: cancellationToken);
            _logger.LogInformation("Workstatus inserted successfully");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting workstatus for refId {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously updates a workstatus.
    /// </summary>
    /// <param name="workstatusId">The workstatus identifier.</param>
    /// <param name="workstatusText">The workstatus text.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> UpdateWorkstatusAsync(int? workstatusId, string? workstatusText, int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating workstatus {WorkstatusId}", workstatusId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_UpdateWorkstatusAsync(workstatusId, workstatusText, userId, cancellationToken: cancellationToken);
            _logger.LogInformation("Workstatus {WorkstatusId} updated successfully", workstatusId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating workstatus {WorkstatusId}", workstatusId);
            throw;
        }
    }


    #endregion
}
