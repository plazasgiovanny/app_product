using App_product.Domain.Entities;
using App_product.Domain.Repositories;
using App_product.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace App_product.Infrastructure.Repositories;

/// <summary>
/// Adaptador de persistencia que implementa el puerto <see cref="IProductoRepository"/>
/// definido en la capa Domain, usando Entity Framework Core como mecanismo de acceso a datos.
/// Es el único lugar de la solución donde se conocen los detalles de SQL Server.
/// </summary>
public sealed class ProductoRepository : IProductoRepository
{
    private readonly ApplicationDbContext _contexto;

    public ProductoRepository(ApplicationDbContext contexto)
    {
        _contexto = contexto;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Producto>> ObtenerTodosAsync() =>
        await _contexto.Productos.AsNoTracking().ToListAsync();

    /// <inheritdoc/>
    public async Task<Producto?> ObtenerPorIdAsync(int id) =>
        await _contexto.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    /// <inheritdoc/>
    public async Task<Producto> AgregarAsync(Producto producto)
    {
        _contexto.Productos.Add(producto);
        await _contexto.SaveChangesAsync();
        return producto;
    }

    /// <inheritdoc/>
    public async Task ActualizarAsync(Producto producto)
    {
        _contexto.Productos.Update(producto);
        await _contexto.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task EliminarAsync(int id)
    {
        var producto = await _contexto.Productos.FindAsync(id);
        if (producto is not null)
        {
            _contexto.Productos.Remove(producto);
            await _contexto.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExisteAsync(int id) =>
        await _contexto.Productos.AnyAsync(p => p.Id == id);
}
