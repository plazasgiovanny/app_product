# Esquema de la Base de Datos

## Tabla: Productos

La tabla fue generada por la migración `InitialCreate` de EF Core a partir de la configuración en `ProductoConfiguration.cs`.

```sql
CREATE TABLE [Productos] (
    [Id]          INT            IDENTITY(1,1) NOT NULL,
    [Nombre]      NVARCHAR(120)  NOT NULL,
    [Descripcion] NVARCHAR(500)  NULL,
    [Precio]      DECIMAL(18,2)  NOT NULL,
    CONSTRAINT [PK_Productos] PRIMARY KEY ([Id])
);
```

## Descripción de columnas

| Columna | Tipo SQL | Nulable | Descripción | Decisión de diseño |
|---|---|---|---|---|
| `Id` | `INT IDENTITY(1,1)` | No | Identificador único autoincremental | La BD asigna el Id; el dominio no lo conoce en creación |
| `Nombre` | `NVARCHAR(120)` | No | Nombre del producto | `NVARCHAR` para soporte de Unicode (caracteres latinos/especiales). Máximo 120: suficiente para cualquier nombre de producto sin desperdiciar espacio |
| `Descripcion` | `NVARCHAR(500)` | Sí | Descripción breve del producto | Nulable porque la actividad la define como opcional. 500 caracteres para una descripción breve significativa |
| `Precio` | `DECIMAL(18,2)` | No | Precio del producto | `DECIMAL(18,2)`: 18 dígitos totales, 2 decimales. Evita errores de punto flotante (`FLOAT`/`REAL`) en valores monetarios. Soporta hasta `9,999,999,999,999,999.99` |

## Tabla de historial de migraciones

EF Core gestiona automáticamente la tabla `__EFMigrationsHistory`:

| Columna | Tipo | Descripción |
|---|---|---|
| `MigrationId` | NVARCHAR(150) | Identificador único de la migración (timestamp + nombre) |
| `ProductVersion` | NVARCHAR(32) | Versión de EF Core que creó la migración |

Esta tabla registra qué migraciones se han aplicado. EF Core la consulta antes de cada `database update` para aplicar solo las pendientes.

## Índices

| Índice | Columna | Tipo |
|---|---|---|
| `PK_Productos` | `Id` | Primary Key (clustered) |

No se definen índices adicionales en esta versión porque las consultas son simples (por Id o todas). En futuras versiones se podría agregar un índice sobre `Nombre` para búsquedas por texto.
