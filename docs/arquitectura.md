# Arquitectura — PeoplePortal

## C4 Nivel 1 — Diagrama de Contexto

```mermaid
flowchart LR
    Colaborador([Colaborador])
    Jefe([Jefe Inmediato])
    RRHH([RRHH])
    Nomina([Nómina])
    Admin([Administrador])
    
    PeoplePortal[[PeoplePortal API]]
    
    Keycloak[(Keycloak)]
    SQLServer[(SQL Server)]
    NATS[(NATS JetStream)]
    APISIX[[APISIX Gateway]]
    
    Colaborador --> APISIX
    Jefe --> APISIX
    RRHH --> APISIX
    Nomina --> APISIX
    Admin --> APISIX
    
    APISIX --> PeoplePortal
    PeoplePortal --> Keycloak
    PeoplePortal --> SQLServer
    PeoplePortal --> NATS
```

## C4 Nivel 2 — Diagrama de Contenedores

```mermaid
flowchart TB
    subgraph Cliente
        React[React 19 SPA\nVite + MUI]
    end
    
    subgraph Gateway
        APISIX[APISIX\nAPI Gateway]
    end
    
    subgraph Backend
        API[.NET 9 Web API\nClean Architecture]
        API --> Domain[PeoplePortal.Domain]
        API --> Application[PeoplePortal.Application\nCQRS + MediatR]
        API --> Infrastructure[PeoplePortal.Infrastructure\nEF Core + NATS]
    end
    
    subgraph Datos
        SQL[(SQL Server)]
    end
    
    subgraph Mensajeria
        NATS[NATS JetStream]
    end
    
    subgraph Identidad
        KC[Keycloak]
    end
    
    React --> APISIX
    APISIX --> API
    API --> KC
    API --> SQL
    API --> NATS
```

## Stack tecnológico

| Componente | Tecnología |
|---|---|
| Backend | .NET 9, Clean Architecture, CQRS, MediatR |
| Frontend Colaborador | React 19, Vite, MUI v9, Keycloak-js |
| Frontend RRHH | React 19, Vite, MUI v9, Keycloak-js |
| Base de datos | SQL Server 2022, EF Core 9 |
| Autenticación | Keycloak (JWT + PKCE) |
| API Gateway | APISIX con plugin openid-connect |
| Mensajería | NATS JetStream |
| Contenedores | Docker + docker-compose |
| Orquestación | Kubernetes (manifiestos en /k8s) |
| CI/CD | GitHub Actions + Codacy + Trivy |
