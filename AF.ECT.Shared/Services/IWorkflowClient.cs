namespace AF.ECT.Shared.Services;

/// <summary>
/// Interface for the GreeterClient service that provides gRPC communication with the server.
/// </summary>
public interface IWorkflowClient : IDisposable
{
    #region Core User Methods

    /// <summary>
    /// Retrieves reinvestigation requests with optional filtering.
    /// </summary>
    /// <param name="userId">Optional user ID filter.</param>
    /// <param name="sarc">Optional SARC flag filter.</param>
    /// <returns>A task representing the asynchronous operation, containing the reinvestigation requests response.</returns>
    Task<GetReinvestigationRequestsResponse> GetReinvestigationRequestsAsync(int? userId = null, bool? sarc = null);

    /// <summary>
    /// Retrieves mailing list for LOD with specified parameters.
    /// </summary>
    /// <param name="refId">The reference ID for the mailing list.</param>
    /// <param name="groupId">The group ID for filtering.</param>
    /// <param name="status">The status for filtering.</param>
    /// <param name="callingService">The calling service identifier.</param>
    /// <returns>A task representing the asynchronous operation, containing the mailing list response.</returns>
    Task<GetMailingListForLODResponse> GetMailingListForLODAsync(int refId, int groupId, int status, string callingService);

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
    Task<GetManagedUsersResponse> GetManagedUsersAsync(int? userid, string ssn, string name, int? status, int? role, int? srchUnit, bool? showAllUsers);

    /// <summary>
    /// Retrieves the user ID for a member by their SSN.
    /// </summary>
    /// <param name="memberSsn">The member's SSN.</param>
    /// <returns>A task representing the asynchronous operation, containing the member user ID response.</returns>
    Task<GetMembersUserIdResponse> GetMembersUserIdAsync(string memberSsn);

    /// <summary>
    /// Retrieves user alternate titles with specified parameters.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate titles response.</returns>
    Task<GetUserAltTitleResponse> GetUserAltTitleAsync(int userId, int groupId);

    /// <summary>
    /// Retrieves user alternate titles by group component.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <param name="workCompo">The work component.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate titles by group component response.</returns>
    Task<GetUserAltTitleByGroupCompoResponse> GetUserAltTitleByGroupCompoAsync(int groupId, int workCompo);

    /// <summary>
    /// Retrieves user names with specified filters.
    /// </summary>
    /// <param name="first">The first name filter.</param>
    /// <param name="last">The last name filter.</param>
    /// <returns>A task representing the asynchronous operation, containing the user names response.</returns>
    Task<GetUserNameResponse> GetUserNameAsync(string first, string last);

    /// <summary>
    /// Retrieves users alternate titles by group.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the users alternate titles by group response.</returns>
    Task<GetUsersAltTitleByGroupResponse> GetUsersAltTitleByGroupAsync(int groupId);

    /// <summary>
    /// Retrieves users who are currently online.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the users online response.</returns>
    Task<GetUsersOnlineResponse> GetUsersOnlineAsync();

    /// <summary>
    /// Retrieves reinvestigation requests as a stream with optional filtering.
    /// </summary>
    /// <param name="userId">Optional user ID filter.</param>
    /// <param name="sarc">Optional SARC flag filter.</param>
    /// <returns>An asynchronous enumerable of reinvestigation request items.</returns>
    IAsyncEnumerable<ReinvestigationRequestItem> GetReinvestigationRequestsStream(int? userId = null, bool? sarc = null);

    /// <summary>
    /// Retrieves mailing list for LOD as a stream with specified parameters.
    /// </summary>
    /// <param name="refId">The reference ID for the mailing list.</param>
    /// <param name="groupId">The group ID for filtering.</param>
    /// <param name="status">The status for filtering.</param>
    /// <param name="callingService">The calling service identifier.</param>
    /// <returns>An asynchronous enumerable of mailing list items.</returns>
    IAsyncEnumerable<MailingListItem> GetMailingListForLODStream(int refId, int groupId, int status, string callingService);

    /// <summary>
    /// Retrieves managed users as a stream with specified filters.
    /// </summary>
    /// <param name="userid">The user ID filter.</param>
    /// <param name="ssn">The SSN filter.</param>
    /// <param name="name">The name filter.</param>
    /// <param name="status">The status filter.</param>
    /// <param name="role">The role filter.</param>
    /// <param name="srchUnit">The search unit filter.</param>
    /// <param name="showAllUsers">Whether to show all users.</param>
    /// <returns>An asynchronous enumerable of managed user items.</returns>
    IAsyncEnumerable<ManagedUserItem> GetManagedUsersStream(int? userid, string ssn, string name, int? status, int? role, int? srchUnit, bool? showAllUsers);

    /// <summary>
    /// Retrieves user alternate titles as a stream with specified parameters.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>An asynchronous enumerable of user alternate title items.</returns>
    IAsyncEnumerable<UserAltTitleItem> GetUserAltTitleStream(int userId, int groupId);

    /// <summary>
    /// Retrieves user alternate titles by group component as a stream.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <param name="workCompo">The work component.</param>
    /// <returns>An asynchronous enumerable of user alternate titles by group component items.</returns>
    IAsyncEnumerable<UserAltTitleByGroupCompoItem> GetUserAltTitleByGroupCompoStream(int groupId, int workCompo);

    /// <summary>
    /// Retrieves user names as a stream with specified filters.
    /// </summary>
    /// <param name="first">The first name filter.</param>
    /// <param name="last">The last name filter.</param>
    /// <returns>An asynchronous enumerable of user name items.</returns>
    IAsyncEnumerable<UserNameItem> GetUserNameStream(string first, string last);

    /// <summary>
    /// Retrieves users alternate titles by group as a stream.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <returns>An asynchronous enumerable of users alternate titles by group items.</returns>
    IAsyncEnumerable<UsersAltTitleByGroupItem> GetUsersAltTitleByGroupStream(int groupId);

    /// <summary>
    /// Retrieves users who are currently online as a stream.
    /// </summary>
    /// <returns>An asynchronous enumerable of users online items.</returns>
    IAsyncEnumerable<UserOnlineItem> GetUsersOnlineStream();

    /// <summary>
    /// Retrieves WHOIS information for a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the WHOIS response.</returns>
    Task<GetWhoisResponse> GetWhoisAsync(int userId);

    /// <summary>
    /// Checks if a user has HQ tech account.
    /// </summary>
    /// <param name="originUserId">The origin user ID.</param>
    /// <param name="userEdipin">The user EDIPIN.</param>
    /// <returns>A task representing the asynchronous operation, containing the HQ tech account response.</returns>
    Task<HasHQTechAccountResponse> HasHQTechAccountAsync(int originUserId, string userEdipin);

    /// <summary>
    /// Checks if a status code is final.
    /// </summary>
    /// <param name="statusId">The status ID to check.</param>
    /// <returns>A task representing the asynchronous operation, containing the final status code response.</returns>
    Task<IsFinalStatusCodeResponse> IsFinalStatusCodeAsync(int statusId);

    /// <summary>
    /// Logs out a user.
    /// </summary>
    /// <param name="userId">The user ID to logout.</param>
    /// <returns>A task representing the asynchronous operation, containing the logout response.</returns>
    Task<LogoutResponse> LogoutAsync(int userId);

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
    Task<RegisterUserResponse> RegisterUserAsync(int userId, string workCompo, bool receiveEmail, int groupId, int accountStatus, string expirationDate);

    /// <summary>
    /// Registers a user role.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <param name="status">The status.</param>
    /// <returns>A task representing the asynchronous operation, containing the user role registration response.</returns>
    Task<RegisterUserRoleResponse> RegisterUserRoleAsync(int userId, int groupId, int status);

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
    Task<SearchMemberDataResponse> SearchMemberDataAsync(int userId, string ssn, string lastName, string firstName, string middleName, int srchUnit, int rptView);

    /// <summary>
    /// Searches member data (test version) with specified parameters.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="ssn">The SSN.</param>
    /// <param name="name">The name.</param>
    /// <param name="srchUnit">The search unit.</param>
    /// <param name="rptView">The report view.</param>
    /// <returns>A task representing the asynchronous operation, containing the member data test search response.</returns>
    Task<SearchMemberDataTestResponse> SearchMemberDataTestAsync(int userId, string ssn, string name, int srchUnit, int rptView);

    /// <summary>
    /// Updates account status for a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="accountStatus">The account status.</param>
    /// <param name="expirationDate">The expiration date.</param>
    /// <param name="comment">The comment.</param>
    /// <returns>A task representing the asynchronous operation, containing the account status update response.</returns>
    Task<UpdateAccountStatusResponse> UpdateAccountStatusAsync(int userId, int accountStatus, string expirationDate, string comment);

    /// <summary>
    /// Updates login information for a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="sessionId">The session ID.</param>
    /// <param name="remoteAddr">The remote address.</param>
    /// <returns>A task representing the asynchronous operation, containing the login update response.</returns>
    Task<UpdateLoginResponse> UpdateLoginAsync(int userId, string sessionId, string remoteAddr);

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
    Task<UpdateManagedSettingsResponse> UpdateManagedSettingsAsync(int userId, string compo, int roleId, int groupId, string comment, bool receiveEmail, string expirationDate);

    /// <summary>
    /// Updates user alternate title.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <param name="newTitle">The new title.</param>
    /// <returns>A task representing the asynchronous operation, containing the user alternate title update response.</returns>
    Task<UpdateUserAltTitleResponse> UpdateUserAltTitleAsync(int userId, int groupId, string newTitle);

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
    Task<AddSignatureResponse> AddSignatureAsync(int refId, int moduleType, int userId, int actionId, int groupId, int statusIn, int statusOut);

    /// <summary>
    /// Copies actions from one workflow to another.
    /// </summary>
    /// <param name="destWsoid">The destination WSOID.</param>
    /// <param name="srcWsoid">The source WSOID.</param>
    /// <returns>A task representing the asynchronous operation, containing the action copy response.</returns>
    Task<CopyActionsResponse> CopyActionsAsync(int destWsoid, int srcWsoid);

    /// <summary>
    /// Copies rules from one workflow to another.
    /// </summary>
    /// <param name="destWsoid">The destination WSOID.</param>
    /// <param name="srcWsoid">The source WSOID.</param>
    /// <returns>A task representing the asynchronous operation, containing the rule copy response.</returns>
    Task<CopyRulesResponse> CopyRulesAsync(int destWsoid, int srcWsoid);

    /// <summary>
    /// Copies a workflow from one ID to another.
    /// </summary>
    /// <param name="fromId">The source workflow ID.</param>
    /// <param name="toId">The destination workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow copy response.</returns>
    Task<CopyWorkflowResponse> CopyWorkflowAsync(int fromId, int toId);

    /// <summary>
    /// Deletes a status code.
    /// </summary>
    /// <param name="statusId">The status ID to delete.</param>
    /// <returns>A task representing the asynchronous operation, containing the status code deletion response.</returns>
    Task<DeleteStatusCodeResponse> DeleteStatusCodeAsync(int statusId);

    /// <summary>
    /// Retrieves actions by step.
    /// </summary>
    /// <param name="stepId">The step ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the actions by step response.</returns>
    Task<GetActionsByStepResponse> GetActionsByStepAsync(int stepId);

    /// <summary>
    /// Retrieves active cases with specified parameters.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the active cases response.</returns>
    Task<GetActiveCasesResponse> GetActiveCasesAsync(int refId, int groupId);

    /// <summary>
    /// Retrieves all findings by reason of.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the all findings by reason of response.</returns>
    Task<GetAllFindingByReasonOfResponse> GetAllFindingByReasonOfAsync();

    /// <summary>
    /// Retrieves all locks.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the all locks response.</returns>
    Task<GetAllLocksResponse> GetAllLocksAsync();

    /// <summary>
    /// Retrieves cancel reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="isFormal">Whether it's formal.</param>
    /// <returns>A task representing the asynchronous operation, containing the cancel reasons response.</returns>
    Task<GetCancelReasonsResponse> GetCancelReasonsAsync(int workflowId, bool isFormal);

    /// <summary>
    /// Retrieves creatable workflows by group.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the creatable by group response.</returns>
    Task<GetCreatableByGroupResponse> GetCreatableByGroupAsync(string compo, int module, int groupId);

    /// <summary>
    /// Retrieves finding by reason of by ID.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the finding by reason of by ID response.</returns>
    Task<GetFindingByReasonOfByIdResponse> GetFindingByReasonOfByIdAsync(int id);

    /// <summary>
    /// Retrieves findings for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="groupId">The group ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the findings response.</returns>
    Task<GetFindingsResponse> GetFindingsAsync(int workflowId, int groupId);

    /// <summary>
    /// Retrieves module from workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the module from workflow response.</returns>
    Task<GetModuleFromWorkflowResponse> GetModuleFromWorkflowAsync(int workflowId);

    /// <summary>
    /// Retrieves page access by group.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="group">The group.</param>
    /// <returns>A task representing the asynchronous operation, containing the page access by group response.</returns>
    Task<GetPageAccessByGroupResponse> GetPageAccessByGroupAsync(int workflow, int status, int group);

    /// <summary>
    /// Retrieves page access by workflow view.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <returns>A task representing the asynchronous operation, containing the page access by workflow view response.</returns>
    Task<GetPageAccessByWorkflowViewResponse> GetPageAccessByWorkflowViewAsync(string compo, int workflow, int status);

    /// <summary>
    /// Retrieves pages by workflow ID.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the pages by workflow ID response.</returns>
    Task<GetPagesByWorkflowIdResponse> GetPagesByWorkflowIdAsync(int workflowId);

    /// <summary>
    /// Retrieves permissions for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the permissions response.</returns>
    Task<GetPermissionsResponse> GetPermissionsAsync(int workflowId);

    /// <summary>
    /// Retrieves permissions by component.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="compo">The component.</param>
    /// <returns>A task representing the asynchronous operation, containing the permissions by component response.</returns>
    Task<GetPermissionsByCompoResponse> GetPermissionsByCompoAsync(int workflowId, string compo);

    /// <summary>
    /// Retrieves return reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the return reasons response.</returns>
    Task<GetReturnReasonsResponse> GetReturnReasonsAsync(int workflowId);

    /// <summary>
    /// Retrieves RWOA reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the RWOA reasons response.</returns>
    Task<GetRwoaReasonsResponse> GetRwoaReasonsAsync(int workflowId);

    /// <summary>
    /// Retrieves status codes by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by component response.</returns>
    Task<GetStatusCodesByCompoResponse> GetStatusCodesByCompoAsync(string compo);

    /// <summary>
    /// Retrieves status codes by component and module.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by component and module response.</returns>
    Task<GetStatusCodesByCompoAndModuleResponse> GetStatusCodesByCompoAndModuleAsync(string compo, int module);

    /// <summary>
    /// Retrieves status codes by sign code.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by sign code response.</returns>
    Task<GetStatusCodesBySignCodeResponse> GetStatusCodesBySignCodeAsync(int groupId, int module);

    /// <summary>
    /// Retrieves status codes by workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by workflow response.</returns>
    Task<GetStatusCodesByWorkflowResponse> GetStatusCodesByWorkflowAsync(int workflowId);

    /// <summary>
    /// Retrieves status codes by workflow and access scope.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="accessScope">The access scope.</param>
    /// <returns>A task representing the asynchronous operation, containing the status codes by workflow and access scope response.</returns>
    Task<GetStatusCodesByWorkflowAndAccessScopeResponse> GetStatusCodesByWorkflowAndAccessScopeAsync(int workflowId, int accessScope);

    /// <summary>
    /// Retrieves status code scope.
    /// </summary>
    /// <param name="statusId">The status ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the status code scope response.</returns>
    Task<GetStatusCodeScopeResponse> GetStatusCodeScopeAsync(int statusId);

    /// <summary>
    /// Retrieves steps by workflow.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <returns>A task representing the asynchronous operation, containing the steps by workflow response.</returns>
    Task<GetStepsByWorkflowResponse> GetStepsByWorkflowAsync(int workflow);

    /// <summary>
    /// Retrieves steps by workflow and status.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="deathStatus">The death status.</param>
    /// <returns>A task representing the asynchronous operation, containing the steps by workflow and status response.</returns>
    Task<GetStepsByWorkflowAndStatusResponse> GetStepsByWorkflowAndStatusAsync(int workflow, int status, string deathStatus);

    /// <summary>
    /// Retrieves viewable workflows by group.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the viewable by group response.</returns>
    Task<GetViewableByGroupResponse> GetViewableByGroupAsync(int groupId, int module);

    /// <summary>
    /// Retrieves workflow by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow by component response.</returns>
    Task<GetWorkflowByCompoResponse> GetWorkflowByCompoAsync(string compo, int userId);

    /// <summary>
    /// Retrieves workflow from module.
    /// </summary>
    /// <param name="moduleId">The module ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow from module response.</returns>
    Task<GetWorkflowFromModuleResponse> GetWorkflowFromModuleAsync(int moduleId);

    /// <summary>
    /// Retrieves workflow initial status code.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow initial status code response.</returns>
    Task<GetWorkflowInitialStatusCodeResponse> GetWorkflowInitialStatusCodeAsync(int compo, int module, int workflowId);

    /// <summary>
    /// Retrieves workflow title.
    /// </summary>
    /// <param name="moduleId">The module ID.</param>
    /// <param name="subCase">The sub case.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow title response.</returns>
    Task<GetWorkflowTitleResponse> GetWorkflowTitleAsync(int moduleId, int subCase);

    /// <summary>
    /// Retrieves workflow title by work status ID.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="subCase">The sub case.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow title by work status ID response.</returns>
    Task<GetWorkflowTitleByWorkStatusIdResponse> GetWorkflowTitleByWorkStatusIdAsync(int workflowId, int subCase);

    /// <summary>
    /// Inserts an action.
    /// </summary>
    /// <param name="type">The action type.</param>
    /// <param name="stepId">The step ID.</param>
    /// <param name="target">The target.</param>
    /// <param name="data">The data.</param>
    /// <returns>A task representing the asynchronous operation, containing the action insertion response.</returns>
    Task<InsertActionResponse> InsertActionAsync(int type, int stepId, int target, int data);

    /// <summary>
    /// Inserts an option action.
    /// </summary>
    /// <param name="type">The action type.</param>
    /// <param name="wsoid">The WSOID.</param>
    /// <param name="target">The target.</param>
    /// <param name="data">The data.</param>
    /// <returns>A task representing the asynchronous operation, containing the option action insertion response.</returns>
    Task<InsertOptionActionResponse> InsertOptionActionAsync(int type, int wsoid, int target, int data);

    #endregion

    #region Application Warmup Process Methods

    /// <summary>
    /// Deletes a log by ID.
    /// </summary>
    /// <param name="logId">The log ID to delete.</param>
    /// <returns>A task representing the asynchronous operation, containing the log deletion response.</returns>
    Task<DeleteLogByIdResponse> DeleteLogByIdAsync(int logId);

    /// <summary>
    /// Finds the last execution date of a process.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>A task representing the asynchronous operation, containing the process last execution date response.</returns>
    Task<FindProcessLastExecutionDateResponse> FindProcessLastExecutionDateAsync(string processName);

    /// <summary>
    /// Finds the last execution date of a process as a streaming response.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>An asynchronous enumerable of process last execution date items.</returns>
    IAsyncEnumerable<ProcessLastExecutionDateItem> FindProcessLastExecutionDateStream(string processName);

    /// <summary>
    /// Retrieves all logs.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the all logs response.</returns>
    Task<GetAllLogsResponse> GetAllLogsAsync();

    /// <summary>
    /// Retrieves all logs as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of log items.</returns>
    IAsyncEnumerable<LogItem> GetAllLogsStream();

    /// <summary>
    /// Retrieves all logs with pagination, filtering, and sorting.
    /// </summary>
    /// <param name="pageNumber">The page number (optional, defaults to 1).</param>
    /// <param name="pageSize">The page size (optional, defaults to 10).</param>
    /// <param name="processName">The process name filter (optional).</param>
    /// <param name="startDate">The start date filter (optional).</param>
    /// <param name="endDate">The end date filter (optional).</param>
    /// <param name="messageFilter">The message filter (optional).</param>
    /// <param name="sortBy">The sort by field (optional, defaults to "ExecutionDate").</param>
    /// <param name="sortOrder">The sort order (optional, defaults to "DESC").</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated logs response.</returns>
    Task<GetAllLogsPaginationResponse> GetAllLogsPaginationAsync(int? pageNumber = null, int? pageSize = null, string? processName = null, string? startDate = null, string? endDate = null, string? messageFilter = null, string? sortBy = null, string? sortOrder = null);

    /// <summary>
    /// Inserts a log entry.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <param name="executionDate">The execution date.</param>
    /// <param name="message">The log message.</param>
    /// <returns>A task representing the asynchronous operation, containing the log insertion response.</returns>
    Task<InsertLogResponse> InsertLogAsync(string processName, string executionDate, string message);

    /// <summary>
    /// Checks if a process is active.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>A task representing the asynchronous operation, containing the process active response.</returns>
    Task<IsProcessActiveResponse> IsProcessActiveAsync(string processName);

    /// <summary>
    /// Checks if a process is active as a streaming response.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>An asynchronous enumerable of process active items.</returns>
    IAsyncEnumerable<ProcessActiveItem> IsProcessActiveStream(string processName);

    #endregion

    #region Workflow Methods

    /// <summary>
    /// Retrieves a workflow by ID.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow by ID response.</returns>
    Task<GetWorkflowByIdResponse> GetWorkflowByIdAsync(int workflowId);

    /// <summary>
    /// Retrieves a workflow by ID as a streaming response.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <returns>An asynchronous enumerable of workflow by ID items.</returns>
    IAsyncEnumerable<WorkflowByIdItem> GetWorkflowByIdStream(int workflowId);

    /// <summary>
    /// Retrieves workflows by reference ID.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflows by ref ID response.</returns>
    Task<GetWorkflowsByRefIdResponse> GetWorkflowsByRefIdAsync(int refId, int module);

    /// <summary>
    /// Retrieves workflows by reference ID as a streaming response.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>An asynchronous enumerable of workflow by ref ID items.</returns>
    IAsyncEnumerable<WorkflowByRefIdItem> GetWorkflowsByRefIdStream(int refId, int module);

    /// <summary>
    /// Retrieves workflows by reference ID and type.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowType">The workflow type.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflows by ref ID and type response.</returns>
    Task<GetWorkflowsByRefIdAndTypeResponse> GetWorkflowsByRefIdAndTypeAsync(int refId, int module, int workflowType);

    /// <summary>
    /// Retrieves workflows by reference ID and type as a streaming response.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowType">The workflow type.</param>
    /// <returns>An asynchronous enumerable of workflow by ref ID and type items.</returns>
    IAsyncEnumerable<WorkflowByRefIdAndTypeItem> GetWorkflowsByRefIdAndTypeStream(int refId, int module, int workflowType);

    /// <summary>
    /// Retrieves workflow types.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the workflow types response.</returns>
    Task<GetWorkflowTypesResponse> GetWorkflowTypesAsync();

    /// <summary>
    /// Retrieves workflow types as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of workflow type items.</returns>
    IAsyncEnumerable<WorkflowTypeItem> GetWorkflowTypesStream();

    /// <summary>
    /// Inserts a new workflow.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowType">The workflow type.</param>
    /// <param name="workflowText">The workflow text.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow insertion response.</returns>
    Task<InsertWorkflowResponse> InsertWorkflowAsync(int refId, int module, int workflowType, string workflowText, int userId);

    /// <summary>
    /// Updates a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow ID.</param>
    /// <param name="workflowText">The workflow text.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workflow update response.</returns>
    Task<UpdateWorkflowResponse> UpdateWorkflowAsync(int workflowId, string workflowText, int userId);

    #endregion

    #region Workstatus Methods

    /// <summary>
    /// Retrieves a workstatus by ID.
    /// </summary>
    /// <param name="workstatusId">The workstatus ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus by ID response.</returns>
    Task<GetWorkstatusByIdResponse> GetWorkstatusByIdAsync(int workstatusId);

    /// <summary>
    /// Retrieves a workstatus by ID as a streaming response.
    /// </summary>
    /// <param name="workstatusId">The workstatus ID.</param>
    /// <returns>An asynchronous enumerable of workstatus by ID items.</returns>
    IAsyncEnumerable<WorkstatusByIdItem> GetWorkstatusByIdStream(int workstatusId);

    /// <summary>
    /// Retrieves workstatuses by reference ID.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatuses by ref ID response.</returns>
    Task<GetWorkstatusesByRefIdResponse> GetWorkstatusesByRefIdAsync(int refId, int module);

    /// <summary>
    /// Retrieves workstatuses by reference ID as a streaming response.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <returns>An asynchronous enumerable of workstatus by ref ID items.</returns>
    IAsyncEnumerable<WorkstatusByRefIdItem> GetWorkstatusesByRefIdStream(int refId, int module);

    /// <summary>
    /// Retrieves workstatuses by reference ID and type.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workstatusType">The workstatus type.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatuses by ref ID and type response.</returns>
    Task<GetWorkstatusesByRefIdAndTypeResponse> GetWorkstatusesByRefIdAndTypeAsync(int refId, int module, int workstatusType);

    /// <summary>
    /// Retrieves workstatuses by reference ID and type as a streaming response.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workstatusType">The workstatus type.</param>
    /// <returns>An asynchronous enumerable of workstatus by ref ID and type items.</returns>
    IAsyncEnumerable<WorkstatusByRefIdAndTypeItem> GetWorkstatusesByRefIdAndTypeStream(int refId, int module, int workstatusType);

    /// <summary>
    /// Retrieves workstatus types.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the workstatus types response.</returns>
    Task<GetWorkstatusTypesResponse> GetWorkstatusTypesAsync();

    /// <summary>
    /// Retrieves workstatus types as a streaming response.
    /// </summary>
    /// <returns>An asynchronous enumerable of workstatus type items.</returns>
    IAsyncEnumerable<WorkstatusTypeItem> GetWorkstatusTypesStream();

    /// <summary>
    /// Inserts a new workstatus.
    /// </summary>
    /// <param name="refId">The reference ID.</param>
    /// <param name="module">The module.</param>
    /// <param name="workstatusType">The workstatus type.</param>
    /// <param name="workstatusText">The workstatus text.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus insertion response.</returns>
    Task<InsertWorkstatusResponse> InsertWorkstatusAsync(int refId, int module, int workstatusType, string workstatusText, int userId);

    /// <summary>
    /// Updates a workstatus.
    /// </summary>
    /// <param name="workstatusId">The workstatus ID.</param>
    /// <param name="workstatusText">The workstatus text.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>A task representing the asynchronous operation, containing the workstatus update response.</returns>
    Task<UpdateWorkstatusResponse> UpdateWorkstatusAsync(int workstatusId, string workstatusText, int userId);

    #endregion
}