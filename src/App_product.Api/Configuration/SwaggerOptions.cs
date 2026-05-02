using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace App_product.Api.Configuration;

/// <summary>
/// Configuración de Swagger/OpenAPI con soporte de múltiples versiones de API.
/// Implementa <see cref="IConfigureNamedOptions{SwaggerGenOptions}"/> para
/// generar un documento por cada versión descubierta por API Explorer.
/// </summary>
public sealed class SwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _proveedor;

    public SwaggerOptions(IApiVersionDescriptionProvider proveedor)
    {
        _proveedor = proveedor;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var descripcion in _proveedor.ApiVersionDescriptions)
        {
            options.SwaggerDoc(descripcion.GroupName, CrearInformacion(descripcion));
        }
    }

    public void Configure(string? name, SwaggerGenOptions options) => Configure(options);

    private static OpenApiInfo CrearInformacion(ApiVersionDescription descripcion)
    {
        var info = new OpenApiInfo
        {
            Title = "App Product API",
            Version = descripcion.ApiVersion.ToString(),
            Description = "API REST para la gestión de productos. " +
                          "Implementa operaciones CRUD completas bajo arquitectura limpia (Clean Architecture).",
            Contact = new OpenApiContact
            {
                Name = "Maestría Arquitectura de Software",
            }
        };

        if (descripcion.IsDeprecated)
            info.Description += " (Versión obsoleta)";

        return info;
    }
}
