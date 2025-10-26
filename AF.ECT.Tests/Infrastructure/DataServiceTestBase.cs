using System.Linq.Expressions;
using AF.ECT.Data.Models;
using AF.ECT.Data.Interfaces;
using AF.ECT.Data.ResultTypes;
using AF.ECT.Data.Services;

namespace AF.ECT.Tests.Infrastructure;

/// <summary>
/// Base class for DataService tests providing common mock setup and helper methods.
/// </summary>
public abstract class DataServiceTestBase
{
    protected readonly Mock<IDbContextFactory<ALODContext>> MockContextFactory;
    protected readonly Mock<ALODContext> MockContext;
    protected readonly Mock<IALODContextProcedures> MockProcedures;
    protected readonly Mock<ILogger<DataService>> MockLogger;
    protected readonly DbContextOptions<ALODContext> ContextOptions;

    protected DataServiceTestBase()
    {
        MockContextFactory = new Mock<IDbContextFactory<ALODContext>>();
        ContextOptions = new DbContextOptions<ALODContext>();
        MockContext = new Mock<ALODContext>(ContextOptions);
        MockProcedures = new Mock<IALODContextProcedures>();
        MockLogger = new Mock<ILogger<DataService>>();

        // Default setup
        MockContext.Setup(c => c.Procedures).Returns(MockProcedures.Object);
        MockContextFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(MockContext.Object);
    }

    /// <summary>
    /// Creates a DataService instance with the configured mocks.
    /// </summary>
    protected DataService CreateDataService() => new(MockContextFactory.Object, MockLogger.Object);

    /// <summary>
    /// Sets up the mock procedures to return the specified results for GetReinvestigationRequestsAsync.
    /// </summary>
    /// <param name="results">The results to return from the stored procedure.</param>
    protected void SetupProceduresToReturn(List<core_lod_sp_GetReinvestigationRequestsResult> results)
    {
        MockProcedures.Setup(p => p.GetReinvestigationRequestsAsync(It.IsAny<int?>(), It.IsAny<bool?>(), null, It.IsAny<CancellationToken?>()))
            .Returns(Task.FromResult(results));
    }

    /// <summary>
    /// Generic method to set up any stored procedure to return specified results.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the stored procedure.</typeparam>
    /// <param name="setupExpression">Expression defining which stored procedure method to mock.</param>
    /// <param name="results">The results to return from the stored procedure.</param>
    protected void SetupStoredProcedureToReturn<TResult>(Expression<Func<IALODContextProcedures, Task<TResult>>> setupExpression, TResult results)
    {
        MockProcedures.Setup(setupExpression).Returns(Task.FromResult(results));
    }

    /// <summary>
    /// Generic method to set up any stored procedure to return a list of results.
    /// </summary>
    /// <typeparam name="TResult">The type of items in the result list.</typeparam>
    /// <param name="setupExpression">Expression defining which stored procedure method to mock.</param>
    /// <param name="results">The list of results to return from the stored procedure.</param>
    protected void SetupStoredProcedureToReturn<TResult>(Expression<Func<IALODContextProcedures, Task<List<TResult>>>> setupExpression, List<TResult> results)
    {
        MockProcedures.Setup(setupExpression).Returns(Task.FromResult(results));
    }

    /// <summary>
    /// Generic method to set up any stored procedure to throw an exception.
    /// </summary>
    /// <typeparam name="TResult">The type of result the stored procedure would return.</typeparam>
    /// <param name="setupExpression">Expression defining which stored procedure method to mock.</param>
    /// <param name="exception">The exception to throw when the stored procedure is called.</param>
    protected void SetupStoredProcedureToThrow<TResult>(Expression<Func<IALODContextProcedures, Task<TResult>>> setupExpression, Exception exception)
    {
        MockProcedures.Setup(setupExpression).ThrowsAsync(exception);
    }

    /// <summary>
    /// Sets up the mock procedures to throw the specified exception for GetReinvestigationRequestsAsync.
    /// </summary>
    /// <param name="exception">The exception to throw when the stored procedure is called.</param>
    protected void SetupProceduresToThrow(Exception exception)
    {
        MockProcedures.Setup(p => p.GetReinvestigationRequestsAsync(It.IsAny<int?>(), It.IsAny<bool?>(), null, It.IsAny<CancellationToken?>()))
            .ThrowsAsync(exception);
    }

    /// <summary>
    /// Sets up the mock context factory to throw the specified exception for CreateDbContextAsync.
    /// </summary>
    /// <param name="exception">The exception to throw when the context factory is called.</param>
    protected void SetupContextFactoryToThrow(Exception exception)
    {
        MockContextFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);
    }

    /// <summary>
    /// Generic method to set up the context factory to throw an exception for any method.
    /// </summary>
    /// <typeparam name="TResult">The type of result the context factory method would return.</typeparam>
    /// <param name="setupExpression">Expression defining which context factory method to mock.</param>
    /// <param name="exception">The exception to throw when the context factory method is called.</param>
    protected void SetupContextFactoryToThrow<TResult>(Expression<Func<IDbContextFactory<ALODContext>, Task<TResult>>> setupExpression, Exception exception)
    {
        MockContextFactory.Setup(setupExpression).ThrowsAsync(exception);
    }
}
