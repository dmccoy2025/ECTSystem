using System.ComponentModel.DataAnnotations;

namespace AF.ECT.Shared.Options;

/// <summary>
/// Configuration options for database connectivity and retry policies.
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// Gets or sets the maximum number of retry attempts for database operations.
    /// </summary>
    [Range(1, 10, ErrorMessage = "MaxRetryCount must be between 1 and 10.")]
    public int MaxRetryCount { get; set; } = 3;

    /// <summary>
    /// Gets or sets the maximum retry delay in seconds for database operations.
    /// </summary>
    [Range(1, 300, ErrorMessage = "MaxRetryDelaySeconds must be between 1 and 300.")]
    public int MaxRetryDelaySeconds { get; set; } = 30;

    /// <summary>
    /// Gets or sets the command timeout in seconds for database operations.
    /// </summary>
    [Range(1, 300, ErrorMessage = "CommandTimeoutSeconds must be between 1 and 300.")]
    public int CommandTimeoutSeconds { get; set; } = 30;
}