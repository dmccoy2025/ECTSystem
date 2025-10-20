using AF.ECT.Server.Extensions;
using AF.ECT.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebComponents(builder);
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddThemeServices();
builder.Services.AddApplicationCors(builder.Configuration);
builder.Services.AddGrpcServices();
builder.Services.AddHealthCheckServices(builder.Configuration);
builder.Services.AddLoggingServices(builder.Configuration);
builder.Services.AddAntiforgeryServices();
builder.Services.AddResilienceServices();
builder.Services.AddCachingServices(builder.Configuration);
builder.Services.AddRateLimitingServices(builder.Configuration);
builder.Services.AddDocumentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseRouting();
app.UseAntiforgery();
app.UseRateLimiter();
app.MapHealthChecks("/healthz");
app.MapGrpcService<WorkflowServiceImpl>().EnableGrpcWeb();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapFallbackToFile("index.html");

await app.RunAsync();

// Make Program class public for testing
public partial class Program { }
