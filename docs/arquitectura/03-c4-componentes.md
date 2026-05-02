# C4 Nivel 3 — Diagrama de Componentes

## Descripción

Hace zoom dentro del contenedor **App Product Web API** y muestra sus componentes internos organizados en las **cuatro capas de Clean Architecture**. Este es el diagrama más importante para evidenciar la separación de responsabilidades.

```mermaid
graph TB
    Cliente["👤 Cliente HTTP"]

    subgraph Api ["📦 App_product.Api (Composition Root)"]
        Controller["ProductosController<br/>(GET/POST/PUT/DELETE /api/v1/productos)"]
        Middleware["ExceptionHandlingMiddleware<br/>(ProblemDetails RFC 7807)"]
        DI["DependencyInjection.cs<br/>(AddPersistence / AddApplicationServices<br/>/ AddPresentationServices)"]
        Swagger["Swagger / SwaggerOptions<br/>(Documentación OpenAPI)"]
    end

    subgraph Application ["📦 App_product.Application"]
        IService["IProductoService<br/>(Contrato del caso de uso)"]
        Service["ProductoService<br/>(Implementación del caso de uso)"]
        DTOs["DTOs<br/>(ProductoDto / CrearProductoDto<br/>/ ActualizarProductoDto)"]
        Mapper["ProductoMapper<br/>(ToDto / ToEntity / ApplyUpdate)"]
        Validators["Validators<br/>(CrearProductoDtoValidator<br/>/ ActualizarProductoDtoValidator)"]
    end

    subgraph Domain ["📦 App_product.Domain (Núcleo)"]
        Entidad["Producto<br/>(Entidad POCO)"]
        IRepo["IProductoRepository<br/>(Puerto del dominio)"]
        Excepciones["DomainException<br/>ProductoNoEncontradoException"]
    end

    subgraph Infrastructure ["📦 App_product.Infrastructure"]
        DbContext["ApplicationDbContext<br/>(EF Core DbContext)"]
        Config["ProductoConfiguration<br/>(IEntityTypeConfiguration)"]
        Repo["ProductoRepository<br/>(Adaptador EF Core)"]
    end

    DB[("SQL Server Express<br/>AppProductDb")]

    Cliente --> Middleware
    Middleware --> Controller
    Controller --> IService
    Controller --> Validators
    IService --> Service
    Service --> IRepo
    Service --> Mapper
    Service --> Excepciones
    Mapper --> DTOs
    DI -.->|"Registra"| Repo
    DI -.->|"Registra"| Service
    Repo --> DbContext
    DbContext --> Config
    Repo -.->|"Implementa"| IRepo
    DbContext --> DB
```

## Flujo de una solicitud POST /api/v1/productos

1. El cliente envía `POST /api/v1/productos` con un JSON `CrearProductoDto`.
2. `ExceptionHandlingMiddleware` envuelve la solicitud.
3. `ProductosController.Crear()` recibe el DTO y lo valida con `CrearProductoDtoValidator`.
4. Si es válido, llama a `IProductoService.CrearAsync(dto)`.
5. `ProductoService` convierte el DTO a entidad con `ProductoMapper.ToEntity()`.
6. `ProductoService` llama a `IProductoRepository.AgregarAsync(entidad)`.
7. `ProductoRepository` persiste con EF Core y retorna la entidad con Id asignado.
8. `ProductoService` mapea la entidad a `ProductoDto` y retorna al controller.
9. El controller retorna `201 Created` con el `ProductoDto` y el header `Location`.

## Regla de dependencias cumplida

```
Api  →  Application  →  Domain  (sin dependencias)
Api  →  Infrastructure  →  Domain
```

- `Domain` no importa ningún proyecto externo.
- `Application` no importa `Infrastructure`: nunca toca EF Core directamente.
- `Infrastructure` implementa el puerto `IProductoRepository` del Dominio.
- Solo `Api` conoce a todas las capas (Composition Root).
