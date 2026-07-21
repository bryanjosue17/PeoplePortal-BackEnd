# Endpoints — PeoplePortal BackEnd API

Base URL (local): `http://localhost:30090/api` (vía APISIX) o `http://localhost:8081/api` (directo)

---

## Empleados

| Método | Ruta | Policy | Descripción |
|---|---|---|---|
| `GET` | `/api/employees/me` | EmployeePolicy | Obtener perfil propio |
| `PUT` | `/api/employees/me` | EmployeePolicy | Actualizar datos personales |
| `GET` | `/api/hr/employees` | HrPolicy | Listar todos los empleados |
| `GET` | `/api/hr/employees/{id}` | HrPolicy | Detalle de un empleado |
| `POST` | `/api/hr/employees` | HrPolicy | Crear empleado |
| `PUT` | `/api/hr/employees/{id}` | HrPolicy | Actualizar empleado |

---

## Solicitudes (HrRequests)

| Método | Ruta | Policy | Descripción |
|---|---|---|---|
| `POST` | `/api/requests/vacation` | EmployeePolicy | Crear solicitud de vacaciones |
| `POST` | `/api/requests/certificate` | EmployeePolicy | Crear solicitud de constancia |
| `POST` | `/api/requests/voucher` | EmployeePolicy | Crear solicitud de voucher de pago |
| `POST` | `/api/requests/{id}/cancel` | EmployeePolicy | Cancelar solicitud propia |
| `GET` | `/api/requests/me` | EmployeePolicy | Listar mis solicitudes |
| `GET` | `/api/manager/requests` | ManagerPolicy | Listar solicitudes del equipo propio (ReviewedBy = managerId) |
| `PATCH` | `/api/manager/requests/{id}/status` | ManagerPolicy | Aprobar/rechazar como jefe inmediato |
| `GET` | `/api/hr/requests` | HrPolicy | Listar todas las solicitudes |
| `PATCH` | `/api/hr/requests/{id}/status` | HrPolicy | Actualizar estado como RRHH (acepta `hrComment`) |

---

## Documentos

| Método | Ruta | Policy | Descripción |
|---|---|---|---|
| `GET` | `/api/documents/me` | EmployeePolicy | Mis documentos |
| `GET` | `/api/hr/documents` | HrPolicy | Todos los documentos |
| `POST` | `/api/hr/documents` | HrPolicy | Subir documento a un empleado |
| `PATCH` | `/api/hr/documents/{id}/status` | HrPolicy | Actualizar estado de documento |

---

## Comunicados

| Método | Ruta | Policy | Descripción |
|---|---|---|---|
| `GET` | `/api/announcements` | EmployeePolicy | Comunicados activos |
| `POST` | `/api/hr/announcements` | HrPolicy | Publicar comunicado |
| `PATCH` | `/api/hr/announcements/{id}/deactivate` | HrPolicy | Desactivar comunicado |

---

## Beneficios

| Método | Ruta | Policy | Descripción |
|---|---|---|---|
| `GET` | `/api/benefits` | EmployeePolicy | Catálogo de beneficios activos |
| `GET` | `/api/hr/benefits` | HrPolicy | Todos los beneficios (incluso inactivos) |
| `POST` | `/api/hr/benefits` | HrPolicy | Crear beneficio |
| `PUT` | `/api/hr/benefits/{id}` | HrPolicy | Actualizar beneficio |
| `DELETE` | `/api/hr/benefits/{id}` | HrPolicy | Desactivar beneficio (soft-delete) |
| `PATCH` | `/api/hr/benefits/{id}/activate` | HrPolicy | Reactivar beneficio inactivo |

---

## Dashboard

| Método | Ruta | Policy | Descripción |
|---|---|---|---|
| `GET` | `/api/dashboard` | EmployeePolicy | Perfil + solicitudes + documentos + comunicados + beneficios |

---

## Nómina (Comprobantes de Pago)

| Método | Ruta | Policy / Roles | Descripción |
|---|---|---|---|
| `GET` | `/api/nomina/me` | EmployeePolicy | Mis comprobantes de nómina |
| `GET` | `/api/hr/nomina` | nomina, hr, admin | Listar todos los registros de nómina |
| `POST` | `/api/hr/nomina` | nomina, hr, admin | Crear registro (tipo: ComprobanteDepago, Bonificacion, etc.) |
| `PATCH` | `/api/hr/nomina/{id}/upload` | nomina, hr, admin | Adjuntar URL de archivo al registro |

---

## Gestión de Usuarios (Keycloak Admin)

> Solo accesible con rol `hr` o `admin`. Proxy seguro a la Keycloak Admin REST API.

| Método | Ruta | Policy | Descripción |
|---|---|---|---|
| `GET` | `/api/hr/users` | HrPolicy | Listar usuarios de Keycloak con empleado vinculado y roles |
| `GET` | `/api/hr/users/roles` | HrPolicy | Listar roles del realm disponibles |
| `GET` | `/api/hr/users/{id}/roles` | HrPolicy | Roles actuales de un usuario |
| `POST` | `/api/hr/users` | HrPolicy | Crear usuario en Keycloak |
| `PATCH` | `/api/hr/users/{id}/enabled` | HrPolicy | Habilitar / deshabilitar usuario |
| `PUT` | `/api/hr/users/{id}/roles` | HrPolicy | Reemplazar roles del usuario |
| `POST` | `/api/hr/users/{id}/reset-password` | HrPolicy | Restablecer contraseña |

---

## Reportes

| Método | Ruta | Policy | Descripción |
|---|---|---|---|
| `GET` | `/api/hr/reports/requests-by-status` | HrPolicy | Solicitudes agrupadas por estado |
| `GET` | `/api/hr/reports/requests-by-type` | HrPolicy | Solicitudes agrupadas por tipo |
| `GET` | `/api/hr/reports/requests-over-time` | HrPolicy | Solicitudes en el tiempo |
| `GET` | `/api/hr/reports/active-employees` | HrPolicy | Empleados activos |
| `GET` | `/api/hr/reports/pending-documents` | HrPolicy | Documentos pendientes |

---

## Health

| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| `GET` | `/health` | Público | Health check del servicio |

---

## Políticas de autorización

| Policy | Rol requerido (Keycloak) |
|---|---|
| `EmployeePolicy` | `employee` |
| `ManagerPolicy` | `jefe_inmediato` |
| `HrPolicy` | `hr` |
| `NominaPolicy` | `nomina` |
| `AdminPolicy` | `admin` |
