# AI Coding Assistant Instructions for ECTSystem

## Overview
ECTSystem is an Electronic Case Tracking application for ALOD (Army Lodging) built with .NET 9.0, ASP.NET Core, Blazor WebAssembly, .NET Aspire orchestration, gRPC services, Entity Framework Core, and SQL Server. It manages case workflows, user management, and reporting in a distributed microservices architecture.

## Architecture
- **Orchestration**: .NET Aspire manages service discovery, health checks, and observability via `AF.ECT.AppHost/AppHost.cs`.
- **Client**: Blazor WASM in `AF.ECT.Client` communicates with server via gRPC-Web.
- **Server**: ASP.NET Core API in `AF.ECT.Server` exposes gRPC services with JSON transcoding for REST-like endpoints.
- **Data**: EF Core with stored procedures in `AF.ECT.Database` connects to SQL Server.
- **Shared**: Protobuf definitions in `AF.ECT.Shared/Protos` for gRPC contracts.
- **Communication**: Client uses `WorkflowClient` for gRPC calls; server implements `WorkflowServiceImpl`.

## Development Workflow
- **Build**: `dotnet build ElectronicCaseTracking.sln` (uses tasks.json for automation).
- **Run**: `dotnet run` in `AF.ECT.AppHost` launches Aspire dashboard at http://localhost:15888.
- **Debug**: Use launchSettings.json profiles; attach debugger to processes.
- **Test**: `dotnet test` runs xUnit tests in `AF.ECT.Tests`.
- **Database**: Migrations via EF Core; stored procedures for complex queries.

## Conventions and Patterns
- **Naming**: Projects prefixed `AF.ECT.*`; methods use Async suffix; streaming methods end with `Stream`.
- **gRPC**: Unary for single responses, streaming for large datasets (e.g., `GetUsersOnlineStreamAsync`).
- **Resilience**: Polly policies for retries/circuit breakers in `ServiceDefaults`.
- **Security**: Antiforgery tokens in Blazor forms; CORS restricted to trusted origins.
- **Observability**: OpenTelemetry integrated; logs via Serilog; health checks at `/healthz`.
- **Injection**: Dependency injection everywhere; configure in `Program.cs`.

## Examples
- Add gRPC method: Define in `workflow.proto`, implement in `WorkflowServiceImpl.cs`, call via `WorkflowClient`.
- Database query: Use EF Core context with stored procedures like `GetWorkflowById`.
- Client component: In `AF.ECT.Client/Pages`, inject `IWorkflowClient` for data.

## Key Files
- `AF.ECT.AppHost/AppHost.cs`: Service orchestration.
- `AF.ECT.Server/Program.cs`: Server setup with gRPC.
- `AF.ECT.Client/Services/WorkflowClient.cs`: Client gRPC wrapper.
- `AF.ECT.Database/dbo/Tables/`: SQL schemas.
- `Documentation/`: Architectural and REST guidelines.

Focus on gRPC-first design, Aspire for cloud readiness, and military-specific workflows. Avoid generic patterns; follow existing protobuf and EF conventions.