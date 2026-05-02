namespace App_product.Application.DTOs;

/// <summary>
/// Objeto de transferencia de datos de salida (response) para la entidad Producto.
/// Aísla el contrato HTTP externo de la entidad del dominio, permitiendo que
/// ambos evolucionen de manera independiente.
/// </summary>
public sealed record ProductoDto(
    int Id,
    string Nombre,
    string? Descripcion,
    decimal Precio
);
