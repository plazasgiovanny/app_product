# Ejemplos de uso GraphQL

## Requisitos previos

1. Ejecutar la API en modo Development:

```bash
dotnet run --project src/App_product.Api --launch-profile https
```

2. Asegurarse de que la base de datos tenga datos (las migraciones se aplican automáticamente en Development).

3. Abrir Nitro: [https://localhost:7185/graphql](https://localhost:7185/graphql)

---

## Consultas (Query)

### Listar productos

```graphql
query ListarProductos {
  productos {
    id
    nombre
    precio
  }
}
```

**Respuesta esperada:**

```json
{
  "data": {
    "productos": [
      {
        "id": 1,
        "nombre": "Cuaderno universitario",
        "precio": 8500.00
      }
    ]
  }
}
```

### Obtener un producto por Id

```graphql
query ProductoPorId {
  producto(id: 1) {
    id
    nombre
    descripcion
    precio
  }
}
```

### Combinar lecturas en una petición

```graphql
query VistaCombinada {
  listado: productos {
    id
    nombre
  }
  detalle: producto(id: 1) {
    id
    nombre
    descripcion
    precio
  }
}
```

---

## Mutaciones (Mutation)

### Crear producto

```graphql
mutation Crear {
  crear(
    input: {
      nombre: "Cuaderno universitario"
      descripcion: "Cuaderno de 200 hojas tamaño carta"
      precio: 8500.00
    }
  ) {
    id
    nombre
    descripcion
    precio
  }
}
```

Equivalente REST: `POST /api/v1/productos`.

### Actualizar producto

```graphql
mutation Actualizar {
  actualizar(
    id: 1
    input: {
      nombre: "Cuaderno universitario actualizado"
      descripcion: "Nueva descripción"
      precio: 9000.00
    }
  ) {
    id
    nombre
    precio
  }
}
```

Retorna el `Producto` actualizado (el resolver consulta de nuevo tras persistir). Equivalente REST: `PUT /api/v1/productos/1`.

### Eliminar producto

```graphql
mutation Eliminar {
  eliminar(id: 1)
}
```

Equivalente REST: `DELETE /api/v1/productos/1`.

---

## Errores

### Producto no encontrado

```graphql
query ProductoInexistente {
  producto(id: 99999) {
    id
    nombre
  }
}
```

```json
{
  "errors": [
    {
      "message": "No se encontró el producto con Id '99999'.",
      "extensions": {
        "code": "PRODUCTO_NO_ENCONTRADO",
        "statusCode": 404
      }
    }
  ],
  "data": null
}
```

### Error de validación

```graphql
mutation CrearInvalido {
  crear(
    input: {
      nombre: ""
      descripcion: null
      precio: -100
    }
  ) {
    id
  }
}
```

Respuesta con `code: VALIDATION_ERROR` y `statusCode: 400` (mismas reglas que REST vía `CrearProductoDtoValidator`).

---

## Probar con cURL (Windows)

```bash
curl.exe -k -X POST "https://localhost:7185/graphql" ^
  -H "Content-Type: application/json" ^
  -d "{\"query\":\"query { productos { id nombre precio } }\"}"
```

```bash
curl.exe -k -X POST "https://localhost:7185/graphql" ^
  -H "Content-Type: application/json" ^
  -d "{\"query\":\"mutation { crear(input: { nombre: \\\"Lapiz\\\", descripcion: \\\"HB\\\", precio: 500 }) { id nombre } }\"}"
```

HTTP sin TLS:

```bash
curl.exe -X POST "http://localhost:5181/graphql" -H "Content-Type: application/json" -d "{\"query\":\"query { productos { id nombre } }\"}"
```

### PowerShell (alternativa)

```powershell
$body = @{ query = "query { productos { id nombre precio } }" } | ConvertTo-Json
# -SkipCertificateCheck requiere PowerShell 7+. En Windows PowerShell 5.x usar http://localhost:5181/graphql
Invoke-RestMethod -Uri "http://localhost:5181/graphql" -Method Post -Body $body -ContentType "application/json"
```

---

## Tabla REST ↔ GraphQL

| Operación REST | GraphQL |
|---|---|
| `GET /api/v1/productos` | `query { productos { ... } }` |
| `GET /api/v1/productos/{id}` | `query { producto(id: N) { ... } }` |
| `POST /api/v1/productos` | `mutation { crear(input: {...}) { ... } }` |
| `PUT /api/v1/productos/{id}` | `mutation { actualizar(id: N, input: {...}) { ... } }` |
| `DELETE /api/v1/productos/{id}` | `mutation { eliminar(id: N) }` |

Documentación REST: [productos.md](../endpoints/productos.md).

---

## Consejos para Nitro

1. En **Schema**, confirmar el tipo `Producto` y los campos `productos` / `producto`.
2. Autocompletar sobre `Producto { ... }` para elegir campos.
3. Si `data` es `null`, revisar la sección `errors` de la respuesta.
