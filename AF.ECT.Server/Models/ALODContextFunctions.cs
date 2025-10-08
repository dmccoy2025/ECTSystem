
using AF.ECT.Server.Models;
using AF.ECT.Server.Models.Interfaces;

namespace AF.ECT.Server.Models;

public class ALODContextFunctions : IALODContextFunctions
{
    private readonly ALODContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ALODContextFunctions"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
    public ALODContextFunctions(ALODContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
}