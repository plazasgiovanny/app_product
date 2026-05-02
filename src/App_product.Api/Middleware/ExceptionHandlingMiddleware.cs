using App_product.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace App_product.Api.Middleware;

/// <summary>
/// Middleware de manejo global de excepciones.
/// Implementa el patrón Chain of Responsibility del pipeline de ASP.NET Core.
/// Captura todas las excepciones no manejadas y las traduce a respuestas HTTP
/// estandarizadas con formato ProblemDetails (RFC 7807).
/// Esto centraliza el manejo de errores (SRP) y evita duplicar bloques try/catch en los controladores.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada: {Mensaje}", ex.Message);
            await ManejarExcepcionAsync(context, ex);
        }
    }

    private static async Task ManejarExcepcionAsync(HttpContext context, Exception excepcion)
    {
        var (statusCode, titulo, detalle) = excepcion switch
        {
            ProductoNoEncontradoException notFound =>
                (StatusCodes.Status404NotFound, "Recurso no encontrado", notFound.Message),

            ValidationException validation =>
                (StatusCodes.Status400BadRequest, "Error de validación",
                 string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage))),

            DomainException domain =>
                (StatusCodes.Status422UnprocessableEntity, "Error de negocio", domain.Message),

            _ =>
                (StatusCodes.Status500InternalServerError, "Error interno del servidor",
                 "Ocurrió un error inesperado. Por favor intente más tarde.")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = titulo,
            Detail = detalle,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var opciones = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, opciones));
    }
}
