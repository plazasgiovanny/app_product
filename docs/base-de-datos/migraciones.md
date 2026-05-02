# Migraciones EF Core — Code First

## Prerrequisitos

1. SQL Server Express corriendo en `.\SQLEXPRESS`.
2. `dotnet-ef` instalado globalmente:
   ```bash
   dotnet tool install --global dotnet-ef
   ```
3. Solución compilada sin errores:
   ```bash
   dotnet build App_product.sln
   ```

## Flujo estándar de migraciones

### 1. Crear una nueva migración

```bash
dotnet ef migrations add <NombreDeLaMigracion> \
  --project src/App_product.Infrastructure \
  --startup-project src/App_product.Api \
  --output-dir Migrations
```

Ejemplo: la migración inicial del proyecto:
```bash
dotnet ef migrations add InitialCreate \
  --project src/App_product.Infrastructure \
  --startup-project src/App_product.Api \
  --output-dir Migrations
```

Esto genera tres archivos en `src/App_product.Infrastructure/Migrations/`:
- `<timestamp>_InitialCreate.cs` — instrucciones de `Up()` y `Down()`.
- `<timestamp>_InitialCreate.Designer.cs` — metadatos del snapshot.
- `ApplicationDbContextModelSnapshot.cs` — estado actual del modelo.

### 2. Aplicar la migración a la base de datos

```bash
dotnet ef database update \
  --project src/App_product.Infrastructure \
  --startup-project src/App_product.Api
```

EF Core:
1. Crea la base de datos `AppProductDb` si no existe.
2. Crea la tabla `__EFMigrationsHistory` si no existe.
3. Aplica las migraciones pendientes en orden.
4. Registra cada migración aplicada en `__EFMigrationsHistory`.

### 3. Verificar migraciones aplicadas

```bash
dotnet ef migrations list \
  --project src/App_product.Infrastructure \
  --startup-project src/App_product.Api
```

### 4. Revertir la última migración (deshacer)

```bash
# Primero revertir en la BD (vuelve al estado anterior)
dotnet ef database update <MigracionAnterior> \
  --project src/App_product.Infrastructure \
  --startup-project src/App_product.Api

# Luego eliminar el archivo de migración del código
dotnet ef migrations remove \
  --project src/App_product.Infrastructure \
  --startup-project src/App_product.Api
```

### 5. Generar script SQL (para revisión o producción)

```bash
dotnet ef migrations script \
  --project src/App_product.Infrastructure \
  --startup-project src/App_product.Api \
  --output migrations.sql
```

Genera el SQL equivalente a todas las migraciones. Útil para revisar el DDL antes de aplicar en producción.

## Migración automática al iniciar (modo Development)

El proyecto está configurado para ejecutar `database.Migrate()` automáticamente al arrancar en modo Development (`Program.cs`):

```csharp
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
```

Esto garantiza que la BD y la tabla `Productos` estén siempre actualizadas al ejecutar `dotnet run`.

## Flujo para agregar una nueva propiedad (ejemplo futuro)

1. Modificar `Producto.cs` en Domain (agregar propiedad `Categoria`).
2. Actualizar `ProductoConfiguration.cs` en Infrastructure con el mapeo de la nueva columna.
3. Crear migración: `dotnet ef migrations add AgregarCategoria ...`
4. Aplicar: `dotnet ef database update ...`

No se necesita tocar la lógica de negocio ni los endpoints si la propiedad es opcional.
