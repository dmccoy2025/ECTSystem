# DataService Partial Classes

The `DataService` class has been refactored into partial classes organized by functional regions to improve code maintainability and readability.

## File Organization

| File | Lines | Description |
|------|-------|-------------|
| `DataService.cs` | 35 | Base partial class containing Fields and Constructors |
| `DataService.CoreUser.cs` | 524 | Core User Methods - User management, authentication, and user-related operations |
| `DataService.CoreWorkflow.cs` | 827 | Core Workflow Methods - Workflow management, status codes, steps, and permissions |
| `DataService.ApplicationWarmup.cs` | 291 | Application Warmup Process Methods - Log management and process monitoring |
| `DataService.Workflow.cs` | 153 | Workflow Methods - Workflow CRUD operations |
| `DataService.Workstatus.cs` | 153 | Workstatus Methods - Work status tracking and management |

## Benefits

- **Improved Maintainability**: Each file focuses on a specific functional area
- **Better Organization**: Related methods are grouped together by region
- **Easier Navigation**: Smaller files are easier to navigate and understand
- **Team Collaboration**: Multiple developers can work on different functional areas simultaneously
- **Code Reviews**: Focused partial classes make code reviews more manageable

## Implementation Details

- All partial classes share the same namespace: `AF.ECT.Data.Services`
- The base class (`DataService.cs`) contains the shared fields (`_contextFactory`, `_logger`) and constructor
- Each partial class includes only the necessary using directives for its methods
- All methods maintain their original functionality and signatures
- XML documentation comments are preserved in each partial class

## Usage

The partial class structure is transparent to consumers of the `DataService` class. All methods are accessible through a single `DataService` instance as before:

```csharp
// Inject as usual
public class MyService
{
    private readonly IDataService _dataService;
    
    public MyService(IDataService dataService)
    {
        _dataService = dataService;
    }
    
    // All methods available regardless of which partial class they're in
    public async Task Example()
    {
        await _dataService.GetUsersOnlineAsync();           // From CoreUser
        await _dataService.GetWorkflowByIdAsync(1);         // From Workflow
        await _dataService.AddSignatureAsync(request);       // From CoreWorkflow
    }
}
```

## Maintenance

When adding new methods:
1. Identify the appropriate functional region
2. Add the method to the corresponding partial class file
3. Ensure proper XML documentation is included
4. Follow existing patterns for logging and error handling

When creating a new region:
1. Create a new partial class file following the naming convention: `DataService.{RegionName}.cs`
2. Include appropriate using directives
3. Add the region markers: `#region {RegionName}` and `#endregion`
4. Document the partial class purpose in the XML summary
