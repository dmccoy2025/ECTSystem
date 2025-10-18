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

// Configure gRPC client for browser compatibility
var serverUrl = builder.Configuration["ServerUrl"] ?? "https://localhost:5001";
builder.Services.AddScoped(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var channel = GrpcChannel.ForAddress(serverUrl, new GrpcChannelOptions
    {
        HttpClient = httpClient,
        DisposeHttpClient = false
    });
    return new AF.ECT.Shared.WorkflowService.WorkflowServiceClient(channel);
});

// Register WorkflowClient for gRPC communication
builder.Services.AddScoped<IWorkflowClient, WorkflowClient>();

await builder.Build().RunAsync();
