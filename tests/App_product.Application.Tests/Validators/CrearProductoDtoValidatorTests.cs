using App_product.Application.DTOs;
using App_product.Application.Validators;
using FluentAssertions;

namespace App_product.Application.Tests.Validators;

/// <summary>
/// Pruebas unitarias para <see cref="CrearProductoDtoValidator"/>.
/// Aplica el patrón table-driven con [Theory][InlineData]:
/// cada fila representa una casuística de validación con entrada y resultado esperado.
/// </summary>
public sealed class CrearProductoDtoValidatorTests
{
    private readonly CrearProductoDtoValidator _validator = new();

    /// <summary>
    /// Tabla de casuísticas de validación:
    /// (nombre, descripcion, precio, deberiaSerValido, descripcionDelCaso)
    /// </summary>
    [Theory]
    [InlineData("Lapiz",    "Descripcion", 100.0,  true,  "Caso válido completo")]
    [InlineData("Lapiz",    null,          100.0,  true,  "Descripcion es opcional")]
    [InlineData("Lapiz",    "Descripcion", 0.0,    true,  "Precio cero es permitido")]
    [InlineData("",         "Descripcion", 100.0,  false, "Nombre vacío no es válido")]
    [InlineData(null,       "Descripcion", 100.0,  false, "Nombre null no es válido")]
    [InlineData("Lapiz",    "Descripcion", -0.01,  false, "Precio negativo no es válido")]
    [InlineData("Lapiz",    "Descripcion", -100.0, false, "Precio muy negativo no es válido")]
    public async Task Validar_CrearProductoDto_TableDriven(
        string? nombre,
        string? descripcion,
        double precio,
        bool deberiaSerValido,
        string descripcionCaso)
    {
        // Arrange
        var dto = new CrearProductoDto(nombre!, descripcion, (decimal)precio);

        // Act
        var resultado = await _validator.ValidateAsync(dto);

        // Assert
        resultado.IsValid.Should().Be(deberiaSerValido,
            because: $"el caso '{descripcionCaso}' debería ser {(deberiaSerValido ? "válido" : "inválido")}");
    }

    [Fact]
    public async Task Validar_NombreMayorA120Caracteres_RetornaInvalido()
    {
        // Arrange
        var nombreLargo = new string('A', 121);
        var dto = new CrearProductoDto(nombreLargo, null, 100m);

        // Act
        var resultado = await _validator.ValidateAsync(dto);

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle(e => e.PropertyName == "Nombre");
    }

    [Fact]
    public async Task Validar_DescripcionMayorA500Caracteres_RetornaInvalido()
    {
        // Arrange
        var descLarga = new string('X', 501);
        var dto = new CrearProductoDto("Producto", descLarga, 100m);

        // Act
        var resultado = await _validator.ValidateAsync(dto);

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle(e => e.PropertyName == "Descripcion");
    }
}
