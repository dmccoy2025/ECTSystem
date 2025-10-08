#nullable enable
#pragma warning disable CS8604 // Possible null reference argument
using Microsoft.Data.SqlClient;
using AF.ECT.Server.Models.Extensions;
using AF.ECT.Server.Models.ResultTypes;
using AF.ECT.Server.Models.Interfaces;

namespace AF.ECT.Server.Models;

public partial class ALODContextProcedures : IALODContextProcedures
{
    #region Appeal and APSA Operations

    /// <summary>
    /// Retrieves appeal post completion data by appeal ID.
    /// </summary>
    /// <param name="appealId">The ID of the appeal.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of appeal post completion results.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[core_appeal_sp_GetAppealPostCompletion].</remarks>
    public async virtual Task<List<core_appeal_sp_GetAppealPostCompletionResult>> core_appeal_sp_GetAppealPostCompletionAsync(int? appealId, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = System.Data.ParameterDirection.Output,
            SqlDbType = System.Data.SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
                new SqlParameter
                {
                    ParameterName = "appealId",
                    Value = appealId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<core_appeal_sp_GetAppealPostCompletionResult>("EXEC @returnValue = [dbo].[core_appeal_sp_GetAppealPostCompletion] @appealId = @appealId", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Retrieves APSA post completion data by appeal ID.
    /// </summary>
    /// <param name="appealId">The ID of the appeal.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of APSA post completion results.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[core_APSA_sp_GetPostCompletion].</remarks>
    public async virtual Task<List<core_APSA_sp_GetPostCompletionResult>> core_APSA_sp_GetPostCompletionAsync(int? appealId, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = System.Data.ParameterDirection.Output,
            SqlDbType = System.Data.SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
                new SqlParameter
                {
                    ParameterName = "appealId",
                    Value = appealId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<core_APSA_sp_GetPostCompletionResult>("EXEC @returnValue = [dbo].[core_APSA_sp_GetPostCompletion] @appealId = @appealId", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    #endregion
}
