using AF.ECT.Shared.Services;
using AF.ECT.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AF.ECT.Client.Pages;

/// <summary>
/// Code-behind for the Index page.
/// </summary>
public partial class Index
{
    /// <summary>
    /// Gets or sets the workflow client for gRPC communication.
    /// </summary>
    [Inject]
    private IWorkflowClient WorkflowClient { get; set; } = default!;

    /// <summary>
    /// Log data collection for the grid.
    /// </summary>
    private List<LogItem> logData = new();

    /// <summary>
    /// Total count of logs for pagination.
    /// </summary>
    private int totalCount = 0;

    /// <summary>
    /// Loading state indicator.
    /// </summary>
    private bool isLoading = false;

    /// <summary>
    /// Error message if data loading fails.
    /// </summary>
    private string? errorMessage;

    public Index()
    {
        
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await LoadDataAsync(new LoadDataArgs
        {
            Skip = 0,
            Top = 10,
            OrderBy = "ExecutionDate desc"
        });
    }

    /// <summary>
    /// Handles the LoadData event from RadzenDataGrid with pagination, filtering, and sorting.
    /// </summary>
    /// <param name="args">The LoadDataArgs containing pagination, filter, and sort information.</param>
    private async Task LoadDataAsync(LoadDataArgs args)
    {
        try
        {
            isLoading = true;
            errorMessage = null;

            // Calculate page number from skip and top
            var pageNumber = (args.Skip ?? 0) / (args.Top ?? 10) + 1;
            var pageSize = args.Top ?? 10;

            // Extract sort information
            var sortBy = args.OrderBy?.Split(' ').FirstOrDefault() ?? "ExecutionDate";
            var sortOrder = args.OrderBy?.Contains(" desc") == true ? "DESC" : "ASC";

            // Extract filter information (basic implementation)
            string? processNameFilter = null;
            string? messageFilter = null;

            if (args.Filters != null)
            {
                foreach (var filter in args.Filters)
                {
                    if (filter.Property == "ProcessName" && filter.FilterValue != null)
                        processNameFilter = filter.FilterValue.ToString();
                    else if (filter.Property == "Message" && filter.FilterValue != null)
                        messageFilter = filter.FilterValue.ToString();
                }
            }

            var response = await WorkflowClient.GetAllLogsPaginationAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                processName: processNameFilter,
                messageFilter: messageFilter,
                sortBy: sortBy,
                sortOrder: sortOrder
            );

            logData = response.Items.ToList();
            totalCount = response.TotalCount;
        }
        catch (Exception ex)
        {
            errorMessage = $"Error loading logs: {ex.Message}";
            logData = new List<LogItem>();
            totalCount = 0;
        }
        finally
        {
            isLoading = false;
        }
    }
}
