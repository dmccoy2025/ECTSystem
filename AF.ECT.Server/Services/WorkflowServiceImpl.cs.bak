using Grpc.Core;
using AF.ECT.Shared;
using AF.ECT.Server.Services.Interfaces;
using AF.ECT.Data.Interfaces;

namespace AF.ECT.Server.Services;

/// <summary>
/// gRPC service implementation for workflow management operations.
/// </summary>
/// <remarks>
/// This service extends the auto-generated WorkflowServiceBase class and provides
/// workflow management functionality with integration to the application's data services.
/// </remarks>
public class WorkflowServiceImpl : WorkflowService.WorkflowServiceBase
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

    #region Core User Methods

    /// <summary>
    /// Handles the GetReinvestigationRequests gRPC request.
    /// </summary>
    /// <param name="request">The request containing filter parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the reinvestigation requests Response.</returns>
    public async override Task<GetReinvestigationRequestsResponse> GetReinvestigationRequestsAsync(GetReinvestigationRequestsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting reinvestigation requests");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetReinvestigationRequestsAsync(request.UserId, request.Sarc, context?.CancellationToken ?? CancellationToken.None));

        return new GetReinvestigationRequestsResponse
        {
            Items = { results?.Select(r => new ReinvestigationRequestItem {
                Id = r.request_id,
                Description = $"{r.Member_Name ?? "Unknown"} - {r.Case_Id} ({r.Status})"
            }) ?? [] },
            Count = results?.Count ?? 0
        };
    }

    /// <summary>
    /// Handles the GetReinvestigationRequestsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing filter parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetReinvestigationRequestsStreamAsync(GetReinvestigationRequestsRequest request, IServerStreamWriter<ReinvestigationRequestItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming reinvestigation requests");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetReinvestigationRequestsAsync(request.UserId, request.Sarc, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new ReinvestigationRequestItem
                {
                    Id = item.request_id,
                    Description = $"{item.Member_Name ?? "Unknown"} - {item.Case_Id} ({item.Status})"
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetMailingListForLOD gRPC request.
    /// </summary>
    /// <param name="request">The request containing mailing list parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the mailing list Response.</returns>
    public async override Task<GetMailingListForLODResponse> GetMailingListForLODAsync(GetMailingListForLODRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting mailing list for LOD");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetMailingListForLODAsync(request, context?.CancellationToken ?? CancellationToken.None));

        return new GetMailingListForLODResponse
        {
            Items = { results?.Select((r, index) => new MailingListItem { Id = index, Email = r.Email ?? string.Empty, Name = string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetMailingListForLODStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing mailing list parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetMailingListForLODStreamAsync(GetMailingListForLODRequest request, IServerStreamWriter<MailingListItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming mailing list for LOD");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetMailingListForLODAsync(request, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            for (var i = 0; i < results.Count; i++)
            {
                await responseStream.WriteAsync(new MailingListItem
                {
                    Id = i,
                    Email = results[i].Email ?? string.Empty,
                    Name = string.Empty
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetManagedUsers gRPC request.
    /// </summary>
    /// <param name="request">The request containing managed users parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the managed users Response.</returns>
    public async override Task<GetManagedUsersResponse> GetManagedUsersAsync(GetManagedUsersRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting managed users");
        
        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetManagedUsersAsync(request, context?.CancellationToken ?? CancellationToken.None));

        return new GetManagedUsersResponse
        {
            Items = { results?.Select(r => new ManagedUserItem { UserId = r.Id, UserName = r.username ?? string.Empty, Email = string.Empty, Status = r.Status }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetManagedUsersStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing managed users parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetManagedUsersStreamAsync(GetManagedUsersRequest request, IServerStreamWriter<ManagedUserItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming managed users");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetManagedUsersAsync(request, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new ManagedUserItem
                {
                    UserId = item.Id,
                    UserName = item.username ?? string.Empty,
                    Email = string.Empty,
                    Status = item.Status
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetMembersUserId gRPC request.
    /// </summary>
    /// <param name="request">The request containing member SSN.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the member user ID Response.</returns>
    public async override Task<GetMembersUserIdResponse> GetMembersUserIdAsync(GetMembersUserIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Getting member user ID for SSN: {request.MemberSsn}");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetMembersUserIdAsync(request.MemberSsn, context?.CancellationToken ?? CancellationToken.None));

        return new GetMembersUserIdResponse
        {
            UserId = result
        };
    }

    /// <summary>
    /// Handles the GetUserAltTitle gRPC request.
    /// </summary>
    /// <param name="request">The request containing user alternate title parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate title Response.</returns>
    public async override Task<GetUserAltTitleResponse> GetUserAltTitleAsync(GetUserAltTitleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting user alternate title");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserAltTitleAsync(request.UserId, request.GroupId, context?.CancellationToken ?? CancellationToken.None));

        return new GetUserAltTitleResponse
        {
            Items = { results?.Select(r => new UserAltTitleItem { UserId = request.UserId, Title = r.Title ?? string.Empty, GroupId = request.GroupId }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetUserAltTitleStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing user alternate title parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetUserAltTitleStreamAsync(GetUserAltTitleRequest request, IServerStreamWriter<UserAltTitleItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming user alternate title");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserAltTitleAsync(request.UserId, request.GroupId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new UserAltTitleItem
                {
                    UserId = request.UserId,
                    Title = item.Title ?? string.Empty,
                    GroupId = request.GroupId
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetUserAltTitleByGroupCompo gRPC request.
    /// </summary>
    /// <param name="request">The request containing user alternate title by group component parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate title by group component Response.</returns>
    public async override Task<GetUserAltTitleByGroupCompoResponse> GetUserAltTitleByGroupCompoAsync(GetUserAltTitleByGroupCompoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting user alternate title by group component");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserAltTitleByGroupCompoAsync(request.GroupId, request.WorkCompo, context?.CancellationToken ?? CancellationToken.None));

        return new GetUserAltTitleByGroupCompoResponse
        {
            Items = { results?.Select(r => new UserAltTitleByGroupCompoItem { UserId = r.userID, Title = r.Title ?? string.Empty, Component = r.Name ?? string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetUserAltTitleByGroupCompoStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing user alternate title by group component parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetUserAltTitleByGroupCompoStreamAsync(GetUserAltTitleByGroupCompoRequest request, IServerStreamWriter<UserAltTitleByGroupCompoItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming user alternate title by group component");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserAltTitleByGroupCompoAsync(request.GroupId, request.WorkCompo, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new UserAltTitleByGroupCompoItem
                {
                    UserId = item.userID,
                    Title = item.Title ?? string.Empty,
                    Component = item.Name ?? string.Empty
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetUserName gRPC request.
    /// </summary>
    /// <param name="request">The request containing user name parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user name Response.</returns>
    public async override Task<GetUserNameResponse> GetUserNameAsync(GetUserNameRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting user name");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserNameAsync(request.First, request.Last, context?.CancellationToken ?? CancellationToken.None));

        return new GetUserNameResponse
        {
            Items = { results?.Select(r => new UserNameItem { UserId = r.UserId, FirstName = r.FirstName, LastName = r.LastName, FullName = r.FullName }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetUserNameStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing user name parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetUserNameStreamAsync(GetUserNameRequest request, IServerStreamWriter<UserNameItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming user name");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserNameAsync(request.First, request.Last, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new UserNameItem
                {
                    UserId = 0,
                    FirstName = "First",
                    LastName = "Last",
                    FullName = "Full Name"
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetUsersAltTitleByGroup gRPC request.
    /// </summary>
    /// <param name="request">The request containing users alternate title by group parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the users alternate title by group Response.</returns>
    public async override Task<GetUsersAltTitleByGroupResponse> GetUsersAltTitleByGroupAsync(GetUsersAltTitleByGroupRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting users alternate title by group");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUsersAltTitleByGroupAsync(request.GroupId, context?.CancellationToken ?? CancellationToken.None));

        return new GetUsersAltTitleByGroupResponse
        {
            Items = { results?.Select(r => new UsersAltTitleByGroupItem { UserId = r.userID, Title = r.Title ?? string.Empty, GroupId = request.GroupId }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetUsersAltTitleByGroupStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing users alternate title by group parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetUsersAltTitleByGroupStreamAsync(GetUsersAltTitleByGroupRequest request, IServerStreamWriter<UsersAltTitleByGroupItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming users alternate title by group");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUsersAltTitleByGroupAsync(request.GroupId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new UsersAltTitleByGroupItem
                {
                    UserId = item.userID,
                    Title = item.Title ?? string.Empty,
                    GroupId = request.GroupId
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetUsersOnline gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the users online Response.</returns>
    public async override Task<GetUsersOnlineResponse> GetUsersOnlineAsync(EmptyRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting users online");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUsersOnlineAsync(context?.CancellationToken ?? CancellationToken.None));

        return new GetUsersOnlineResponse
        {
            Items = { results?.Select(r => new UserOnlineItem { UserId = r.userId, UserName = r.UserName ?? string.Empty, LastActivity = r.loginTime.ToString("yyyy-MM-dd") }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetUsersOnlineStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetUsersOnlineStreamAsync(EmptyRequest request, IServerStreamWriter<UserOnlineItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming users online");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUsersOnlineAsync(context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new UserOnlineItem
                {
                    UserId = item.userId,
                    UserName = item.UserName ?? string.Empty,
                    LastActivity = item.loginTime.ToString("yyyy-MM-dd")
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetWhoisStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing WHOIS parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWhoisStreamAsync(GetWhoisRequest request, IServerStreamWriter<WhoisItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming WHOIS information");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWhoisAsync(request.UserId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WhoisItem { UserId = item.UserId, UserName = $"{item.FirstName ?? ""} {item.LastName ?? ""}".Trim(), IpAddress = item.Role ?? string.Empty, LastLogin = string.Empty });
            }
        }
    }

    /// <summary>
    /// Handles the HasHQTechAccountStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing HQ tech account parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task HasHQTechAccountStreamAsync(HasHQTechAccountRequest request, IServerStreamWriter<HQTechAccountItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming HQ tech account check");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.HasHQTechAccountAsync(request.OriginUserId, request.UserEdipin, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new HQTechAccountItem { UserId = 0, HasAccount = true, AccountType = "Type" });
            }
        }
    }

    /// <summary>
    /// Handles the IsFinalStatusCodeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing final status code parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task IsFinalStatusCodeStreamAsync(IsFinalStatusCodeRequest request, IServerStreamWriter<FinalStatusCodeItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming final status code check");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.IsFinalStatusCodeAsync((byte?)request.StatusId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new FinalStatusCodeItem { StatusId = (int)request.StatusId, IsFinal = item.isFinal, Description = string.Empty });
            }
        }
    }

    /// <summary>
    /// Handles the UpdateLoginStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing login update parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task UpdateLoginStreamAsync(UpdateLoginRequest request, IServerStreamWriter<LoginUpdateItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming login update");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateLoginAsync(request.UserId, request.SessionId, request.RemoteAddr, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new LoginUpdateItem { UserId = 0, SessionId = "session123", LoginTime = "2023-01-01" });
            }
        }
    }

    /// <summary>
    /// Handles the GetWhois gRPC request.
    /// </summary>
    /// <param name="request">The request containing WHOIS parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the WHOIS Response.</returns>
    public async override Task<GetWhoisResponse> GetWhoisAsync(GetWhoisRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting WHOIS information");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWhoisAsync(request.UserId, context?.CancellationToken ?? CancellationToken.None));

        return new GetWhoisResponse
        {
            Items = { results?.Select(r => new WhoisItem { UserId = r.UserId, UserName = $"{r.FirstName ?? ""} {r.LastName ?? ""}".Trim(), IpAddress = r.Role ?? string.Empty, LastLogin = string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the HasHQTechAccount gRPC request.
    /// </summary>
    /// <param name="request">The request containing HQ tech account parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the HQ tech account Response.</returns>
    public async override Task<HasHQTechAccountResponse> HasHQTechAccountAsync(HasHQTechAccountRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Checking HQ tech account");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.HasHQTechAccountAsync(request.OriginUserId, request.UserEdipin, context?.CancellationToken ?? CancellationToken.None));

        return new HasHQTechAccountResponse
        {
            Items = { results?.Select(r => new HQTechAccountItem { UserId = 0, HasAccount = true, AccountType = "Type" }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the IsFinalStatusCode gRPC request.
    /// </summary>
    /// <param name="request">The request containing final status code parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the final status code Response.</returns>
    public async override Task<IsFinalStatusCodeResponse> IsFinalStatusCodeAsync(IsFinalStatusCodeRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Checking if status code is final");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.IsFinalStatusCodeAsync((byte?)request.StatusId, context?.CancellationToken ?? CancellationToken.None));

        return new IsFinalStatusCodeResponse
        {
            Items = { results?.Select(r => new FinalStatusCodeItem { StatusId = (int)request.StatusId, IsFinal = r.isFinal, Description = string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the Logout gRPC request.
    /// </summary>
    /// <param name="request">The request containing logout parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the logout Response.</returns>
    public async override Task<LogoutResponse> LogoutAsync(LogoutRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Logging out user: {request.UserId}");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.LogoutAsync(request.UserId, context?.CancellationToken ?? CancellationToken.None));

        return new LogoutResponse
        {
            Result = result
        };
    }

    /// <summary>
    /// Handles the RegisterUser gRPC request.
    /// </summary>
    /// <param name="request">The request containing user registration parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user registration Response.</returns>
    public async override Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Registering user");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.RegisterUserAsync(request, context?.CancellationToken ?? CancellationToken.None));

        return new RegisterUserResponse
        {
            Result = result
        };
    }

    /// <summary>
    /// Handles the RegisterUserRole gRPC request.
    /// </summary>
    /// <param name="request">The request containing user role registration parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user role registration Response.</returns>
    public async override Task<RegisterUserRoleResponse> RegisterUserRoleAsync(RegisterUserRoleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Registering user role");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.RegisterUserRoleAsync(request.UserId, (short?)request.GroupId, (byte?)request.Status, context?.CancellationToken ?? CancellationToken.None));

        return new RegisterUserRoleResponse
        {
            UserRoleId = result
        };
    }

    /// <summary>
    /// Handles the SearchMemberData gRPC request.
    /// </summary>
    /// <param name="request">The request containing member data search parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the member data search Response.</returns>
    public async override Task<SearchMemberDataResponse> SearchMemberDataAsync(SearchMemberDataRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Searching member data");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.SearchMemberDataAsync(request, context?.CancellationToken ?? CancellationToken.None));

        return new SearchMemberDataResponse
        {
            Items = { results?.Select(r => new MemberDataItem { MemberId = r.Id, Ssn = r.SSAN ?? string.Empty, FirstName = r.FirstName ?? string.Empty, LastName = r.LastName ?? string.Empty, MiddleName = r.MiddleName ?? string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the SearchMemberDataStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing member data search parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task SearchMemberDataStreamAsync(SearchMemberDataRequest request, IServerStreamWriter<MemberDataItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming member data search");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.SearchMemberDataAsync(request, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new MemberDataItem
                {
                    MemberId = item.Id,
                    Ssn = item.SSAN ?? string.Empty,
                    FirstName = item.FirstName ?? string.Empty,
                    LastName = item.LastName ?? string.Empty,
                    MiddleName = item.MiddleName ?? string.Empty
                });
            }
        }
    }

    /// <summary>
    /// Handles the SearchMemberDataTest gRPC request.
    /// </summary>
    /// <param name="request">The request containing member data test search parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the member data test search Response.</returns>
    public async override Task<SearchMemberDataTestResponse> SearchMemberDataTestAsync(SearchMemberDataTestRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Searching member data (test version)");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.SearchMemberDataTestAsync(request, context?.CancellationToken ?? CancellationToken.None));

        return new SearchMemberDataTestResponse
        {
            Items = { results?.Select(r => new MemberDataTestItem { MemberId = r.Id, Ssn = r.SSAN ?? string.Empty, Name = $"{r.FirstName ?? string.Empty} {r.LastName ?? string.Empty}".Trim() }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the SearchMemberDataTestStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing member data test search parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task SearchMemberDataTestStreamAsync(SearchMemberDataTestRequest request, IServerStreamWriter<MemberDataTestItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming member data test search");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.SearchMemberDataTestAsync(request, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new MemberDataTestItem
                {
                    MemberId = item.Id,
                    Ssn = item.SSAN ?? string.Empty,
                    Name = $"{item.FirstName ?? string.Empty} {item.LastName ?? string.Empty}".Trim()
                });
            }
        }
    }

    /// <summary>
    /// Handles the UpdateAccountStatus gRPC request.
    /// </summary>
    /// <param name="request">The request containing account status update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the account status update Response.</returns>
    public async override Task<UpdateAccountStatusResponse> UpdateAccountStatusAsync(UpdateAccountStatusRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Updating account status");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateAccountStatusAsync(request, context?.CancellationToken ?? CancellationToken.None));

        return new UpdateAccountStatusResponse
        {
            Result = result
        };
    }

    /// <summary>
    /// Handles the UpdateLogin gRPC request.
    /// </summary>
    /// <param name="request">The request containing login update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the login update Response.</returns>
    public async override Task<UpdateLoginResponse> UpdateLoginAsync(UpdateLoginRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Updating login information");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateLoginAsync(request.UserId, request.SessionId, request.RemoteAddr, context?.CancellationToken ?? CancellationToken.None));

        return new UpdateLoginResponse
        {
            Items = { results?.Select(r => new LoginUpdateItem { UserId = 0, SessionId = "session123", LoginTime = "2023-01-01" }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the UpdateManagedSettings gRPC request.
    /// </summary>
    /// <param name="request">The request containing managed settings update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the managed settings update Response.</returns>
    public async override Task<UpdateManagedSettingsResponse> UpdateManagedSettingsAsync(UpdateManagedSettingsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Updating managed settings");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateManagedSettingsAsync(request, context?.CancellationToken ?? CancellationToken.None));

        return new UpdateManagedSettingsResponse
        {
            Result = result
        };
    }

    /// <summary>
    /// Handles the UpdateUserAltTitle gRPC request.
    /// </summary>
    /// <param name="request">The request containing user alternate title update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate title update Response.</returns>
    public async override Task<UpdateUserAltTitleResponse> UpdateUserAltTitleAsync(UpdateUserAltTitleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Updating user alternate title");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateUserAltTitleAsync(request.UserId, request.GroupId, request.NewTitle, context?.CancellationToken ?? CancellationToken.None));

        return new UpdateUserAltTitleResponse
        {
            Result = result
        };
    }

    #endregion

    #region Core Workflow Methods

    /// <summary>
    /// Handles the AddSignature gRPC request.
    /// </summary>
    /// <param name="request">The request containing signature addition parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the signature addition Response.</returns>
    public async override Task<AddSignatureResponse> AddSignatureAsync(AddSignatureRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Adding signature");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.AddSignatureAsync(request, context?.CancellationToken ?? CancellationToken.None));

        return new AddSignatureResponse
        {
            Items = { results?.Select(r => new SignatureItem { SignatureId = 0, RefId = 0, UserId = 0, SignatureDate = "2023-01-01" }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the CopyActions gRPC request.
    /// </summary>
    /// <param name="request">The request containing action copy parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the action copy Response.</returns>
    public async override Task<CopyActionsResponse> CopyActionsAsync(CopyActionsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Copying actions");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.CopyActionsAsync(request.DestWsoid, request.SrcWsoid, context?.CancellationToken ?? CancellationToken.None));

        return new CopyActionsResponse
        {
            Result = result
        };
    }

    /// <summary>
    /// Handles the CopyRules gRPC request.
    /// </summary>
    /// <param name="request">The request containing rule copy parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the rule copy Response.</returns>
    public async override Task<CopyRulesResponse> CopyRulesAsync(CopyRulesRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Copying rules");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.CopyRulesAsync(request.DestWsoid, request.SrcWsoid, context?.CancellationToken ?? CancellationToken.None));

        return new CopyRulesResponse
        {
            Result = result
        };
    }

    /// <summary>
    /// Handles the CopyWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow copy parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow copy Response.</returns>
    public async override Task<CopyWorkflowResponse> CopyWorkflowAsync(CopyWorkflowRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Copying workflow");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.CopyWorkflowAsync(request.FromId, request.ToId, context?.CancellationToken ?? CancellationToken.None));

        return new CopyWorkflowResponse
        {
            Items = { results?.Select(r => new WorkflowCopyItem { WorkflowId = 0, WorkflowName = "Workflow", CopySuccess = true }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the DeleteStatusCode gRPC request.
    /// </summary>
    /// <param name="request">The request containing status code deletion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status code deletion Response.</returns>
    public async override Task<DeleteStatusCodeResponse> DeleteStatusCodeAsync(DeleteStatusCodeRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Deleting status code");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.DeleteStatusCodeAsync(request.StatusId, context?.CancellationToken ?? CancellationToken.None));

        return new DeleteStatusCodeResponse
        {
            Result = result
        };
    }

    /// <summary>
    /// Handles the GetActionsByStep gRPC request.
    /// </summary>
    /// <param name="request">The request containing actions by step parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the actions by step Response.</returns>
    public async override Task<GetActionsByStepResponse> GetActionsByStepAsync(GetActionsByStepRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting actions by step");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetActionsByStepAsync(request.StepId, context?.CancellationToken ?? CancellationToken.None));

        return new GetActionsByStepResponse
        {
            Items = { results?.Select(r => new ActionByStepItem { ActionId = r.wsa_id, StepId = r.wso_id, ActionType = r.actionType.ToString(), ActionDescription = r.text ?? string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetActiveCases gRPC request.
    /// </summary>
    /// <param name="request">The request containing active cases parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the active cases Response.</returns>
    public async override Task<GetActiveCasesResponse> GetActiveCasesAsync(GetActiveCasesRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting active cases");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetActiveCasesAsync(request.RefId, (short?)request.GroupId, context?.CancellationToken ?? CancellationToken.None));

        return new GetActiveCasesResponse
        {
            Items = { results?.Select(r => new ActiveCaseItem { CaseId = 0, RefId = 0, GroupId = 0, Status = "Status" }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetAllFindingByReasonOf gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the all findings by reason of Response.</returns>
    public async override Task<GetAllFindingByReasonOfResponse> GetAllFindingByReasonOfAsync(EmptyRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting all findings by reason of");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetAllFindingByReasonOfAsync(context?.CancellationToken ?? CancellationToken.None));

        return new GetAllFindingByReasonOfResponse
        {
            Items = { results?.Select(r => new FindingByReasonOfItem { FindingId = r.Id, Reason = string.Empty, Description = r.Description ?? string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetAllLocks gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the all locks Response.</returns>
    public async override Task<GetAllLocksResponse> GetAllLocksAsync(EmptyRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting all locks");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetAllLocksAsync(context?.CancellationToken ?? CancellationToken.None));

        return new GetAllLocksResponse
        {
            Items = { results?.Select(r => new LockItem { LockId = r.lockId, UserId = r.userId, LockType = r.moduleName ?? string.Empty, LockTime = r.lockTime.ToString("yyyy-MM-dd") }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetCancelReasons gRPC request.
    /// </summary>
    /// <param name="request">The request containing cancel reasons parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the cancel reasons Response.</returns>
    public async override Task<GetCancelReasonsResponse> GetCancelReasonsAsync(GetCancelReasonsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting cancel reasons");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetCancelReasonsAsync((byte?)request.WorkflowId, request.IsFormal, context?.CancellationToken ?? CancellationToken.None));

        return new GetCancelReasonsResponse
        {
            Items = { results?.Select(r => new CancelReasonItem { ReasonId = r.Id, ReasonText = r.Description ?? string.Empty, IsFormal = request.IsFormal }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetCreatableByGroup gRPC request.
    /// </summary>
    /// <param name="request">The request containing creatable by group parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the creatable by group Response.</returns>
    public async override Task<GetCreatableByGroupResponse> GetCreatableByGroupAsync(GetCreatableByGroupRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting creatable by group");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetCreatableByGroupAsync(request.Compo, (byte?)request.Module, (byte?)request.GroupId, context?.CancellationToken ?? CancellationToken.None));

        return new GetCreatableByGroupResponse
        {
            Items = { results?.Select(r => new CreatableByGroupItem { WorkflowId = r.workFlowId, WorkflowName = r.title ?? string.Empty, GroupId = request.GroupId }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetFindingByReasonOfById gRPC request.
    /// </summary>
    /// <param name="request">The request containing finding by reason of by ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the finding by reason of by ID Response.</returns>
    public async override Task<GetFindingByReasonOfByIdResponse> GetFindingByReasonOfByIdAsync(GetFindingByReasonOfByIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting finding by reason of by ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetFindingByReasonOfByIdAsync(request.Id, context?.CancellationToken ?? CancellationToken.None));

        return new GetFindingByReasonOfByIdResponse
        {
            Items = { results?.Select(r => new FindingByReasonOfByIdItem { FindingId = r.Id, Reason = string.Empty, Description = r.Description ?? string.Empty, Id = r.Id }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetFindings gRPC request.
    /// </summary>
    /// <param name="request">The request containing findings parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the findings Response.</returns>
    public async override Task<GetFindingsResponse> GetFindingsAsync(GetFindingsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting findings");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetFindingsAsync((byte?)request.WorkflowId, request.GroupId, context?.CancellationToken ?? CancellationToken.None));

        return new GetFindingsResponse
        {
            Items = { results?.Select(r => new FindingItem { FindingId = r.Id ?? 0, WorkflowId = request.WorkflowId, GroupId = request.GroupId, FindingText = r.Description ?? string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetModuleFromWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing module from workflow parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the module from workflow Response.</returns>
    public async override Task<GetModuleFromWorkflowResponse> GetModuleFromWorkflowAsync(GetModuleFromWorkflowRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting module from workflow");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetModuleFromWorkflowAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        return new GetModuleFromWorkflowResponse
        {
            Items = { results?.Select(r => new ModuleFromWorkflowItem { ModuleId = 0, ModuleName = "Module", WorkflowId = 0 }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetPageAccessByGroup gRPC request.
    /// </summary>
    /// <param name="request">The request containing page access by group parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the page access by group Response.</returns>
    public async override Task<GetPageAccessByGroupResponse> GetPageAccessByGroupAsync(GetPageAccessByGroupRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting page access by group");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPageAccessByGroupAsync((byte?)request.Workflow, request.Status, (byte?)request.Group, context?.CancellationToken ?? CancellationToken.None));

        return new GetPageAccessByGroupResponse
        {
            Items = { results?.Select(r => new PageAccessByGroupItem { PageId = r.PageId, PageName = r.PageTitle ?? string.Empty, HasAccess = r.Access != 0, GroupId = r.GroupId ?? 0 }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetPageAccessByWorkflowView gRPC request.
    /// </summary>
    /// <param name="request">The request containing page access by workflow view parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the page access by workflow view Response.</returns>
    public async override Task<GetPageAccessByWorkflowViewResponse> GetPageAccessByWorkflowViewAsync(GetPageAccessByWorkflowViewRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting page access by workflow view");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPageAccessByWorkflowViewAsync(request.Compo, (byte?)request.Workflow, request.Status, context?.CancellationToken ?? CancellationToken.None));

        return new GetPageAccessByWorkflowViewResponse
        {
            Items = { results?.Select(r => new PageAccessByWorkflowViewItem { PageId = r.PageId, PageName = r.PageTitle ?? string.Empty, HasAccess = r.Access != 0, Component = request.Compo }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetPagesByWorkflowId gRPC request.
    /// </summary>
    /// <param name="request">The request containing pages by workflow ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the pages by workflow ID Response.</returns>
    public async override Task<GetPagesByWorkflowIdResponse> GetPagesByWorkflowIdAsync(GetPagesByWorkflowIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting pages by workflow ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPagesByWorkflowIdAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        return new GetPagesByWorkflowIdResponse
        {
            Items = { results?.Select(r => new PageByWorkflowItem { PageId = r.pageId, PageName = r.title ?? string.Empty, WorkflowId = request.WorkflowId, PageUrl = "/page" }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetPermissions gRPC request.
    /// </summary>
    /// <param name="request">The request containing permissions parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the permissions Response.</returns>
    public async override Task<GetPermissionsResponse> GetPermissionsAsync(GetPermissionsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting permissions");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPermissionsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        return new GetPermissionsResponse
        {
            Items = { results?.Select(r => new PermissionItem { PermissionId = r.groupId, PermissionName = r.name ?? string.Empty, WorkflowId = request.WorkflowId, IsGranted = r.canView ?? false }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetPermissionsByCompo gRPC request.
    /// </summary>
    /// <param name="request">The request containing permissions by component parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the permissions by component Response.</returns>
    public async override Task<GetPermissionsByCompoResponse> GetPermissionsByCompoAsync(GetPermissionsByCompoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting permissions by component");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPermissionsByCompoAsync((byte?)request.WorkflowId, request.Compo, context?.CancellationToken ?? CancellationToken.None));

        return new GetPermissionsByCompoResponse
        {
            Items = { results?.Select(r => new PermissionByCompoItem { PermissionId = r.groupId, PermissionName = r.name ?? string.Empty, Component = request.Compo, IsGranted = r.canView ?? false }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetReturnReasons gRPC request.
    /// </summary>
    /// <param name="request">The request containing return reasons parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the return reasons Response.</returns>
    public async override Task<GetReturnReasonsResponse> GetReturnReasonsAsync(GetReturnReasonsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting return reasons");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetReturnReasonsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        return new GetReturnReasonsResponse
        {
            Items = { results?.Select(r => new ReturnReasonItem { ReasonId = r.Id, ReasonText = r.Description ?? string.Empty, WorkflowId = request.WorkflowId }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetRwoaReasons gRPC request.
    /// </summary>
    /// <param name="request">The request containing RWOA reasons parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the RWOA reasons Response.</returns>
    public async override Task<GetRwoaReasonsResponse> GetRwoaReasonsAsync(GetRwoaReasonsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting RWOA reasons");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetRwoaReasonsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        return new GetRwoaReasonsResponse
        {
            Items = { results?.Select(r => new RwoaReasonItem { ReasonId = r.Id, ReasonText = r.Description ?? string.Empty, WorkflowId = request.WorkflowId }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetStatusCodesByCompo gRPC request.
    /// </summary>
    /// <param name="request">The request containing status codes by component parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by component Response.</returns>
    public async override Task<GetStatusCodesByCompoResponse> GetStatusCodesByCompoAsync(GetStatusCodesByCompoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting status codes by component");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByCompoAsync(request.Compo, context?.CancellationToken ?? CancellationToken.None));

        return new GetStatusCodesByCompoResponse
        {
            Items = { results?.Select(r => new StatusCodeByCompoItem { StatusId = r.statusId, StatusName = r.description ?? string.Empty, Component = r.compo }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetStatusCodesByCompoAndModule gRPC request.
    /// </summary>
    /// <param name="request">The request containing status codes by component and module parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by component and module Response.</returns>
    public async override Task<GetStatusCodesByCompoAndModuleResponse> GetStatusCodesByCompoAndModuleAsync(GetStatusCodesByCompoAndModuleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting status codes by component and module");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByCompoAndModuleAsync(request.Compo, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

        return new GetStatusCodesByCompoAndModuleResponse
        {
            Items = { results?.Select(r => new StatusCodeByCompoAndModuleItem { StatusId = r.statusId, StatusName = r.description ?? string.Empty, Component = r.compo, ModuleId = r.moduleId }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetStatusCodesBySignCode gRPC request.
    /// </summary>
    /// <param name="request">The request containing status codes by sign code parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by sign code Response.</returns>
    public async override Task<GetStatusCodesBySignCodeResponse> GetStatusCodesBySignCodeAsync(GetStatusCodesBySignCodeRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting status codes by sign code");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesBySignCodeAsync((short?)request.GroupId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

        return new GetStatusCodesBySignCodeResponse
        {
            Items = { results?.Select(r => new StatusCodeBySignCodeItem { StatusId = r.statusId, StatusName = r.description ?? string.Empty, GroupId = request.GroupId, ModuleId = request.Module }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetStatusCodesByWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing status codes by workflow parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by workflow Response.</returns>
    public async override Task<GetStatusCodesByWorkflowResponse> GetStatusCodesByWorkflowAsync(GetStatusCodesByWorkflowRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting status codes by workflow");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByWorkflowAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        return new GetStatusCodesByWorkflowResponse
        {
            Items = { results?.Select(r => new StatusCodeByWorkflowItem { StatusId = 0, StatusName = "Status", WorkflowId = 0 }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetStatusCodesByWorkflowAndAccessScope gRPC request.
    /// </summary>
    /// <param name="request">The request containing status codes by workflow and access scope parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by workflow and access scope Response.</returns>
    public async override Task<GetStatusCodesByWorkflowAndAccessScopeResponse> GetStatusCodesByWorkflowAndAccessScopeAsync(GetStatusCodesByWorkflowAndAccessScopeRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting status codes by workflow and access scope");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByWorkflowAndAccessScopeAsync((byte?)request.WorkflowId, (byte?)request.AccessScope, context?.CancellationToken ?? CancellationToken.None));

        return new GetStatusCodesByWorkflowAndAccessScopeResponse
        {
            Items = { results?.Select(r => new StatusCodeByWorkflowAndAccessScopeItem { StatusId = 0, StatusName = "Status", WorkflowId = 0, AccessScope = 0 }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetStatusCodeScope gRPC request.
    /// </summary>
    /// <param name="request">The request containing status code scope parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the status code scope Response.</returns>
    public async override Task<GetStatusCodeScopeResponse> GetStatusCodeScopeAsync(GetStatusCodeScopeRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting status code scope");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodeScopeAsync((byte?)request.StatusId, context?.CancellationToken ?? CancellationToken.None));

        return new GetStatusCodeScopeResponse
        {
            Items = { results?.Select(r => new StatusCodeScopeItem { StatusId = request.StatusId, ScopeName = r.accessScope.ToString(), Description = "Access Scope" }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetStepsByWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing steps by workflow parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the steps by workflow Response.</returns>
    public async override Task<GetStepsByWorkflowResponse> GetStepsByWorkflowAsync(GetStepsByWorkflowRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting steps by workflow");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStepsByWorkflowAsync((byte?)request.Workflow, context?.CancellationToken ?? CancellationToken.None));

        return new GetStepsByWorkflowResponse
        {
            Items = { results?.Select(r => new StepByWorkflowItem { StepId = 0, StepName = "Step", WorkflowId = 0, StepOrder = 0 }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetStepsByWorkflowAndStatus gRPC request.
    /// </summary>
    /// <param name="request">The request containing steps by workflow and status parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the steps by workflow and status Response.</returns>
    public async override Task<GetStepsByWorkflowAndStatusResponse> GetStepsByWorkflowAndStatusAsync(GetStepsByWorkflowAndStatusRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting steps by workflow and status");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStepsByWorkflowAndStatusAsync((byte?)request.Workflow, (byte?)request.Status, request.DeathStatus, context?.CancellationToken ?? CancellationToken.None));

        return new GetStepsByWorkflowAndStatusResponse
        {
            Items = { results?.Select(r => new StepByWorkflowAndStatusItem { StepId = 0, StepName = "Step", WorkflowId = 0, StatusId = 0, DeathStatus = "Status" }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetViewableByGroup gRPC request.
    /// </summary>
    /// <param name="request">The request containing viewable by group parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the viewable by group Response.</returns>
    public async override Task<GetViewableByGroupResponse> GetViewableByGroupAsync(GetViewableByGroupRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting viewable by group");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetViewableByGroupAsync((byte?)request.GroupId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

        return new GetViewableByGroupResponse
        {
            Items = { results?.Select(r => new ViewableByGroupItem { WorkflowId = r.workFlowId, WorkflowName = r.title, GroupId = request.GroupId, IsViewable = r.active }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkflowByCompo gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow by component parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow by component Response.</returns>
    public async override Task<GetWorkflowByCompoResponse> GetWorkflowByCompoAsync(GetWorkflowByCompoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workflow by component");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowByCompoAsync(request.Compo, request.UserId, context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkflowByCompoResponse
        {
            Items = { results?.Select(r => new WorkflowByCompoItem { WorkflowId = r.workflowId, WorkflowName = r.title ?? string.Empty, Component = request.Compo, UserId = request.UserId }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkflowFromModule gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow from module parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow from module Response.</returns>
    public async override Task<GetWorkflowFromModuleResponse> GetWorkflowFromModuleAsync(GetWorkflowFromModuleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workflow from module");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowFromModuleAsync(request.ModuleId, context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkflowFromModuleResponse
        {
            Items = { results?.Select(r => new WorkflowFromModuleItem { WorkflowId = r.workflowId ?? 0, WorkflowName = "Workflow", ModuleId = request.ModuleId }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkflowInitialStatusCode gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow initial status code parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow initial status code Response.</returns>
    public async override Task<GetWorkflowInitialStatusCodeResponse> GetWorkflowInitialStatusCodeAsync(GetWorkflowInitialStatusCodeRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workflow initial status code");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowInitialStatusCodeAsync(request.Compo, request.Module, request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkflowInitialStatusCodeResponse
        {
            Items = { results?.Select(r => new WorkflowInitialStatusCodeItem { WorkflowId = request.WorkflowId, InitialStatusId = r.statusId ?? 0, StatusName = "Status" }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkflowTitle gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow title parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow title Response.</returns>
    public async override Task<GetWorkflowTitleResponse> GetWorkflowTitleAsync(GetWorkflowTitleRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workflow title");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTitleAsync(request.ModuleId, request.SubCase, context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkflowTitleResponse
        {
            Items = { results?.Select(r => new WorkflowTitleItem { WorkflowId = 0, Title = "Title", ModuleId = 0, SubCase = 0 }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkflowTitleByWorkStatusId gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow title by work status ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow title by work status ID Response.</returns>
    public async override Task<GetWorkflowTitleByWorkStatusIdResponse> GetWorkflowTitleByWorkStatusIdAsync(GetWorkflowTitleByWorkStatusIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workflow title by work status ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTitleByWorkStatusIdAsync(request.WorkflowId, request.SubCase, context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkflowTitleByWorkStatusIdResponse
        {
            Items = { results?.Select(r => new WorkflowTitleByWorkStatusIdItem { WorkflowId = 0, Title = "Title", WorkStatusId = 0, SubCase = 0 }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the InsertAction gRPC request.
    /// </summary>
    /// <param name="request">The request containing action insertion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the action insertion Response.</returns>
    public async override Task<InsertActionResponse> InsertActionAsync(InsertActionRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Inserting action");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertActionAsync(request, context?.CancellationToken ?? CancellationToken.None));

        return new InsertActionResponse
        {
            Items = { results?.Select(r => new InsertActionItem { ActionId = 0, Type = 0, StepId = 0, ResultMessage = "Success" }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the InsertOptionAction gRPC request.
    /// </summary>
    /// <param name="request">The request containing option action insertion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the option action insertion Response.</returns>
    public async override Task<InsertOptionActionResponse> InsertOptionActionAsync(InsertOptionActionRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Inserting option action");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertOptionActionAsync(request, context?.CancellationToken ?? CancellationToken.None));

        return new InsertOptionActionResponse
        {
            Items = { results?.Select(r => new InsertOptionActionItem { ActionId = 0, Type = 0, Wsoid = 0, ResultMessage = "Success" }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the AddSignatureStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing signature addition parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task AddSignatureStreamAsync(AddSignatureRequest request, IServerStreamWriter<SignatureItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming signature addition");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.AddSignatureAsync(request, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new SignatureItem { SignatureId = 0, RefId = 0, UserId = 0, SignatureDate = "2023-01-01" });
            }
        }
    }

    /// <summary>
    /// Handles the CopyWorkflowStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow copy parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task CopyWorkflowStreamAsync(CopyWorkflowRequest request, IServerStreamWriter<WorkflowCopyItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workflow copy");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.CopyWorkflowAsync(request.FromId, request.ToId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkflowCopyItem { WorkflowId = 0, WorkflowName = "Workflow", CopySuccess = true });
            }
        }
    }

    /// <summary>
    /// Handles the GetActionsByStepStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing actions by step parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetActionsByStepStreamAsync(GetActionsByStepRequest request, IServerStreamWriter<ActionByStepItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming actions by step");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetActionsByStepAsync(request.StepId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new ActionByStepItem { ActionId = item.wsa_id, StepId = item.wso_id, ActionType = item.actionType.ToString(), ActionDescription = item.text ?? string.Empty });
            }
        }
    }

    /// <summary>
    /// Handles the GetActiveCasesStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing active cases parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetActiveCasesStreamAsync(GetActiveCasesRequest request, IServerStreamWriter<ActiveCaseItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming active cases");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetActiveCasesAsync(request.RefId, (short?)request.GroupId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new ActiveCaseItem { CaseId = 0, RefId = 0, GroupId = 0, Status = "Status" });
            }
        }
    }

    /// <summary>
    /// Handles the GetAllFindingByReasonOfStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetAllFindingByReasonOfStreamAsync(EmptyRequest request, IServerStreamWriter<FindingByReasonOfItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming all findings by reason of");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetAllFindingByReasonOfAsync(context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new FindingByReasonOfItem { FindingId = item.Id, Reason = string.Empty, Description = item.Description ?? string.Empty });
            }
        }
    }

    /// <summary>
    /// Handles the GetAllLocksStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetAllLocksStreamAsync(EmptyRequest request, IServerStreamWriter<LockItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming all locks");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetAllLocksAsync(context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new LockItem { LockId = item.lockId, UserId = item.userId, LockType = item.moduleName ?? string.Empty, LockTime = item.lockTime.ToString("yyyy-MM-dd") });
            }
        }
    }

    /// <summary>
    /// Handles the GetCancelReasonsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing cancel reasons parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetCancelReasonsStreamAsync(GetCancelReasonsRequest request, IServerStreamWriter<CancelReasonItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming cancel reasons");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetCancelReasonsAsync((byte?)request.WorkflowId, request.IsFormal, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new CancelReasonItem { ReasonId = item.Id, ReasonText = item.Description ?? string.Empty, IsFormal = request.IsFormal });
            }
        }
    }

    /// <summary>
    /// Handles the GetCreatableByGroupStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing creatable by group parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetCreatableByGroupStreamAsync(GetCreatableByGroupRequest request, IServerStreamWriter<CreatableByGroupItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming creatable by group");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetCreatableByGroupAsync(request.Compo, (byte?)request.Module, (byte?)request.GroupId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new CreatableByGroupItem { WorkflowId = item.workFlowId, WorkflowName = item.title ?? string.Empty, GroupId = request.GroupId });
            }
        }
    }

    /// <summary>
    /// Handles the GetFindingByReasonOfByIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing finding by reason of by ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetFindingByReasonOfByIdStreamAsync(GetFindingByReasonOfByIdRequest request, IServerStreamWriter<FindingByReasonOfByIdItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming finding by reason of by ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetFindingByReasonOfByIdAsync(request.Id, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new FindingByReasonOfByIdItem { FindingId = item.Id, Reason = string.Empty, Description = item.Description ?? string.Empty, Id = item.Id });
            }
        }
    }

    /// <summary>
    /// Handles the GetFindingsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing findings parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetFindingsStreamAsync(GetFindingsRequest request, IServerStreamWriter<FindingItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming findings");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetFindingsAsync((byte?)request.WorkflowId, request.GroupId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new FindingItem { FindingId = item.Id ?? 0, WorkflowId = request.WorkflowId, GroupId = request.GroupId, FindingText = item.Description ?? string.Empty });
            }
        }
    }

    /// <summary>
    /// Handles the GetModuleFromWorkflowStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing module from workflow parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetModuleFromWorkflowStreamAsync(GetModuleFromWorkflowRequest request, IServerStreamWriter<ModuleFromWorkflowItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming module from workflow");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetModuleFromWorkflowAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new ModuleFromWorkflowItem { ModuleId = 0, ModuleName = "Module", WorkflowId = 0 });
            }
        }
    }

    /// <summary>
    /// Handles the GetPageAccessByGroupStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing page access by group parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetPageAccessByGroupStreamAsync(GetPageAccessByGroupRequest request, IServerStreamWriter<PageAccessByGroupItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming page access by group");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPageAccessByGroupAsync((byte?)request.Workflow, request.Status, (byte?)request.Group, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new PageAccessByGroupItem { PageId = item.PageId, PageName = item.PageTitle ?? string.Empty, HasAccess = item.Access != 0, GroupId = item.GroupId ?? 0 });
            }
        }
    }

    /// <summary>
    /// Handles the GetPageAccessByWorkflowViewStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing page access by workflow view parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetPageAccessByWorkflowViewStreamAsync(GetPageAccessByWorkflowViewRequest request, IServerStreamWriter<PageAccessByWorkflowViewItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming page access by workflow view");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPageAccessByWorkflowViewAsync(request.Compo, (byte?)request.Workflow, request.Status, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new PageAccessByWorkflowViewItem { PageId = item.PageId, PageName = item.PageTitle ?? string.Empty, HasAccess = item.Access != 0, Component = request.Compo });
            }
        }
    }

    /// <summary>
    /// Handles the GetPagesByWorkflowIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing pages by workflow ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetPagesByWorkflowIdStreamAsync(GetPagesByWorkflowIdRequest request, IServerStreamWriter<PageByWorkflowItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming pages by workflow ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPagesByWorkflowIdAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new PageByWorkflowItem { PageId = item.pageId, PageName = item.title ?? string.Empty, WorkflowId = request.WorkflowId, PageUrl = "/page" });
            }
        }
    }

    /// <summary>
    /// Handles the GetPermissionsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing permissions parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetPermissionsStreamAsync(GetPermissionsRequest request, IServerStreamWriter<PermissionItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming permissions");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPermissionsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new PermissionItem { PermissionId = item.groupId, PermissionName = item.name ?? string.Empty, WorkflowId = request.WorkflowId, IsGranted = item.canView ?? false });
            }
        }
    }

    /// <summary>
    /// Handles the GetPermissionsByCompoStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing permissions by component parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetPermissionsByCompoStreamAsync(GetPermissionsByCompoRequest request, IServerStreamWriter<PermissionByCompoItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming permissions by component");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetPermissionsByCompoAsync((byte?)request.WorkflowId, request.Compo, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new PermissionByCompoItem { PermissionId = item.groupId, PermissionName = item.name ?? string.Empty, Component = request.Compo, IsGranted = item.canView ?? false });
            }
        }
    }

    /// <summary>
    /// Handles the GetReturnReasonsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing return reasons parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetReturnReasonsStreamAsync(GetReturnReasonsRequest request, IServerStreamWriter<ReturnReasonItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming return reasons");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetReturnReasonsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new ReturnReasonItem { ReasonId = item.Id, ReasonText = item.Description ?? string.Empty, WorkflowId = request.WorkflowId });
            }
        }
    }

    /// <summary>
    /// Handles the GetRwoaReasonsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing RWOA reasons parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetRwoaReasonsStreamAsync(GetRwoaReasonsRequest request, IServerStreamWriter<RwoaReasonItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming RWOA reasons");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetRwoaReasonsAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new RwoaReasonItem { ReasonId = item.Id, ReasonText = item.Description ?? string.Empty, WorkflowId = request.WorkflowId });
            }
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByCompoStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status codes by component parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodesByCompoStreamAsync(GetStatusCodesByCompoRequest request, IServerStreamWriter<StatusCodeByCompoItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming status codes by component");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByCompoAsync(request.Compo, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new StatusCodeByCompoItem { StatusId = item.statusId, StatusName = item.description ?? string.Empty, Component = item.compo });
            }
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByCompoAndModuleStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status codes by component and module parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodesByCompoAndModuleStreamAsync(GetStatusCodesByCompoAndModuleRequest request, IServerStreamWriter<StatusCodeByCompoAndModuleItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming status codes by component and module");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByCompoAndModuleAsync(request.Compo, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new StatusCodeByCompoAndModuleItem { StatusId = item.statusId, StatusName = item.description ?? string.Empty, Component = item.compo, ModuleId = item.moduleId });
            }
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesBySignCodeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status codes by sign code parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodesBySignCodeStreamAsync(GetStatusCodesBySignCodeRequest request, IServerStreamWriter<StatusCodeBySignCodeItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming status codes by sign code");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesBySignCodeAsync((short?)request.GroupId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new StatusCodeBySignCodeItem { StatusId = item.statusId, StatusName = item.description ?? string.Empty, GroupId = request.GroupId, ModuleId = request.Module });
            }
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByWorkflowStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status codes by workflow parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodesByWorkflowStreamAsync(GetStatusCodesByWorkflowRequest request, IServerStreamWriter<StatusCodeByWorkflowItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming status codes by workflow");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByWorkflowAsync((byte?)request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new StatusCodeByWorkflowItem { StatusId = 0, StatusName = "Status", WorkflowId = 0 });
            }
        }
    }

    /// <summary>
    /// Handles the GetStatusCodesByWorkflowAndAccessScopeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status codes by workflow and access scope parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodesByWorkflowAndAccessScopeStreamAsync(GetStatusCodesByWorkflowAndAccessScopeRequest request, IServerStreamWriter<StatusCodeByWorkflowAndAccessScopeItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming status codes by workflow and access scope");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodesByWorkflowAndAccessScopeAsync((byte?)request.WorkflowId, (byte?)request.AccessScope, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new StatusCodeByWorkflowAndAccessScopeItem { StatusId = 0, StatusName = "Status", WorkflowId = 0, AccessScope = 0 });
            }
        }
    }

    /// <summary>
    /// Handles the GetStatusCodeScopeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing status code scope parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStatusCodeScopeStreamAsync(GetStatusCodeScopeRequest request, IServerStreamWriter<StatusCodeScopeItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming status code scope");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStatusCodeScopeAsync((byte?)request.StatusId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new StatusCodeScopeItem { StatusId = request.StatusId, ScopeName = item.accessScope.ToString(), Description = "Access Scope" });
            }
        }
    }

    /// <summary>
    /// Handles the GetStepsByWorkflowStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing steps by workflow parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStepsByWorkflowStreamAsync(GetStepsByWorkflowRequest request, IServerStreamWriter<StepByWorkflowItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming steps by workflow");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStepsByWorkflowAsync((byte?)request.Workflow, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new StepByWorkflowItem { StepId = 0, StepName = "Step", WorkflowId = 0, StepOrder = 0 });
            }
        }
    }

    /// <summary>
    /// Handles the GetStepsByWorkflowAndStatusStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing steps by workflow and status parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetStepsByWorkflowAndStatusStreamAsync(GetStepsByWorkflowAndStatusRequest request, IServerStreamWriter<StepByWorkflowAndStatusItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming steps by workflow and status");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetStepsByWorkflowAndStatusAsync((byte?)request.Workflow, (byte?)request.Status, request.DeathStatus, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new StepByWorkflowAndStatusItem { StepId = 0, StepName = "Step", WorkflowId = 0, StatusId = 0, DeathStatus = "Status" });
            }
        }
    }

    /// <summary>
    /// Handles the GetViewableByGroupStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing viewable by group parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetViewableByGroupStreamAsync(GetViewableByGroupRequest request, IServerStreamWriter<ViewableByGroupItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming viewable by group");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetViewableByGroupAsync((byte?)request.GroupId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new ViewableByGroupItem { WorkflowId = item.workFlowId, WorkflowName = item.title ?? string.Empty, GroupId = request.GroupId, IsViewable = item.active });
            }
        }
    }

    /// <summary>
    /// Handles the GetWorkflowByCompoStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow by component parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowByCompoStreamAsync(GetWorkflowByCompoRequest request, IServerStreamWriter<WorkflowByCompoItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workflow by component");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowByCompoAsync(request.Compo, request.UserId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkflowByCompoItem { WorkflowId = item.workflowId, WorkflowName = item.title, Component = request.Compo, UserId = request.UserId });
            }
        }
    }

    /// <summary>
    /// Handles the GetWorkflowFromModuleStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow from module parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowFromModuleStreamAsync(GetWorkflowFromModuleRequest request, IServerStreamWriter<WorkflowFromModuleItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workflow from module");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowFromModuleAsync(request.ModuleId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkflowFromModuleItem { WorkflowId = item.workflowId ?? 0, WorkflowName = "Workflow", ModuleId = request.ModuleId });
            }
        }
    }

    /// <summary>
    /// Handles the GetWorkflowInitialStatusCodeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow initial status code parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowInitialStatusCodeStreamAsync(GetWorkflowInitialStatusCodeRequest request, IServerStreamWriter<WorkflowInitialStatusCodeItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workflow initial status code");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowInitialStatusCodeAsync(request.Compo, request.Module, request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkflowInitialStatusCodeItem { WorkflowId = request.WorkflowId, InitialStatusId = item.statusId ?? 0, StatusName = "Status" });
            }
        }
    }

    /// <summary>
    /// Handles the GetWorkflowTitleStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow title parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowTitleStreamAsync(GetWorkflowTitleRequest request, IServerStreamWriter<WorkflowTitleItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workflow title");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTitleAsync(request.ModuleId, request.SubCase, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkflowTitleItem { WorkflowId = 0, Title = "Title", ModuleId = 0, SubCase = 0 });
            }
        }
    }

    /// <summary>
    /// Handles the GetWorkflowTitleByWorkStatusIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow title by work status ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowTitleByWorkStatusIdStreamAsync(GetWorkflowTitleByWorkStatusIdRequest request, IServerStreamWriter<WorkflowTitleByWorkStatusIdItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workflow title by work status ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTitleByWorkStatusIdAsync(request.WorkflowId, request.SubCase, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkflowTitleByWorkStatusIdItem { WorkflowId = 0, Title = "Title", WorkStatusId = 0, SubCase = 0 });
            }
        }
    }

    /// <summary>
    /// Handles the InsertActionStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing action insertion parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task InsertActionStreamAsync(InsertActionRequest request, IServerStreamWriter<InsertActionItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming action insertion");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertActionAsync(request, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new InsertActionItem { ActionId = 0, Type = 0, StepId = 0, ResultMessage = "Success" });
            }
        }
    }

    /// <summary>
    /// Handles the InsertOptionActionStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing option action insertion parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task InsertOptionActionStreamAsync(InsertOptionActionRequest request, IServerStreamWriter<InsertOptionActionItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming option action insertion");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertOptionActionAsync(request, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new InsertOptionActionItem { ActionId = 0, Type = 0, Wsoid = 0, ResultMessage = "Success" });
            }
        }
    }

    #endregion

    #region Application Warmup Process Methods

    /// <summary>
    /// Handles the DeleteLogById gRPC request.
    /// </summary>
    /// <param name="request">The request containing log deletion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the log deletion Response.</returns>
    public async override Task<DeleteLogByIdResponse> DeleteLogByIdAsync(DeleteLogByIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Deleting log by ID: {request.LogId}");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.DeleteLogByIdAsync(request.LogId, context?.CancellationToken ?? CancellationToken.None));

        return new DeleteLogByIdResponse
        {
            Result = result
        };
    }

    /// <summary>
    /// Handles the FindProcessLastExecutionDate gRPC request.
    /// </summary>
    /// <param name="request">The request containing process last execution date parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the process last execution date Response.</returns>
    public async override Task<FindProcessLastExecutionDateResponse> FindProcessLastExecutionDateAsync(FindProcessLastExecutionDateRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Finding process last execution date");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.FindProcessLastExecutionDateAsync(request.ProcessName, context?.CancellationToken ?? CancellationToken.None));

        return new FindProcessLastExecutionDateResponse
        {
            Items = { results?.Select(r => new ProcessLastExecutionDateItem { ProcessName = request.ProcessName ?? string.Empty, LastExecutionDate = r.ExecutionDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty, Message = string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the FindProcessLastExecutionDateStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing process last execution date parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task FindProcessLastExecutionDateStreamAsync(FindProcessLastExecutionDateRequest request, IServerStreamWriter<ProcessLastExecutionDateItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming process last execution date");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.FindProcessLastExecutionDateAsync(request.ProcessName, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new ProcessLastExecutionDateItem
                {
                    ProcessName = request.ProcessName ?? string.Empty,
                    LastExecutionDate = item.ExecutionDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty,
                    Message = string.Empty
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetAllLogs gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the all logs Response.</returns>
    public async override Task<GetAllLogsResponse> GetAllLogsAsync(EmptyRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting all logs");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetAllLogsAsync(context?.CancellationToken ?? CancellationToken.None));

        return new GetAllLogsResponse
        {
            Items = { results?.Select(r => new LogItem { LogId = r.Id, ProcessName = r.Name ?? string.Empty, ExecutionDate = r.ExecutionDate.ToString("yyyy-MM-dd HH:mm:ss"), Message = r.Message ?? string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetAllLogsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetAllLogsStreamAsync(EmptyRequest request, IServerStreamWriter<LogItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming all logs");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetAllLogsAsync(context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new LogItem
                {
                    LogId = item.Id,
                    ProcessName = item.Name ?? string.Empty,
                    ExecutionDate = item.ExecutionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Message = item.Message ?? string.Empty
                });
            }
        }
    }

    /// <summary>
    /// Handles the InsertLog gRPC request.
    /// </summary>
    /// <param name="request">The request containing log insertion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the log insertion Response.</returns>
    public async override Task<InsertLogResponse> InsertLogAsync(InsertLogRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Inserting log");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertLogAsync(request.ProcessName, DateTime.TryParse(request.ExecutionDate, out var execDate) ? execDate : DateTime.Now, request.Message, context?.CancellationToken ?? CancellationToken.None));

        return new InsertLogResponse
        {
            Result = result
        };
    }

    /// <summary>
    /// Handles the IsProcessActive gRPC request.
    /// </summary>
    /// <param name="request">The request containing process active check parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the process active Response.</returns>
    public async override Task<IsProcessActiveResponse> IsProcessActiveAsync(IsProcessActiveRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Checking if process is active");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.IsProcessActiveAsync(request.ProcessName, context?.CancellationToken ?? CancellationToken.None));

        return new IsProcessActiveResponse
        {
            Items = { results?.Select(r => new ProcessActiveItem { ProcessName = request.ProcessName ?? string.Empty, IsActive = results.Any() }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the IsProcessActiveStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing process active check parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task IsProcessActiveStreamAsync(IsProcessActiveRequest request, IServerStreamWriter<ProcessActiveItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming process active check");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.IsProcessActiveAsync(request.ProcessName, context?.CancellationToken ?? CancellationToken.None));

        if (results != null && results.Any())
        {
            await responseStream.WriteAsync(new ProcessActiveItem
            {
                ProcessName = request.ProcessName ?? string.Empty,
                IsActive = true
            });
        }
    }

    /// <summary>
    /// Handles the GetAllLogsPagination gRPC request.
    /// </summary>
    /// <param name="request">The request containing pagination, filtering, and sorting parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated logs Response.</returns>
    public async override Task<GetAllLogsPaginationResponse> GetAllLogsPaginationAsync(GetAllLogsPaginationRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting all logs with pagination, filtering, and sorting");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () =>
        {
            return await _dataService.GetAllLogsPaginationAsync(
                request.HasPageNumber ? request.PageNumber : 1,
                request.HasPageSize ? request.PageSize : 10,
                request.ProcessName,
                string.IsNullOrEmpty(request.StartDate) ? null : DateTime.Parse(request.StartDate),
                string.IsNullOrEmpty(request.EndDate) ? null : DateTime.Parse(request.EndDate),
                request.MessageFilter,
                request.SortBy ?? "ExecutionDate",
                request.SortOrder ?? "DESC", context?.CancellationToken ?? CancellationToken.None);
        });

        return new GetAllLogsPaginationResponse
        {
            Items = { results?.Select(r => new LogItem { LogId = r.Id, ProcessName = r.Name ?? string.Empty, ExecutionDate = r.ExecutionDate.ToString("yyyy-MM-dd HH:mm:ss"), Message = r.Message ?? string.Empty }) ?? [] },
            TotalCount = results?.Count ?? 0
        };
    }

    #endregion

    #region Workflow Methods

    /// <summary>
    /// Handles the GetWorkflowById gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow by ID Response.</returns>
    public async override Task<GetWorkflowByIdResponse> GetWorkflowByIdAsync(GetWorkflowByIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workflow by ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowByIdAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkflowByIdResponse
        {
            Items = { results?.Select(r => new WorkflowByIdItem { WorkflowId = (int)r.workflowId, WorkflowText = r.title ?? string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkflowByIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflow ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowByIdStreamAsync(GetWorkflowByIdRequest request, IServerStreamWriter<WorkflowByIdItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workflow by ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowByIdAsync(request.WorkflowId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkflowByIdItem
                {
                    WorkflowId = (int)item.workflowId,
                    WorkflowText = item.title ?? string.Empty
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetWorkflowsByRefId gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflows by ref ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflows by ref ID Response.</returns>
    public async override Task<GetWorkflowsByRefIdResponse> GetWorkflowsByRefIdAsync(GetWorkflowsByRefIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workflows by ref ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowsByRefIdAsync(request.RefId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkflowsByRefIdResponse
        {
            Items = { results?.Select(r => new WorkflowByRefIdItem { WorkflowId = (int)r.workflowId, RefId = r.refId ?? 0, Module = (int)request.Module }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkflowsByRefIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflows by ref ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowsByRefIdStreamAsync(GetWorkflowsByRefIdRequest request, IServerStreamWriter<WorkflowByRefIdItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workflows by ref ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowsByRefIdAsync(request.RefId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkflowByRefIdItem
                {
                    WorkflowId = (int)item.workflowId,
                    RefId = item.refId ?? 0,
                    Module = (int)request.Module
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetWorkflowsByRefIdAndType gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflows by ref ID and type parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflows by ref ID and type Response.</returns>
    public async override Task<GetWorkflowsByRefIdAndTypeResponse> GetWorkflowsByRefIdAndTypeAsync(GetWorkflowsByRefIdAndTypeRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workflows by ref ID and type");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowsByRefIdAndTypeAsync(request.RefId, (byte?)request.Module, request.WorkflowType, context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkflowsByRefIdAndTypeResponse
        {
            Items = { results?.Select(r => new WorkflowByRefIdAndTypeItem { WorkflowId = (int)r.workflowId, RefId = r.refId ?? 0, Module = (int)request.Module, WorkflowType = r.workflowType ?? 0 }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkflowsByRefIdAndTypeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workflows by ref ID and type parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowsByRefIdAndTypeStreamAsync(GetWorkflowsByRefIdAndTypeRequest request, IServerStreamWriter<WorkflowByRefIdAndTypeItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workflows by ref ID and type");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowsByRefIdAndTypeAsync(request.RefId, (byte?)request.Module, request.WorkflowType, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkflowByRefIdAndTypeItem
                {
                    WorkflowId = (int)item.workflowId,
                    RefId = item.refId ?? 0,
                    Module = (int)request.Module,
                    WorkflowType = item.workflowType ?? 0
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetWorkflowTypes gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow types Response.</returns>
    public async override Task<GetWorkflowTypesResponse> GetWorkflowTypesAsync(EmptyRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workflow types");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTypesAsync(context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkflowTypesResponse
        {
            Items = { results?.Select(r => new WorkflowTypeItem { WorkflowTypeId = r.workflowType ?? 0, TypeName = r.typeName ?? string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkflowTypesStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkflowTypesStreamAsync(EmptyRequest request, IServerStreamWriter<WorkflowTypeItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workflow types");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkflowTypesAsync(context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkflowTypeItem
                {
                    WorkflowTypeId = item.workflowType ?? 0,
                    TypeName = item.typeName ?? string.Empty
                });
            }
        }
    }

    /// <summary>
    /// Handles the InsertWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow insertion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow insertion Response.</returns>
    public async override Task<InsertWorkflowResponse> InsertWorkflowAsync(InsertWorkflowRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Inserting workflow");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertWorkflowAsync(request.RefId, (byte?)request.Module, request.WorkflowType, request.WorkflowText, request.UserId, context?.CancellationToken ?? CancellationToken.None));

        return new InsertWorkflowResponse
        {
            Result = result
        };
    }

    /// <summary>
    /// Handles the UpdateWorkflow gRPC request.
    /// </summary>
    /// <param name="request">The request containing workflow update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow update Response.</returns>
    public async override Task<UpdateWorkflowResponse> UpdateWorkflowAsync(UpdateWorkflowRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Updating workflow");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateWorkflowAsync(request.WorkflowId, request.WorkflowText, request.UserId, context?.CancellationToken ?? CancellationToken.None));

        return new UpdateWorkflowResponse
        {
            Result = result
        };
    }

    #endregion

    #region Workstatus Methods

    /// <summary>
    /// Handles the GetWorkstatusById gRPC request.
    /// </summary>
    /// <param name="request">The request containing workstatus ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus by ID Response.</returns>
    public async override Task<GetWorkstatusByIdResponse> GetWorkstatusByIdAsync(GetWorkstatusByIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workstatus by ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusByIdAsync(request.WorkstatusId, context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkstatusByIdResponse
        {
            Items = { results?.Select(r => new WorkstatusByIdItem { WorkstatusId = r.workstatusId ?? 0, WorkstatusText = r.name ?? string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkstatusByIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workstatus ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkstatusByIdStreamAsync(GetWorkstatusByIdRequest request, IServerStreamWriter<WorkstatusByIdItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workstatus by ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusByIdAsync(request.WorkstatusId, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkstatusByIdItem
                {
                    WorkstatusId = item.workstatusId ?? 0,
                    WorkstatusText = item.name ?? string.Empty
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetWorkstatusesByRefId gRPC request.
    /// </summary>
    /// <param name="request">The request containing workstatuses by ref ID parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatuses by ref ID Response.</returns>
    public async override Task<GetWorkstatusesByRefIdResponse> GetWorkstatusesByRefIdAsync(GetWorkstatusesByRefIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workstatuses by ref ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusesByRefIdAsync(request.RefId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkstatusesByRefIdResponse
        {
            Items = { results?.Select(r => new WorkstatusByRefIdItem { WorkstatusId = r.workstatusId ?? 0, RefId = r.refId ?? 0, Module = (int)request.Module }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkstatusesByRefIdStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workstatuses by ref ID parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkstatusesByRefIdStreamAsync(GetWorkstatusesByRefIdRequest request, IServerStreamWriter<WorkstatusByRefIdItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workstatuses by ref ID");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusesByRefIdAsync(request.RefId, (byte?)request.Module, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkstatusByRefIdItem
                {
                    WorkstatusId = item.workstatusId ?? 0,
                    RefId = item.refId ?? 0,
                    Module = (int)request.Module
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetWorkstatusesByRefIdAndType gRPC request.
    /// </summary>
    /// <param name="request">The request containing workstatuses by ref ID and type parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatuses by ref ID and type Response.</returns>
    public async override Task<GetWorkstatusesByRefIdAndTypeResponse> GetWorkstatusesByRefIdAndTypeAsync(GetWorkstatusesByRefIdAndTypeRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workstatuses by ref ID and type");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusesByRefIdAndTypeAsync(request.RefId, (byte?)request.Module, request.WorkstatusType, context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkstatusesByRefIdAndTypeResponse
        {
            Items = { results?.Select(r => new WorkstatusByRefIdAndTypeItem { WorkstatusId = r.workstatusId ?? 0, RefId = r.refId ?? 0, Module = (int)request.Module, WorkstatusType = r.workstatusType ?? 0 }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkstatusesByRefIdAndTypeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing workstatuses by ref ID and type parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkstatusesByRefIdAndTypeStreamAsync(GetWorkstatusesByRefIdAndTypeRequest request, IServerStreamWriter<WorkstatusByRefIdAndTypeItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workstatuses by ref ID and type");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusesByRefIdAndTypeAsync(request.RefId, (byte?)request.Module, request.WorkstatusType, context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkstatusByRefIdAndTypeItem
                {
                    WorkstatusId = item.workstatusId ?? 0,
                    RefId = item.refId ?? 0,
                    Module = (int)request.Module,
                    WorkstatusType = item.workstatusType ?? 0
                });
            }
        }
    }

    /// <summary>
    /// Handles the GetWorkstatusTypes gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus types Response.</returns>
    public async override Task<GetWorkstatusTypesResponse> GetWorkstatusTypesAsync(EmptyRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting workstatus types");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusTypesAsync(context?.CancellationToken ?? CancellationToken.None));

        return new GetWorkstatusTypesResponse
        {
            Items = { results?.Select(r => new WorkstatusTypeItem { WorkstatusTypeId = r.workstatusType ?? 0, TypeName = r.typeName ?? string.Empty }) ?? [] }
        };
    }

    /// <summary>
    /// Handles the GetWorkstatusTypesStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWorkstatusTypesStreamAsync(EmptyRequest request, IServerStreamWriter<WorkstatusTypeItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Streaming workstatus types");

        var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWorkstatusTypesAsync(context?.CancellationToken ?? CancellationToken.None));

        if (results != null)
        {
            foreach (var item in results)
            {
                await responseStream.WriteAsync(new WorkstatusTypeItem
                {
                    WorkstatusTypeId = item.workstatusType ?? 0,
                    TypeName = item.typeName ?? string.Empty
                });
            }
        }
    }

    /// <summary>
    /// Handles the InsertWorkstatus gRPC request.
    /// </summary>
    /// <param name="request">The request containing workstatus insertion parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus insertion Response.</returns>
    public async override Task<InsertWorkstatusResponse> InsertWorkstatusAsync(InsertWorkstatusRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Inserting workstatus");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.InsertWorkstatusAsync(request.RefId, (byte?)request.Module, request.WorkstatusType, request.WorkstatusText, request.UserId, context?.CancellationToken ?? CancellationToken.None));

        return new InsertWorkstatusResponse
        {
            Result = result
        };
    }

    /// <summary>
    /// Handles the UpdateWorkstatus gRPC request.
    /// </summary>
    /// <param name="request">The request containing workstatus update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus update Response.</returns>
    public async override Task<UpdateWorkstatusResponse> UpdateWorkstatusAsync(UpdateWorkstatusRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Updating workstatus");

        var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateWorkstatusAsync(request.WorkstatusId, request.WorkstatusText, request.UserId, context?.CancellationToken ?? CancellationToken.None));

        return new UpdateWorkstatusResponse
        {
            Result = result
        };
    }

    #endregion
}

