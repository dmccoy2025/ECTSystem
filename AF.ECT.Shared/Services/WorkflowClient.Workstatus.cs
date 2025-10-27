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
    #region Workstatus Methods

/// <summary>
    /// Retrieves a workstatus by ID.
    /// </summary>
    /// <param name="workstatusId">The workstatus ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus by ID response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkstatusByIdResponse> GetWorkstatusByIdAsync(int workstatusId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkstatusByIdAsync(new GetWorkstatusByIdRequest { WorkstatusId = workstatusId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkstatusByIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkstatusId = workstatusId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkstatusByIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkstatusId = workstatusId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves a workstatus by ID as a streaming response.
    /// </summary>
    /// <param name="workstatusId">The workstatus ID.</param>
    /// <returns>An asynchronous enumerable of workstatus by ID items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<WorkstatusByIdItem> GetWorkstatusByIdStream(int workstatusId)
    {
        using var call = _client.GetWorkstatusByIdStream(new GetWorkstatusByIdRequest { WorkstatusId = workstatusId });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves workstatuses by reference ID.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatuses by ref ID response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkstatusesByRefIdResponse> GetWorkstatusesByRefIdAsync(int refId, int module)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkstatusesByRefIdAsync(new GetWorkstatusesByRefIdRequest
                {
                    RefId = refId,
                    Module = module
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkstatusesByRefIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { RefId = refId, Module = module });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkstatusesByRefIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { RefId = refId, Module = module });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workstatuses by reference ID as a streaming response.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>An asynchronous enumerable of workstatus by ref ID items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<WorkstatusByRefIdItem> GetWorkstatusesByRefIdStream(int refId, int module)
    {
        using var call = _client.GetWorkstatusesByRefIdStream(new GetWorkstatusesByRefIdRequest
        {
            RefId = refId,
            Module = module
        });

        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves workstatuses by reference ID and type.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workstatusType">The workstatus type.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatuses by ref ID and type response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkstatusesByRefIdAndTypeResponse> GetWorkstatusesByRefIdAndTypeAsync(int refId, int module, int workstatusType)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkstatusesByRefIdAndTypeAsync(new GetWorkstatusesByRefIdAndTypeRequest
                {
                    RefId = refId,
                    Module = module,
                    WorkstatusType = workstatusType
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkstatusesByRefIdAndTypeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { RefId = refId, Module = module, WorkstatusType = workstatusType });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkstatusesByRefIdAndTypeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { RefId = refId, Module = module, WorkstatusType = workstatusType });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workstatuses by reference ID and type as a streaming response.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workstatusType">The workstatus type.</param>
    /// <returns>An asynchronous enumerable of workstatus by ref ID and type items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<WorkstatusByRefIdAndTypeItem> GetWorkstatusesByRefIdAndTypeStream(int refId, int module, int workstatusType)
    {
        using var call = _client.GetWorkstatusesByRefIdAndTypeStream(new GetWorkstatusesByRefIdAndTypeRequest
        {
            RefId = refId,
            Module = module,
            WorkstatusType = workstatusType
        });

        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves workstatus types.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the workstatus types response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkstatusTypesResponse> GetWorkstatusTypesAsync()
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkstatusTypesAsync(new EmptyRequest()).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkstatusTypesAsync), correlationId, startTime, _stopwatch.Elapsed, true);

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkstatusTypesAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves workstatus types as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of workstatus type items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<WorkstatusTypeItem> GetWorkstatusTypesStream()
    {
        using var call = _client.GetWorkstatusTypesStream(new EmptyRequest());
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Inserts a new workstatus.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workstatusType">The workstatus type.</param>
    /// <param name="workstatusText">The workstatus text.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus insertion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<InsertWorkstatusResponse> InsertWorkstatusAsync(int refId, int module, int workstatusType, string workstatusText, int userId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.InsertWorkstatusAsync(new InsertWorkstatusRequest
                {
                    RefId = refId,
                    Module = module,
                    WorkstatusType = workstatusType,
                    WorkstatusText = workstatusText,
                    UserId = userId
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertWorkstatusAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { RefId = refId, Module = module, WorkstatusType = workstatusType, UserId = userId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertWorkstatusAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { RefId = refId, Module = module, WorkstatusType = workstatusType, UserId = userId });
            throw;
        }
    }

    /// <summary>
    /// Updates a workstatus.
    /// </summary>
    /// <param name="workstatusId">The workstatus ID.</param>
    /// <param name="workstatusText">The workstatus text.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus update response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<UpdateWorkstatusResponse> UpdateWorkstatusAsync(int workstatusId, string workstatusText, int userId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.UpdateWorkstatusAsync(new UpdateWorkstatusRequest
                {
                    WorkstatusId = workstatusId,
                    WorkstatusText = workstatusText,
                    UserId = userId
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateWorkstatusAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkstatusId = workstatusId, UserId = userId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateWorkstatusAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkstatusId = workstatusId, UserId = userId });
            throw;
        }
    }

    #endregion
}
