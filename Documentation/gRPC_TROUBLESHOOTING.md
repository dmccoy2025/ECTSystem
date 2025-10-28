# gRPC Troubleshooting Guide for ECTSystem

This guide covers common gRPC connectivity issues between the Blazor WebAssembly client and the ASP.NET Core server.

## Common Issue: "TypeError: Failed to fetch" in Blazor WASM

### Symptoms

```
Grpc.Core.RpcException: Status(StatusCode="Internal", Detail="Error starting gRPC call. HttpRequestException: TypeError: Failed to fetch")
 ---> System.Net.Http.HttpRequestException: TypeError: Failed to fetch
```

The client retries 3 times with exponential backoff but all attempts fail.

### Root Causes

This error typically occurs due to one of the following:

#### 1. CORS Configuration Mismatch ? Most Common

**Problem:** The server's CORS policy doesn't include the WebClient's origin.

**How to Identify:**
- Check browser DevTools Console for CORS errors
- Look for "Access-Control-Allow-Origin" errors
- Server running on different port than allowed in CORS

**Solution:**

Update `AF.ECT.Server/appsettings.Development.json`:

```json
{
  "CorsOptions": {
    "AllowedOrigins": [
      "https://localhost:7217",  // WebClient HTTPS
 "http://localhost:5280",   // WebClient HTTP
      "https://localhost:7000",  // Server HTTPS (for self-calls)
   "http://localhost:5173"    // Server HTTP
    ]
  }
}
```

**Verification:**
1. Restart both Server and WebClient
2. Check Server logs for CORS debug messages (enable with `"Microsoft.AspNetCore.Cors": "Debug"`)
3. Verify browser DevTools Network tab shows successful preflight OPTIONS requests

#### 2. gRPC-Web Not Enabled

**Problem:** Server not configured to handle gRPC-Web protocol required by browsers.

**How to Identify:**
- Check if `.UseGrpcWeb()` is in server middleware pipeline
- Look for `EnableGrpcWeb()` on service mappings

**Solution:**

Verify `AF.ECT.Server/Program.cs` has:

```csharp
// Middleware pipeline
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

// Service mapping
app.MapGrpcService<WorkflowServiceImpl>().EnableGrpcWeb();
```

#### 3. SSL Certificate Trust Issues

**Problem:** Browser doesn't trust self-signed development certificate.

**How to Identify:**
- Browser shows "Not Secure" or certificate warnings
- Network tab shows "ERR_CERT_AUTHORITY_INVALID"

**Solution:**

Trust the .NET development certificate:

```powershell
dotnet dev-certs https --trust
```

Restart browser after trusting the certificate.

#### 4. Wrong Server URL in Client Configuration

**Problem:** WebClient is configured to connect to incorrect server address.

**How to Identify:**
- Check `AF.ECT.WebClient/appsettings.json` `ServerUrl`
- Compare with actual server listening ports in startup logs

**Solution:**

Update `AF.ECT.WebClient/appsettings.json`:

```json
{
  "Server": {
    "ServerUrl": "https://localhost:7000"  // Must match Server's HTTPS port
  }
}
```

Or for HTTP (not recommended for production):

```json
{
  "Server": {
    "ServerUrl": "http://localhost:5173"
  }
}
```

#### 5. Firewall or Antivirus Blocking

**Problem:** Firewall or antivirus software blocking local network connections.

**How to Identify:**
- Works on one machine but not another
- Firewall logs show blocked connections to localhost ports

**Solution:**

Add firewall rules for development:

```powershell
# Allow Server (adjust ports if needed)
netsh advfirewall firewall add rule name="ECT Server HTTPS" dir=in action=allow protocol=TCP localport=7000
netsh advfirewall firewall add rule name="ECT Server HTTP" dir=in action=allow protocol=TCP localport=5173

# Allow WebClient
netsh advfirewall firewall add rule name="ECT WebClient HTTPS" dir=in action=allow protocol=TCP localport=7217
netsh advfirewall firewall add rule name="ECT WebClient HTTP" dir=in action=allow protocol=TCP localport=5280
```

Or temporarily disable firewall for development (not recommended for production).

#### 6. Middleware Pipeline Order

**Problem:** Middleware configured in wrong order, preventing gRPC-Web from working.

**How to Identify:**
- CORS errors despite correct configuration
- gRPC requests timing out

**Solution:**

Ensure correct middleware order in `AF.ECT.Server/Program.cs`:

```csharp
app.UseHttpsRedirection();         // 1. HTTPS first
app.UseCors();     // 2. CORS early for preflight
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });  // 3. gRPC-Web
app.UseRouting();     // 4. Routing
app.UseAntiforgery();       // 5. Antiforgery
app.UseIpRateLimiting();       // 6. Rate limiting
app.MapHealthChecks("/healthz");   // 7. Health checks
app.MapGrpcService<WorkflowServiceImpl>().EnableGrpcWeb();  // 8. Services
```

#### 7. HTTP/2 vs HTTP/1.1 Protocol Mismatch

**Problem:** Blazor WASM requires HTTP/1.1 for gRPC-Web, but server is HTTP/2 only.

**How to Identify:**
- Check Kestrel logs for "HTTP/2 is not enabled" warnings
- Browser DevTools shows protocol negotiation failures

**Solution:**

Ensure `AF.ECT.Server/Extensions/ServiceCollectionExtensions.cs` configures both protocols:

```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });
});
```

---

## Diagnostic Checklist

Use this checklist to systematically diagnose gRPC connectivity issues:

- [ ] **Server is running** - Verify in Task Manager or `dotnet run` output
- [ ] **WebClient is running** - Check browser tab or VS debug output
- [ ] **Correct ports** - Compare startup logs with configuration files
- [ ] **CORS includes WebClient origin** - Check appsettings.Development.json
- [ ] **gRPC-Web enabled** - Verify `.UseGrpcWeb()` and `.EnableGrpcWeb()`
- [ ] **SSL certificate trusted** - Run `dotnet dev-certs https --trust`
- [ ] **Browser DevTools open** - Check Console and Network tabs for errors
- [ ] **Middleware order correct** - CORS before gRPC-Web, both before Routing
- [ ] **Firewall not blocking** - Temporarily disable or add exceptions
- [ ] **No proxy interference** - Disable VPN or corporate proxy if applicable

---

## Debugging Techniques

### Enable Detailed Logging

**Server** (`appsettings.Development.json`):

```json
{
  "Logging": {
    "LogLevel": {
 "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
    "Grpc": "Debug",
      "Microsoft.AspNetCore.Cors": "Debug",
      "Microsoft.AspNetCore.Routing": "Debug"
    }
  }
}
```

**Client** - Add to `Program.cs`:

```csharp
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddFilter("System.Net.Http", LogLevel.Debug);
builder.Logging.AddFilter("Grpc", LogLevel.Debug);
```

### Use Browser DevTools

1. **Open DevTools** (F12)
2. **Console Tab** - Check for JavaScript errors and CORS messages
3. **Network Tab** - Inspect HTTP requests:
   - Look for failed requests (red)
   - Check Response Headers for CORS headers
   - Verify Request Headers include `Content-Type: application/grpc-web+proto`

### Test with curl

Test server endpoint directly:

```bash
curl -X POST https://localhost:7000/workflow.WorkflowService/SayHello \
  -H "Content-Type: application/grpc-web+proto" \
  -H "Origin: https://localhost:7217" \
  --insecure
```

Should return CORS headers:

```
Access-Control-Allow-Origin: https://localhost:7217
Access-Control-Allow-Methods: POST, GET, OPTIONS
```

### Use grpcurl (Server-side gRPC)

Test native gRPC (bypasses gRPC-Web):

```bash
grpcurl -plaintext -d '{"name": "Test"}' localhost:5173 workflow.WorkflowService.SayHello
```

If this works but browser doesn't, issue is gRPC-Web or CORS, not the service itself.

---

## Quick Fixes

### Restart Everything

Sometimes a clean restart resolves issues:

```powershell
# Stop all processes
# In Visual Studio: Stop Debugging (Shift+F5)

# Clean solution
dotnet clean ElectronicCaseTracking.sln

# Rebuild
dotnet build ElectronicCaseTracking.sln

# Restart from Aspire
# Set AF.ECT.AppHost as startup project
# Press F5
```

### Reset Development Certificates

If SSL issues persist:

```powershell
# Remove existing certificates
dotnet dev-certs https --clean

# Create new certificate
dotnet dev-certs https --trust

# Restart browser completely (close all windows)
```

### Clear Browser Cache

Cached CORS preflight responses can cause issues:

1. Open DevTools (F12)
2. Right-click Refresh button
3. Select "Empty Cache and Hard Reload"

Or use Incognito/Private browsing mode to bypass cache.

---

## Configuration Templates

### Minimal Working Configuration

**Server appsettings.Development.json:**

```json
{
  "ConnectionStrings": {
    "ALODConnection": "Server=localhost;Database=ALOD;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "CorsOptions": {
    "AllowedOrigins": [
      "https://localhost:7217",
 "http://localhost:5280"
    ]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Grpc": "Debug",
      "Microsoft.AspNetCore.Cors": "Debug"
    }
  }
}
```

**Client appsettings.json:**

```json
{
  "WorkflowClientOptions": {
    "MaxRetryAttempts": 3,
    "InitialRetryDelayMs": 200,
    "MaxRetryDelayMs": 1000,
    "RequestTimeoutSeconds": 30
  },
  "Server": {
    "ServerUrl": "https://localhost:7000"
  }
}
```

---

## Performance Optimization

Once connectivity is working, optimize for performance:

### Connection Pooling

WorkflowClient already uses a single GrpcChannel instance per service lifetime (Scoped).

### Retry Policy Tuning

Adjust in `appsettings.json`:

```json
{
  "WorkflowClientOptions": {
    "MaxRetryAttempts": 3,      // Increase for unreliable networks
    "InitialRetryDelayMs": 100,   // Decrease for faster retries
    "MaxRetryDelayMs": 1000,      // Increase for longer waits
    "RequestTimeoutSeconds": 30   // Increase for slow operations
  }
}
```

### Message Size Limits

If sending large payloads, increase limits in `GrpcChannelFactory`:

```csharp
public const int DefaultMaxReceiveMessageSize = 100 * 1024 * 1024; // 100 MB
public const int DefaultMaxSendMessageSize = 100 * 1024 * 1024;     // 100 MB
```

---

## Still Having Issues?

If you've tried all the above and still experiencing problems:

1. **Check GitHub Issues:** https://github.com/dmccoy2025/ECTSystem/issues
2. **Create New Issue:** Include:
   - Error messages from both client and server logs
   - Browser DevTools Console output
 - Browser DevTools Network tab screenshots
   - Configuration files (appsettings.json, appsettings.Development.json)
   - Steps to reproduce
3. **Contact Team:** Reach out on team communication channels

---

## Additional Resources

- [gRPC for .NET Documentation](https://docs.microsoft.com/en-us/aspnet/core/grpc/)
- [gRPC-Web in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/grpc/browser)
- [CORS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/cors)
- [Blazor WebAssembly Hosting](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models)

---

**Last Updated:** January 26, 2025  
**Version:** 1.0.0
