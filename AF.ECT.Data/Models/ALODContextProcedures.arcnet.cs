#nullable enable
#pragma warning disable CS8604 // Possible null reference argument
using Microsoft.Data.SqlClient;
using System.Data;
using AF.ECT.Data.Extensions;
using AF.ECT.Data.ResultTypes;
using AF.ECT.Data.Interfaces;

namespace AF.ECT.Data.Models;

public partial class ALODContextProcedures : IALODContextProcedures
{
    #region Arcnet Operations

    /// <summary>
    /// Retrieves IAT training data for users based on specified criteria.
    /// </summary>
    /// <param name="ediPIN">The EDI PIN of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="middleNames">The middle names of the user.</param>
    /// <param name="beginDate">The beginning date for the training data range.</param>
    /// <param name="endDate">The ending date for the training data range.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of training data results for the specified users.</returns>
    /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
    public async virtual Task<List<arcnet_GetIAATrainingDataForUsersResult>> ArcnetGetIAATrainingDataForUsersAsync(string? ediPIN, string? lastName, string? firstName, string? middleNames, DateTime? beginDate, DateTime? endDate, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
                new SqlParameter
                {
                    ParameterName = "ediPIN",
                    Size = 100,
                    Value = ediPIN ?? Convert.DBNull,
                    SqlDbType = SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "lastName",
                    Size = 100,
                    Value = lastName ?? Convert.DBNull,
                    SqlDbType = SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "firstName",
                    Size = 100,
                    Value = firstName ?? Convert.DBNull,
                    SqlDbType = SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "middleNames",
                    Size = 100,
                    Value = middleNames ?? Convert.DBNull,
                    SqlDbType = SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "beginDate",
                    Value = beginDate ?? Convert.DBNull,
                    SqlDbType = SqlDbType.DateTime,
                },
                new SqlParameter
                {
                    ParameterName = "endDate",
                    Value = endDate ?? Convert.DBNull,
                    SqlDbType = SqlDbType.DateTime,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<arcnet_GetIAATrainingDataForUsersResult>("EXEC @returnValue = [dbo].[arcnet_GetIAATrainingDataForUsers] @ediPIN = @ediPIN, @lastName = @lastName, @firstName = @firstName, @middleNames = @middleNames, @beginDate = @beginDate, @endDate = @endDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Retrieves the last execution date for the arcnet process.
    /// </summary>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of results containing the last execution date.</returns>
    public async virtual Task<List<arcnet_GetLastExecutionDateResult>> ArcnetGetLastExecutionDateAsync(OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<arcnet_GetLastExecutionDateResult>("EXEC @returnValue = [dbo].[arcnet_GetLastExecutionDate]", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Imports data for the arcnet process using the specified log ID.
    /// </summary>
    /// <param name="logId">The ID of the log entry associated with the import.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The number of affected rows.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logId"/> is null.</exception>
    public async virtual Task<int> ArcnetImportAsync(int? logId, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
                new SqlParameter
                {
                    ParameterName = "logId",
                    Value = logId ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                parameterreturnValue,
            };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [dbo].[arcnet_import] @logId = @logId", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    #endregion
}
