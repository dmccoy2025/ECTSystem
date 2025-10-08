using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AF.ECT.Client;
using AF.ECT.Client.Services;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddRadzenComponents();

// Register WorkflowClient for gRPC communication
builder.Services.AddScoped<IWorkflowClient, WorkflowClient>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


await builder.Build().RunAsync();
