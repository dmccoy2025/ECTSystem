using AF.ECT.Data.Interfaces;

namespace AF.ECT.Data.Models;

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
