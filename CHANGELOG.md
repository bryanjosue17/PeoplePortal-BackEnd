# Changelog

## [0.3.0] — 2026-07-03

### Added
- **Vouchers API completa**: `IVoucherRepository`, `VoucherRepository`, mapeo EF Core de la entidad `Voucher`
  - `GET /api/vouchers/me` (EmployeePolicy) — consulta de vouchers propios
  - `GET /api/hr/vouchers` (roles: nomina, hr, admin) — listado completo
  - `POST /api/hr/vouchers` — crear voucher para un empleado
  - `PATCH /api/hr/vouchers/{id}/upload` — adjuntar URL de archivo al voucher
- **Endpoint Jefe Inmediato** `GET /api/manager/requests` — lista solicitudes asignadas al manager (usa `ReviewedBy = managerId`)
- **Deactivate Announcement**: `PATCH /api/hr/announcements/{id}/deactivate` (HrPolicy) con `DeactivateAnnouncementCommand`
- `GetMyTeamRequestsQuery` + handler para el nuevo endpoint de manager
- `CreateVoucherForEmployeeCommand`, `UploadVoucherFileCommand` + handlers
- `GetMyVouchersQuery`, `GetAllVouchersQuery` + handlers
- `IVoucherRepository` registrado en DI

### Changed
- `IHrRequestRepository`: nuevo método `GetByManagerIdAsync(managerId)` filtrando por campo `reviewed_by`
- `HrRequestRepository`: implementa `GetByManagerIdAsync`
- `PeoplePortalDbContext`: mapeo completo de la entidad `Voucher` (tabla `vouchers`, índices)
- `AnnouncementsController`: agrega ruta `PATCH ~/api/hr/announcements/{id:guid}/deactivate`
- `ManagerController`: agrega `GET` para listar solicitudes del equipo



### Added
- 5 nuevas entidades de dominio: Employee, Document, Voucher, Announcement, Benefit
- Enums: EmployeeStatus, ContractType, DocumentStatus, VoucherStatus, AnnouncementType
- Migración completa con 6 tablas (employees, hr_requests, documents, vouchers, announcements, benefits)
- Repositorios para todas las entidades (interfaces + implementaciones)
- Módulo Employee: perfil, CRUD, actualización de datos personales y laborales
- Módulo Document: subida, descarga, gestión de estados por RRHH
- Módulo Announcement: comunicados activos, publicación por RRHH
- Módulo Benefit: catálogo de beneficios consultable
- Dashboard: endpoint que agrega perfil + solicitudes + documentos + comunicados + beneficios
- Solicitudes expandidas: voucher request, cancelación por owner
- Flujo de aprobación por jefe inmediato (ManagerController + ApproveByManager)
- Roles completos: employee, jefe_inmediato, hr, nomina, admin + policies
- NATS JetStream: IEventBus, NatsEventBus, EventConsumerService, eventos hr.request.submitted y hr.request.approved
- APISIX: configuración standalone con rutas protegidas vía openid-connect
- docker-compose extendido con NATS + etcd + APISIX
- Manifiesto K8s para APISIX
- Documentación /docs: arquitectura C4, flujos sequence, ER diagrama, despliegue, seguridad OWASP
- Catálogo de prompts en /docs/prompts/
- FluentValidation: 5 validadores + ValidationBehavior pipeline
- Exception handling middleware
- CORS configurado para frontend local
- 30 nuevos tests unitarios (37 total, todos verdes)

### Changed
- HrRequest: nuevos tipos (Voucher, Permission, DataUpdate, Other), nuevos estados (InReview, Cancelled), método Cancel
- CreateVacation ahora incluye ManagerId para flujo de aprobación
- CI/CD pipeline: jobs de Docker build+push a GHCR + deploy a K8s
- DbContextFactory: ya no usa connection string hardcodeada
- Program.cs: RequireHttpsMetadata condicional, deduplicación de roles, 5 policies

### Fixed
- ArgumentException sin nameof en HrRequest.CreateVacation
- Test HrRequestTests incompleto (faltaba assertion de HrComment)
- Posibles roles duplicados en JWT validation

## [0.2.1] — 2026-06-30

### Added
- `.gitmessage` template para Conventional Commits
- **Benefits CRUD**: `HrBenefitsController` con GET/POST/PUT/DELETE protegido con HrPolicy
- **Reports API**: `ReportsController` con 5 endpoints (requests-by-status, requests-by-type, requests-over-time, active-employees, pending-documents)
- **Integration Tests**: nuevo proyecto `PeoplePortal.IntegrationTests` con 5 tests de dominio
- `CreateBenefitCommand`, `UpdateBenefitCommand`, `DeactivateBenefitCommand` + handlers
- `GetAllBenefitsQuery` + handler (incluye beneficios inactivos)
- 5 report queries con handlers y DTOs

### Changed
- Frontend CIs replican estructura del backend: `build-test` + `docker` (build+push GHCR)
- Ambos frontends ahora reportan cobertura a Codacy via `@vitest/coverage-v8`
- `.sln` incluye `PeoplePortal.IntegrationTests`
- Total tests: 86 (49 backend + 19 colaborador + 18 RRHH)
