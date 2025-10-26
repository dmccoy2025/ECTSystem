# ECTSystem Developer Onboarding Guide

Welcome to the Electronic Case Tracking (ECT) System development team! This guide will help you get set up and productive quickly.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Development Environment Setup](#development-environment-setup)
3. [Getting the Code](#getting-the-code)
4. [Building the Solution](#building-the-solution)
5. [Running the Application](#running-the-application)
6. [Development Workflow](#development-workflow)
7. [Architecture Overview](#architecture-overview)
8. [Key Concepts](#key-concepts)
9. [Testing](#testing)
10. [Deployment](#deployment)
11. [Resources](#resources)

---

## Prerequisites

Before starting, ensure you have the following installed:

### Required Software

| Software | Version | Download Link | Purpose |
|----------|---------|---------------|---------|
| .NET SDK | 9.0.306 or later | [Download](https://dotnet.microsoft.com/download/dotnet/9.0) | Core runtime and development tools |
| Visual Studio 2022 | 17.12+ | [Download](https://visualstudio.microsoft.com/) | Primary IDE (Community/Professional/Enterprise) |
| SQL Server | 2022+ | [Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) | Database (Developer/Express edition OK) |
| Git | Latest | [Download](https://git-scm.com/) | Version control |
| Windows App SDK | Latest | [Download](https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/) | For Win UI desktop client |

### Optional Tools

| Tool | Purpose |
|------|---------|
| [VS Code](https://code.visualstudio.com/) | Lightweight editor alternative |
| [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) | Database management |
| [Postman](https://www.postman.com/) | API testing |
| [grpcurl](https://github.com/fullstorydev/grpcurl) | gRPC testing from command line |
| [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/) | Cross-platform database tool |

### Visual Studio Workloads

When installing Visual Studio, ensure these workloads are selected:

- ✅ **ASP.NET and web development**
- ✅ **.NET desktop development**
- ✅ **Data storage and processing**
- ✅ **.NET Multi-platform App UI development** (for MAUI mobile client)

---

## Development Environment Setup

### Step 1: Install .NET SDK

1. Download .NET 9.0 SDK from https://dotnet.microsoft.com/download/dotnet/9.0
2. Run the installer and follow prompts
3. Verify installation:
   ```powershell
   dotnet --version
   # Should show 9.0.306 or later
   ```

### Step 2: Install SQL Server

1. Download SQL Server 2022 Developer Edition (free for development)
2. During installation, select:
   - **Database Engine Services**
   - **Windows Authentication** (recommended for development)
3. Note the instance name (usually `MSSQLSERVER` for default instance)
4. Verify installation:
   ```powershell
   sqlcmd -S localhost -E -Q "SELECT @@VERSION"
   ```

### Step 3: Configure SQL Server

1. Enable TCP/IP protocol:
   - Open **SQL Server Configuration Manager**
   - Navigate to **SQL Server Network Configuration** → **Protocols for MSSQLSERVER**
   - Right-click **TCP/IP** → **Enable**
   - Restart SQL Server service

2. Create ECT database (will be done via deployment scripts later)

### Step 4: Install Visual Studio

1. Download Visual Studio 2022 (any edition)
2. Select workloads listed above
3. Install optional components:
   - GitHub extension for Visual Studio
   - IntelliCode
   - Code cleanup

---

## Getting the Code

### Clone the Repository

```powershell
# Navigate to your preferred development directory
cd C:\Dev

# Clone the repository
git clone https://github.com/dmccoy2025/ECTSystem.git

# Navigate into the cloned directory
cd ECTSystem
```

### Verify Repository Structure

You should see the following structure:

```
ECTSystem/
├── AF.ECT.AppHost/          # .NET Aspire orchestration
├── AF.ECT.Server/           # ASP.NET Core API server
├── AF.ECT.WebClient/        # Blazor WebAssembly client
├── AF.ECT.WindowsClient/    # Win UI desktop client
├── AF.ECT.MobileClient/     # .NET MAUI mobile client (future development)
├── AF.ECT.Data/             # Data access layer (EF Core)
├── AF.ECT.Database/         # SQL Server database project
├── AF.ECT.Shared/           # Shared models and gRPC contracts
├── AF.ECT.ServiceDefaults/  # Common service configuration
├── AF.ECT.Tests/            # Unit and integration tests
├── AF.ECT.Wiki/             # Documentation wiki (Blazor)
├── ElectronicCaseTracking.sln
└── README.md
```

---

## Building the Solution

### First-Time Build

1. **Open Solution in Visual Studio**
   - Double-click `ElectronicCaseTracking.sln`
   - Wait for Visual Studio to restore NuGet packages

2. **Build from Visual Studio**
   - Press `Ctrl+Shift+B` or use Build → Build Solution
   - Check Output window for any errors

3. **Build from Command Line** (alternative)
   ```powershell
   dotnet build ElectronicCaseTracking.sln
   ```

### Common Build Issues

If you encounter build errors:

1. **Restore NuGet Packages**
   ```powershell
   dotnet restore ElectronicCaseTracking.sln
   ```

2. **Clean and Rebuild**
   ```powershell
   dotnet clean ElectronicCaseTracking.sln
   dotnet build ElectronicCaseTracking.sln
   ```

3. See [TROUBLESHOOTING.md](TROUBLESHOOTING.md) for detailed solutions

---

## Running the Application

### Using .NET Aspire (Recommended)

.NET Aspire provides orchestration, service discovery, and an integrated dashboard.

1. **Set Startup Project**
   - In Visual Studio Solution Explorer, right-click `AF.ECT.AppHost`
   - Select **Set as Startup Project**

2. **Run with Debugging**
   - Press `F5` or click the green **Start** button
   - Aspire dashboard will open at http://localhost:15888

3. **View Services**
   - Dashboard shows all running services (Server, WebClient, Wiki)
   - Click on service to view logs, traces, and metrics
   - Access WebClient at http://localhost:5000 (or port shown in dashboard)

### Manual Startup (Without Aspire)

If you prefer to run services individually:

#### 1. Start the Server

```powershell
cd AF.ECT.Server
dotnet run
```

Server will start on https://localhost:7293 and http://localhost:5173

#### 2. Start the WebClient

```powershell
cd AF.ECT.WebClient
dotnet run
```

WebClient will start on https://localhost:7000 and http://localhost:5000

#### 3. Configure WebClient to Connect to Server

In `AF.ECT.WebClient/appsettings.Development.json`:

```json
{
  "WorkflowClientOptions": {
    "ServerUrl": "http://localhost:5173"
  }
}
```

---

## Development Workflow

### Daily Workflow

1. **Pull Latest Changes**
   ```powershell
   git pull origin main
   ```

2. **Create Feature Branch**
   ```powershell
   git checkout -b feature/your-feature-name
   ```

3. **Make Changes**
   - Follow coding standards in [CONTRIBUTING.md](CONTRIBUTING.md)
   - Add XML documentation to all public APIs
   - Write unit tests for new functionality

4. **Build and Test**
   ```powershell
   dotnet build ElectronicCaseTracking.sln
   dotnet test
   ```

5. **Commit Changes**
   ```powershell
   git add .
   git commit -m "feat: Add your feature description"
   ```

6. **Push and Create PR**
   ```powershell
   git push origin feature/your-feature-name
   ```
   - Open Pull Request on GitHub
   - Request code review from team members

### Coding Standards

- **Use file-scoped namespaces** throughout the solution
- **Follow SOLID principles** for all classes
- **Add XML documentation** to all public APIs (required)
- **Use meaningful variable names** (avoid single-letter variables except in loops)
- **Keep methods focused** (single responsibility)
- See [CONTRIBUTING.md](CONTRIBUTING.md) for complete guidelines

---

## Architecture Overview

ECTSystem follows a **distributed microservices architecture** using **.NET Aspire** for orchestration.

### High-Level Architecture

```
┌─────────────────┐
│   .NET Aspire   │  ← Orchestration & Service Discovery
│    Dashboard    │    (http://localhost:15888)
└────────┬────────┘
         │
    ┌────┴─────┬──────────┬────────────┐
    │          │          │            │
┌───▼───┐  ┌──▼───┐  ┌──▼──────┐  ┌──▼────┐
│ Server│  │ Web  │  │ Desktop │  │ Wiki  │
│  API  │  │Client│  │ Client  │  │  App  │
│ gRPC  │  │Blazor│  │ Win UI  │  │Blazor │
└───┬───┘  └──┬───┘  └────┬────┘  └───────┘
    │         │           │
    │    gRPC-Web    gRPC │
    │         │           │
┌───▼─────────▼───────────▼────┐
│      Data Access Layer       │  ← Entity Framework Core
│        (AF.ECT.Data)          │    + Audit.NET Interceptors
└──────────────┬────────────────┘
               │
       ┌───────▼────────┐
       │   SQL Server   │  ← Database with stored procedures
       │  (ALOD Schema) │    + AuditLogs table
       └────────────────┘
```

### Technology Stack

| Layer | Technologies |
|-------|-------------|
| **Frontend** | Blazor WebAssembly, Win UI 3, .NET MAUI (planned) |
| **Backend** | ASP.NET Core 9.0, gRPC, gRPC-Web |
| **Data Access** | Entity Framework Core 9.0, Stored Procedures |
| **Database** | SQL Server 2022+, SSDT Database Projects |
| **Orchestration** | .NET Aspire |
| **Observability** | OpenTelemetry, Serilog, Audit.NET |
| **Resilience** | Polly (Retry, Circuit Breaker, Timeout) |
| **Security** | Antiforgery Tokens, CORS, Rate Limiting |
| **Testing** | xUnit, FluentAssertions, Moq |

For detailed architecture diagrams, see [ARCHITECTURE_DIAGRAMS.md](ARCHITECTURE_DIAGRAMS.md).

---

## Key Concepts

### gRPC Communication

ECTSystem uses gRPC for efficient client-server communication. Blazor WebAssembly clients use gRPC-Web (HTTP/1.1 compatible), while desktop clients can use native gRPC (HTTP/2).

**Protobuf Definitions** are in `AF.ECT.Shared/Protos/`:

```protobuf
syntax = "proto3";

package workflow;

service WorkflowService {
  rpc GetUserById(GetUserByIdRequest) returns (GetUserByIdResponse);
  rpc GetUsersOnlineStream(Empty) returns (stream GetUsersOnlineResponse);
}
```

**Server Implementation** in `AF.ECT.Server/Services/WorkflowServiceImpl.cs`:

```csharp
public class WorkflowServiceImpl : WorkflowService.WorkflowServiceBase
{
    public override async Task<GetUserByIdResponse> GetUserById(
        GetUserByIdRequest request, ServerCallContext context)
    {
        // Implementation with automatic EF Core audit logging
        var user = await _dataService.GetUserByIdAsync(request.UserId);
        return new GetUserByIdResponse { User = user };
    }
}
```

**Middleware Pipeline** (from `AF.ECT.Server/Program.cs`):

The server uses a carefully ordered middleware pipeline:

1. **HTTPS Redirection** - Force secure connections
2. **CORS** - Handle cross-origin requests (must be early for preflight)
3. **gRPC-Web** - Protocol translation for browser clients
4. **Routing** - Endpoint matching
5. **Antiforgery** - Token validation for CSRF protection
6. **Rate Limiting** - IP-based throttling
7. **Endpoints** - gRPC services and health checks

This order is critical for security and performance.

**Client Usage** in `AF.ECT.Shared/Services/WorkflowClient.cs`:

```csharp
public class WorkflowClient : IWorkflowClient
{
    private readonly WorkflowService.WorkflowServiceClient _client;

    public async Task<User> GetUserByIdAsync(int userId)
    {
        var response = await _client.GetUserByIdAsync(new GetUserByIdRequest { UserId = userId });
        return response.User;
    }
}
```

### Service Configuration Order

The `AF.ECT.Server/Program.cs` configures services in a specific order for proper dependency resolution:

1. **Web Components** - Kestrel, Razor, Blazor Server
2. **Data Access** - EF Core, DbContext factory, repositories
3. **Theme Services** - Radzen UI components
4. **CORS** - Cross-origin resource sharing
5. **gRPC Services** - Core business logic
6. **Health Checks** - Database and service monitoring
7. **Logging** - Structured logging with Serilog
8. **Antiforgery** - CSRF protection
9. **Resilience** - Polly policies (retry, circuit breaker)
10. **Caching** - Redis distributed caching
11. **Rate Limiting** - IP-based throttling
12. **Documentation** - OpenAPI/Swagger
13. **Telemetry** - OpenTelemetry for observability

This order ensures dependencies are available when services need them.

### Strongly-Typed Configuration

All configuration uses validated options classes:

```csharp
public class DatabaseOptions
{
    [Required]
    public string ConnectionString { get; set; } = string.Empty;

    [Range(1, 300)]
    public int CommandTimeout { get; set; } = 30;

    [Range(0, 10)]
    public int MaxRetryCount { get; set; } = 3;
}
```

Registered in `Program.cs`:

```csharp
builder.Services.AddOptions<DatabaseOptions>()
    .BindConfiguration("DatabaseOptions")
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

### Audit Logging

Automatic audit logging with Audit.NET:

```csharp
// EF Core operations are automatically audited
using (var scope = AuditScope.Create("gRPC:GetUserById", 
    () => new { userId, correlationId }))
{
    var user = await _dataService.GetUserByIdAsync(userId);
    scope.Event.CustomFields["Success"] = true;
    return user;
}
```

---

## Testing

### Running Tests

```powershell
# Run all tests
dotnet test

# Run specific test project
dotnet test AF.ECT.Tests/AF.ECT.Tests.csproj

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Writing Tests

Tests use xUnit, FluentAssertions, and Moq:

```csharp
public class DataServiceTests
{
    [Fact]
    public async Task GetUserById_ValidId_ReturnsUser()
    {
        // Arrange
        var contextFactory = CreateMockContextFactory();
        var logger = Mock.Of<ILogger<DataService>>();
        var service = new DataService(contextFactory, logger);

        // Act
        var user = await service.GetUserByIdAsync(1);

        // Assert
        user.Should().NotBeNull();
        user.UserId.Should().Be(1);
    }
}
```

See [CONTRIBUTING.md](CONTRIBUTING.md) for testing requirements and patterns.

---

## Deployment

### Local Development Deployment

Database deployment is handled automatically when running via Aspire, but you can also deploy manually:

```powershell
# Publish database project
sqlpackage /Action:Publish `
  /SourceFile:"AF.ECT.Database\bin\Debug\AF.ECT.Database.dacpac" `
  /TargetServerName:"localhost" `
  /TargetDatabaseName:"ALOD"
```

### Production Deployment

Production deployment uses Azure DevOps pipelines (CI/CD):

1. **Build Pipeline**: Compiles solution, runs tests, creates artifacts
2. **Release Pipeline**: Deploys to Azure App Service
3. **Database Pipeline**: Applies schema changes via DACPAC

See `Documentation/DEPLOYMENT.md` for detailed deployment procedures (if available).

---

## Resources

### Documentation

| Document | Purpose |
|----------|---------|
| [README.md](README.md) | Project overview and getting started |
| [CONTRIBUTING.md](CONTRIBUTING.md) | Contribution guidelines and coding standards |
| [API_DOCUMENTATION.md](API_DOCUMENTATION.md) | API reference and examples |
| [ARCHITECTURE_DIAGRAMS.md](ARCHITECTURE_DIAGRAMS.md) | System architecture diagrams |
| [CHANGELOG.md](CHANGELOG.md) | Version history and release notes |
| [TROUBLESHOOTING.md](TROUBLESHOOTING.md) | Common issues and solutions |

### External Resources

- [.NET 9.0 Documentation](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9)
- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Blazor Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/)
- [gRPC in .NET](https://docs.microsoft.com/en-us/aspnet/core/grpc/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Win UI 3](https://docs.microsoft.com/en-us/windows/apps/winui/)

### Team Communication

- **GitHub Issues**: https://github.com/dmccoy2025/ECTSystem/issues
- **Pull Requests**: https://github.com/dmccoy2025/ECTSystem/pulls
- **Discussions**: https://github.com/dmccoy2025/ECTSystem/discussions

---

## Next Steps

Now that you're set up, here's what to do next:

1. ✅ **Run the application** using .NET Aspire (F5 in Visual Studio)
2. ✅ **Explore the codebase** - start with `AF.ECT.Server/Program.cs` and `AF.ECT.AppHost/AppHost.cs`
3. ✅ **Read architecture docs** - review [ARCHITECTURE_DIAGRAMS.md](ARCHITECTURE_DIAGRAMS.md)
4. ✅ **Pick up a task** - check GitHub Issues for "good first issue" labels
5. ✅ **Ask questions** - don't hesitate to reach out to the team!

Welcome to the team! 🚀

---

**Last Updated:** January 26, 2025  
**Version:** 1.0.1
