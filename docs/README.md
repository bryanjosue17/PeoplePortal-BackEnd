# PeoplePortal — Documentación del Backend

## 1. Arquitectura por Capas (Clean Architecture)

```mermaid
graph TB
    subgraph API["PeoplePortal.Api — ASP.NET Core 9"]
        Controllers["Controllers<br/>Announcements, Benefits, Dashboard,<br/>Documents, Employees, HrDocuments,<br/>HrRequests, Manager, Requests"]
        Middleware["ExceptionHandlingMiddleware"]
        Program["Program.cs<br/>JWT + Policies + DI + CORS"]
        Contracts["Request Body DTOs"]
        Extensions["ClaimsPrincipalExtensions"]
    end

    subgraph Application["PeoplePortal.Application — CQRS / MediatR"]
        Commands["Commands & Handlers<br/>CreateVacation, ApproveByManager,<br/>CancelRequest, UploadDocument,<br/>CreateEmployee, UpdateProfile, etc."]
        Queries["Queries & Handlers<br/>GetMyRequests, GetAllEmployee,<br/>GetDashboard, GetActiveBenefits, etc."]
        Validators["FluentValidation<br/>ValidationBehavior&lt;T&gt;"]
        Interfaces["IEventBus"]
        RepoInterfaces["IRepository Interfaces<br/>IHrRequestRepository, IEmployeeRepository,<br/>IDocumentRepository, IAnnouncementRepository,<br/>IBenefitRepository"]
        DTOs["Dto Models"]
    end

    subgraph Domain["PeoplePortal.Domain — Core"]
        Entities["Entities<br/>Employee, HrRequest, Document,<br/>Voucher, Announcement, Benefit"]
        Enums["Enums<br/>RequestStatus, RequestType,<br/>ContractType, EmployeeStatus,<br/>DocumentStatus, VoucherStatus,<br/>AnnouncementType"]
    end

    subgraph Infrastructure["PeoplePortal.Infrastructure — Adapters"]
        EF["EF Core / SQL Server<br/>PeoplePortalDbContext"]
        Repos["Repositories<br/>HrRequestRepository, EmployeeRepository,<br/>DocumentRepository, AnnouncementRepository,<br/>BenefitRepository"]
        NATS["NATS JetStream<br/>NatsEventBus, EventConsumerService<br/>Stream: peopleportal-events<br/>Subjects: hr.>, employee.>"]
        DI["DependencyInjection<br/>AddInfrastructure()"]
    end

    Controllers --> Commands
    Controllers --> Queries
    Commands --> RepoInterfaces
    Commands --> Interfaces
    Queries --> RepoInterfaces
    Commands --> Entities
    Queries --> DTOs
    Program --> Middleware
    Program --> Controllers
    RepoInterfaces --> Repos
    Interfaces --> NATS
    Repos --> EF
    EF --> Entities
```

---

## 2. Flujo de Eventos

```mermaid
sequenceDiagram
    participant C as Cliente (React)
    participant API as PeoplePortal API
    participant MediatR as MediatR Pipeline
    participant Handler as Command Handler
    participant DB as SQL Server
    participant NATS as NATS JetStream

    C->>API: POST /api/requests/vacation
    API->>API: Validar JWT + Policy
    API->>MediatR: Send(CreateVacationRequestCommand)
    MediatR->>MediatR: ValidationBehavior
    MediatR->>Handler: Handle()
    Handler->>Handler: HrRequest.CreateVacation()
    Handler->>DB: repository.AddAsync()
    Handler->>DB: SaveChangesAsync()
    Handler->>NATS: Publish("hr.request.submitted", payload)
    NATS-->>Handler: ack
    Handler-->>API: HrRequestDto
    API-->>C: 201 Created

    Note over NATS: Stream "peopleportal-events"<br/>Subjects: hr.request.submitted,<br/>hr.request.approved,<br/>employee.*
```

---

## 3. Autenticación y Autorización

```mermaid
flowchart TD
    A[Request con JWT] --> B{¿Token presente?}
    B -->|No| C[401 Unauthorized]
    B -->|Sí| D[Validar JWT Bearer]
    D --> E{¿Token válido?<br/>Issuer + Audience + SigningKey}
    E -->|No| C
    E -->|Sí| F[OnTokenValidated]
    F --> G[Extraer realm_access]
    G --> H{¿Contiene roles?}
    H -->|No| I[Solo autenticado]
    H -->|Sí| J[Mapear roles a<br/>ClaimTypes.Role]
    J --> I
    I --> K{Evaluar Policy}
    
    K --> L[EmployeePolicy<br/>RequireRole: employee]
    K --> M[ManagerPolicy<br/>RequireRole: jefe_inmediato]
    K --> N[HrPolicy<br/>RequireRole: hr]
    K --> O[NominaPolicy<br/>RequireRole: nomina]
    K --> P[AdminPolicy<br/>RequireRole: admin]
    
    L --> Q[Allow]
    M --> Q
    N --> Q
    O --> Q
    P --> Q
    K -->|Ninguna coincide| R[403 Forbidden]

    subgraph Keycloak["Keycloak Realm"]
        S[Roles disponibles:<br/>employee, jefe_inmediato,<br/>hr, nomina, admin]
    end
    E --> Keycloak
```

---

## 4. Endpoints de la API

| Controller | Método | Ruta | Auth Policy |
|---|---|---|---|
| **RequestsController** | POST | `/api/requests/vacation` | EmployeePolicy |
| | POST | `/api/requests/certificate` | EmployeePolicy |
| | POST | `/api/requests/voucher` | EmployeePolicy |
| | POST | `/api/requests/{id}/cancel` | EmployeePolicy |
| | GET | `/api/requests/me` | EmployeePolicy |
| **ManagerController** | PATCH | `/api/manager/requests/{id}/status` | ManagerPolicy |
| **HrRequestsController** | GET | `/api/hr/requests` | HrPolicy |
| | PATCH | `/api/hr/requests/{id}/status` | HrPolicy |
| **DocumentsController** | GET | `/api/documents/me` | EmployeePolicy |
| **HrDocumentsController** | GET | `/api/hr/documents` | HrPolicy |
| | POST | `/api/hr/documents` | HrPolicy |
| | PATCH | `/api/hr/documents/{id}/status` | HrPolicy |
| **DashboardController** | GET | `/api/dashboard` | EmployeePolicy |
| **AnnouncementsController** | GET | `/api/announcements` | EmployeePolicy |
| | POST | `/api/hr/announcements` | HrPolicy |
| **BenefitsController** | GET | `/api/benefits` | EmployeePolicy |
| **EmployeesController** | GET | `/api/employees/me` | EmployeePolicy |
| | PUT | `/api/employees/me` | EmployeePolicy |
| | GET | `/api/hr/employees` | HrPolicy |
| | GET | `/api/hr/employees/{id}` | HrPolicy |
| | POST | `/api/hr/employees` | HrPolicy |
| | PUT | `/api/hr/employees/{id}` | HrPolicy |
| Health Checks | GET | `/health` | Público |

---

## 5. Estructura del Proyecto

```
BackEnd/
├── PeoplePortal.sln
├── Dockerfile
├── docker-compose.yml
├── src/
│   ├── PeoplePortal.Domain/          # Capa de dominio
│   │   ├── Entities/                 #   Employee, HrRequest, Document, Voucher, Announcement, Benefit
│   │   └── Enums/                    #   RequestStatus, RequestType, ContractType, EmployeeStatus, etc.
│   │
│   ├── PeoplePortal.Application/     # Capa de aplicación (CQRS)
│   │   ├── Common/
│   │   │   ├── Behaviors/            #   ValidationBehavior<T>
│   │   │   └── Interfaces/           #   IEventBus
│   │   ├── Contracts/Persistence/    #   Repositorios (interfaces)
│   │   ├── Announcements/            #   Commands/Queries/Dtos/Mappings
│   │   ├── Benefits/                 #   Queries/Dtos/Mappings
│   │   ├── Dashboard/                #   Queries/Dtos
│   │   ├── Documents/                #   Commands/Queries/Dtos/Mappings
│   │   ├── Employees/                #   Commands/Queries/Dtos/Mappings
│   │   ├── Requests/                 #   Commands/Queries/Dtos/Mappings
│   │   ├── Vouchers/                 #   Commands/Queries/Dtos/Mappings
│   │   └── DependencyInjection.cs
│   │
│   ├── PeoplePortal.Infrastructure/  # Capa de infraestructura
│   │   ├── Messaging/                #   NatsEventBus, EventConsumerService
│   │   ├── Persistence/
│   │   │   ├── Migrations/           #   EF Core migrations
│   │   │   ├── Repositories/         #   Implementaciones de repositorios
│   │   │   ├── PeoplePortalDbContext.cs
│   │   │   └── PeoplePortalDbContextFactory.cs
│   │   └── DependencyInjection.cs
│   │
│   └── PeoplePortal.Api/            # Capa de presentación (API)
│       ├── Controllers/              #   9 controladores
│       ├── Middleware/               #   ExceptionHandlingMiddleware
│       ├── Extensions/               #   ClaimsPrincipalExtensions
│       ├── Contracts/                #   Request body DTOs
│       └── Program.cs                #   Startup: JWT, Policies, DI, CORS, Swagger
│
├── tests/                            # Pruebas
├── docs/                             # Documentación
│   ├── README.md                     # Este archivo
│   ├── arquitectura.md               # Diagramas C4
│   ├── base-de-datos.md              # Diagrama ER
│   ├── seguridad.md                  # OWASP Top 10
│   ├── flujos.md                     # Flujos del sistema
│   └── despliegue.md                 # Despliegue
├── deploy/                           # Scripts de despliegue
├── k8s/                              # Manifiestos Kubernetes
└── .github/                          # CI/CD (GitHub Actions)
```

---

## Stack Tecnológico

| Componente | Tecnología |
|---|---|
| Runtime | .NET 9 |
| Arquitectura | Clean Architecture + CQRS |
| Mediator | MediatR |
| Validación | FluentValidation |
| ORM | Entity Framework Core 9 |
| Base de datos | SQL Server 2022 |
| Mensajería | NATS JetStream |
| Autenticación | Keycloak (JWT Bearer + PKCE) |
| API Gateway | APISIX (plugin openid-connect) |
| Contenedores | Docker + docker-compose |
| Orquestación | Kubernetes |
| CI/CD | GitHub Actions + Trivy + Codacy |

> Para más detalle, ver `arquitectura.md`, `base-de-datos.md`, `seguridad.md`, `flujos.md` y `despliegue.md` en este directorio.
