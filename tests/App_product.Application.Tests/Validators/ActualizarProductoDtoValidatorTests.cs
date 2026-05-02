using App_product.Application.DTOs;
using App_product.Application.Validators;
using FluentAssertions;

namespace App_product.Application.Tests.Validators;

/// <summary>
/// Pruebas unitarias para <see cref="ActualizarProductoDtoValidator"/>.
/// Mismas reglas de negocio que CrearProductoDto: el nombre sigue siendo obligatorio
/// y el precio no puede ser negativo incluso en actualizaciones.
/// </summary>
public sealed class ActualizarProductoDtoValidatorTests
{
    private readonly ActualizarProductoDtoValidator _validator = new();

    /// <summary>
    /// Tabla de casuísticas de validación para actualización:
    /// (nombre, descripcion, precio, deberiaSerValido, descripcionDelCaso)
    /// </summary>
    [Theory]
    [InlineData("Lapiz actualizado", "Nueva desc", 150.0, true,  "Actualización completa válida")]
    [InlineData("Lapiz actualizado", null,         150.0, true,  "Descripción puede ser null en actualización")]
    [InlineData("Lapiz actualizado", "Nueva desc", 0.0,   true,  "Precio cero permitido en actualización")]
    [InlineData("",                  "Nueva desc", 150.0, false, "Nombre vacío inválido en actualización")]
    [InlineData(null,                "Nueva desc", 150.0, false, "Nombre null inválido en actualización")]
    [InlineData("Lapiz actualizado", "Nueva desc", -1.0,  false, "Precio negativo inválido en actualización")]
    public async Task Validar_ActualizarProductoDto_TableDriven(
        string? nombre,
        string? descripcion,
        double precio,
        bool deberiaSerValido,
        string descripcionCaso)
    {
        // Arrange
        var dto = new ActualizarProductoDto(nombre!, descripcion, (decimal)precio);

        // Act
        var resultado = await _validator.ValidateAsync(dto);

        // Assert
        resultado.IsValid.Should().Be(deberiaSerValido,
            because: $"el caso '{descripcionCaso}' debería ser {(deberiaSerValido ? "válido" : "inválido")}");
    }

    [Fact]
    public async Task Validar_NombreMayorA120Caracteres_RetornaInvalido()
    {
        var dto = new ActualizarProductoDto(new string('Z', 121), null, 100m);
        var resultado = await _validator.ValidateAsync(dto);
        resultado.IsValid.Should().BeFalse();
    }
}
