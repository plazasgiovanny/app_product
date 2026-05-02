# Pipeline de Integración Continua (CI)

## Archivo de configuración

```
.github/
└── workflows/
    └── ci.yml
```

## Activación del pipeline

El pipeline se ejecuta automáticamente en dos situaciones:

- **Push** a las ramas `main` o `develop`.
- **Pull Request** que tenga como destino `main` o `develop`.

```yaml
on:
  push:
    branches: [ "main", "develop" ]
  pull_request:
    branches: [ "main", "develop" ]
```

## Estructura del pipeline

El pipeline está compuesto por **3 jobs** que se ejecutan en Ubuntu. Los jobs de pruebas dependen del job de compilación (`needs: build`), de modo que si el código no compila, no se pierden recursos ejecutando las pruebas.

```
build
  ├── unit-tests  (solo si build pasa)
  └── arch-tests  (solo si build pasa)
```

## Job 1: Build (warnings-as-errors)

**Propósito:** Garantizar que el código compila limpiamente y sin advertencias.

La bandera `-p:TreatWarningsAsErrors=true` convierte cualquier warning del compilador en un error de compilación. Esto obliga a los desarrolladores a mantener el código limpio y sin deudas técnicas acumuladas.

```yaml
- name: Compilar solución (warnaserror)
  run: >
    dotnet build ${{ env.SOLUTION }}
    --no-restore
    --configuration Release
    -p:TreatWarningsAsErrors=true
```

## Job 2: Pruebas Unitarias

**Propósito:** Verificar que la lógica de negocio en la capa `Application` funciona correctamente ante diferentes escenarios.

Ejecuta los 26 casos de prueba del proyecto `App_product.Application.Tests` usando el patrón table-driven con xUnit.

Los resultados se publican como reporte legible directamente en la interfaz de GitHub usando `dorny/test-reporter`.

## Job 3: Pruebas de Arquitectura

**Propósito:** Verificar que las 10 reglas de fronteras entre capas continúan cumpliéndose.

Ejecuta el proyecto `App_product.ArchTests` que usa `NetArchTest.Rules` para inspeccionar los ensamblados compilados.

Si una nueva clase viola las reglas de dependencia (por ejemplo, un servicio de Application importa `ApplicationDbContext`), este job falla y bloquea el merge del PR.

## Variables de entorno globales

| Variable | Valor | Descripción |
|----------|-------|-------------|
| `DOTNET_VERSION` | `9.0.x` | Versión del SDK de .NET |
| `SOLUTION` | `App_product.sln` | Ruta relativa al archivo de solución |

## Artefactos generados

Los archivos de resultados `.trx` se guardan en `./test-results/` y se publican como reportes en el tab **Checks** del PR en GitHub.

## Cómo ver los resultados

1. Ir al PR en GitHub.
2. Hacer clic en la pestaña **Checks**.
3. Seleccionar "Resultados – Pruebas Unitarias" o "Resultados – Pruebas de Arquitectura".
4. Ver el detalle de qué prueba falló y el mensaje de error.

## Cómo extender el pipeline

Para agregar una nueva etapa (por ejemplo, análisis de cobertura):

1. Agregar un nuevo job en `.github/workflows/ci.yml`.
2. Incluir `needs: unit-tests` si depende de las pruebas unitarias.
3. Usar la bandera `--collect:"XPlat Code Coverage"` en `dotnet test`.
4. Publicar el reporte con `codecov/codecov-action`.
