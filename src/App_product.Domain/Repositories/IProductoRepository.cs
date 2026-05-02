using App_product.Domain.Entities;

namespace App_product.Domain.Repositories;

/// <summary>
/// Puerto del dominio que define el contrato de persistencia para el agregado Producto.
/// Vive en la capa Domain porque en DDD el Repositorio es un building block del modelo
/// de dominio (Evans, 2003) y Onion Architecture lo prescribe explícitamente.
/// La implementación concreta (EF Core) reside en la capa Infrastructure.
/// </summary>
public interface IProductoRepository
{
    /// <summary>Obtiene todos los productos existentes.</summary>
    Task<IEnumerable<Producto>> ObtenerTodosAsync();

    /// <summary>
    /// Obtiene un producto por su identificador.
    /// Retorna null si no existe.
    /// </summary>
    Task<Producto?> ObtenerPorIdAsync(int id);

    /// <summary>Persiste un nuevo producto y retorna la entidad con el Id asignado.</summary>
    Task<Producto> AgregarAsync(Producto producto);

    /// <summary>Actualiza los datos de un producto existente.</summary>
    Task ActualizarAsync(Producto producto);

    /// <summary>Elimina el producto con el identificador dado.</summary>
    Task EliminarAsync(int id);

    /// <summary>Indica si existe un producto con el identificador dado.</summary>
    Task<bool> ExisteAsync(int id);
}
