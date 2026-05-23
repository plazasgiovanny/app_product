using App_product.Domain.Exceptions;
using FluentValidation;

namespace App_product.Api.GraphQL.Errors;

/// <summary>
/// Filtro de errores GraphQL que traduce excepciones de dominio y validación
/// a errores estructurados, emulando la matriz de <see cref="Middleware.ExceptionHandlingMiddleware"/>.
/// </summary>
public sealed class GraphQLDomainErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception is null)
            return error;

        return error.Exception switch
        {
            ProductoNoEncontradoException ex => error
                .WithMessage(ex.Message)
                .WithCode("PRODUCTO_NO_ENCONTRADO")
                .SetExtension("statusCode", StatusCodes.Status404NotFound),

            ValidationException validation => error
                .WithMessage(string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage)))
                .WithCode("VALIDATION_ERROR")
                .SetExtension("statusCode", StatusCodes.Status400BadRequest),

            DomainException domain => error
                .WithMessage(domain.Message)
                .WithCode("DOMAIN_ERROR")
                .SetExtension("statusCode", StatusCodes.Status422UnprocessableEntity),

            _ => error
                .WithMessage("Ocurrió un error inesperado. Por favor intente más tarde.")
                .WithCode("INTERNAL_SERVER_ERROR")
                .SetExtension("statusCode", StatusCodes.Status500InternalServerError)
        };
    }
}
