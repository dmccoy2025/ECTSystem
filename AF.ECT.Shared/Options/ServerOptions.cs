using System.ComponentModel.DataAnnotations;

namespace AF.ECT.Shared.Options;

/// <summary>
/// Configuration options for server connectivity.
/// </summary>
public class ServerOptions
{
    /// <summary>
    /// Gets or sets the base URL for the server.
    /// </summary>
    [Required(ErrorMessage = "ServerUrl is required.")]
    [Url(ErrorMessage = "ServerUrl must be a valid URL.")]
    public string ServerUrl { get; set; } = "https://localhost:7000";
}