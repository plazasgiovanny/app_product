using App_product.Application.DTOs;
using FluentValidation;

namespace App_product.Application.Validators;

/// <summary>
/// Reglas de validación para el DTO de creación de producto.
/// Implementa FluentValidation para reglas declarativas, encadenables y testeables
/// de forma independiente al pipeline HTTP (patrón Specification implícito).
/// </summary>
public sealed class CrearProductoDtoValidator : AbstractValidator<CrearProductoDto>
{
    public CrearProductoDtoValidator()
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
