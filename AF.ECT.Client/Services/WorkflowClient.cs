using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using AF.ECT.Shared;
using System.Diagnostics;
using Polly;
using Polly.Retry;
using Microsoft.Extensions.Options;
using AF.ECT.Client.Options;

namespace AF.ECT.Client.Services;

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
public class WorkflowClient : IWorkflowClient
{
    #region Fields

    private readonly GrpcChannel? _channel;
    private readonly WorkflowService.WorkflowServiceClient _client;
    private readonly ILogger<WorkflowClient>? _logger;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly Stopwatch _stopwatch = new();
    private readonly WorkflowClientOptions _options;

    #endregion

    #region Audit Logging Methods

    /// <summary>
    /// Logs audit information for gRPC calls with correlation ID and performance metrics.
    /// </summary>
    /// <param name="methodName">The name of the method being called.</param>
    /// <param name="correlationId">The correlation ID for tracing.</param>
    /// <param name="startTime">The start time of the operation.</param>
    /// <param name="duration">The duration of the operation.</param>
    /// <param name="success">Whether the operation was successful.</param>
    /// <param name="errorMessage">The error message if the operation failed.</param>
    /// <param name="additionalData">Additional audit data.</param>
    private void LogAuditEvent(string methodName, string correlationId, DateTime startTime, TimeSpan duration, bool success, string? errorMessage = null, object? additionalData = null)
    {
        var auditEvent = new
        {
            Timestamp = startTime,
            CorrelationId = correlationId,
            MethodName = methodName,
            DurationMs = duration.TotalMilliseconds,
            Success = success,
            ErrorMessage = errorMessage,
            AdditionalData = additionalData,
            ClientInfo = new
            {
                UserAgent = "ECTSystem-BlazorClient",
                Version = "1.0.0",
                Environment = "Production" // Could be made configurable
            }
        };

        if (success)
        {
            _logger?.LogInformation("gRPC Audit: {MethodName} completed successfully. CorrelationId: {CorrelationId}, Duration: {DurationMs}ms",
                methodName, correlationId, duration.TotalMilliseconds);
        }
        else
        {
            _logger?.LogError("gRPC Audit: {MethodName} failed. CorrelationId: {CorrelationId}, Duration: {DurationMs}ms, Error: {ErrorMessage}",
                methodName, correlationId, duration.TotalMilliseconds, errorMessage);
        }

        // Structured logging for audit trail
        _logger?.LogInformation("gRPC Audit Event: {@AuditEvent}", auditEvent);
    }

    /// <summary>
    /// Generates a unique correlation ID for request tracing.
    /// </summary>
    /// <returns>A unique correlation ID.</returns>
    private static string GenerateCorrelationId() => Guid.NewGuid().ToString();

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
    /// Initializes a new instance of the GreeterClient.
    /// </summary>
    /// <param name="httpClient">The HTTP client for making web requests.</param>
    /// <param name="logger">The logger for performance monitoring.</param>
    /// <param name="options">The configuration options for the workflow client.</param>
    /// <exception cref="ArgumentNullException">Thrown when httpClient is null.</exception>
    public WorkflowClient(HttpClient httpClient, ILogger<WorkflowClient>? logger = null, IOptions<WorkflowClientOptions>? options = null)
    {
        if (httpClient == null)
        {
            throw new ArgumentNullException(nameof(httpClient));
        }

        _logger = logger;
        _options = options?.Value ?? new WorkflowClientOptions();

        // Create optimized HttpClientHandler for connection pooling
        var httpClientHandler = new HttpClientHandler();

        // Configure handler for non-browser platforms (these properties are not available in Blazor WebAssembly)
        // Note: MaxConnectionsPerServer, UseProxy, and UseCookies are not supported in browser environments

        // Create gRPC-Web channel for browser compatibility with optimized settings
        _channel = GrpcChannel.ForAddress(httpClient.BaseAddress!,
            new GrpcChannelOptions
            {
                HttpHandler = new GrpcWebHandler(httpClientHandler),
                MaxReceiveMessageSize = 10 * 1024 * 1024, // 10MB max message size
                MaxSendMessageSize = 10 * 1024 * 1024, // 10MB max message size
                Credentials = Grpc.Core.ChannelCredentials.Insecure, // For development
            });

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

    #endregion
    
    #region Core User Methods

    /// <summary>
    /// Retrieves reinvestigation requests with optional filtering.
    /// </summary>
    /// <param name="userId">Optional user ID filter.</param>
    /// <param name="sarc">Optional SARC flag filter.</param>
    /// <returns>A task representing the asynchronous operation, containing the reinvestigation requests response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetReinvestigationRequestsResponse> GetReinvestigationRequestsAsync(int? userId = null, bool? sarc = null)
    {
        return await _retryPolicy.ExecuteAsync(async () => await _client.GetReinvestigationRequestsAsync(new GetReinvestigationRequestsRequest
        {
            UserId = userId ?? 0,
            Sarc = sarc ?? false
        }));
    }

    /// <summary>
    /// Retrieves reinvestigation requests as a streaming response with optional filtering.
    /// </summary>
    /// <param name="userId">Optional user ID filter.</param>
    /// <param name="sarc">Optional SARC flag filter.</param>
    /// <returns>An asynchronous enumerable of reinvestigation request items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<ReinvestigationRequestItem> GetReinvestigationRequestsStreamAsync(int? userId = null, bool? sarc = null)
    {
        var request = new GetReinvestigationRequestsRequest
        {
            UserId = userId ?? 0,
            Sarc = sarc ?? false
        };

        using var call = _client.GetReinvestigationRequestsStream(request);
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves mailing list for LOD with specified parameters.
    /// </summary>
    /// <param name="refId">The reference ID for the mailing list.</param>
    /// <param name="groupId">The group ID for filtering.</param>
    /// <param name="status">The status for filtering.</param>
    /// <param name="callingService">The calling service identifier.</param>
    /// <returns>A task representing the asynchronous operation, containing the mailing list response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetMailingListForLODResponse> GetMailingListForLODAsync(int refId, int groupId, int status, string callingService)
    {
        return await _retryPolicy.ExecuteAsync(async () => await _client.GetMailingListForLODAsync(new GetMailingListForLODRequest
        {
            RefId = refId,
            GroupId = groupId,
            Status = status,
            CallingService = callingService
        }));
    }

    /// <summary>
    /// Retrieves mailing list for LOD as a streaming response with specified parameters.
    /// </summary>
    /// <param name="refId">The reference ID for the mailing list.</param>
    /// <param name="groupId">The group ID for filtering.</param>
    /// <param name="status">The status for filtering.</param>
    /// <param name="callingService">The calling service identifier.</param>
    /// <returns>An asynchronous enumerable of mailing list items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<MailingListItem> GetMailingListForLODStreamAsync(int refId, int groupId, int status, string callingService)
    {
        var request = new GetMailingListForLODRequest
        {
            RefId = refId,
            GroupId = groupId,
            Status = status,
            CallingService = callingService
        };

        using var call = _client.GetMailingListForLODStream(request);
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves managed users with specified filters.
    /// </summary>
    /// <param name="userid">The user ID filter.</param>
    /// <param name="ssn">The SSN filter.</param>
    /// <param name="name">The name filter.</param>
    /// <param name="status">The status filter.</param>
    /// <param name="role">The role filter.</param>
    /// <param name="srchUnit">The search unit filter.</param>
    /// <param name="showAllUsers">Whether to show all users.</param>
    /// <returns>A task representing the asynchronous operation, containing the managed users response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetManagedUsersResponse> GetManagedUsersAsync(int? userid, string ssn, string name, int? status, int? role, int? srchUnit, bool? showAllUsers)
    {
        return await _retryPolicy.ExecuteAsync(async () => await _client.GetManagedUsersAsync(new GetManagedUsersRequest
        {
            Userid = userid ?? 0,
            Ssn = ssn ?? string.Empty,
            Name = name ?? string.Empty,
            Status = status ?? 0,
            Role = role ?? 0,
            SrchUnit = srchUnit ?? 0,
            ShowAllUsers = showAllUsers ?? false
        }));
    }

    /// <summary>
    /// Retrieves managed users as a streaming response with specified filters.
    /// </summary>
    /// <param name="userid">The user ID filter.</param>
    /// <param name="ssn">The SSN filter.</param>
    /// <param name="name">The name filter.</param>
    /// <param name="status">The status filter.</param>
    /// <param name="role">The role filter.</param>
    /// <param name="srchUnit">The search unit filter.</param>
    /// <param name="showAllUsers">Whether to show all users.</param>
    /// <returns>An asynchronous enumerable of managed user items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<ManagedUserItem> GetManagedUsersStreamAsync(int? userid, string ssn, string name, int? status, int? role, int? srchUnit, bool? showAllUsers)
    {
        using var call = _client.GetManagedUsersStream(new GetManagedUsersRequest
        {
            Userid = userid ?? 0,
            Ssn = ssn ?? string.Empty,
            Name = name ?? string.Empty,
            Status = status ?? 0,
            Role = role ?? 0,
            SrchUnit = srchUnit ?? 0,
            ShowAllUsers = showAllUsers ?? false
        });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves the user ID for a member by their SSN.
    /// </summary>
    /// <param name="memberSsn">The member's SSN.</param>
    /// <returns>A task representing the asynchronous operation, containing the member user ID response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetMembersUserIdResponse> GetMembersUserIdAsync(string memberSsn)
    {
        return await _retryPolicy.ExecuteAsync(async () => await _client.GetMembersUserIdAsync(new GetMembersUserIdRequest { MemberSsn = memberSsn }));
    }

    /// <summary>
    /// Retrieves user alternate titles with specified parameters.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate titles response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetUserAltTitleResponse> GetUserAltTitleAsync(int userId, int groupId)
    {
        return await _retryPolicy.ExecuteAsync(async () => await _client.GetUserAltTitleAsync(new GetUserAltTitleRequest
        {
            UserId = userId,
            GroupId = groupId
        }));
    }

    /// <summary>
    /// Retrieves user alternate titles as a streaming response with specified parameters.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>An asynchronous enumerable of user alt title items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<UserAltTitleItem> GetUserAltTitleStreamAsync(int userId, int groupId)
    {
        using var call = _client.GetUserAltTitleStream(new GetUserAltTitleRequest
        {
            UserId = userId,
            GroupId = groupId
        });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves user alternate titles by group component.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <param name="workCompo">The work component.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate titles by group component response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetUserAltTitleByGroupCompoResponse> GetUserAltTitleByGroupCompoAsync(int groupId, int workCompo)
    {
        return await _retryPolicy.ExecuteAsync(async () => await _client.GetUserAltTitleByGroupCompoAsync(new GetUserAltTitleByGroupCompoRequest
        {
            GroupId = groupId,
            WorkCompo = workCompo
        }));
    }

    /// <summary>
    /// Retrieves user alternate titles by group component as a streaming response.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <param name="workCompo">The work component.</param>
    /// <returns>An asynchronous enumerable of user alt title by group compo items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<UserAltTitleByGroupCompoItem> GetUserAltTitleByGroupCompoStreamAsync(int groupId, int workCompo)
    {
        using var call = _client.GetUserAltTitleByGroupCompoStream(new GetUserAltTitleByGroupCompoRequest
        {
            GroupId = groupId,
            WorkCompo = workCompo
        });
        
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves user names with specified filters.
    /// </summary>
    /// <param name="first">The first name filter.</param>
    /// <param name="last">The last name filter.</param>
    /// <returns>A task representing the asynchronous operation, containing the user names response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetUserNameResponse> GetUserNameAsync(string first, string last)
    {
        return await _retryPolicy.ExecuteAsync(async () => await _client.GetUserNameAsync(new GetUserNameRequest
        {
            First = first,
            Last = last
        }));
    }

    /// <summary>
    /// Retrieves user names as a streaming response with specified filters.
    /// </summary>
    /// <param name="first">The first name filter.</param>
    /// <param name="last">The last name filter.</param>
    /// <returns>An asynchronous enumerable of user name items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<UserNameItem> GetUserNameStreamAsync(string first, string last)
    {
        using var call = _client.GetUserNameStream(new GetUserNameRequest
        {
            First = first,
            Last = last
        });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves users alternate titles by group.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the users alternate titles by group response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetUsersAltTitleByGroupResponse> GetUsersAltTitleByGroupAsync(int groupId)
    {
        return await _retryPolicy.ExecuteAsync(async () => await _client.GetUsersAltTitleByGroupAsync(new GetUsersAltTitleByGroupRequest { GroupId = groupId }));
    }

    /// <summary>
    /// Retrieves users alternate titles by group as a streaming response.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <returns>An asynchronous enumerable of users alt title by group items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<UsersAltTitleByGroupItem> GetUsersAltTitleByGroupStreamAsync(int groupId)
    {
        using var call = _client.GetUsersAltTitleByGroupStream(new GetUsersAltTitleByGroupRequest { GroupId = groupId });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves users who are currently online.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the users online response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetUsersOnlineResponse> GetUsersOnlineAsync()
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetUsersOnlineAsync(new EmptyRequest()));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetUsersOnlineAsync), correlationId, startTime, _stopwatch.Elapsed, true);

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetUsersOnlineAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves users who are currently online as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of user online items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<UserOnlineItem> GetUsersOnlineStreamAsync()
    {
        using var call = _client.GetUsersOnlineStream(new EmptyRequest());
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves WHOIS information for a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the WHOIS response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWhoisResponse> GetWhoisAsync(int userId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWhoisAsync(new GetWhoisRequest { UserId = userId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWhoisAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWhoisAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId });
            throw;
        }
    }

    /// <summary>
    /// Checks if a user has HQ tech account.
    /// </summary>
    /// <param name="originUserId">The origin user ID.</param>
    /// <param name="userEdipin">The user EDIPIN.</param>
    /// <returns>A task representing the asynchronous operation, containing the HQ tech account response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<HasHQTechAccountResponse> HasHQTechAccountAsync(int originUserId, string userEdipin)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.HasHQTechAccountAsync(new HasHQTechAccountRequest
            {
                OriginUserId = originUserId,
                UserEdipin = userEdipin
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(HasHQTechAccountAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { OriginUserId = originUserId, UserEdipin = userEdipin });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(HasHQTechAccountAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { OriginUserId = originUserId, UserEdipin = userEdipin });
            throw;
        }
    }

    /// <summary>
    /// Checks if a status code is final.
    /// </summary>
    /// <param name="statusId">The status ID to check.</param>
    /// <returns>A task representing the asynchronous operation, containing the final status code response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<IsFinalStatusCodeResponse> IsFinalStatusCodeAsync(int statusId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.IsFinalStatusCodeAsync(new IsFinalStatusCodeRequest { StatusId = statusId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(IsFinalStatusCodeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { StatusId = statusId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(IsFinalStatusCodeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { StatusId = statusId });
            throw;
        }
    }

    /// <summary>
    /// Logs out a user.
    /// </summary>
    /// <param name="userId">The user ID to logout.</param>
    /// <returns>A task representing the asynchronous operation, containing the logout response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<LogoutResponse> LogoutAsync(int userId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.LogoutAsync(new LogoutRequest { UserId = userId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(LogoutAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(LogoutAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId });
            throw;
        }
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="workCompo">The work component.</param>
    /// <param name="receiveEmail">Whether to receive email.</param>
    /// <param name="groupId">The group ID.</param>
    /// <param name="accountStatus">The account status.</param>
    /// <param name="expirationDate">The expiration date.</param>
    /// <returns>A task representing the asynchronous operation, containing the user registration response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<RegisterUserResponse> RegisterUserAsync(int userId, string workCompo, bool receiveEmail, int groupId, int accountStatus, string expirationDate)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.RegisterUserAsync(new RegisterUserRequest
            {
                UserId = userId,
                WorkCompo = workCompo,
                ReceiveEmail = receiveEmail,
                GroupId = groupId,
                AccountStatus = accountStatus,
                ExpirationDate = expirationDate ?? string.Empty
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(RegisterUserAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId, WorkCompo = workCompo, ReceiveEmail = receiveEmail, GroupId = groupId, AccountStatus = accountStatus, ExpirationDate = expirationDate });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(RegisterUserAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId, WorkCompo = workCompo, ReceiveEmail = receiveEmail, GroupId = groupId, AccountStatus = accountStatus, ExpirationDate = expirationDate });
            throw;
        }
    }

    /// <summary>
    /// Registers a user role.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <param name="status">The status.</param>
    /// <returns>A task representing the asynchronous operation, containing the user role registration response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<RegisterUserRoleResponse> RegisterUserRoleAsync(int userId, int groupId, int status)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.RegisterUserRoleAsync(new RegisterUserRoleRequest
            {
                UserId = userId,
                GroupId = groupId,
                Status = status
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(RegisterUserRoleAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId, GroupId = groupId, Status = status });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(RegisterUserRoleAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId, GroupId = groupId, Status = status });
            throw;
        }
    }

    /// <summary>
    /// Searches member data with specified parameters.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="ssn">The SSN.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="middleName">The middle name.</param>
    /// <param name="srchUnit">The search unit.</param>
    /// <param name="rptView">The report view.</param>
    /// <returns>A task representing the asynchronous operation, containing the member data search response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<SearchMemberDataResponse> SearchMemberDataAsync(int userId, string ssn, string lastName, string firstName, string middleName, int srchUnit, int rptView)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.SearchMemberDataAsync(new SearchMemberDataRequest
            {
                UserId = userId,
                Ssn = ssn,
                LastName = lastName,
                FirstName = firstName,
                MiddleName = middleName,
                SrchUnit = srchUnit,
                RptView = rptView
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(SearchMemberDataAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId, Ssn = ssn, LastName = lastName, FirstName = firstName, MiddleName = middleName, SrchUnit = srchUnit, RptView = rptView });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(SearchMemberDataAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId, Ssn = ssn, LastName = lastName, FirstName = firstName, MiddleName = middleName, SrchUnit = srchUnit, RptView = rptView });
            throw;
        }
    }

    /// <summary>
    /// Searches member data as a streaming response with specified parameters.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="ssn">The SSN.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="middleName">The middle name.</param>
    /// <param name="srchUnit">The search unit.</param>
    /// <param name="rptView">The report view.</param>
    /// <returns>An asynchronous enumerable of member data items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<MemberDataItem> SearchMemberDataStreamAsync(int userId, string ssn, string lastName, string firstName, string middleName, int srchUnit, int rptView)
    {
        using var call = _client.SearchMemberDataStream(new SearchMemberDataRequest
        {
            UserId = userId,
            Ssn = ssn,
            LastName = lastName,
            FirstName = firstName,
            MiddleName = middleName,
            SrchUnit = srchUnit,
            RptView = rptView
        });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Searches member data (test version) with specified parameters.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="ssn">The SSN.</param>
    /// <param name="name">The name.</param>
    /// <param name="srchUnit">The search unit.</param>
    /// <param name="rptView">The report view.</param>
    /// <returns>A task representing the asynchronous operation, containing the member data test search response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<SearchMemberDataTestResponse> SearchMemberDataTestAsync(int userId, string ssn, string name, int srchUnit, int rptView)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.SearchMemberDataTestAsync(new SearchMemberDataTestRequest
            {
                UserId = userId,
                Ssn = ssn,
                Name = name,
                SrchUnit = srchUnit,
                RptView = rptView
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(SearchMemberDataTestAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId, Ssn = ssn, Name = name, SrchUnit = srchUnit, RptView = rptView });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(SearchMemberDataTestAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId, Ssn = ssn, Name = name, SrchUnit = srchUnit, RptView = rptView });
            throw;
        }
    }

    /// <summary>
    /// Searches member data (test version) as a streaming response with specified parameters.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="ssn">The SSN.</param>
    /// <param name="name">The name.</param>
    /// <param name="srchUnit">The search unit.</param>
    /// <param name="rptView">The report view.</param>
    /// <returns>An asynchronous enumerable of member data test items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<MemberDataTestItem> SearchMemberDataTestStreamAsync(int userId, string ssn, string name, int srchUnit, int rptView)
    {
        using var call = _client.SearchMemberDataTestStream(new SearchMemberDataTestRequest
        {
            UserId = userId,
            Ssn = ssn,
            Name = name,
            SrchUnit = srchUnit,
            RptView = rptView
        });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Updates account status for a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="accountStatus">The account status.</param>
    /// <param name="expirationDate">The expiration date.</param>
    /// <param name="comment">The comment.</param>
    /// <returns>A task representing the asynchronous operation, containing the account status update response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<UpdateAccountStatusResponse> UpdateAccountStatusAsync(int userId, int accountStatus, string expirationDate, string comment)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.UpdateAccountStatusAsync(new UpdateAccountStatusRequest
            {
                UserId = userId,
                AccountStatus = accountStatus,
                ExpirationDate = expirationDate ?? string.Empty,
                Comment = comment
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateAccountStatusAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId, AccountStatus = accountStatus, ExpirationDate = expirationDate, Comment = comment });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateAccountStatusAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId, AccountStatus = accountStatus, ExpirationDate = expirationDate, Comment = comment });
            throw;
        }
    }

    /// <summary>
    /// Updates login information for a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="sessionId">The session ID.</param>
    /// <param name="remoteAddr">The remote address.</param>
    /// <returns>A task representing the asynchronous operation, containing the login update response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<UpdateLoginResponse> UpdateLoginAsync(int userId, string sessionId, string remoteAddr)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.UpdateLoginAsync(new UpdateLoginRequest
            {
                UserId = userId,
                SessionId = sessionId,
                RemoteAddr = remoteAddr
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateLoginAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId, SessionId = sessionId, RemoteAddr = remoteAddr });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateLoginAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId, SessionId = sessionId, RemoteAddr = remoteAddr });
            throw;
        }
    }

    /// <summary>
    /// Updates managed settings for a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="compo">The component.</param>
    /// <param name="roleId">The role ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <param name="comment">The comment.</param>
    /// <param name="receiveEmail">Whether to receive email.</param>
    /// <param name="expirationDate">The expiration date.</param>
    /// <returns>A task representing the asynchronous operation, containing the managed settings update response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<UpdateManagedSettingsResponse> UpdateManagedSettingsAsync(int userId, string compo, int roleId, int groupId, string comment, bool receiveEmail, string expirationDate)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.UpdateManagedSettingsAsync(new UpdateManagedSettingsRequest
            {
                UserId = userId,
                Compo = compo,
                RoleId = roleId,
                GroupId = groupId,
                Comment = comment,
                ReceiveEmail = receiveEmail,
                ExpirationDate = expirationDate ?? string.Empty
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateManagedSettingsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId, Compo = compo, RoleId = roleId, GroupId = groupId, Comment = comment, ReceiveEmail = receiveEmail, ExpirationDate = expirationDate });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateManagedSettingsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId, Compo = compo, RoleId = roleId, GroupId = groupId, Comment = comment, ReceiveEmail = receiveEmail, ExpirationDate = expirationDate });
            throw;
        }
    }

    /// <summary>
    /// Updates user alternate title.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <param name="newTitle">The new title.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate title update response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<UpdateUserAltTitleResponse> UpdateUserAltTitleAsync(int userId, int groupId, string newTitle)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.UpdateUserAltTitleAsync(new UpdateUserAltTitleRequest
            {
                UserId = userId,
                GroupId = groupId,
                NewTitle = newTitle
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateUserAltTitleAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId, GroupId = groupId, NewTitle = newTitle });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(UpdateUserAltTitleAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId, GroupId = groupId, NewTitle = newTitle });
            throw;
        }
    }

    #endregion

    #region Core Workflow Methods

    /// <summary>
    /// Adds a signature to a workflow.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="moduleType">The module type.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="actionId">The action ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <param name="statusIn">The status in.</param>
    /// <param name="statusOut">The status out.</param>
    /// <returns>A task representing the asynchronous operation, containing the signature addition response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<AddSignatureResponse> AddSignatureAsync(int refId, int moduleType, int userId, int actionId, int groupId, int statusIn, int statusOut)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.AddSignatureAsync(new AddSignatureRequest
            {
                RefId = refId,
                ModuleType = moduleType,
                UserId = userId,
                ActionId = actionId,
                GroupId = groupId,
                StatusIn = statusIn,
                StatusOut = statusOut
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(AddSignatureAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { RefId = refId, ModuleType = moduleType, UserId = userId, ActionId = actionId, GroupId = groupId, StatusIn = statusIn, StatusOut = statusOut });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(AddSignatureAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { RefId = refId, ModuleType = moduleType, UserId = userId, ActionId = actionId, GroupId = groupId, StatusIn = statusIn, StatusOut = statusOut });
            throw;
        }
    }

    /// <summary>
    /// Copies actions from one workflow to another.
    /// </summary>
    /// <param name="destWsoid">The destination WSOID.</param>
    /// <param name="srcWsoid">The source WSOID.</param>
    /// <returns>A task representing the asynchronous operation, containing the action copy response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<CopyActionsResponse> CopyActionsAsync(int destWsoid, int srcWsoid)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.CopyActionsAsync(new CopyActionsRequest
            {
                DestWsoid = destWsoid,
                SrcWsoid = srcWsoid
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyActionsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { DestWsoid = destWsoid, SrcWsoid = srcWsoid });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyActionsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { DestWsoid = destWsoid, SrcWsoid = srcWsoid });
            throw;
        }
    }

    /// <summary>
    /// Copies rules from one workflow to another.
    /// </summary>
    /// <param name="destWsoid">The destination WSOID.</param>
    /// <param name="srcWsoid">The source WSOID.</param>
    /// <returns>A task representing the asynchronous operation, containing the rule copy response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<CopyRulesResponse> CopyRulesAsync(int destWsoid, int srcWsoid)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.CopyRulesAsync(new CopyRulesRequest
            {
                DestWsoid = destWsoid,
                SrcWsoid = srcWsoid
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyRulesAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { DestWsoid = destWsoid, SrcWsoid = srcWsoid });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyRulesAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { DestWsoid = destWsoid, SrcWsoid = srcWsoid });
            throw;
        }
    }

    /// <summary>
    /// Copies a workflow from one ID to another.
    /// </summary>
    /// <param name="fromId">The source workflow ID.</param>
    /// <param name="toId">The destination workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow copy response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<CopyWorkflowResponse> CopyWorkflowAsync(int fromId, int toId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.CopyWorkflowAsync(new CopyWorkflowRequest
            {
                FromId = fromId,
                ToId = toId
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { FromId = fromId, ToId = toId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(CopyWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { FromId = fromId, ToId = toId });
            throw;
        }
    }

    /// <summary>
    /// Deletes a status code.
    /// </summary>
    /// <param name="statusId">The status ID to delete.</param>
    /// <returns>A task representing the asynchronous operation, containing the status code deletion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<DeleteStatusCodeResponse> DeleteStatusCodeAsync(int statusId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.DeleteStatusCodeAsync(new DeleteStatusCodeRequest { StatusId = statusId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(DeleteStatusCodeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { StatusId = statusId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(DeleteStatusCodeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { StatusId = statusId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves actions by step.
    /// </summary>
    /// <param name="stepId">The step ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the actions by step response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetActionsByStepResponse> GetActionsByStepAsync(int stepId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetActionsByStepAsync(new GetActionsByStepRequest { StepId = stepId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetActionsByStepAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { StepId = stepId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetActionsByStepAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { StepId = stepId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves active cases with specified parameters.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the active cases response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetActiveCasesResponse> GetActiveCasesAsync(int refId, int groupId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetActiveCasesAsync(new GetActiveCasesRequest
            {
                RefId = refId,
                GroupId = groupId
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetActiveCasesAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { RefId = refId, GroupId = groupId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetActiveCasesAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { RefId = refId, GroupId = groupId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves all findings by reason of.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the all findings by reason of response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetAllFindingByReasonOfResponse> GetAllFindingByReasonOfAsync()
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetAllFindingByReasonOfAsync(new EmptyRequest()));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllFindingByReasonOfAsync), correlationId, startTime, _stopwatch.Elapsed, true);

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllFindingByReasonOfAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all locks.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the all locks response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetAllLocksResponse> GetAllLocksAsync()
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetAllLocksAsync(new EmptyRequest()));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllLocksAsync), correlationId, startTime, _stopwatch.Elapsed, true);

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllLocksAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves cancel reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="isFormal">Whether it's formal.</param>
    /// <returns>A task representing the asynchronous operation, containing the cancel reasons response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetCancelReasonsResponse> GetCancelReasonsAsync(int workflowId, bool isFormal)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetCancelReasonsAsync(new GetCancelReasonsRequest
            {
                WorkflowId = workflowId,
                IsFormal = isFormal
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetCancelReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId, IsFormal = isFormal });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetCancelReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId, IsFormal = isFormal });
            throw;
        }
    }

    /// <summary>
    /// Retrieves creatable workflows by group.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the creatable by group response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetCreatableByGroupResponse> GetCreatableByGroupAsync(string compo, int module, int groupId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetCreatableByGroupAsync(new GetCreatableByGroupRequest
            {
                Compo = compo,
                Module = module,
                GroupId = groupId
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetCreatableByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo, Module = module, GroupId = groupId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetCreatableByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo, Module = module, GroupId = groupId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves finding by reason of by ID.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the finding by reason of by ID response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetFindingByReasonOfByIdResponse> GetFindingByReasonOfByIdAsync(int id)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetFindingByReasonOfByIdAsync(new GetFindingByReasonOfByIdRequest { Id = id }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetFindingByReasonOfByIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Id = id });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetFindingByReasonOfByIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Id = id });
            throw;
        }
    }

    /// <summary>
    /// Retrieves findings for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the findings response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetFindingsResponse> GetFindingsAsync(int workflowId, int groupId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetFindingsAsync(new GetFindingsRequest
            {
                WorkflowId = workflowId,
                GroupId = groupId
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetFindingsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId, GroupId = groupId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetFindingsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId, GroupId = groupId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves module from workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the module from workflow response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetModuleFromWorkflowResponse> GetModuleFromWorkflowAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetModuleFromWorkflowAsync(new GetModuleFromWorkflowRequest { WorkflowId = workflowId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetModuleFromWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetModuleFromWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves page access by group.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="group">The group.</param>
    /// <returns>A task representing the asynchronous operation, containing the page access by group response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetPageAccessByGroupResponse> GetPageAccessByGroupAsync(int workflow, int status, int group)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetPageAccessByGroupAsync(new GetPageAccessByGroupRequest
            {
                Workflow = workflow,
                Status = status,
                Group = group
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPageAccessByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Workflow = workflow, Status = status, Group = group });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPageAccessByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Workflow = workflow, Status = status, Group = group });
            throw;
        }
    }

    /// <summary>
    /// Retrieves page access by workflow view.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <returns>A task representing the asynchronous operation, containing the page access by workflow view response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetPageAccessByWorkflowViewResponse> GetPageAccessByWorkflowViewAsync(string compo, int workflow, int status)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetPageAccessByWorkflowViewAsync(new GetPageAccessByWorkflowViewRequest
            {
                Compo = compo,
                Workflow = workflow,
                Status = status
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPageAccessByWorkflowViewAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo, Workflow = workflow, Status = status });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPageAccessByWorkflowViewAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo, Workflow = workflow, Status = status });
            throw;
        }
    }

    /// <summary>
    /// Retrieves pages by workflow ID.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the pages by workflow ID response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetPagesByWorkflowIdResponse> GetPagesByWorkflowIdAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetPagesByWorkflowIdAsync(new GetPagesByWorkflowIdRequest { WorkflowId = workflowId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPagesByWorkflowIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPagesByWorkflowIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves permissions for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the permissions response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetPermissionsResponse> GetPermissionsAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetPermissionsAsync(new GetPermissionsRequest { WorkflowId = workflowId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPermissionsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPermissionsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves permissions by component.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="compo">The component.</param>
    /// <returns>A task representing the asynchronous operation, containing the permissions by component response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetPermissionsByCompoResponse> GetPermissionsByCompoAsync(int workflowId, string compo)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetPermissionsByCompoAsync(new GetPermissionsByCompoRequest
            {
                WorkflowId = workflowId,
                Compo = compo
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPermissionsByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId, Compo = compo });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetPermissionsByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId, Compo = compo });
            throw;
        }
    }

    /// <summary>
    /// Retrieves return reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the return reasons response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetReturnReasonsResponse> GetReturnReasonsAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetReturnReasonsAsync(new GetReturnReasonsRequest { WorkflowId = workflowId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetReturnReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetReturnReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves RWOA reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the RWOA reasons response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetRwoaReasonsResponse> GetRwoaReasonsAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetRwoaReasonsAsync(new GetRwoaReasonsRequest { WorkflowId = workflowId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetRwoaReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetRwoaReasonsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status codes by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by component response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodesByCompoResponse> GetStatusCodesByCompoAsync(string compo)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetStatusCodesByCompoAsync(new GetStatusCodesByCompoRequest { Compo = compo }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status codes by component and module.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by component and module response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodesByCompoAndModuleResponse> GetStatusCodesByCompoAndModuleAsync(string compo, int module)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetStatusCodesByCompoAndModuleAsync(new GetStatusCodesByCompoAndModuleRequest
            {
                Compo = compo,
                Module = module
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByCompoAndModuleAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo, Module = module });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByCompoAndModuleAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo, Module = module });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status codes by sign code.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by sign code response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodesBySignCodeResponse> GetStatusCodesBySignCodeAsync(int groupId, int module)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetStatusCodesBySignCodeAsync(new GetStatusCodesBySignCodeRequest
            {
                GroupId = groupId,
                Module = module
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesBySignCodeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { GroupId = groupId, Module = module });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesBySignCodeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { GroupId = groupId, Module = module });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status codes by workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by workflow response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodesByWorkflowResponse> GetStatusCodesByWorkflowAsync(int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetStatusCodesByWorkflowAsync(new GetStatusCodesByWorkflowRequest { WorkflowId = workflowId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status codes by workflow and access scope.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="accessScope">The access scope.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by workflow and access scope response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodesByWorkflowAndAccessScopeResponse> GetStatusCodesByWorkflowAndAccessScopeAsync(int workflowId, int accessScope)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetStatusCodesByWorkflowAndAccessScopeAsync(new GetStatusCodesByWorkflowAndAccessScopeRequest
            {
                WorkflowId = workflowId,
                AccessScope = accessScope
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByWorkflowAndAccessScopeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId, AccessScope = accessScope });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodesByWorkflowAndAccessScopeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId, AccessScope = accessScope });
            throw;
        }
    }

    /// <summary>
    /// Retrieves status code scope.
    /// </summary>
    /// <param name="statusId">The status ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the status code scope response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStatusCodeScopeResponse> GetStatusCodeScopeAsync(int statusId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetStatusCodeScopeAsync(new GetStatusCodeScopeRequest { StatusId = statusId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodeScopeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { StatusId = statusId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStatusCodeScopeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { StatusId = statusId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves steps by workflow.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <returns>A task representing the asynchronous operation, containing the steps by workflow response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStepsByWorkflowResponse> GetStepsByWorkflowAsync(int workflow)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetStepsByWorkflowAsync(new GetStepsByWorkflowRequest { Workflow = workflow }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStepsByWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Workflow = workflow });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStepsByWorkflowAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Workflow = workflow });
            throw;
        }
    }

    /// <summary>
    /// Retrieves steps by workflow and status.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="deathStatus">The death status.</param>
    /// <returns>A task representing the asynchronous operation, containing the steps by workflow and status response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetStepsByWorkflowAndStatusResponse> GetStepsByWorkflowAndStatusAsync(int workflow, int status, string deathStatus)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetStepsByWorkflowAndStatusAsync(new GetStepsByWorkflowAndStatusRequest
            {
                Workflow = workflow,
                Status = status,
                DeathStatus = deathStatus
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStepsByWorkflowAndStatusAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Workflow = workflow, Status = status, DeathStatus = deathStatus });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetStepsByWorkflowAndStatusAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Workflow = workflow, Status = status, DeathStatus = deathStatus });
            throw;
        }
    }

    /// <summary>
    /// Retrieves viewable workflows by group.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the viewable by group response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetViewableByGroupResponse> GetViewableByGroupAsync(int groupId, int module)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetViewableByGroupAsync(new GetViewableByGroupRequest
            {
                GroupId = groupId,
                Module = module
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetViewableByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { GroupId = groupId, Module = module });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetViewableByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { GroupId = groupId, Module = module });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflow by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow by component response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowByCompoResponse> GetWorkflowByCompoAsync(string compo, int userId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkflowByCompoAsync(new GetWorkflowByCompoRequest
            {
                Compo = compo,
                UserId = userId
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo, UserId = userId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowByCompoAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo, UserId = userId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflow from module.
    /// </summary>
    /// <param name="moduleId">The module ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow from module response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowFromModuleResponse> GetWorkflowFromModuleAsync(int moduleId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkflowFromModuleAsync(new GetWorkflowFromModuleRequest { ModuleId = moduleId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowFromModuleAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { ModuleId = moduleId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowFromModuleAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { ModuleId = moduleId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflow initial status code.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow initial status code response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowInitialStatusCodeResponse> GetWorkflowInitialStatusCodeAsync(int compo, int module, int workflowId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkflowInitialStatusCodeAsync(new GetWorkflowInitialStatusCodeRequest
            {
                Compo = compo,
                Module = module,
                WorkflowId = workflowId
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowInitialStatusCodeAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Compo = compo, Module = module, WorkflowId = workflowId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowInitialStatusCodeAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Compo = compo, Module = module, WorkflowId = workflowId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflow title.
    /// </summary>
    /// <param name="moduleId">The module ID.</param>
    /// <param name="subCase">The sub case.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow title response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowTitleResponse> GetWorkflowTitleAsync(int moduleId, int subCase)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkflowTitleAsync(new GetWorkflowTitleRequest
            {
                ModuleId = moduleId,
                SubCase = subCase
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowTitleAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { ModuleId = moduleId, SubCase = subCase });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowTitleAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { ModuleId = moduleId, SubCase = subCase });
            throw;
        }
    }

    /// <summary>
    /// Retrieves workflow title by work status ID.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="subCase">The sub case.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow title by work status ID response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetWorkflowTitleByWorkStatusIdResponse> GetWorkflowTitleByWorkStatusIdAsync(int workflowId, int subCase)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkflowTitleByWorkStatusIdAsync(new GetWorkflowTitleByWorkStatusIdRequest
            {
                WorkflowId = workflowId,
                SubCase = subCase
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowTitleByWorkStatusIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { WorkflowId = workflowId, SubCase = subCase });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetWorkflowTitleByWorkStatusIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { WorkflowId = workflowId, SubCase = subCase });
            throw;
        }
    }

    /// <summary>
    /// Inserts an action.
    /// </summary>
    /// <param name="type">The action type.</param>
    /// <param name="stepId">The step ID.</param>
    /// <param name="target">The target.</param>
    /// <param name="data">The data.</param>
    /// <returns>A task representing the asynchronous operation, containing the action insertion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<InsertActionResponse> InsertActionAsync(int type, int stepId, int target, int data)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.InsertActionAsync(new InsertActionRequest
            {
                Type = type,
                StepId = stepId,
                Target = target,
                Data = data
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertActionAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Type = type, StepId = stepId, Target = target, Data = data });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertActionAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Type = type, StepId = stepId, Target = target, Data = data });
            throw;
        }
    }

    /// <summary>
    /// Inserts an option action.
    /// </summary>
    /// <param name="type">The action type.</param>
    /// <param name="wsoid">The WSOID.</param>
    /// <param name="target">The target.</param>
    /// <param name="data">The data.</param>
    /// <returns>A task representing the asynchronous operation, containing the option action insertion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<InsertOptionActionResponse> InsertOptionActionAsync(int type, int wsoid, int target, int data)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.InsertOptionActionAsync(new InsertOptionActionRequest
            {
                Type = type,
                Wsoid = wsoid,
                Target = target,
                Data = data
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertOptionActionAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Type = type, Wsoid = wsoid, Target = target, Data = data });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertOptionActionAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Type = type, Wsoid = wsoid, Target = target, Data = data });
            throw;
        }
    }

    #endregion

    #region Application Warmup Process Methods

    /// <summary>
    /// Deletes a log by ID.
    /// </summary>
    /// <param name="logId">The log ID to delete.</param>
    /// <returns>A task representing the asynchronous operation, containing the log deletion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<DeleteLogByIdResponse> DeleteLogByIdAsync(int logId)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.DeleteLogByIdAsync(new DeleteLogByIdRequest { LogId = logId }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(DeleteLogByIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { LogId = logId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(DeleteLogByIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { LogId = logId });
            throw;
        }
    }

    /// <summary>
    /// Finds the last execution date of a process.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>A task representing the asynchronous operation, containing the process last execution date response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<FindProcessLastExecutionDateResponse> FindProcessLastExecutionDateAsync(string processName)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.FindProcessLastExecutionDateAsync(new FindProcessLastExecutionDateRequest { ProcessName = processName }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(FindProcessLastExecutionDateAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { ProcessName = processName });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(FindProcessLastExecutionDateAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { ProcessName = processName });
            throw;
        }
    }

    /// <summary>
    /// Finds the last execution date of a process as a streaming response.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>An asynchronous enumerable of process last execution date items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<ProcessLastExecutionDateItem> FindProcessLastExecutionDateStreamAsync(string processName)
    {
        using var call = _client.FindProcessLastExecutionDateStream(new FindProcessLastExecutionDateRequest { ProcessName = processName });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Retrieves all logs.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the all logs response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetAllLogsResponse> GetAllLogsAsync()
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetAllLogsAsync(new EmptyRequest()));

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllLogsAsync), correlationId, startTime, _stopwatch.Elapsed, true);

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetAllLogsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all logs as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of log items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<LogItem> GetAllLogsStreamAsync()
    {
        using var call = _client.GetAllLogsStream(new EmptyRequest());
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    /// <summary>
    /// Inserts a log entry.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <param name="executionDate">The execution date.</param>
    /// <param name="message">The log message.</param>
    /// <returns>A task representing the asynchronous operation, containing the log insertion response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<InsertLogResponse> InsertLogAsync(string processName, string executionDate, string message)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.InsertLogAsync(new InsertLogRequest
            {
                ProcessName = processName,
                ExecutionDate = executionDate,
                Message = message
            }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertLogAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { ProcessName = processName, ExecutionDate = executionDate, Message = message });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(InsertLogAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { ProcessName = processName, ExecutionDate = executionDate, Message = message });
            throw;
        }
    }

    /// <summary>
    /// Checks if a process is active.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>A task representing the asynchronous operation, containing the process active response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<IsProcessActiveResponse> IsProcessActiveAsync(string processName)
    {
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.IsProcessActiveAsync(new IsProcessActiveRequest { ProcessName = processName }));

            _stopwatch.Stop();
            LogAuditEvent(nameof(IsProcessActiveAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { ProcessName = processName });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(IsProcessActiveAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { ProcessName = processName });
            throw;
        }
    }

    /// <summary>
    /// Checks if a process is active as a streaming response.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>An asynchronous enumerable of process active items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<ProcessActiveItem> IsProcessActiveStreamAsync(string processName)
    {
        using var call = _client.IsProcessActiveStream(new IsProcessActiveRequest { ProcessName = processName });
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            yield return call.ResponseStream.Current;
        }
    }

    #endregion

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
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkflowByIdAsync(new GetWorkflowByIdRequest { WorkflowId = workflowId }));

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
    public async IAsyncEnumerable<WorkflowByIdItem> GetWorkflowByIdStreamAsync(int workflowId)
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
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkflowsByRefIdAsync(new GetWorkflowsByRefIdRequest
            {
                RefId = refId,
                Module = module
            }));

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
    public async IAsyncEnumerable<WorkflowByRefIdItem> GetWorkflowsByRefIdStreamAsync(int refId, int module)
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
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkflowsByRefIdAndTypeAsync(new GetWorkflowsByRefIdAndTypeRequest
            {
                RefId = refId,
                Module = module,
                WorkflowType = workflowType
            }));

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
    public async IAsyncEnumerable<WorkflowByRefIdAndTypeItem> GetWorkflowsByRefIdAndTypeStreamAsync(int refId, int module, int workflowType)
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
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkflowTypesAsync(new EmptyRequest()));

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
    public async IAsyncEnumerable<WorkflowTypeItem> GetWorkflowTypesStreamAsync()
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
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.InsertWorkflowAsync(new InsertWorkflowRequest
            {
                RefId = refId,
                Module = module,
                WorkflowType = workflowType,
                WorkflowText = workflowText,
                UserId = userId
            }));

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
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.UpdateWorkflowAsync(new UpdateWorkflowRequest
            {
                WorkflowId = workflowId,
                WorkflowText = workflowText,
                UserId = userId
            }));

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
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkstatusByIdAsync(new GetWorkstatusByIdRequest { WorkstatusId = workstatusId }));

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
    public async IAsyncEnumerable<WorkstatusByIdItem> GetWorkstatusByIdStreamAsync(int workstatusId)
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
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkstatusesByRefIdAsync(new GetWorkstatusesByRefIdRequest
            {
                RefId = refId,
                Module = module
            }));

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
    public async IAsyncEnumerable<WorkstatusByRefIdItem> GetWorkstatusesByRefIdStreamAsync(int refId, int module)
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
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkstatusesByRefIdAndTypeAsync(new GetWorkstatusesByRefIdAndTypeRequest
            {
                RefId = refId,
                Module = module,
                WorkstatusType = workstatusType
            }));

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
    public async IAsyncEnumerable<WorkstatusByRefIdAndTypeItem> GetWorkstatusesByRefIdAndTypeStreamAsync(int refId, int module, int workstatusType)
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
        return await _retryPolicy.ExecuteAsync(async () => await _client.GetWorkstatusTypesAsync(new EmptyRequest()));
    }

    /// <summary>
    /// Retrieves workstatus types as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of workstatus type items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<WorkstatusTypeItem> GetWorkstatusTypesStreamAsync()
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
        return await _retryPolicy.ExecuteAsync(async () => await _client.InsertWorkstatusAsync(new InsertWorkstatusRequest
        {
            RefId = refId,
            Module = module,
            WorkstatusType = workstatusType,
            WorkstatusText = workstatusText,
            UserId = userId
        }));
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
            var result = await _retryPolicy.ExecuteAsync(async () => await _client.UpdateWorkstatusAsync(new UpdateWorkstatusRequest
            {
                WorkstatusId = workstatusId,
                WorkstatusText = workstatusText,
                UserId = userId
            }));

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

    /// <summary>
    /// Disposes the gRPC channel and releases resources.
    /// </summary>
    public void Dispose()
    {
        _channel?.Dispose();
    }
}
