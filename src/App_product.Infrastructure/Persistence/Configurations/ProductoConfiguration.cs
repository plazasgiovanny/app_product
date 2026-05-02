using App_product.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App_product.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de mapeo EF Core para la entidad <see cref="Producto"/>.
/// Al implementar <see cref="IEntityTypeConfiguration{T}"/> y aplicarse vía
/// <c>ApplyConfigurationsFromAssembly</c>, se mantiene la entidad de dominio
/// completamente libre de anotaciones de infraestructura (Persistence Ignorance).
/// </summary>
public sealed class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Productos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .UseIdentityColumn();

        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(120)
            .HasColumnType("nvarchar(120)");

        builder.Property(p => p.Descripcion)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasColumnType("nvarchar(500)");

        // decimal(18,2): 18 dígitos totales, 2 decimales.
        // Suficiente para precios de hasta 9,999,999,999,999,999.99.
        builder.Property(p => p.Precio)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}
