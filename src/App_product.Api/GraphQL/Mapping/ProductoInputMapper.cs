using App_product.Application.DTOs;
using App_product.Api.GraphQL.Inputs;

namespace App_product.Api.GraphQL.Mapping;

/// <summary>
/// Mapeo entre tipos de entrada GraphQL y DTOs de la capa Application.
/// </summary>
internal static class ProductoInputMapper
{
    public static CrearProductoDto ToDto(this ProductoCrearInput input) =>
        new(input.Nombre, input.Descripcion, input.Precio);

    public static ActualizarProductoDto ToDto(this ProductoActualizarInput input) =>
        new(input.Nombre, input.Descripcion, input.Precio);
}
