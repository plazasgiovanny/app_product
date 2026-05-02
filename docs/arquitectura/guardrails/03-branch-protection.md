# Branch Protection y Guardrails Estáticos

Esta sección describe las protecciones complementarias que refuerzan la arquitectura más allá de las pruebas automatizadas.

---

## 1. Branch Protection Rules en GitHub

Las reglas de protección de ramas son configuraciones del repositorio en GitHub que **impiden mergear** un Pull Request si las validaciones obligatorias no pasan.

### Cómo configurarlas

1. Ir al repositorio en GitHub.
2. Navegar a **Settings → Branches**.
3. Hacer clic en **Add branch protection rule**.
4. En el campo **Branch name pattern** escribir `main`.
5. Activar las siguientes opciones:

| Opción | Descripción |
|--------|-------------|
| ✅ Require a pull request before merging | Todo cambio a `main` debe pasar por un PR |
| ✅ Require status checks to pass before merging | Los jobs de CI deben ser exitosos |
| ✅ Require branches to be up to date before merging | El PR debe estar actualizado con `main` |
| ✅ Do not allow bypassing the above settings | Ni los administradores pueden saltarse las reglas |

### Status checks requeridos

Una vez que el pipeline de CI haya ejecutado al menos una vez, aparecerán estos checks para seleccionar como obligatorios:

- `Build (warnings-as-errors)`
- `Pruebas unitarias`
- `Pruebas de arquitectura`

Con estas tres reglas activas, es **imposible** mergear código que:
- No compila.
- Rompe pruebas unitarias.
- Viola las fronteras arquitectónicas.

---

## 2. `.editorconfig` – Estilo de código consistente

El archivo `.editorconfig` en la raíz del proyecto define convenciones de codificación que son respetadas automáticamente por Visual Studio, Rider y VS Code.

**Ubicación:** `.editorconfig`

### Configuraciones clave

```ini
# Indentación con espacios (4 espacios para C#)
indent_style = space
indent_size = 4

# Archivo termina con nueva línea
insert_final_newline = true

# Calificadores: preferir this. en miembros de instancia
dotnet_style_qualification_for_field = false
dotnet_style_qualification_for_property = false

# Usar var cuando el tipo es evidente
csharp_style_var_for_built_in_types = true:suggestion
csharp_style_var_when_type_is_apparent = true:suggestion
```

Estas reglas se aplican localmente en el editor. Para hacerlas obligatorias en CI, se puede agregar el analizador `dotnet format` al pipeline.

---

## 3. Roslyn Analyzers

Los proyectos de la solución pueden aprovechar los analizadores de código de Roslyn para detectar problemas en tiempo de compilación.

### Activación recomendada

Agregar en el archivo `.csproj` de los proyectos `src/`:

```xml
<PropertyGroup>
  <AnalysisMode>All</AnalysisMode>
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
</PropertyGroup>
```

Con `AnalysisMode>All`, el compilador habilita todas las reglas de análisis de código disponibles en .NET.

---

## 4. Resumen de capas de protección

La arquitectura del proyecto queda protegida por múltiples capas complementarias:

| Capa | Herramienta | Momento de detección |
|------|-------------|----------------------|
| Estilo de código | `.editorconfig` | En el editor, al escribir |
| Reglas de compilación | Roslyn Analyzers + TreatWarningsAsErrors | Al compilar localmente o en CI |
| Lógica de negocio | xUnit + Moq (Application.Tests) | En CI (job unit-tests) |
| Fronteras arquitectónicas | NetArchTest (ArchTests) | En CI (job arch-tests) |
| Merge a producción | GitHub Branch Protection | Al intentar el merge del PR |

Cada capa captura un tipo diferente de error. Juntas garantizan que el proyecto mantenga su arquitectura limpia independientemente de quién contribuya al código.
