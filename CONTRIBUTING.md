# Contributing

## Conventional Commits

Este proyecto sigue [Conventional Commits](https://www.conventionalcommits.org/):

```
<tipo>(<alcance>): <descripción>
```

### Tipos
| Tipo | Uso |
|------|-----|
| `feat` | Nueva funcionalidad |
| `fix` | Corrección de bug |
| `refactor` | Cambio de código que no agrega funcionalidad ni corrige bug |
| `test` | Agregar o modificar tests |
| `docs` | Cambios en documentación |
| `chore` | Tareas de mantenimiento, CI/CD, configuraciones |
| `style` | Formato, estilo, linting (no cambia lógica) |
| `perf` | Mejora de rendimiento |

### Ejemplos
```
feat(api): add employee profile endpoint
fix(db): correct migration column type
docs(readme): update setup instructions
chore(ci): add docker build step to pipeline
```

## Branch strategy
- `main` — producción
- `develop` — integración
- `feat/<nombre>` — features nuevas desde `develop`
- `fix/<nombre>` — correcciones desde `develop`

## PR requirements
- CI/CD pipeline verde
- Code review de al menos 1 persona
- Sin issues Críticos ni Altos en Codacy
