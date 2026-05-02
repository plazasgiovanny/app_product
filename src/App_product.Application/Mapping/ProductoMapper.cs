using App_product.Application.DTOs;
using App_product.Domain.Entities;

namespace App_product.Application.Mapping;

/// <summary>
/// Clase estática con métodos de extensión para la conversión entre la entidad
/// de dominio <see cref="Producto"/> y los DTOs de la capa de Aplicación.
/// Se evita AutoMapper para mantener el mapeo explícito, visible y fácilmente testeable.
/// </summary>
public static class ProductoMapper
{
    /// <summary>Convierte una entidad <see cref="Producto"/> en su DTO de salida.</summary>
    public static ProductoDto ToDto(this Producto producto) =>
        new(producto.Id, producto.Nombre, producto.Descripcion, producto.Precio);

    /// <summary>
    /// Crea una nueva entidad <see cref="Producto"/> a partir del DTO de creación.
    /// El Id queda en su valor por defecto (0) hasta que la BD lo asigne.
    /// </summary>
    public static Producto ToEntity(this CrearProductoDto dto) =>
        new()
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio
        };

    /// <summary>
    /// Aplica los valores del DTO de actualización sobre una entidad existente.
    /// Modifica la entidad recibida en lugar de crear una nueva para respetar
    /// el ciclo de vida de EF Core (change tracking).
    /// </summary>
    public static void ApplyUpdate(this Producto producto, ActualizarProductoDto dto)
    {
        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.Precio = dto.Precio;
    }
}
