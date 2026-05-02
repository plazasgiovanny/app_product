# C4 Nivel 1 — Diagrama de Contexto del Sistema

## Descripción

El diagrama de contexto muestra el sistema **App Product API** en su entorno más amplio: quién lo usa y con qué sistemas externos se comunica. Es el nivel más alto de abstracción del modelo C4.

```mermaid
graph TB
    Usuario["👤 Usuario / Desarrollador<br/>(Cliente HTTP)<br/>Usa Swagger UI, Postman o Insomnia<br/>para consumir la API"]

    subgraph sistema [Sistema: App Product API]
        Api["🖥️ App Product API<br/>(ASP.NET Core 9 Web API)<br/>Expone servicios REST para<br/>gestión de productos (CRUD)"]
    end

    DB[("🗄️ SQL Server Express<br/>Base de datos relacional<br/>Almacena los productos")]

    Usuario -->|"HTTP/JSON<br/>(GET, POST, PUT, DELETE)"| Api
    Api -->|"TCP/SQL<br/>(EF Core 9)"| DB
```

## Elementos

| Elemento | Tipo | Descripción |
|---|---|---|
| Usuario / Desarrollador | Persona | Cualquier cliente HTTP que consume la API (Swagger, Postman, Insomnia, app frontend). |
| App Product API | Sistema de software | Backend REST que expone operaciones CRUD sobre productos. Es el sistema en foco. |
| SQL Server Express | Sistema externo | Motor de base de datos relacional local donde se persisten los datos. |

## Decisiones de diseño relevantes

- El sistema **no expone autenticación** en esta versión porque la actividad no la requiere. El endpoint de versionado `/api/v1/` facilita agregar seguridad en `/api/v2/` sin romper clientes existentes.
- La comunicación con SQL Server usa **Trusted Connection** (autenticación de Windows) para simplificar el entorno de desarrollo local.
