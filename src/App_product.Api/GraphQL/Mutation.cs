using App_product.Api.GraphQL.Inputs;
using App_product.Api.GraphQL.Mapping;
using App_product.Application.DTOs;
using App_product.Application.Services;
using FluentValidation;

namespace App_product.Api.GraphQL;

/// <summary>
/// Raíz de mutaciones GraphQL sobre el recurso <c>Producto</c>.
/// </summary>
public sealed class Mutation
{
    private readonly IProductoService _servicio;
    private readonly IValidator<CrearProductoDto> _validadorCrear;
    private readonly IValidator<ActualizarProductoDto> _validadorActualizar;

    public Mutation(
        IProductoService servicio,
        IValidator<CrearProductoDto> validadorCrear,
        IValidator<ActualizarProductoDto> validadorActualizar)
    {
        _servicio = servicio;
        _validadorCrear = validadorCrear;
        _validadorActualizar = validadorActualizar;
    }

    [GraphQLName("crear")]
    public async Task<ProductoDto> Crear(ProductoCrearInput input)
    {
        var dto = input.ToDto();
        var resultado = await _validadorCrear.ValidateAsync(dto);
        if (!resultado.IsValid)
            throw new ValidationException(resultado.Errors);

        return await _servicio.CrearAsync(dto);
    }

    [GraphQLName("actualizar")]
    public async Task<ProductoDto> Actualizar(int id, ProductoActualizarInput input)
    {
        var dto = input.ToDto();
        var resultado = await _validadorActualizar.ValidateAsync(dto);
        if (!resultado.IsValid)
            throw new ValidationException(resultado.Errors);

        await _servicio.ActualizarAsync(id, dto);
        return await _servicio.ObtenerPorIdAsync(id);
    }

    [GraphQLName("eliminar")]
    public async Task<bool> Eliminar(int id)
    {
        await _servicio.EliminarAsync(id);
        return true;
    }
}
