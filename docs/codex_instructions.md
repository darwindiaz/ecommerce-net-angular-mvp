# PRUEBA TÉCNICA — SISTEMA E-COMMERCE MVP

## Contexto para Codex

Actúa como un desarrollador Fullstack Senior especializado en:

- .NET
- Angular
- Clean Architecture
- Entity Framework
- SQL
- APIs REST
- Buenas prácticas de ingeniería

No generes soluciones rápidas sin estructura. Prioriza:

1. Arquitectura mantenible
2. Código limpio
3. Consistencia
4. Extensibilidad futura
5. Simplicidad para un MVP

No sobreingenierizar.

NO usar:

- Microservicios
- Event sourcing
- CQRS complejo
- DDD extremo
- Kubernetes
- Redis
- Infraestructura innecesaria

El objetivo es construir un MVP sólido y defendible.

---

# Stack requerido

Backend:

- .NET 10 o superior
- Entity Framework Core
- SQLite
- JWT Authentication
- Swagger

Frontend:

- Angular 20+
- Signals
- Observables
- Reactive Forms

Otros:

- Git
- Conventional Commits

---

# REQUISITOS FUNCIONALES

## 1. Autenticación y usuarios

Implementar:

Registro de usuarios:

Campos:

- nombres
- apellidos
- edad
- fecha nacimiento
- país
- departamento
- ciudad
- celular
- dirección
- email
- contraseña

Login:

- email
- contraseña

Roles:

- Cliente
- Administrador

---

## 2. Productos

Cada producto:

- código único
- imagen
- nombre
- descripción
- talla
- color
- precio
- cantidad stock

Tallas:

- 7
- 8
- 9
- 10

Colores:

- blanco
- negro
- gris

Crear datos semilla:

mínimo:

10 productos

---

## 3. Catálogo

Debe permitir:

Listar:

- imagen
- nombre
- disponibilidad
- precio

Filtros:

- nombre
- descripción
- código
- talla
- color

Permitir:

- seleccionar cantidades
- agregar múltiples productos al carrito

---

## 4. Detalle producto

Mostrar:

- toda la información disponible

---

## 5. Carrito

Funciones:

- listar productos
- valor unitario
- cantidad
- subtotal
- total

Permitir:

- editar cantidades
- eliminar productos
- finalizar compra

Pago:

- Contra entrega
- Sin pasarela

---

## DECISIÓN ARQUITECTÓNICA IMPORTANTE

Persistir carrito en backend.

Razones:

- permite múltiples dispositivos
- mantiene sesión
- facilita escalabilidad futura
- facilita auditoría

---

## 6. Panel administrador

CRUD completo productos:

- crear
- editar
- eliminar
- listar

Órdenes:

Ver:

- listado
- filtros por estado

Estados:

EnProceso
Pagado
Enviado
Entregado

Permitir:

- actualizar estado
- eliminar órdenes

---

# REQUISITOS NO FUNCIONALES

Aplicar:

- SOLID
- Clean Code
- Clean Architecture

Patrones sugeridos:

- Repository
- MediatR
- Strategy
- Factory
- Dependency Injection

---

# EXTENSIBILIDAD FUTURA

El sistema deberá soportar en el futuro:

## Autenticación

Diseñar:

```csharp
interface IAuthProvider
{
   Task<AuthResult> Authenticate();
}
```

Ejemplos futuros:

- JwtAuthProvider
- GoogleAuthProvider
- MicrosoftAuthProvider

## Pagos

Diseñar:

```csharp
interface IPaymentProvider
{
   Task<PaymentResult> Process();
}
```

Ejemplos futuros:

- CashOnDeliveryProvider
- StripeProvider
- PaypalProvider

NO acoplar lógica directamente.

---

# ARQUITECTURA BACKEND

Crear solución:

```text
backend/

Ecommerce.sln

src/

Ecommerce.API
Ecommerce.Application
Ecommerce.Domain
Ecommerce.Infrastructure

tests/

Ecommerce.Tests
```

---

# RESPONSABILIDADES

## Domain

Contiene:

- Entities
- Enums
- ValueObjects

Sin dependencias externas.

---

## Application

Contiene:

- DTOs
- Commands
- Queries
- Validators
- Interfaces
- Services

---

## Infrastructure

Contiene:

- EF Core
- Repositories
- Persistence
- External Services

---

## API

Contiene:

- Controllers
- JWT
- Swagger
- Middleware
- Dependency Injection

---

# MODELO DE DOMINIO

Crear entidades:

## User

```csharp
Id
Names
LastNames
Age
BirthDate
Country
Department
City
Phone
Address
Email
PasswordHash
Role
```

## Product

```csharp
Id
Code
ImageUrl
Name
Description
Size
Color
Price
Stock
```

## Cart

```csharp
Id
UserId
```

## CartItem

```csharp
Id
CartId
ProductId
Quantity
```

## Order

```csharp
Id
UserId
Total
Status
CreatedAt
```

## OrderItem

```csharp
Id
OrderId
ProductId
Price
Quantity
```

---

# ENUMS

```csharp
enum UserRole
{
    Admin,
    Customer
}

enum ProductSize
{
    Seven=7,
    Eight=8,
    Nine=9,
    Ten=10
}

enum ProductColor
{
    White,
    Black,
    Gray
}

enum OrderStatus
{
    InProcess,
    Paid,
    Shipped,
    Delivered
}
```

---

# API ENDPOINTS ESPERADOS

## Auth

```text
POST /api/auth/register

POST /api/auth/login
```

---

## Products

```text
GET /api/products

GET /api/products/{id}

POST /api/products

PUT /api/products/{id}

DELETE /api/products/{id}
```

---

## Cart

```text
GET /api/cart

POST /api/cart/items

PUT /api/cart/items

DELETE /api/cart/items/{id}
```

---

## Orders

```text
POST /api/orders

GET /api/orders

PUT /api/orders/{id}/status

DELETE /api/orders/{id}
```

---

# BASE DE DATOS

Usar:

SQLite

Configurar:

- PK
- FK
- relaciones
- restricciones

Generar:

- migraciones
- seed data

---

# FRONTEND

Estructura:

```text
frontend/

src/app/

core
shared
features
```

---

## Core

Contiene:

```text
guards
services
interceptors
store
```

---

## Shared

Contiene:

```text
components
pipes
models
```

---

## Features

Crear:

```text
auth
products
cart
orders
admin
```

---

# ESTADO FRONTEND

Usar:

- Signals
- RxJS

NO usar NgRx.

Ejemplo:

```ts
@Injectable()
export class CartStore {
  products = signal([]);

  total = computed(() => {});
}
```

---

# PANTALLAS

Cliente:

- Login
- Registro
- Catálogo
- Detalle producto
- Carrito
- Checkout

Administrador:

- Dashboard
- Productos CRUD
- Gestión órdenes

---

# FORMULARIOS

Usar exclusivamente:

Reactive Forms

Agregar:

- validaciones
- mensajes de error
- manejo de errores HTTP

---

# RESPONSIVE

Debe funcionar:

- Desktop
- Tablet
- Mobile

No invertir tiempo excesivo en diseño visual complejo.

---

# TESTS

Implementar mínimo:

Backend:

xUnit

Casos sugeridos:

```csharp
ShouldCreateOrder()

ShouldRejectInvalidLogin()

ShouldRejectInsufficientStock()
```

Frontend:

mínimo:

1 test de componente Angular

---

# GIT

Usar Conventional Commits, se muy especifico y espera la confirmacion para subir el commit:

Ejemplos:

```bash
feat(auth): implement jwt login

feat(product): add product crud

feat(cart): implement cart service

feat(order): add order workflow

fix(auth): validate token expiration

docs(readme): add setup instructions
```

Usa ramas para cada fase

Ejemplos:

Rama docs
Rama backend
Rama frontend

## suguierele estrucutra

# FORMA DE TRABAJO CON CODEX

Trabajar por tareas pequeñas.

NO generar todo el proyecto de una vez.

Orden:

FASE 1
1 Crear estructura de proyecto
2 Documentacion (MER, Arquitectura)

FASE 2

1 Crear solución backend
2 Crear Angular app
3 Configurar referencias

FASE 3

1 Crear entidades
2 Configurar DbContext
3 Crear migraciones
4 Crear seed data

FASE 4

1 Implementar Auth

FASE 5

1 Implementar Products

FASE 6

1 Implementar Cart

FASE 7

1 Implementar Orders

FASE 8

1 Implementar Angular

FASE 9

1 Integrar frontend-backend

FASE 10

1 Corregir errores
2 Refactorizar
3 Documentacion requerida (Actualizacion de readme.md, requerimientos y paso a paso para desplegar todo localmente)

---

# REGLAS IMPORTANTES

- No eliminar código existente sin justificación.
- Explicar cambios grandes antes de aplicarlos.
- Mantener consistencia.
- Evitar duplicación.
- Mantener Clean Architecture.
- No crear archivos innecesarios.
- Mantener código compilable.
- Verificar imports y dependencias.
- Después de cada tarea indicar:
  - qué se creó
  - qué falta
  - siguiente paso recomendado
