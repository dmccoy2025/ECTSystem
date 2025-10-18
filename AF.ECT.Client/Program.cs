using AF.ECT.Client;
using AF.ECT.Shared;
using AF.ECT.Shared.Options;
using AF.ECT.Shared.Services;
using Blazored.LocalStorage;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    return new HttpClient 
    { 
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
    };
});

builder.Services.AddRadzenComponents();
builder.Services.AddBlazoredLocalStorage();

// Configure WorkflowClient options from appsettings.json
builder.Services.Configure<WorkflowClientOptions>(options =>
{
    builder.Configuration.GetSection("WorkflowClientOptions").Bind(options);
});

// Configure gRPC client for browser compatibility
builder.Services.AddScoped(sp =>
{
    return new WorkflowService.WorkflowServiceClient(GrpcChannel.ForAddress(builder.Configuration["ServerUrl"] ?? "https://localhost:5001", new GrpcChannelOptions
    {
        HttpClient = sp.GetRequiredService<HttpClient>(),
        DisposeHttpClient = false
    }));
});

builder.Services.AddScoped<IWorkflowClient, WorkflowClient>();

await builder.Build().RunAsync();
