# Contrato API

Base URL local esperada:

```text
https://localhost:{port}/api
```

## Auth

### POST /auth/register

Registra un usuario cliente.

Request:

```json
{
  "names": "Juan",
  "lastNames": "Perez",
  "age": 30,
  "birthDate": "1996-01-10",
  "country": "Colombia",
  "department": "Antioquia",
  "city": "Medellin",
  "phone": "3000000000",
  "address": "Calle 1 # 2-3",
  "email": "juan@example.com",
  "password": "Password123*"
}
```

### POST /auth/login

Autentica un usuario.

Request:

```json
{
  "email": "juan@example.com",
  "password": "Password123*"
}
```

Response esperado:

```json
{
  "token": "jwt-token",
  "email": "juan@example.com",
  "role": "Customer"
}
```

## Products

### GET /products

Lista productos con filtros opcionales.

Query params:

```text
name
description
code
size
color
```

### GET /products/{id}

Obtiene detalle de producto.

### POST /products

Crea producto. Requiere rol `Admin`.

### PUT /products/{id}

Actualiza producto. Requiere rol `Admin`.

### DELETE /products/{id}

Elimina producto. Requiere rol `Admin`.

## Cart

### GET /cart

Obtiene el carrito del usuario autenticado.

### POST /cart/items

Agrega producto al carrito.

Request:

```json
{
  "productId": "guid",
  "quantity": 1
}
```

### PUT /cart/items

Actualiza cantidad.

Request:

```json
{
  "cartItemId": "guid",
  "quantity": 2
}
```

### DELETE /cart/items/{id}

Elimina item del carrito.

## Orders

### POST /orders

Finaliza compra del carrito usando pago contra entrega.

### GET /orders

Lista ordenes.

- Cliente: solo sus ordenes.
- Admin: puede ver todas.

### PUT /orders/{id}/status

Actualiza estado. Requiere rol `Admin`.

Request:

```json
{
  "status": "Shipped"
}
```

### DELETE /orders/{id}

Elimina orden. Requiere rol `Admin`.

## Estados HTTP esperados

- `200 OK`: consulta o actualizacion exitosa.
- `201 Created`: recurso creado.
- `400 Bad Request`: request invalido.
- `401 Unauthorized`: usuario no autenticado.
- `403 Forbidden`: usuario sin permisos.
- `404 Not Found`: recurso inexistente.
- `409 Conflict`: conflicto de negocio, por ejemplo stock insuficiente o email duplicado.

