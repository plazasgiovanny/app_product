using App_product.Application.DTOs;
using FluentValidation;

namespace App_product.Application.Validators;

/// <summary>
/// Reglas de validación para el DTO de actualización de producto.
/// Aplica las mismas restricciones de negocio que la creación,
/// garantizando consistencia del dominio en ambas operaciones de escritura.
/// </summary>
public sealed class ActualizarProductoDtoValidator : AbstractValidator<ActualizarProductoDto>
{
    public ActualizarProductoDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
            .MaximumLength(120).WithMessage("El nombre no puede superar los 120 caracteres.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.")
            .When(x => x.Descripcion is not null);

        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo.");
    }
}
