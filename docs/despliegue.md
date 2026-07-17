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

# Base de datos y mensajería
kubectl apply -f k8s/postgres.yaml
kubectl apply -f k8s/nats.yaml

# Migraciones (esperar a que completen)
kubectl apply -f k8s/migration-job.yaml
kubectl wait --for=condition=complete job/peopleportal-migrations \
  -n peopleportal --timeout=180s

# API
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
| `docker` | Push a `develop` / `main` | docker build → push a GHCR |

Tags de imagen: `{branch}-{short-sha}` y `latest` en `main`.

> Deploy a K8s no automatizado — runners de GitHub no acceden al cluster Docker Desktop local.
