using Grpc.Core;
using Grpc.Core.Interceptors;

namespace AF.ECT.Server.Interceptors;

/// <summary>
/// Interceptor for handling unhandled exceptions in gRPC services globally.
/// </summary>
public class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionInterceptor"/> class.
    /// </summary>
    /// <param name="logger">The logger for logging exceptions.</param>
    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles unary server calls and catches exceptions.
    /// </summary>
    public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing gRPC request");
            // Include exception details for debugging in test environment
            var errorMessage = context.GetHttpContext()?.RequestServices?
                .GetService<IHostEnvironment>()?.IsDevelopment() == true
                ? $"An internal error occurred: {ex.Message}"
                : "An internal error occurred";
            throw new RpcException(new Status(StatusCode.Internal, errorMessage));
        }
    }
}
