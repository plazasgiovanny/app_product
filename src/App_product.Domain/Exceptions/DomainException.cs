namespace App_product.Domain.Exceptions;

/// <summary>
/// Excepción base abstracta para todos los errores de dominio.
/// Permite al middleware global distinguir errores de negocio
/// de errores inesperados de infraestructura.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string mensaje) : base(mensaje) { }
}
