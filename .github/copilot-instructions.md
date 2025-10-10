# AI Coding Assistant Instructions for ECTSystem

## CRITICAL: Build Verification Requirement
**ALWAYS verify the solution builds successfully before responding to any code changes.**

- After making ANY code modifications, run `dotnet build ElectronicCaseTracking.sln` and wait for completion
- Only respond after confirming the build succeeds (exit code 0)
- If build fails, fix errors before providing any analysis or next steps
- Never assume builds will succeed - always verify explicitly

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
- **Audit Logging**: Client-side audit logging with correlation IDs for end-to-end traceability; implemented in `WorkflowClient` methods for military-grade compliance.
- **Resilience**: Polly policies for retries/circuit breakers in `ServiceDefaults`.
- **Security**: Antiforgery tokens in Blazor forms; CORS restricted to trusted origins.
- **Observability**: OpenTelemetry integrated; logs via Serilog; health checks at `/healthz`; structured audit events with performance metrics.
- **Injection**: Dependency injection everywhere; configure in `Program.cs`.
- **Code Refactoring**: Follow SOLID principles; use partial classes and regions for organization. Use static methods and extension methods to keep code clean.

## Audit Logging Implementation
- **Client-Side Audit**: All unary gRPC methods in `WorkflowClient.cs` include audit logging with correlation IDs, performance timing, and structured events.
- **Correlation IDs**: Generated per operation for linking client and server audit trails using `GenerateCorrelationId()`.
- **Audit Events**: Logged via `LogAuditEvent()` with method name, duration, success/failure status, and parameter data.
- **Performance Metrics**: Stopwatch-based timing for all gRPC operations.
- **Structured Logging**: Audit events include timestamp, correlation ID, method name, duration, success status, error messages, and additional context.
- **Coverage**: Applied to all applicable unary methods; streaming methods excluded as they are not single operations.
- **Compliance**: Supports military-grade observability and end-to-end traceability requirements.

## Examples
- Add gRPC method: Define in `workflow.proto`, implement in `WorkflowServiceImpl.cs`, call via `WorkflowClient`.
- Database query: Use EF Core context with stored procedures like `GetWorkflowById`.
- Client component: In `AF.ECT.Client/Pages`, inject `IWorkflowClient` for data.

## Key Files
- `AF.ECT.AppHost/AppHost.cs`: Service orchestration.
- `AF.ECT.Server/Program.cs`: Server setup with gRPC.
- `AF.ECT.Client/Services/WorkflowClient.cs`: Client gRPC wrapper with comprehensive audit logging and correlation IDs.
- `AF.ECT.Database/dbo/Tables/`: SQL schemas.
- `Documentation/`: Architectural and REST guidelines.

## Documentation
- Use XML documentation comments for all methods, classes, properties and fields to enable IntelliSense and API documentation.
- Format: `/// <summary>Description</summary>` for summaries, `<param name="param">Description</param>` for parameters, `<returns>Description</returns>` for return values, `<exception cref="ExceptionType">Description</exception>` for exceptions.
- Enable XML documentation generation in project files: `<GenerateDocumentationFile>true</GenerateDocumentationFile>`.
- Example: See `AF.ECT.Client/Services/WorkflowClient.cs` for extensive XML comments on gRPC client methods.

Focus on gRPC-first design, Aspire for cloud readiness, and military-specific workflows. Avoid generic patterns; follow existing protobuf and EF conventions.