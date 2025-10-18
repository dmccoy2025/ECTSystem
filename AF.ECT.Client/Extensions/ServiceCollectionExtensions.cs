using AF.ECT.Shared;
using AF.ECT.Shared.Options;
using AF.ECT.Shared.Services;
using Blazored.LocalStorage;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

namespace AF.ECT.Client.Extensions;

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

        // Configure WorkflowClient options from appsettings.json
        services.Configure<WorkflowClientOptions>(options =>
        {
            builder.Configuration.GetSection("WorkflowClientOptions").Bind(options);
        });

        // Configure gRPC client for browser compatibility
        services.AddScoped(serviceProvider =>
        {
            return new WorkflowService.WorkflowServiceClient(GrpcChannel.ForAddress(builder.Configuration["ServerUrl"] ?? "https://localhost:5001", new GrpcChannelOptions
            {
                HttpClient = serviceProvider.GetRequiredService<HttpClient>(),
                DisposeHttpClient = false
            }));
        });

        services.AddScoped<IWorkflowClient, WorkflowClient>();

        return services;
    }
}
