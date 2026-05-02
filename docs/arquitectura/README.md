# Documentación de Arquitectura

Este directorio describe la arquitectura del proyecto **App Product API** usando el **modelo C4** y documenta los principios y patrones aplicados.

## Índice

| Documento | Descripción |
|---|---|
| [01-c4-contexto.md](01-c4-contexto.md) | C4 Nivel 1 — Contexto del sistema (alcance y actores externos) |
| [02-c4-contenedores.md](02-c4-contenedores.md) | C4 Nivel 2 — Contenedores (procesos y almacenes desplegables) |
| [03-c4-componentes.md](03-c4-componentes.md) | C4 Nivel 3 — Componentes (estructura interna de la Web API) |
| [04-c4-codigo.md](04-c4-codigo.md) | C4 Nivel 4 — Código (diagrama de clases clave) |
| [05-solid.md](05-solid.md) | Aplicación de los 5 principios SOLID con evidencia en el código |
| [06-patrones-diseno.md](06-patrones-diseno.md) | Patrones de diseño aplicados con ubicación y justificación |
| [guardrails/](guardrails/README.md) | Guardrails: pruebas de arquitectura, pipeline CI y branch protection |

## Arquitectura en una línea

> **Clean Architecture** (Uncle Bob) sobre **ASP.NET Core 9**, con influencias de **Onion Architecture** (Palermo) y **DDD** (Evans), organizada en cuatro capas de proyectos con dependencias estrictamente unidireccionales hacia el Dominio.

## Relación con el documento U1+U2

Esta arquitectura cumple directamente con los principios del documento `Resumen_Lecturas_Fundamentales_U1_U2.md`:

- **§3.2**: separación de capas, dominio aislado de persistencia ✓
- **§3.2**: versionado desde etapas tempranas (`/api/v1/`) ✓
- **§3.2**: JSON como formato predeterminado ✓
- **§3.2**: códigos HTTP correctos y consistentes ✓
- **§4.1**: dominio, aplicación, interfaces, infraestructura separadas ✓
- **§4.1**: SRP aplicado por módulo ✓
- **§4.2**: monolito modular en lugar de microservicios (proyecto pequeño) ✓
