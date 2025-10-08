
using AF.ECT.Server.Models.Interfaces;

namespace AF.ECT.Server.Models;

public partial class ALODContext
{
    private IALODContextFunctions? _functions;

    /// <summary>
    /// Gets or sets the functions interface for executing stored functions.
    /// </summary>
    /// <value>The functions interface instance.</value>
    public virtual IALODContextFunctions Functions
    {
        get
        {
            _functions ??= new ALODContextFunctions(this);

            return _functions;
        }
        set
        {
            _functions = value;
        }
    }
}