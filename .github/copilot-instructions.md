# AI Coding Assistant Instructions for ECTSystem

## CRITICAL: Microsoft Best Patterns and Practices
**ALWAYS use Microsoft Best Patterns and Practices as the basis for all recommendations, architectural decisions, and code suggestions.**

- Follow official Microsoft guidance from Microsoft Learn, Azure Architecture Center, and .NET documentation
- Adhere to established patterns for .NET, ASP.NET Core, Entity Framework Core, and Azure services
- Apply Microsoft's recommended practices for security, performance, scalability, and maintainability
- Reference Microsoft's best practices for cloud-native applications, microservices, and distributed systems
- Consult Microsoft's guidance for specific technologies used in this project (gRPC, Blazor, .NET Aspire, etc.)

## CRITICAL: Build Verification Requirement
**ALWAYS verify the solution builds successfully before responding to any code changes. Do not rebuild solution if markdown are changed**

- After making ANY code modifications, run `dotnet build ElectronicCaseTracking.sln` and wait for completion
- Only respond after confirming the build succeeds (exit code 0)
- If build fails, fix errors before providing any analysis or next steps
- Never assume builds will succeed - always verify explicitly

## Overview
ECTSystem is an Electronic Case Tracking application for ALOD (Army Lodging) built with .NET 9.0, ASP.NET Core, Blazor WebAssembly, Win UI (desktop), .NET Aspire orchestration, gRPC services, Entity Framework Core, and SQL Server. It manages case workflows, user management, and reporting in a distributed microservices architecture.

## Architecture
- **Orchestration**: .NET Aspire manages service discovery, health checks, and observability via `AF.ECT.AppHost/AppHost.cs`.
- **Client**: Blazor WASM in `AF.ECT.WebClient` and Win UI desktop in `AF.ECT.WindowsClient` communicate with server via gRPC-Web.
- **Server**: ASP.NET Core API in `AF.ECT.Server` exposes gRPC services with JSON transcoding for REST-like endpoints.
- **Data**: EF Core with stored procedures in `AF.ECT.Database` connects to SQL Server; data access layer in `AF.ECT.Data`.
- **Shared**: Protobuf definitions in `AF.ECT.Shared/Protos` for gRPC contracts.
- **Communication**: Client uses `WorkflowClient` for gRPC calls; server implements `WorkflowServiceImpl`.

## Development Workflow
- **Build**: `dotnet build ElectronicCaseTracking.sln` (uses tasks.json for automation).
- **Run**: `dotnet run` in `AF.ECT.AppHost` launches Aspire dashboard at http://localhost:15888.
- **Debug**: Use launchSettings.json profiles; attach debugger to processes.
- **Test**: `dotnet test` runs xUnit tests in `AF.ECT.Tests`.
- **Package Updates**: `dotnet list ElectronicCaseTracking.sln package --outdated` to check for outdated NuGet packages.
- **Database**: Migrations via EF Core; stored procedures for complex queries.


## Conventions and Patterns
- **Naming**: Projects prefixed `AF.ECT.*`; methods use Async suffix; streaming methods end with `Stream`.
- **gRPC**: Unary for single responses, streaming for large datasets (e.g., `GetUsersOnlineStreamAsync`).
- **Configuration Validation**: Use strongly-typed options classes with data annotations for all configuration settings. Validate on startup using `ValidateDataAnnotations().ValidateOnStart()`. Options classes in `AF.ECT.Shared/Options` with `[Required]`, `[Range]`, `[Url]`, etc.
- **Audit Logging**: Client-side audit logging with correlation IDs for end-to-end traceability; implemented in `WorkflowClient` methods for military-grade compliance.
- **Resilience**: Polly policies for retries/circuit breakers in `ServiceDefaults`.
- **Security**: Antiforgery tokens in Blazor forms; CORS restricted to trusted origins.
- **Observability**: OpenTelemetry integrated; logs via Serilog; health checks at `/healthz`; structured audit events with performance metrics.
- **Injection**: Dependency injection everywhere; configure in `Program.cs`.
- **Code Refactoring**: Follow SOLID principles; use partial classes and regions for organization. Use static methods and extension methods to keep code clean.
- **Styling**: Inline temporary variables wherever possible; use block body for lambda expressions.
- **File-Scoped Namespaces**: Use file-scoped namespaces for all C# files throughout the solution to improve readability and reduce indentation.
- **Usings Cleanup**: Remove unused and duplicate `using` statements in all files to keep code clean and maintainable.
- **Project Versioning**: All projects added to the solution must target at least .NET 9.0 to maintain compatibility with the global.json specification.

## Audit Logging Implementation
- **Automated Auditing**: Implemented Audit.NET for comprehensive auditing of Entity Framework changes and gRPC operations.
- **EF Core Auditing**: Automatic audit logging for all database operations using Audit.EntityFramework.Core with SQL Server storage.
- **gRPC Auditing**: Client-side gRPC calls audited via AuditScope.Create() with correlation IDs, performance metrics, and structured events.
- **Correlation IDs**: Generated per operation for linking client and server audit trails using `GenerateCorrelationId()`.
- **Audit Events**: Stored in AuditLogs table with EventType ("EF:{entity}" or "gRPC:{method}"), timestamps, duration, success/failure status, and parameter data.
- **Performance Metrics**: Automatic timing for all operations.
- **Structured Logging**: Audit events include timestamp, correlation ID, method name, duration, success status, error messages, and additional context.
- **Coverage**: Applied to all EF operations and unary gRPC methods; streaming methods excluded as they are not single operations.
- **Compliance**: Supports military-grade observability and end-to-end traceability requirements with automated audit trails.

## Examples
- Add gRPC method: Define in `workflow.proto`, implement in `WorkflowServiceImpl.cs`, call via `WorkflowClient`.
- Database query: Use EF Core context with stored procedures like `GetWorkflowById`.
- Client component: In `AF.ECT.WebClient/Pages`, inject `IWorkflowClient` for data.

## Key Files
- `AF.ECT.AppHost/AppHost.cs`: Service orchestration.
- `AF.ECT.Server/Program.cs`: Server setup with gRPC.
- `AF.ECT.Server/Extensions/ServiceCollectionExtensions.cs`: Audit.NET configuration for EF Core and SQL Server storage.
- `AF.ECT.Shared/Services/WorkflowClient.cs`: Shared gRPC client wrapper with Audit.NET audit logging and correlation IDs.
- `AF.ECT.Shared/Options/`: Strongly-typed configuration options with validation (DatabaseOptions, CorsOptions, ServerOptions, WorkflowClientOptions).
- `AF.ECT.WindowsClient/`: Win UI desktop application.
- `AF.ECT.Database/dbo/Tables/`: SQL schemas.
- `AF.ECT.Data/`: Data access layer.
- `Documentation/`: Architectural and REST guidelines.

## Documentation
- Use XML documentation comments for all methods, classes, properties and fields to enable IntelliSense and API documentation.
- Format: `/// <summary>Description</summary>` for summaries, `<param name="param">Description</param>` for parameters, `<returns>Description</returns>` for return values, `<exception cref="ExceptionType">Description</exception>` for exceptions.
- Enable XML documentation generation in project files: `<GenerateDocumentationFile>true</GenerateDocumentationFile>`.
- Configuration: Use strongly-typed options classes with data annotations; validate on startup with `ValidateDataAnnotations().ValidateOnStart()`.
- Example: See `AF.ECT.WebClient/Services/WorkflowClient.cs` for extensive XML comments on gRPC client methods.

Focus on gRPC-first design, Aspire for cloud readiness, and military-specific workflows. Avoid generic patterns; follow existing protobuf and EF conventions.