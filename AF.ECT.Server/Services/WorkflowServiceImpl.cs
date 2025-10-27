using AF.ECT.Server.Services.Interfaces;
using AF.ECT.Data.Interfaces;
using Google.Protobuf.Collections;

namespace AF.ECT.Server.Services;

/// <summary>
/// Main service implementation for workflow operations.
/// This partial class contains the core infrastructure (fields, constructors, helpers).
/// Additional methods are organized in separate partial class files:
/// - WorkflowServiceImpl.UserMethods.cs
/// - WorkflowServiceImpl.CoreWorkflowMethods.cs
/// - WorkflowServiceImpl.WarmupMethods.cs
/// - WorkflowServiceImpl.WorkflowMethods.cs
/// - WorkflowServiceImpl.WorkstatusMethods.cs
/// </summary>
public partial class WorkflowServiceImpl : WorkflowService.WorkflowServiceBase
{
    #region Fields

    private readonly ILogger<WorkflowServiceImpl> _logger;
    private readonly IDataService _dataService;
    private readonly IResilienceService _resilienceService;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the WorkflowManagementService.
    /// </summary>
    /// <param name="logger">The logger for recording service operations.</param>
    /// <param name="dataService">The data service for accessing application data.</param>
    /// <param name="resilienceService">The resilience service for fault tolerance patterns.</param>
    /// <exception cref="ArgumentNullException">Thrown when logger, dataService, or resilienceService is null.</exception>
    public WorkflowServiceImpl(ILogger<WorkflowServiceImpl> logger, IDataService dataService, IResilienceService resilienceService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        _resilienceService = resilienceService ?? throw new ArgumentNullException(nameof(resilienceService));
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Creates a gRPC exception for internal errors with a unique error ID in metadata.
    /// </summary>
    /// <param name="message">The error message to include in the exception.</param>
    /// <returns>An RpcException with Internal status code and error ID metadata.</returns>
    private static RpcException CreateInternalErrorException(string message = "An internal error occurred")
    {
        var errorId = Guid.NewGuid().ToString();
        var trailers = new Metadata { { "error-id", errorId } };
        return new RpcException(new Status(StatusCode.Internal, message), trailers);
    }

    /// <summary>
    /// Creates a gRPC exception for cancelled operations.
    /// </summary>
    /// <param name="message">The cancellation message to include in the exception.</param>
    /// <returns>An RpcException with Cancelled status code.</returns>
    private static RpcException CreateCancelledException(string message = "Operation was cancelled")
    {
        return new RpcException(new Status(StatusCode.Cancelled, message));
    }

    /// <summary>
    /// Executes a data fetch operation and maps results to a gRPC response.
    /// </summary>
    private async Task<TResponse> ExecuteListOperationAsync<TData, TItem, TResponse>(
        Func<Task<IList<TData>>> dataFetcher,
        Func<TData, TItem> itemMapper,
        Func<RepeatedField<TItem>, TResponse> responseFactory,
        string operationName)
        where TResponse : new()
    {
        return await ExecuteGrpcOperationAsync(async () =>
        {
            var results = await _resilienceService.ExecuteWithRetryAsync(dataFetcher);
            var items = new RepeatedField<TItem>();
            items.AddRange(results?.Select(itemMapper) ?? Enumerable.Empty<TItem>());
            return responseFactory(items);
        }, operationName);
    }

    /// <summary>
    /// Executes a gRPC operation with standardized error handling.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <param name="operation">The operation to execute.</param>
    /// <param name="operationName">The name of the operation for logging.</param>
    /// <returns>The operation result.</returns>
    private async Task<TResponse> ExecuteGrpcOperationAsync<TResponse>(
        Func<Task<TResponse>> operation,
        string operationName)
    {
        try
        {
            _logger.LogInformation(operationName);
            return await operation();
        }
        catch (RpcException)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation was cancelled");
            throw CreateCancelledException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while {operationName.ToLower()}");
            throw CreateInternalErrorException();
        }
    }
    
    /// <summary>
    /// Executes a streaming gRPC operation with standardized error handling and cancellation support.
    /// </summary>
    /// <typeparam name="TData">The source data type.</typeparam>
    /// <typeparam name="TItem">The gRPC response item type.</typeparam>
    /// <param name="responseStream">The response stream writer.</param>
    /// <param name="context">The server call context.</param>
    /// <param name="dataFetcher">Function to fetch data from the data service.</param>
    /// <param name="itemMapper">Function to map data items to gRPC items.</param>
    /// <param name="operationName">The name of the operation for logging.</param>
    private async Task ExecuteStreamingOperationAsync<TData, TItem>(
        IServerStreamWriter<TItem> responseStream,
        ServerCallContext context,
        Func<Task<IList<TData>>> dataFetcher,
        Func<TData, TItem> itemMapper,
        string operationName)
    {
        await ExecuteGrpcOperationAsync(async () =>
        {
            var results = await _resilienceService.ExecuteWithRetryAsync(dataFetcher);
            
            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(itemMapper(item));
                }
            }

            return Task.CompletedTask;
            
        }, operationName);
    }

    #endregion

}
