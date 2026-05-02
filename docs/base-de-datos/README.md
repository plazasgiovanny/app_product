# Documentación de Base de Datos

## Motor de base de datos

| Propiedad | Valor |
|---|---|
| Motor | Microsoft SQL Server Express |
| Servidor | `.\SQLEXPRESS` (instancia local) |
| Base de datos | `AppProductDb` |
| Autenticación | Windows Authentication (Trusted Connection) |
| Cadena de conexión | Ver `src/App_product.Api/appsettings.json` |

## Patrón de acceso: Code First con EF Core

El proyecto utiliza el enfoque **Code First** de Entity Framework Core:

1. Se define la entidad `Producto` como clase C# en la capa `Domain` (POCO, sin anotaciones EF).
2. La configuración de mapeo (tabla, columnas, tipos) se define en `ProductoConfiguration.cs` dentro de `Infrastructure`.
3. EF Core genera el SQL automáticamente a partir del modelo.
4. Las migraciones mantienen el historial de cambios al esquema.

**Ventaja:** No se escribe T-SQL manualmente. El esquema evoluciona desde el modelo de dominio.

## Documentos

| Documento | Descripción |
|---|---|
| [esquema.md](esquema.md) | Definición detallada de la tabla `Productos` |
| [migraciones.md](migraciones.md) | Flujo completo de migraciones EF Core |
