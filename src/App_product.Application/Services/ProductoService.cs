using App_product.Application.DTOs;
using App_product.Application.Mapping;
using App_product.Domain.Exceptions;
using App_product.Domain.Repositories;

namespace App_product.Application.Services;

/// <summary>
/// Implementación del servicio de aplicación para la gestión de productos.
/// Orquesta las operaciones CRUD coordinando el repositorio del dominio.
/// Recibe <see cref="IProductoRepository"/> por inyección de dependencias (DIP):
/// depende de la abstracción definida en Domain, no de la implementación EF Core.
/// </summary>
public sealed class ProductoService : IProductoService
{
    private readonly IProductoRepository _repositorio;

    public ProductoService(IProductoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProductoDto>> ObtenerTodosAsync()
    {
        var productos = await _repositorio.ObtenerTodosAsync();
        return productos.Select(p => p.ToDto());
    }

    /// <inheritdoc/>
    public async Task<ProductoDto> ObtenerPorIdAsync(int id)
    {
        var producto = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new ProductoNoEncontradoException(id);

        return producto.ToDto();
    }

    /// <inheritdoc/>
    public async Task<ProductoDto> CrearAsync(CrearProductoDto dto)
    {
        var entidad = dto.ToEntity();
        var creado = await _repositorio.AgregarAsync(entidad);
        return creado.ToDto();
    }

    /// <inheritdoc/>
    public async Task ActualizarAsync(int id, ActualizarProductoDto dto)
    {
        var producto = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new ProductoNoEncontradoException(id);

        producto.ApplyUpdate(dto);
        await _repositorio.ActualizarAsync(producto);
    }

    /// <inheritdoc/>
    public async Task EliminarAsync(int id)
    {
        if (!await _repositorio.ExisteAsync(id))
            throw new ProductoNoEncontradoException(id);

        await _repositorio.EliminarAsync(id);
    }
}
