using App_product.Application.DTOs;
using App_product.Application.Services;

namespace App_product.Api.GraphQL;

/// <summary>
/// Raíz de consultas GraphQL. Expone el recurso <c>Producto</c> con campos sustantivos.
/// </summary>
public sealed class Query
{
    private readonly IProductoService _servicio;

    public Query(IProductoService servicio) => _servicio = servicio;

    [GraphQLName("productos")]
    public Task<IEnumerable<ProductoDto>> Productos() =>
        _servicio.ObtenerTodosAsync();

    [GraphQLName("producto")]
    [GraphQLType(typeof(Types.ProductoType))]
    public Task<ProductoDto> ProductoPorId(int id) =>
        _servicio.ObtenerPorIdAsync(id);
}
