using AF.ECT.Server.Services;
using AF.ECT.Server.Services.Interfaces;
using static AF.ECT.Tests.Data.WorkflowServiceTestData;
using AF.ECT.Data.Interfaces;
using AF.ECT.Data.ResultTypes;

namespace AF.ECT.Tests.Unit;

/// <summary>
/// Contains unit tests for the <see cref="WorkflowServiceImpl"/> class.
/// Tests cover constructor validation, data service integration, error handling, and various gRPC method scenarios.
/// 
/// <para>Test Scenarios Outline:</para>
/// <list type="bullet">
/// <item><description>Constructor validation: Ensures proper exception handling for null dependencies (logger and data service).</description></item>
/// <item><description>Data service integration: Verifies correct interaction with IDataService for various operations.</description></item>
/// <item><description>Input validation: Tests handling of null requests and various parameter combinations.</description></item>
/// <item><description>Error propagation: Tests exception handling from underlying data service failures.</description></item>
/// <item><description>Response mapping: Ensures proper mapping of data service results to gRPC response objects.</description></item>
/// </list>
/// </summary>
[Collection("WorkflowService Tests")]
[Trait("Category", "Unit")]
[Trait("Component", "WorkflowService")]
public class WorkflowServiceTests
{
    private readonly Mock<ILogger<WorkflowServiceImpl>> _mockLogger;
    private readonly Mock<IDataService> _mockDataService;
    private readonly Mock<IResilienceService> _mockResilienceService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowServiceTests"/> class.
    /// Sets up mock objects for testing.
    /// </summary>
    public WorkflowServiceTests()
    {
        _mockLogger = new Mock<ILogger<WorkflowServiceImpl>>();
        _mockDataService = new Mock<IDataService>();
        _mockResilienceService = new Mock<IResilienceService>();
    }

    /// <summary>
    /// Creates a new instance of <see cref="WorkflowServiceImpl"/> with mocked dependencies.
    /// </summary>
    /// <returns>A new WorkflowManagementService instance for testing.</returns>
    private WorkflowServiceImpl CreateWorkflowManagementService() =>
        new(_mockLogger.Object, _mockDataService.Object, new TestResilienceService());

    /// <summary>
    /// Sets up the mock data service to return a specified number of reinvestigation requests.
    /// </summary>
    /// <param name="requestCount">The number of mock requests to return.</param>
    private void SetupDataServiceWithRequests(int requestCount)
    {
        var mockRequests = new List<core_lod_sp_GetReinvestigationRequestsResult>();
        for (var i = 0; i < requestCount; i++)
        {
            mockRequests.Add(new core_lod_sp_GetReinvestigationRequestsResult());
        }

        _mockDataService.Setup(ds => ds.GetReinvestigationRequestsAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockRequests);
    }

    /// <summary>
    /// Tests that the WorkflowManagementService constructor throws <see cref="ArgumentNullException"/>
    /// when null parameters are provided.
    /// </summary>
    /// <param name="nullLogger">Whether to pass null for the logger parameter.</param>
    /// <param name="nullDataService">Whether to pass null for the data service parameter.</param>
    [Theory]
    [ClassData(typeof(WorkflowServiceConstructorNullParameterData))]
    public void Constructor_ThrowsArgumentNullException_WhenParametersAreNull(bool nullLogger, bool nullDataService)
    {
        // Arrange
        var logger = nullLogger ? null : _mockLogger.Object;
        var dataService = nullDataService ? null : _mockDataService.Object;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new WorkflowServiceImpl(logger!, dataService!, _mockResilienceService.Object));
    }

    #region GetReinvestigationRequests Tests

    /// <summary>
    /// Tests that GetReinvestigationRequests returns correct response for different request counts.
    /// </summary>
    /// <param name="requestCount">The number of mock requests to return.</param>
    [Theory(Timeout = 5000)]
    [ClassData(typeof(WorkflowServiceRequestScenariosData))]
    public async Task GetReinvestigationRequests_ReturnsCorrectResponse_ForDifferentRequestCounts(int requestCount)
    {
        // Arrange
        var request = new GetReinvestigationRequestsRequest { UserId = 123, Sarc = true };
        SetupDataServiceWithRequests(requestCount);
        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetReinvestigationRequests(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(requestCount, response.Count);
        Assert.Equal(requestCount, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetReinvestigationRequests returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetReinvestigationRequests_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetReinvestigationRequestsRequest { UserId = 123, Sarc = false };

        _mockDataService.Setup(ds => ds.GetReinvestigationRequestsAsync(request.UserId, request.Sarc, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<List<core_lod_sp_GetReinvestigationRequestsResult>>(null!));

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetReinvestigationRequests(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(0, response.Count);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetReinvestigationRequests returns empty response when data service returns empty list.
    /// </summary>
    [Fact]
    public async Task GetReinvestigationRequests_ReturnsEmptyResponse_WhenDataServiceReturnsEmptyList()
    {
        // Arrange
        var request = new GetReinvestigationRequestsRequest { UserId = 123, Sarc = true };
        var mockResults = new List<core_lod_sp_GetReinvestigationRequestsResult>();

        _mockDataService.Setup(ds => ds.GetReinvestigationRequestsAsync(request.UserId, request.Sarc, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetReinvestigationRequests(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(0, response.Count);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetReinvestigationRequests handles different name formats correctly.
    /// </summary>
    /// <param name="name">The name to test.</param>
    /// <param name="expectedDescription">The expected description format.</param>
    [Theory(Timeout = 5000)]
    [ClassData(typeof(WorkflowServiceNameFormatData))]
    public async Task GetReinvestigationRequests_HandlesDifferentNameFormatsCorrectly(string name, string expectedDescription)
    {
        // Arrange
        var request = new GetReinvestigationRequestsRequest { UserId = 123, Sarc = true };
        var mockResults = new List<core_lod_sp_GetReinvestigationRequestsResult>
        {
            new() { request_id = 1, Member_Name = name, Case_Id = "CASE001", Status = "Active" }
        };

        _mockDataService.Setup(ds => ds.GetReinvestigationRequestsAsync(request.UserId, request.Sarc, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetReinvestigationRequests(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.Items);
        Assert.Equal(expectedDescription, response.Items[0].Description);
    }

    /// <summary>
    /// Tests that GetReinvestigationRequests propagates different types of exceptions from data service.
    /// </summary>
    /// <param name="exceptionType">The type of exception to test.</param>
    /// <param name="message">The exception message.</param>
    [Theory(Timeout = 5000)]
    [ClassData(typeof(WorkflowServiceExceptionTypeData))]
    public async Task GetReinvestigationRequests_PropagatesDifferentExceptionTypes_FromDataService(Type exceptionType, string message)
    {
        // Arrange
        var request = new GetReinvestigationRequestsRequest { UserId = 123, Sarc = true };
        var exception = (Exception)Activator.CreateInstance(exceptionType, message)!;

        _mockDataService.Setup(ds => ds.GetReinvestigationRequestsAsync(request.UserId, request.Sarc, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetReinvestigationRequests(request, null!));
        
    }

    #endregion

    #region Core User Methods Tests

    #region GetMailingListForLOD Tests

    /// <summary>
    /// Tests that GetMailingListForLOD returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetMailingListForLOD_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetMailingListForLODRequest { RefId = 123, GroupId = 456, Status = 1, CallingService = "TestService" };
        var mockResults = new List<core_user_sp_GetMailingListForLODResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetMailingListForLODAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetMailingListForLOD(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetMailingListForLOD returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetMailingListForLOD_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetMailingListForLODRequest { RefId = 123, GroupId = 456, Status = 1, CallingService = "TestService" };

        _mockDataService.Setup(ds => ds.GetMailingListForLODAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetMailingListForLOD(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetMailingListForLOD propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetMailingListForLOD_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetMailingListForLODRequest { RefId = 123, GroupId = 456, Status = 1, CallingService = "TestService" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetMailingListForLODAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetMailingListForLOD(request, null!));
        
    }

    #endregion

    #region GetManagedUsers Tests

    /// <summary>
    /// Tests that GetManagedUsers returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetManagedUsers_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetManagedUsersRequest { Userid = 123, Ssn = "123-45-6789", Name = "John Doe", Status = 1, Role = 2, SrchUnit = 3, ShowAllUsers = true };
        var mockResults = new List<core_user_sp_GetManagedUsersResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetManagedUsersAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetManagedUsers(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetManagedUsers returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetManagedUsers_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetManagedUsersRequest { Userid = 123, Ssn = "123-45-6789", Name = "John Doe", Status = 1, Role = 2, SrchUnit = 3, ShowAllUsers = false };

        _mockDataService.Setup(ds => ds.GetManagedUsersAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetManagedUsers(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetManagedUsers propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetManagedUsers_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetManagedUsersRequest { Userid = 123, Ssn = "123-45-6789", Name = "John Doe", Status = 1, Role = 2, SrchUnit = 3, ShowAllUsers = true };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetManagedUsersAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetManagedUsers(request, null!));
        
    }

    #endregion

    #region GetMembersUserId Tests

    /// <summary>
    /// Tests that GetMembersUserId returns correct response when data service returns result.
    /// </summary>
    [Fact]
    public async Task GetMembersUserId_ReturnsCorrectResponse_WhenDataServiceReturnsResult()
    {
        // Arrange
        var request = new GetMembersUserIdRequest { MemberSsn = "123-45-6789" };
        var expectedUserId = 456;

        _mockDataService.Setup(ds => ds.GetMembersUserIdAsync(request.MemberSsn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUserId);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetMembersUserId(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedUserId, response.UserId);
    }

    /// <summary>
    /// Tests that GetMembersUserId propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetMembersUserId_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetMembersUserIdRequest { MemberSsn = "123-45-6789" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetMembersUserIdAsync(request.MemberSsn, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetMembersUserId(request, null!));
        
    }

    #endregion

    #region GetUserAltTitle Tests

    /// <summary>
    /// Tests that GetUserAltTitle returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetUserAltTitle_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetUserAltTitleRequest { UserId = 123, GroupId = 456 };
        var mockResults = new List<core_user_sp_GetUserAltTitleResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetUserAltTitleAsync(request.UserId, request.GroupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetUserAltTitle(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetUserAltTitle returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetUserAltTitle_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetUserAltTitleRequest { UserId = 123, GroupId = 456 };

        _mockDataService.Setup(ds => ds.GetUserAltTitleAsync(request.UserId, request.GroupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetUserAltTitle(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetUserAltTitle propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetUserAltTitle_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetUserAltTitleRequest { UserId = 123, GroupId = 456 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetUserAltTitleAsync(request.UserId, request.GroupId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetUserAltTitle(request, null!));

    }

    #endregion

    #region GetUserAltTitleByGroupCompo Tests

    /// <summary>
    /// Tests that GetUserAltTitleByGroupCompo returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetUserAltTitleByGroupCompo_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetUserAltTitleByGroupCompoRequest { GroupId = 123, WorkCompo = 456 };
        var mockResults = new List<core_user_sp_GetUserAltTitleByGroupCompoResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetUserAltTitleByGroupCompoAsync(request.GroupId, request.WorkCompo, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetUserAltTitleByGroupCompo(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetUserAltTitleByGroupCompo returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetUserAltTitleByGroupCompo_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetUserAltTitleByGroupCompoRequest { GroupId = 123, WorkCompo = 456 };

        _mockDataService.Setup(ds => ds.GetUserAltTitleByGroupCompoAsync(request.GroupId, request.WorkCompo, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetUserAltTitleByGroupCompo(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetUserAltTitleByGroupCompo propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetUserAltTitleByGroupCompo_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetUserAltTitleByGroupCompoRequest { GroupId = 123, WorkCompo = 456 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetUserAltTitleByGroupCompoAsync(request.GroupId, request.WorkCompo, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetUserAltTitleByGroupCompo(request, null!));

    }

    #endregion

    #region GetUserName Tests

    /// <summary>
    /// Tests that GetUserName returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetUserName_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetUserNameRequest { First = "John", Last = "Doe" };
        var mockResults = new List<core_user_sp_GetUserNameResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetUserNameAsync(request.First, request.Last, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetUserName(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetUserName returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetUserName_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetUserNameRequest { First = "John", Last = "Doe" };

        _mockDataService.Setup(ds => ds.GetUserNameAsync(request.First, request.Last, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetUserName(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetUserName propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetUserName_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetUserNameRequest { First = "John", Last = "Doe" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetUserNameAsync(request.First, request.Last, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetUserName(request, null!));

    }

    #endregion

    #region GetUsersAltTitleByGroup Tests

    /// <summary>
    /// Tests that GetUsersAltTitleByGroup returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetUsersAltTitleByGroup_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetUsersAltTitleByGroupRequest { GroupId = 123 };
        var mockResults = new List<core_user_sp_GetUsersAltTitleByGroupResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetUsersAltTitleByGroupAsync(request.GroupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetUsersAltTitleByGroup(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetUsersAltTitleByGroup returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetUsersAltTitleByGroup_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetUsersAltTitleByGroupRequest { GroupId = 123 };

        _mockDataService.Setup(ds => ds.GetUsersAltTitleByGroupAsync(request.GroupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetUsersAltTitleByGroup(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetUsersAltTitleByGroup propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetUsersAltTitleByGroup_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetUsersAltTitleByGroupRequest { GroupId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetUsersAltTitleByGroupAsync(request.GroupId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetUsersAltTitleByGroup(request, null!));

    }

    #endregion

    #region GetUsersOnline Tests

    /// <summary>
    /// Tests that GetUsersOnline returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetUsersOnline_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new EmptyRequest();
        var mockResults = new List<core_user_sp_GetUsersOnlineResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetUsersOnlineAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetUsersOnline(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetUsersOnline returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetUsersOnline_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new EmptyRequest();

        _mockDataService.Setup(ds => ds.GetUsersOnlineAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetUsersOnline(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetUsersOnline propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetUsersOnline_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new EmptyRequest();
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetUsersOnlineAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetUsersOnline(request, null!));

    }

    #endregion

    #region GetWhois Tests

    /// <summary>
    /// Tests that GetWhois returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetWhois_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetWhoisRequest { UserId = 123 };
        var mockResults = new List<core_user_sp_GetWhoisResult>
        {
            new() { UserId = 456, FirstName = "John", LastName = "Doe", Role = "Admin" },
            new() { UserId = 789, FirstName = "Jane", LastName = "Smith", Role = "User" }
        };

        _mockDataService.Setup(ds => ds.GetWhoisAsync(request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWhois(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(456, response.Items[0].UserId);
        Assert.Equal("John Doe", response.Items[0].UserName);
        Assert.Equal("Admin", response.Items[0].IpAddress);
        Assert.Equal("", response.Items[0].LastLogin);
        Assert.Equal(789, response.Items[1].UserId);
        Assert.Equal("Jane Smith", response.Items[1].UserName);
        Assert.Equal("User", response.Items[1].IpAddress);
        Assert.Equal("", response.Items[1].LastLogin);
    }

    /// <summary>
    /// Tests that GetWhois returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetWhois_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetWhoisRequest { UserId = 123 };

        _mockDataService.Setup(ds => ds.GetWhoisAsync(request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWhois(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetWhois propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetWhois_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetWhoisRequest { UserId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetWhoisAsync(request.UserId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetWhois(request, null!));

    }

    #endregion

    #region HasHQTechAccount Tests

    /// <summary>
    /// Tests that HasHQTechAccount returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task HasHQTechAccount_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new HasHQTechAccountRequest { OriginUserId = 123, UserEdipin = "ABC123" };
        var mockResults = new List<core_user_sp_HasHQTechAccountResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.HasHQTechAccountAsync(request.OriginUserId, request.UserEdipin, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.HasHQTechAccount(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that HasHQTechAccount returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task HasHQTechAccount_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new HasHQTechAccountRequest { OriginUserId = 123, UserEdipin = "ABC123" };

        _mockDataService.Setup(ds => ds.HasHQTechAccountAsync(request.OriginUserId, request.UserEdipin, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.HasHQTechAccount(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that HasHQTechAccount propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task HasHQTechAccount_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new HasHQTechAccountRequest { OriginUserId = 123, UserEdipin = "ABC123" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.HasHQTechAccountAsync(request.OriginUserId, request.UserEdipin, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.HasHQTechAccount(request, null!));

    }

    #endregion

    #region IsFinalStatusCode Tests

    /// <summary>
    /// Tests that IsFinalStatusCode returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task IsFinalStatusCode_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new IsFinalStatusCodeRequest { StatusId = 1 };
        var mockResults = new List<core_user_sp_IsFinalStatusCodeResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.IsFinalStatusCodeAsync((byte?)request.StatusId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.IsFinalStatusCode(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that IsFinalStatusCode returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task IsFinalStatusCode_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new IsFinalStatusCodeRequest { StatusId = 1 };

        _mockDataService.Setup(ds => ds.IsFinalStatusCodeAsync((byte?)request.StatusId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.IsFinalStatusCode(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that IsFinalStatusCode propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task IsFinalStatusCode_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new IsFinalStatusCodeRequest { StatusId = 1 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.IsFinalStatusCodeAsync((byte?)request.StatusId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.IsFinalStatusCode(request, null!));

    }

    #endregion

    #region Logout Tests

    /// <summary>
    /// Tests that Logout returns correct response when data service returns result.
    /// </summary>
    [Fact]
    public async Task Logout_ReturnsCorrectResponse_WhenDataServiceReturnsResult()
    {
        // Arrange
        var request = new LogoutRequest { UserId = 123 };
        var expectedResult = 1;

        _mockDataService.Setup(ds => ds.LogoutAsync(request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.Logout(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedResult, response.Result);
    }

    /// <summary>
    /// Tests that Logout propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task Logout_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new LogoutRequest { UserId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.LogoutAsync(request.UserId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.Logout(request, null!));

    }

    #endregion

    #region RegisterUser Tests

    /// <summary>
    /// Tests that RegisterUser returns correct response when data service returns result.
    /// </summary>
    [Fact]
    public async Task RegisterUser_ReturnsCorrectResponse_WhenDataServiceReturnsResult()
    {
        // Arrange
        var request = new RegisterUserRequest { UserId = 123, WorkCompo = "Component1", ReceiveEmail = true, GroupId = 456, AccountStatus = 1, ExpirationDate = "2024-12-31" };
        var expectedResult = 1;

        _mockDataService.Setup(ds => ds.RegisterUserAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.RegisterUser(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedResult, response.Result);
    }

    /// <summary>
    /// Tests that RegisterUser propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task RegisterUser_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new RegisterUserRequest { UserId = 123, WorkCompo = "Component1", ReceiveEmail = true, GroupId = 456, AccountStatus = 1, ExpirationDate = "2024-12-31" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.RegisterUserAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.RegisterUser(request, null!));

    }

    #endregion

    #region RegisterUserRole Tests

    /// <summary>
    /// Tests that RegisterUserRole returns correct response when data service returns result.
    /// </summary>
    [Fact]
    public async Task RegisterUserRole_ReturnsCorrectResponse_WhenDataServiceReturnsResult()
    {
        // Arrange
        var request = new RegisterUserRoleRequest { UserId = 123, GroupId = 456, Status = 1 };
        var expectedUserRoleId = 789;

        _mockDataService.Setup(ds => ds.RegisterUserRoleAsync(request.UserId, (short?)request.GroupId, (byte?)request.Status, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUserRoleId);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.RegisterUserRole(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedUserRoleId, response.UserRoleId);
    }

    /// <summary>
    /// Tests that RegisterUserRole propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task RegisterUserRole_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new RegisterUserRoleRequest { UserId = 123, GroupId = 456, Status = 1 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.RegisterUserRoleAsync(request.UserId, (short?)request.GroupId, (byte?)request.Status, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.RegisterUserRole(request, null!));

    }

    #endregion

    #region SearchMemberData Tests

    /// <summary>
    /// Tests that SearchMemberData returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task SearchMemberData_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new SearchMemberDataRequest { UserId = 123, Ssn = "123-45-6789", LastName = "Doe", FirstName = "John", MiddleName = "A", SrchUnit = 456, RptView = 1 };
        var mockResults = new List<core_user_sp_SearchMemberDataResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.SearchMemberDataAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.SearchMemberData(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that SearchMemberData returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task SearchMemberData_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new SearchMemberDataRequest { UserId = 123, Ssn = "123-45-6789", LastName = "Doe", FirstName = "John", MiddleName = "A", SrchUnit = 456, RptView = 1 };

        _mockDataService.Setup(ds => ds.SearchMemberDataAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.SearchMemberData(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that SearchMemberData propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task SearchMemberData_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new SearchMemberDataRequest { UserId = 123, Ssn = "123-45-6789", LastName = "Doe", FirstName = "John", MiddleName = "A", SrchUnit = 456, RptView = 1 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.SearchMemberDataAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.SearchMemberData(request, null!));

    }

    #endregion

    #region SearchMemberDataTest Tests

    /// <summary>
    /// Tests that SearchMemberDataTest returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task SearchMemberDataTest_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new SearchMemberDataTestRequest { UserId = 123, Ssn = "123-45-6789", Name = "John Doe", SrchUnit = 456, RptView = 1 };
        var mockResults = new List<core_user_sp_SearchMemberData_TestResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.SearchMemberDataTestAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.SearchMemberDataTest(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that SearchMemberDataTest returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task SearchMemberDataTest_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new SearchMemberDataTestRequest { UserId = 123, Ssn = "123-45-6789", Name = "John Doe", SrchUnit = 456, RptView = 1 };

        _mockDataService.Setup(ds => ds.SearchMemberDataTestAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.SearchMemberDataTest(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that SearchMemberDataTest propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task SearchMemberDataTest_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new SearchMemberDataTestRequest { UserId = 123, Ssn = "123-45-6789", Name = "John Doe", SrchUnit = 456, RptView = 1 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.SearchMemberDataTestAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.SearchMemberDataTest(request, null!));

    }

    #endregion

    #region UpdateAccountStatus Tests

    /// <summary>
    /// Tests that UpdateAccountStatus returns correct response when data service returns result.
    /// </summary>
    [Fact]
    public async Task UpdateAccountStatus_ReturnsCorrectResponse_WhenDataServiceReturnsResult()
    {
        // Arrange
        var request = new UpdateAccountStatusRequest { UserId = 123, AccountStatus = 1, ExpirationDate = "2024-12-31", Comment = "Test comment" };
        var expectedResult = 1;

        _mockDataService.Setup(ds => ds.UpdateAccountStatusAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.UpdateAccountStatus(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedResult, response.Result);
    }

    /// <summary>
    /// Tests that UpdateAccountStatus propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task UpdateAccountStatus_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new UpdateAccountStatusRequest { UserId = 123, AccountStatus = 1, ExpirationDate = "2024-12-31", Comment = "Test comment" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.UpdateAccountStatusAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.UpdateAccountStatus(request, null!));

    }

    #endregion

    #region UpdateLogin Tests

    /// <summary>
    /// Tests that UpdateLogin returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task UpdateLogin_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new UpdateLoginRequest { UserId = 123, SessionId = "session123", RemoteAddr = "127.0.0.1" };
        var mockResults = new List<core_user_sp_UpdateLoginResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.UpdateLoginAsync(request.UserId, request.SessionId, request.RemoteAddr, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.UpdateLogin(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that UpdateLogin returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task UpdateLogin_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new UpdateLoginRequest { UserId = 123, SessionId = "session123", RemoteAddr = "127.0.0.1" };

        _mockDataService.Setup(ds => ds.UpdateLoginAsync(request.UserId, request.SessionId, request.RemoteAddr, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.UpdateLogin(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that UpdateLogin propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task UpdateLogin_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new UpdateLoginRequest { UserId = 123, SessionId = "session123", RemoteAddr = "127.0.0.1" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.UpdateLoginAsync(request.UserId, request.SessionId, request.RemoteAddr, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.UpdateLogin(request, null!));

    }

    #endregion

    #region UpdateManagedSettings Tests

    /// <summary>
    /// Tests that UpdateManagedSettings returns correct response when data service returns result.
    /// </summary>
    [Fact]
    public async Task UpdateManagedSettings_ReturnsCorrectResponse_WhenDataServiceReturnsResult()
    {
        // Arrange
        var request = new UpdateManagedSettingsRequest { UserId = 123, Compo = "Component1", RoleId = 456, GroupId = 1, Comment = "Test comment", ReceiveEmail = true, ExpirationDate = "2024-12-31" };
        var expectedResult = 1;

        _mockDataService.Setup(ds => ds.UpdateManagedSettingsAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.UpdateManagedSettings(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedResult, response.Result);
    }

    /// <summary>
    /// Tests that UpdateManagedSettings propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task UpdateManagedSettings_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new UpdateManagedSettingsRequest { UserId = 123, Compo = "Component1", RoleId = 456, GroupId = 1, Comment = "Test comment", ReceiveEmail = true, ExpirationDate = "2024-12-31" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.UpdateManagedSettingsAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.UpdateManagedSettings(request, null!));

    }

    #endregion

    #region UpdateUserAltTitle Tests

    /// <summary>
    /// Tests that UpdateUserAltTitle returns correct response when data service returns result.
    /// </summary>
    [Fact]
    public async Task UpdateUserAltTitle_ReturnsCorrectResponse_WhenDataServiceReturnsResult()
    {
        // Arrange
        var request = new UpdateUserAltTitleRequest { UserId = 123, GroupId = 456, NewTitle = "New Title" };
        var expectedResult = 1;

        _mockDataService.Setup(ds => ds.UpdateUserAltTitleAsync(request.UserId, request.GroupId, request.NewTitle, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.UpdateUserAltTitle(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedResult, response.Result);
    }

    /// <summary>
    /// Tests that UpdateUserAltTitle propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task UpdateUserAltTitle_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new UpdateUserAltTitleRequest { UserId = 123, GroupId = 456, NewTitle = "New Title" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.UpdateUserAltTitleAsync(request.UserId, request.GroupId, request.NewTitle, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.UpdateUserAltTitle(request, null!));

    }

    #endregion

    #endregion

    #region Core Workflow Methods

    #region AddSignature Tests

    /// <summary>
    /// Tests that AddSignature returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task AddSignature_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new AddSignatureRequest { RefId = 123, ModuleType = 1, UserId = 456, ActionId = 789, GroupId = 1, StatusIn = 2, StatusOut = 3 };
        var mockResults = new List<core_workflow_sp_AddSignatureResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.AddSignatureAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.AddSignature(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that AddSignature returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task AddSignature_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new AddSignatureRequest { RefId = 123, ModuleType = 1, UserId = 456, ActionId = 789, GroupId = 1, StatusIn = 2, StatusOut = 3 };

        _mockDataService.Setup(ds => ds.AddSignatureAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.AddSignature(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that AddSignature propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task AddSignature_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new AddSignatureRequest { RefId = 123, ModuleType = 1, UserId = 456, ActionId = 789, GroupId = 1, StatusIn = 2, StatusOut = 3 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.AddSignatureAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddSignature(request, null!));

    }

    #endregion

    #region CopyActions Tests

    /// <summary>
    /// Tests that CopyActions returns correct response when data service returns result.
    /// </summary>
    [Fact]
    public async Task CopyActions_ReturnsCorrectResponse_WhenDataServiceReturnsResult()
    {
        // Arrange
        var request = new CopyActionsRequest { DestWsoid = 123, SrcWsoid = 456 };
        var expectedResult = 1;

        _mockDataService.Setup(ds => ds.CopyActionsAsync(request.DestWsoid, request.SrcWsoid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.CopyActions(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedResult, response.Result);
    }

    /// <summary>
    /// Tests that CopyActions propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task CopyActions_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new CopyActionsRequest { DestWsoid = 123, SrcWsoid = 456 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.CopyActionsAsync(request.DestWsoid, request.SrcWsoid, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CopyActions(request, null!));

    }

    #endregion

    #region CopyRules Tests

    /// <summary>
    /// Tests that CopyRules returns correct response when data service returns result.
    /// </summary>
    [Fact]
    public async Task CopyRules_ReturnsCorrectResponse_WhenDataServiceReturnsResult()
    {
        // Arrange
        var request = new CopyRulesRequest { DestWsoid = 123, SrcWsoid = 456 };
        var expectedResult = 1;

        _mockDataService.Setup(ds => ds.CopyRulesAsync(request.DestWsoid, request.SrcWsoid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.CopyRules(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedResult, response.Result);
    }

    /// <summary>
    /// Tests that CopyRules propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task CopyRules_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new CopyRulesRequest { DestWsoid = 123, SrcWsoid = 456 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.CopyRulesAsync(request.DestWsoid, request.SrcWsoid, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CopyRules(request, null!));

    }

    #endregion

    #region CopyWorkflow Tests

    /// <summary>
    /// Tests that CopyWorkflow returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task CopyWorkflow_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new CopyWorkflowRequest { FromId = 123, ToId = 456 };
        var mockResults = new List<core_workflow_sp_CopyWorkflowResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.CopyWorkflowAsync(request.FromId, request.ToId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.CopyWorkflow(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that CopyWorkflow returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task CopyWorkflow_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new CopyWorkflowRequest { FromId = 123, ToId = 456 };

        _mockDataService.Setup(ds => ds.CopyWorkflowAsync(request.FromId, request.ToId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.CopyWorkflow(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that CopyWorkflow propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task CopyWorkflow_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new CopyWorkflowRequest { FromId = 123, ToId = 456 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.CopyWorkflowAsync(request.FromId, request.ToId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CopyWorkflow(request, null!));

    }

    #endregion

    #region DeleteStatusCode Tests

    /// <summary>
    /// Tests that DeleteStatusCode returns correct response when data service returns result.
    /// </summary>
    [Fact]
    public async Task DeleteStatusCode_ReturnsCorrectResponse_WhenDataServiceReturnsResult()
    {
        // Arrange
        var request = new DeleteStatusCodeRequest { StatusId = 123 };
        var expectedResult = 1;

        _mockDataService.Setup(ds => ds.DeleteStatusCodeAsync(request.StatusId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.DeleteStatusCode(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedResult, response.Result);
    }

    /// <summary>
    /// Tests that DeleteStatusCode propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task DeleteStatusCode_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new DeleteStatusCodeRequest { StatusId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.DeleteStatusCodeAsync(request.StatusId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteStatusCode(request, null!));

    }

    #endregion

    #region GetActionsByStep Tests

    /// <summary>
    /// Tests that GetActionsByStep returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetActionsByStep_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetActionsByStepRequest { StepId = 123 };
        var mockResults = new List<core_workflow_sp_GetActionsByStepResult>
        {
            new() { wso_id = 123, wsa_id = 456, actionType = 1, target = 789, data = 101, text = "Test Action" },
            new() { wso_id = 123, wsa_id = 457, actionType = 2, target = 790, data = null, text = "Another Action" }
        };

        _mockDataService.Setup(ds => ds.GetActionsByStepAsync(request.StepId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetActionsByStep(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(456, response.Items[0].ActionId);
        Assert.Equal(123, response.Items[0].StepId);
        Assert.Equal("1", response.Items[0].ActionType);
        Assert.Equal("Test Action", response.Items[0].ActionDescription);
        Assert.Equal(457, response.Items[1].ActionId);
        Assert.Equal(123, response.Items[1].StepId);
        Assert.Equal("2", response.Items[1].ActionType);
        Assert.Equal("Another Action", response.Items[1].ActionDescription);
    }

    /// <summary>
    /// Tests that GetActionsByStep returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetActionsByStep_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetActionsByStepRequest { StepId = 123 };

        _mockDataService.Setup(ds => ds.GetActionsByStepAsync(request.StepId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetActionsByStep(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetActionsByStep propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetActionsByStep_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetActionsByStepRequest { StepId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetActionsByStepAsync(request.StepId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetActionsByStep(request, null!));

    }

    #endregion

    #region GetActiveCases Tests

    /// <summary>
    /// Tests that GetActiveCases returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetActiveCases_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetActiveCasesRequest { RefId = 123, GroupId = 456 };
        var mockResults = new List<core_workflow_sp_GetActiveCasesResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetActiveCasesAsync(request.RefId, (short?)request.GroupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetActiveCases(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetActiveCases returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetActiveCases_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetActiveCasesRequest { RefId = 123, GroupId = 456 };

        _mockDataService.Setup(ds => ds.GetActiveCasesAsync(request.RefId, (short?)request.GroupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetActiveCases(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetActiveCases propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetActiveCases_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetActiveCasesRequest { RefId = 123, GroupId = 456 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetActiveCasesAsync(request.RefId, (short?)request.GroupId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetActiveCases(request, null!));

    }

    #endregion

    #region GetAllFindingByReasonOf Tests

    /// <summary>
    /// Tests that GetAllFindingByReasonOf returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetAllFindingByReasonOf_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new EmptyRequest();
        var mockResults = new List<core_workflow_sp_GetAllFindingByReasonOfResult>
        {
            new() { Id = 1, Description = "Finding One" },
            new() { Id = 2, Description = "Finding Two" }
        };

        _mockDataService.Setup(ds => ds.GetAllFindingByReasonOfAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllFindingByReasonOf(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(1, response.Items[0].FindingId);
        Assert.Equal("Finding One", response.Items[0].Description);
        Assert.Equal(2, response.Items[1].FindingId);
        Assert.Equal("Finding Two", response.Items[1].Description);
    }

    /// <summary>
    /// Tests that GetAllFindingByReasonOf returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetAllFindingByReasonOf_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new EmptyRequest();

        _mockDataService.Setup(ds => ds.GetAllFindingByReasonOfAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllFindingByReasonOf(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetAllFindingByReasonOf propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetAllFindingByReasonOf_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new EmptyRequest();
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetAllFindingByReasonOfAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetAllFindingByReasonOf(request, null!));

    }

    #endregion

    #region GetAllLocks Tests

    /// <summary>
    /// Tests that GetAllLocks returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetAllLocks_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new EmptyRequest();
        var mockResults = new List<core_workflow_sp_GetAllLocksResult>
        {
            new() { lockId = 1, userId = 100, moduleName = "ModuleA", lockTime = new DateTime(2023, 10, 1) },
            new() { lockId = 2, userId = 200, moduleName = "ModuleB", lockTime = new DateTime(2023, 10, 2) }
        };

        _mockDataService.Setup(ds => ds.GetAllLocksAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLocks(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(1, response.Items[0].LockId);
        Assert.Equal(100, response.Items[0].UserId);
        Assert.Equal("ModuleA", response.Items[0].LockType);
        Assert.Equal("2023-10-01", response.Items[0].LockTime);
        Assert.Equal(2, response.Items[1].LockId);
        Assert.Equal(200, response.Items[1].UserId);
        Assert.Equal("ModuleB", response.Items[1].LockType);
        Assert.Equal("2023-10-02", response.Items[1].LockTime);
    }

    /// <summary>
    /// Tests that GetAllLocks returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetAllLocks_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new EmptyRequest();

        _mockDataService.Setup(ds => ds.GetAllLocksAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLocks(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetAllLocks propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetAllLocks_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new EmptyRequest();
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetAllLocksAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetAllLocks(request, null!));

    }

    #endregion

    #region GetCancelReasons Tests

    /// <summary>
    /// Tests that GetCancelReasons returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetCancelReasons_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetCancelReasonsRequest { WorkflowId = 123, IsFormal = true };
        var mockResults = new List<core_workflow_sp_GetCancelReasonsResult>
        {
            new() { Id = 1, Description = "Cancel Reason One" },
            new() { Id = 2, Description = "Cancel Reason Two" }
        };

        _mockDataService.Setup(ds => ds.GetCancelReasonsAsync((byte?)request.WorkflowId, request.IsFormal, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetCancelReasons(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(1, response.Items[0].ReasonId);
        Assert.Equal("Cancel Reason One", response.Items[0].ReasonText);
        Assert.Equal(2, response.Items[1].ReasonId);
        Assert.Equal("Cancel Reason Two", response.Items[1].ReasonText);
    }

    /// <summary>
    /// Tests that GetCancelReasons returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetCancelReasons_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetCancelReasonsRequest { WorkflowId = 123, IsFormal = true };

        _mockDataService.Setup(ds => ds.GetCancelReasonsAsync((byte?)request.WorkflowId, request.IsFormal, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetCancelReasons(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetCancelReasons propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetCancelReasons_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetCancelReasonsRequest { WorkflowId = 123, IsFormal = true };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetCancelReasonsAsync((byte?)request.WorkflowId, request.IsFormal, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetCancelReasons(request, null!));

    }

    #endregion

    #region GetCreatableByGroup Tests

    /// <summary>
    /// Tests that GetCreatableByGroup returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetCreatableByGroup_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetCreatableByGroupRequest { Compo = "Component1", Module = 1, GroupId = 456 };
        var mockResults = new List<core_workflow_sp_GetCreatableByGroupResult>
        {
            new() { workFlowId = 1, compo = "Component1", title = "Workflow One", formal = true, moduleId = 1, active = true, initialStatus = 100, description = "Description One" },
            new() { workFlowId = 2, compo = "Component1", title = "Workflow Two", formal = false, moduleId = 1, active = true, initialStatus = 200, description = "Description Two" }
        };

        _mockDataService.Setup(ds => ds.GetCreatableByGroupAsync(request.Compo, (byte?)request.Module, (byte?)request.GroupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetCreatableByGroup(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(1, response.Items[0].WorkflowId);
        Assert.Equal("Workflow One", response.Items[0].WorkflowName);
        Assert.Equal(456, response.Items[0].GroupId);
        Assert.Equal(2, response.Items[1].WorkflowId);
        Assert.Equal("Workflow Two", response.Items[1].WorkflowName);
        Assert.Equal(456, response.Items[1].GroupId);
    }

    /// <summary>
    /// Tests that GetCreatableByGroup returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetCreatableByGroup_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetCreatableByGroupRequest { Compo = "Component1", Module = 1, GroupId = 456 };

        _mockDataService.Setup(ds => ds.GetCreatableByGroupAsync(request.Compo, (byte?)request.Module, (byte?)request.GroupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetCreatableByGroup(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetCreatableByGroup propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetCreatableByGroup_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetCreatableByGroupRequest { Compo = "Component1", Module = 1, GroupId = 456 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetCreatableByGroupAsync(request.Compo, (byte?)request.Module, (byte?)request.GroupId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetCreatableByGroup(request, null!));

    }

    #endregion

    #region GetFindingByReasonOfById Tests

    /// <summary>
    /// Tests that GetFindingByReasonOfById returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetFindingByReasonOfById_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetFindingByReasonOfByIdRequest { Id = 123 };
        var mockResults = new List<core_workflow_sp_GetFindingByReasonOfByIdResult>
        {
            new() { Id = 1, Description = "Finding Description One" },
            new() { Id = 2, Description = "Finding Description Two" }
        };

        _mockDataService.Setup(ds => ds.GetFindingByReasonOfByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetFindingByReasonOfById(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(1, response.Items[0].FindingId);
        Assert.Equal("Finding Description One", response.Items[0].Description);
        Assert.Equal(1, response.Items[0].Id);
        Assert.Equal(2, response.Items[1].FindingId);
        Assert.Equal("Finding Description Two", response.Items[1].Description);
        Assert.Equal(2, response.Items[1].Id);
    }

    /// <summary>
    /// Tests that GetFindingByReasonOfById returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetFindingByReasonOfById_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetFindingByReasonOfByIdRequest { Id = 123 };

        _mockDataService.Setup(ds => ds.GetFindingByReasonOfByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetFindingByReasonOfById(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetFindingByReasonOfById propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetFindingByReasonOfById_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetFindingByReasonOfByIdRequest { Id = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetFindingByReasonOfByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetFindingByReasonOfById(request, null!));

    }

    #endregion

    #region GetFindings Tests

    /// <summary>
    /// Tests that GetFindings returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetFindings_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetFindingsRequest { WorkflowId = 123, GroupId = 456 };
        var mockResults = new List<core_workflow_sp_GetFindingsResult>
        {
            new() { Id = 1, FindingType = "Type1", Description = "Finding Description One" },
            new() { Id = 2, FindingType = "Type2", Description = "Finding Description Two" }
        };

        _mockDataService.Setup(ds => ds.GetFindingsAsync((byte?)request.WorkflowId, request.GroupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetFindings(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(1, response.Items[0].FindingId);
        Assert.Equal(123, response.Items[0].WorkflowId);
        Assert.Equal(456, response.Items[0].GroupId);
        Assert.Equal("Finding Description One", response.Items[0].FindingText);
        Assert.Equal(2, response.Items[1].FindingId);
        Assert.Equal(123, response.Items[1].WorkflowId);
        Assert.Equal(456, response.Items[1].GroupId);
        Assert.Equal("Finding Description Two", response.Items[1].FindingText);
    }

    /// <summary>
    /// Tests that GetFindings returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetFindings_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetFindingsRequest { WorkflowId = 123, GroupId = 456 };

        _mockDataService.Setup(ds => ds.GetFindingsAsync((byte?)request.WorkflowId, request.GroupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetFindings(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetFindings propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetFindings_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetFindingsRequest { WorkflowId = 123, GroupId = 456 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetFindingsAsync((byte?)request.WorkflowId, request.GroupId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetFindings(request, null!));

    }

    #endregion

    #region GetModuleFromWorkflow Tests

    /// <summary>
    /// Tests that GetModuleFromWorkflow returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetModuleFromWorkflow_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetModuleFromWorkflowRequest { WorkflowId = 123 };
        var mockResults = new List<core_workflow_sp_GetModuleFromWorkflowResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetModuleFromWorkflowAsync(request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetModuleFromWorkflow(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetModuleFromWorkflow returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetModuleFromWorkflow_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetModuleFromWorkflowRequest { WorkflowId = 123 };

        _mockDataService.Setup(ds => ds.GetModuleFromWorkflowAsync(request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetModuleFromWorkflow(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetModuleFromWorkflow propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetModuleFromWorkflow_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetModuleFromWorkflowRequest { WorkflowId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetModuleFromWorkflowAsync(request.WorkflowId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetModuleFromWorkflow(request, null!));

    }

    #endregion

    #region GetPageAccessByGroup Tests

    /// <summary>
    /// Tests that GetPageAccessByGroup returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetPageAccessByGroup_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetPageAccessByGroupRequest { Workflow = 123, Status = 1, Group = 456 };
        var mockResults = new List<core_workflow_sp_GetPageAccessByGroupResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetPageAccessByGroupAsync((byte?)request.Workflow, request.Status, (byte?)request.Group, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetPageAccessByGroup(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetPageAccessByGroup returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetPageAccessByGroup_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetPageAccessByGroupRequest { Workflow = 123, Status = 1, Group = 456 };

        _mockDataService.Setup(ds => ds.GetPageAccessByGroupAsync((byte?)request.Workflow, request.Status, (byte?)request.Group, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetPageAccessByGroup(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetPageAccessByGroup propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetPageAccessByGroup_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetPageAccessByGroupRequest { Workflow = 123, Status = 1, Group = 456 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetPageAccessByGroupAsync((byte?)request.Workflow, request.Status, (byte?)request.Group, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetPageAccessByGroup(request, null!));

    }

    #endregion

    #region GetPageAccessByWorkflowView Tests

    /// <summary>
    /// Tests that GetPageAccessByWorkflowView returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetPageAccessByWorkflowView_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetPageAccessByWorkflowViewRequest { Compo = "Component1", Workflow = 123, Status = 1 };
        var mockResults = new List<core_workflow_sp_GetPageAccessByWorkflowViewResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetPageAccessByWorkflowViewAsync(request.Compo, (byte?)request.Workflow, request.Status, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetPageAccessByWorkflowView(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetPageAccessByWorkflowView returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetPageAccessByWorkflowView_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetPageAccessByWorkflowViewRequest { Compo = "Component1", Workflow = 123, Status = 1 };

        _mockDataService.Setup(ds => ds.GetPageAccessByWorkflowViewAsync(request.Compo, (byte?)request.Workflow, request.Status, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetPageAccessByWorkflowView(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetPageAccessByWorkflowView propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetPageAccessByWorkflowView_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetPageAccessByWorkflowViewRequest { Compo = "Component1", Workflow = 123, Status = 1 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetPageAccessByWorkflowViewAsync(request.Compo, (byte?)request.Workflow, request.Status, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetPageAccessByWorkflowView(request, null!));

    }

    #endregion

    #region GetPagesByWorkflowId Tests

    /// <summary>
    /// Tests that GetPagesByWorkflowId returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetPagesByWorkflowId_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetPagesByWorkflowIdRequest { WorkflowId = 123 };
        var mockResults = new List<core_workflow_sp_GetPagesByWorkflowIdResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetPagesByWorkflowIdAsync(request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetPagesByWorkflowId(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetPagesByWorkflowId returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetPagesByWorkflowId_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetPagesByWorkflowIdRequest { WorkflowId = 123 };

        _mockDataService.Setup(ds => ds.GetPagesByWorkflowIdAsync(request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetPagesByWorkflowId(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetPagesByWorkflowId propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetPagesByWorkflowId_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetPagesByWorkflowIdRequest { WorkflowId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetPagesByWorkflowIdAsync(request.WorkflowId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetPagesByWorkflowId(request, null!));

    }

    #endregion

    #region GetPermissions Tests

    /// <summary>
    /// Tests that GetPermissions returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetPermissions_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetPermissionsRequest { WorkflowId = 123 };
        var mockResults = new List<core_Workflow_sp_GetPermissionsResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetPermissionsAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetPermissions(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetPermissions returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetPermissions_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetPermissionsRequest { WorkflowId = 123 };

        _mockDataService.Setup(ds => ds.GetPermissionsAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetPermissions(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetPermissions propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetPermissions_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetPermissionsRequest { WorkflowId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetPermissionsAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetPermissions(request, null!));

    }

    #endregion

    #region GetPermissionsByCompo Tests

    /// <summary>
    /// Tests that GetPermissionsByCompo returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetPermissionsByCompo_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetPermissionsByCompoRequest { WorkflowId = 123, Compo = "Component1" };
        var mockResults = new List<core_Workflow_sp_GetPermissionsByCompoResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetPermissionsByCompoAsync((byte?)request.WorkflowId, request.Compo, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetPermissionsByCompo(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetPermissionsByCompo returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetPermissionsByCompo_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetPermissionsByCompoRequest { WorkflowId = 123, Compo = "Component1" };

        _mockDataService.Setup(ds => ds.GetPermissionsByCompoAsync((byte?)request.WorkflowId, request.Compo, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetPermissionsByCompo(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetPermissionsByCompo propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetPermissionsByCompo_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetPermissionsByCompoRequest { WorkflowId = 123, Compo = "Component1" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetPermissionsByCompoAsync((byte?)request.WorkflowId, request.Compo, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetPermissionsByCompo(request, null!));

    }

    #endregion

    #region GetReturnReasons Tests

    /// <summary>
    /// Tests that GetReturnReasons returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetReturnReasons_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetReturnReasonsRequest { WorkflowId = 123 };
        var mockResults = new List<core_workflow_sp_GetReturnReasonsResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetReturnReasonsAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetReturnReasons(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetReturnReasons returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetReturnReasons_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetReturnReasonsRequest { WorkflowId = 123 };

        _mockDataService.Setup(ds => ds.GetReturnReasonsAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetReturnReasons(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetReturnReasons propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetReturnReasons_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetReturnReasonsRequest { WorkflowId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetReturnReasonsAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetReturnReasons(request, null!));

    }

    #endregion

    #region GetRwoaReasons Tests

    /// <summary>
    /// Tests that GetRwoaReasons returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetRwoaReasons_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetRwoaReasonsRequest { WorkflowId = 123 };
        var mockResults = new List<core_workflow_sp_GetRwoaReasonsResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetRwoaReasonsAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetRwoaReasons(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetRwoaReasons returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetRwoaReasons_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetRwoaReasonsRequest { WorkflowId = 123 };

        _mockDataService.Setup(ds => ds.GetRwoaReasonsAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetRwoaReasons(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetRwoaReasons propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetRwoaReasons_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetRwoaReasonsRequest { WorkflowId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetRwoaReasonsAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetRwoaReasons(request, null!));

    }

    #endregion

    #region GetStatusCodesByCompo Tests

    /// <summary>
    /// Tests that GetStatusCodesByCompo returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByCompo_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetStatusCodesByCompoRequest { Compo = "Component1" };
        var mockResults = new List<core_workflow_sp_GetStatusCodesByCompoResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetStatusCodesByCompoAsync(request.Compo, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodesByCompo(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetStatusCodesByCompo returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByCompo_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetStatusCodesByCompoRequest { Compo = "Component1" };

        _mockDataService.Setup(ds => ds.GetStatusCodesByCompoAsync(request.Compo, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodesByCompo(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetStatusCodesByCompo propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByCompo_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetStatusCodesByCompoRequest { Compo = "Component1" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetStatusCodesByCompoAsync(request.Compo, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetStatusCodesByCompo(request, null!));

    }

    #endregion

    #region GetStatusCodesByCompoAndModule Tests

    /// <summary>
    /// Tests that GetStatusCodesByCompoAndModule returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByCompoAndModule_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetStatusCodesByCompoAndModuleRequest { Compo = "Component1", Module = 1 };
        var mockResults = new List<core_workflow_sp_GetStatusCodesByCompoAndModuleResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetStatusCodesByCompoAndModuleAsync(request.Compo, (byte?)request.Module, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodesByCompoAndModule(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetStatusCodesByCompoAndModule returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByCompoAndModule_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetStatusCodesByCompoAndModuleRequest { Compo = "Component1", Module = 1 };

        _mockDataService.Setup(ds => ds.GetStatusCodesByCompoAndModuleAsync(request.Compo, (byte?)request.Module, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodesByCompoAndModule(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetStatusCodesByCompoAndModule propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByCompoAndModule_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetStatusCodesByCompoAndModuleRequest { Compo = "Component1", Module = 1 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetStatusCodesByCompoAndModuleAsync(request.Compo, (byte?)request.Module, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetStatusCodesByCompoAndModule(request, null!));

    }

    #endregion

    #region GetStatusCodesBySignCode Tests

    /// <summary>
    /// Tests that GetStatusCodesBySignCode returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesBySignCode_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetStatusCodesBySignCodeRequest { GroupId = 123, Module = 1 };
        var mockResults = new List<core_workflow_sp_GetStatusCodesBySignCodeResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetStatusCodesBySignCodeAsync((short?)request.GroupId, (byte?)request.Module, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodesBySignCode(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetStatusCodesBySignCode returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesBySignCode_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetStatusCodesBySignCodeRequest { GroupId = 123, Module = 1 };

        _mockDataService.Setup(ds => ds.GetStatusCodesBySignCodeAsync((short?)request.GroupId, (byte?)request.Module, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodesBySignCode(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetStatusCodesBySignCode propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesBySignCode_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetStatusCodesBySignCodeRequest { GroupId = 123, Module = 1 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetStatusCodesBySignCodeAsync((short?)request.GroupId, (byte?)request.Module, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetStatusCodesBySignCode(request, null!));

    }

    #endregion

    #region GetStatusCodesByWorkflow Tests

    /// <summary>
    /// Tests that GetStatusCodesByWorkflow returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByWorkflow_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetStatusCodesByWorkflowRequest { WorkflowId = 123 };
        var mockResults = new List<core_workflow_sp_GetStatusCodesByWorkflowResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetStatusCodesByWorkflowAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodesByWorkflow(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetStatusCodesByWorkflow returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByWorkflow_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetStatusCodesByWorkflowRequest { WorkflowId = 123 };

        _mockDataService.Setup(ds => ds.GetStatusCodesByWorkflowAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodesByWorkflow(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetStatusCodesByWorkflow propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByWorkflow_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetStatusCodesByWorkflowRequest { WorkflowId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetStatusCodesByWorkflowAsync((byte?)request.WorkflowId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetStatusCodesByWorkflow(request, null!));

    }

    #endregion

    #region GetStatusCodesByWorkflowAndAccessScope Tests

    /// <summary>
    /// Tests that GetStatusCodesByWorkflowAndAccessScope returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByWorkflowAndAccessScope_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetStatusCodesByWorkflowAndAccessScopeRequest { WorkflowId = 123, AccessScope = 1 };
        var mockResults = new List<core_workflow_sp_GetStatusCodesByWorkflowAndAccessScopeResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetStatusCodesByWorkflowAndAccessScopeAsync((byte?)request.WorkflowId, (byte?)request.AccessScope, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodesByWorkflowAndAccessScope(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetStatusCodesByWorkflowAndAccessScope returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByWorkflowAndAccessScope_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetStatusCodesByWorkflowAndAccessScopeRequest { WorkflowId = 123, AccessScope = 1 };

        _mockDataService.Setup(ds => ds.GetStatusCodesByWorkflowAndAccessScopeAsync((byte?)request.WorkflowId, (byte?)request.AccessScope, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodesByWorkflowAndAccessScope(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetStatusCodesByWorkflowAndAccessScope propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetStatusCodesByWorkflowAndAccessScope_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetStatusCodesByWorkflowAndAccessScopeRequest { WorkflowId = 123, AccessScope = 1 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetStatusCodesByWorkflowAndAccessScopeAsync((byte?)request.WorkflowId, (byte?)request.AccessScope, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetStatusCodesByWorkflowAndAccessScope(request, null!));

    }

    #endregion

    #region GetStatusCodeScope Tests

    /// <summary>
    /// Tests that GetStatusCodeScope returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetStatusCodeScope_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetStatusCodeScopeRequest { StatusId = 123 };
        var mockResults = new List<core_workflow_sp_GetStatusCodeScopeResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetStatusCodeScopeAsync((byte?)request.StatusId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodeScope(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetStatusCodeScope returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetStatusCodeScope_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetStatusCodeScopeRequest { StatusId = 123 };

        _mockDataService.Setup(ds => ds.GetStatusCodeScopeAsync((byte?)request.StatusId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStatusCodeScope(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetStatusCodeScope propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetStatusCodeScope_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetStatusCodeScopeRequest { StatusId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetStatusCodeScopeAsync((byte?)request.StatusId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetStatusCodeScope(request, null!));

    }

    #endregion

    #region GetStepsByWorkflow Tests

    /// <summary>
    /// Tests that GetStepsByWorkflow returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetStepsByWorkflow_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetStepsByWorkflowRequest { Workflow = 123 };
        var mockResults = new List<core_workflow_sp_GetStepsByWorkflowResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetStepsByWorkflowAsync((byte?)request.Workflow, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStepsByWorkflow(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetStepsByWorkflow returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetStepsByWorkflow_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetStepsByWorkflowRequest { Workflow = 123 };

        _mockDataService.Setup(ds => ds.GetStepsByWorkflowAsync((byte?)request.Workflow, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStepsByWorkflow(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetStepsByWorkflow propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetStepsByWorkflow_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetStepsByWorkflowRequest { Workflow = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetStepsByWorkflowAsync((byte?)request.Workflow, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetStepsByWorkflow(request, null!));

    }

    #endregion

    #region GetStepsByWorkflowAndStatus Tests

    /// <summary>
    /// Tests that GetStepsByWorkflowAndStatus returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetStepsByWorkflowAndStatus_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetStepsByWorkflowAndStatusRequest { Workflow = 123, Status = 1, DeathStatus = "Final" };
        var mockResults = new List<core_workflow_sp_GetStepsByWorkflowAndStatusResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetStepsByWorkflowAndStatusAsync((byte?)request.Workflow, (byte?)request.Status, request.DeathStatus, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStepsByWorkflowAndStatus(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetStepsByWorkflowAndStatus returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetStepsByWorkflowAndStatus_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetStepsByWorkflowAndStatusRequest { Workflow = 123, Status = 1, DeathStatus = "Final" };

        _mockDataService.Setup(ds => ds.GetStepsByWorkflowAndStatusAsync((byte?)request.Workflow, (byte?)request.Status, request.DeathStatus, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetStepsByWorkflowAndStatus(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetStepsByWorkflowAndStatus propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetStepsByWorkflowAndStatus_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetStepsByWorkflowAndStatusRequest { Workflow = 123, Status = 1, DeathStatus = "Final" };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetStepsByWorkflowAndStatusAsync((byte?)request.Workflow, (byte?)request.Status, request.DeathStatus, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetStepsByWorkflowAndStatus(request, null!));

    }

    #endregion

    #region GetWorkflowInitialStatusCode Tests

    /// <summary>
    /// Tests that GetWorkflowInitialStatusCode returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetWorkflowInitialStatusCode_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetWorkflowInitialStatusCodeRequest { Compo = 1, Module = 1, WorkflowId = 123 };
        var mockResults = new List<core_workflow_sp_GetWorkflowInitialStatusCodeResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetWorkflowInitialStatusCodeAsync(request.Compo, request.Module, request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWorkflowInitialStatusCode(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetWorkflowInitialStatusCode returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetWorkflowInitialStatusCode_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetWorkflowInitialStatusCodeRequest { Compo = 1, Module = 1, WorkflowId = 123 };

        _mockDataService.Setup(ds => ds.GetWorkflowInitialStatusCodeAsync(request.Compo, request.Module, request.WorkflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWorkflowInitialStatusCode(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetWorkflowInitialStatusCode propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetWorkflowInitialStatusCode_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetWorkflowInitialStatusCodeRequest { Compo = 1, Module = 1, WorkflowId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetWorkflowInitialStatusCodeAsync(request.Compo, request.Module, request.WorkflowId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetWorkflowInitialStatusCode(request, null!));

    }

    #endregion

    #region GetWorkflowByCompo Tests

    /// <summary>
    /// Tests that GetWorkflowByCompo returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetWorkflowByCompo_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetWorkflowByCompoRequest { Compo = "TestCompo", UserId = 123 };
        var mockResults = new List<core_workflow_sp_GetWorkflowByCompoResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetWorkflowByCompoAsync(request.Compo, request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWorkflowByCompo(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetWorkflowByCompo returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetWorkflowByCompo_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetWorkflowByCompoRequest { Compo = "TestCompo", UserId = 123 };

        _mockDataService.Setup(ds => ds.GetWorkflowByCompoAsync(request.Compo, request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWorkflowByCompo(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetWorkflowByCompo propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetWorkflowByCompo_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetWorkflowByCompoRequest { Compo = "TestCompo", UserId = 123 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetWorkflowByCompoAsync(request.Compo, request.UserId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetWorkflowByCompo(request, null!));

    }

    #endregion

    #region GetWorkflowFromModule Tests

    /// <summary>
    /// Tests that GetWorkflowFromModule returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetWorkflowFromModule_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetWorkflowFromModuleRequest { ModuleId = 456 };
        var mockResults = new List<core_workflow_sp_GetWorkflowFromModuleResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetWorkflowFromModuleAsync(request.ModuleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWorkflowFromModule(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetWorkflowFromModule returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetWorkflowFromModule_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetWorkflowFromModuleRequest { ModuleId = 456 };

        _mockDataService.Setup(ds => ds.GetWorkflowFromModuleAsync(request.ModuleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWorkflowFromModule(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetWorkflowFromModule propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetWorkflowFromModule_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetWorkflowFromModuleRequest { ModuleId = 456 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetWorkflowFromModuleAsync(request.ModuleId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetWorkflowFromModule(request, null!));

    }

    #endregion

    #region GetWorkflowTitle Tests

    /// <summary>
    /// Tests that GetWorkflowTitle returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetWorkflowTitle_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetWorkflowTitleRequest { ModuleId = 456, SubCase = 789 };
        var mockResults = new List<core_workflow_sp_GetWorkflowTitleResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetWorkflowTitleAsync(request.ModuleId, request.SubCase, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWorkflowTitle(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetWorkflowTitle returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetWorkflowTitle_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetWorkflowTitleRequest { ModuleId = 456, SubCase = 789 };

        _mockDataService.Setup(ds => ds.GetWorkflowTitleAsync(request.ModuleId, request.SubCase, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWorkflowTitle(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetWorkflowTitle propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetWorkflowTitle_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetWorkflowTitleRequest { ModuleId = 456, SubCase = 789 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetWorkflowTitleAsync(request.ModuleId, request.SubCase, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetWorkflowTitle(request, null!));

    }

    #endregion

    #region GetWorkflowTitleByWorkStatusId Tests

    /// <summary>
    /// Tests that GetWorkflowTitleByWorkStatusId returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetWorkflowTitleByWorkStatusId_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new GetWorkflowTitleByWorkStatusIdRequest { WorkflowId = 123, SubCase = 789 };
        var mockResults = new List<core_workflow_sp_GetWorkflowTitleByWorkStatusIdResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.GetWorkflowTitleByWorkStatusIdAsync(request.WorkflowId, request.SubCase, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWorkflowTitleByWorkStatusId(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that GetWorkflowTitleByWorkStatusId returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetWorkflowTitleByWorkStatusId_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetWorkflowTitleByWorkStatusIdRequest { WorkflowId = 123, SubCase = 789 };

        _mockDataService.Setup(ds => ds.GetWorkflowTitleByWorkStatusIdAsync(request.WorkflowId, request.SubCase, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetWorkflowTitleByWorkStatusId(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that GetWorkflowTitleByWorkStatusId propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetWorkflowTitleByWorkStatusId_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetWorkflowTitleByWorkStatusIdRequest { WorkflowId = 123, SubCase = 789 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetWorkflowTitleByWorkStatusIdAsync(request.WorkflowId, request.SubCase, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetWorkflowTitleByWorkStatusId(request, null!));

    }

    #endregion

    #region InsertAction Tests

    /// <summary>
    /// Tests that InsertAction returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task InsertAction_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new InsertActionRequest { Type = 1, StepId = 123, Target = 456, Data = 789 };
        var mockResults = new List<core_workflow_sp_InsertActionResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.InsertActionAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.InsertAction(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that InsertAction returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task InsertAction_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new InsertActionRequest { Type = 1, StepId = 123, Target = 456, Data = 789 };

        _mockDataService.Setup(ds => ds.InsertActionAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.InsertAction(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that InsertAction propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task InsertAction_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new InsertActionRequest { Type = 1, StepId = 123, Target = 456, Data = 789 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.InsertActionAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.InsertAction(request, null!));

    }

    #endregion

    #region InsertOptionAction Tests

    /// <summary>
    /// Tests that InsertOptionAction returns correct response when data service returns results.
    /// </summary>
    [Fact]
    public async Task InsertOptionAction_ReturnsCorrectResponse_WhenDataServiceReturnsResults()
    {
        // Arrange
        var request = new InsertOptionActionRequest { Type = 1, Wsoid = 456, Target = 789, Data = 101 };
        var mockResults = new List<core_workflow_sp_InsertOptionActionResult>
        {
            new() { /* mock data */ },
            new() { /* mock data */ }
        };

        _mockDataService.Setup(ds => ds.InsertOptionActionAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.InsertOptionAction(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// Tests that InsertOptionAction returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task InsertOptionAction_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new InsertOptionActionRequest { Type = 1, Wsoid = 456, Target = 789, Data = 101 };

        _mockDataService.Setup(ds => ds.InsertOptionActionAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.InsertOptionAction(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// Tests that InsertOptionAction propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task InsertOptionAction_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new InsertOptionActionRequest { Type = 1, Wsoid = 456, Target = 789, Data = 101 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.InsertOptionActionAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.InsertOptionAction(request, null!));

    }

    #endregion

    #region Application Warmup Process Methods Tests

    #region GetAllLogsPagination Tests

    /// <summary>
    /// Tests that GetAllLogsPagination returns correct response with default pagination when data service returns results.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_ReturnsCorrectResponse_WithDefaultPagination()
    {
        // Arrange
        var request = new GetAllLogsPaginationRequest();
        var mockData = new List<ApplicationWarmupProcess_sp_GetAllLogsResult>
        {
            new() { Id = 1, Name = "Process1", ExecutionDate = DateTime.Now, Message = "Success" },
            new() { Id = 2, Name = "Process2", ExecutionDate = DateTime.Now.AddMinutes(-5), Message = "Completed" }
        };
        var mockResults = new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result
        {
            TotalCount = 2,
            Data = mockData
        };

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLogsPagination(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(2, response.TotalCount);
        Assert.Equal(1, response.Items[0].LogId);
        Assert.Equal("Process1", response.Items[0].ProcessName);
        Assert.Equal("Success", response.Items[0].Message);
        Assert.Equal(2, response.Items[1].LogId);
        Assert.Equal("Process2", response.Items[1].ProcessName);
        Assert.Equal("Completed", response.Items[1].Message);
    }

    /// <summary>
    /// Tests that GetAllLogsPagination returns correct response with custom pagination parameters.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_ReturnsCorrectResponse_WithCustomPagination()
    {
        // Arrange
        var request = new GetAllLogsPaginationRequest
        {
            PageNumber = 2,
            PageSize = 25,
            ProcessName = "TestProcess",
            SortBy = "Name",
            SortOrder = "ASC"
        };

        var mockData = new List<ApplicationWarmupProcess_sp_GetAllLogsResult>
        {
            new() { Id = 26, Name = "TestProcess", ExecutionDate = DateTime.Now, Message = "Result 26" },
            new() { Id = 27, Name = "TestProcess", ExecutionDate = DateTime.Now, Message = "Result 27" }
        };
        var mockResults = new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result
        {
            TotalCount = 2,
            Data = mockData
        };

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLogsPagination(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(2, response.TotalCount);
        Assert.Equal(26, response.Items[0].LogId);
        Assert.Equal("TestProcess", response.Items[0].ProcessName);
    }

    /// <summary>
    /// Tests that GetAllLogsPagination returns correct response with date filtering.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_ReturnsCorrectResponse_WithDateFiltering()
    {
        // Arrange
        var startDate = "2024-01-01";
        var endDate = "2024-12-31";
        var request = new GetAllLogsPaginationRequest
        {
            StartDate = startDate,
            EndDate = endDate,
            PageNumber = 1,
            PageSize = 10
        };

        var mockData = new List<ApplicationWarmupProcess_sp_GetAllLogsResult>
        {
            new() { Id = 1, Name = "Process1", ExecutionDate = new DateTime(2024, 6, 15), Message = "Mid-year" }
        };
        var mockResults = new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result
        {
            TotalCount = 1,
            Data = mockData
        };

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(),
            It.IsAny<DateTime?>(), It.IsAny<DateTime?>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLogsPagination(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.Items);
        Assert.Equal(1, response.TotalCount);
        Assert.Equal("Mid-year", response.Items[0].Message);
    }

    /// <summary>
    /// Tests that GetAllLogsPagination returns correct response with message filtering.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_ReturnsCorrectResponse_WithMessageFiltering()
    {
        // Arrange
        var request = new GetAllLogsPaginationRequest
        {
            MessageFilter = "error",
            PageNumber = 1,
            PageSize = 10
        };

        var mockData = new List<ApplicationWarmupProcess_sp_GetAllLogsResult>
        {
            new() { Id = 1, Name = "Process1", ExecutionDate = DateTime.Now, Message = "An error occurred" },
            new() { Id = 2, Name = "Process2", ExecutionDate = DateTime.Now, Message = "Another error" }
        };
        var mockResults = new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result
        {
            TotalCount = 2,
            Data = mockData
        };

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLogsPagination(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(2, response.TotalCount);
        Assert.Contains("error", response.Items[0].Message.ToLower());
        Assert.Contains("error", response.Items[1].Message.ToLower());
    }

    /// <summary>
    /// Tests that GetAllLogsPagination returns correct response with all filters combined.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_ReturnsCorrectResponse_WithAllFiltersCombined()
    {
        // Arrange
        var request = new GetAllLogsPaginationRequest
        {
            PageNumber = 1,
            PageSize = 50,
            ProcessName = "CriticalProcess",
            StartDate = "2024-10-01",
            EndDate = "2024-10-12",
            MessageFilter = "completed",
            SortBy = "ExecutionDate",
            SortOrder = "ASC"
        };

        var mockData = new List<ApplicationWarmupProcess_sp_GetAllLogsResult>
        {
            new() { Id = 100, Name = "CriticalProcess", ExecutionDate = new DateTime(2024, 10, 5), Message = "Task completed successfully" },
            new() { Id = 101, Name = "CriticalProcess", ExecutionDate = new DateTime(2024, 10, 10), Message = "Process completed" }
        };
        var mockResults = new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result
        {
            TotalCount = 2,
            Data = mockData
        };

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLogsPagination(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(2, response.TotalCount);
        Assert.All(response.Items, item => Assert.Equal("CriticalProcess", item.ProcessName));
        Assert.All(response.Items, item => Assert.Contains("completed", item.Message.ToLower()));
    }

    /// <summary>
    /// Tests that GetAllLogsPagination returns empty response when data service returns null.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_ReturnsEmptyResponse_WhenDataServiceReturnsNull()
    {
        // Arrange
        var request = new GetAllLogsPaginationRequest { PageNumber = 1, PageSize = 10 };

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result)null!);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLogsPagination(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
        Assert.Equal(0, response.TotalCount);
    }

    /// <summary>
    /// Tests that GetAllLogsPagination returns empty response when data service returns empty list.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_ReturnsEmptyResponse_WhenDataServiceReturnsEmptyList()
    {
        // Arrange
        var request = new GetAllLogsPaginationRequest { PageNumber = 1, PageSize = 10 };

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result { TotalCount = 0, Data = [] });

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLogsPagination(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Items);
        Assert.Equal(0, response.TotalCount);
    }

    /// <summary>
    /// Tests that GetAllLogsPagination handles null message in results correctly.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_HandlesNullMessage_Correctly()
    {
        // Arrange
        var request = new GetAllLogsPaginationRequest { PageNumber = 1, PageSize = 10 };
        var mockData = new List<ApplicationWarmupProcess_sp_GetAllLogsResult>
        {
            new() { Id = 1, Name = "Process1", ExecutionDate = DateTime.Now, Message = null }
        };
        var mockResults = new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result
        {
            TotalCount = 1,
            Data = mockData
        };

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLogsPagination(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.Items);
        Assert.Equal(string.Empty, response.Items[0].Message);
    }

    /// <summary>
    /// Tests that GetAllLogsPagination propagates exceptions from data service.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_PropagatesExceptionsFromDataService()
    {
        // Arrange
        var request = new GetAllLogsPaginationRequest { PageNumber = 1, PageSize = 10 };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var service = CreateWorkflowManagementService();

        // Act & Assert
        await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetAllLogsPagination(request, null!));

    }

    /// <summary>
    /// Tests that GetAllLogsPagination formats execution date correctly.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_FormatsExecutionDate_Correctly()
    {
        // Arrange
        var executionDate = new DateTime(2024, 10, 12, 14, 30, 45);
        var request = new GetAllLogsPaginationRequest { PageNumber = 1, PageSize = 10 };
        var mockData = new List<ApplicationWarmupProcess_sp_GetAllLogsResult>
        {
            new() { Id = 1, Name = "Process1", ExecutionDate = executionDate, Message = "Test" }
        };
        var mockResults = new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result
        {
            TotalCount = 1,
            Data = mockData
        };

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLogsPagination(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.Items);
        Assert.Equal("2024-10-12 14:30:45", response.Items[0].ExecutionDate);
    }

    /// <summary>
    /// Tests that GetAllLogsPagination handles large result sets correctly.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_HandlesLargeResultSets_Correctly()
    {
        // Arrange
        var request = new GetAllLogsPaginationRequest { PageNumber = 1, PageSize = 100 };
        var mockData = new List<ApplicationWarmupProcess_sp_GetAllLogsResult>();
        
        for (var i = 1; i <= 100; i++)
        {
            mockData.Add(new ApplicationWarmupProcess_sp_GetAllLogsResult
            {
                Id = i,
                Name = $"Process{i}",
                ExecutionDate = DateTime.Now.AddMinutes(-i),
                Message = $"Message {i}"
            });
        }
        var mockResults = new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result
        {
            TotalCount = 100,
            Data = mockData
        };

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults);

        var service = CreateWorkflowManagementService();

        // Act
        var response = await service.GetAllLogsPagination(request, null!);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(100, response.Items.Count);
        Assert.Equal(100, response.TotalCount);
        Assert.All(response.Items, item => Assert.NotNull(item.ProcessName));
    }

    /// <summary>
    /// Tests that GetAllLogsPagination verifies correct parameters are passed to data service.
    /// </summary>
    [Fact]
    public async Task GetAllLogsPagination_PassesCorrectParametersToDataService()
    {
        // Arrange
        var request = new GetAllLogsPaginationRequest
        {
            PageNumber = 3,
            PageSize = 20,
            ProcessName = "SpecificProcess",
            StartDate = "2024-01-01",
            EndDate = "2024-12-31",
            MessageFilter = "warning",
            SortBy = "Name",
            SortOrder = "DESC"
        };

        var mockResults = new ApplicationWarmupProcess_sp_GetAllLogs_pagination_Result
        {
            TotalCount = 0,
            Data = new List<ApplicationWarmupProcess_sp_GetAllLogsResult>()
        };

        _mockDataService.Setup(ds => ds.GetAllLogsPaginationAsync(
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResults)
            .Verifiable();

        var service = CreateWorkflowManagementService();

        // Act
        await service.GetAllLogsPagination(request, null!);

        // Assert
        _mockDataService.Verify();
    }

    #endregion

    #endregion
}

/// <summary>
/// A test implementation of IResilienceService that executes operations without retry logic.
/// Used for unit testing to bypass resilience patterns.
/// </summary>
internal class TestResilienceService : IResilienceService
{
    /// <summary>
    /// Executes the HTTP request without any resilience policies.
    /// </summary>
    /// <param name="action">The HTTP request action to execute</param>
    /// <returns>The HTTP response message</returns>
    public async Task<HttpResponseMessage> ExecuteResilientHttpRequestAsync(Func<Task<HttpResponseMessage>> action)
    {
        return await action();
    }

    /// <summary>
    /// Executes the operation without any retry logic.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="action">The operation to execute.</param>
    /// <returns>The result of the operation.</returns>
    public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action)
    {
        return await action();
    }

    /// <summary>
    /// Executes the database operation without any resilience policies.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="action">The database operation to execute.</param>
    /// <returns>The result of the database operation.</returns>
    public async Task<T> ExecuteDatabaseOperationAsync<T>(Func<Task<T>> action)
    {
        return await action();
    }

    /// <summary>
    /// Gets the current circuit breaker state (always closed for tests).
    /// </summary>
    public Polly.CircuitBreaker.CircuitState CircuitBreakerState => Polly.CircuitBreaker.CircuitState.Closed;

    /// <summary>
    /// Gets the last exception (always null for tests).
    /// </summary>
    public Exception? LastException => null;

    /// <summary>
    /// Manually resets the circuit breaker (no-op for tests).
    /// </summary>
    public void ResetCircuitBreaker()
    {
        // No-op for tests
    }
}
#endregion











