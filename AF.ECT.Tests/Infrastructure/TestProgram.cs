using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AF.ECT.Server.Extensions;
// using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

// Configure dependency injection services
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddApplicationCors()
    .AddHealthChecks(builder.Configuration)
    .AddAntiforgeryServices()
    .AddGrpcServices();

var app = builder.Build();

// Configure forwarded headers for proxy scenarios
var forwardingOptions = new ForwardedHeadersOptions()
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
};
forwardingOptions.KnownNetworks.Clear();
forwardingOptions.KnownProxies.Clear();

app.UseForwardedHeaders(forwardingOptions);

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error", createScopeForErrors: true);

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable gRPC services for testing
app.MapGrpcService<AF.ECT.Server.Services.WorkflowServiceImpl>();
app.MapGrpcReflectionService();

app.UseRouting();
// app.UseIpRateLimiting();
app.UseCors();

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.MapControllers();
app.UseAntiforgery();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready")
});
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
});

await app.RunAsync();

public partial class TestProgram
{
    // This makes TestProgram accessible for testing
}