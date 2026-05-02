namespace App_product.Domain.Exceptions;

/// <summary>
/// Se lanza cuando se intenta acceder a un producto que no existe en el repositorio.
/// El middleware global la traduce a una respuesta HTTP 404 Not Found con ProblemDetails.
/// </summary>
public sealed class ProductoNoEncontradoException : DomainException
{
    public int ProductoId { get; }

    public ProductoNoEncontradoException(int id)
        : base($"No se encontró el producto con Id '{id}'.") 
    {
        ProductoId = id;
    }
}
