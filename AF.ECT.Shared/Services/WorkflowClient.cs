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
public partial class WorkflowClient : IWorkflowClient
{
    #region Fields

    private readonly GrpcChannel? _channel;
    private readonly WorkflowService.WorkflowServiceClient _client;
    private readonly ILogger<WorkflowClient>? _logger;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly Stopwatch _stopwatch = new();
    private readonly WorkflowClientOptions _options;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the GreeterClient with a gRPC client (for testing).
    /// </summary>
    /// <param name="grpcClient">The gRPC client to use for communication.</param>
    /// <exception cref="ArgumentNullException">Thrown when grpcClient is null.</exception>
    public WorkflowClient(WorkflowService.WorkflowServiceClient grpcClient)
    {
        _client = grpcClient ?? throw new ArgumentNullException(nameof(grpcClient));
        _channel = null; // No channel when using injected client
        _logger = null; // No logger in test mode
        _options = new WorkflowClientOptions(); // Use default options for testing
        _retryPolicy = Policy.Handle<Grpc.Core.RpcException>()
            .WaitAndRetryAsync(_options.MaxRetryAttempts, attempt =>
            {
                return TimeSpan.FromMilliseconds(Math.Min(_options.InitialRetryDelayMs * Math.Pow(2, attempt), _options.MaxRetryDelayMs));
            });
    }

    /// <summary>
    /// Initializes a new instance of the WorkflowClient with a pre-configured gRPC channel.
    /// </summary>
    /// <param name="channel">The pre-configured gRPC channel to use for communication.</param>
    /// <param name="logger">The logger for performance monitoring.</param>
    /// <param name="options">The configuration options for the workflow client.</param>
    /// <exception cref="ArgumentNullException">Thrown when channel is null.</exception>
    public WorkflowClient(GrpcChannel channel, ILogger<WorkflowClient>? logger = null, IOptions<WorkflowClientOptions>? options = null)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        _logger = logger;
        _options = options?.Value ?? new WorkflowClientOptions();

        // Create client
        _client = new WorkflowService.WorkflowServiceClient(_channel);

        // Configure retry policy for transient failures
        _retryPolicy = Policy.Handle<Grpc.Core.RpcException>(ex =>
                ex.StatusCode == Grpc.Core.StatusCode.Unavailable ||
                ex.StatusCode == Grpc.Core.StatusCode.DeadlineExceeded ||
                ex.StatusCode == Grpc.Core.StatusCode.Internal)
            .WaitAndRetryAsync(_options.MaxRetryAttempts,
                attempt => TimeSpan.FromMilliseconds(Math.Min(_options.InitialRetryDelayMs * Math.Pow(2, attempt), _options.MaxRetryDelayMs)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger?.LogWarning(exception, "gRPC call failed, retrying in {Delay}ms (attempt {Attempt}/{MaxAttempts})",
                        timeSpan.TotalMilliseconds, retryCount, _options.MaxRetryAttempts);
                });
    }

    /// <summary>
    /// Initializes a new instance of the WorkflowClient with an injected gRPC client (for DI with AddGrpcClient).
    /// </summary>
    /// <param name="client">The injected gRPC client.</param>
    /// <param name="logger">The logger for performance monitoring.</param>
    /// <param name="options">The configuration options for the workflow client.</param>
    /// <exception cref="ArgumentNullException">Thrown when client is null.</exception>
    public WorkflowClient(WorkflowService.WorkflowServiceClient client, ILogger<WorkflowClient>? logger = null, IOptions<WorkflowClientOptions>? options = null)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _channel = null; // No channel when using injected client
        _logger = logger;
        _options = options?.Value ?? new WorkflowClientOptions();

        // Configure retry policy for transient failures
        _retryPolicy = Policy.Handle<Grpc.Core.RpcException>(ex =>
                ex.StatusCode == Grpc.Core.StatusCode.Unavailable ||
                ex.StatusCode == Grpc.Core.StatusCode.DeadlineExceeded ||
                ex.StatusCode == Grpc.Core.StatusCode.Internal)
            .WaitAndRetryAsync(_options.MaxRetryAttempts,
                attempt => TimeSpan.FromMilliseconds(Math.Min(_options.InitialRetryDelayMs * Math.Pow(2, attempt), _options.MaxRetryDelayMs)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger?.LogWarning(exception, "gRPC call failed, retrying in {Delay}ms (attempt {Attempt}/{MaxAttempts})",
                        timeSpan.TotalMilliseconds, retryCount, _options.MaxRetryAttempts);
                });
    }

    #endregion

    /// <summary>
    /// Disposes the gRPC channel and releases resources (only when channel is owned by this instance).
    /// </summary>
    public void Dispose()
    {
        _channel?.Dispose();
    }
}
