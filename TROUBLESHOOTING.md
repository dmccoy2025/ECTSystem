# ECTSystem Troubleshooting Guide

This guide provides solutions to common issues encountered when developing, deploying, and operating the Electronic Case Tracking System.

## Table of Contents

- [Build and Compilation Issues](#build-and-compilation-issues)
- [Database Connection Problems](#database-connection-problems)
- [gRPC Communication Errors](#grpc-communication-errors)
- [Authentication and Authorization](#authentication-and-authorization)
- [Performance Issues](#performance-issues)
- [Deployment Problems](#deployment-problems)
- [Aspire Dashboard Issues](#aspire-dashboard-issues)
- [Logging and Debugging](#logging-and-debugging)

---

## Build and Compilation Issues

### Problem: `dotnet build` fails with NuGet restore errors

**Symptoms:**
```
error NU1101: Unable to find package Microsoft.EntityFrameworkCore
```

**Solution:**
1. Clear NuGet package cache:
   ```powershell
   dotnet nuget locals all --clear
   ```

2. Restore packages explicitly:
   ```powershell
   dotnet restore ElectronicCaseTracking.sln
   ```

3. Rebuild the solution:
   ```powershell
   dotnet build ElectronicCaseTracking.sln
   ```

### Problem: Build fails with "SDK not found" error

**Symptoms:**
```
error MSB4236: The SDK 'Microsoft.NET.Sdk' specified could not be found.
```

**Solution:**
1. Verify .NET 9.0 SDK is installed:
   ```powershell
   dotnet --list-sdks
   ```

2. If .NET 9.0 is missing, download from: https://dotnet.microsoft.com/download/dotnet/9.0

3. Check `global.json` specifies correct SDK version:
   ```json
   {
     "sdk": {
       "version": "9.0.100",
       "rollForward": "latestMinor"
     }
   }
   ```

### Problem: Protobuf code generation fails

**Symptoms:**
```
error CS0246: The type or namespace name 'WorkflowService' could not be found
```

**Solution:**
1. Clean and rebuild the Shared project first:
   ```powershell
   dotnet clean AF.ECT.Shared/AF.ECT.Shared.csproj
   dotnet build AF.ECT.Shared/AF.ECT.Shared.csproj
   ```

2. Verify `.proto` files exist in `AF.ECT.Shared/Protos/`

3. Check `AF.ECT.Shared.csproj` contains Protobuf item groups:
   ```xml
   <ItemGroup>
     <Protobuf Include="Protos\workflow.proto" GrpcServices="Both" />
   </ItemGroup>
   ```

---

## Database Connection Problems

### Problem: Cannot connect to SQL Server

**Symptoms:**
```
Microsoft.Data.SqlClient.SqlException: A network-related or instance-specific error occurred
```

**Solution:**
1. Verify SQL Server is running:
   ```powershell
   Get-Service -Name MSSQL*
   ```

2. Check connection string in `appsettings.Development.json`:
   ```json
   "DatabaseOptions": {
     "ConnectionString": "Server=localhost;Database=ALOD;Integrated Security=true;TrustServerCertificate=true"
   }
   ```

3. Test connection with sqlcmd:
   ```powershell
   sqlcmd -S localhost -E -Q "SELECT @@VERSION"
   ```

4. Enable TCP/IP protocol in SQL Server Configuration Manager

### Problem: Database timeout errors during startup

**Symptoms:**
```
System.TimeoutException: Timeout expired during database operation
```

**Solution:**
1. Increase command timeout in `appsettings.json`:
   ```json
   "DatabaseOptions": {
     "CommandTimeout": 60,
     "MaxRetryCount": 3,
     "MaxRetryDelay": 30
   }
   ```

2. Check database performance:
   ```sql
   -- Check for blocking queries
   SELECT * FROM sys.dm_exec_requests WHERE blocking_session_id <> 0;
   ```

3. Rebuild database indexes:
   ```sql
   EXEC sp_MSforeachtable 'ALTER INDEX ALL ON ? REBUILD';
   ```

---

## gRPC Communication Errors

### Problem: gRPC calls fail with "Status(StatusCode=Unavailable)"

**Symptoms:**
```
Grpc.Core.RpcException: Status(StatusCode=Unavailable, Detail="Error connecting to service")
```

**Solution:**
1. Verify server is running and listening on the correct port:
   ```powershell
   netstat -ano | findstr :5173
   ```

2. Check `WorkflowClientOptions` in `appsettings.json`:
   ```json
   "WorkflowClientOptions": {
     "ServerUrl": "http://localhost:5173"
   }
   ```

3. Ensure HTTP/2 is enabled in server configuration (Program.cs):
   ```csharp
   builder.WebHost.ConfigureKestrel(options =>
   {
       options.ListenLocalhost(5173, o => o.Protocols = HttpProtocols.Http1AndHttp2);
   });
   ```

### Problem: Browser cannot make gRPC-Web calls

**Symptoms:**
```
TypeError: Failed to fetch
```

**Solution:**
1. Verify gRPC-Web middleware is registered in `Program.cs`:
   ```csharp
   app.UseGrpcWeb();
   app.MapGrpcService<WorkflowServiceImpl>().EnableGrpcWeb();
   ```

2. Check CORS configuration allows gRPC-Web headers:
   ```json
   "CorsOptions": {
     "AllowedOrigins": "http://localhost:5173,https://localhost:7293"
   }
   ```

3. Verify browser DevTools Network tab shows correct headers:
   - `content-type: application/grpc-web`
   - `grpc-accept-encoding: identity,deflate,gzip`

---

## Authentication and Authorization

### Problem: Users cannot access protected pages

**Symptoms:**
```
401 Unauthorized
```

**Solution:**
1. In development, verify dev mode authentication is working (no CAC required yet)

2. Check user exists in database:
   ```sql
   SELECT * FROM dbo.CoreUser WHERE Username = 'testuser';
   ```

3. Verify user has required roles:
   ```sql
   SELECT u.Username, r.RoleName 
   FROM dbo.CoreUser u
   JOIN dbo.CoreUserRole ur ON u.UserId = ur.UserId
   JOIN dbo.CoreLkupRole r ON ur.RoleId = r.RoleId
   WHERE u.Username = 'testuser';
   ```

### Problem: CORS errors when calling API

**Symptoms:**
```
Access to fetch at 'http://localhost:5173' from origin 'http://localhost:5000' has been blocked by CORS policy
```

**Solution:**
1. Add client origin to `CorsOptions` in `appsettings.json`:
   ```json
   "CorsOptions": {
     "AllowedOrigins": "http://localhost:5000,http://localhost:5173"
   }
   ```

2. Restart the server after configuration changes

---

## Performance Issues

### Problem: Slow API response times

**Symptoms:**
- API calls take >5 seconds to complete
- High CPU usage on server

**Solution:**
1. Enable SQL query logging to identify slow queries:
   ```json
   "Logging": {
     "LogLevel": {
       "Microsoft.EntityFrameworkCore.Database.Command": "Information"
     }
   }
   ```

2. Check for N+1 query problems - use `Include()` for related entities:
   ```csharp
   var users = await context.CoreUsers
       .Include(u => u.CoreUserRoles)
       .Include(u => u.Cs)
       .ToListAsync();
   ```

3. Add database indexes for frequently queried columns

4. Enable response compression in `Program.cs`:
   ```csharp
   builder.Services.AddResponseCompression();
   ```

### Problem: High memory usage

**Symptoms:**
- Application memory grows over time
- OutOfMemoryException errors

**Solution:**
1. Ensure DbContext instances are properly disposed:
   ```csharp
   await using var context = await contextFactory.CreateDbContextAsync();
   ```

2. Use pagination for large datasets:
   ```csharp
   var logs = await GetAllLogsPaginationAsync(pageNumber: 1, pageSize: 100);
   ```

3. Configure EF Core tracking behavior:
   ```csharp
   query.AsNoTracking()  // For read-only queries
   ```

---

## Deployment Problems

### Problem: Application fails to start in production

**Symptoms:**
```
Application startup exception
```

**Solution:**
1. Check application logs for detailed error messages

2. Verify all required environment variables are set:
   - `ASPNETCORE_ENVIRONMENT=Production`
   - `ConnectionStrings__DefaultConnection=...`

3. Ensure `appsettings.Production.json` exists and contains correct configuration

4. Validate configuration on startup - check for validation errors:
   ```
Invalid configuration: DatabaseOptions.ConnectionString is required
   ```

### Problem: Health checks fail in Kubernetes

**Symptoms:**
- Pod restarts frequently
- Readiness probe failures

**Solution:**
1. Verify health check endpoints are accessible:
   ```powershell
   curl http://localhost:5173/health
   curl http://localhost:5173/alive
   ```

2. Increase probe timeout in Kubernetes manifest:
   ```yaml
   livenessProbe:
     httpGet:
       path: /alive
       port: 5173
     initialDelaySeconds: 30
     timeoutSeconds: 5
   ```

3. Add database health check to detect connection issues early:
   ```csharp
   builder.Services.AddHealthChecks()
       .AddSqlServer(connectionString, name: "database");
   ```

---

## Aspire Dashboard Issues

### Problem: Aspire dashboard doesn't show services

**Symptoms:**
- Dashboard opens at http://localhost:15888 but shows no services
- Services are running but not visible

**Solution:**
1. Verify `AF.ECT.AppHost` is the startup project

2. Check `AppHost.cs` has service registrations:
   ```csharp
   builder.AddProject<Projects.AF_ECT_Server>("server");
   builder.AddProject<Projects.AF_ECT_WebClient>("webclient");
   ```

3. Restart the AppHost project:
   ```powershell
   cd AF.ECT.AppHost
   dotnet run
   ```

### Problem: OpenTelemetry data not appearing in dashboard

**Symptoms:**
- Traces and metrics not visible in Aspire dashboard

**Solution:**
1. Verify `OTEL_EXPORTER_OTLP_ENDPOINT` is set (automatically set by Aspire)

2. Check service has `AddServiceDefaults()` called:
   ```csharp
   var builder = WebApplication.CreateBuilder(args);
   builder.AddServiceDefaults();  // Required for OpenTelemetry
   ```

3. Ensure OpenTelemetry instrumentation is enabled in `ServiceDefaults`

---

## Logging and Debugging

### Problem: Logs not appearing in output

**Symptoms:**
- No log messages in console or files
- Unable to diagnose issues

**Solution:**
1. Check log level in `appsettings.Development.json`:
   ```json
   "Logging": {
     "LogLevel": {
       "Default": "Information",
       "Microsoft.AspNetCore": "Warning",
       "AF.ECT": "Debug"
     }
   }
   ```

2. Verify Serilog is configured (if using):
   ```csharp
   Log.Logger = new LoggerConfiguration()
       .ReadFrom.Configuration(builder.Configuration)
       .CreateLogger();
   ```

3. Enable structured logging for better diagnostics:
   ```csharp
   _logger.LogInformation("Processing request for user {UserId} at {Time}", userId, DateTime.UtcNow);
   ```

### Problem: Cannot attach debugger to running process

**Symptoms:**
- Debugger shows "Unable to attach"
- Breakpoints not hit

**Solution:**
1. Ensure you're running in Debug configuration:
   ```powershell
   dotnet run --configuration Debug
   ```

2. In Visual Studio, use Debug → Attach to Process (Ctrl+Alt+P)

3. For Blazor WebAssembly debugging, enable browser debugging:
   - Chrome: `chrome://inspect`
   - Edge: `edge://inspect`

---

## Getting Help

If these solutions don't resolve your issue:

1. **Check Logs**: Review detailed logs in the Aspire dashboard or application output
2. **Search Documentation**: See `API_DOCUMENTATION.md`, `ARCHITECTURE_DIAGRAMS.md`, `CONTRIBUTING.md`
3. **GitHub Issues**: Open an issue at https://github.com/dmccoy2025/ECTSystem/issues
4. **Stack Traces**: Include full exception stack traces when reporting issues

### Useful Diagnostic Commands

```powershell
# Check .NET SDK version
dotnet --info

# List all running processes
Get-Process dotnet

# Check port usage
netstat -ano | findstr :5173

# Test database connectivity
sqlcmd -S localhost -E -Q "SELECT 1"

# View application logs
Get-Content ./logs/app-*.log -Tail 50

# Check NuGet package versions
dotnet list package --outdated
```

---

**Last Updated:** October 26, 2025  
**Version:** 1.0.0
