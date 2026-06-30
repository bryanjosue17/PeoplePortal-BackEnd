# Plan de Implementación — PeoplePortal Backend

## Leyenda
| Ícono | Significado |
|-------|-------------|
| ✅ | Implementado |
| ⚠️ | Parcial |
| ❌ | Pendiente |

---

## Acciones recientes en el workspace

- Se creó el monorepo `ProyectoIA-Forza` y se añadieron los submódulos: `PeoplePortal-BackEnd`, `PeoplePortal-FrontEnd-Colaborador`, `PeoplePortal-FrontEnd-RRHH`.
- Se crearon y subieron las ramas `main` y `develop` en el monorepo y en los submódulos (siendo `develop` la rama de trabajo).
- Se eliminó la rama remota `master` en los repos front-end y se estandarizó a `main`/`develop`.
- Se actualizó el `README.md` raíz con instrucciones de monorepo y se agregó `CONTRIBUTING.md` con el flujo de trabajo.
- Se activó protección de rama sobre `develop` en el repo `ProyectoIA-Forza`.
- Backups locales creados durante el proceso fueron eliminados.

Notas: algunas tareas puntuales (p. ej. branch protection en `PeoplePortal-BackEnd`, configuración CD, documentación interna del backend) quedan pendientes y se listan abajo.


## Fase 0 — Fundamentos (Semana 1, ~3h)

### Infraestructura y repo
| Tarea | Estado | Detalle |
|-------|--------|---------|
| Branch protection en `main` y `develop` | ⚠️ | Protección activada en el repo monorepo `ProyectoIA-Forza` (develop). Falta activar en este repo (`PeoplePortal-BackEnd`). |
| Conventional Commits | ❌ | Adoptar formato `tipo(alcance): descripción` |
| `.editorconfig` | ❌ | Crear desde el estándar Forza |
| `CHANGELOG.md` | ❌ | Iniciar con cambios actuales |

### Calidad y CI/CD
| Tarea | Estado | Detalle |
|-------|--------|---------|
| CI actual (build + test + Trivy) | ✅ | Funciona |
| CD: build + push Docker image a GHCR | ❌ | Agregar step de `docker build` + `docker push` |
| CD: deploy a K8s | ❌ | Agregar step de `kubectl apply` o helm upgrade |
| Codacy — issues críticos/altos en 0 | ❌ | Revisar y limpiar |
| Cobertura ≥ 60% | ❌ | Meta para fin del plan |

---

## Fase 1 — Ampliación del Dominio (Semana 2, ~4h)

### Nuevas entidades
| Entidad | Atributos clave | Prioridad |
|---------|----------------|-----------|
| `Employee` | Id, Code, FullName, Email, Phone, Department, Position, HireDate, ContractType, Status, EmergencyContact, Site, ManagerId | Alta |
| `Document` | Id, EmployeeId, Name, Type, Status, FileUrl, ExpiresAt, UploadedAt | Alta |
| `Announcement` | Id, Title, Body, PublishedAt, ExpiresAt, CreatedBy | Media |
| `Benefit` | Id, Name, Description, Type, IsActive | Media |
| `Voucher` | Id, EmployeeId, Period, Status, FileUrl, RequestedAt | Alta |
| `User` | Id, EmployeeId, Role, KeycloakId, IsActive | Alta |

### Enumeraciones nuevas
| Enum | Valores |
|------|---------|
| `DocumentStatus` | Available, Pending, InReview, Approved, Rejected, Expired |
| `VoucherStatus` | Requested, InProcess, AvailableForDownload, Rejected, Completed |
| `AnnouncementType` | News, HrNotice, PolicyChange, Event, Reminder, Birthday, Institutional |
| `EmployeeStatus` | Active, Inactive, OnLeave, Terminated |
| `ContractType` | Permanent, Temporary, Freelance, Intern |

### Eventos de dominio (EDD)
| Evento | Disparador | Consumidor |
|--------|------------|------------|
| `hr.request.submitted` | Creación de solicitud | Notificar a RRHH / jefe inmediato |
| `hr.request.approved` | Cambio de estado a Approved | Generar documento si aplica |
| `employee.document.uploaded` | RRHH sube documento | Notificar colaborador |
| `employee.voucher.available` | Nómina carga voucher | Notificar colaborador |

---

## Fase 2 — EDD: NATS JetStream (Semana 2-3, ~3h)

### Configuración
| Tarea | Archivo / Ruta | Detalle |
|-------|----------------|---------|
| Agregar paquete `NATS.Net` | Infrastructure.csproj | `dotnet add package NATS.Net` |
| `NatsOptions` + settings | `appsettings.json` | URL, credenciales, streams |
| `NatsEventBus` | `Infrastructure/Messaging/NatsEventBus.cs` | Implementar `IEventBus` |
| `IEventBus` interface | `Application/Common/Interfaces/IEventBus.cs` | `PublishAsync<T>` |
| Stream config al iniciar | `Infrastructure/DependencyInjection.cs` | Crear stream `peopleportal-events` |
| Publicar eventos en handlers | Handlers existentes + nuevos | Ej: `CreateVacationRequestCommandHandler` publica `hr.request.submitted` |

### Consumidores (background service)
| Tarea | Archivo | Detalle |
|-------|---------|---------|
| `RequestSubmittedConsumer` | `Infrastructure/Consumers/` | Escucha `hr.request.submitted` |
| `RequestApprovedConsumer` | `Infrastructure/Consumers/` | Escucha `hr.request.approved` |

---

## Fase 3 — APISIX (Semana 2-3, ~1h)

### Configuración declarativa
| Tarea | Archivo | Detalle |
|-------|---------|---------|
| Ruta para Requests API | `deploy/apisix/routes.yml` | `GET/POST /api/requests/*` → upstream `peopleportal-api:8080` |
| Ruta para HR API | `deploy/apisix/routes.yml` | `PATCH /api/hr/*` → upstream `peopleportal-api:8080` |
| Plugin `openid-connect` | `deploy/apisix/routes.yml` | Conectar con Keycloak realm `peopleportal` |
| `docker-compose` con APISIX | `docker-compose.yml` | Agregar servicio APISIX + etcd |
| K8s APISIX ingress | `k8s/apisix-ingress.yaml` | O APISIX standalone deployment |

---

## Fase 4 — Expansión de API (Semana 3, ~4h)

### EmployeesController
| Endpoint | Método | Ruta | Rol |
|----------|--------|------|-----|
| Get my profile | GET | `/api/employees/me` | employee |
| Update my profile | PUT | `/api/employees/me` | employee |
| List employees (HR) | GET | `/api/hr/employees` | hr |
| Get employee by id (HR) | GET | `/api/hr/employees/{id}` | hr |
| Create employee (HR) | POST | `/api/hr/employees` | hr |
| Update employee (HR) | PUT | `/api/hr/employees/{id}` | hr |

### DocumentsController
| Endpoint | Método | Ruta | Rol |
|----------|--------|------|-----|
| List my documents | GET | `/api/documents/me` | employee |
| Download document | GET | `/api/documents/{id}/download` | employee |
| Upload document (HR) | POST | `/api/hr/documents` | hr |
| List all documents (HR) | GET | `/api/hr/documents` | hr |
| Update document status (HR) | PATCH | `/api/hr/documents/{id}/status` | hr |

### VouchersController
| Endpoint | Método | Ruta | Rol |
|----------|--------|------|-----|
| Request voucher | POST | `/api/vouchers` | employee |
| My vouchers | GET | `/api/vouchers/me` | employee |
| Download voucher | GET | `/api/vouchers/{id}/download` | employee |
| Upload voucher (Nómina) | POST | `/api/nomina/vouchers/{id}/upload` | nomina |
| List vouchers (HR) | GET | `/api/hr/vouchers` | hr |
| Update voucher status (HR) | PATCH | `/api/hr/vouchers/{id}/status` | hr |

### AnnouncementsController
| Endpoint | Método | Ruta | Rol |
|----------|--------|------|-----|
| List active announcements | GET | `/api/announcements` | employee |
| Get announcement | GET | `/api/announcements/{id}` | employee |
| Create announcement (HR) | POST | `/api/hr/announcements` | hr |
| Update announcement (HR) | PUT | `/api/hr/announcements/{id}` | hr |
| Delete announcement (HR) | DELETE | `/api/hr/announcements/{id}` | hr |

### BenefitsController
| Endpoint | Método | Ruta | Rol |
|----------|--------|------|-----|
| List benefits | GET | `/api/benefits` | employee |
| Get benefit detail | GET | `/api/benefits/{id}` | employee |
| Manage benefits (HR) | CRUD | `/api/hr/benefits` | hr |

### ReportsController
| Endpoint | Método | Ruta | Rol |
|----------|--------|------|-----|
| Requests by status | GET | `/api/hr/reports/requests-by-status` | hr |
| Requests by type | GET | `/api/hr/reports/requests-by-type` | hr |
| Requests over time | GET | `/api/hr/reports/requests-over-time` | hr |
| Active employees count | GET | `/api/hr/reports/active-employees` | hr |
| Pending documents | GET | `/api/hr/reports/pending-documents` | hr |

### Expansión de RequestsController
| Endpoint | Método | Ruta | Rol |
|----------|--------|------|-----|
| Request voucher | POST | `/api/requests/voucher` | employee |
| Request permission | POST | `/api/requests/permission` | employee |
| Cancel my request | POST | `/api/requests/{id}/cancel` | employee |
| List my requests (paginated) | GET | `/api/requests/me?page=&size=` | employee |

### Approval flow (Jefe inmediato)
| Endpoint | Método | Ruta | Rol |
|----------|--------|------|-----|
| List team requests | GET | `/api/manager/requests` | jefe_inmediato |
| Approve/Reject vacation | PATCH | `/api/manager/requests/{id}/status` | jefe_inmediato |

---

## Fase 5 — Documentación (Semana 3, ~2h)

### Carpeta `docs/`
| Archivo | Contenido | Estado |
|---------|-----------|--------|
| `docs/arquitectura.md` | Diagrama C4 N1 + N2 en Mermaid | ❌ |
| `docs/flujos.md` | Diagramas sequence: crear solicitud, aprobar, subir documento | ❌ |
| `docs/base-de-datos.md` | erDiagram Mermaid con todas las tablas | ❌ |
| `docs/despliegue.md` | Pipeline + runbook de deploy local y K8s | ❌ |
| `docs/seguridad.md` | Mapeo OWASP Top 10 + cómo se mitiga cada uno | ❌ |
| `docs/prompts/README.md` | Índice del catálogo + reglas de uso | ❌ |
| `docs/prompts/arquitectura/` | Prompts usados para diseñar entidades, C4, etc. | ❌ |
| `docs/prompts/codigo/` | Prompts usados para scaffolding, handlers, NATS, etc. | ❌ |
| `docs/prompts/tests/` | Prompts para generar tests unitarios e integración | ❌ |

### README.md raíz
| Tarea | Estado |
|-------|--------|
| Actualizar con prerequisitos, comandos, variables de entorno | ✅ (actualizado en el monorepo raíz) |
| Agregar matriz de cumplimiento (Anexo B del Brief) | ✅ (presente en el monorepo raíz) |
| Agregar screenshots / enlaces | ❌ |

---

## Fase 6 — Roles y Seguridad (Semana 3-4, ~2h)

### Roles completos
| Rol | README dice | Estado |
|-----|-------------|--------|
| `employee` | Consulta info, crea solicitudes | ✅ |
| `jefe_inmediato` | Aprueba vacaciones/permisos de su equipo | ❌ |
| `hr` | Administra colaboradores, docs, solicitudes | ⚠️ (solo parcial) |
| `nomina` | Carga vouchers de pago | ❌ |
| `admin` | Gestiona usuarios, roles, permisos | ❌ |

### Tareas de seguridad
| Tarea | Detalle |
|-------|---------|
| Agregar policies para los 5 roles | `Program.cs` |
| Mapeo OWASP Top 10 en `docs/seguridad.md` | Incluir mitigación de cada riesgo |
| Validación con FluentValidation | Implementar validadores para todos los requests |
| Exception handling middleware | Capturar y devolver `ProblemDetails` |
| CORS | Configurar origen del frontend |
| Rate limiting (opcional) | `AspNetCoreRateLimit` o middleware propio |

---

## Fase 7 — Tests y Calidad (Semana 4, ~2h)

### Tests unitarios
| Módulo | Tests a agregar | Mínimo |
|--------|-----------------|--------|
| Domain entities | Employee, Document, Voucher, Announcement, Benefit | 2 c/u |
| Command handlers | Todos los commands nuevos | 2 c/u |
| Query handlers | Todos los queries nuevos | 2 c/u |
| Controllers | RequestsController, HrRequestsController | 3 c/u |
| Validators | FluentValidation tests | 1 c/u |

### Tests de integración
| Escenario | Descripción |
|-----------|-------------|
| Crear solicitud + verificar en BD | Testcontainers para SQL Server |
| Flujo completo: solicitar → aprobar → descargar | |
| Autenticación: sin token → 401 | |
| Autorización: employee → endpoint hr → 403 | |

### Meta de cobertura
| Objetivo | Actual | Meta |
|----------|--------|------|
| Líneas | ~20% | ≥ 60% |

---

## Resumen de esfuerzo

| Fase | Horas estimadas | Depende de |
|------|----------------|------------|
| Fase 0 — Fundamentos | 3h | — |
| Fase 1 — Ampliación dominio | 4h | Fase 0 |
| Fase 2 — NATS EDD | 3h | Fase 1 |
| Fase 3 — APISIX | 1h | Fase 0 |
| Fase 4 — Expansión API | 4h | Fase 1 |
| Fase 5 — Documentación | 2h | Fase 1 |
| Fase 6 — Roles y seguridad | 2h | Fase 1 |
| Fase 7 — Tests | 2h | Fase 4 |
| **Total** | **~21h** | |

> Las 21h exceden el estimado de 10-15h del Brief porque se está construyendo un sistema más completo que el MVP mínimo. Se puede recortar alcance (ej. postergar Benefits, Reports, o simplificar Announcements) para ajustarse al tiempo.

---

## Orden sugerido de ejecución

1. **Fase 0** → git setup + CI/CD completo
2. **Fase 1** → Entidades + migraciones + eventos de dominio
3. **Fase 3** → APISIX (son archivos YAML, independiente del código)
4. **Fase 2** → NATS (una vez que existen eventos que publicar)
5. **Fase 4** → Endpoints (controller x controller, priorizando Employee y Documents)
6. **Fase 6** → Roles faltantes + seguridad
7. **Fase 5** → Documentación (se puede empezar en paralelo desde Fase 1)
8. **Fase 7** → Tests (se puede empezar desde Fase 4)

¿Empezamos por la **Fase 0** o prefieres ir directo a una fase específica?
