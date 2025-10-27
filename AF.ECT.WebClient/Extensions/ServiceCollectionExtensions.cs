using AF.ECT.Shared.Extensions;
using Blazored.LocalStorage;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

namespace AF.ECT.WebClient.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services.AddScoped(serviceProvider =>
        {
            return new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            };
        });
        
        services.AddRadzenComponents();
        services.AddBlazoredLocalStorage();

        // Configure and validate WorkflowClient options from appsettings.json
        services.AddValidatedOptions<WorkflowClientOptions>(builder.Configuration);

        // Configure and validate server options
        services.AddValidatedOptions<ServerOptions>(builder.Configuration, "Server");

        // Configure gRPC client for browser compatibility
        services.AddScoped(serviceProvider =>
        {
            var serverOptions = serviceProvider.GetRequiredService<IOptions<ServerOptions>>().Value;
            var httpClient = serviceProvider.GetRequiredService<HttpClient>();
            var channel = GrpcChannelFactory.CreateForBrowser(
                serverOptions.ServerUrl,
                httpClient,
                disposeHttpClient: false);
            return new WorkflowService.WorkflowServiceClient(channel);
        });

        services.AddScoped<IWorkflowClient, WorkflowClient>();

        // Configure OpenTelemetry for client-side tracing
        services.AddClientTelemetry();

        return services;
    }
}
