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
    #region Command Structure Management

    /// <summary>
    /// Retrieves all sub-units for a specified unit based on the report view.
    /// </summary>
    /// <param name="unitId">The ID of the unit.</param>
    /// <param name="rptView">The report view type.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of sub-units for the specified unit.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="unitId"/> or <paramref name="rptView"/> is null.</exception>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStruct_sp_GetAllSubUnitsForUnit].</remarks>
    public async virtual Task<List<cmdStruct_sp_GetAllSubUnitsForUnitResult>> cmdStruct_sp_GetAllSubUnitsForUnitAsync(int? unitId, int? rptView, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                    ParameterName = "unitId",
                    Value = unitId ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "rptView",
                    Value = rptView ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<cmdStruct_sp_GetAllSubUnitsForUnitResult>("EXEC @returnValue = [dbo].[cmdStruct_sp_GetAllSubUnitsForUnit] @unitId = @unitId, @rptView = @rptView", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Retrieves the immediate children for a specified unit based on the report view.
    /// </summary>
    /// <param name="unitId">The ID of the unit.</param>
    /// <param name="rptView">The report view type.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of immediate children for the specified unit.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStruct_sp_GetImmediateChildrenForUnit].</remarks>
    public async virtual Task<List<cmdStruct_sp_GetImmediateChildrenForUnitResult>> cmdStruct_sp_GetImmediateChildrenForUnitAsync(int? unitId, int? rptView, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                    ParameterName = "unitId",
                    Value = unitId ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "rptView",
                    Value = rptView ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<cmdStruct_sp_GetImmediateChildrenForUnitResult>("EXEC @returnValue = [dbo].[cmdStruct_sp_GetImmediateChildrenForUnit] @unitId = @unitId, @rptView = @rptView", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Retrieves the numbered air forces for PH.
    /// </summary>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of numbered air forces for PH.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStruct_sp_GetNumberedAirForcesForPH].</remarks>
    public async virtual Task<List<cmdStruct_sp_GetNumberedAirForcesForPHResult>> cmdStruct_sp_GetNumberedAirForcesForPHAsync(OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
        var _ = await _context.SqlQueryToListAsync<cmdStruct_sp_GetNumberedAirForcesForPHResult>("EXEC @returnValue = [dbo].[cmdStruct_sp_GetNumberedAirForcesForPH]", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Retrieves the unit ID based on the unit name and PAS code.
    /// </summary>
    /// <param name="unitName">The name of the unit.</param>
    /// <param name="pasCode">The PAS code of the unit.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list containing the unit ID information.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStruct_sp_GetUnitId].</remarks>
    public async virtual Task<List<cmdStruct_sp_GetUnitIdResult>> cmdStruct_sp_GetUnitIdAsync(string? unitName, string? pasCode, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                    ParameterName = "unitName",
                    Size = 100,
                    Value = unitName ?? Convert.DBNull,
                    SqlDbType = SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "pasCode",
                    Size = 10,
                    Value = pasCode ?? Convert.DBNull,
                    SqlDbType = SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<cmdStruct_sp_GetUnitIdResult>("EXEC @returnValue = [dbo].[cmdStruct_sp_GetUnitId] @unitName = @unitName, @pasCode = @pasCode", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Retrieves the PAS code for a unit based on the unit ID or unit name.
    /// </summary>
    /// <param name="unitId">The ID of the unit.</param>
    /// <param name="unitName">The name of the unit.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list containing the PAS code information.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStruct_sp_GetUnitPasCode].</remarks>
    public async virtual Task<List<cmdStruct_sp_GetUnitPasCodeResult>> cmdStruct_sp_GetUnitPasCodeAsync(int? unitId, string? unitName, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                    ParameterName = "unitId",
                    Value = unitId ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "unitName",
                    Size = 100,
                    Value = unitName ?? Convert.DBNull,
                    SqlDbType = SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<cmdStruct_sp_GetUnitPasCodeResult>("EXEC @returnValue = [dbo].[cmdStruct_sp_GetUnitPasCode] @unitId = @unitId, @unitName = @unitName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Rebuilds all trees in the command structure.
    /// </summary>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of results from rebuilding all trees.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStruct_sp_RebuildAllTrees].</remarks>
    public async virtual Task<List<cmdStruct_sp_RebuildAllTreesResult>> cmdStruct_sp_RebuildAllTreesAsync(OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
        var _ = await _context.SqlQueryToListAsync<cmdStruct_sp_RebuildAllTreesResult>("EXEC @returnValue = [dbo].[cmdStruct_sp_RebuildAllTrees]", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Rebuilds all trees for a single command structure ID.
    /// </summary>
    /// <param name="cs_id">The command structure ID.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of results from rebuilding all trees for the single ID.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStruct_sp_RebuildAllTrees_Single].</remarks>
    public async virtual Task<List<cmdStruct_sp_RebuildAllTrees_SingleResult>> cmdStruct_sp_RebuildAllTrees_SingleAsync(int? cs_id, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                    ParameterName = "cs_id",
                    Value = cs_id ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<cmdStruct_sp_RebuildAllTrees_SingleResult>("EXEC @returnValue = [dbo].[cmdStruct_sp_RebuildAllTrees_Single] @cs_id = @cs_id", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Rebuilds the tree for a specified view type.
    /// </summary>
    /// <param name="viewType">The view type for which to rebuild the tree.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of results from rebuilding the tree.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStruct_sp_RebuildTree].</remarks>
    public async virtual Task<List<cmdStruct_sp_RebuildTreeResult>> cmdStruct_sp_RebuildTreeAsync(byte? viewType, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                    ParameterName = "viewType",
                    Value = viewType ?? Convert.DBNull,
                    SqlDbType = SqlDbType.TinyInt,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<cmdStruct_sp_RebuildTreeResult>("EXEC @returnValue = [dbo].[cmdStruct_sp_RebuildTree] @viewType = @viewType", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Rebuilds the tree for a single command structure ID and view type.
    /// </summary>
    /// <param name="cs_id">The command structure ID for which to rebuild the tree.</param>
    /// <param name="viewType">The view type for which to rebuild the tree.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of results from rebuilding the tree for the single command structure.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStruct_sp_RebuildTree_Single].</remarks>
    public async virtual Task<List<cmdStruct_sp_RebuildTree_SingleResult>> cmdStruct_sp_RebuildTree_SingleAsync(int? cs_id, byte? viewType, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                    ParameterName = "cs_id",
                    Value = cs_id ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "viewType",
                    Value = viewType ?? Convert.DBNull,
                    SqlDbType = SqlDbType.TinyInt,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<cmdStruct_sp_RebuildTree_SingleResult>("EXEC @returnValue = [dbo].[cmdStruct_sp_RebuildTree_Single] @cs_id = @cs_id, @viewType = @viewType", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Updates affected items for a command structure and user.
    /// </summary>
    /// <param name="cs_id">The command structure ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of results from updating affected items.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStruct_sp_UpdateAffected].</remarks>
    public async virtual Task<List<cmdStruct_sp_UpdateAffectedResult>> cmdStruct_sp_UpdateAffectedAsync(int? cs_id, int? userId, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                    ParameterName = "cs_id",
                    Value = cs_id ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "userId",
                    Value = userId ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<cmdStruct_sp_UpdateAffectedResult>("EXEC @returnValue = [dbo].[cmdStruct_sp_UpdateAffected] @cs_id = @cs_id, @userId = @userId", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Retrieves command structure chain by user PAS code.
    /// </summary>
    /// <param name="userPasCode">The user PAS code.</param>
    /// <param name="viewType">The view type.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of command structure chain results by PAS code.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStructChain_sp_GetChaninByPascode].</remarks>
    public async virtual Task<List<cmdStructChain_sp_GetChaninByPascodeResult>> cmdStructChain_sp_GetChaninByPascodeAsync(string? userPasCode, int? viewType, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                    ParameterName = "userPasCode",
                    Size = 10,
                    Value = userPasCode ?? Convert.DBNull,
                    SqlDbType = SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "viewType",
                    Value = viewType ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<cmdStructChain_sp_GetChaninByPascodeResult>("EXEC @returnValue = [dbo].[cmdStructChain_sp_GetChaninByPascode] @userPasCode = @userPasCode, @viewType = @viewType", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Retrieves command structure chain by unit ID.
    /// </summary>
    /// <param name="cs_id">The command structure ID.</param>
    /// <param name="viewType">The view type.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of command structure chain results by unit.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStructChain_sp_GetChaninByUnit].</remarks>
    public async virtual Task<List<cmdStructChain_sp_GetChaninByUnitResult>> cmdStructChain_sp_GetChaninByUnitAsync(int? cs_id, int? viewType, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                    ParameterName = "cs_id",
                    Value = cs_id ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "viewType",
                    Value = viewType ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<cmdStructChain_sp_GetChaninByUnitResult>("EXEC @returnValue = [dbo].[cmdStructChain_sp_GetChaninByUnit] @cs_id = @cs_id, @viewType = @viewType", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    /// <summary>
    /// Retrieves command structure chain by user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="viewType">The view type.</param>
    /// <param name="returnValue">Output parameter containing the return value from the stored procedure.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of command structure chain results by user ID.</returns>
    /// <remarks>This method executes the stored procedure [dbo].[cmdStructChain_sp_GetChaninByUserId].</remarks>
    public async virtual Task<List<cmdStructChain_sp_GetChaninByUserIdResult>> cmdStructChain_sp_GetChaninByUserIdAsync(int? userId, int? viewType, OutputParameter<int>? returnValue = null, CancellationToken? cancellationToken = default)
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
                    ParameterName = "userId",
                    Value = userId ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "viewType",
                    Value = viewType ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                parameterreturnValue,
            };
        var _ = await _context.SqlQueryToListAsync<cmdStructChain_sp_GetChaninByUserIdResult>("EXEC @returnValue = [dbo].[cmdStructChain_sp_GetChaninByUserId] @userId = @userId, @viewType = @viewType", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    #endregion
}
