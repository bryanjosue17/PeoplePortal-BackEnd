# PeoplePortal — BackEnd API

API REST en .NET 9 con Clean Architecture + CQRS para el sistema de autoservicio de colaboradores.

## Stack

| Componente | Tecnología |
|---|---|
| Runtime | .NET 9 |
| Arquitectura | Clean Architecture + CQRS |
| Mediator | MediatR |
| Validación | FluentValidation |
| ORM | Entity Framework Core 9 |
| Base de datos | PostgreSQL 16 |
| Mensajería | NATS JetStream |
| Autenticación | Keycloak (JWT Bearer + PKCE S256) |
| API Gateway | APISIX (plugin openid-connect) |
| CI/CD | GitHub Actions + Trivy + Codacy |

## Cómo correr localmente

```bash
# Variable opcional (tiene valor por defecto en docker-compose)
export POSTGRES_PASSWORD=YourStrong@Passw0rd

# Levantar servicios
docker-compose up -d

# Aplicar migraciones
docker-compose run --rm migrate

# API disponible en: http://localhost:8081
# Health check:      http://localhost:8081/health
```

## Tests

```bash
dotnet test --collect:"XPlat Code Coverage"
```

Estado actual: **49 tests unitarios** + **5 tests de integración** — todos en verde ✅  
Cobertura: ≥ 60% (líneas y ramas)

## Módulos de la API

| Módulo | Controlador | Descripción |
|---|---|---|
| Empleados | `EmployeesController` | Perfil propio + CRUD RRHH |
| Solicitudes | `RequestsController` | Vacaciones, constancias, vouchers |
| Aprobación | `ManagerController` | Aprobación por jefe inmediato |
| Solicitudes RRHH | `HrRequestsController` | Gestión de solicitudes por RRHH |
| Documentos | `DocumentsController` + `HrDocumentsController` | Expediente digital |
| Comunicados | `AnnouncementsController` | Publicación y consulta |
| Beneficios | `BenefitsController` + `HrBenefitsController` | Catálogo + CRUD RRHH |
| Dashboard | `DashboardController` | Datos agregados para el colaborador |
| Reportes | `ReportsController` | 5 reportes administrativos |

## Documentación técnica

Ver [`docs/`](./docs/README.md) para documentación detallada de arquitectura, endpoints, base de datos, flujos, seguridad y despliegue.
