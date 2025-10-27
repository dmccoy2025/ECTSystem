using AF.ECT.Data.Models;
using AF.ECT.Data.Extensions;
using AF.ECT.Data.ResultTypes;

#nullable enable

namespace AF.ECT.Data.Services;

/// <summary>
/// Partial class containing Core User Methods.
/// </summary>
public partial class DataService
{
    #region Core User Methods


    /// <summary>
    /// Asynchronously retrieves reinvestigation requests based on user and SARC criteria.
    /// </summary>
    /// <param name="userId">The optional user identifier to filter requests. Pass null to retrieve all requests.</param>
    /// <param name="sarc">The optional SARC flag to filter requests. Pass null to ignore this filter.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of reinvestigation request results.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled via the cancellation token.</exception>
    public async virtual Task<List<core_lod_sp_GetReinvestigationRequestsResult>> GetReinvestigationRequestsAsync(int? userId, bool? sarc, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving reinvestigation requests for user {UserId}, sarc {Sarc}", userId, sarc);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.GetReinvestigationRequestsAsync(userId, sarc, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} reinvestigation requests", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reinvestigation requests for user {UserId}, sarc {Sarc}. Exception: {Exception}", userId, sarc, ex);
            throw;
        }
    }

    public async virtual Task<List<core_lod_sp_GetSpecialCasesResult>> GetSpecialCasesAsync1(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving special cases");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_lod_sp_GetSpecialCasesAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} special cases", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving special cases");
            throw;
        }
    }


    /// <summary>
    /// Asynchronously retrieves special cases.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of special cases results.</returns>
    public async virtual Task<List<core_lod_sp_GetSpecialCasesResult>> GetSpecialCasesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving special cases");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_lod_sp_GetSpecialCasesAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} special cases", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving special cases");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves mailing list for LOD based on reference ID, group ID, status, and calling service.
    /// </summary>
    /// <param name="request">The request containing the parameters for the mailing list retrieval.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of mailing list results.</returns>
    public async Task<List<core_user_sp_GetMailingListForLODResult>> GetMailingListForLODAsync(GetMailingListForLODRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving mailing list for LOD with refId {RefId}, groupId {GroupId}, status {Status}, callingService {CallingService}", request.RefId, request.GroupId, request.Status, request.CallingService);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_GetMailingListForLODAsync(request.RefId, request.GroupId, request.Status, request.CallingService, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} mailing list items", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving mailing list for LOD with refId {RefId}", request.RefId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves managed users based on various criteria.
    /// </summary>
    /// <param name="request">The request containing the search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of managed users.</returns>
    public async Task<List<core_user_sp_GetManagedUsersResult>> GetManagedUsersAsync(GetManagedUsersRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving managed users for userid {UserId}, ssn {Ssn}, name {Name}", request.Userid, request.Ssn, request.Name);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_GetManagedUsersAsync(request.Userid, request.Ssn, request.Name, request.Status, request.Role, request.SrchUnit, request.ShowAllUsers, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} managed users", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving managed users for userid {UserId}", request.Userid);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves user ID for a member SSN.
    /// </summary>
    /// <param name="memberSSN">The member SSN.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user ID.</returns>
    public async Task<int> GetMembersUserIdAsync(string? memberSSN, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving user ID for member SSN {MemberSSN}", memberSSN);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_GetMembersUserIdAsync(memberSSN, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved user ID {UserId} for member SSN", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user ID for member SSN {MemberSSN}", memberSSN);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves user alternate title.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of user alternate titles.</returns>
    public async Task<List<core_user_sp_GetUserAltTitleResult>> GetUserAltTitleAsync(int? userId, int? groupId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving user alt title for userId {UserId}, groupId {GroupId}", userId, groupId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_GetUserAltTitleAsync(userId, groupId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} user alt titles", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user alt title for userId {UserId}", userId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves user alternate title by group component.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="workCompo">The work component identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of user alternate titles by group component.</returns>
    public async Task<List<core_user_sp_GetUserAltTitleByGroupCompoResult>> GetUserAltTitleByGroupCompoAsync(int? groupId, int? workCompo, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving user alt title by group compo for groupId {GroupId}, workCompo {WorkCompo}", groupId, workCompo);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_GetUserAltTitleByGroupCompoAsync(groupId, workCompo, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} user alt titles by group compo", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user alt title by group compo for groupId {GroupId}", groupId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves user name by first and last name.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of user names.</returns>
    public async virtual Task<List<core_user_sp_GetUserNameResult>> GetUserNameAsync(string? first, string? last, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving user name for first {First}, last {Last}", first, last);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_GetUserNameAsync(first, last, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} user names", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user name for first {First}", first);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves users alternate title by group.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of users alternate titles by group.</returns>
    public async Task<List<core_user_sp_GetUsersAltTitleByGroupResult>> GetUsersAltTitleByGroupAsync(int? groupId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving users alt title by group for groupId {GroupId}", groupId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_GetUsersAltTitleByGroupAsync(groupId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} users alt titles by group", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users alt title by group for groupId {GroupId}", groupId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves users who are currently online.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of online users.</returns>
    public async Task<List<core_user_sp_GetUsersOnlineResult>> GetUsersOnlineAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving users online");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_GetUsersOnlineAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} users online", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users online");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves WHOIS information for a user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of WHOIS results.</returns>
    public async Task<List<core_user_sp_GetWhoisResult>> GetWhoisAsync(int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving WHOIS for userId {UserId}", userId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_GetWhoisAsync(userId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} WHOIS results", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving WHOIS for userId {UserId}", userId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously checks if user has HQ tech account.
    /// </summary>
    /// <param name="originUserId">The origin user identifier.</param>
    /// <param name="userEDIPIN">The user EDIPIN.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of HQ tech account results.</returns>
    public async Task<List<core_user_sp_HasHQTechAccountResult>> HasHQTechAccountAsync(int? originUserId, string? userEDIPIN, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking HQ tech account for originUserId {OriginUserId}, userEDIPIN {UserEDIPIN}", originUserId, userEDIPIN);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_HasHQTechAccountAsync(originUserId, userEDIPIN, cancellationToken: cancellationToken);
            _logger.LogInformation("Checked HQ tech account, found {Count} results", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking HQ tech account for originUserId {OriginUserId}", originUserId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously checks if status code is final.
    /// </summary>
    /// <param name="statusId">The status identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of final status code results.</returns>
    public async Task<List<core_user_sp_IsFinalStatusCodeResult>> IsFinalStatusCodeAsync(byte? statusId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking if status code {StatusId} is final", statusId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_IsFinalStatusCodeAsync(statusId, cancellationToken: cancellationToken);
            _logger.LogInformation("Checked status code {StatusId}, found {Count} results", statusId, result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if status code {StatusId} is final", statusId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously logs out a user.
    /// </summary>
    /// <param name="userId">The user identifier to logout.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> LogoutAsync(int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Logging out user {UserId}", userId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_LogoutAsync(userId, cancellationToken: cancellationToken);
            _logger.LogInformation("User {UserId} logged out successfully", userId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging out user {UserId}", userId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously registers a new user.
    /// </summary>
    /// <param name="request">The register user request containing user details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Registering user with userID {UserID}, workCompo {WorkCompo}", request.UserId, request.WorkCompo);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_RegisterUserAsync(request.UserId, request.WorkCompo, request.ReceiveEmail, request.GroupId, (byte?)request.AccountStatus, string.IsNullOrEmpty(request.ExpirationDate) ? null : DateTime.Parse(request.ExpirationDate), cancellationToken: cancellationToken);
            _logger.LogInformation("User registered successfully with result {Result}", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with userID {UserID}", request.UserId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously registers a user role.
    /// </summary>
    /// <param name="userID">The user identifier.</param>
    /// <param name="groupID">The group identifier.</param>
    /// <param name="status">The status.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the user role ID.</returns>
    public async Task<int> RegisterUserRoleAsync(int? userID, short? groupID, byte? status, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Registering user role for userID {UserID}, groupID {GroupID}", userID, groupID);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var userRoleID = new OutputParameter<int?>();
            var result = await context.Procedures.core_user_sp_RegisterUserRoleAsync(userID, groupID, status, userRoleID, cancellationToken: cancellationToken);
            _logger.LogInformation("User role registered successfully with userRoleID {UserRoleID}", userRoleID.Value);
            return userRoleID.Value ?? 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user role for userID {UserID}", userID);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously searches member data.
    /// </summary>
    /// <param name="request">The search member data request containing search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of member data search results.</returns>
    public async Task<List<core_user_sp_SearchMemberDataResult>> SearchMemberDataAsync(SearchMemberDataRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching member data for userId {UserId}, ssn {Ssn}, lastName {LastName}", request.UserId, request.Ssn, request.LastName);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_SearchMemberDataAsync(request.UserId, request.Ssn, request.LastName, request.FirstName, request.MiddleName, request.SrchUnit, request.RptView, cancellationToken: cancellationToken);
            _logger.LogInformation("Searched member data, found {Count} results", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching member data for userId {UserId}", request.UserId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously searches member data (test version).
    /// </summary>
    /// <param name="request">The search member data test request containing search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of member data search test results.</returns>
    public async Task<List<core_user_sp_SearchMemberData_TestResult>> SearchMemberDataTestAsync(SearchMemberDataTestRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching member data test for userId {UserId}, ssn {Ssn}, name {Name}", request.UserId, request.Ssn, request.Name);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_SearchMemberData_TestAsync(request.UserId, request.Ssn, request.Name, request.SrchUnit, request.RptView, cancellationToken: cancellationToken);
            _logger.LogInformation("Searched member data test, found {Count} results", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching member data test for userId {UserId}", request.UserId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously updates account status.
    /// </summary>
    /// <param name="request">The update account status request containing user and status details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> UpdateAccountStatusAsync(UpdateAccountStatusRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating account status for userID {UserID}, accountStatus {AccountStatus}", request.UserId, request.AccountStatus);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_UdpateAccountStatusAsync(request.UserId, (byte?)request.AccountStatus, string.IsNullOrEmpty(request.ExpirationDate) ? null : DateTime.Parse(request.ExpirationDate), request.Comment, cancellationToken: cancellationToken);
            _logger.LogInformation("Account status updated successfully for userID {UserID}", request.UserId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating account status for userID {UserID}", request.UserId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously updates user login information.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="remoteAddr">The remote address.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of login update results.</returns>
    public async Task<List<core_user_sp_UpdateLoginResult>> UpdateLoginAsync(int? userId, string? sessionId, string? remoteAddr, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating login for user {UserId}", userId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_UpdateLoginAsync(userId, sessionId, remoteAddr, cancellationToken: cancellationToken);
            _logger.LogInformation("Login updated for user {UserId}", userId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating login for user {UserId}", userId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously updates managed user settings.
    /// </summary>
    /// <param name="request">The update managed settings request containing user and settings details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> UpdateManagedSettingsAsync(UpdateManagedSettingsRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating managed settings for userId {UserId}, compo {Compo}", request.UserId, request.Compo);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_UpdateManagedSettingsAsync(request.UserId, request.Compo, request.RoleId, (byte?)request.GroupId, request.Comment, request.ReceiveEmail, string.IsNullOrEmpty(request.ExpirationDate) ? null : DateTime.Parse(request.ExpirationDate), cancellationToken: cancellationToken);
            _logger.LogInformation("Managed settings updated successfully for userId {UserId}", request.UserId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating managed settings for userId {UserId}", request.UserId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously updates user alternate title.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="newTitle">The new title.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> UpdateUserAltTitleAsync(int? userId, int? groupId, string? newTitle, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating user alt title for userId {UserId}, groupId {GroupId}", userId, groupId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_user_sp_UpdateUserAltTitleAsync(userId, groupId, newTitle, cancellationToken: cancellationToken);
            _logger.LogInformation("User alt title updated successfully for userId {UserId}", userId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user alt title for userId {UserId}", userId);
            throw;
        }
    }


    #endregion
}
