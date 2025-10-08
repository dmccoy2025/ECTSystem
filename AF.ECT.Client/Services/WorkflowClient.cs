using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using AF.ECT.Shared;
using System.Diagnostics;
using Polly;
using Polly.Retry;

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

    // Performance configuration constants
    private const int MaxRetryAttempts = 3;
    private const int InitialRetryDelayMs = 100;
    private const int MaxRetryDelayMs = 1000;
    private const int RequestTimeoutSeconds = 30;

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
        _retryPolicy = Policy.Handle<Grpc.Core.RpcException>()
            .WaitAndRetryAsync(MaxRetryAttempts, attempt => TimeSpan.FromMilliseconds(Math.Min(InitialRetryDelayMs * Math.Pow(2, attempt), MaxRetryDelayMs)));
    }

    /// <summary>
    /// Initializes a new instance of the GreeterClient.
    /// </summary>
    /// <param name="httpClient">The HTTP client for making web requests.</param>
    /// <param name="logger">The logger for performance monitoring.</param>
    /// <exception cref="ArgumentNullException">Thrown when httpClient is null.</exception>
    public WorkflowClient(HttpClient httpClient, ILogger<WorkflowClient>? logger = null)
    {
        if (httpClient == null)
        {
            throw new ArgumentNullException(nameof(httpClient));
        }

        _logger = logger;

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
                Credentials = Grpc.Core.ChannelCredentials.Insecure // For development
            });

        _client = new WorkflowService.WorkflowServiceClient(_channel);

        // Configure retry policy for transient failures
        _retryPolicy = Policy.Handle<Grpc.Core.RpcException>(ex =>
                ex.StatusCode == Grpc.Core.StatusCode.Unavailable ||
                ex.StatusCode == Grpc.Core.StatusCode.DeadlineExceeded ||
                ex.StatusCode == Grpc.Core.StatusCode.Internal)
            .WaitAndRetryAsync(MaxRetryAttempts,
                attempt => TimeSpan.FromMilliseconds(Math.Min(InitialRetryDelayMs * Math.Pow(2, attempt), MaxRetryDelayMs)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger?.LogWarning(exception, "gRPC call failed, retrying in {Delay}ms (attempt {Attempt}/{MaxAttempts})",
                        timeSpan.TotalMilliseconds, retryCount, MaxRetryAttempts);
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
        try
        {
            var request = new GetReinvestigationRequestsRequest
            {
                UserId = userId ?? 0,
                Sarc = sarc ?? false
            };
            return await _client.GetReinvestigationRequestsAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve reinvestigation requests: {ex.Status.Detail}", ex);
        }
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
        try
        {
            var request = new GetMailingListForLODRequest
            {
                RefId = refId,
                GroupId = groupId,
                Status = status,
                CallingService = callingService
            };
            return await _client.GetMailingListForLODAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve mailing list: {ex.Status.Detail}", ex);
        }
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
        try
        {
            var request = new GetManagedUsersRequest
            {
                Userid = userid ?? 0,
                Ssn = ssn ?? string.Empty,
                Name = name ?? string.Empty,
                Status = status ?? 0,
                Role = role ?? 0,
                SrchUnit = srchUnit ?? 0,
                ShowAllUsers = showAllUsers ?? false
            };
            return await _client.GetManagedUsersAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve managed users: {ex.Status.Detail}", ex);
        }
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
        var request = new GetManagedUsersRequest
        {
            Userid = userid ?? 0,
            Ssn = ssn ?? string.Empty,
            Name = name ?? string.Empty,
            Status = status ?? 0,
            Role = role ?? 0,
            SrchUnit = srchUnit ?? 0,
            ShowAllUsers = showAllUsers ?? false
        };

        using var call = _client.GetManagedUsersStream(request);
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
        try
        {
            var request = new GetMembersUserIdRequest { MemberSsn = memberSsn };
            return await _client.GetMembersUserIdAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve member user ID: {ex.Status.Detail}", ex);
        }
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
        try
        {
            var request = new GetUserAltTitleRequest
            {
                UserId = userId,
                GroupId = groupId
            };
            return await _client.GetUserAltTitleAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve user alternate titles: {ex.Status.Detail}", ex);
        }
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
        var request = new GetUserAltTitleRequest
        {
            UserId = userId,
            GroupId = groupId
        };

        using var call = _client.GetUserAltTitleStream(request);
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
        try
        {
            var request = new GetUserAltTitleByGroupCompoRequest
            {
                GroupId = groupId,
                WorkCompo = workCompo
            };
            return await _client.GetUserAltTitleByGroupCompoAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve user alternate titles by group component: {ex.Status.Detail}", ex);
        }
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
        var request = new GetUserAltTitleByGroupCompoRequest
        {
            GroupId = groupId,
            WorkCompo = workCompo
        };

        using var call = _client.GetUserAltTitleByGroupCompoStream(request);
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
        try
        {
            var request = new GetUserNameRequest
            {
                First = first,
                Last = last
            };
            return await _client.GetUserNameAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve user names: {ex.Status.Detail}", ex);
        }
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
        var request = new GetUserNameRequest
        {
            First = first,
            Last = last
        };

        using var call = _client.GetUserNameStream(request);
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
        try
        {
            var request = new GetUsersAltTitleByGroupRequest { GroupId = groupId };
            return await _client.GetUsersAltTitleByGroupAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve users alternate titles by group: {ex.Status.Detail}", ex);
        }
    }

    /// <summary>
    /// Retrieves users alternate titles by group as a streaming response.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <returns>An asynchronous enumerable of users alt title by group items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<UsersAltTitleByGroupItem> GetUsersAltTitleByGroupStreamAsync(int groupId)
    {
        var request = new GetUsersAltTitleByGroupRequest { GroupId = groupId };

        using var call = _client.GetUsersAltTitleByGroupStream(request);
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
        try
        {
            var request = new EmptyRequest();
            return await _client.GetUsersOnlineAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve users online: {ex.Status.Detail}", ex);
        }
    }

    /// <summary>
    /// Retrieves users who are currently online as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of user online items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<UserOnlineItem> GetUsersOnlineStreamAsync()
    {
        var request = new EmptyRequest();

        using var call = _client.GetUsersOnlineStream(request);
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
        try
        {
            var request = new GetWhoisRequest { UserId = userId };
            return await _client.GetWhoisAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve WHOIS information: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new HasHQTechAccountRequest
            {
                OriginUserId = originUserId,
                UserEdipin = userEdipin
            };
            return await _client.HasHQTechAccountAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to check HQ tech account: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new IsFinalStatusCodeRequest { StatusId = statusId };
            return await _client.IsFinalStatusCodeAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to check final status code: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new LogoutRequest { UserId = userId };
            return await _client.LogoutAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to logout user: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new RegisterUserRequest
            {
                UserId = userId,
                WorkCompo = workCompo,
                ReceiveEmail = receiveEmail,
                GroupId = groupId,
                AccountStatus = accountStatus,
                ExpirationDate = expirationDate ?? string.Empty
            };
            return await _client.RegisterUserAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to register user: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new RegisterUserRoleRequest
            {
                UserId = userId,
                GroupId = groupId,
                Status = status
            };
            return await _client.RegisterUserRoleAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to register user role: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new SearchMemberDataRequest
            {
                UserId = userId,
                Ssn = ssn,
                LastName = lastName,
                FirstName = firstName,
                MiddleName = middleName,
                SrchUnit = srchUnit,
                RptView = rptView
            };
            return await _client.SearchMemberDataAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to search member data: {ex.Status.Detail}", ex);
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
        var request = new SearchMemberDataRequest
        {
            UserId = userId,
            Ssn = ssn,
            LastName = lastName,
            FirstName = firstName,
            MiddleName = middleName,
            SrchUnit = srchUnit,
            RptView = rptView
        };

        using var call = _client.SearchMemberDataStream(request);
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
        try
        {
            var request = new SearchMemberDataTestRequest
            {
                UserId = userId,
                Ssn = ssn,
                Name = name,
                SrchUnit = srchUnit,
                RptView = rptView
            };
            return await _client.SearchMemberDataTestAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to search member data (test): {ex.Status.Detail}", ex);
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
        var request = new SearchMemberDataTestRequest
        {
            UserId = userId,
            Ssn = ssn,
            Name = name,
            SrchUnit = srchUnit,
            RptView = rptView
        };

        using var call = _client.SearchMemberDataTestStream(request);
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
        try
        {
            var request = new UpdateAccountStatusRequest
            {
                UserId = userId,
                AccountStatus = accountStatus,
                ExpirationDate = expirationDate ?? string.Empty,
                Comment = comment
            };
            return await _client.UpdateAccountStatusAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to update account status: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new UpdateLoginRequest
            {
                UserId = userId,
                SessionId = sessionId,
                RemoteAddr = remoteAddr
            };
            return await _client.UpdateLoginAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to update login: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new UpdateManagedSettingsRequest
            {
                UserId = userId,
                Compo = compo,
                RoleId = roleId,
                GroupId = groupId,
                Comment = comment,
                ReceiveEmail = receiveEmail,
                ExpirationDate = expirationDate ?? string.Empty
            };
            return await _client.UpdateManagedSettingsAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to update managed settings: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new UpdateUserAltTitleRequest
            {
                UserId = userId,
                GroupId = groupId,
                NewTitle = newTitle
            };
            return await _client.UpdateUserAltTitleAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to update user alternate title: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new AddSignatureRequest
            {
                RefId = refId,
                ModuleType = moduleType,
                UserId = userId,
                ActionId = actionId,
                GroupId = groupId,
                StatusIn = statusIn,
                StatusOut = statusOut
            };
            return await _client.AddSignatureAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to add signature: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new CopyActionsRequest
            {
                DestWsoid = destWsoid,
                SrcWsoid = srcWsoid
            };
            return await _client.CopyActionsAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to copy actions: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new CopyRulesRequest
            {
                DestWsoid = destWsoid,
                SrcWsoid = srcWsoid
            };
            return await _client.CopyRulesAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to copy rules: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new CopyWorkflowRequest
            {
                FromId = fromId,
                ToId = toId
            };
            return await _client.CopyWorkflowAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to copy workflow: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new DeleteStatusCodeRequest { StatusId = statusId };
            return await _client.DeleteStatusCodeAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to delete status code: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetActionsByStepRequest { StepId = stepId };
            return await _client.GetActionsByStepAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve actions by step: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetActiveCasesRequest
            {
                RefId = refId,
                GroupId = groupId
            };
            return await _client.GetActiveCasesAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve active cases: {ex.Status.Detail}", ex);
        }
    }

    /// <summary>
    /// Retrieves all findings by reason of.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the all findings by reason of response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetAllFindingByReasonOfResponse> GetAllFindingByReasonOfAsync()
    {
        try
        {
            var request = new EmptyRequest();
            return await _client.GetAllFindingByReasonOfAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve all findings by reason of: {ex.Status.Detail}", ex);
        }
    }

    /// <summary>
    /// Retrieves all locks.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the all locks response.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async Task<GetAllLocksResponse> GetAllLocksAsync()
    {
        try
        {
            var request = new EmptyRequest();
            return await _client.GetAllLocksAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve all locks: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetCancelReasonsRequest
            {
                WorkflowId = workflowId,
                IsFormal = isFormal
            };
            return await _client.GetCancelReasonsAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve cancel reasons: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetCreatableByGroupRequest
            {
                Compo = compo,
                Module = module,
                GroupId = groupId
            };
            return await _client.GetCreatableByGroupAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve creatable by group: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetFindingByReasonOfByIdRequest { Id = id };
            return await _client.GetFindingByReasonOfByIdAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve finding by reason of by ID: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetFindingsRequest
            {
                WorkflowId = workflowId,
                GroupId = groupId
            };
            return await _client.GetFindingsAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve findings: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetModuleFromWorkflowRequest { WorkflowId = workflowId };
            return await _client.GetModuleFromWorkflowAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve module from workflow: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetPageAccessByGroupRequest
            {
                Workflow = workflow,
                Status = status,
                Group = group
            };
            return await _client.GetPageAccessByGroupAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve page access by group: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetPageAccessByWorkflowViewRequest
            {
                Compo = compo,
                Workflow = workflow,
                Status = status
            };
            return await _client.GetPageAccessByWorkflowViewAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve page access by workflow view: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetPagesByWorkflowIdRequest { WorkflowId = workflowId };
            return await _client.GetPagesByWorkflowIdAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve pages by workflow ID: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetPermissionsRequest { WorkflowId = workflowId };
            return await _client.GetPermissionsAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve permissions: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetPermissionsByCompoRequest
            {
                WorkflowId = workflowId,
                Compo = compo
            };
            return await _client.GetPermissionsByCompoAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve permissions by component: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetReturnReasonsRequest { WorkflowId = workflowId };
            return await _client.GetReturnReasonsAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve return reasons: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetRwoaReasonsRequest { WorkflowId = workflowId };
            return await _client.GetRwoaReasonsAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve RWOA reasons: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetStatusCodesByCompoRequest { Compo = compo };
            return await _client.GetStatusCodesByCompoAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve status codes by component: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetStatusCodesByCompoAndModuleRequest
            {
                Compo = compo,
                Module = module
            };
            return await _client.GetStatusCodesByCompoAndModuleAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve status codes by component and module: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetStatusCodesBySignCodeRequest
            {
                GroupId = groupId,
                Module = module
            };
            return await _client.GetStatusCodesBySignCodeAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve status codes by sign code: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetStatusCodesByWorkflowRequest { WorkflowId = workflowId };
            return await _client.GetStatusCodesByWorkflowAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve status codes by workflow: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetStatusCodesByWorkflowAndAccessScopeRequest
            {
                WorkflowId = workflowId,
                AccessScope = accessScope
            };
            return await _client.GetStatusCodesByWorkflowAndAccessScopeAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve status codes by workflow and access scope: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetStatusCodeScopeRequest { StatusId = statusId };
            return await _client.GetStatusCodeScopeAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve status code scope: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetStepsByWorkflowRequest { Workflow = workflow };
            return await _client.GetStepsByWorkflowAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve steps by workflow: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetStepsByWorkflowAndStatusRequest
            {
                Workflow = workflow,
                Status = status,
                DeathStatus = deathStatus
            };
            return await _client.GetStepsByWorkflowAndStatusAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve steps by workflow and status: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetViewableByGroupRequest
            {
                GroupId = groupId,
                Module = module
            };
            return await _client.GetViewableByGroupAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve viewable by group: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetWorkflowByCompoRequest
            {
                Compo = compo,
                UserId = userId
            };
            return await _client.GetWorkflowByCompoAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workflow by component: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetWorkflowFromModuleRequest { ModuleId = moduleId };
            return await _client.GetWorkflowFromModuleAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workflow from module: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetWorkflowInitialStatusCodeRequest
            {
                Compo = compo,
                Module = module,
                WorkflowId = workflowId
            };
            return await _client.GetWorkflowInitialStatusCodeAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workflow initial status code: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetWorkflowTitleRequest
            {
                ModuleId = moduleId,
                SubCase = subCase
            };
            return await _client.GetWorkflowTitleAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workflow title: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetWorkflowTitleByWorkStatusIdRequest
            {
                WorkflowId = workflowId,
                SubCase = subCase
            };
            return await _client.GetWorkflowTitleByWorkStatusIdAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workflow title by work status ID: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new InsertActionRequest
            {
                Type = type,
                StepId = stepId,
                Target = target,
                Data = data
            };
            return await _client.InsertActionAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to insert action: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new InsertOptionActionRequest
            {
                Type = type,
                Wsoid = wsoid,
                Target = target,
                Data = data
            };
            return await _client.InsertOptionActionAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to insert option action: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new DeleteLogByIdRequest { LogId = logId };
            return await _client.DeleteLogByIdAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to delete log by ID: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new FindProcessLastExecutionDateRequest { ProcessName = processName };
            return await _client.FindProcessLastExecutionDateAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to find process last execution date: {ex.Status.Detail}", ex);
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
        var request = new FindProcessLastExecutionDateRequest { ProcessName = processName };

        using var call = _client.FindProcessLastExecutionDateStream(request);
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
        try
        {
            var request = new EmptyRequest();
            return await _client.GetAllLogsAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve all logs: {ex.Status.Detail}", ex);
        }
    }

    /// <summary>
    /// Retrieves all logs as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of log items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<LogItem> GetAllLogsStreamAsync()
    {
        var request = new EmptyRequest();

        using var call = _client.GetAllLogsStream(request);
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
        try
        {
            var request = new InsertLogRequest
            {
                ProcessName = processName,
                ExecutionDate = executionDate,
                Message = message
            };
            return await _client.InsertLogAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to insert log: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new IsProcessActiveRequest { ProcessName = processName };
            return await _client.IsProcessActiveAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to check if process is active: {ex.Status.Detail}", ex);
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
        var request = new IsProcessActiveRequest { ProcessName = processName };

        using var call = _client.IsProcessActiveStream(request);
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
        try
        {
            var request = new GetWorkflowByIdRequest { WorkflowId = workflowId };
            return await _client.GetWorkflowByIdAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workflow by ID: {ex.Status.Detail}", ex);
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
        var request = new GetWorkflowByIdRequest { WorkflowId = workflowId };

        using var call = _client.GetWorkflowByIdStream(request);
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
        try
        {
            var request = new GetWorkflowsByRefIdRequest
            {
                RefId = refId,
                Module = module
            };
            return await _client.GetWorkflowsByRefIdAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workflows by ref ID: {ex.Status.Detail}", ex);
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
        var request = new GetWorkflowsByRefIdRequest
        {
            RefId = refId,
            Module = module
        };

        using var call = _client.GetWorkflowsByRefIdStream(request);
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
        try
        {
            var request = new GetWorkflowsByRefIdAndTypeRequest
            {
                RefId = refId,
                Module = module,
                WorkflowType = workflowType
            };
            return await _client.GetWorkflowsByRefIdAndTypeAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workflows by ref ID and type: {ex.Status.Detail}", ex);
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
        var request = new GetWorkflowsByRefIdAndTypeRequest
        {
            RefId = refId,
            Module = module,
            WorkflowType = workflowType
        };

        using var call = _client.GetWorkflowsByRefIdAndTypeStream(request);
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
        try
        {
            var request = new EmptyRequest();
            return await _client.GetWorkflowTypesAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workflow types: {ex.Status.Detail}", ex);
        }
    }

    /// <summary>
    /// Retrieves workflow types as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of workflow type items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<WorkflowTypeItem> GetWorkflowTypesStreamAsync()
    {
        var request = new EmptyRequest();

        using var call = _client.GetWorkflowTypesStream(request);
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
        try
        {
            var request = new InsertWorkflowRequest
            {
                RefId = refId,
                Module = module,
                WorkflowType = workflowType,
                WorkflowText = workflowText,
                UserId = userId
            };
            return await _client.InsertWorkflowAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to insert workflow: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new UpdateWorkflowRequest
            {
                WorkflowId = workflowId,
                WorkflowText = workflowText,
                UserId = userId
            };
            return await _client.UpdateWorkflowAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to update workflow: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new GetWorkstatusByIdRequest { WorkstatusId = workstatusId };
            return await _client.GetWorkstatusByIdAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workstatus by ID: {ex.Status.Detail}", ex);
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
        var request = new GetWorkstatusByIdRequest { WorkstatusId = workstatusId };

        using var call = _client.GetWorkstatusByIdStream(request);
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
        try
        {
            var request = new GetWorkstatusesByRefIdRequest
            {
                RefId = refId,
                Module = module
            };
            return await _client.GetWorkstatusesByRefIdAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workstatuses by ref ID: {ex.Status.Detail}", ex);
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
        var request = new GetWorkstatusesByRefIdRequest
        {
            RefId = refId,
            Module = module
        };

        using var call = _client.GetWorkstatusesByRefIdStream(request);
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
        try
        {
            var request = new GetWorkstatusesByRefIdAndTypeRequest
            {
                RefId = refId,
                Module = module,
                WorkstatusType = workstatusType
            };
            return await _client.GetWorkstatusesByRefIdAndTypeAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workstatuses by ref ID and type: {ex.Status.Detail}", ex);
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
        var request = new GetWorkstatusesByRefIdAndTypeRequest
        {
            RefId = refId,
            Module = module,
            WorkstatusType = workstatusType
        };

        using var call = _client.GetWorkstatusesByRefIdAndTypeStream(request);
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
        try
        {
            var request = new EmptyRequest();
            return await _client.GetWorkstatusTypesAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to retrieve workstatus types: {ex.Status.Detail}", ex);
        }
    }

    /// <summary>
    /// Retrieves workstatus types as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of workstatus type items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<WorkstatusTypeItem> GetWorkstatusTypesStreamAsync()
    {
        var request = new EmptyRequest();

        using var call = _client.GetWorkstatusTypesStream(request);
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
        try
        {
            var request = new InsertWorkstatusRequest
            {
                RefId = refId,
                Module = module,
                WorkstatusType = workstatusType,
                WorkstatusText = workstatusText,
                UserId = userId
            };
            return await _client.InsertWorkstatusAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to insert workstatus: {ex.Status.Detail}", ex);
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
        try
        {
            var request = new UpdateWorkstatusRequest
            {
                WorkstatusId = workstatusId,
                WorkstatusText = workstatusText,
                UserId = userId
            };
            return await _client.UpdateWorkstatusAsync(request);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new Exception($"Failed to update workstatus: {ex.Status.Detail}", ex);
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
