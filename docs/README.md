# Documentación del Proyecto

Índice maestro de toda la documentación de **App Product API**.

## Secciones

### Arquitectura [`arquitectura/`](arquitectura/README.md)

Documenta la arquitectura del proyecto usando el modelo C4 (4 niveles) y los principios aplicados.

| Documento | Descripción |
|---|---|
| [01-c4-contexto.md](arquitectura/01-c4-contexto.md) | Diagrama de Contexto — actores y sistemas externos |
| [02-c4-contenedores.md](arquitectura/02-c4-contenedores.md) | Diagrama de Contenedores — procesos y almacenes |
| [03-c4-componentes.md](arquitectura/03-c4-componentes.md) | Diagrama de Componentes — estructura interna de la API |
| [04-c4-codigo.md](arquitectura/04-c4-codigo.md) | Diagrama de Código — diagrama de clases clave |
| [05-solid.md](arquitectura/05-solid.md) | Principios SOLID con evidencia en el código |
| [06-patrones-diseno.md](arquitectura/06-patrones-diseno.md) | Patrones de diseño aplicados |

### Endpoints [`endpoints/`](endpoints/README.md)

Documentación detallada de los servicios REST.

| Documento | Descripción |
|---|---|
| [README.md](endpoints/README.md) | Convenciones REST, herramientas de prueba, manejo de errores |
| [productos.md](endpoints/productos.md) | Los 5 endpoints CRUD con payloads JSON y códigos HTTP |

### Base de Datos [`base-de-datos/`](base-de-datos/README.md)

Información sobre el motor de BD, el esquema y las migraciones.

| Documento | Descripción |
|---|---|
| [README.md](base-de-datos/README.md) | Motor, servidor, patrón Code First |
| [esquema.md](base-de-datos/esquema.md) | Tabla Productos — columnas, tipos y decisiones |
| [migraciones.md](base-de-datos/migraciones.md) | Flujo completo de migraciones EF Core |

### Guardrails de Arquitectura [`arquitectura/guardrails/`](arquitectura/guardrails/README.md)

Mecanismos automatizados que protegen las fronteras de la arquitectura limpia y garantizan que el proyecto se mantenga íntegro a medida que crece.

| Documento | Descripción |
|---|---|
| [README.md](arquitectura/guardrails/README.md) | Visión general de los guardrails y su relación con el flujo de trabajo |
| [01-pruebas-arquitectura.md](arquitectura/guardrails/01-pruebas-arquitectura.md) | 10 reglas NetArchTest que verifican las fronteras entre capas |
| [02-pipeline-ci.md](arquitectura/guardrails/02-pipeline-ci.md) | Pipeline de GitHub Actions con 3 jobs: build, unit tests y arch tests |
| [03-branch-protection.md](arquitectura/guardrails/03-branch-protection.md) | Branch Protection Rules, `.editorconfig` y Roslyn Analyzers |
