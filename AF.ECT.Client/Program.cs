using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AF.ECT.Client;
using AF.ECT.Client.Services;
using AF.ECT.Client.Options;
using Radzen;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddRadzenComponents();
builder.Services.AddBlazoredLocalStorage();

// Configure WorkflowClient options from appsettings.json
builder.Services.Configure<WorkflowClientOptions>(options => builder.Configuration.GetSection("WorkflowClient").Bind(options));

// Register WorkflowClient for gRPC communication
builder.Services.AddScoped<IWorkflowClient, WorkflowClient>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


await builder.Build().RunAsync();
