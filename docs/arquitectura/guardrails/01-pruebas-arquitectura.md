# Pruebas de Arquitectura con NetArchTest

## ¿Qué es NetArchTest?

[NetArchTest](https://github.com/BenMorris/NetArchTest) es una biblioteca para .NET que permite escribir pruebas xUnit/MSTest/NUnit que inspeccionan los ensamblados compilados y verifican reglas de estructura y dependencia.

Funciona leyendo los metadatos de los archivos `.dll` en tiempo de ejecución, por lo que **no depende del código fuente** sino del binario real.

## Ubicación del proyecto

```
tests/
└── App_product.ArchTests/
    ├── App_product.ArchTests.csproj
    └── Architecture/
        └── CapasTests.cs          ← 11 reglas de arquitectura
```

## Paquete NuGet utilizado

```xml
<PackageReference Include="NetArchTest.Rules" Version="1.3.2" />
```

## Las 11 Reglas implementadas

### Reglas de aislamiento del Dominio (1–3)

El núcleo del negocio es el activo más valioso. **No debe conocer nada** de cómo se almacenan los datos, ni de cómo se exponen.

| Regla | Descripción |
|-------|-------------|
| 1 | `Domain` no depende de `Application` |
| 2 | `Domain` no depende de `Infrastructure` |
| 3 | `Domain` no depende de `Api` |

```csharp
// Ejemplo de cómo se verifica la Regla 1
var result = Types.InAssembly(typeof(Producto).Assembly)
    .ShouldNot()
    .HaveDependencyOn("App_product.Application")
    .GetResult();

Assert.True(result.IsSuccessful, ...);
```

### Reglas de aislamiento de Application (4–5)

La capa de aplicación orquesta casos de uso a través de **interfaces**. Nunca debe conocer las implementaciones concretas.

| Regla | Descripción |
|-------|-------------|
| 4 | `Application` no depende de `Infrastructure` |
| 5 | `Application` no depende de `Api` |

### Reglas de aislamiento de Infrastructure (6)

La infraestructura puede conocer el dominio y la aplicación (para implementar sus interfaces), pero nunca la capa de presentación.

| Regla | Descripción |
|-------|-------------|
| 6 | `Infrastructure` no depende de `Api` |

### Reglas de ubicación de tipos (7–10)

Garantizan que los artefactos característicos de cada capa residan exactamente donde la arquitectura lo indica.

| Regla | Descripción |
|-------|-------------|
| 7 | Las clases concretas cuyo nombre termina en `Repository` deben estar en `Infrastructure` |
| 8 | Las clases cuyo nombre termina en `Controller` deben estar en `Api` |
| 9 | Las clases cuyo nombre termina en `Dto` deben estar en `Application` |
| 10 | Las entidades dentro del namespace `Domain.Entities` deben residir en `Domain` |

### Regla de ubicación de GraphQL (11)

Garantiza que GraphQL permanezca en la capa de presentación.

| Regla | Descripción |
|-------|-------------|
| 11 | Los tipos en `App_product.Api.GraphQL` y las clases cuyo nombre termina en `Query` o `Mutation` deben estar en `Api` |

Documentación detallada: [`docs/graphql/02-arquitectura-limpia.md`](../../graphql/02-arquitectura-limpia.md).

## Cómo ejecutar las pruebas

```bash
dotnet test tests/App_product.ArchTests/App_product.ArchTests.csproj --verbosity normal
```

Salida esperada:

```
Pruebas totales: 11
     Correcto: 11
```

## Cómo agregar nuevas reglas

1. Abrir `tests/App_product.ArchTests/Architecture/CapasTests.cs`.
2. Agregar un nuevo método `[Fact]` siguiendo el patrón existente.
3. Usar la API fluida de `NetArchTest.Rules` (`Types.InAssembly(...).That()...Should()...GetResult()`).
4. Ejecutar para verificar que la nueva regla pasa antes de hacer commit.

## Qué sucede si una regla falla

El mensaje de error de xUnit indica exactamente qué tipos violan la regla:

```
Application tiene dependencias no permitidas en Infrastructure:
App_product.Application.Services.ProductoService
```

Esto facilita la corrección inmediata.
