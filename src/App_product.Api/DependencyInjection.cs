using App_product.Api.Configuration;
using App_product.Application.Services;
using App_product.Application.Validators;
using App_product.Domain.Repositories;
using App_product.Infrastructure.Persistence;
using App_product.Infrastructure.Repositories;
using Asp.Versioning;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App_product.Api;

/// <summary>
/// Composition Root de la solución.
/// Es el único lugar donde se conocen y se registran las implementaciones concretas
/// de todas las interfaces (patrón Composition Root, Mark Seemann).
/// Se organiza en métodos de extensión por responsabilidad para mantener Program.cs limpio.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra el DbContext con SQL Server y la implementación del repositorio.
    /// Aquí queda confinado todo conocimiento de EF Core y la cadena de conexión.
    /// </summary>
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var cadenaConexion = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión 'DefaultConnection' en la configuración.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(cadenaConexion));

        // Registra la implementación concreta del puerto del dominio.
        // Solo la Api (Composition Root) conoce que ProductoRepository implementa IProductoRepository.
        services.AddScoped<IProductoRepository, ProductoRepository>();

        return services;
    }

    /// <summary>
    /// Registra los servicios de la capa Application y los validadores FluentValidation.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductoService, ProductoService>();

        // Registra todos los validadores del ensamblado de Application automáticamente.
        services.AddValidatorsFromAssemblyContaining<CrearProductoDtoValidator>();

        return services;
    }

    /// <summary>
    /// Registra los servicios de presentación: controllers, Swagger con soporte de
    /// versionado y API Explorer.
    /// </summary>
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            // Formato de grupo: 'v' seguido del número de versión (ej. v1)
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // Habilita los comentarios XML en Swagger
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);
        });

        // Configura los documentos Swagger por versión (uno por ApiVersion encontrada)
        services.ConfigureOptions<SwaggerOptions>();

        return services;
    }
}
