using App_product.Application.DTOs;

namespace App_product.Api.GraphQL.Types;

/// <summary>
/// Tipo GraphQL de salida para el recurso Producto.
/// Mapea <see cref="ProductoDto"/> sin exponer el nombre del DTO en el esquema.
/// </summary>
public sealed class ProductoType : ObjectType<ProductoDto>
{
    protected override void Configure(IObjectTypeDescriptor<ProductoDto> descriptor)
    {
        descriptor.Name("Producto");
        descriptor.Description("Producto del catálogo.");
    }
}
