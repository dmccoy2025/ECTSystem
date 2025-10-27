using Grpc.Net.Client;

namespace AF.ECT.Shared.Extensions;

/// <summary>
/// Factory for creating configured gRPC channels with consistent settings.
/// </summary>
/// <remarks>
/// This factory centralizes gRPC channel configuration to ensure consistency across
/// the application (WebClient, Tests, and any future gRPC clients). It provides
/// standardized channel options including message size limits, timeouts, and HTTP client
/// configuration for both browser-based (Blazor WASM) and server-side scenarios.
/// </remarks>
public static class GrpcChannelFactory
{
    /// <summary>
    /// Default maximum receive message size (100 MB).
    /// </summary>
    public const int DefaultMaxReceiveMessageSize = 100 * 1024 * 1024;

    /// <summary>
    /// Default maximum send message size (100 MB).
    /// </summary>
    public const int DefaultMaxSendMessageSize = 100 * 1024 * 1024;

    /// <summary>
    /// Creates a gRPC channel for browser-based clients (Blazor WebAssembly).
    /// </summary>
    /// <param name="address">The server address (e.g., "http://localhost:5173").</param>
    /// <param name="httpClient">The HttpClient instance to use for the channel.</param>
    /// <param name="disposeHttpClient">Whether to dispose the HttpClient when the channel is disposed. Default is false for shared HttpClient instances.</param>
    /// <returns>A configured GrpcChannel instance.</returns>
    /// <remarks>
    /// This method is designed for Blazor WebAssembly scenarios where an HttpClient
    /// is typically managed by the DI container and should not be disposed by the channel.
    /// The channel uses gRPC-Web protocol which is required for browser-based gRPC clients.
    /// </remarks>
    /// <example>
    /// <code>
    /// var channel = GrpcChannelFactory.CreateForBrowser(
    ///     "http://localhost:5173", 
    ///     httpClient, 
    ///     disposeHttpClient: false
    /// );
    /// var client = new MyService.MyServiceClient(channel);
    /// </code>
    /// </example>
    public static GrpcChannel CreateForBrowser(
        string address,
        HttpClient httpClient,
        bool disposeHttpClient = false)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address cannot be null or whitespace.", nameof(address));
        }

        if (httpClient == null)
        {
            throw new ArgumentNullException(nameof(httpClient));
        }

        return GrpcChannel.ForAddress(address, new GrpcChannelOptions
        {
            HttpClient = httpClient,
            DisposeHttpClient = disposeHttpClient,
            MaxReceiveMessageSize = DefaultMaxReceiveMessageSize,
            MaxSendMessageSize = DefaultMaxSendMessageSize
        });
    }

    /// <summary>
    /// Creates a gRPC channel for testing scenarios.
    /// </summary>
    /// <param name="baseAddress">The test server base address (typically from WebApplicationFactory).</param>
    /// <param name="httpClient">The test HttpClient instance.</param>
    /// <returns>A configured GrpcChannel instance for testing.</returns>
    /// <remarks>
    /// This method is optimized for integration testing scenarios where the channel
    /// needs to communicate with a test server instance. The HttpClient is provided
    /// by the test framework and the channel takes ownership of it.
    /// </remarks>
    /// <example>
    /// <code>
    /// var testClient = testFactory.CreateClient();
    /// var channel = GrpcChannelFactory.CreateForTesting(testClient.BaseAddress!, testClient);
    /// var grpcClient = new MyService.MyServiceClient(channel);
    /// </code>
    /// </example>
    public static GrpcChannel CreateForTesting(Uri baseAddress, HttpClient httpClient)
    {
        if (baseAddress == null)
        {
            throw new ArgumentNullException(nameof(baseAddress));
        }

        if (httpClient == null)
        {
            throw new ArgumentNullException(nameof(httpClient));
        }

        return GrpcChannel.ForAddress(baseAddress, new GrpcChannelOptions
        {
            HttpClient = httpClient,
            MaxReceiveMessageSize = DefaultMaxReceiveMessageSize,
            MaxSendMessageSize = DefaultMaxSendMessageSize
        });
    }

    /// <summary>
    /// Creates a gRPC channel with custom options for advanced scenarios.
    /// </summary>
    /// <param name="address">The server address.</param>
    /// <param name="configureOptions">Action to configure additional channel options.</param>
    /// <returns>A configured GrpcChannel instance.</returns>
    /// <remarks>
    /// This method provides maximum flexibility for scenarios requiring custom
    /// channel configuration beyond the standard defaults. Use this when you need
    /// to configure credentials, interceptors, or other advanced settings.
    /// </remarks>
    /// <example>
    /// <code>
    /// var channel = GrpcChannelFactory.CreateWithOptions(
    ///     "https://api.example.com",
    ///     options =>
    ///     {
    ///         options.Credentials = ChannelCredentials.SecureSsl;
    ///         options.MaxRetryAttempts = 5;
    ///     }
    /// );
    /// </code>
    /// </example>
    public static GrpcChannel CreateWithOptions(
        string address,
        Action<GrpcChannelOptions> configureOptions)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address cannot be null or whitespace.", nameof(address));
        }

        if (configureOptions == null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        var options = new GrpcChannelOptions
        {
            MaxReceiveMessageSize = DefaultMaxReceiveMessageSize,
            MaxSendMessageSize = DefaultMaxSendMessageSize
        };

        configureOptions(options);

        return GrpcChannel.ForAddress(address, options);
    }
}
