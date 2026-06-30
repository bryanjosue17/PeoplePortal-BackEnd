# Seguridad — PeoplePortal

## Mapeo OWASP Top 10

| OWASP | Riesgo | Mitigación en PeoplePortal |
|-------|--------|---------------------------|
| A01 Broken Access Control | Usuarios acceden a recursos no autorizados | Keycloak con roles (employee, jefe_inmediato, hr, nomina, admin); policies por endpoint; validación de employeeId vs JWT sub |
| A02 Cryptographic Failures | Datos sensibles expuestos | JWT con Keycloak; HTTPS en producción; RequireHttpsMetadata activo fuera de Development; secrets en K8s Secrets / env vars |
| A03 Injection | SQL injection | EF Core parametriza todas las consultas; no raw SQL |
| A04 Insecure Design | Lógica de negocio insegura | Domain entities con factory methods que validan invariantes; CQRS separa commands de queries; validación con FluentValidation |
| A05 Security Misconfiguration | Configuraciones inseguras | APISIX como único punto de entrada; CORS configurado; environment-specific configs |
| A06 Vulnerable Components | Dependencias con vulnerabilidades | Trivy scan en CI/CD; NuGet packages actualizados |
| A07 Auth Failures | Autenticación débil | Keycloak con PKCE; JWT Bearer token; validación de audience + issuer + signing key |
| A08 Integrity Failures | Integridad de datos comprometida | EF Core migrations versionadas; Conventional Commits; branch protection |
| A09 Logging Failures | Falta de monitoreo | NATS eventos de dominio; health checks; logs estructurados |
| A10 SSRF | Server-side request forgery | No se hacen request a URLs externas desde el backend |

## Secretos
- Nunca en el repositorio
- Siempre vía environment variables o K8s Secrets
- .env ignorado por .gitignore
- Connection strings sin hardcode (lanzan excepción si no hay env var)
