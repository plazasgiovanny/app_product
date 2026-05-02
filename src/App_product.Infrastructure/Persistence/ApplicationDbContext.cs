using App_product.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App_product.Infrastructure.Persistence;

/// <summary>
/// Contexto de base de datos de EF Core para la aplicación.
/// Registra los sets de entidades del dominio y aplica automáticamente todas las
/// configuraciones de mapeo que implementen <see cref="IEntityTypeConfiguration{T}"/>
/// dentro de este ensamblado, evitando acoplar el dominio a los detalles de EF Core.
/// </summary>
public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    /// <summary>Conjunto de productos en la base de datos.</summary>
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas las clases IEntityTypeConfiguration<T> del ensamblado actual.
        // Permite agregar configuraciones sin modificar este archivo (OCP).
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
