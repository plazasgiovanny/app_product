# Guardrails de Arquitectura

Este directorio contiene la documentación de todos los mecanismos automatizados que protegen la arquitectura limpia del proyecto. Un **guardrail** es una barrera técnica que impide —en tiempo de compilación, en pruebas o en el pipeline de CI— que la arquitectura sea violada accidentalmente.

## Índice

| # | Documento | Descripción |
|---|-----------|-------------|
| 1 | [Pruebas de Arquitectura](01-pruebas-arquitectura.md) | 10 reglas con NetArchTest que verifican las fronteras entre capas |
| 2 | [Pipeline CI](02-pipeline-ci.md) | GitHub Actions con 3 jobs: build, unit tests y arch tests |
| 3 | [Branch Protection](03-branch-protection.md) | Reglas de protección de ramas en GitHub y uso de `.editorconfig` |

## ¿Por qué son necesarios?

La arquitectura limpia (Clean Architecture) establece reglas de dependencia estrictas:

```
Api → Application → Domain
Infrastructure → Domain
Infrastructure → Application
```

Sin guardrails, cualquier desarrollador podría:

- Importar `ApplicationDbContext` directamente desde un controlador.
- Referenciar `Microsoft.EntityFrameworkCore` desde `Domain`.
- Crear entidades de dominio en la capa de infraestructura.

Los guardrails convierten estas reglas implícitas en **pruebas ejecutables** que fallan de forma explícita y descriptiva.

## Relación con el flujo de trabajo

```
Desarrollador escribe código
        ↓
Commit / PR
        ↓
GitHub Actions (ci.yml)
        ├── Job build          → Errores de compilación y warnings
        ├── Job unit-tests     → Pruebas de lógica de aplicación
        └── Job arch-tests     → Pruebas de fronteras arquitectónicas
                                  (NetArchTest – 10 reglas)
        ↓
Branch Protection bloquea el merge si algún job falla
```
