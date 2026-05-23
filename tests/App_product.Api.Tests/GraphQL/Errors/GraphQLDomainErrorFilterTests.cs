using App_product.Api.GraphQL.Errors;
using App_product.Domain.Exceptions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using HotChocolate;

namespace App_product.Api.Tests.GraphQL.Errors;

public sealed class GraphQLDomainErrorFilterTests
{
    private readonly GraphQLDomainErrorFilter _filter = new();

    [Fact]
    public void OnError_ProductoNoEncontrado_AsignaCodigoYStatus404()
    {
        var error = CrearError(new ProductoNoEncontradoException(1));

        var resultado = _filter.OnError(error);

        resultado.Code.Should().Be("PRODUCTO_NO_ENCONTRADO");
        resultado.Extensions!["statusCode"].Should().Be(404);
    }

    [Fact]
    public void OnError_ValidationException_AsignaCodigoYStatus400()
    {
        var fallos = new[] { new ValidationFailure("Nombre", "Requerido") };
        var error = CrearError(new ValidationException(fallos));

        var resultado = _filter.OnError(error);

        resultado.Code.Should().Be("VALIDATION_ERROR");
        resultado.Extensions!["statusCode"].Should().Be(400);
        resultado.Message.Should().Contain("Requerido");
    }

    [Fact]
    public void OnError_DomainException_AsignaCodigoYStatus422()
    {
        var error = CrearError(new DomainExceptionTest("Regla de negocio violada"));

        var resultado = _filter.OnError(error);

        resultado.Code.Should().Be("DOMAIN_ERROR");
        resultado.Extensions!["statusCode"].Should().Be(422);
    }

    [Fact]
    public void OnError_ExcepcionGenerica_AsignaCodigoYStatus500()
    {
        var error = CrearError(new InvalidOperationException("fallo inesperado"));

        var resultado = _filter.OnError(error);

        resultado.Code.Should().Be("INTERNAL_SERVER_ERROR");
        resultado.Extensions!["statusCode"].Should().Be(500);
    }

    [Fact]
    public void OnError_SinExcepcion_RetornaErrorSinModificar()
    {
        var error = ErrorBuilder.New().SetMessage("error base").Build();

        var resultado = _filter.OnError(error);

        resultado.Should().BeSameAs(error);
    }

    private static IError CrearError(Exception excepcion) =>
        ErrorBuilder.New()
            .SetMessage(excepcion.Message)
            .SetException(excepcion)
            .Build();

    private sealed class DomainExceptionTest : DomainException
    {
        public DomainExceptionTest(string mensaje) : base(mensaje) { }
    }
}
