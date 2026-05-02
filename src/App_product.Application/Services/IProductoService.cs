using App_product.Application.DTOs;

namespace App_product.Application.Services;

/// <summary>
/// Contrato del servicio de aplicación para la gestión de productos.
/// Define los casos de uso disponibles. El controlador de la capa Api
/// depende de esta interfaz, nunca de la implementación concreta (DIP).
/// </summary>
public interface IProductoService
{
    /// <summary>Retorna la lista completa de productos.</summary>
    Task<IEnumerable<ProductoDto>> ObtenerTodosAsync();

    /// <summary>
    /// Retorna el producto con el Id especificado.
    /// Lanza <see cref="Domain.Exceptions.ProductoNoEncontradoException"/> si no existe.
    /// </summary>
    Task<ProductoDto> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Crea un nuevo producto y retorna su representación con el Id asignado.
    /// </summary>
    Task<ProductoDto> CrearAsync(CrearProductoDto dto);

    /// <summary>
    /// Actualiza un producto existente.
    /// Lanza <see cref="Domain.Exceptions.ProductoNoEncontradoException"/> si no existe.
    /// </summary>
    Task ActualizarAsync(int id, ActualizarProductoDto dto);

    /// <summary>
    /// Elimina el producto con el Id especificado.
    /// Lanza <see cref="Domain.Exceptions.ProductoNoEncontradoException"/> si no existe.
    /// </summary>
    Task EliminarAsync(int id);
}
