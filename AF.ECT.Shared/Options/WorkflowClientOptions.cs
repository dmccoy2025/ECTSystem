using System.ComponentModel.DataAnnotations;

namespace AF.ECT.Shared.Options;

/// <summary>
/// Configuration options for the WorkflowClient.
/// </summary>
public class WorkflowClientOptions
{
    /// <summary>
    /// Gets or sets the maximum number of retry attempts for gRPC calls.
    /// </summary>
    [Range(1, 10)]
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Gets or sets the initial retry delay in milliseconds.
    /// </summary>
    [Range(50, 5000)]
    public int InitialRetryDelayMs { get; set; } = 100;

    /// <summary>
    /// Gets or sets the maximum retry delay in milliseconds.
    /// </summary>
    [Range(500, 10000)]
    public int MaxRetryDelayMs { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the request timeout in seconds.
    /// </summary>
    [Range(10, 300)]
    public int RequestTimeoutSeconds { get; set; } = 30;
}