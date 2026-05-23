# ¿Qué es GraphQL?

## Definición

**GraphQL** es un lenguaje de consulta y un **protocolo de transporte** para APIs, creado por Facebook (ahora Meta) y estandarizado por la [GraphQL Foundation](https://graphql.org/). A diferencia de REST, donde cada recurso tiene su propia URL y forma de respuesta fija, GraphQL expone un **único endpoint** (en este proyecto: `/graphql`) y el cliente **declara exactamente qué datos necesita**.

Una petición GraphQL típica es un documento JSON con dos partes:

```json
{
  "query": "query { productos { id nombre precio } }"
}
```

El esquema de este proyecto nombra el **recurso** (`productos`, `producto`) y el **tipo de salida** (`Producto`), en lugar de exponer métodos estilo RPC (`getProductos`, `getProductoById`).

El servidor interpreta esa consulta, ejecuta los **resolvers** correspondientes y devuelve un JSON con la misma forma que pidió el cliente.

## Conceptos fundamentales

| Concepto | Descripción |
|---|---|
| **Schema** | Contrato tipado de lo que la API puede hacer (tipos, campos, argumentos). |
| **Query** | Operación de **solo lectura** (equivalente semántico a GET en REST). |
| **Mutation** | Operación de **escritura** (crear, actualizar, eliminar). |
| **Resolver** | Función que obtiene el valor de un campo del esquema. En este proyecto, cada método público de `Query` o `Mutation` es un resolver. |
| **Input** | Tipo de entrada para mutaciones (similar a un DTO de request, pero definido en la capa de transporte). |

## GraphQL vs REST en este proyecto

| Aspecto | REST (`/api/v1/productos`) | GraphQL (`/graphql`) |
|---|---|---|
| Endpoints | Uno por operación (5 rutas) | Uno para todas las operaciones |
| Forma de respuesta | Fija por endpoint | El cliente elige campos |
| Versionado | URL (`/api/v1/`) | Evolución del esquema (sin segmento de versión en URL) |
| Errores | `ProblemDetails` (RFC 7807) | Array `errors[]` con `message`, `code` y `extensions` |
| IDE de prueba | Swagger UI | Nitro (integrado en Hot Chocolate) |
| Capa de negocio | `IProductoService` | **El mismo** `IProductoService` |

Ambos protocolos comparten la misma base de datos, los mismos casos de uso y las mismas reglas de validación.

## ¿Cómo nos ayuda en App Product?

1. **Consultas flexibles:** un cliente móvil puede pedir solo `id` y `nombre`; un panel administrativo puede pedir todos los campos en una sola petición, sin crear endpoints adicionales.

2. **Menos sobre-fetching:** REST en `GET /productos` siempre devuelve todos los campos del `ProductoDto`. GraphQL permite pedir solo lo necesario.

3. **Agregación en el cliente:** varias operaciones de lectura pueden combinarse en un solo documento GraphQL (por ejemplo, listar productos y obtener uno por id en la misma request).

4. **Coexistencia sin reescritura:** se añadió GraphQL en la capa `Api` sin tocar `Domain`, `Application` ni `Infrastructure`. Los equipos que prefieren REST siguen usando Swagger; los que prefieren GraphQL usan Nitro.

5. **Misma lógica de negocio:** no hay duplicación de reglas en resolvers; todo pasa por `IProductoService`, igual que el controlador REST.

## Limitaciones a tener en cuenta

- GraphQL no reemplaza automáticamente el versionado por URL de REST; los cambios breaking del esquema deben gestionarse con disciplina (deprecaciones, campos opcionales).
- El cache HTTP estándar (CDN, proxies) funciona mejor con GET REST idempotentes; GraphQL suele usar POST.
- En este proyecto v1, **no hay autenticación** en ninguno de los dos protocolos.

## Herramienta utilizada: Hot Chocolate

[Hot Chocolate](https://chillicream.com/docs/hotchocolate/v14) es el servidor GraphQL para .NET mantenido por ChilliCream. Ofrece:

- Enfoque **code-first** (el esquema se infiere de clases C#).
- Integración con **ASP.NET Core** (`AddGraphQLServer`, `MapGraphQL`).
- IDE **Nitro** embebido en Development.
- Filtros de error (`IErrorFilter`) para traducir excepciones a errores GraphQL estructurados.

El paquete `HotChocolate.AspNetCore` está instalado **únicamente** en `App_product.Api`.
