using AF.ECT.Server.Services.Interfaces;
using AF.ECT.Data.Interfaces;
using Google.Protobuf.Collections;

namespace AF.ECT.Server.Services;

/// <summary>
/// Partial class containing Core User Methods
/// </summary>
public partial class WorkflowServiceImpl : WorkflowService.WorkflowServiceBase
{
    #region Core User Methods


    /// <summary>
    /// Handles the GetReinvestigationRequests gRPC request.
    /// </summary>
    /// <param name="request">The request containing filter parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the reinvestigation requests Response.</returns>
    public async override Task<GetReinvestigationRequestsResponse> GetReinvestigationRequests(GetReinvestigationRequestsRequest request, ServerCallContext context)
    {
        try
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
            _logger.LogError(ex, "An error occurred while getting reinvestigation requests");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetReinvestigationRequestsStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing filter parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetReinvestigationRequestsStream(GetReinvestigationRequestsRequest request, IServerStreamWriter<ReinvestigationRequestItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming reinvestigation requests");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () =>
            {
                return await _dataService.GetReinvestigationRequestsAsync(request.UserId, request.Sarc, context?.CancellationToken ?? CancellationToken.None);
            });

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new ReinvestigationRequestItem
                    {
                        Id = item.request_id,
                        Description = $"{item.Member_Name ?? "Unknown"} - {item.Case_Id} ({item.Status})"
                    });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming reinvestigation requests");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetMailingListForLOD gRPC request.
    /// </summary>
    /// <param name="request">The request containing mailing list parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the mailing list Response.</returns>
    public async override Task<GetMailingListForLODResponse> GetMailingListForLOD(GetMailingListForLODRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting mailing list for LOD");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () =>
            {
                return await _dataService.GetMailingListForLODAsync(request, context?.CancellationToken ?? CancellationToken.None);
            });

            return new GetMailingListForLODResponse
            {
                Items = { results?.Select((r, index) => new MailingListItem { Id = index, Email = r.Email ?? string.Empty, Name = string.Empty }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while getting mailing list for LOD");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetMailingListForLODStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing mailing list parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetMailingListForLODStream(GetMailingListForLODRequest request, IServerStreamWriter<MailingListItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming mailing list for LOD");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () =>
            {
                return await _dataService.GetMailingListForLODAsync(request, context?.CancellationToken ?? CancellationToken.None);
            });

            if (results != null)
            {
                for (var i = 0; i < results.Count; i++)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new MailingListItem
                    {
                        Id = i,
                        Email = results[i].Email ?? string.Empty,
                        Name = string.Empty
                    });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming mailing list for LOD");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetManagedUsers gRPC request.
    /// </summary>
    /// <param name="request">The request containing managed users parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the managed users Response.</returns>
    public async override Task<GetManagedUsersResponse> GetManagedUsers(GetManagedUsersRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting managed users");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () =>
            {
                return await _dataService.GetManagedUsersAsync(request, context?.CancellationToken ?? CancellationToken.None);
            });

            return new GetManagedUsersResponse
            {
                Items = { results?.Select(r => new ManagedUserItem { UserId = r.Id, UserName = r.username ?? string.Empty, Email = string.Empty, Status = r.Status }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while getting managed users");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetManagedUsersStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing managed users parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetManagedUsersStream(GetManagedUsersRequest request, IServerStreamWriter<ManagedUserItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming managed users");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () =>
            {
                return await _dataService.GetManagedUsersAsync(request, context?.CancellationToken ?? CancellationToken.None);
            });

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
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
            _logger.LogError(ex, "An error occurred while streaming managed users");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetMembersUserId gRPC request.
    /// </summary>
    /// <param name="request">The request containing member SSN.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the member user ID Response.</returns>
    public async override Task<GetMembersUserIdResponse> GetMembersUserId(GetMembersUserIdRequest request, ServerCallContext context)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.MemberSsn))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Member SSN is required"));
            }

            _logger.LogInformation($"Getting member user ID for SSN: {request.MemberSsn}");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetMembersUserIdAsync(request.MemberSsn, context?.CancellationToken ?? CancellationToken.None));

            return new GetMembersUserIdResponse
            {
                UserId = result
            };
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
            _logger.LogError(ex, "An error occurred while getting member user ID");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetUserAltTitle gRPC request.
    /// </summary>
    /// <param name="request">The request containing user alternate title parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate title Response.</returns>
    public async override Task<GetUserAltTitleResponse> GetUserAltTitle(GetUserAltTitleRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting user alternate title");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserAltTitleAsync(request.UserId, request.GroupId, context?.CancellationToken ?? CancellationToken.None));

            return new GetUserAltTitleResponse
            {
                Items = { results?.Select(r => new UserAltTitleItem { UserId = request.UserId, Title = r.Title ?? string.Empty, GroupId = request.GroupId }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while getting user alternate title");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetUserAltTitleStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing user alternate title parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetUserAltTitleStream(GetUserAltTitleRequest request, IServerStreamWriter<UserAltTitleItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming user alternate title");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserAltTitleAsync(request.UserId, request.GroupId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new UserAltTitleItem
                    {
                        UserId = request.UserId,
                        Title = item.Title ?? string.Empty,
                        GroupId = request.GroupId
                    });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming user alternate title");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetUserAltTitleByGroupCompo gRPC request.
    /// </summary>
    /// <param name="request">The request containing user alternate title by group component parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate title by group component Response.</returns>
    public async override Task<GetUserAltTitleByGroupCompoResponse> GetUserAltTitleByGroupCompo(GetUserAltTitleByGroupCompoRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting user alternate title by group component");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserAltTitleByGroupCompoAsync(request.GroupId, request.WorkCompo, context?.CancellationToken ?? CancellationToken.None));

            return new GetUserAltTitleByGroupCompoResponse
            {
                Items = { results?.Select(r => new UserAltTitleByGroupCompoItem { UserId = r.userID, Title = r.Title ?? string.Empty, Component = r.Name ?? string.Empty }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while getting user alternate title by group component");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetUserAltTitleByGroupCompoStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing user alternate title by group component parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetUserAltTitleByGroupCompoStream(GetUserAltTitleByGroupCompoRequest request, IServerStreamWriter<UserAltTitleByGroupCompoItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming user alternate title by group component");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserAltTitleByGroupCompoAsync(request.GroupId, request.WorkCompo, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new UserAltTitleByGroupCompoItem
                    {
                        UserId = item.userID,
                        Title = item.Title ?? string.Empty,
                        Component = item.Name ?? string.Empty
                    });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming user alternate title by group component");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetUserName gRPC request.
    /// </summary>
    /// <param name="request">The request containing user name parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user name Response.</returns>
    public async override Task<GetUserNameResponse> GetUserName(GetUserNameRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting user name");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserNameAsync(request.First, request.Last, context?.CancellationToken ?? CancellationToken.None));

            return new GetUserNameResponse
            {
                Items = { results?.Select(r => new UserNameItem { UserId = r.UserId, FirstName = r.FirstName, LastName = r.LastName, FullName = r.FullName }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while getting user name");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetUserNameStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing user name parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetUserNameStream(GetUserNameRequest request, IServerStreamWriter<UserNameItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming user name");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUserNameAsync(request.First, request.Last, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new UserNameItem
                    {
                        UserId = item.UserId,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        FullName = item.FullName
                    });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming user name");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetUsersAltTitleByGroup gRPC request.
    /// </summary>
    /// <param name="request">The request containing users alternate title by group parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the users alternate title by group Response.</returns>
    public async override Task<GetUsersAltTitleByGroupResponse> GetUsersAltTitleByGroup(GetUsersAltTitleByGroupRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting users alternate title by group");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUsersAltTitleByGroupAsync(request.GroupId, context?.CancellationToken ?? CancellationToken.None));

            return new GetUsersAltTitleByGroupResponse
            {
                Items = { results?.Select(r => new UsersAltTitleByGroupItem { UserId = r.userID, Title = r.Title ?? string.Empty, GroupId = request.GroupId }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while getting users alternate title by group");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetUsersAltTitleByGroupStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing users alternate title by group parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetUsersAltTitleByGroupStream(GetUsersAltTitleByGroupRequest request, IServerStreamWriter<UsersAltTitleByGroupItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming users alternate title by group");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUsersAltTitleByGroupAsync(request.GroupId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new UsersAltTitleByGroupItem
                    {
                        UserId = item.userID,
                        Title = item.Title ?? string.Empty,
                        GroupId = request.GroupId
                    });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming users alternate title by group");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetUsersOnline gRPC request.
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the users online Response.</returns>
    public async override Task<GetUsersOnlineResponse> GetUsersOnline(EmptyRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting users online");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUsersOnlineAsync(context?.CancellationToken ?? CancellationToken.None));

            return new GetUsersOnlineResponse
            {
                Items = { results?.Select(r => new UserOnlineItem { UserId = r.userId, UserName = r.UserName ?? string.Empty, LastActivity = r.loginTime.ToString("yyyy-MM-dd") }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while getting users online");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetUsersOnlineStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The empty request.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetUsersOnlineStream(EmptyRequest request, IServerStreamWriter<UserOnlineItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming users online");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetUsersOnlineAsync(context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new UserOnlineItem
                    {
                        UserId = item.userId,
                        UserName = item.UserName ?? string.Empty,
                        LastActivity = item.loginTime.ToString("yyyy-MM-dd")
                    });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming users online");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWhoisStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing WHOIS parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task GetWhoisStream(GetWhoisRequest request, IServerStreamWriter<WhoisItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming WHOIS information");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWhoisAsync(request.UserId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new WhoisItem { UserId = item.UserId, UserName = $"{item.FirstName ?? ""} {item.LastName ?? ""}".Trim(), IpAddress = item.Role ?? string.Empty, LastLogin = string.Empty });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming WHOIS information");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the HasHQTechAccountStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing HQ tech account parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task HasHQTechAccountStream(HasHQTechAccountRequest request, IServerStreamWriter<HQTechAccountItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming HQ tech account check");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.HasHQTechAccountAsync(request.OriginUserId, request.UserEdipin, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new HQTechAccountItem { UserId = 0, HasAccount = true, AccountType = "Type" });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming HQ tech account check");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the IsFinalStatusCodeStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing final status code parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task IsFinalStatusCodeStream(IsFinalStatusCodeRequest request, IServerStreamWriter<FinalStatusCodeItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming final status code check");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.IsFinalStatusCodeAsync((byte?)request.StatusId, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new FinalStatusCodeItem { StatusId = (int)request.StatusId, IsFinal = item.isFinal, Description = string.Empty });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming final status code check");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the UpdateLoginStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing login update parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task UpdateLoginStream(UpdateLoginRequest request, IServerStreamWriter<LoginUpdateItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming login update");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateLoginAsync(request.UserId, request.SessionId, request.RemoteAddr, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new LoginUpdateItem { UserId = 0, SessionId = "session123", LoginTime = "2023-01-01" });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming login update");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the GetWhois gRPC request.
    /// </summary>
    /// <param name="request">The request containing WHOIS parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the WHOIS Response.</returns>
    public async override Task<GetWhoisResponse> GetWhois(GetWhoisRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting WHOIS information");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.GetWhoisAsync(request.UserId, context?.CancellationToken ?? CancellationToken.None));

            return new GetWhoisResponse
            {
                Items = { results?.Select(r => new WhoisItem { UserId = r.UserId, UserName = $"{r.FirstName ?? ""} {r.LastName ?? ""}".Trim(), IpAddress = r.Role ?? string.Empty, LastLogin = string.Empty }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while getting WHOIS information");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the HasHQTechAccount gRPC request.
    /// </summary>
    /// <param name="request">The request containing HQ tech account parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the HQ tech account Response.</returns>
    public async override Task<HasHQTechAccountResponse> HasHQTechAccount(HasHQTechAccountRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Checking HQ tech account");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.HasHQTechAccountAsync(request.OriginUserId, request.UserEdipin, context?.CancellationToken ?? CancellationToken.None));

            return new HasHQTechAccountResponse
            {
                Items = { results?.Select(r => new HQTechAccountItem { UserId = 0, HasAccount = true, AccountType = "Type" }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while checking HQ tech account");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the IsFinalStatusCode gRPC request.
    /// </summary>
    /// <param name="request">The request containing final status code parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the final status code Response.</returns>
    public async override Task<IsFinalStatusCodeResponse> IsFinalStatusCode(IsFinalStatusCodeRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Checking if status code is final");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.IsFinalStatusCodeAsync((byte?)request.StatusId, context?.CancellationToken ?? CancellationToken.None));

            return new IsFinalStatusCodeResponse
            {
                Items = { results?.Select(r => new FinalStatusCodeItem { StatusId = (int)request.StatusId, IsFinal = r.isFinal, Description = string.Empty }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while checking if status code is final");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the Logout gRPC request.
    /// </summary>
    /// <param name="request">The request containing logout parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the logout Response.</returns>
    public async override Task<LogoutResponse> Logout(LogoutRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation($"Logging out user: {request.UserId}");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.LogoutAsync(request.UserId, context?.CancellationToken ?? CancellationToken.None));

            return new LogoutResponse
            {
                Result = result
            };
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
            _logger.LogError(ex, "An error occurred while logging out user");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the RegisterUser gRPC request.
    /// </summary>
    /// <param name="request">The request containing user registration parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user registration Response.</returns>
    public async override Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Registering user");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.RegisterUserAsync(request, context?.CancellationToken ?? CancellationToken.None));

            return new RegisterUserResponse
            {
                Result = result
            };
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
            _logger.LogError(ex, "An error occurred while registering user");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the RegisterUserRole gRPC request.
    /// </summary>
    /// <param name="request">The request containing user role registration parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user role registration Response.</returns>
    public async override Task<RegisterUserRoleResponse> RegisterUserRole(RegisterUserRoleRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Registering user role");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.RegisterUserRoleAsync(request.UserId, (short?)request.GroupId, (byte?)request.Status, context?.CancellationToken ?? CancellationToken.None));

            return new RegisterUserRoleResponse
            {
                UserRoleId = result
            };
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
            _logger.LogError(ex, "An error occurred while registering user role");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the SearchMemberData gRPC request.
    /// </summary>
    /// <param name="request">The request containing member data search parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the member data search Response.</returns>
    public async override Task<SearchMemberDataResponse> SearchMemberData(SearchMemberDataRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Searching member data");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.SearchMemberDataAsync(request, context?.CancellationToken ?? CancellationToken.None));

            return new SearchMemberDataResponse
            {
                Items = { results?.Select(r => new MemberDataItem { MemberId = r.Id, Ssn = r.SSAN ?? string.Empty, FirstName = r.FirstName ?? string.Empty, LastName = r.LastName ?? string.Empty, MiddleName = r.MiddleName ?? string.Empty }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while searching member data");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the SearchMemberDataStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing member data search parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task SearchMemberDataStream(SearchMemberDataRequest request, IServerStreamWriter<MemberDataItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming member data search");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.SearchMemberDataAsync(request, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
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
            _logger.LogError(ex, "An error occurred while streaming member data search");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the SearchMemberDataTest gRPC request.
    /// </summary>
    /// <param name="request">The request containing member data test search parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the member data test search Response.</returns>
    public async override Task<SearchMemberDataTestResponse> SearchMemberDataTest(SearchMemberDataTestRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Searching member data (test version)");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.SearchMemberDataTestAsync(request, context?.CancellationToken ?? CancellationToken.None));

            return new SearchMemberDataTestResponse
            {
                Items = { results?.Select(r => new MemberDataTestItem { MemberId = r.Id, Ssn = r.SSAN ?? string.Empty, Name = $"{r.FirstName ?? string.Empty} {r.LastName ?? string.Empty}".Trim() }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while searching member data (test version)");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the SearchMemberDataTestStream gRPC request (streaming version).
    /// </summary>
    /// <param name="request">The request containing member data test search parameters.</param>
    /// <param name="responseStream">The server stream writer for sending items.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async override Task SearchMemberDataTestStream(SearchMemberDataTestRequest request, IServerStreamWriter<MemberDataTestItem> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Streaming member data test search");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.SearchMemberDataTestAsync(request, context?.CancellationToken ?? CancellationToken.None));

            if (results != null)
            {
                foreach (var item in results)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await responseStream.WriteAsync(new MemberDataTestItem
                    {
                        MemberId = item.Id,
                        Ssn = item.SSAN ?? string.Empty,
                        Name = $"{item.FirstName ?? string.Empty} {item.LastName ?? string.Empty}".Trim()
                    });
                }
            }
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
            _logger.LogError(ex, "An error occurred while streaming member data test search");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the UpdateAccountStatus gRPC request.
    /// </summary>
    /// <param name="request">The request containing account status update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the account status update Response.</returns>
    public async override Task<UpdateAccountStatusResponse> UpdateAccountStatus(UpdateAccountStatusRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Updating account status");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateAccountStatusAsync(request, context?.CancellationToken ?? CancellationToken.None));

            return new UpdateAccountStatusResponse
            {
                Result = result
            };
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
            _logger.LogError(ex, "An error occurred while updating account status");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the UpdateLogin gRPC request.
    /// </summary>
    /// <param name="request">The request containing login update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the login update Response.</returns>
    public async override Task<UpdateLoginResponse> UpdateLogin(UpdateLoginRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Updating login information");

            var results = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateLoginAsync(request.UserId, request.SessionId, request.RemoteAddr, context?.CancellationToken ?? CancellationToken.None));

            return new UpdateLoginResponse
            {
                Items = { results?.Select(r => new LoginUpdateItem { UserId = 0, SessionId = "session123", LoginTime = "2023-01-01" }) ?? [] }
            };
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
            _logger.LogError(ex, "An error occurred while updating login information");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the UpdateManagedSettings gRPC request.
    /// </summary>
    /// <param name="request">The request containing managed settings update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the managed settings update Response.</returns>
    public async override Task<UpdateManagedSettingsResponse> UpdateManagedSettings(UpdateManagedSettingsRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Updating managed settings");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateManagedSettingsAsync(request, context?.CancellationToken ?? CancellationToken.None));

            return new UpdateManagedSettingsResponse
            {
                Result = result
            };
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
            _logger.LogError(ex, "An error occurred while updating managed settings");
            throw CreateInternalErrorException();
        }
    }

    /// <summary>
    /// Handles the UpdateUserAltTitle gRPC request.
    /// </summary>
    /// <param name="request">The request containing user alternate title update parameters.</param>
    /// <param name="context">The server call context for the gRPC operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate title update Response.</returns>
    public async override Task<UpdateUserAltTitleResponse> UpdateUserAltTitle(UpdateUserAltTitleRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Updating user alternate title");

            var result = await _resilienceService.ExecuteWithRetryAsync(async () => await _dataService.UpdateUserAltTitleAsync(request.UserId, request.GroupId, request.NewTitle, context?.CancellationToken ?? CancellationToken.None));

            return new UpdateUserAltTitleResponse
            {
                Result = result
            };
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
            _logger.LogError(ex, "An error occurred while updating user alternate title");
            throw CreateInternalErrorException();
        }
    }


    #endregion
}
