using AF.ECT.Server.Extensions;
using AF.ECT.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Add application services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddApplicationCors();
builder.Services.AddGrpcServices();
builder.Services.AddHealthChecks(builder.Configuration);
builder.Services.AddAntiforgeryServices();
builder.Services.AddRateLimitingServices(builder.Configuration);
builder.Services.AddRateLimiter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAntiforgery();
app.UseRateLimiter();

// Map health checks endpoint
app.MapHealthChecks("/healthz");

// Map gRPC services
app.MapGrpcService<WorkflowServiceImpl>();

// Map fallback for SPA
app.MapFallbackToFile("index.html");

app.Run();
