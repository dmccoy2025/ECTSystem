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
public partial class DataService : IDataService
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
}
