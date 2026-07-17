# Despliegue — PeoplePortal BackEnd

## Local con docker-compose

```bash
cd PeoplePortal-BackEnd

# Variable opcional (tiene valor por defecto en docker-compose)
POSTGRES_PASSWORD=YourStrong@Passw0rd

# Levantar PostgreSQL + NATS + API
docker-compose up -d

# Aplicar migraciones
docker-compose run --rm migrate

# Verificar
curl http://localhost:8081/health
```

Servicios levantados:

| Servicio | Puerto local |
|---|---|
| API | 8081 |
| PostgreSQL | 5432 |
| NATS | 4222 |

---

## Kubernetes (Docker Desktop)

```bash
# Namespace
kubectl apply -f k8s/namespace.yaml

# Config y secretos
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.yaml

# Crear imagePullSecret para GHCR (requiere gh CLI)
kubectl create secret docker-registry ghcr-secret \
  --docker-server=ghcr.io \
  --docker-username=bryanjosue17 \
  --docker-password="$(gh auth token)" \
  --namespace=peopleportal \
  --dry-run=client -o yaml | kubectl apply -f -

# Base de datos y mensajería
kubectl apply -f k8s/postgres.yaml
kubectl apply -f k8s/nats.yaml

# Migraciones (usa imagen GHCR ghcr.io/bryanjosue17/peopleportal-api-migrations)
kubectl apply -f k8s/migration-job.yaml
kubectl wait --for=condition=complete job/peopleportal-migrations \
  -n peopleportal --timeout=180s

# API (usa imagen GHCR ghcr.io/bryanjosue17/peopleportal-api)
kubectl apply -f k8s/api.yaml

# Verificar
kubectl rollout status deployment/peopleportal-api -n peopleportal
kubectl get pods -n peopleportal
```

---

## Variables de entorno requeridas

| Variable | Descripción | Ejemplo |
|---|---|---|
| `POSTGRES_PASSWORD` | Contraseña del usuario `postgres` en PostgreSQL | `YourStrong@Passw0rd` |
| `ConnectionStrings__DefaultConnection` | Connection string completa | `Host=postgres;Database=PeoplePortalDb;Username=postgres;Password=...` |
| `Keycloak__Authority` | URL del realm Keycloak | `http://keycloak:8080/realms/peopleportal` |
| `Keycloak__Audience` | Audience del JWT | `peopleportal-api` |
| `NATS__Url` | URL del servidor NATS | `nats://nats-service:4222` |

---

## CI/CD (GitHub Actions)

Pipeline: `.github/workflows/ci.yml`

| Job | Trigger | Acciones |
|---|---|---|
| `build-test` | Push a cualquier rama | restore → build → dotnet test (cobertura XPlat) → Codacy → Trivy |
| `docker` | Push a `develop` / `main` | docker build → push a GHCR (`ghcr.io/bryanjosue17/peopleportal-api` y `peopleportal-api-migrations`) |

Tags de imagen: `{branch}` y `{short-sha}` (7 chars del commit SHA).

> Deploy a K8s no automatizado — runners de GitHub no acceden al cluster Docker Desktop local.
