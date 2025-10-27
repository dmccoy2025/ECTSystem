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
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetReinvestigationRequestsAsync(new GetReinvestigationRequestsRequest
                {
                    UserId = userId ?? 0,
                    Sarc = sarc ?? false
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetReinvestigationRequestsAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId, Sarc = sarc });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetReinvestigationRequestsAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId, Sarc = sarc });
            throw;
        }
    }

    /// <summary>
    /// Retrieves reinvestigation requests as a streaming response with optional filtering.
    /// </summary>
    /// <param name="userId">Optional user ID filter.</param>
    /// <param name="sarc">Optional SARC flag filter.</param>
    /// <returns>An asynchronous enumerable of reinvestigation request items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<ReinvestigationRequestItem> GetReinvestigationRequestsStream(int? userId = null, bool? sarc = null)
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
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetMailingListForLODAsync(new GetMailingListForLODRequest
                {
                    RefId = refId,
                    GroupId = groupId,
                    Status = status,
                    CallingService = callingService
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetMailingListForLODAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { RefId = refId, GroupId = groupId, Status = status, CallingService = callingService });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetMailingListForLODAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { RefId = refId, GroupId = groupId, Status = status, CallingService = callingService });
            throw;
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
    public async IAsyncEnumerable<MailingListItem> GetMailingListForLODStream(int refId, int groupId, int status, string callingService)
    {
        using var call = _client.GetMailingListForLODStream(new GetMailingListForLODRequest
        {
            RefId = refId,
            GroupId = groupId,
            Status = status,
            CallingService = callingService
        });
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
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetManagedUsersAsync(new GetManagedUsersRequest
                {
                    Userid = userid ?? 0,
                    Ssn = ssn ?? string.Empty,
                    Name = name ?? string.Empty,
                    Status = status ?? 0,
                    Role = role ?? 0,
                    SrchUnit = srchUnit ?? 0,
                    ShowAllUsers = showAllUsers ?? false
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetManagedUsersAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { Userid = userid, Ssn = ssn, Name = name, Status = status, Role = role, SrchUnit = srchUnit, ShowAllUsers = showAllUsers });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetManagedUsersAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { Userid = userid, Ssn = ssn, Name = name, Status = status, Role = role, SrchUnit = srchUnit, ShowAllUsers = showAllUsers });
            throw;
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
    public async IAsyncEnumerable<ManagedUserItem> GetManagedUsersStream(int? userid, string ssn, string name, int? status, int? role, int? srchUnit, bool? showAllUsers)
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
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetMembersUserIdAsync(new GetMembersUserIdRequest { MemberSsn = memberSsn }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetMembersUserIdAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { MemberSsn = memberSsn });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetMembersUserIdAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { MemberSsn = memberSsn });
            throw;
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
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetUserAltTitleAsync(new GetUserAltTitleRequest
                {
                    UserId = userId,
                    GroupId = groupId
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetUserAltTitleAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { UserId = userId, GroupId = groupId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetUserAltTitleAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { UserId = userId, GroupId = groupId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves user alternate titles as a streaming response with specified parameters.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>An asynchronous enumerable of user alt title items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<UserAltTitleItem> GetUserAltTitleStream(int userId, int groupId)
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
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetUserAltTitleByGroupCompoAsync(new GetUserAltTitleByGroupCompoRequest
                {
                    GroupId = groupId,
                    WorkCompo = workCompo
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetUserAltTitleByGroupCompoAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { GroupId = groupId, WorkCompo = workCompo });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetUserAltTitleByGroupCompoAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { GroupId = groupId, WorkCompo = workCompo });
            throw;
        }
    }

    /// <summary>
    /// Retrieves user alternate titles by group component as a streaming response.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <param name="workCompo">The work component.</param>
    /// <returns>An asynchronous enumerable of user alt title by group compo items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<UserAltTitleByGroupCompoItem> GetUserAltTitleByGroupCompoStream(int groupId, int workCompo)
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
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetUserNameAsync(new GetUserNameRequest
                {
                    First = first,
                    Last = last
                }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetUserNameAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { First = first, Last = last });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetUserNameAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { First = first, Last = last });
            throw;
        }
    }

    /// <summary>
    /// Retrieves user names as a streaming response with specified filters.
    /// </summary>
    /// <param name="first">The first name filter.</param>
    /// <param name="last">The last name filter.</param>
    /// <returns>An asynchronous enumerable of user name items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<UserNameItem> GetUserNameStream(string first, string last)
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
        var correlationId = GenerateCorrelationId();
        var startTime = DateTime.UtcNow;
        _stopwatch.Restart();

        try
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetUsersAltTitleByGroupAsync(new GetUsersAltTitleByGroupRequest { GroupId = groupId }).ResponseAsync;
            });

            _stopwatch.Stop();
            LogAuditEvent(nameof(GetUsersAltTitleByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, true, additionalData: new { GroupId = groupId });

            return result;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            LogAuditEvent(nameof(GetUsersAltTitleByGroupAsync), correlationId, startTime, _stopwatch.Elapsed, false, ex.Message, new { GroupId = groupId });
            throw;
        }
    }

    /// <summary>
    /// Retrieves users alternate titles by group as a streaming response.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <returns>An asynchronous enumerable of users alt title by group items.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown when gRPC communication fails.</exception>
    public async IAsyncEnumerable<UsersAltTitleByGroupItem> GetUsersAltTitleByGroupStream(int groupId)
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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetUsersOnlineAsync(new EmptyRequest()).ResponseAsync;
            });

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
    public async IAsyncEnumerable<UserOnlineItem> GetUsersOnlineStream()
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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.GetWhoisAsync(new GetWhoisRequest { UserId = userId }).ResponseAsync;
            });

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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.HasHQTechAccountAsync(new HasHQTechAccountRequest
                {
                    OriginUserId = originUserId,
                    UserEdipin = userEdipin
                }).ResponseAsync;
            });

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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.IsFinalStatusCodeAsync(new IsFinalStatusCodeRequest { StatusId = statusId }).ResponseAsync;
            });

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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.LogoutAsync(new LogoutRequest { UserId = userId }).ResponseAsync;
            });

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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.RegisterUserAsync(new RegisterUserRequest
                {
                    UserId = userId,
                    WorkCompo = workCompo,
                    ReceiveEmail = receiveEmail,
                    GroupId = groupId,
                    AccountStatus = accountStatus,
                    ExpirationDate = expirationDate ?? string.Empty
                }).ResponseAsync;
            });

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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.RegisterUserRoleAsync(new RegisterUserRoleRequest
                {
                    UserId = userId,
                    GroupId = groupId,
                    Status = status
                }).ResponseAsync;
            });

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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.SearchMemberDataAsync(new SearchMemberDataRequest
                {
                    UserId = userId,
                    Ssn = ssn,
                    LastName = lastName,
                    FirstName = firstName,
                    MiddleName = middleName,
                    SrchUnit = srchUnit,
                    RptView = rptView
                }).ResponseAsync;
            });

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
    public async IAsyncEnumerable<MemberDataItem> SearchMemberDataStream(int userId, string ssn, string lastName, string firstName, string middleName, int srchUnit, int rptView)
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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.SearchMemberDataTestAsync(new SearchMemberDataTestRequest
                {
                    UserId = userId,
                    Ssn = ssn,
                    Name = name,
                    SrchUnit = srchUnit,
                    RptView = rptView
                }).ResponseAsync;
            });

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
    public async IAsyncEnumerable<MemberDataTestItem> SearchMemberDataTestStream(int userId, string ssn, string name, int srchUnit, int rptView)
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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.UpdateAccountStatusAsync(new UpdateAccountStatusRequest
                {
                    UserId = userId,
                    AccountStatus = accountStatus,
                    ExpirationDate = expirationDate ?? string.Empty,
                    Comment = comment
                }).ResponseAsync;
            });

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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.UpdateLoginAsync(new UpdateLoginRequest
                {
                    UserId = userId,
                    SessionId = sessionId,
                    RemoteAddr = remoteAddr
                }).ResponseAsync;
            });

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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.UpdateManagedSettingsAsync(new UpdateManagedSettingsRequest
                {
                    UserId = userId,
                    Compo = compo,
                    RoleId = roleId,
                    GroupId = groupId,
                    Comment = comment,
                    ReceiveEmail = receiveEmail,
                    ExpirationDate = expirationDate ?? string.Empty
                }).ResponseAsync;
            });

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
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _client.UpdateUserAltTitleAsync(new UpdateUserAltTitleRequest
                {
                    UserId = userId,
                    GroupId = groupId,
                    NewTitle = newTitle
                }).ResponseAsync;
            });

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
}
