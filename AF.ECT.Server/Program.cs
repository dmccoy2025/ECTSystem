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
//builder.Services.AddHealthChecks(builder.Configuration);
//builder.Services.AddAntiforgeryServices();
//builder.Services.AddRateLimitingServices(builder.Configuration);
//builder.Services.AddRateLimiter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS before GrpcWeb
app.UseCors();

// Enable gRPC-Web for browser clients (MUST be before UseRouting)
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

// Explicitly call UseRouting to ensure gRPC-Web is before it
app.UseRouting();

app.UseAntiforgery();
//app.UseRateLimiter();

// Map health checks endpoint
//app.MapHealthChecks("/healthz");

// Map gRPC services with GrpcWeb enabled
app.MapGrpcService<WorkflowServiceImpl>().EnableGrpcWeb();

// Map gRPC reflection service (for development)
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

// Map fallback for SPA (must be last)
app.MapFallbackToFile("index.html");

app.Run();
