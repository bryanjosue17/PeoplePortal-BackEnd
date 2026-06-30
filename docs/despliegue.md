# Despliegue — PeoplePortal

## Local (docker-compose)

```bash
# Clonar repo
git clone <repo-url>
cd BackEnd

# Configurar variables
$env:SA_PASSWORD = "YourStrong@Passw0rd"

# Iniciar servicios
docker-compose up -d

# Correr migraciones
docker-compose run --rm migrate

# La API está en http://localhost:8081
# Health check: http://localhost:8081/health
```

## Kubernetes

```bash
# Namespace
kubectl apply -f k8s/namespace.yaml

# Config
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.yaml

# Base de datos
# SQL Server corre local en Windows (no en K8s)

# Migraciones
kubectl apply -f k8s/migration-job.yaml

# API
kubectl apply -f k8s/api-deployment.yaml

# APISIX
kubectl apply -f k8s/apisix.yaml

# Verificar
kubectl rollout status deployment/peopleportal-api -n peopleportal
```

## CI/CD (GitHub Actions)
El pipeline en `.github/workflows/ci.yml`:
1. `build-test`: restore, build, test, Codacy, Trivy
2. `docker`: build + push imágenes a GHCR
3. `deploy`: apply manifests + rollout status (solo en `main`)
