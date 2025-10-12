# ECTSystem

Electronic Case Tracking System - A modern web application for managing and tracking electronic cases.

## Overview

ECTSystem is built using .NET 9.0 and leverages ASP.NET Core for the backend, Blazor for the frontend, and .NET Aspire for cloud-ready application orchestration. It provides a comprehensive solution for case management, reporting, and workflow automation.

## Features

- **Case Management**: Create, update, and track electronic cases
- **Workflow Integration**: Automated workflows for case processing
- **Reporting**: Generate reports on case statuses and metrics
- **User Interface**: Modern web UI built with Blazor
- **API Services**: RESTful APIs for integration with other systems

## Architecture

The solution consists of several projects:

- **AF.ECT.AppHost**: ASP.NET Core app host using .NET Aspire for orchestration
- **AF.ECT.Client**: Blazor web application for the user interface
- **AF.ECT.Server**: ASP.NET Core Web API server
- **AF.ECT.Shared**: Shared models and utilities
- **AF.ECT.ServiceDefaults**: Common service configurations and extensions

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extension
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

### Development

- Open the solution in Visual Studio or VS Code
- Build and run individual projects as needed
- Use the integrated debugging tools

## Configuration

- Appsettings files are located in each project directory
- Environment-specific settings can be configured in `appsettings.Development.json` and `appsettings.json`

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