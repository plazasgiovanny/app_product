using App_product.Api.GraphQL;
using App_product.Api.GraphQL.Inputs;
using App_product.Application.DTOs;
using App_product.Application.Services;
using App_product.Domain.Exceptions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace App_product.Api.Tests.GraphQL;

public sealed class MutationTests
{
    private readonly Mock<IProductoService> _servicioMock;
    private readonly Mock<IValidator<CrearProductoDto>> _validadorCrearMock;
    private readonly Mock<IValidator<ActualizarProductoDto>> _validadorActualizarMock;
    private readonly Mutation _mutation;

    public MutationTests()
    {
        _servicioMock = new Mock<IProductoService>();
        _validadorCrearMock = new Mock<IValidator<CrearProductoDto>>();
        _validadorActualizarMock = new Mock<IValidator<ActualizarProductoDto>>();
        _mutation = new Mutation(
            _servicioMock.Object,
            _validadorCrearMock.Object,
            _validadorActualizarMock.Object);
    }

    [Fact]
    public async Task Crear_CuandoValidacionExitosa_RetornaProductoCreado()
    {
        var input = new ProductoCrearInput("Cuaderno", "200 hojas", 8500);
        var creado = new ProductoDto(1, "Cuaderno", "200 hojas", 8500);

        _validadorCrearMock
            .Setup(v => v.ValidateAsync(It.IsAny<CrearProductoDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _servicioMock.Setup(s => s.CrearAsync(It.IsAny<CrearProductoDto>())).ReturnsAsync(creado);

        var resultado = await _mutation.Crear(input);

        resultado.Should().Be(creado);
        _servicioMock.Verify(s => s.CrearAsync(It.Is<CrearProductoDto>(d =>
            d.Nombre == input.Nombre && d.Precio == input.Precio)), Times.Once);
    }

    [Fact]
    public async Task Crear_CuandoValidacionFalla_LanzaValidationExceptionSinInvocarServicio()
    {
        var input = new ProductoCrearInput("", null, -1);
        var fallos = new[] { new ValidationFailure("Nombre", "El nombre es obligatorio.") };

        _validadorCrearMock
            .Setup(v => v.ValidateAsync(It.IsAny<CrearProductoDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(fallos));

        var act = () => _mutation.Crear(input);

        await act.Should().ThrowAsync<ValidationException>();
        _servicioMock.Verify(s => s.CrearAsync(It.IsAny<CrearProductoDto>()), Times.Never);
    }

    [Fact]
    public async Task Actualizar_CuandoValidacionExitosa_RetornaProductoActualizado()
    {
        var input = new ProductoActualizarInput("Cuaderno actualizado", null, 9000);
        var actualizado = new ProductoDto(1, "Cuaderno actualizado", null, 9000);

        _validadorActualizarMock
            .Setup(v => v.ValidateAsync(It.IsAny<ActualizarProductoDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _servicioMock.Setup(s => s.ActualizarAsync(1, It.IsAny<ActualizarProductoDto>())).Returns(Task.CompletedTask);
        _servicioMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(actualizado);

        var resultado = await _mutation.Actualizar(1, input);

        resultado.Should().Be(actualizado);
        _servicioMock.Verify(s => s.ActualizarAsync(1, It.IsAny<ActualizarProductoDto>()), Times.Once);
        _servicioMock.Verify(s => s.ObtenerPorIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task Actualizar_CuandoNoExiste_PropagaProductoNoEncontradoException()
    {
        var input = new ProductoActualizarInput("Nombre", null, 100);

        _validadorActualizarMock
            .Setup(v => v.ValidateAsync(It.IsAny<ActualizarProductoDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _servicioMock
            .Setup(s => s.ActualizarAsync(99, It.IsAny<ActualizarProductoDto>()))
            .ThrowsAsync(new ProductoNoEncontradoException(99));

        var act = () => _mutation.Actualizar(99, input);

        await act.Should().ThrowAsync<ProductoNoEncontradoException>();
    }

    [Fact]
    public async Task Eliminar_CuandoExiste_RetornaTrue()
    {
        _servicioMock.Setup(s => s.EliminarAsync(1)).Returns(Task.CompletedTask);

        var resultado = await _mutation.Eliminar(1);

        resultado.Should().BeTrue();
        _servicioMock.Verify(s => s.EliminarAsync(1), Times.Once);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExiste_PropagaProductoNoEncontradoException()
    {
        _servicioMock
            .Setup(s => s.EliminarAsync(99))
            .ThrowsAsync(new ProductoNoEncontradoException(99));

        var act = () => _mutation.Eliminar(99);

        await act.Should().ThrowAsync<ProductoNoEncontradoException>();
    }
}
