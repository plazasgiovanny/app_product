namespace App_product.Application.DTOs;

/// <summary>
/// Objeto de transferencia de datos de entrada para la operación de actualización completa (PUT).
/// El Id se toma de la ruta URL, no del cuerpo, para seguir las convenciones REST.
/// Las reglas de validación se definen en <see cref="Validators.ActualizarProductoDtoValidator"/>.
/// </summary>
public sealed record ActualizarProductoDto(
    string Nombre,
    string? Descripcion,
    decimal Precio
);
