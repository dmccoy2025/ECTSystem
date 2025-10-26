#nullable enable

using AF.ECT.Data.ResultTypes;

namespace AF.ECT.Data.Interfaces;

/// <summary>
/// Defines operations for data access and business logic services.
/// </summary>
public interface IDataService
{
    #region Core User Methods

    /// <summary>
    /// Asynchronously retrieves reinvestigation requests based on user and SARC criteria.
    /// </summary>
    /// <param name="userId">The optional user identifier to filter requests. Pass null to retrieve all requests.</param>
    /// <param name="sarc">The optional SARC flag to filter requests. Pass null to ignore this filter.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of reinvestigation request results.</returns>
    Task<List<core_lod_sp_GetReinvestigationRequestsResult>> GetReinvestigationRequestsAsync(int? userId, bool? sarc, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves special cases.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of special cases results.</returns>
    Task<List<core_lod_sp_GetSpecialCasesResult>> GetSpecialCasesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves mailing list for LOD based on reference ID, group ID, status, and calling service.
    /// </summary>
    /// <param name="request">The request containing the parameters for the mailing list retrieval.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of mailing list results.</returns>
    Task<List<core_user_sp_GetMailingListForLODResult>> GetMailingListForLODAsync(GetMailingListForLODRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves managed users based on various criteria.
    /// </summary>
    /// <param name="request">The request containing the search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of managed users.</returns>
    Task<List<core_user_sp_GetManagedUsersResult>> GetManagedUsersAsync(GetManagedUsersRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves user ID for a member SSN.
    /// </summary>
    /// <param name="memberSSN">The member SSN.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user ID.</returns>
    Task<int> GetMembersUserIdAsync(string? memberSSN, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves user alternate title.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of user alternate titles.</returns>
    Task<List<core_user_sp_GetUserAltTitleResult>> GetUserAltTitleAsync(int? userId, int? groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves user alternate title by group component.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="workCompo">The work component identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of user alternate titles by group component.</returns>
    Task<List<core_user_sp_GetUserAltTitleByGroupCompoResult>> GetUserAltTitleByGroupCompoAsync(int? groupId, int? workCompo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves user name by first and last name.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of user names.</returns>
    Task<List<core_user_sp_GetUserNameResult>> GetUserNameAsync(string? first, string? last, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves users alternate title by group.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of users alternate titles by group.</returns>
    Task<List<core_user_sp_GetUsersAltTitleByGroupResult>> GetUsersAltTitleByGroupAsync(int? groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves users who are currently online.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of online users.</returns>
    Task<List<core_user_sp_GetUsersOnlineResult>> GetUsersOnlineAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves WHOIS information for a user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of WHOIS results.</returns>
    Task<List<core_user_sp_GetWhoisResult>> GetWhoisAsync(int? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously checks if user has HQ tech account.
    /// </summary>
    /// <param name="originUserId">The origin user identifier.</param>
    /// <param name="userEDIPIN">The user EDIPIN.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of HQ tech account results.</returns>
    Task<List<core_user_sp_HasHQTechAccountResult>> HasHQTechAccountAsync(int? originUserId, string? userEDIPIN, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously checks if status code is final.
    /// </summary>
    /// <param name="statusId">The status identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of final status code results.</returns>
    Task<List<core_user_sp_IsFinalStatusCodeResult>> IsFinalStatusCodeAsync(byte? statusId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously logs out a user.
    /// </summary>
    /// <param name="userId">The user identifier to logout.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> LogoutAsync(int? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously registers a new user.
    /// </summary>
    /// <param name="request">The register user request containing user details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously registers a user role.
    /// </summary>
    /// <param name="userID">The user identifier.</param>
    /// <param name="groupID">The group identifier.</param>
    /// <param name="status">The status.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user role ID.</returns>
    Task<int> RegisterUserRoleAsync(int? userID, short? groupID, byte? status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously searches member data.
    /// </summary>
    /// <param name="request">The search member data request containing search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of member data search results.</returns>
    Task<List<core_user_sp_SearchMemberDataResult>> SearchMemberDataAsync(SearchMemberDataRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously searches member data (test version).
    /// </summary>
    /// <param name="request">The search member data test request containing search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of member data search test results.</returns>
    Task<List<core_user_sp_SearchMemberData_TestResult>> SearchMemberDataTestAsync(SearchMemberDataTestRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates account status.
    /// </summary>
    /// <param name="request">The update account status request containing user and status details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> UpdateAccountStatusAsync(UpdateAccountStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates user login information.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="remoteAddr">The remote address.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of login update results.</returns>
    Task<List<core_user_sp_UpdateLoginResult>> UpdateLoginAsync(int? userId, string? sessionId, string? remoteAddr, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates managed user settings.
    /// </summary>
    /// <param name="request">The update managed settings request containing user and settings details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> UpdateManagedSettingsAsync(UpdateManagedSettingsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates user alternate title.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="newTitle">The new title.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> UpdateUserAltTitleAsync(int? userId, int? groupId, string? newTitle, CancellationToken cancellationToken = default);

    #endregion

    #region Core Workflow Methods

    /// <summary>
    /// Asynchronously adds a signature to a workflow.
    /// </summary>
    /// <param name="request">The add signature request containing signature details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of signature results.</returns>
    Task<List<core_workflow_sp_AddSignatureResult>> AddSignatureAsync(AddSignatureRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously copies actions from one workflow step to another.
    /// </summary>
    /// <param name="destWsoid">The destination workflow step object identifier.</param>
    /// <param name="srcWsoid">The source workflow step object identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> CopyActionsAsync(int? destWsoid, int? srcWsoid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously copies rules from one workflow step to another.
    /// </summary>
    /// <param name="destWsoid">The destination workflow step object identifier.</param>
    /// <param name="srcWsoid">The source workflow step object identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> CopyRulesAsync(int? destWsoid, int? srcWsoid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously copies a workflow from one ID to another.
    /// </summary>
    /// <param name="fromId">The source workflow identifier.</param>
    /// <param name="toId">The destination workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow copy results.</returns>
    Task<List<core_workflow_sp_CopyWorkflowResult>> CopyWorkflowAsync(int? fromId, int? toId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes a status code.
    /// </summary>
    /// <param name="statusId">The status identifier to delete.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> DeleteStatusCodeAsync(int? statusId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves actions by step identifier.
    /// </summary>
    /// <param name="stepId">The step identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of actions by step.</returns>
    Task<List<core_workflow_sp_GetActionsByStepResult>> GetActionsByStepAsync(int? stepId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves active cases for a reference and group.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of active cases.</returns>
    Task<List<core_workflow_sp_GetActiveCasesResult>> GetActiveCasesAsync(int? refId, short? groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all findings by reason of.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of all findings by reason of.</returns>
    Task<List<core_workflow_sp_GetAllFindingByReasonOfResult>> GetAllFindingByReasonOfAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all locks.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of all locks.</returns>
    Task<List<core_workflow_sp_GetAllLocksResult>> GetAllLocksAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves cancel reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="isFormal">Whether the workflow is formal.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of cancel reasons.</returns>
    Task<List<core_workflow_sp_GetCancelReasonsResult>> GetCancelReasonsAsync(byte? workflowId, bool? isFormal, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves creatable workflows by group.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of creatable workflows by group.</returns>
    Task<List<core_workflow_sp_GetCreatableByGroupResult>> GetCreatableByGroupAsync(string? compo, byte? module, byte? groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves finding by reason of by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of findings by reason of.</returns>
    Task<List<core_workflow_sp_GetFindingByReasonOfByIdResult>> GetFindingByReasonOfByIdAsync(int? id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves findings for a workflow and group.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of findings.</returns>
    Task<List<core_workflow_sp_GetFindingsResult>> GetFindingsAsync(byte? workflowId, int? groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves module from workflow identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of modules from workflow.</returns>
    Task<List<core_workflow_sp_GetModuleFromWorkflowResult>> GetModuleFromWorkflowAsync(int? workflowId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves page access by group.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="group">The group.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of page access by group.</returns>
    Task<List<core_workflow_sp_GetPageAccessByGroupResult>> GetPageAccessByGroupAsync(byte? workflow, int? status, byte? group, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves page access by workflow view.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of page access by workflow view.</returns>
    Task<List<core_workflow_sp_GetPageAccessByWorkflowViewResult>> GetPageAccessByWorkflowViewAsync(string? compo, byte? workflow, int? status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves pages by workflow identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of pages by workflow.</returns>
    Task<List<core_workflow_sp_GetPagesByWorkflowIdResult>> GetPagesByWorkflowIdAsync(int? workflowId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves permissions for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of permissions.</returns>
    Task<List<core_Workflow_sp_GetPermissionsResult>> GetPermissionsAsync(byte? workflowId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves permissions by component for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="compo">The component.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of permissions by component.</returns>
    Task<List<core_Workflow_sp_GetPermissionsByCompoResult>> GetPermissionsByCompoAsync(byte? workflowId, string? compo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves return reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of return reasons.</returns>
    Task<List<core_workflow_sp_GetReturnReasonsResult>> GetReturnReasonsAsync(byte? workflowId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves RWOA reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of RWOA reasons.</returns>
    Task<List<core_workflow_sp_GetRwoaReasonsResult>> GetRwoaReasonsAsync(byte? workflowId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves status codes by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by component.</returns>
    Task<List<core_workflow_sp_GetStatusCodesByCompoResult>> GetStatusCodesByCompoAsync(string? compo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves status codes by component and module.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by component and module.</returns>
    Task<List<core_workflow_sp_GetStatusCodesByCompoAndModuleResult>> GetStatusCodesByCompoAndModuleAsync(string? compo, byte? module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves status codes by sign code.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by sign code.</returns>
    Task<List<core_workflow_sp_GetStatusCodesBySignCodeResult>> GetStatusCodesBySignCodeAsync(short? groupId, byte? module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves status codes by workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by workflow.</returns>
    Task<List<core_workflow_sp_GetStatusCodesByWorkflowResult>> GetStatusCodesByWorkflowAsync(byte? workflowId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves status codes by workflow and access scope.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="accessScope">The access scope.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by workflow and access scope.</returns>
    Task<List<core_workflow_sp_GetStatusCodesByWorkflowAndAccessScopeResult>> GetStatusCodesByWorkflowAndAccessScopeAsync(byte? workflowId, byte? accessScope, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves status code scope.
    /// </summary>
    /// <param name="statusID">The status identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status code scopes.</returns>
    Task<List<core_workflow_sp_GetStatusCodeScopeResult>> GetStatusCodeScopeAsync(byte? statusID, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves steps by workflow.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of steps by workflow.</returns>
    Task<List<core_workflow_sp_GetStepsByWorkflowResult>> GetStepsByWorkflowAsync(byte? workflow, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves steps by workflow and status.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="deathStatus">The death status.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of steps by workflow and status.</returns>
    Task<List<core_workflow_sp_GetStepsByWorkflowAndStatusResult>> GetStepsByWorkflowAndStatusAsync(byte? workflow, byte? status, string? deathStatus, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves viewable workflows by group.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of viewable workflows by group.</returns>
    Task<List<core_workflow_sp_GetViewableByGroupResult>> GetViewableByGroupAsync(byte? groupId, byte? module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves workflow by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows by component.</returns>
    Task<List<core_workflow_sp_GetWorkflowByCompoResult>> GetWorkflowByCompoAsync(string? compo, int? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves workflow from module.
    /// </summary>
    /// <param name="moduleId">The module identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows from module.</returns>
    Task<List<core_workflow_sp_GetWorkflowFromModuleResult>> GetWorkflowFromModuleAsync(int? moduleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves workflow initial status code.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow initial status codes.</returns>
    Task<List<core_workflow_sp_GetWorkflowInitialStatusCodeResult>> GetWorkflowInitialStatusCodeAsync(int? compo, int? module, int? workflowId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves workflow title.
    /// </summary>
    /// <param name="moduleId">The module identifier.</param>
    /// <param name="subCase">The sub case.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow titles.</returns>
    Task<List<core_workflow_sp_GetWorkflowTitleResult>> GetWorkflowTitleAsync(int? moduleId, int? subCase, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves workflow title by work status identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="subCase">The sub case.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow titles by work status.</returns>
    Task<List<core_workflow_sp_GetWorkflowTitleByWorkStatusIdResult>> GetWorkflowTitleByWorkStatusIdAsync(int? workflowId, int? subCase, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously inserts an action.
    /// </summary>
    /// <param name="request">The insert action request containing action details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of insert action results.</returns>
    Task<List<core_workflow_sp_InsertActionResult>> InsertActionAsync(InsertActionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously inserts an option action.
    /// </summary>
    /// <param name="request">The insert option action request containing action details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of insert option action results.</returns>
    Task<List<core_workflow_sp_InsertOptionActionResult>> InsertOptionActionAsync(InsertOptionActionRequest request, CancellationToken cancellationToken = default);

    #endregion

    #region Application Warmup Process Methods

    /// <summary>
    /// Asynchronously deletes a log by identifier.
    /// </summary>
    /// <param name="logId">The log identifier to delete.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> DeleteLogByIdAsync(int? logId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously finds the last execution date for a process.
    /// </summary>
    /// <param name="processName">The process name.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of process last execution dates.</returns>
    Task<List<ApplicationWarmupProcess_sp_FindProcessLastExecutionDateResult>> FindProcessLastExecutionDateAsync(string? processName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all logs.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of all logs.</returns>
    Task<List<ApplicationWarmupProcess_sp_GetAllLogsResult>> GetAllLogsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all logs with pagination, filtering, and sorting.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="processName">Optional filter by process name.</param>
    /// <param name="startDate">Optional filter for execution date from this date.</param>
    /// <param name="endDate">Optional filter for execution date up to this date.</param>
    /// <param name="messageFilter">Optional filter by message content.</param>
    /// <param name="sortBy">Column to sort by ('Id', 'Name', 'ExecutionDate', 'Message').</param>
    /// <param name="sortOrder">Sort order ('ASC' or 'DESC').</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing both the total count and paginated log entries.</returns>
    Task<ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result> GetAllLogsPaginationAsync(int? pageNumber = 1, int? pageSize = 10, string? processName = null, DateTime? startDate = null, DateTime? endDate = null, string? messageFilter = null, string? sortBy = "ExecutionDate", string? sortOrder = "DESC", CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all logs with pagination, filtering, and sorting using LINQ.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="processName">Optional filter by process name.</param>
    /// <param name="startDate">Optional filter for execution date from this date.</param>
    /// <param name="endDate">Optional filter for execution date up to this date.</param>
    /// <param name="messageFilter">Optional filter by message content.</param>
    /// <param name="sortBy">Column to sort by ('Id', 'Name', 'ExecutionDate', 'Message').</param>
    /// <param name="sortOrder">Sort order ('ASC' or 'DESC').</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing both the total count and paginated log entries.</returns>
    Task<ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result> GetAllLogsPaginationAsync1(int? pageNumber = 1, int? pageSize = 10, string? processName = null, DateTime? startDate = null, DateTime? endDate = null, string? messageFilter = null, string? sortBy = "ExecutionDate", string? sortOrder = "DESC", CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously inserts a log entry.
    /// </summary>
    /// <param name="processName">The process name.</param>
    /// <param name="executionDate">The execution date.</param>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> InsertLogAsync(string? processName, DateTime? executionDate, string? message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously checks if a process is active.
    /// </summary>
    /// <param name="processName">The process name.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of process active results.</returns>
    Task<List<ApplicationWarmupProcess_sp_IsProcessActiveResult>> IsProcessActiveAsync(string? processName, CancellationToken cancellationToken = default);

    #endregion

    #region Workflow Methods

    /// <summary>
    /// Asynchronously retrieves a workflow by its identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow results.</returns>
    Task<List<workflow_sp_GetWorkflowByIdResult>> GetWorkflowByIdAsync(int? workflowId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves workflows by reference identifier.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows by reference.</returns>
    Task<List<workflow_sp_GetWorkflowsByRefIdResult>> GetWorkflowsByRefIdAsync(int? refId, byte? module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves workflows by reference identifier and type.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowType">The workflow type.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows by reference and type.</returns>
    Task<List<workflow_sp_GetWorkflowsByRefIdAndTypeResult>> GetWorkflowsByRefIdAndTypeAsync(int? refId, byte? module, int? workflowType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves workflow types.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow types.</returns>
    Task<List<workflow_sp_GetWorkflowTypesResult>> GetWorkflowTypesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously inserts a workflow.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowType">The workflow type.</param>
    /// <param name="workflowText">The workflow text.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> InsertWorkflowAsync(int? refId, byte? module, int? workflowType, string? workflowText, int? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="workflowText">The workflow text.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> UpdateWorkflowAsync(int? workflowId, string? workflowText, int? userId, CancellationToken cancellationToken = default);

    #endregion

    #region Workstatus Methods

    /// <summary>
    /// Asynchronously retrieves a workstatus by its identifier.
    /// </summary>
    /// <param name="workstatusId">The workstatus identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatus results.</returns>
    Task<List<workstatus_sp_GetWorkstatusByIdResult>> GetWorkstatusByIdAsync(int? workstatusId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves workstatuses by reference identifier.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatuses by reference.</returns>
    Task<List<workstatus_sp_GetWorkstatusesByRefIdResult>> GetWorkstatusesByRefIdAsync(int? refId, byte? module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves workstatuses by reference identifier and type.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="workstatusType">The workstatus type.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatuses by reference and type.</returns>
    Task<List<workstatus_sp_GetWorkstatusesByRefIdAndTypeResult>> GetWorkstatusesByRefIdAndTypeAsync(int? refId, byte? module, int? workstatusType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves workstatus types.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatus types.</returns>
    Task<List<workstatus_sp_GetWorkstatusTypesResult>> GetWorkstatusTypesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously inserts a workstatus.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="workstatusType">The workstatus type.</param>
    /// <param name="workstatusText">The workstatus text.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> InsertWorkstatusAsync(int? refId, byte? module, int? workstatusType, string? workstatusText, int? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates a workstatus.
    /// </summary>
    /// <param name="workstatusId">The workstatus identifier.</param>
    /// <param name="workstatusText">The workstatus text.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> UpdateWorkstatusAsync(int? workstatusId, string? workstatusText, int? userId, CancellationToken cancellationToken = default);

    #endregion
}
