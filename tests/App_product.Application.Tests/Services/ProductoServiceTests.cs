using App_product.Application.DTOs;
using App_product.Application.Services;
using App_product.Application.Tests.Helpers;
using App_product.Domain.Entities;
using App_product.Domain.Exceptions;
using App_product.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace App_product.Application.Tests.Services;

/// <summary>
/// Pruebas unitarias para <see cref="ProductoService"/>.
/// Aplica el patrón table-driven usando [Theory] + [MemberData]:
///   - Cada método de test evalúa múltiples casuísticas desde <see cref="ProductoTestData"/>.
///   - El repositorio se mockea con Moq (LSP: cualquier implementación de IProductoRepository es válida).
///   - No se toca la base de datos real en ningún momento.
/// </summary>
public sealed class ProductoServiceTests
{
    private readonly Mock<IProductoRepository> _repoMock;
    private readonly ProductoService _servicio;

    public ProductoServiceTests()
    {
        _repoMock = new Mock<IProductoRepository>();
        _servicio = new ProductoService(_repoMock.Object);
    }

    // ─────────────────────────────────────────────────────
    // ObtenerTodos
    // ─────────────────────────────────────────────────────

    [Fact]
    public async Task ObtenerTodos_CuandoExistenProductos_RetornaListaDtos()
    {
        // Arrange
        var productos = new List<Producto>
        {
            new() { Id = 1, Nombre = "Lapiz", Descripcion = "HB", Precio = 500 },
            new() { Id = 2, Nombre = "Boligrafo", Descripcion = null, Precio = 800 }
        };
        _repoMock.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(productos);

        // Act
        var resultado = await _servicio.ObtenerTodosAsync();

        // Assert
        resultado.Should().HaveCount(2);
        resultado.Should().ContainSingle(p => p.Nombre == "Lapiz");
    }

    [Fact]
    public async Task ObtenerTodos_CuandoNoExistenProductos_RetornaListaVacia()
    {
        _repoMock.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(new List<Producto>());

        var resultado = await _servicio.ObtenerTodosAsync();

        resultado.Should().BeEmpty();
    }

    // ─────────────────────────────────────────────────────
    // ObtenerPorId — TABLE-DRIVEN con MemberData
    // ─────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(ProductoTestData.CasosObtenerPorId), MemberType = typeof(ProductoTestData))]
    public async Task ObtenerPorId_TableDriven(int id, Producto? productoEnRepo, bool debeArrojarExcepcion)
    {
        // Arrange
        _repoMock.Setup(r => r.ObtenerPorIdAsync(id)).ReturnsAsync(productoEnRepo);

        if (debeArrojarExcepcion)
        {
            // Act & Assert
            var act = async () => await _servicio.ObtenerPorIdAsync(id);
            await act.Should().ThrowAsync<ProductoNoEncontradoException>()
                .WithMessage($"*{id}*");
        }
        else
        {
            // Act
            var resultado = await _servicio.ObtenerPorIdAsync(id);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(id);
        }
    }

    // ─────────────────────────────────────────────────────
    // Crear — TABLE-DRIVEN con MemberData
    // ─────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(ProductoTestData.CasosCrear), MemberType = typeof(ProductoTestData))]
    public async Task Crear_TableDriven(CrearProductoDto dto, ProductoDto esperado)
    {
        // Arrange — simula que el repositorio devuelve el producto con el Id asignado
        _repoMock.Setup(r => r.AgregarAsync(It.IsAny<Producto>()))
            .ReturnsAsync((Producto p) =>
            {
                p.Id = esperado.Id; // simula auto-increment de la BD
                return p;
            });

        // Act
        var resultado = await _servicio.CrearAsync(dto);

        // Assert
        resultado.Id.Should().Be(esperado.Id);
        resultado.Nombre.Should().Be(esperado.Nombre);
        resultado.Precio.Should().Be(esperado.Precio);
        _repoMock.Verify(r => r.AgregarAsync(It.IsAny<Producto>()), Times.Once);
    }

    // ─────────────────────────────────────────────────────
    // Actualizar — TABLE-DRIVEN con MemberData
    // ─────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(ProductoTestData.CasosActualizar), MemberType = typeof(ProductoTestData))]
    public async Task Actualizar_TableDriven(int id, Producto? productoExistente, ActualizarProductoDto dto, bool debeArrojarExcepcion)
    {
        // Arrange
        _repoMock.Setup(r => r.ObtenerPorIdAsync(id)).ReturnsAsync(productoExistente);
        _repoMock.Setup(r => r.ActualizarAsync(It.IsAny<Producto>())).Returns(Task.CompletedTask);

        if (debeArrojarExcepcion)
        {
            var act = async () => await _servicio.ActualizarAsync(id, dto);
            await act.Should().ThrowAsync<ProductoNoEncontradoException>();
        }
        else
        {
            await _servicio.ActualizarAsync(id, dto);
            _repoMock.Verify(r => r.ActualizarAsync(It.IsAny<Producto>()), Times.Once);
        }
    }

    // ─────────────────────────────────────────────────────
    // Eliminar — TABLE-DRIVEN con MemberData
    // ─────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(ProductoTestData.CasosEliminar), MemberType = typeof(ProductoTestData))]
    public async Task Eliminar_TableDriven(int id, bool existe, bool debeArrojarExcepcion)
    {
        // Arrange
        _repoMock.Setup(r => r.ExisteAsync(id)).ReturnsAsync(existe);
        _repoMock.Setup(r => r.EliminarAsync(id)).Returns(Task.CompletedTask);

        if (debeArrojarExcepcion)
        {
            var act = async () => await _servicio.EliminarAsync(id);
            await act.Should().ThrowAsync<ProductoNoEncontradoException>();
            _repoMock.Verify(r => r.EliminarAsync(id), Times.Never);
        }
        else
        {
            await _servicio.EliminarAsync(id);
            _repoMock.Verify(r => r.EliminarAsync(id), Times.Once);
        }
    }
}
