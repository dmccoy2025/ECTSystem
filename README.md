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
- Git (for version control)

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

Run tests using:
```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For questions or issues, please open an issue on GitHub.