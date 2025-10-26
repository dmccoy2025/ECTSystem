using AF.ECT.Data.Models;
using AF.ECT.Data.Extensions;
using AF.ECT.Data.ResultTypes;
using AF.ECT.Data.Entities;
using AF.ECT.Data.Interfaces;

#nullable enable

namespace AF.ECT.Data.Services;

/// <summary>
/// Provides data access operations for the application.
/// </summary>
/// <remarks>
/// This service implements the repository pattern and provides centralized
/// access to database operations through Entity Framework Core.
/// </remarks>
public class DataService : IDataService
{
    #region Fields
    
    private readonly IDbContextFactory<ALODContext> _contextFactory;
    private readonly ILogger<DataService> _logger;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the DataService.
    /// </summary>
    /// <param name="contextFactory">The database context factory for creating database contexts.</param>
    /// <param name="logger">The logger for the data service.</param>
    /// <exception cref="ArgumentNullException">Thrown when contextFactory is null.</exception>
    public DataService(IDbContextFactory<ALODContext> contextFactory, ILogger<DataService> logger)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

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

    #region Core Workflow Methods

    /// <summary>
    /// Asynchronously adds a signature to a workflow.
    /// </summary>
    /// <param name="request">The add signature request containing signature details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of signature results.</returns>
    public async Task<List<core_workflow_sp_AddSignatureResult>> AddSignatureAsync(AddSignatureRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding signature for refId {RefId}, userId {UserId}", request.RefId, request.UserId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_AddSignatureAsync(request.RefId, request.ModuleType, request.UserId, request.ActionId, (byte?)request.GroupId, (byte?)request.StatusIn, (byte?)request.StatusOut, cancellationToken: cancellationToken);
            _logger.LogInformation("Signature added successfully, {Count} results", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding signature for refId {RefId}", request.RefId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously copies actions from one workflow step to another.
    /// </summary>
    /// <param name="destWsoid">The destination workflow step object identifier.</param>
    /// <param name="srcWsoid">The source workflow step object identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> CopyActionsAsync(int? destWsoid, int? srcWsoid, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Copying actions from srcWsoid {SrcWsoid} to destWsoid {DestWsoid}", srcWsoid, destWsoid);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_CopyActionsAsync(destWsoid, srcWsoid, cancellationToken: cancellationToken);
            _logger.LogInformation("Actions copied successfully, result {Result}", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error copying actions from srcWsoid {SrcWsoid}", srcWsoid);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously copies rules from one workflow step to another.
    /// </summary>
    /// <param name="destWsoid">The destination workflow step object identifier.</param>
    /// <param name="srcWsoid">The source workflow step object identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> CopyRulesAsync(int? destWsoid, int? srcWsoid, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Copying rules from srcWsoid {SrcWsoid} to destWsoid {DestWsoid}", srcWsoid, destWsoid);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_CopyRulesAsync(destWsoid, srcWsoid, cancellationToken: cancellationToken);
            _logger.LogInformation("Rules copied successfully, result {Result}", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error copying rules from srcWsoid {SrcWsoid}", srcWsoid);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously copies a workflow from one ID to another.
    /// </summary>
    /// <param name="fromId">The source workflow identifier.</param>
    /// <param name="toId">The destination workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow copy results.</returns>
    public async Task<List<core_workflow_sp_CopyWorkflowResult>> CopyWorkflowAsync(int? fromId, int? toId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Copying workflow from {FromId} to {ToId}", fromId, toId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_CopyWorkflowAsync(fromId, toId, cancellationToken: cancellationToken);
            _logger.LogInformation("Workflow copied successfully, {Count} results", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error copying workflow from {FromId}", fromId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously deletes a status code.
    /// </summary>
    /// <param name="statusId">The status identifier to delete.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> DeleteStatusCodeAsync(int? statusId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting status code {StatusId}", statusId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_DeleteStatusCodeAsync(statusId, cancellationToken: cancellationToken);
            _logger.LogInformation("Status code {StatusId} deleted successfully", statusId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting status code {StatusId}", statusId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves actions by step identifier.
    /// </summary>
    /// <param name="stepId">The step identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of actions by step.</returns>
    public async Task<List<core_workflow_sp_GetActionsByStepResult>> GetActionsByStepAsync(int? stepId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving actions by step for stepId {StepId}", stepId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetActionsByStepAsync(stepId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} actions by step", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actions by step for stepId {StepId}", stepId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves active cases for a reference and group.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of active cases.</returns>
    public async Task<List<core_workflow_sp_GetActiveCasesResult>> GetActiveCasesAsync(int? refId, short? groupId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving active cases for refId {RefId}, groupId {GroupId}", refId, groupId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetActiveCasesAsync(refId, groupId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} active cases", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active cases for refId {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves all findings by reason of.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of all findings by reason of.</returns>
    public async Task<List<core_workflow_sp_GetAllFindingByReasonOfResult>> GetAllFindingByReasonOfAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all findings by reason of");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetAllFindingByReasonOfAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} all findings by reason of", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all findings by reason of");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves all locks.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of all locks.</returns>
    public async Task<List<core_workflow_sp_GetAllLocksResult>> GetAllLocksAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all locks");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetAllLocksAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} all locks", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all locks");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves cancel reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="isFormal">Whether the workflow is formal.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of cancel reasons.</returns>
    public async Task<List<core_workflow_sp_GetCancelReasonsResult>> GetCancelReasonsAsync(byte? workflowId, bool? isFormal, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving cancel reasons for workflowId {WorkflowId}, isFormal {IsFormal}", workflowId, isFormal);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetCancelReasonsAsync(workflowId, isFormal, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} cancel reasons", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cancel reasons for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves creatable workflows by group.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of creatable workflows by group.</returns>
    public async Task<List<core_workflow_sp_GetCreatableByGroupResult>> GetCreatableByGroupAsync(string? compo, byte? module, byte? groupId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving creatable by group for compo {Compo}, module {Module}, groupId {GroupId}", compo, module, groupId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetCreatableByGroupAsync(compo, module, groupId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} creatable by group", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving creatable by group for compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves finding by reason of by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of findings by reason of.</returns>
    public async Task<List<core_workflow_sp_GetFindingByReasonOfByIdResult>> GetFindingByReasonOfByIdAsync(int? id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving finding by reason of by id {Id}", id);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetFindingByReasonOfByIdAsync(id, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} findings by reason of by id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving finding by reason of by id {Id}", id);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves findings for a workflow and group.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of findings.</returns>
    public async Task<List<core_workflow_sp_GetFindingsResult>> GetFindingsAsync(byte? workflowId, int? groupId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving findings for workflowId {WorkflowId}, groupId {GroupId}", workflowId, groupId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetFindingsAsync(workflowId, groupId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} findings", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving findings for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves module from workflow identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of modules from workflow.</returns>
    public async Task<List<core_workflow_sp_GetModuleFromWorkflowResult>> GetModuleFromWorkflowAsync(int? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving module from workflow for workflowId {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetModuleFromWorkflowAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} modules from workflow", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving module from workflow for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves page access by group.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="group">The group.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of page access by group.</returns>
    public async Task<List<core_workflow_sp_GetPageAccessByGroupResult>> GetPageAccessByGroupAsync(byte? workflow, int? status, byte? group, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving page access by group for workflow {Workflow}, status {Status}, group {Group}", workflow, status, group);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetPageAccessByGroupAsync(workflow, status, group, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} page access by group", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving page access by group for workflow {Workflow}", workflow);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves page access by workflow view.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of page access by workflow view.</returns>
    public async Task<List<core_workflow_sp_GetPageAccessByWorkflowViewResult>> GetPageAccessByWorkflowViewAsync(string? compo, byte? workflow, int? status, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving page access by workflow view for compo {Compo}, workflow {Workflow}, status {Status}", compo, workflow, status);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetPageAccessByWorkflowViewAsync(compo, workflow, status, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} page access by workflow view", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving page access by workflow view for compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves pages by workflow identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of pages by workflow.</returns>
    public async Task<List<core_workflow_sp_GetPagesByWorkflowIdResult>> GetPagesByWorkflowIdAsync(int? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving pages by workflow id {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetPagesByWorkflowIdAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} pages by workflow id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pages by workflow id {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves permissions for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of permissions.</returns>
    public async Task<List<core_Workflow_sp_GetPermissionsResult>> GetPermissionsAsync(byte? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving permissions for workflowId {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_Workflow_sp_GetPermissionsAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} permissions", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves permissions by component for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="compo">The component.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of permissions by component.</returns>
    public async Task<List<core_Workflow_sp_GetPermissionsByCompoResult>> GetPermissionsByCompoAsync(byte? workflowId, string? compo, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving permissions by compo for workflowId {WorkflowId}, compo {Compo}", workflowId, compo);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_Workflow_sp_GetPermissionsByCompoAsync(workflowId, compo, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} permissions by compo", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions by compo for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves return reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of return reasons.</returns>
    public async Task<List<core_workflow_sp_GetReturnReasonsResult>> GetReturnReasonsAsync(byte? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving return reasons for workflowId {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetReturnReasonsAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} return reasons", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving return reasons for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves RWOA reasons for a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of RWOA reasons.</returns>
    public async Task<List<core_workflow_sp_GetRwoaReasonsResult>> GetRwoaReasonsAsync(byte? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving RWOA reasons for workflowId {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetRwoaReasonsAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} RWOA reasons", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving RWOA reasons for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves status codes by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by component.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodesByCompoResult>> GetStatusCodesByCompoAsync(string? compo, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving status codes by compo {Compo}", compo);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStatusCodesByCompoAsync(compo, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} status codes by compo", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving status codes by compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves status codes by component and module.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by component and module.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodesByCompoAndModuleResult>> GetStatusCodesByCompoAndModuleAsync(string? compo, byte? module, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving status codes by compo and module for compo {Compo}, module {Module}", compo, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStatusCodesByCompoAndModuleAsync(compo, module, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} status codes by compo and module", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving status codes by compo and module for compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves status codes by sign code.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by sign code.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodesBySignCodeResult>> GetStatusCodesBySignCodeAsync(short? groupId, byte? module, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving status codes by sign code for groupId {GroupId}, module {Module}", groupId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStatusCodesBySignCodeAsync(groupId, module, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} status codes by sign code", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving status codes by sign code for groupId {GroupId}", groupId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves status codes by workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by workflow.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodesByWorkflowResult>> GetStatusCodesByWorkflowAsync(byte? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving status codes by workflow for workflowId {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStatusCodesByWorkflowAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} status codes by workflow", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving status codes by workflow for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves status codes by workflow and access scope.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="accessScope">The access scope.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status codes by workflow and access scope.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodesByWorkflowAndAccessScopeResult>> GetStatusCodesByWorkflowAndAccessScopeAsync(byte? workflowId, byte? accessScope, CancellationToken cancellationToken = default)
    {
        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Procedures.core_workflow_sp_GetStatusCodesByWorkflowAndAccessScopeAsync(workflowId, accessScope, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves status code scope.
    /// </summary>
    /// <param name="statusID">The status identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of status code scopes.</returns>
    public async Task<List<core_workflow_sp_GetStatusCodeScopeResult>> GetStatusCodeScopeAsync(byte? statusID, CancellationToken cancellationToken = default)
    {
        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Procedures.core_workflow_sp_GetStatusCodeScopeAsync(statusID, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves steps by workflow.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of steps by workflow.</returns>
    public async Task<List<core_workflow_sp_GetStepsByWorkflowResult>> GetStepsByWorkflowAsync(byte? workflow, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving steps by workflow for workflow {Workflow}", workflow);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStepsByWorkflowAsync(workflow, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} steps by workflow", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving steps by workflow for workflow {Workflow}", workflow);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves steps by workflow and status.
    /// </summary>
    /// <param name="workflow">The workflow.</param>
    /// <param name="status">The status.</param>
    /// <param name="deathStatus">The death status.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of steps by workflow and status.</returns>
    public async Task<List<core_workflow_sp_GetStepsByWorkflowAndStatusResult>> GetStepsByWorkflowAndStatusAsync(byte? workflow, byte? status, string? deathStatus, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving steps by workflow and status for workflow {Workflow}, status {Status}, deathStatus {DeathStatus}", workflow, status, deathStatus);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetStepsByWorkflowAndStatusAsync(workflow, status, deathStatus, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} steps by workflow and status", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving steps by workflow and status for workflow {Workflow}", workflow);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves viewable workflows by group.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of viewable workflows by group.</returns>
    public async Task<List<core_workflow_sp_GetViewableByGroupResult>> GetViewableByGroupAsync(byte? groupId, byte? module, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving viewable by group for groupId {GroupId}, module {Module}", groupId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetViewableByGroupAsync(groupId, module, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} viewable by group", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving viewable by group for groupId {GroupId}", groupId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow by component.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows by component.</returns>
    public async Task<List<core_workflow_sp_GetWorkflowByCompoResult>> GetWorkflowByCompoAsync(string? compo, int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow by compo for compo {Compo}, userId {UserId}", compo, userId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetWorkflowByCompoAsync(compo, userId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow by compo", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow by compo for compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow from module.
    /// </summary>
    /// <param name="moduleId">The module identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows from module.</returns>
    public async Task<List<core_workflow_sp_GetWorkflowFromModuleResult>> GetWorkflowFromModuleAsync(int? moduleId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow from module for moduleId {ModuleId}", moduleId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetWorkflowFromModuleAsync(moduleId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow from module", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow from module for moduleId {ModuleId}", moduleId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow initial status code.
    /// </summary>
    /// <param name="compo">The component.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow initial status codes.</returns>
    public async Task<List<core_workflow_sp_GetWorkflowInitialStatusCodeResult>> GetWorkflowInitialStatusCodeAsync(int? compo, int? module, int? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow initial status code for compo {Compo}, module {Module}, workflowId {WorkflowId}", compo, module, workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetWorkflowInitialStatusCodeAsync(compo, module, workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow initial status code", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow initial status code for compo {Compo}", compo);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow title.
    /// </summary>
    /// <param name="moduleId">The module identifier.</param>
    /// <param name="subCase">The sub case.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow titles.</returns>
    public async virtual Task<List<core_workflow_sp_GetWorkflowTitleResult>> GetWorkflowTitleAsync(int? moduleId, int? subCase, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow title for moduleId {ModuleId}, subCase {SubCase}", moduleId, subCase);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetWorkflowTitleAsync(moduleId, subCase, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow title", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow title for moduleId {ModuleId}", moduleId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow title by work status identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="subCase">The sub case.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow titles by work status.</returns>
    public async Task<List<core_workflow_sp_GetWorkflowTitleByWorkStatusIdResult>> GetWorkflowTitleByWorkStatusIdAsync(int? workflowId, int? subCase, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow title by work status id for workflowId {WorkflowId}, subCase {SubCase}", workflowId, subCase);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_GetWorkflowTitleByWorkStatusIdAsync(workflowId, subCase, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow title by work status id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow title by work status id for workflowId {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously inserts an action.
    /// </summary>
    /// <param name="request">The insert action request containing action details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of insert action results.</returns>
    public async Task<List<core_workflow_sp_InsertActionResult>> InsertActionAsync(InsertActionRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting action with type {Type}, stepId {StepId}", request.Type, request.StepId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_InsertActionAsync((byte?)request.Type, (short?)request.StepId, request.Target, request.Data, cancellationToken: cancellationToken);
            _logger.LogInformation("Inserted {Count} action", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting action with type {Type}", request.Type);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously inserts an option action.
    /// </summary>
    /// <param name="request">The insert option action request containing action details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of insert option action results.</returns>
    public async Task<List<core_workflow_sp_InsertOptionActionResult>> InsertOptionActionAsync(InsertOptionActionRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting option action with type {Type}, wsoid {Wsoid}", request.Type, request.Wsoid);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.core_workflow_sp_InsertOptionActionAsync((byte?)request.Type, request.Wsoid, request.Target, request.Data, cancellationToken: cancellationToken);
            _logger.LogInformation("Inserted {Count} option action", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting option action with type {Type}", request.Type);
            throw;
        }
    }

    #endregion

    #region Application Warmup Process Methods

    /// <summary>
    /// Asynchronously deletes a log entry by its identifier.
    /// </summary>
    /// <param name="logId">The log entry identifier to delete.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> DeleteLogByIdAsync(int? logId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting log entry with id {LogId}", logId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.ApplicationWarmupProcess_sp_DeleteLogByIdAsync(logId, cancellationToken: cancellationToken);
            _logger.LogInformation("Log entry {LogId} deleted successfully", logId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting log entry with id {LogId}", logId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously finds the last execution date for a process.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of process last execution date results.</returns>
    public async Task<List<ApplicationWarmupProcess_sp_FindProcessLastExecutionDateResult>> FindProcessLastExecutionDateAsync(string? processName, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Finding last execution date for process {ProcessName}", processName);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await ((ALODContextProcedures)context.Procedures).ApplicationWarmupProcess_sp_FindProcessLastExecutionDateAsync(processName, cancellationToken: cancellationToken);
            _logger.LogInformation("Found {Count} last execution date results for process {ProcessName}", result.Count, processName);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding last execution date for process {ProcessName}", processName);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves all log entries.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of all log entries.</returns>
    public async Task<List<ApplicationWarmupProcess_sp_GetAllLogsResult>> GetAllLogsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all log entries");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await ((ALODContextProcedures)context.Procedures).ApplicationWarmupProcess_sp_GetAllLogsAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} log entries", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all log entries");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves all log entries with pagination, filtering, and sorting.
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
    public async Task<ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result> GetAllLogsPaginationAsync(int? pageNumber = 1, int? pageSize = 10, string? processName = null, DateTime? startDate = null, DateTime? endDate = null, string? messageFilter = null, string? sortBy = "ExecutionDate", string? sortOrder = "DESC", CancellationToken cancellationToken = default)
    {
        // Validate input parameters
        if ((pageNumber ?? 1) <= 0)
        {
            throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
        }

        if ((pageSize ?? 10) <= 0 || (pageSize ?? 10) > 1000)
        {
            throw new ArgumentException("Page size must be between 1 and 1000", nameof(pageSize));
        }

        var validSortColumns = new[] { "Id", "Name", "ExecutionDate", "Message" };
        if (!validSortColumns.Contains(sortBy ?? "ExecutionDate"))
        {
            throw new ArgumentException($"Invalid sortBy parameter. Valid values are: {string.Join(", ", validSortColumns)}", nameof(sortBy));
        }

        var validSortOrders = new[] { "ASC", "DESC" };
        if (!validSortOrders.Contains(sortOrder ?? "DESC"))
        {
            throw new ArgumentException("Invalid sortOrder parameter. Valid values are: ASC, DESC", nameof(sortOrder));
        }

        if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
        {
            throw new ArgumentException("Start date cannot be after end date", nameof(startDate));
        }

        // Check for cancellation before proceeding
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Retrieving all log entries with pagination, filtering, and sorting, page {PageNumber}, size {PageSize}", pageNumber, pageSize);

        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.ApplicationWarmupProcess_sp_GetAllLogs_paginationAsync(
                pageNumber,
                pageSize,
                processName,
                startDate,
                endDate,
                messageFilter,
                sortBy,
                sortOrder,
                cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} log entries (total: {TotalCount}) for page {PageNumber}", result.Data.Count, result.TotalCount, pageNumber);
            return result;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Operation was cancelled while retrieving log entries with pagination for page {PageNumber}", pageNumber);
            throw;
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            _logger.LogError(ex, "Database error occurred while retrieving log entries with pagination for page {PageNumber}: {Message}", pageNumber, ex.Message);
            throw new InvalidOperationException("A database error occurred while retrieving log entries. Please try again later.", ex);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            _logger.LogError(ex, "Entity Framework update error occurred while retrieving log entries with pagination for page {PageNumber}: {Message}", pageNumber, ex.Message);
            throw new InvalidOperationException("An error occurred while accessing the database. Please try again later.", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Timeout occurred while retrieving log entries with pagination for page {PageNumber}: {Message}", pageNumber, ex.Message);
            throw new InvalidOperationException("The operation timed out. Please try again later.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while retrieving log entries with pagination for page {PageNumber}: {Message}", pageNumber, ex.Message);
            throw new InvalidOperationException("An unexpected error occurred while retrieving log entries. Please contact support if the problem persists.", ex);
        }
    }

    /// <summary>
    /// Asynchronously retrieves all log entries with pagination, filtering, and sorting using LINQ.
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
    public async Task<ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result> GetAllLogsPaginationAsync1(int? pageNumber = 1, int? pageSize = 10, string? processName = null, DateTime? startDate = null, DateTime? endDate = null, string? messageFilter = null, string? sortBy = "ExecutionDate", string? sortOrder = "DESC", CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all log entries with pagination, filtering, and sorting using LINQ, page {PageNumber}, size {PageSize}", pageNumber, pageSize);
        try
        {
            // Validate sort parameters
            var validSortColumns = new[] { "Id", "Name", "ExecutionDate", "Message" };
            if (!validSortColumns.Contains(sortBy ?? "ExecutionDate"))
            {
                throw new ArgumentException($"Invalid sortBy parameter. Valid values are: {string.Join(", ", validSortColumns)}", nameof(sortBy));
            }

            var validSortOrders = new[] { "ASC", "DESC" };
            if (!validSortOrders.Contains(sortOrder ?? "DESC"))
            {
                throw new ArgumentException("Invalid sortOrder parameter. Valid values are: ASC, DESC", nameof(sortOrder));
            }

            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            // Build base query with filters
            var query = from l in context.Set<ApplicationWarmupProcessLog>()
                        join p in context.Set<ApplicationWarmupProcess>() on l.ProcessId equals p.Id
                        where (processName == null || p.Name.Contains(processName)) &&
                              (startDate == null || l.ExecutionDate >= startDate) &&
                              (endDate == null || l.ExecutionDate <= endDate) &&
                              (messageFilter == null || (l.Message != null && l.Message.Contains(messageFilter)))
                        select new { Log = l, Process = p };

            // Get total count
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply dynamic sorting
            var sortedQuery = sortBy switch
            {
                "Id" => sortOrder == "ASC" 
                    ? query.OrderBy(x => x.Log.Id) 
                    : query.OrderByDescending(x => x.Log.Id),
                "Name" => sortOrder == "ASC" 
                    ? query.OrderBy(x => x.Process.Name) 
                    : query.OrderByDescending(x => x.Process.Name),
                "ExecutionDate" => sortOrder == "ASC" 
                    ? query.OrderBy(x => x.Log.ExecutionDate) 
                    : query.OrderByDescending(x => x.Log.ExecutionDate),
                "Message" => sortOrder == "ASC" 
                    ? query.OrderBy(x => x.Log.Message) 
                    : query.OrderByDescending(x => x.Log.Message),
                _ => query.OrderByDescending(x => x.Log.ExecutionDate)
            };

            // Apply pagination and project to result type
            var data = await sortedQuery
                .Skip(((pageNumber ?? 1) - 1) * (pageSize ?? 10))
                .Take(pageSize ?? 10)
                .Select(x => new ApplicationWarmupProcess_sp_GetAllLogsResult
                {
                    Id = x.Log.Id,
                    Name = x.Process.Name,
                    ExecutionDate = x.Log.ExecutionDate,
                    Message = x.Log.Message
                })
                .ToListAsync(cancellationToken);

            var result = new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result
            {
                TotalCount = totalCount,
                Data = data
            };

            _logger.LogInformation("Retrieved {Count} log entries (total: {TotalCount}) for page {PageNumber} using LINQ", result.Data.Count, result.TotalCount, pageNumber);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all log entries with pagination, filtering, and sorting using LINQ, page {PageNumber}", pageNumber);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously inserts a new log entry.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <param name="executionDate">The execution date.</param>
    /// <param name="message">The log message.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> InsertLogAsync(string? processName, DateTime? executionDate, string? message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting log entry for process {ProcessName}", processName);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.ApplicationWarmupProcess_sp_InsertLogAsync(processName, executionDate, message, cancellationToken: cancellationToken);
            _logger.LogInformation("Log entry inserted successfully for process {ProcessName}", processName);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting log entry for process {ProcessName}", processName);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously checks if a process is active.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of process active status results.</returns>
    public async Task<List<ApplicationWarmupProcess_sp_IsProcessActiveResult>> IsProcessActiveAsync(string? processName, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking if process {ProcessName} is active", processName);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.ApplicationWarmupProcess_sp_IsProcessActiveAsync(processName, cancellationToken: cancellationToken);
            _logger.LogInformation("Checked process {ProcessName} active status, found {Count} results", processName, result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if process {ProcessName} is active", processName);
            throw;
        }
    }

    #endregion

    #region Workflow Methods

    /// <summary>
    /// Asynchronously retrieves a workflow by its identifier.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow results.</returns>
    public async Task<List<workflow_sp_GetWorkflowByIdResult>> GetWorkflowByIdAsync(int? workflowId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow by id {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_GetWorkflowByIdAsync(workflowId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow by id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow by id {WorkflowId}", workflowId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflows by reference identifier.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows by reference.</returns>
    public async Task<List<workflow_sp_GetWorkflowsByRefIdResult>> GetWorkflowsByRefIdAsync(int? refId, byte? module, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflows by ref id {RefId}, module {Module}", refId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_GetWorkflowsByRefIdAsync(refId, module, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflows by ref id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflows by ref id {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflows by reference identifier and type.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="workflowType">The workflow type.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflows by reference and type.</returns>
    public async Task<List<workflow_sp_GetWorkflowsByRefIdAndTypeResult>> GetWorkflowsByRefIdAndTypeAsync(int? refId, byte? module, int? workflowType, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflows by ref id and type for refId {RefId}, module {Module}, workflowType {WorkflowType}", refId, module, workflowType);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_GetWorkflowsByRefIdAndTypeAsync(refId, module, workflowType, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflows by ref id and type", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflows by ref id and type for refId {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workflow types.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workflow types.</returns>
    public async Task<List<workflow_sp_GetWorkflowTypesResult>> GetWorkflowTypesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workflow types");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_GetWorkflowTypesAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workflow types", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow types");
            throw;
        }
    }

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
    public async Task<int> InsertWorkflowAsync(int? refId, byte? module, int? workflowType, string? workflowText, int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting workflow for refId {RefId}, module {Module}", refId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_InsertWorkflowAsync(refId, module, workflowType, workflowText, userId, cancellationToken: cancellationToken);
            _logger.LogInformation("Workflow inserted successfully");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting workflow for refId {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously updates a workflow.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="workflowText">The workflow text.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> UpdateWorkflowAsync(int? workflowId, string? workflowText, int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating workflow {WorkflowId}", workflowId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workflow_sp_UpdateWorkflowAsync(workflowId, workflowText, userId, cancellationToken: cancellationToken);
            _logger.LogInformation("Workflow {WorkflowId} updated successfully", workflowId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating workflow {WorkflowId}", workflowId);
            throw;
        }
    }

    #endregion

    #region Workstatus Methods

    /// <summary>
    /// Asynchronously retrieves a workstatus by its identifier.
    /// </summary>
    /// <param name="workstatusId">The workstatus identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatus results.</returns>
    public async Task<List<workstatus_sp_GetWorkstatusByIdResult>> GetWorkstatusByIdAsync(int? workstatusId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workstatus by id {WorkstatusId}", workstatusId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_GetWorkstatusByIdAsync(workstatusId, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workstatus by id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workstatus by id {WorkstatusId}", workstatusId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workstatuses by reference identifier.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatuses by reference.</returns>
    public async Task<List<workstatus_sp_GetWorkstatusesByRefIdResult>> GetWorkstatusesByRefIdAsync(int? refId, byte? module, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workstatuses by ref id {RefId}, module {Module}", refId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_GetWorkstatusesByRefIdAsync(refId, module, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workstatuses by ref id", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workstatuses by ref id {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workstatuses by reference identifier and type.
    /// </summary>
    /// <param name="refId">The reference identifier.</param>
    /// <param name="module">The module.</param>
    /// <param name="workstatusType">The workstatus type.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatuses by reference and type.</returns>
    public async Task<List<workstatus_sp_GetWorkstatusesByRefIdAndTypeResult>> GetWorkstatusesByRefIdAndTypeAsync(int? refId, byte? module, int? workstatusType, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workstatuses by ref id and type for refId {RefId}, module {Module}, workstatusType {WorkstatusType}", refId, module, workstatusType);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_GetWorkstatusesByRefIdAndTypeAsync(refId, module, workstatusType, cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workstatuses by ref id and type", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workstatuses by ref id and type for refId {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves workstatus types.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of workstatus types.</returns>
    public async Task<List<workstatus_sp_GetWorkstatusTypesResult>> GetWorkstatusTypesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving workstatus types");
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_GetWorkstatusTypesAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Retrieved {Count} workstatus types", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workstatus types");
            throw;
        }
    }

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
    public async Task<int> InsertWorkstatusAsync(int? refId, byte? module, int? workstatusType, string? workstatusText, int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting workstatus for refId {RefId}, module {Module}", refId, module);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_InsertWorkstatusAsync(refId, module, workstatusType, workstatusText, userId, cancellationToken: cancellationToken);
            _logger.LogInformation("Workstatus inserted successfully");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting workstatus for refId {RefId}", refId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously updates a workstatus.
    /// </summary>
    /// <param name="workstatusId">The workstatus identifier.</param>
    /// <param name="workstatusText">The workstatus text.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> UpdateWorkstatusAsync(int? workstatusId, string? workstatusText, int? userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating workstatus {WorkstatusId}", workstatusId);
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var result = await context.Procedures.workstatus_sp_UpdateWorkstatusAsync(workstatusId, workstatusText, userId, cancellationToken: cancellationToken);
            _logger.LogInformation("Workstatus {WorkstatusId} updated successfully", workstatusId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating workstatus {WorkstatusId}", workstatusId);
            throw;
        }
    }

    #endregion
}
