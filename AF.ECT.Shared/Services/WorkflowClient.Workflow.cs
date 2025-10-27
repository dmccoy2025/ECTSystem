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
    #region Workflow Methods

/// <summary>
    /// Retrieves a workflow by ID.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow by ID response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowByIdResponse> GetWorkflowByIdAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkflowByIdAsync(new GetWorkflowByIdRequest { WorkflowId = workflowId }).ResponseAsync;
            });

            var duration = DateTime.UtcNow - startTime;
            LogAuditEvent(nameof(GetWorkflowByIdAsync), correlationId, startTime, duration, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            LogAuditEvent(nameof(GetWorkflowByIdAsync), correlationId, startTime, duration, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves a workflow by ID as a streaming response.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>An asynchronous enumerable of workflow by ID items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<WorkflowByIdItem> GetWorkflowByIdStream(int workflowId)
    {
        using var call = _client.GetWorkflowByIdStream(new GetWorkflowByIdRequest { WorkflowId = workflowId });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves workflows by reference ID.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflows by ref ID response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowsByRefIdResponse> GetWorkflowsByRefIdAsync(int refId, int module)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkflowsByRefIdAsync(new GetWorkflowsByRefIdRequest
                {
                    RefId = refId,
                    Module = module
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowsByRefIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { RefId = refId, Module = module });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowsByRefIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { RefId = refId, Module = module });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflows by reference ID as a streaming response.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>An asynchronous enumerable of workflow by ref ID items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<WorkflowByRefIdItem> GetWorkflowsByRefIdStream(int refId, int module)
    {
        using var call = _client.GetWorkflowsByRefIdStream(new GetWorkflowsByRefIdRequest
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
    /// Retrieves workflows by reference ID and type.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowType">The workflow type.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflows by ref ID and type response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowsByRefIdAndTypeResponse> GetWorkflowsByRefIdAndTypeAsync(int refId, int module, int workflowType)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkflowsByRefIdAndTypeAsync(new GetWorkflowsByRefIdAndTypeRequest
                {
                    RefId = refId,
                    Module = module,
                    WorkflowType = workflowType
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowsByRefIdAndTypeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { RefId = refId, Module = module, WorkflowType = workflowType });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowsByRefIdAndTypeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { RefId = refId, Module = module, WorkflowType = workflowType });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflows by reference ID and type as a streaming response.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowType">The workflow type.</param>
    /// <returns>An asynchronous enumerable of workflow by ref ID and type items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<WorkflowByRefIdAndTypeItem> GetWorkflowsByRefIdAndTypeStream(int refId, int module, int workflowType)
    {
        using var call = _client.GetWorkflowsByRefIdAndTypeStream(new GetWorkflowsByRefIdAndTypeRequest
        {
            RefId = refId,
            Module = module,
            WorkflowType = workflowType
        });

        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves workflow types.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the workflow types response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowTypesResponse> GetWorkflowTypesAsync()
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWorkflowTypesAsync(new EmptyRequest()).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowTypesAsync), correlationId, startTime, _stopwatch.Elapsed, true);

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowTypesAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflow types as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of workflow type items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<WorkflowTypeItem> GetWorkflowTypesStream()
    {
        using var call = _client.GetWorkflowTypesStream(new EmptyRequest());
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Inserts a new workflow.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowType">The workflow type.</param>
    /// <param name="workflowText">The workflow text.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow insertion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<InsertWorkflowResponse> InsertWorkflowAsync(int refId, int module, int workflowType, string workflowText, int userId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.InsertWorkflowAsync(new InsertWorkflowRequest
                {
                    RefId = refId,
                    Module = module,
                    WorkflowType = workflowType,
                    WorkflowText = workflowText,
                    UserId = userId
                }).ResponseAsync;
            });

            var duration = DateTime.UtcNow - startTime;
            LogAuditEvent(nameof(InsertWorkflowAsync), correlationId, startTime, duration, true, additionalData: new { RefId = refId, Module = module, WorkflowType = workflowType, UserId = userId });

            return result;
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            LogAuditEvent(nameof(InsertWorkflowAsync), correlationId, startTime, duration, false, ex.Message, new { RefId = refId, Module = module, WorkflowType = workflowType, UserId = userId });
            throw;
        }
    }

    /// <summary>
    /// Updates a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="workflowText">The workflow text.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow update response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<UpdateWorkflowResponse> UpdateWorkflowAsync(int workflowId, string workflowText, int userId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.UpdateWorkflowAsync(new UpdateWorkflowRequest
                {
                    WorkflowId = workflowId,
                    WorkflowText = workflowText,
                    UserId = userId
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId, UserId = userId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId, UserId = userId });
            throw;
        }
    }

    #endregion
}
