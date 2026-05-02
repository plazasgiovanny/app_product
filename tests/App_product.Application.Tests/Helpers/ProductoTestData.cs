using App_product.Application.DTOs;
using App_product.Domain.Entities;

namespace App_product.Application.Tests.Helpers;

/// <summary>
/// Clase de datos de prueba para el patrón table-driven con [MemberData].
/// Provee filas de datos complejos (entidades, DTOs) que no caben en [InlineData].
/// Se usa como fuente de datos en los tests con [Theory][MemberData(nameof(XXX))].
/// </summary>
public static class ProductoTestData
{
    /// <summary>
    /// Casos de prueba para ObtenerPorId:
    /// (id, productoEnRepositorio, debeArrojarExcepcion)
    /// </summary>
    public static IEnumerable<object?[]> CasosObtenerPorId()
    {
        // Id existente — debe retornar el producto
        yield return new object?[]
        {
            1,
            new Producto { Id = 1, Nombre = "Lapiz", Descripcion = "Lapiz HB", Precio = 500 },
            false
        };

        // Id inexistente — debe arrojar ProductoNoEncontradoException
        yield return new object?[]
        {
            99,
            null,
            true
        };
    }

    /// <summary>
    /// Casos de prueba para Crear:
    /// (dto, resultadoEsperado)
    /// </summary>
    public static IEnumerable<object[]> CasosCrear()
    {
        yield return new object[]
        {
            new CrearProductoDto("Cuaderno", "Cuaderno universitario", 8500m),
            new ProductoDto(1, "Cuaderno", "Cuaderno universitario", 8500m)
        };

        yield return new object[]
        {
            new CrearProductoDto("Borrador", null!, 300m),
            new ProductoDto(2, "Borrador", null!, 300m)
        };
    }

    /// <summary>
    /// Casos de prueba para Actualizar:
    /// (id, productoExistente, dto, debeArrojarExcepcion)
    /// </summary>
    public static IEnumerable<object?[]> CasosActualizar()
    {
        // Producto existente — actualización exitosa
        yield return new object?[]
        {
            1,
            new Producto { Id = 1, Nombre = "Lapiz", Descripcion = "Original", Precio = 500 },
            new ActualizarProductoDto("Lapiz Actualizado", "Nueva descripción", 600m),
            false
        };

        // Producto inexistente — debe arrojar excepción
        yield return new object?[]
        {
            99,
            null,
            new ActualizarProductoDto("Cualquier", null, 100m),
            true
        };
    }

    /// <summary>
    /// Casos de prueba para Eliminar:
    /// (id, existe, debeArrojarExcepcion)
    /// </summary>
    public static IEnumerable<object[]> CasosEliminar()
    {
        yield return new object[] { 1, true,  false }; // Producto existe — eliminación OK
        yield return new object[] { 99, false, true  }; // No existe — excepción
    }
}
