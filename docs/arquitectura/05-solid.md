# Principios SOLID en el Proyecto

Cada principio se documenta con una descripción, su evidencia concreta en el código y el archivo donde se puede verificar.

---

## S — Single Responsibility Principle (Responsabilidad Única)

> *Cada clase debe tener un único motivo para cambiar.*

| Clase | Única responsabilidad | Archivo |
|---|---|---|
| `Producto` | Representar los datos del agregado de negocio | `src/App_product.Domain/Entities/Producto.cs` |
| `ProductoService` | Orquestar los casos de uso CRUD | `src/App_product.Application/Services/ProductoService.cs` |
| `ProductoRepository` | Persistir y recuperar Productos de SQL Server | `src/App_product.Infrastructure/Repositories/ProductoRepository.cs` |
| `ProductosController` | Exponer los endpoints HTTP y traducir entre HTTP y DTOs | `src/App_product.Api/Controllers/V1/ProductosController.cs` |
| `ProductoConfiguration` | Configurar el mapeo EF Core de la entidad Producto | `src/App_product.Infrastructure/Persistence/Configurations/ProductoConfiguration.cs` |
| `ExceptionHandlingMiddleware` | Capturar excepciones y traducirlas a respuestas HTTP | `src/App_product.Api/Middleware/ExceptionHandlingMiddleware.cs` |
| `CrearProductoDtoValidator` | Validar los datos del DTO de creación | `src/App_product.Application/Validators/CrearProductoDtoValidator.cs` |

---

## O — Open/Closed Principle (Abierto/Cerrado)

> *Las entidades de software deben estar abiertas para extensión y cerradas para modificación.*

**Evidencia 1:** `ApplicationDbContext` aplica todas las configuraciones EF del ensamblado vía `ApplyConfigurationsFromAssembly`. Para agregar una nueva entidad, se crea una nueva clase `IEntityTypeConfiguration<T>` sin modificar `ApplicationDbContext`.

```csharp
// src/App_product.Infrastructure/Persistence/ApplicationDbContext.cs
modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
```

**Evidencia 2:** Para agregar un nuevo medio de persistencia (ej. MongoDB), basta con crear `MongoProductoRepository : IProductoRepository` y registrarlo en `DependencyInjection.cs`. `ProductoService` no se modifica.

**Evidencia 3:** `ExceptionHandlingMiddleware` usa una expresión `switch` que puede extenderse con nuevos tipos de excepción sin tocar los casos existentes.

---

## L — Liskov Substitution Principle (Sustitución de Liskov)

> *Los subtipos deben poder sustituir a sus tipos base sin alterar la corrección del programa.*

**Evidencia:** En las pruebas unitarias (`ProductoServiceTests`), se sustituye `ProductoRepository` (la implementación real con EF Core) por `Mock<IProductoRepository>` (implementación en memoria). `ProductoService` funciona exactamente igual con ambas implementaciones.

```csharp
// tests/App_product.Application.Tests/Services/ProductoServiceTests.cs
_repoMock = new Mock<IProductoRepository>();  // sustituye a ProductoRepository
_servicio = new ProductoService(_repoMock.Object);
```

Los 26 tests pasan con el mock, confirmando que el contrato de `IProductoRepository` se respeta.

---

## I — Interface Segregation Principle (Segregación de Interfaces)

> *Los clientes no deben depender de interfaces que no usan.*

**Evidencia 1:** `IProductoRepository` expone exactamente los métodos necesarios para el agregado Producto. No hay una interfaz genérica `IRepository<T>` que forzaría implementar métodos no necesarios.

**Evidencia 2:** `IProductoService` expone solo las operaciones de caso de uso CRUD. El controlador no tiene acceso a lógica de infraestructura.

**Evidencia 3:** Los validadores `IValidator<CrearProductoDto>` e `IValidator<ActualizarProductoDto>` son interfaces separadas, inyectadas independientemente. El controlador solo inyecta los que necesita.

---

## D — Dependency Inversion Principle (Inversión de Dependencias)

> *Los módulos de alto nivel no deben depender de módulos de bajo nivel. Ambos deben depender de abstracciones.*

**Evidencia 1 (cadena de DIP):**

```
ProductosController  →  IProductoService  (abstracción)
ProductoService      →  IProductoRepository  (abstracción, definida en Domain)
ProductoRepository   →  ApplicationDbContext  (implementación EF Core)
```

Nunca: `ProductosController → ProductoService` ni `ProductoService → ProductoRepository`.

**Evidencia 2:** `IProductoRepository` vive en la capa `Domain`, lo que garantiza que la capa `Application` nunca necesita referenciar `Infrastructure`. Esto es verificable en el `.csproj`:

```xml
<!-- App_product.Application.csproj — solo referencia Domain, NUNCA Infrastructure -->
<ProjectReference Include="..\App_product.Domain\App_product.Domain.csproj" />
```

**Evidencia 3:** El único lugar donde se "conectan" interfaces con implementaciones es el `DependencyInjection.cs` de la capa Api (Composition Root):

```csharp
// src/App_product.Api/DependencyInjection.cs
services.AddScoped<IProductoRepository, ProductoRepository>();
services.AddScoped<IProductoService, ProductoService>();
```
