using SharedModels = AF.ECT.Shared;
using static AF.ECT.Tests.Data.DataServiceTestData;
using AF.ECT.Tests.Infrastructure;
using AF.ECT.Data.Extensions;
using AF.ECT.Data.ResultTypes;
using System.Reflection;
using AF.ECT.Data.Services;

namespace AF.ECT.Tests.Unit;

/// <summary>
/// Contains unit tests for the <see cref="DataService"/> class.
/// Tests cover constructor validation, database operations, error handling, parameter combinations, and resource management.
///
/// <para>Test Scenarios Outline:</para>
/// <list type="bullet">
/// <item><description>Constructor validation: Ensures proper exception handling for invalid constructor arguments.</description></item>
/// <item><description>Database operations: Verifies correct stored procedure calls with various parameter combinations.</description></item>
/// <item><description>Parameter handling: Tests behavior with null and valid parameters.</description></item>
/// <item><description>Cancellation support: Validates proper handling of cancellation tokens in async operations.</description></item>
/// <item><description>Error propagation: Tests exception handling from context factory and stored procedure failures.</description></item>
/// <item><description>Resource management: Ensures proper disposal of database contexts after use.</description></item>
/// <item><description>Exception types: Covers different exception scenarios from both context factory and procedures.</description></item>
/// </list>
/// </summary>
[Collection("DataService Tests")]
[Trait("Category", "Unit")]
[Trait("Component", "DataService")]
public class DataServiceTests : DataServiceTestBase
{
    /// <summary>
    /// Tests that the DataService constructor throws <see cref="ArgumentNullException"/>
    /// when a null context factory is provided.
    /// </summary>
    [Fact]
    [Trait("Type", "Constructor")]
    [Trait("Scenario", "Validation")]
    public void Constructor_ThrowsArgumentNullException_WhenContextFactoryIsNull()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<DataService>>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DataService(null!, mockLogger.Object));
    }

    /// <summary>
    /// Tests that GetReinvestigationRequestsAsync correctly calls the stored procedure
    /// with the provided parameters and returns the expected results.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "HappyPath")]
    public async Task GetReinvestigationRequestsAsync_CallsProceduresMethod_WithCorrectParameters()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResults = new List<core_lod_sp_GetReinvestigationRequestsResult>
        {
            new()
        };
        SetupStoredProcedureToReturn(p => p.GetReinvestigationRequestsAsync(It.IsAny<int?>(), It.IsAny<bool?>(), null, It.IsAny<CancellationToken?>()), expectedResults);

        // Act
        var result = await dataService.GetReinvestigationRequestsAsync(1, true);

        // Assert
        Assert.Equal(expectedResults, result);
        MockProcedures.Verify(p => p.GetReinvestigationRequestsAsync(1, true, null, It.IsAny<CancellationToken?>()), Times.Once);
    }

    /// <summary>
    /// Tests that GetReinvestigationRequestsAsync correctly handles null parameters
    /// and passes them to the stored procedure.
    /// </summary>
    [Fact]
    public async Task GetReinvestigationRequestsAsync_CallsProceduresMethod_WithNullParameters()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResults = new List<core_lod_sp_GetReinvestigationRequestsResult>();
        SetupStoredProcedureToReturn(p => p.GetReinvestigationRequestsAsync(It.IsAny<int?>(), It.IsAny<bool?>(), null, It.IsAny<CancellationToken?>()), expectedResults);

        // Act
        var result = await dataService.GetReinvestigationRequestsAsync(null, null);

        // Assert
        Assert.Equal(expectedResults, result);
        MockProcedures.Verify(p => p.GetReinvestigationRequestsAsync(null, null, null, It.IsAny<CancellationToken?>()), Times.Once);
    }

    /// <summary>
    /// Tests that GetReinvestigationRequestsAsync properly handles and passes through
    /// cancellation tokens to the underlying database operations.
    /// </summary>
    [Fact]
    public async Task GetReinvestigationRequestsAsync_HandlesCancellationToken()
    {
        // Arrange
        var dataService = CreateDataService();
        var cancellationToken = new CancellationToken(true);
        var expectedResults = new List<core_lod_sp_GetReinvestigationRequestsResult>();
        SetupStoredProcedureToReturn(p => p.GetReinvestigationRequestsAsync(It.IsAny<int?>(), It.IsAny<bool?>(), null, It.IsAny<CancellationToken?>()), expectedResults);

        // Act
        var result = await dataService.GetReinvestigationRequestsAsync(1, false, cancellationToken);

        // Assert
        Assert.Equal(expectedResults, result);
        MockProcedures.Verify(p => p.GetReinvestigationRequestsAsync(1, false, null, cancellationToken), Times.Once);
    }

    /// <summary>
    /// Tests that GetReinvestigationRequestsAsync properly propagates exceptions
    /// thrown by the context factory.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "ErrorHandling")]
    public async Task GetReinvestigationRequestsAsync_ThrowsException_WhenContextFactoryFails()
    {
        // Arrange
        var dataService = CreateDataService();
        var exception = new InvalidOperationException("Database connection failed");
        SetupContextFactoryToThrow(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()), exception);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            dataService.GetReinvestigationRequestsAsync(1, true));
    }

    /// <summary>
    /// Tests that GetReinvestigationRequestsAsync properly propagates exceptions
    /// thrown by the stored procedure execution.
    /// </summary>
    [Fact]
    public async Task GetReinvestigationRequestsAsync_ThrowsException_WhenProceduresMethodFails()
    {
        // Arrange
        var dataService = CreateDataService();
        var exception = new Exception("Stored procedure failed");
        SetupStoredProcedureToThrow(p => p.GetReinvestigationRequestsAsync(It.IsAny<int?>(), It.IsAny<bool?>(), null, It.IsAny<CancellationToken?>()), exception);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            dataService.GetReinvestigationRequestsAsync(1, true));
    }

    /// <summary>
    /// Tests that GetReinvestigationRequestsAsync properly handles already canceled tokens
    /// and throws <see cref="OperationCanceledException"/> when appropriate.
    /// </summary>
    [Fact]
    public async Task GetReinvestigationRequestsAsync_ThrowsOperationCanceledException_WhenTokenIsAlreadyCanceled()
    {
        // Arrange
        var dataService = CreateDataService();
        var canceledToken = new CancellationToken(true);
        var exception = new OperationCanceledException();
        MockContextFactory.Setup(f => f.CreateDbContextAsync(canceledToken))
            .ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            dataService.GetReinvestigationRequestsAsync(1, true, canceledToken));
    }

    /// <summary>
    /// Tests that GetReinvestigationRequestsAsync correctly handles various combinations
    /// of userId and sarc parameters, including null values.
    /// </summary>
    /// <param name="userId">The user ID parameter to test.</param>
    /// <param name="sarc">The SARC flag parameter to test.</param>
    [Theory]
    [ClassData(typeof(DataServiceParameterCombinationData))]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "ParameterCombinations")]
    public async Task GetReinvestigationRequestsAsync_CallsProceduresMethod_WithVariousParameterCombinations(int? userId, bool? sarc)
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResults = new List<core_lod_sp_GetReinvestigationRequestsResult>
        {
            new()
        };
        SetupStoredProcedureToReturn(p => p.GetReinvestigationRequestsAsync(It.IsAny<int?>(), It.IsAny<bool?>(), null, It.IsAny<CancellationToken?>()), expectedResults);

        // Act
        var result = await dataService.GetReinvestigationRequestsAsync(userId, sarc);

        // Assert
        Assert.Equal(expectedResults, result);
        MockProcedures.Verify(p => p.GetReinvestigationRequestsAsync(userId, sarc, null, It.IsAny<CancellationToken?>()), Times.Once);
    }

    /// <summary>
    /// Tests that GetReinvestigationRequestsAsync properly disposes of the database context
    /// after use, ensuring proper resource management.
    /// </summary>
    [Fact]
    public async Task GetReinvestigationRequestsAsync_DisposesContext_AfterUse()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResults = new List<core_lod_sp_GetReinvestigationRequestsResult>();
        SetupStoredProcedureToReturn(p => p.GetReinvestigationRequestsAsync(It.IsAny<int?>(), It.IsAny<bool?>(), null, It.IsAny<CancellationToken?>()), expectedResults);

        // Act
        await dataService.GetReinvestigationRequestsAsync(1, true);

        // Assert
        MockContext.Verify(c => c.Dispose(), Times.Once);
    }

    /// <summary>
    /// Tests that GetReinvestigationRequestsAsync properly propagates different types of exceptions
    /// thrown by the context factory.
    /// </summary>
    /// <param name="exceptionType">The type of exception to test.</param>
    /// <param name="message">The exception message.</param>
    [Theory]
    [ClassData(typeof(DataServiceExceptionTypeData))]
    public async Task GetReinvestigationRequestsAsync_ThrowsDifferentExceptionTypes_FromContextFactory(Type exceptionType, string message)
    {
        // Arrange
        var dataService = CreateDataService();
        Exception exception;
        try
        {
            exception = (Exception)Activator.CreateInstance(exceptionType, message)!;
        }
        catch
        {
            // Fallback for exceptions that don't have a simple message constructor
            try
            {
                exception = (Exception)Activator.CreateInstance(exceptionType)!;
            }
            catch
            {
                // Final fallback
                exception = new Exception(message);
            }
        }
        SetupContextFactoryToThrow(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()), exception);

        // Act & Assert
        await Assert.ThrowsAsync(exceptionType, () =>
            dataService.GetReinvestigationRequestsAsync(1, true));
    }

    /// <summary>
    /// Tests that GetReinvestigationRequestsAsync properly propagates different types of exceptions
    /// thrown by the stored procedure execution.
    /// </summary>
    /// <param name="exceptionType">The type of exception to test.</param>
    /// <param name="message">The exception message.</param>
    [Theory]
    [ClassData(typeof(DataServiceExceptionTypeData))]
    public async Task GetReinvestigationRequestsAsync_ThrowsDifferentExceptionTypes_FromProcedures(Type exceptionType, string message)
    {
        // Arrange
        var dataService = CreateDataService();
        Exception exception;
        try
        {
            exception = (Exception)Activator.CreateInstance(exceptionType, message)!;
        }
        catch
        {
            // Fallback for exceptions that don't have a simple message constructor
            exception = new Exception(message);
        }
        SetupStoredProcedureToThrow(p => p.GetReinvestigationRequestsAsync(It.IsAny<int?>(), It.IsAny<bool?>(), null, It.IsAny<CancellationToken?>()), exception);

        // Act & Assert
        await Assert.ThrowsAsync(exceptionType, () =>
            dataService.GetReinvestigationRequestsAsync(1, true));
    }

    #region Core User Method Tests

    /// <summary>
    /// Tests that GetMailingListForLODAsync correctly calls the stored procedure
    /// with the provided parameters and returns the expected results.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "HappyPath")]
    [Trait("Category", "CoreUser")]
    public async Task GetMailingListForLODAsync_CallsProceduresMethod_WithCorrectParameters()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResults = new List<core_user_sp_GetMailingListForLODResult>(); // Simplified for testing
        SetupStoredProcedureToReturn(p => p.core_user_sp_GetMailingListForLODAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>(), null, It.IsAny<CancellationToken?>()), expectedResults);

        // Act
        var result = await dataService.GetMailingListForLODAsync(new SharedModels.GetMailingListForLODRequest { RefId = 1, GroupId = 2, Status = 3, CallingService = "test" });

        // Assert
        Assert.NotNull(result);
        MockProcedures.Verify(p => p.core_user_sp_GetMailingListForLODAsync(1, 2, 3, "test", null, It.IsAny<CancellationToken?>()), Times.Once);
    }

    /// <summary>
    /// Tests that GetMembersUserIdAsync correctly calls the stored procedure
    /// and returns the expected integer result.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "HappyPath")]
    [Trait("Category", "CoreUser")]
    public async Task GetMembersUserIdAsync_CallsProceduresMethod_WithCorrectParameters()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResult = 42;
        SetupStoredProcedureToReturn(p => p.core_user_sp_GetMembersUserIdAsync(It.IsAny<string?>(), null, It.IsAny<CancellationToken?>()), expectedResult);

        // Act
        var result = await dataService.GetMembersUserIdAsync("test-ssn");

        // Assert
        Assert.Equal(expectedResult, result);
        MockProcedures.Verify(p => p.core_user_sp_GetMembersUserIdAsync("test-ssn", null, It.IsAny<CancellationToken?>()), Times.Once);
    }

    /// <summary>
    /// Tests that LogoutAsync correctly calls the stored procedure
    /// and returns the expected integer result.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "HappyPath")]
    [Trait("Category", "CoreUser")]
    public async Task LogoutAsync_CallsProceduresMethod_WithCorrectParameters()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResult = 1;
        SetupStoredProcedureToReturn(p => p.core_user_sp_LogoutAsync(It.IsAny<int?>(), null, It.IsAny<CancellationToken?>()), expectedResult);

        // Act
        var result = await dataService.LogoutAsync(123);

        // Assert
        Assert.Equal(expectedResult, result);
        MockProcedures.Verify(p => p.core_user_sp_LogoutAsync(123, null, It.IsAny<CancellationToken?>()), Times.Once);
    }

    /// <summary>
    /// Tests that RegisterUserRoleAsync correctly calls the stored procedure
    /// with output parameter handling and returns the expected result.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "OutputParameter")]
    [Trait("Category", "CoreUser")]
    public async Task RegisterUserRoleAsync_CallsProceduresMethod_WithOutputParameter()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResult = 1;
        MockProcedures.Setup(p => p.core_user_sp_RegisterUserRoleAsync(It.IsAny<int?>(), It.IsAny<short?>(), It.IsAny<byte?>(), It.IsAny<OutputParameter<int?>>(), It.IsAny<OutputParameter<int>?>(), It.IsAny<CancellationToken?>()))
            .Callback<int?, short?, byte?, OutputParameter<int?>?, OutputParameter<int>?, CancellationToken?>((userID, groupID, status, userRoleID, returnValue, _) =>
            {
                if (userRoleID != null)
                {
                    // Use reflection to set the value since SetValue is internal
                    var setValueMethod = typeof(OutputParameter<int?>).GetMethod("SetValue", BindingFlags.NonPublic | BindingFlags.Instance);
                    setValueMethod?.Invoke(userRoleID, [expectedResult]);
                }
            })
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await dataService.RegisterUserRoleAsync(1, 2, 3);

        // Assert
        Assert.Equal(expectedResult, result);
        MockProcedures.Verify(p => p.core_user_sp_RegisterUserRoleAsync(1, (short)2, (byte)3, It.IsAny<OutputParameter<int?>>(), It.IsAny<OutputParameter<int>?>(), It.IsAny<CancellationToken?>()), Times.Once);
    }

    #endregion

    #region Core Workflow Method Tests

    /// <summary>
    /// Tests that AddSignatureAsync correctly calls the stored procedure
    /// with the provided parameters and returns the expected results.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "HappyPath")]
    [Trait("Category", "CoreWorkflow")]
    public async Task AddSignatureAsync_CallsProceduresMethod_WithCorrectParameters()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResults = new List<core_workflow_sp_AddSignatureResult>
        {
            new()
        };
        SetupStoredProcedureToReturn(
            p => p.core_workflow_sp_AddSignatureAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<byte?>(), It.IsAny<byte?>(), It.IsAny<byte?>(), null, It.IsAny<CancellationToken?>()), 
            expectedResults);

        // Act
        var result = await dataService.AddSignatureAsync(new SharedModels.AddSignatureRequest { RefId = 1, ModuleType = 2, UserId = 3, ActionId = 4, GroupId = 5, StatusIn = 6, StatusOut = 7 });

        // Assert
        Assert.Equal(expectedResults, result);
        MockProcedures.Verify(p => p.core_workflow_sp_AddSignatureAsync(1, 2, 3, 4, (byte)5, (byte)6, (byte)7, null, It.IsAny<CancellationToken?>()), Times.Once);
    }

    /// <summary>
    /// Tests that CopyActionsAsync correctly calls the stored procedure
    /// and returns the expected integer result.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "HappyPath")]
    [Trait("Category", "CoreWorkflow")]
    public async Task CopyActionsAsync_CallsProceduresMethod_WithCorrectParameters()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResult = 1;
        SetupStoredProcedureToReturn(p => p.core_workflow_sp_CopyActionsAsync(It.IsAny<int?>(), It.IsAny<int?>(), null, It.IsAny<CancellationToken?>()), expectedResult);

        // Act
        var result = await dataService.CopyActionsAsync(100, 200);

        // Assert
        Assert.Equal(expectedResult, result);
        MockProcedures.Verify(p => p.core_workflow_sp_CopyActionsAsync(100, 200, null, It.IsAny<CancellationToken?>()), Times.Once);
    }

    /// <summary>
    /// Tests that GetActionsByStepAsync correctly calls the stored procedure
    /// with the provided parameters and returns the expected results.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "HappyPath")]
    [Trait("Category", "CoreWorkflow")]
    public async Task GetActionsByStepAsync_CallsProceduresMethod_WithCorrectParameters()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResults = new List<core_workflow_sp_GetActionsByStepResult>
        {
            new()
        };
        SetupStoredProcedureToReturn(
            p => p.core_workflow_sp_GetActionsByStepAsync(It.IsAny<int?>(), null, It.IsAny<CancellationToken?>()), 
            expectedResults);

        // Act
        var result = await dataService.GetActionsByStepAsync(123);

        // Assert
        Assert.Equal(expectedResults, result);
        MockProcedures.Verify(p => p.core_workflow_sp_GetActionsByStepAsync(123, null, It.IsAny<CancellationToken?>()), Times.Once);
    }

    /// <summary>
    /// Tests that GetPermissionsAsync correctly calls the stored procedure
    /// with the provided parameters and returns the expected results.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "HappyPath")]
    [Trait("Category", "CoreWorkflow")]
    public async Task GetPermissionsAsync_CallsProceduresMethod_WithCorrectParameters()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResults = new List<core_Workflow_sp_GetPermissionsResult>
        {
            new()
        };
        SetupStoredProcedureToReturn(
            p => p.core_Workflow_sp_GetPermissionsAsync(It.IsAny<byte?>(), null, It.IsAny<CancellationToken?>()), 
            expectedResults);

        // Act
        var result = await dataService.GetPermissionsAsync(5);

        // Assert
        Assert.Equal(expectedResults, result);
        MockProcedures.Verify(p => p.core_Workflow_sp_GetPermissionsAsync((byte)5, null, It.IsAny<CancellationToken?>()), Times.Once);
    }

    /// <summary>
    /// Tests that GetStatusCodesByWorkflowAsync correctly calls the stored procedure
    /// with the provided parameters and returns the expected results.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "HappyPath")]
    [Trait("Category", "CoreWorkflow")]
    public async Task GetStatusCodesByWorkflowAsync_CallsProceduresMethod_WithCorrectParameters()
    {
        // Arrange
        var dataService = CreateDataService();
        var expectedResults = new List<core_workflow_sp_GetStatusCodesByWorkflowResult>
        {
            new()
        };
        SetupStoredProcedureToReturn(
            p => p.core_workflow_sp_GetStatusCodesByWorkflowAsync(It.IsAny<byte?>(), null, It.IsAny<CancellationToken?>()), 
            expectedResults);

        // Act
        var result = await dataService.GetStatusCodesByWorkflowAsync(10);

        // Assert
        Assert.Equal(expectedResults, result);
        MockProcedures.Verify(p => p.core_workflow_sp_GetStatusCodesByWorkflowAsync((byte)10, null, It.IsAny<CancellationToken?>()), Times.Once);
    }

    #endregion

    #region Error Handling Tests for New Methods

    /// <summary>
    /// Tests that GetMailingListForLODAsync properly propagates exceptions
    /// thrown by the context factory.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "ErrorHandling")]
    [Trait("Category", "CoreUser")]
    public async Task GetMailingListForLODAsync_ThrowsException_WhenContextFactoryFails()
    {
        // Arrange
        var dataService = CreateDataService();
        var exception = new InvalidOperationException("Database connection failed");
        SetupContextFactoryToThrow(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()), exception);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            dataService.GetMailingListForLODAsync(new SharedModels.GetMailingListForLODRequest { RefId = 1, GroupId = 2, Status = 3, CallingService = "test" }));
    }

    /// <summary>
    /// Tests that AddSignatureAsync properly propagates exceptions
    /// thrown by the stored procedure execution.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "ErrorHandling")]
    [Trait("Category", "CoreWorkflow")]
    public async Task AddSignatureAsync_ThrowsException_WhenProceduresMethodFails()
    {
        // Arrange
        var dataService = CreateDataService();
        var exception = new Exception("Stored procedure failed");
        SetupStoredProcedureToThrow(
            p => p.core_workflow_sp_AddSignatureAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<byte?>(), It.IsAny<byte?>(), It.IsAny<byte?>(), null, It.IsAny<CancellationToken?>()), 
            exception);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            dataService.AddSignatureAsync(new SharedModels.AddSignatureRequest { RefId = 1, ModuleType = 2, UserId = 3, ActionId = 4, GroupId = 5, StatusIn = 6, StatusOut = 7 }));
    }

    /// <summary>
    /// Tests that GetMembersUserIdAsync properly handles cancellation tokens.
    /// </summary>
    [Fact]
    [Trait("Type", "Integration")]
    [Trait("Scenario", "Cancellation")]
    [Trait("Category", "CoreUser")]
    public async Task GetMembersUserIdAsync_HandlesCancellationToken()
    {
        // Arrange
        var dataService = CreateDataService();
        var cancellationToken = new CancellationToken(true);
        var expectedResult = 42;
        SetupStoredProcedureToReturn(p => p.core_user_sp_GetMembersUserIdAsync(It.IsAny<string?>(), null, It.IsAny<CancellationToken?>()), expectedResult);

        // Act
        var result = await dataService.GetMembersUserIdAsync("test-ssn", cancellationToken);

        // Assert
        Assert.Equal(expectedResult, result);
        MockProcedures.Verify(p => p.core_user_sp_GetMembersUserIdAsync("test-ssn", null, cancellationToken), Times.Once);
    }

    #endregion
}
