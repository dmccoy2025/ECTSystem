# ECTSystem Architecture Diagrams

This document contains visual architecture diagrams for the Electronic Case Tracking System.

## Table of Contents
- [System Architecture Overview](#system-architecture-overview)
- [gRPC Communication Flow](#grpc-communication-flow)
- [Database Schema Relationships](#database-schema-relationships)
- [Deployment Architecture](#deployment-architecture)
- [Authentication Flow](#authentication-flow)
- [Request Processing Pipeline](#request-processing-pipeline)

---

## System Architecture Overview

```mermaid
graph TB
    subgraph "Client Layer"
        WebClient[Blazor WebAssembly<br/>AF.ECT.WebClient]
        WinClient[Win UI Desktop<br/>AF.ECT.WindowsClient]
        MobileClient[.NET MAUI Mobile<br/>AF.ECT.MobileClient]
    end
    
    subgraph "Aspire Orchestration"
        AppHost[.NET Aspire AppHost<br/>Service Discovery & Monitoring]
        Dashboard[Aspire Dashboard<br/>http://localhost:15888]
    end
    
    subgraph "Server Layer"
        Server[ASP.NET Core Server<br/>AF.ECT.Server<br/>gRPC + REST API]
        Wiki[Blazor Server Wiki<br/>AF.ECT.Wiki]
    end
    
    subgraph "Data Layer"
        DataService[Data Service<br/>AF.ECT.Data]
        EFCore[Entity Framework Core]
        Database[(SQL Server<br/>ALOD Database)]
    end
    
    subgraph "Cross-Cutting Concerns"
        Shared[Shared Library<br/>AF.ECT.Shared<br/>Protobuf, Options, Services]
        ServiceDefaults[Service Defaults<br/>OpenTelemetry, Health Checks]
        Audit[Audit.NET<br/>Audit Logging]
    end
    
    WebClient -->|gRPC-Web| Server
    WinClient -->|gRPC| Server
    MobileClient -->|gRPC| Server
    
    Server --> DataService
    DataService --> EFCore
    EFCore --> Database
    
    AppHost -.->|Orchestrates| Server
    AppHost -.->|Orchestrates| WebClient
    AppHost -.->|Orchestrates| Wiki
    AppHost --> Dashboard
    
    Server --> Shared
    WebClient --> Shared
    WinClient --> Shared
    Server --> ServiceDefaults
    Server --> Audit
    
    style WebClient fill:#4CAF50
    style Server fill:#2196F3
    style Database fill:#FF9800
    style AppHost fill:#9C27B0
```

---

## gRPC Communication Flow

```mermaid
sequenceDiagram
    participant Client as Blazor Client
    participant GrpcWeb as gRPC-Web Layer
    participant Server as gRPC Server
    participant DataService as Data Service
    participant EFCore as EF Core
    participant Database as SQL Server
    participant Audit as Audit.NET
    
    Client->>Client: Generate Correlation ID
    Client->>GrpcWeb: HTTP POST /v1/users/1<br/>(JSON or Protobuf)
    
    GrpcWeb->>Server: gRPC GetUserById Request<br/>(HTTP/2)
    
    Note over Server: Rate Limiting Check
    Note over Server: CORS Validation
    Note over Server: Antiforgery Token
    
    Server->>Server: WorkflowServiceImpl
    Server->>DataService: GetUserByIdAsync(1)
    
    DataService->>EFCore: LINQ Query
    EFCore->>Database: SQL: sp_GetUserById @userId=1
    Database-->>EFCore: User Data
    EFCore-->>DataService: User Entity
    
    DataService->>Audit: Log Data Access<br/>(Correlation ID)
    Audit->>Database: Insert Audit Log
    
    DataService-->>Server: User Result
    Server-->>GrpcWeb: gRPC Response
    GrpcWeb-->>Client: HTTP 200 OK<br/>User JSON
    
    Client->>Audit: Log gRPC Call<br/>(Client-side Audit)
    
    Note over Client,Database: End-to-End Tracing via Correlation ID
```

---

## Database Schema Relationships

```mermaid
erDiagram
    Users ||--o{ UserRoles : has
    Users ||--o{ AuditLogs : creates
    Users ||--o{ WorkflowCases : assigned
    
    UserRoles }o--|| Groups : belongs_to
    Groups ||--o{ GroupPermissions : has
    
    WorkflowCases ||--|| Workflows : follows
    WorkflowCases ||--o{ CaseSignatures : requires
    WorkflowCases ||--o{ CaseComments : contains
    WorkflowCases ||--o{ CaseDocuments : attached
    
    Workflows ||--o{ WorkflowSteps : contains
    WorkflowSteps ||--o{ WorkflowActions : defines
    WorkflowSteps ||--o{ StatusCodes : uses
    
    Members ||--o{ LODCases : subject_of
    LODCases ||--o{ Form348SC : has
    LODCases ||--o{ Form348SARC : has
    LODCases ||--o{ PALData : associated
    
    Users {
        int UserId PK
        string FirstName
        string LastName
        string Email
        string Rank
        int Status
        datetime CreatedDate
    }
    
    WorkflowCases {
        int CaseId PK
        int WorkflowId FK
        int AssignedUserId FK
        int StatusId FK
        datetime CreatedDate
        datetime ModifiedDate
    }
    
    Workflows {
        int WorkflowId PK
        string Name
        string Description
        int ModuleType
        bool Active
    }
    
    AuditLogs {
        int AuditId PK
        string EventType
        datetime Timestamp
        string CorrelationId
        int UserId FK
        string Details
    }
```

---

## Deployment Architecture

```mermaid
graph TB
    subgraph "User Access"
        Users[End Users<br/>CAC Authentication]
        Admins[Administrators]
    end
    
    subgraph "Azure Cloud / On-Premises"
        subgraph "Load Balancer"
            LB[Azure Load Balancer<br/>or F5]
        end
        
        subgraph "Web Tier - DMZ"
            Web1[Web Server 1<br/>Blazor WASM + Static Files]
            Web2[Web Server 2<br/>Blazor WASM + Static Files]
        end
        
        subgraph "Application Tier"
            App1[App Server 1<br/>ASP.NET Core + gRPC]
            App2[App Server 2<br/>ASP.NET Core + gRPC]
            Wiki[Wiki Server<br/>Blazor Server]
        end
        
        subgraph "Data Tier"
            Primary[(SQL Server<br/>Primary)]
            Replica[(SQL Server<br/>Read Replica)]
        end
        
        subgraph "Caching Layer"
            Redis[Redis Cache<br/>Distributed Cache]
        end
        
        subgraph "Monitoring"
            AppInsights[Application Insights<br/>Telemetry]
            Dashboard[Aspire Dashboard<br/>Local Dev Only]
        end
    end
    
    Users --> LB
    Admins --> LB
    
    LB --> Web1
    LB --> Web2
    
    Web1 --> App1
    Web1 --> App2
    Web2 --> App1
    Web2 --> App2
    
    App1 --> Primary
    App2 --> Primary
    App1 --> Replica
    App2 --> Replica
    
    App1 --> Redis
    App2 --> Redis
    
    App1 --> AppInsights
    App2 --> AppInsights
    Wiki --> AppInsights
    
    style Users fill:#4CAF50
    style LB fill:#2196F3
    style Primary fill:#FF9800
    style Redis fill:#F44336
    style AppInsights fill:#9C27B0
```

---

## Authentication Flow

```mermaid
sequenceDiagram
    participant User as User Browser
    participant Client as Blazor Client
    participant Server as gRPC Server
    participant CAC as CAC Auth Service<br/>(Future)
    participant DB as Database
    
    Note over User,DB: Phase 1: Development (No Auth)
    User->>Client: Access Application
    Client->>Server: gRPC Request
    Server->>DB: Query Data
    DB-->>Server: Return Data
    Server-->>Client: gRPC Response
    Client-->>User: Display Data
    
    Note over User,DB: Phase 2: Production (Planned)
    User->>Client: Access Application
    Client->>CAC: Redirect to CAC Login
    CAC->>User: Request CAC Certificate
    User->>CAC: Present CAC
    CAC->>CAC: Validate Certificate
    CAC-->>Client: Return JWT Token
    
    Client->>Client: Store JWT in Local Storage
    Client->>Server: gRPC Request<br/>+ Authorization: Bearer JWT
    
    Server->>Server: Validate JWT
    Server->>Server: Extract Claims<br/>(UserId, Roles, Groups)
    Server->>Server: Check Permissions
    
    alt Authorized
        Server->>DB: Query Data
        DB-->>Server: Return Data
        Server-->>Client: gRPC Response
        Client-->>User: Display Data
    else Unauthorized
        Server-->>Client: 403 Forbidden
        Client-->>User: Access Denied
    end
    
    Note over Client,Server: JWT Refresh Flow
    Client->>Server: Request with Expired JWT
    Server-->>Client: 401 Unauthorized
    Client->>CAC: Refresh Token
    CAC-->>Client: New JWT
    Client->>Server: Retry with New JWT
```

---

## Request Processing Pipeline

```mermaid
flowchart TD
    Start([Client HTTP Request]) --> HTTPS{HTTPS<br/>Redirect?}
    HTTPS -->|HTTP| Redirect[302 Redirect to HTTPS]
    HTTPS -->|HTTPS| CORS{CORS<br/>Preflight?}
    
    CORS -->|OPTIONS| CORSCheck[Validate Origin]
    CORSCheck --> CORSResponse[Return CORS Headers]
    CORSResponse --> End([Response])
    
    CORS -->|GET/POST| GrpcWeb[gRPC-Web Translation]
    GrpcWeb --> Routing[Route Matching]
    
    Routing --> RateLimit{Rate Limit<br/>Exceeded?}
    RateLimit -->|Yes| RateLimitError[429 Too Many Requests]
    RateLimitError --> End
    
    RateLimit -->|No| Antiforgery{State-Changing<br/>Request?}
    Antiforgery -->|Yes| ValidateToken{Valid<br/>Token?}
    ValidateToken -->|No| AntiforgeryError[400 Bad Request]
    AntiforgeryError --> End
    
    ValidateToken -->|Yes| ServiceCall[gRPC Service Method]
    Antiforgery -->|No| ServiceCall
    
    ServiceCall --> Resilience[Polly Resilience Policies]
    Resilience --> DataAccess[Data Service Call]
    DataAccess --> EFCore[EF Core Query]
    EFCore --> RetryPolicy{Transient<br/>Failure?}
    
    RetryPolicy -->|Yes, Retries Left| Retry[Exponential Backoff]
    Retry --> EFCore
    
    RetryPolicy -->|No Retries| DbError[500 Internal Server Error]
    DbError --> Audit1[Log Error + Correlation ID]
    Audit1 --> End
    
    RetryPolicy -->|Success| DBQuery[(SQL Server Query)]
    DBQuery --> AuditLog[Write Audit Log]
    AuditLog --> Cache{Cacheable?}
    
    Cache -->|Yes| CacheWrite[Write to Redis]
    CacheWrite --> Response[Build gRPC Response]
    Cache -->|No| Response
    
    Response --> Telemetry[OpenTelemetry Trace]
    Telemetry --> GrpcResponse[gRPC-Web Response]
    GrpcResponse --> End
    
    style Start fill:#4CAF50
    style End fill:#F44336
    style ServiceCall fill:#2196F3
    style DBQuery fill:#FF9800
    style Cache fill:#9C27B0
```

---

## Service Dependencies

```mermaid
graph LR
    subgraph "AF.ECT.AppHost"
        AppHost[AppHost]
    end
    
    subgraph "AF.ECT.Server"
        Server[Server]
        ServerExt[Extensions]
        Interceptors[Interceptors]
        GrpcServices[gRPC Services]
    end
    
    subgraph "AF.ECT.WebClient"
        WebClient[WebClient]
        WebPages[Blazor Pages]
        WebServices[Client Services]
    end
    
    subgraph "AF.ECT.Shared"
        Protos[Protobuf Definitions]
        WorkflowClient[WorkflowClient]
        Options[Options Classes]
    end
    
    subgraph "AF.ECT.Data"
        DataService[DataService]
        Entities[EF Entities]
        Configurations[EF Configurations]
    end
    
    subgraph "AF.ECT.ServiceDefaults"
        Telemetry[OpenTelemetry]
        HealthChecks[Health Checks]
        Resilience[Resilience Policies]
    end
    
    AppHost --> Server
    AppHost --> WebClient
    
    Server --> Shared
    Server --> Data
    Server --> ServiceDefaults
    
    WebClient --> Shared
    
    WorkflowClient --> Protos
    DataService --> Entities
    
    style Server fill:#2196F3
    style WebClient fill:#4CAF50
    style Shared fill:#FF9800
    style Data fill:#9C27B0
```

---

## Legend

### Colors
- 🟢 **Green**: Client/User facing components
- 🔵 **Blue**: Server/API components  
- 🟠 **Orange**: Data/Database components
- 🟣 **Purple**: Infrastructure/Support components
- 🔴 **Red**: Error/End states

### Symbols
- **Rectangle**: Process or component
- **Cylinder**: Database or storage
- **Diamond**: Decision point
- **Circle**: Start/End point
- **Dotted line**: Management/orchestration relationship
- **Solid line**: Direct dependency or data flow

---

## Additional Resources

- [System Architecture Wiki](AF.ECT.Wiki/WikiPages/architecture.md)
- [API Documentation](API_DOCUMENTATION.md)
- [Contributing Guidelines](CONTRIBUTING.md)
- [README](README.md)

---

*Last Updated: October 26, 2025*  
*Diagrams created with Mermaid - View in GitHub or Markdown preview*
