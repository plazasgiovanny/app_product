# C4 Nivel 4 — Diagrama de Código

## Justificación

Se incluye este nivel porque el proyecto es pequeño (una sola entidad) y permite evidenciar con precisión las relaciones entre clases y las interfaces clave que sostienen la arquitectura.

## Diagrama de clases simplificado

```mermaid
classDiagram
    class Producto {
        +int Id
        +string Nombre
        +string? Descripcion
        +decimal Precio
    }

    class IProductoRepository {
        <<interface>>
        +ObtenerTodosAsync() Task~IEnumerable~Producto~~
        +ObtenerPorIdAsync(int id) Task~Producto?~
        +AgregarAsync(Producto p) Task~Producto~
        +ActualizarAsync(Producto p) Task
        +EliminarAsync(int id) Task
        +ExisteAsync(int id) Task~bool~
    }

    class ProductoRepository {
        -ApplicationDbContext _contexto
        +ObtenerTodosAsync()
        +ObtenerPorIdAsync(int id)
        +AgregarAsync(Producto p)
        +ActualizarAsync(Producto p)
        +EliminarAsync(int id)
        +ExisteAsync(int id)
    }

    class IProductoService {
        <<interface>>
        +ObtenerTodosAsync() Task~IEnumerable~ProductoDto~~
        +ObtenerPorIdAsync(int id) Task~ProductoDto~
        +CrearAsync(CrearProductoDto dto) Task~ProductoDto~
        +ActualizarAsync(int id, ActualizarProductoDto dto) Task
        +EliminarAsync(int id) Task
    }

    class ProductoService {
        -IProductoRepository _repositorio
        +ObtenerTodosAsync()
        +ObtenerPorIdAsync(int id)
        +CrearAsync(CrearProductoDto dto)
        +ActualizarAsync(int id, ActualizarProductoDto dto)
        +EliminarAsync(int id)
    }

    class ApplicationDbContext {
        +DbSet~Producto~ Productos
        #OnModelCreating(ModelBuilder)
    }

    class ProductoConfiguration {
        +Configure(EntityTypeBuilder~Producto~)
    }

    class ProductosController {
        -IProductoService _servicio
        -IValidator~CrearProductoDto~ _validadorCrear
        -IValidator~ActualizarProductoDto~ _validadorActualizar
        +ObtenerTodos() IActionResult
        +ObtenerPorId(int id) IActionResult
        +Crear(CrearProductoDto dto) IActionResult
        +Actualizar(int id, ActualizarProductoDto dto) IActionResult
        +Eliminar(int id) IActionResult
    }

    ProductoRepository ..|> IProductoRepository : implementa
    ProductoService ..|> IProductoService : implementa
    ProductoService --> IProductoRepository : usa (DIP)
    ProductosController --> IProductoService : usa (DIP)
    ProductoRepository --> ApplicationDbContext : usa
    ApplicationDbContext --> ProductoConfiguration : aplica
    ApplicationDbContext --> Producto : DbSet
    ProductoService --> Producto : mapea
```

## Notas sobre el diseño

- Las flechas de uso (`-->`) siempre apuntan hacia abstracciones (interfaces) o el dominio.
- `ProductosController` nunca referencia `ProductoService` directamente: solo conoce `IProductoService`.
- `ProductoService` nunca referencia `ProductoRepository`: solo conoce `IProductoRepository`.
- Esta cadena de inversiones es la materialización del **Principio de Inversión de Dependencias (DIP)**.
