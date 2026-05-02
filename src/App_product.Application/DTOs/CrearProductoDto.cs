namespace App_product.Application.DTOs;

/// <summary>
/// Objeto de transferencia de datos de entrada para la operación de creación.
/// No incluye el campo Id porque la base de datos lo asigna automáticamente.
/// Las reglas de validación se definen en <see cref="Validators.CrearProductoDtoValidator"/>.
/// </summary>
public sealed record CrearProductoDto(
    string Nombre,
    string? Descripcion,
    decimal Precio
);
