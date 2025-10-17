using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AF.ECT.Client;
using Radzen;
using Blazored.LocalStorage;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using AF.ECT.Shared.Services;
using AF.ECT.Shared.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddRadzenComponents();
builder.Services.AddBlazoredLocalStorage();

// Configure WorkflowClient options from appsettings.json
builder.Services.Configure<WorkflowClientOptions>(options => builder.Configuration.GetSection("WorkflowClient").Bind(options));

// Create gRPC channel for browser compatibility
builder.Services.AddScoped<GrpcChannel>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var isHttps = httpClient.BaseAddress!.Scheme == "https";

    return GrpcChannel.ForAddress(httpClient.BaseAddress!,
        new GrpcChannelOptions
        {
            HttpHandler = new GrpcWebHandler(),
            MaxReceiveMessageSize = 10 * 1024 * 1024, // 10MB max message size
            MaxSendMessageSize = 10 * 1024 * 1024, // 10MB max message size
            Credentials = isHttps ? Grpc.Core.ChannelCredentials.SecureSsl : Grpc.Core.ChannelCredentials.Insecure
        });
});

// Register WorkflowClient for gRPC communication
builder.Services.AddScoped<IWorkflowClient, WorkflowClient>();

await builder.Build().RunAsync();
