using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for configuring and validating options classes.
/// </summary>
public static class OptionsExtensions
{
    /// <summary>
    /// Configures and validates options using data annotations with startup validation.
    /// </summary>
    /// <typeparam name="TOptions">The options type to configure and validate.</typeparam>
    /// <param name="services">The service collection to add the options to.</param>
    /// <param name="configuration">The configuration instance containing the options values.</param>
    /// <param name="sectionName">Optional section name. Defaults to the type name if not provided.</param>
    /// <returns>The service collection for method chaining.</returns>
    /// <remarks>
    /// This extension method provides a consistent, reusable pattern for options configuration
    /// that combines binding, validation, and startup checks in a single call. It ensures that:
    /// <list type="bullet">
    /// <item><description>Options are bound from the specified configuration section</description></item>
    /// <item><description>Data annotations on the options class are validated</description></item>
    /// <item><description>Validation errors are caught at application startup rather than runtime</description></item>
    /// </list>
    /// 
    /// This follows Microsoft best practices for options pattern validation.
    /// See: https://learn.microsoft.com/en-us/dotnet/core/extensions/options
    /// </remarks>
    /// <example>
    /// <code>
    /// // Options class with validation attributes
    /// public class DatabaseOptions
    /// {
    ///     [Required]
    ///     [Range(1, 10)]
    ///     public int MaxRetryCount { get; set; } = 3;
    /// }
    /// 
    /// // Usage in Program.cs or ServiceCollectionExtensions
    /// services.AddValidatedOptions&lt;DatabaseOptions&gt;(configuration);
    /// 
    /// // Or with custom section name
    /// services.AddValidatedOptions&lt;ServerOptions&gt;(configuration, "Server");
    /// </code>
    /// </example>
    public static IServiceCollection AddValidatedOptions<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? sectionName = null)
        where TOptions : class
    {
        sectionName ??= typeof(TOptions).Name;

        services.AddOptions<TOptions>()
            .Bind(configuration.GetSection(sectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
