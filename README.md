# ECTSystem

Electronic Case Tracking System - A modern web application for managing and tracking electronic cases.

## Overview

ECTSystem is built using .NET 9.0 and leverages ASP.NET Core for the backend, Blazor for the web frontend, Win UI for the desktop frontend, and .NET Aspire for cloud-ready application orchestration. It provides a comprehensive solution for case management, reporting, and workflow automation.

## Features

- **Case Management**: Comprehensive case tracking and workflow management for ALOD operations
- **User Management**: Secure user authentication and role-based access control
- **Workflow Automation**: Automated case processing with gRPC-based services
- **Reporting and Analytics**: Generate detailed reports on case metrics and performance
- **Audit Logging**: End-to-end audit trails with correlation IDs for military-grade compliance
- **Observability**: Integrated OpenTelemetry for logging, monitoring, and health checks
- **Multi-Platform UI**: Modern web interface with Blazor and desktop application with Win UI
- **API Integration**: RESTful APIs with JSON transcoding for external system integration
- **Database Layer**: Robust data access with Entity Framework Core and optimized stored procedures

## Architecture

The solution consists of several projects:

- **AF.ECT.AppHost**: ASP.NET Core app host using .NET Aspire for orchestration
- **AF.ECT.WebClient**: Blazor web application for the user interface
- **AF.ECT.WindowsClient**: Win UI desktop application for the user interface
- **AF.ECT.Server**: ASP.NET Core Web API server
- **AF.ECT.Data**: Data access layer using Entity Framework Core
- **AF.ECT.Database**: SQL Server database project with stored procedures and schemas
- **AF.ECT.Shared**: Shared models, utilities, and protobuf definitions for gRPC contracts
- **AF.ECT.ServiceDefaults**: Common service configurations and extensions
- **AF.ECT.Tests**: Unit and integration tests

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extension
- [Windows App SDK](https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/) (for Win UI desktop app)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Developer Edition)
- Git (for version control)
- [SQL Server Command Line Utilities](https://aka.ms/sqlcmd) (optional, for running tests)

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/dmccoy2025/ECTSystem.git
cd ECTSystem
```

### Build the Solution

```bash
dotnet build ECTSystem.sln
```

### Run the Application

1. Navigate to the AppHost project:
   ```bash
   cd AF.ECT.AppHost
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

The application will start and open in your default browser. The Aspire dashboard will be available for monitoring services.

### API Usage

The ECTSystem provides gRPC services with JSON transcoding, enabling REST-like API calls. When running in development, the API endpoints are accessible via HTTP at the server's base URL (typically `http://localhost:5173/v1/`).

#### Example API Calls

You can test the API using tools like curl, Postman, or the provided `AF.ECT.Server/workflow_service_tests.http` file.

For example, to retrieve online users:

```bash
curl -X GET "http://localhost:5173/v1/users/online" \
     -H "accept: application/json"
```

To get workflow information:

```bash
curl -X GET "http://localhost:5173/v1/workflows/1" \
     -H "accept: application/json"
```

#### gRPC Direct Calls

For direct gRPC communication (bypassing JSON transcoding), use gRPC clients or tools like `grpcurl`. This provides better performance and type safety.

**Prerequisites:** Install [grpcurl](https://github.com/fullstorydev/grpcurl).

**List available services:**
```bash
grpcurl -plaintext localhost:5173 list
```

**Call a gRPC method directly:**
```bash
grpcurl -plaintext -d '{"user_id": 1}' localhost:5173 workflow.WorkflowService/GetUserById
```

**Using .NET gRPC Client:**
```csharp
// In your .NET application
using var channel = GrpcChannel.ForAddress("http://localhost:5173");
var client = new WorkflowService.WorkflowServiceClient(channel);
var response = await client.GetUserByIdAsync(new GetUserByIdRequest { UserId = 1 });
```

The protobuf definitions and generated client code are available in `AF.ECT.Shared/Protos/`.

#### OpenAPI Documentation

In development mode, interactive API documentation is available via Swagger UI at `/swagger` on the server port.

### Development

- Open the solution in Visual Studio or VS Code
- Build and run individual projects as needed
- Use the integrated debugging tools

## Configuration

- Appsettings files are located in each project directory
- Environment-specific settings can be configured in `appsettings.Development.json` and `appsettings.json`
- Configuration uses strongly-typed options classes with data annotation validation
- Invalid configuration values will cause the application to fail on startup with detailed error messages
- Key configuration sections: Database, Cors, Server, WorkflowClientOptions

## Testing

### Unit and Integration Tests

Run .NET tests using:
```bash
dotnet test
```

### Database Testing

#### Quick Pagination Test
Test stored procedure pagination consistency without installing additional frameworks:

```powershell
.\test-pagination-quick.ps1 -DatabaseName "ALOD"
```

This test verifies:
- Consistent results across multiple query executions
- No duplicate records across pages
- Proper OFFSET calculation

#### Comprehensive SQL Tests
Run detailed SQL-based pagination tests:

```powershell
sqlcmd -S localhost -d ALOD -E -i "test-sp-pagination.sql"
```

#### tSQLt Unit Tests (Advanced)

For production-grade SQL Server unit testing with the Redgate tSQLt framework:

**1. Install tSQLt Framework**

Prerequisites are already configured (CLR and TRUSTWORTHY). To complete installation:

```powershell
# Run the installation script
.\install-tsqlt.ps1
```

Follow the prompts to download tSQLt from [https://tsqlt.org/downloads/](https://tsqlt.org/downloads/)

**2. Run tSQLt Tests**

```powershell
.\run-tsqlt-tests.ps1
```

The tSQLt test suite includes 10 comprehensive tests covering:
- Default pagination behavior
- OFFSET/FETCH calculation accuracy
- Process name filtering
- Date range filtering
- Message content filtering
- Multi-column sorting (ASC/DESC)
- Invalid parameter validation
- Empty result set handling
- Consistency and deterministic ordering

**Test Files Location:**
- Test suite: `AF.ECT.Database/dbo/Tests/ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.sql`
- Test runner: `AF.ECT.Database/dbo/Tests/RunPaginationTests.sql`
- Documentation: `AF.ECT.Database/dbo/Tests/README.md`

For detailed tSQLt installation and troubleshooting, see: `tsqlt/INSTALLATION_GUIDE.md`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For questions or issues, please open an issue on GitHub.

## Documentation

For comprehensive documentation, see:

- **[DEVELOPER_ONBOARDING.md](DEVELOPER_ONBOARDING.md)** - Complete setup guide for new developers
- **[API_DOCUMENTATION.md](API_DOCUMENTATION.md)** - REST and gRPC API reference with examples
- **[ARCHITECTURE_DIAGRAMS.md](ARCHITECTURE_DIAGRAMS.md)** - System architecture visualizations
- **[CONTRIBUTING.md](CONTRIBUTING.md)** - Contribution guidelines and coding standards
- **[TROUBLESHOOTING.md](TROUBLESHOOTING.md)** - Solutions to common development issues
- **[CHANGELOG.md](CHANGELOG.md)** - Version history and release notes