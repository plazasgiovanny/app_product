# Documentación de Endpoints REST

## Convenciones aplicadas

| Convención | Valor |
|---|---|
| Protocolo | HTTPS (HTTP en desarrollo) |
| Base URL | `https://localhost:5001/api/v1` |
| Versionado | URL segment: `/api/v{version}/` |
| Formato de datos | `application/json` |
| Formato de errores | `application/problem+json` (RFC 7807 ProblemDetails) |
| Autenticación | Sin autenticación (v1) |

## Herramientas para probar la API

La API puede probarse con cualquiera de las herramientas que acepta la rúbrica de la actividad:

1. **Swagger UI** (recomendada para demostración): disponible en `https://localhost:5001/swagger` cuando la app corre en modo Development. Permite ejecutar todos los endpoints directamente desde el navegador.
2. **Postman**: importar la colección manualmente o usar la URL base `https://localhost:5001/api/v1`.
3. **Insomnia**: configurar igual que Postman.

## Semántica HTTP aplicada

| Método | Seguro | Idempotente | Uso en el proyecto |
|---|---|---|---|
| GET | Sí | Sí | Listar todos / obtener por id |
| POST | No | No | Crear un nuevo producto |
| PUT | No | Sí | Actualizar completamente un producto |
| DELETE | No | Sí | Eliminar un producto |

## Manejo de errores

Todos los errores retornan `application/problem+json` con la estructura `ProblemDetails`:

```json
{
  "status": 404,
  "title": "Recurso no encontrado",
  "detail": "No se encontró el producto con Id '99'.",
  "instance": "/api/v1/productos/99"
}
```

## GraphQL (vía alternativa)

La misma lógica de negocio está disponible por GraphQL en `/graphql` (IDE Nitro en Development). Ver la documentación completa en [`docs/graphql/`](../graphql/README.md).

## Documentos

- [productos.md](productos.md) — Detalle de cada endpoint con ejemplos de request/response.
