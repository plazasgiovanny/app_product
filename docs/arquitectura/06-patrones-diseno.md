# Patrones de Diseño Aplicados

---

## 1. Repository (DDD)

**Descripción:** Abstrae el acceso a datos detrás de una interfaz que simula una colección en memoria. El cliente del repositorio no sabe si los datos vienen de SQL Server, MongoDB o un archivo.

**Ubicación en el proyecto:**
- Interfaz (contrato): `src/App_product.Domain/Repositories/IProductoRepository.cs`
- Implementación (adaptador EF Core): `src/App_product.Infrastructure/Repositories/ProductoRepository.cs`

**Por qué la interfaz vive en Domain:** En DDD (Evans, 2003), el Repository es un building block del modelo de dominio. Onion Architecture lo prescribe explícitamente. Esto evita que `Application` deba referenciar `Infrastructure`.

---

## 2. Composition Root

**Descripción:** Existe un único lugar en la aplicación donde se ensamblan todas las dependencias (implementaciones concretas se vinculan a sus interfaces). Evita referencias circulares y hace el grafo de objetos trazable desde un solo punto.

**Ubicación:** `src/App_product.Api/DependencyInjection.cs` — único archivo donde aparece `services.AddScoped<IInterface, Implementacion>()` para las capas de dominio e infraestructura.

```csharp
services.AddScoped<IProductoRepository, ProductoRepository>(); // solo aquí
services.AddScoped<IProductoService, ProductoService>();       // solo aquí
```

---

## 3. Ports & Adapters (Arquitectura Hexagonal)

**Descripción:** El núcleo de la aplicación define **puertos** (interfaces) que describen cómo interactúa con el exterior. Los **adaptadores** implementan esos puertos en cada tecnología concreta.

**Puertos del proyecto:**
- `IProductoRepository` (puerto de persistencia) → adaptador: `ProductoRepository` (EF Core + SQL Server)
- `IProductoService` (puerto de caso de uso) → adaptador: `ProductosController` (HTTP/REST)

---

## 4. DTO (Data Transfer Object)

**Descripción:** Objetos planos que transportan datos entre capas o entre cliente y servidor. Aíslan la entidad de dominio del contrato externo, permitiendo que evolucionen independientemente.

**Ubicación:** `src/App_product.Application/DTOs/`
- `ProductoDto` — respuesta al cliente
- `CrearProductoDto` — cuerpo del POST
- `ActualizarProductoDto` — cuerpo del PUT

---

## 5. Mapper estático (Extension Methods)

**Descripción:** Métodos de extensión para la conversión explícita entre entidades y DTOs. Alternativa a AutoMapper que mantiene el mapeo visible, sin magia, y totalmente testeable.

**Ubicación:** `src/App_product.Application/Mapping/ProductoMapper.cs`

```csharp
producto.ToDto()           // Producto → ProductoDto
dto.ToEntity()             // CrearProductoDto → Producto
producto.ApplyUpdate(dto)  // ActualizarProductoDto aplicado a Producto existente
```

---

## 6. Chain of Responsibility (Pipeline de Middleware)

**Descripción:** ASP.NET Core implementa el pipeline de solicitudes como una cadena de responsabilidad. `ExceptionHandlingMiddleware` es el primer eslabón, capturando cualquier excepción de los eslabones siguientes.

**Ubicación:** `src/App_product.Api/Middleware/ExceptionHandlingMiddleware.cs`

---

## 7. Options Pattern

**Descripción:** Configuración fuertemente tipada para aspectos transversales. `SwaggerOptions` implementa `IConfigureNamedOptions<SwaggerGenOptions>` para generar un documento Swagger por cada versión de API de forma automática.

**Ubicación:** `src/App_product.Api/Configuration/SwaggerOptions.cs`

---

## 8. Specification (implícita vía FluentValidation)

**Descripción:** Las reglas de validación se expresan como especificaciones declarativas, encadenables y testeables de forma independiente al pipeline HTTP.

**Ubicación:** `src/App_product.Application/Validators/`

Cada validator es una clase aislada (SRP) que puede extenderse sin modificar las existentes (OCP).
