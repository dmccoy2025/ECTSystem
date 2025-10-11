#nullable enable
#pragma warning disable CS8604 // Possible null reference argument
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using AF.ECT.Data.Extensions;
using AF.ECT.Data.ResultTypes;
using AF.ECT.Data.Interfaces;

namespace AF.ECT.Data.Models;

public partial class ALODContextProcedures : IALODContextProcedures
{
    #region Application Warmup Process

    /// <summary>
    /// Deletes a log entry by its ID from the Application Warmup Process logs.
    /// </summary>
    /// <param name="logId">The ID of the log entry to delete.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The number of affected rows.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logId"/> is null.</exception>
    public async virtual Task<int> ApplicationWarmupProcess_sp_DeleteLogByIdAsync(int? logId, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [dbo].[ApplicationWarmupProcess_sp_DeleteLogById] @logId = @logId", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Finds the last execution date for a specified process in the Application Warmup Process.
    /// </summary>
    /// <param name="processName">The name of the process to query.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of results containing the last execution date information.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="processName"/> is null.</exception>
    public async virtual Task<List<ApplicationWarmupProcess_sp_FindProcessLastExecutionDateResult>> ApplicationWarmupProcess_sp_FindProcessLastExecutionDateAsync(string? processName, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                ParameterName = "processName",
                Size = 200,
                Value = processName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryToListAsync<ApplicationWarmupProcess_sp_FindProcessLastExecutionDateResult>("EXEC @returnValue = [dbo].[ApplicationWarmupProcess_sp_FindProcessLastExecutionDate] @processName = @processName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Retrieves all logs from the Application Warmup Process.
    /// </summary>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of all log entries.</returns>
    public async virtual Task<List<ApplicationWarmupProcess_sp_GetAllLogsResult>> ApplicationWarmupProcess_sp_GetAllLogsAsync(OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
        var _ = await _context.SqlQueryToListAsync<ApplicationWarmupProcess_sp_GetAllLogsResult>("EXEC @returnValue = [dbo].[ApplicationWarmupProcess_sp_GetAllLogs]", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Inserts a new log entry into the Application Warmup Process logs.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <param name="executionDate">The date and time of execution.</param>
    /// <param name="message">The log message.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The number of affected rows.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="processName"/>, <paramref name="executionDate"/>, or <paramref name="message"/> is null.</exception>
    public async virtual Task<int> ApplicationWarmupProcess_sp_InsertLogAsync(string? processName, DateTime? executionDate, string? message, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                ParameterName = "processName",
                Size = 200,
                Value = processName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "executionDate",
                Value = executionDate ?? Convert.DBNull,
                SqlDbType = SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "message",
                Size = -1,
                Value = message ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [dbo].[ApplicationWarmupProcess_sp_InsertLog] @processName = @processName, @executionDate = @executionDate, @message = @message", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Checks if a specified process is active in the Application Warmup Process.
    /// </summary>
    /// <param name="processName">The name of the process to check.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of results indicating if the process is active.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="processName"/> is null.</exception>
    public async virtual Task<List<ApplicationWarmupProcess_sp_IsProcessActiveResult>> ApplicationWarmupProcess_sp_IsProcessActiveAsync(string? processName, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                ParameterName = "processName",
                Size = 200,
                Value = processName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryToListAsync<ApplicationWarmupProcess_sp_IsProcessActiveResult>("EXEC @returnValue = [dbo].[ApplicationWarmupProcess_sp_IsProcessActive] @processName = @processName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Retrieves all logs from the Application Warmup Process with pagination.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of log entries for the specified page.</returns>
    public async virtual Task<List<ApplicationWarmupProcess_sp_GetAllLogsResult>> ApplicationWarmupProcess_sp_GetAllLogs_paginationAsync(int? pageNumber = 1, int? pageSize = 10, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                ParameterName = "PageNumber",
                Value = pageNumber ?? Convert.DBNull,
                SqlDbType = SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "PageSize",
                Value = pageSize ?? Convert.DBNull,
                SqlDbType = SqlDbType.Int,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryToListAsync<ApplicationWarmupProcess_sp_GetAllLogsResult>("EXEC @returnValue = [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination] @PageNumber = @PageNumber, @PageSize = @PageSize", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    #endregion
}
