using App_product.Api;
using App_product.Api.Middleware;
using App_product.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────
// Composition Root — registro de servicios
// ──────────────────────────────────────────
builder.Services
    .AddPersistence(builder.Configuration)
    .AddApplicationServices()
    .AddPresentationServices();

var app = builder.Build();

// ──────────────────────────────────────────
// Pipeline de la aplicación (orden importa)
// ──────────────────────────────────────────

// 1. Middleware global de errores: debe ir primero para capturar todo
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 2. Redirección HTTPS
app.UseHttpsRedirection();

// 3. Swagger UI — solo en Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Genera un endpoint por cada versión de API registrada
        var proveedor = app.Services.GetRequiredService<Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider>();
        foreach (var descripcion in proveedor.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{descripcion.GroupName}/swagger.json",
                $"App Product API {descripcion.GroupName.ToUpperInvariant()}");
        }
        options.RoutePrefix = "swagger";
    });
}

// 4. Autorización (placeholder para futuras implementaciones)
app.UseAuthorization();

// 5. Mapeo de controllers
app.MapControllers();

// ──────────────────────────────────────────
// Migración automática al arrancar (solo Development)
// ──────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
