# C4 Nivel 2 — Diagrama de Contenedores

## Descripción

El diagrama de contenedores hace zoom dentro del sistema "App Product API" y muestra los **contenedores desplegables** (procesos o almacenes de datos ejecutables de forma independiente).

```mermaid
graph TB
    Usuario["👤 Cliente HTTP<br/>(Swagger UI / Postman / Insomnia)"]

    subgraph SistemaApp [Sistema: App Product API]
        WebApi["🖥️ App Product Web API<br/>Tecnología: ASP.NET Core 9<br/>Puerto: 5001 HTTPS / 5000 HTTP<br/><br/>Expone endpoints REST versionados<br/>bajo /api/v1/productos<br/>Documentado con Swagger UI en /swagger"]
    end

    subgraph Almacenamiento [Almacenamiento]
        SqlServer[("🗄️ SQL Server Express<br/>Servidor: .\\SQLEXPRESS<br/>Base de datos: AppProductDb<br/>Tabla: Productos<br/><br/>Creada y gestionada<br/>por EF Core Code First")]
    end

    Usuario -->|"HTTPS/JSON<br/>REST"| WebApi
    WebApi -->|"TCP Puerto 1433<br/>EF Core 9.0 + SqlClient"| SqlServer
```

## Responsabilidades por contenedor

### App Product Web API

- Recibe y valida solicitudes HTTP.
- Aplica las reglas de negocio a través de la capa de Aplicación.
- Gestiona errores con `ExceptionHandlingMiddleware` (ProblemDetails RFC 7807).
- Persiste y recupera datos a través de EF Core.
- Expone documentación interactiva en `/swagger` (solo en Development).

### SQL Server Express

- Almacena los registros de productos.
- La tabla `Productos` fue creada por la migración `InitialCreate` de EF Core (Code First).
- No requiere scripts SQL manuales. Todo se gestiona desde el modelo de dominio.

## Tecnologías por contenedor

| Contenedor | Lenguaje | Framework | Versión |
|---|---|---|---|
| Web API | C# | ASP.NET Core | 9.0 |
| Base de datos | T-SQL | SQL Server Express | Local |
