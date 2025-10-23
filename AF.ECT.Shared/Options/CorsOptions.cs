using System.ComponentModel.DataAnnotations;

namespace AF.ECT.Shared.Options;

/// <summary>
/// Configuration options for CORS (Cross-Origin Resource Sharing) policies.
/// </summary>
public class CorsOptions
{
    /// <summary>
    /// Gets or sets the allowed origins for CORS requests.
    /// </summary>
    [Required(ErrorMessage = "AllowedOrigins is required.")]
    [MinLength(1, ErrorMessage = "At least one allowed origin must be specified.")]
    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
}