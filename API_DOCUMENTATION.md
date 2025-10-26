# ECTSystem API Documentation

## Table of Contents

- [Overview](#overview)
- [Authentication](#authentication)
- [API Endpoints](#api-endpoints)
- [gRPC Services](#grpc-services)
- [Common Patterns](#common-patterns)
- [Error Handling](#error-handling)
- [Rate Limiting](#rate-limiting)
- [Examples](#examples)

## Overview

ECTSystem provides two primary API interfaces:

1. **gRPC API** - High-performance binary protocol for service-to-service communication
2. **RESTful JSON API** - HTTP/JSON endpoints via gRPC JSON Transcoding for web clients

Both APIs expose the same underlying functionality through the `WorkflowService`.

### Base URLs

**Development:**
- gRPC: `http://localhost:5173`
- REST: `http://localhost:5173/v1/`
- Swagger UI: `http://localhost:5173/swagger`

**Production:**
- gRPC: `https://api.ectsystem.mil`
- REST: `https://api.ectsystem.mil/v1/`

## Authentication

### Current Implementation

**Development Mode**: No authentication required (for testing purposes)

### Planned Authentication

**Production**: Will implement:
- CAC (Common Access Card) authentication
- OAuth 2.0 / OpenID Connect
- JWT bearer tokens
- Role-based access control (RBAC)

### Future Authentication Header

```http
Authorization: Bearer <JWT_TOKEN>
```

## API Endpoints

### Base Endpoint Structure

All REST endpoints follow this pattern:
```
https://api.ectsystem.mil/v1/{resource}/{action}
```

### User Management

#### Get Users Online

Retrieves all currently active users.

**REST Endpoint:**
```http
GET /v1/users/online
```

**gRPC Method:**
```protobuf
rpc GetUsersOnline(EmptyRequest) returns (GetUsersOnlineResponse);
```

**Response:**
```json
{
  "items": [
    {
      "userId": 1,
      "name": "John Doe",
      "sessionId": "abc123",
      "lastActivity": "2025-10-26T10:30:00Z"
    }
  ],
  "count": 1
}
```

**Example:**
```bash
curl -X GET "http://localhost:5173/v1/users/online" \
     -H "accept: application/json"
```

#### Get User by ID

Retrieves detailed information for a specific user.

**REST Endpoint:**
```http
GET /v1/users/{userId}
```

**gRPC Method:**
```protobuf
rpc GetUserById(GetUserByIdRequest) returns (GetUserByIdResponse);
```

**Parameters:**
- `userId` (int32, required) - The unique identifier of the user

**Response:**
```json
{
  "userId": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@mil",
  "rank": "SSgt",
  "unit": "123rd Wing",
  "role": "Administrator",
  "status": "Active"
}
```

**Example:**
```bash
curl -X GET "http://localhost:5173/v1/users/1" \
     -H "accept: application/json"
```

#### Search Member Data

Searches for service members based on various criteria.

**REST Endpoint:**
```http
POST /v1/members/search
```

**gRPC Method:**
```protobuf
rpc SearchMemberData(SearchMemberDataRequest) returns (SearchMemberDataResponse);
```

**Request Body:**
```json
{
  "userId": 1,
  "ssn": "123-45-6789",
  "lastName": "Doe",
  "firstName": "John",
  "middleName": "A",
  "srchUnit": 100,
  "rptView": 1
}
```

**Response:**
```json
{
  "items": [
    {
      "memberId": 101,
      "ssn": "123-45-6789",
      "fullName": "Doe, John A.",
      "rank": "SSgt",
      "unit": "123rd Wing",
      "status": "Active"
    }
  ],
  "count": 1
}
```

**Example:**
```bash
curl -X POST "http://localhost:5173/v1/members/search" \
     -H "Content-Type: application/json" \
     -H "accept: application/json" \
     -d '{
       "userId": 1,
       "lastName": "Doe",
       "firstName": "John"
     }'
```

### Workflow Management

#### Get Workflow by ID

Retrieves a specific workflow instance.

**REST Endpoint:**
```http
GET /v1/workflows/{workflowId}
```

**gRPC Method:**
```protobuf
rpc GetWorkflowById(GetWorkflowByIdRequest) returns (GetWorkflowByIdResponse);
```

**Parameters:**
- `workflowId` (int32, required) - The unique identifier of the workflow

**Response:**
```json
{
  "workflowId": 1,
  "name": "LOD Investigation",
  "status": "In Progress",
  "createdDate": "2025-10-01T08:00:00Z",
  "assignedTo": "John Doe",
  "priority": "High"
}
```

**Example:**
```bash
curl -X GET "http://localhost:5173/v1/workflows/1" \
     -H "accept: application/json"
```

#### Get Active Cases

Retrieves all active workflow cases for a specific reference and group.

**REST Endpoint:**
```http
GET /v1/workflows/cases/active?refId={refId}&groupId={groupId}
```

**gRPC Method:**
```protobuf
rpc GetActiveCases(GetActiveCasesRequest) returns (GetActiveCasesResponse);
```

**Parameters:**
- `refId` (int32, required) - Reference identifier
- `groupId` (int32, required) - Group identifier

**Response:**
```json
{
  "items": [
    {
      "caseId": 201,
      "caseNumber": "LOD-2025-001",
      "status": "Under Review",
      "memberName": "Doe, John",
      "dateOpened": "2025-10-15T10:00:00Z"
    }
  ],
  "count": 1
}
```

**Example:**
```bash
curl -X GET "http://localhost:5173/v1/workflows/cases/active?refId=1&groupId=5" \
     -H "accept: application/json"
```

#### Add Signature to Workflow

Adds a digital signature to a workflow step.

**REST Endpoint:**
```http
POST /v1/workflows/signatures
```

**gRPC Method:**
```protobuf
rpc AddSignature(AddSignatureRequest) returns (AddSignatureResponse);
```

**Request Body:**
```json
{
  "refId": 1,
  "moduleType": 2,
  "userId": 100,
  "actionId": 5,
  "groupId": 10,
  "statusIn": 1,
  "statusOut": 2
}
```

**Response:**
```json
{
  "success": true,
  "signatureId": 501,
  "timestamp": "2025-10-26T14:30:00Z"
}
```

**Example:**
```bash
curl -X POST "http://localhost:5173/v1/workflows/signatures" \
     -H "Content-Type: application/json" \
     -H "accept: application/json" \
     -d '{
       "refId": 1,
       "moduleType": 2,
       "userId": 100,
       "actionId": 5,
       "groupId": 10,
       "statusIn": 1,
       "statusOut": 2
     }'
```

### Reinvestigation Requests

#### Get Reinvestigation Requests

Retrieves reinvestigation requests with optional filtering.

**REST Endpoint:**
```http
GET /v1/reinvestigations?userId={userId}&sarc={sarc}
```

**gRPC Method:**
```protobuf
rpc GetReinvestigationRequests(GetReinvestigationRequestsRequest) returns (GetReinvestigationRequestsResponse);
```

**Parameters:**
- `userId` (int32, optional) - Filter by user ID
- `sarc` (bool, optional) - Filter by SARC flag

**Response:**
```json
{
  "items": [
    {
      "id": 301,
      "description": "Doe, John - LOD-2025-001 (Pending)"
    }
  ],
  "count": 1
}
```

**Example:**
```bash
curl -X GET "http://localhost:5173/v1/reinvestigations?userId=1&sarc=true" \
     -H "accept: application/json"
```

## gRPC Services

### Direct gRPC Usage

For high-performance, strongly-typed communication, use gRPC directly.

#### Prerequisites

Install [grpcurl](https://github.com/fullstorydev/grpcurl) for testing:

```bash
# Windows (via Chocolatey)
choco install grpcurl

# macOS (via Homebrew)
brew install grpcurl

# Linux
go install github.com/fullstorydev/grpcurl/cmd/grpcurl@latest
```

#### List Available Services

```bash
grpcurl -plaintext localhost:5173 list
```

**Output:**
```
grpc.reflection.v1alpha.ServerReflection
workflow.WorkflowService
```

#### List Service Methods

```bash
grpcurl -plaintext localhost:5173 list workflow.WorkflowService
```

**Output:**
```
workflow.WorkflowService.GetUsersOnline
workflow.WorkflowService.GetUserById
workflow.WorkflowService.SearchMemberData
workflow.WorkflowService.GetWorkflowById
...
```

#### Call a gRPC Method

```bash
grpcurl -plaintext -d '{"user_id": 1}' \
    localhost:5173 \
    workflow.WorkflowService/GetUserById
```

### Using .NET gRPC Client

```csharp
using Grpc.Net.Client;
using AF.ECT.Shared.Services;

// Create channel
using var channel = GrpcChannel.ForAddress("http://localhost:5173");

// Create client
var client = new WorkflowService.WorkflowServiceClient(channel);

// Make request
var response = await client.GetUserByIdAsync(new GetUserByIdRequest 
{ 
    UserId = 1 
});

Console.WriteLine($"User: {response.FirstName} {response.LastName}");
```

### Using WorkflowClient (Recommended)

The `WorkflowClient` wrapper provides retry logic, logging, and audit trails:

```csharp
using AF.ECT.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

// Configure in Startup/Program.cs
services.AddGrpcClient<WorkflowService.WorkflowServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5173");
})
.AddStandardResilienceHandler();

services.AddScoped<IWorkflowClient, WorkflowClient>();

// Use in your service
public class MyService
{
    private readonly IWorkflowClient _workflowClient;

    public MyService(IWorkflowClient workflowClient)
    {
        _workflowClient = workflowClient;
    }

    public async Task<GetUserByIdResponse> GetUser(int userId)
    {
        return await _workflowClient.GetUserByIdAsync(userId);
    }
}
```

## Common Patterns

### Pagination

Currently not implemented. All list endpoints return complete result sets.

**Planned Enhancement:**
```json
{
  "items": [...],
  "count": 100,
  "page": 1,
  "pageSize": 20,
  "totalPages": 5
}
```

### Filtering

Use query parameters for filtering:

```http
GET /v1/users/online?status=active&role=admin
```

### Sorting

Planned enhancement:

```http
GET /v1/workflows?sortBy=createdDate&sortOrder=desc
```

### Streaming Responses

For large datasets, use streaming endpoints (gRPC only):

```csharp
using var call = client.GetUsersOnlineStream(new EmptyRequest());

await foreach (var user in call.ResponseStream.ReadAllAsync())
{
    Console.WriteLine($"User: {user.Name}");
}
```

## Error Handling

### HTTP Status Codes

| Code | Description | When It Occurs |
|------|-------------|----------------|
| 200 | OK | Successful request |
| 400 | Bad Request | Invalid input parameters |
| 401 | Unauthorized | Authentication required (future) |
| 403 | Forbidden | Insufficient permissions (future) |
| 404 | Not Found | Resource does not exist |
| 500 | Internal Server Error | Unexpected server error |
| 503 | Service Unavailable | Service temporarily down |

### gRPC Status Codes

| Code | Description | HTTP Equivalent |
|------|-------------|-----------------|
| OK | Success | 200 |
| CANCELLED | Request cancelled | 499 |
| INVALID_ARGUMENT | Invalid parameters | 400 |
| NOT_FOUND | Resource not found | 404 |
| PERMISSION_DENIED | Access denied | 403 |
| UNAUTHENTICATED | Not authenticated | 401 |
| INTERNAL | Server error | 500 |
| UNAVAILABLE | Service unavailable | 503 |

### Error Response Format

**REST API:**
```json
{
  "error": {
    "code": "INVALID_ARGUMENT",
    "message": "User ID must be greater than zero",
    "details": {
      "field": "userId",
      "value": "0"
    }
  }
}
```

**gRPC:**
```json
{
  "code": 3,
  "message": "User ID must be greater than zero",
  "details": []
}
```

### Error Handling Best Practices

**Client-Side:**
```csharp
try
{
    var user = await _workflowClient.GetUserByIdAsync(userId);
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
{
    // Handle not found
    Console.WriteLine("User not found");
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
{
    // Handle invalid input
    Console.WriteLine($"Invalid input: {ex.Message}");
}
catch (RpcException ex)
{
    // Handle other gRPC errors
    Console.WriteLine($"Error: {ex.Status}");
}
```

## Rate Limiting

### Current Configuration

Rate limiting is implemented using AspNetCoreRateLimit:

- **Per IP**: 100 requests per minute
- **Per Endpoint**: Varies by endpoint criticality
- **Global**: 1000 requests per minute

### Rate Limit Headers

```http
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1635264000
```

### Rate Limit Exceeded Response

```json
{
  "error": {
    "code": "RATE_LIMIT_EXCEEDED",
    "message": "Rate limit exceeded. Try again in 30 seconds.",
    "retryAfter": 30
  }
}
```

## Examples

### Complete Workflow Example

This example demonstrates a complete workflow: searching for a member, retrieving their data, and updating a case.

#### 1. Search for Member

```bash
curl -X POST "http://localhost:5173/v1/members/search" \
     -H "Content-Type: application/json" \
     -d '{
       "userId": 1,
       "lastName": "Doe",
       "firstName": "John"
     }'
```

**Response:**
```json
{
  "items": [
    {
      "memberId": 101,
      "ssn": "123-45-6789",
      "fullName": "Doe, John A."
    }
  ]
}
```

#### 2. Get Active Cases for Member

```bash
curl -X GET "http://localhost:5173/v1/workflows/cases/active?refId=101&groupId=5"
```

**Response:**
```json
{
  "items": [
    {
      "caseId": 201,
      "caseNumber": "LOD-2025-001",
      "status": "Under Review"
    }
  ]
}
```

#### 3. Add Signature to Case

```bash
curl -X POST "http://localhost:5173/v1/workflows/signatures" \
     -H "Content-Type: application/json" \
     -d '{
       "refId": 201,
       "moduleType": 2,
       "userId": 1,
       "actionId": 5,
       "groupId": 5,
       "statusIn": 1,
       "statusOut": 2
     }'
```

**Response:**
```json
{
  "success": true,
  "signatureId": 501
}
```

### Using PowerShell

```powershell
# Get users online
$response = Invoke-RestMethod -Uri "http://localhost:5173/v1/users/online" -Method Get
$response.items | Format-Table

# Search for member
$body = @{
    userId = 1
    lastName = "Doe"
    firstName = "John"
} | ConvertTo-Json

$members = Invoke-RestMethod -Uri "http://localhost:5173/v1/members/search" `
                               -Method Post `
                               -Body $body `
                               -ContentType "application/json"
$members.items | Format-Table
```

### Using JavaScript/TypeScript

```javascript
// Fetch users online
const response = await fetch('http://localhost:5173/v1/users/online');
const data = await response.json();
console.log(data.items);

// Search for member
const searchResponse = await fetch('http://localhost:5173/v1/members/search', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    userId: 1,
    lastName: 'Doe',
    firstName: 'John'
  })
});
const members = await searchResponse.json();
console.log(members.items);
```

## OpenAPI / Swagger

### Accessing Swagger UI

In development mode, Swagger UI is available at:

```
http://localhost:5173/swagger
```

### Downloading OpenAPI Specification

```bash
curl -o openapi.json http://localhost:5173/openapi/v1.json
```

### Using with Postman

1. Import OpenAPI spec into Postman
2. Set base URL to `http://localhost:5173`
3. Test endpoints interactively

## Additional Resources

- [gRPC Documentation](https://grpc.io/docs/)
- [Protocol Buffers Guide](https://developers.google.com/protocol-buffers)
- [.NET gRPC Client](https://docs.microsoft.com/en-us/aspnet/core/grpc/client)
- [gRPC-Web Protocol](https://github.com/grpc/grpc-web)

## Support

For API-related questions or issues:

1. Check the [README.md](README.md)
2. Review [CONTRIBUTING.md](CONTRIBUTING.md)
3. Check existing GitHub issues
4. Create a new issue with the "api" label

---

*Last Updated: October 26, 2025*
*API Version: v1*
