# Arquitectura — PeoplePortal BackEnd

## Clean Architecture: capas

```mermaid
graph TB
    subgraph API["PeoplePortal.Api — ASP.NET Core 9"]
        Controllers["Controllers\nAnnouncements, Benefits, Dashboard,\nDocuments, Employees, HrDocuments,\nHrRequests, Manager, Reports, Requests"]
        Middleware["ExceptionHandlingMiddleware"]
        Program["Program.cs\nJWT + Policies + DI + CORS"]
        Contracts["Request Body DTOs"]
        Extensions["ClaimsPrincipalExtensions"]
    end

    subgraph Application["PeoplePortal.Application — CQRS / MediatR"]
        Commands["Commands & Handlers\nCreateVacation, ApproveByManager,\nCancelRequest, UploadDocument,\nCreateEmployee, UpdateProfile,\nCreateBenefit, UpdateBenefit, etc."]
        Queries["Queries & Handlers\nGetMyRequests, GetAllEmployee,\nGetDashboard, GetActiveBenefits,\nGetReports, etc."]
        Validators["FluentValidation\nValidationBehavior<T>"]
        Interfaces["IEventBus"]
        RepoInterfaces["IRepository Interfaces\nIHrRequestRepository, IEmployeeRepository,\nIDocumentRepository, IAnnouncementRepository,\nIBenefitRepository"]
        DTOs["Dto Models"]
    end

    subgraph Domain["PeoplePortal.Domain — Core"]
        Entities["Entities\nEmployee, HrRequest, Document,\nVoucher, Announcement, Benefit"]
        Enums["Enums\nRequestStatus, RequestType,\nContractType, EmployeeStatus,\nDocumentStatus, VoucherStatus,\nAnnouncementType"]
    end

    subgraph Infrastructure["PeoplePortal.Infrastructure — Adapters"]
        EF["EF Core / PostgreSQL\nPeoplePortalDbContext"]
        Repos["Repositories\nHrRequest, Employee, Document,\nAnnouncement, Benefit"]
        NATS["NATS JetStream\nNatsEventBus, EventConsumerService\nStream: peopleportal-events\nSubjects: hr.>, employee.>"]
        DI["DependencyInjection\nAddInfrastructure()"]
    end

    Controllers --> Commands
    Controllers --> Queries
    Commands --> RepoInterfaces
    Commands --> Interfaces
    Queries --> RepoInterfaces
    Commands --> Entities
    Queries --> DTOs
    Program --> Middleware
    RepoInterfaces --> Repos
    Interfaces --> NATS
    Repos --> EF
    EF --> Entities
```

---

## Flujo CQRS con MediatR

```mermaid
sequenceDiagram
    participant C as Cliente (React)
    participant API as PeoplePortal.Api
    participant MediatR as MediatR Pipeline
    participant Val as ValidationBehavior
    participant Handler as Command Handler
    participant Repo as Repository
    participant DB as PostgreSQL
    participant NATS as NATS JetStream

    C->>API: POST /api/requests/vacation (JWT)
    API->>API: Validar JWT + Policy
    API->>MediatR: Send(CreateVacationRequestCommand)
    MediatR->>Val: ValidationBehavior<T>
    Val-->>MediatR: válido
    MediatR->>Handler: Handle()
    Handler->>Handler: HrRequest.CreateVacation()
    Handler->>Repo: AddAsync(hrRequest)
    Repo->>DB: INSERT
    Handler->>NATS: Publish("hr.request.submitted", payload)
    NATS-->>Handler: ack
    Handler-->>API: HrRequestDto
    API-->>C: 201 Created
```

---

## Autenticación y autorización

```mermaid
flowchart TD
    A[Request con JWT] --> B{¿Token presente?}
    B -->|No| C[401 Unauthorized]
    B -->|Sí| D[Validar JWT Bearer]
    D --> E{¿Válido?\nissuer + audience + signingKey}
    E -->|No| C
    E -->|Sí| F[OnTokenValidated]
    F --> G[Extraer realm_access.roles]
    G --> H[Mapear a ClaimTypes.Role]
    H --> I{Evaluar Policy}

    I --> L[EmployeePolicy → role: employee]
    I --> M[ManagerPolicy → role: jefe_inmediato]
    I --> N[HrPolicy → role: hr]
    I --> O[NominaPolicy → role: nomina]
    I --> P[AdminPolicy → role: admin]

    L --> Q[✅ Allow]
    M --> Q
    N --> Q
    O --> Q
    P --> Q
    I -->|ninguna| R[403 Forbidden]
```

---

## Estructura del proyecto

```
PeoplePortal-BackEnd/
├── src/
│   ├── PeoplePortal.Domain/
│   │   ├── Entities/          ← Employee, HrRequest, Document, Voucher, Announcement, Benefit
│   │   └── Enums/             ← RequestStatus, RequestType, ContractType, ...
│   │
│   ├── PeoplePortal.Application/
│   │   ├── Common/
│   │   │   ├── Behaviors/     ← ValidationBehavior<T>
│   │   │   └── Interfaces/    ← IEventBus
│   │   ├── Contracts/Persistence/  ← Interfaces de repositorios
│   │   ├── Announcements/
│   │   ├── Benefits/
│   │   ├── Dashboard/
│   │   ├── Documents/
│   │   ├── Employees/
│   │   ├── Requests/
│   │   ├── Reports/
│   │   ├── Vouchers/
│   │   └── DependencyInjection.cs
│   │
│   ├── PeoplePortal.Infrastructure/
│   │   ├── Messaging/         ← NatsEventBus, EventConsumerService
│   │   ├── Persistence/
│   │   │   ├── Migrations/
│   │   │   ├── Repositories/
│   │   │   └── PeoplePortalDbContext.cs
│   │   └── DependencyInjection.cs
│   │
│   └── PeoplePortal.Api/
│       ├── Controllers/       ← 10 controladores
│       ├── Middleware/        ← ExceptionHandlingMiddleware
│       ├── Extensions/        ← ClaimsPrincipalExtensions
│       ├── Contracts/         ← Request body DTOs
│       └── Program.cs
│
├── tests/
│   ├── PeoplePortal.UnitTests/
│   └── PeoplePortal.IntegrationTests/
└── docs/                      ← esta carpeta
```
