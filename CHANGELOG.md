# Changelog

## [0.2.0] — 2026-06-29

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

### Changed
- Frontend CIs replican estructura del backend: `build-test` + `docker` (build+push GHCR)
- Ambos frontends ahora reportan cobertura a Codacy via `@vitest/coverage-v8`
