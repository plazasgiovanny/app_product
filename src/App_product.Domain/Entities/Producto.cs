namespace App_product.Domain.Entities;

/// <summary>
/// Entidad de negocio que representa un producto en el sistema.
/// Es un POCO puro: no tiene referencias a EF Core, ASP.NET Core ni ningún
/// detalle de infraestructura. Las invariantes de negocio se validan en la
/// capa de Aplicación mediante FluentValidation.
/// </summary>
public sealed class Producto
{
    /// <summary>Identificador único del producto (asignado por la base de datos).</summary>
    public int Id { get; set; }

    /// <summary>Nombre descriptivo del producto. No puede ser nulo ni vacío.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción breve del producto. Es opcional.</summary>
    public string? Descripcion { get; set; }

    /// <summary>Precio del producto. Debe ser mayor o igual a cero.</summary>
    public decimal Precio { get; set; }
}
