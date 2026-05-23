using App_product.Api.GraphQL;
using App_product.Application.DTOs;
using App_product.Application.Services;
using App_product.Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace App_product.Api.Tests.GraphQL;

public sealed class QueryTests
{
    private readonly Mock<IProductoService> _servicioMock;
    private readonly Query _query;

    public QueryTests()
    {
        _servicioMock = new Mock<IProductoService>();
        _query = new Query(_servicioMock.Object);
    }

    [Fact]
    public async Task Productos_CuandoExistenProductos_RetornaListaDelServicio()
    {
        var productos = new List<ProductoDto>
        {
            new(1, "Lapiz", "HB", 500),
            new(2, "Boligrafo", null, 800)
        };
        _servicioMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(productos);

        var resultado = await _query.Productos();

        resultado.Should().BeEquivalentTo(productos);
        _servicioMock.Verify(s => s.ObtenerTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task Producto_CuandoExiste_RetornaProducto()
    {
        var producto = new ProductoDto(1, "Cuaderno", "200 hojas", 8500);
        _servicioMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

        var resultado = await _query.ProductoPorId(1);

        resultado.Should().Be(producto);
        _servicioMock.Verify(s => s.ObtenerPorIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task Producto_CuandoNoExiste_PropagaProductoNoEncontradoException()
    {
        _servicioMock
            .Setup(s => s.ObtenerPorIdAsync(99))
            .ThrowsAsync(new ProductoNoEncontradoException(99));

        var act = () => _query.ProductoPorId(99);

        await act.Should().ThrowAsync<ProductoNoEncontradoException>();
    }
}
