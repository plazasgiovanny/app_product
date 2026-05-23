# Documentación GraphQL

Esta sección describe la **segunda vía de ejecución** de **App Product API**: un servidor GraphQL construido con [Hot Chocolate](https://chillicream.com/) (ChilliCream) que coexiste con los endpoints REST sin modificar su comportamiento.

El esquema está **centrado en el recurso `Producto`**: las lecturas usan sustantivos (`productos`, `producto`) y el tipo expuesto en el contrato se llama `Producto`, no `ProductoDto`.

## Documentos

| Documento | Descripción |
|---|---|
| [01-que-es-graphql.md](01-que-es-graphql.md) | Qué es GraphQL, en qué se diferencia de REST y cómo aporta valor al proyecto |
| [02-arquitectura-limpia.md](02-arquitectura-limpia.md) | Cómo GraphQL respeta las capas, SOLID y la Regla de Dependencia |
| [03-implementacion-hot-chocolate.md](03-implementacion-hot-chocolate.md) | Paso a paso de la implementación, resolvers, Hot Chocolate y manejo de errores |
| [04-ejemplos-uso.md](04-ejemplos-uso.md) | Consultas y mutaciones de ejemplo con Nitro y cURL |

## Esquema resumido

```graphql
type Query {
  productos: [Producto!]!
  producto(id: Int!): Producto!
}

type Mutation {
  crear(input: ProductoCrearInput!): Producto!
  actualizar(id: Int!, input: ProductoActualizarInput!): Producto!
  eliminar(id: Int!): Boolean!
}
```

## Acceso rápido

| Recurso | URL (Development) |
|---|---|
| IDE Nitro (explorador GraphQL) | `https://localhost:7185/graphql` |
| Endpoint HTTP (alternativa) | `http://localhost:5181/graphql` |
| REST (sin cambios) | `https://localhost:7185/api/v1/productos` |
| Swagger REST | `https://localhost:7185/swagger` |

Puertos definidos en [`launchSettings.json`](../../src/App_product.Api/Properties/launchSettings.json).

## Estructura de código

```
src/App_product.Api/
└── GraphQL/
    ├── Query.cs
    ├── Mutation.cs
    ├── Types/
    │   └── ProductoType.cs
    ├── Inputs/
    │   ├── ProductoCrearInput.cs
    │   └── ProductoActualizarInput.cs
    ├── Mapping/
    │   └── ProductoInputMapper.cs
    └── Errors/
        └── GraphQLDomainErrorFilter.cs
```

## Notas de implementación

- `Query` y `Mutation` están registrados como servicios **scoped** en DI (requerido con EF Core).
- El campo GraphQL `producto` se resuelve con el método C# `ProductoPorId` y el atributo `[GraphQLType(typeof(ProductoType))]`.

## Relación con otras secciones de docs

- Endpoints REST: [`docs/endpoints/`](../endpoints/README.md)
- Arquitectura C4 y SOLID: [`docs/arquitectura/`](../arquitectura/README.md)
- Guardrails (Regla 11 GraphQL): [`docs/arquitectura/guardrails/`](../arquitectura/guardrails/README.md)
