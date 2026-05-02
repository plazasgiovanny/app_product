# App Product API

Backend REST para la gestión de productos, implementado en **ASP.NET Core 9** siguiendo **Clean Architecture**. Proyecto desarrollado para la Unidad 2 del módulo de Arquitectura de Aplicaciones Web.

## Tecnologías

| Tecnología | Versión | Rol |
|---|---|---|
| .NET / ASP.NET Core | 9.0 | Framework backend |
| Entity Framework Core | 9.0.4 | ORM (Code First) |
| SQL Server Express | Local | Base de datos relacional |
| FluentValidation | 11.3.1 | Validación de DTOs |
| Swashbuckle (Swagger) | 6.9.0 | Documentación OpenAPI |
| Asp.Versioning | 8.1.0 | Versionado de la API |
| xUnit + Moq + FluentAssertions | — | Pruebas unitarias |

## Estructura del proyecto

```
App_product/
├── src/
│   ├── App_product.Domain/          # Entidades, interfaces de repositorio, excepciones
│   ├── App_product.Application/     # Servicios, DTOs, validadores, mappers
│   ├── App_product.Infrastructure/  # DbContext, repositorios EF Core, migraciones
│   └── App_product.Api/             # Controllers, middleware, Composition Root
├── tests/
│   └── App_product.Application.Tests/  # Pruebas unitarias table-driven (26 tests)
└── docs/
    ├── arquitectura/   # Modelo C4 (4 niveles) + SOLID + patrones de diseño
    ├── endpoints/      # Documentación detallada de los 5 endpoints REST
    └── base-de-datos/  # Esquema, decisiones de diseño y flujo de migraciones
```

## Prerrequisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server Express corriendo en `.\SQLEXPRESS`
- `dotnet-ef` CLI:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Configuración

La cadena de conexión está en `src/App_product.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=AppProductDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Ajusta `Server` si tu instancia de SQL Server tiene un nombre diferente.

## Migraciones (Code First)

```bash
# Generar migración (solo si modificas el modelo)
dotnet ef migrations add NombreMigracion \
  --project src/App_product.Infrastructure \
  --startup-project src/App_product.Api \
  --output-dir Migrations

# Aplicar a SQL Server Express
dotnet ef database update \
  --project src/App_product.Infrastructure \
  --startup-project src/App_product.Api
```

> Al ejecutar en modo Development, la app aplica las migraciones pendientes automáticamente al iniciar.

## Ejecución

```bash
dotnet run --project src/App_product.Api
```

La API estará disponible en:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

## Swagger UI

Con la app corriendo en modo Development, acceder a:

```
https://localhost:5001/swagger
```

Permite ejecutar y probar todos los endpoints directamente desde el navegador.

## Pruebas unitarias

```bash
dotnet test
```

Ejecuta **26 pruebas unitarias** (0 errores) con patrón table-driven usando xUnit + Moq + FluentAssertions.

## Endpoints disponibles

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/v1/productos` | Listar todos los productos |
| GET | `/api/v1/productos/{id}` | Obtener un producto por Id |
| POST | `/api/v1/productos` | Crear un nuevo producto |
| PUT | `/api/v1/productos/{id}` | Actualizar un producto |
| DELETE | `/api/v1/productos/{id}` | Eliminar un producto |

Ver detalles completos en [`docs/endpoints/productos.md`](docs/endpoints/productos.md).

## Documentación

| Sección | Enlace |
|---|---|
| Arquitectura (C4 + SOLID + Patrones) | [`docs/arquitectura/`](docs/arquitectura/README.md) |
| Endpoints REST | [`docs/endpoints/`](docs/endpoints/README.md) |
| Base de datos y migraciones | [`docs/base-de-datos/`](docs/base-de-datos/README.md) |
