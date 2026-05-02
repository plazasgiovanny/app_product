# Endpoints de Productos

Base URL: `/api/v1/productos`

---

## GET /api/v1/productos — Obtener todos los productos

**Descripción:** Retorna la lista completa de productos registrados en el sistema. Si no hay productos, retorna una lista vacía.

**Request:**
```http
GET /api/v1/productos HTTP/1.1
Host: localhost:5001
Accept: application/json
```

**Response exitoso (200 OK):**
```json
[
  {
    "id": 1,
    "nombre": "Cuaderno universitario",
    "descripcion": "Cuaderno de 200 hojas tamaño carta",
    "precio": 8500.00
  },
  {
    "id": 2,
    "nombre": "Lapiz HB",
    "descripcion": null,
    "precio": 500.00
  }
]
```

**Códigos de respuesta posibles:**

| Código | Descripción |
|---|---|
| 200 OK | Lista retornada exitosamente (puede ser lista vacía `[]`) |

---

## GET /api/v1/productos/{id} — Obtener un producto por Id

**Descripción:** Retorna el producto con el identificador especificado.

**Request:**
```http
GET /api/v1/productos/1 HTTP/1.1
Host: localhost:5001
Accept: application/json
```

**Response exitoso (200 OK):**
```json
{
  "id": 1,
  "nombre": "Cuaderno universitario",
  "descripcion": "Cuaderno de 200 hojas tamaño carta",
  "precio": 8500.00
}
```

**Response de error (404 Not Found):**
```json
{
  "status": 404,
  "title": "Recurso no encontrado",
  "detail": "No se encontró el producto con Id '99'.",
  "instance": "/api/v1/productos/99"
}
```

**Códigos de respuesta posibles:**

| Código | Descripción |
|---|---|
| 200 OK | Producto encontrado y retornado |
| 404 Not Found | No existe un producto con el Id indicado |

---

## POST /api/v1/productos — Crear un producto

**Descripción:** Crea un nuevo producto en el sistema. El `id` es asignado automáticamente por la base de datos. El campo `descripcion` es opcional.

**Request:**
```http
POST /api/v1/productos HTTP/1.1
Host: localhost:5001
Content-Type: application/json

{
  "nombre": "Cuaderno universitario",
  "descripcion": "Cuaderno de 200 hojas tamaño carta",
  "precio": 8500.00
}
```

**Request mínimo (sin descripción):**
```json
{
  "nombre": "Borrador",
  "descripcion": null,
  "precio": 300.00
}
```

**Response exitoso (201 Created):**
```json
{
  "id": 3,
  "nombre": "Cuaderno universitario",
  "descripcion": "Cuaderno de 200 hojas tamaño carta",
  "precio": 8500.00
}
```
Header incluido: `Location: /api/v1/productos/3`

**Response de error (400 Bad Request — validación):**
```json
{
  "status": 400,
  "title": "Error de validación",
  "detail": "El nombre del producto es obligatorio.",
  "instance": "/api/v1/productos"
}
```

**Reglas de validación:**

| Campo | Regla |
|---|---|
| `nombre` | Obligatorio, máximo 120 caracteres |
| `descripcion` | Opcional, máximo 500 caracteres |
| `precio` | Obligatorio, debe ser >= 0 |

**Códigos de respuesta posibles:**

| Código | Descripción |
|---|---|
| 201 Created | Producto creado. Header `Location` apunta al recurso creado |
| 400 Bad Request | Los datos del producto no pasan la validación |

---

## PUT /api/v1/productos/{id} — Actualizar un producto

**Descripción:** Reemplaza completamente los datos de un producto existente. El `id` se toma de la URL, no del cuerpo (convención REST). Es idempotente: ejecutar la misma solicitud varias veces produce el mismo resultado.

**Request:**
```http
PUT /api/v1/productos/1 HTTP/1.1
Host: localhost:5001
Content-Type: application/json

{
  "nombre": "Cuaderno universitario actualizado",
  "descripcion": "Nueva descripción del producto",
  "precio": 9000.00
}
```

**Response exitoso (204 No Content):**
```
HTTP/1.1 204 No Content
```
Sin cuerpo en la respuesta.

**Response de error (404 Not Found):**
```json
{
  "status": 404,
  "title": "Recurso no encontrado",
  "detail": "No se encontró el producto con Id '99'.",
  "instance": "/api/v1/productos/99"
}
```

**Códigos de respuesta posibles:**

| Código | Descripción |
|---|---|
| 204 No Content | Producto actualizado exitosamente |
| 400 Bad Request | Los datos del producto no pasan la validación |
| 404 Not Found | No existe un producto con el Id indicado |

---

## DELETE /api/v1/productos/{id} — Eliminar un producto

**Descripción:** Elimina permanentemente el producto con el identificador especificado. Es idempotente si el Id no existe (retorna 404) o existe (retorna 204 y lo elimina).

**Request:**
```http
DELETE /api/v1/productos/1 HTTP/1.1
Host: localhost:5001
```

**Response exitoso (204 No Content):**
```
HTTP/1.1 204 No Content
```
Sin cuerpo en la respuesta.

**Response de error (404 Not Found):**
```json
{
  "status": 404,
  "title": "Recurso no encontrado",
  "detail": "No se encontró el producto con Id '99'.",
  "instance": "/api/v1/productos/99"
}
```

**Códigos de respuesta posibles:**

| Código | Descripción |
|---|---|
| 204 No Content | Producto eliminado exitosamente |
| 404 Not Found | No existe un producto con el Id indicado |

---

## Resumen de todos los endpoints

| Método | Ruta | Descripción | Respuesta exitosa |
|---|---|---|---|
| GET | `/api/v1/productos` | Listar todos los productos | 200 OK |
| GET | `/api/v1/productos/{id}` | Obtener un producto por Id | 200 OK |
| POST | `/api/v1/productos` | Crear un nuevo producto | 201 Created |
| PUT | `/api/v1/productos/{id}` | Actualizar un producto | 204 No Content |
| DELETE | `/api/v1/productos/{id}` | Eliminar un producto | 204 No Content |
