using AF.ECT.Shared;
using AF.ECT.Shared.Options;
using AF.ECT.Shared.Services;
using Blazored.LocalStorage;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

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
        services.AddOptions<WorkflowClientOptions>().Bind(builder.Configuration.GetSection("WorkflowClientOptions")).ValidateDataAnnotations().ValidateOnStart();

        // Configure and validate server options
        services.AddOptions<ServerOptions>().Bind(builder.Configuration.GetSection("Server")).ValidateDataAnnotations().ValidateOnStart();

        // Configure gRPC client for browser compatibility
        services.AddScoped(serviceProvider =>
        {
            var serverOptions = serviceProvider.GetRequiredService<IOptions<ServerOptions>>().Value;
            return new WorkflowService.WorkflowServiceClient(GrpcChannel.ForAddress(serverOptions.ServerUrl, new GrpcChannelOptions
            {
                HttpClient = serviceProvider.GetRequiredService<HttpClient>(),
                DisposeHttpClient = false
            }));
        });

        services.AddScoped<IWorkflowClient, WorkflowClient>();

        services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .AddGrpcClientInstrumentation()
                .AddOtlpExporter()
            );

        return services;
    }
}
