# Ecommerce .NET + Angular MVP

MVP de e-commerce desarrollado con .NET y Angular. El objetivo es demostrar un flujo completo de compra en tiempo reducido, manteniendo una arquitectura simple, mantenible y defendible para una prueba tecnica.

## Que es

Es una aplicacion web de comercio electronico que permite a un cliente registrarse, iniciar sesion, consultar productos, agregarlos a un carrito persistido en backend y finalizar una compra contra entrega.

Tambien incluye un panel administrador para gestionar productos y ordenes.

## Que hace

- Registro e inicio de sesion con JWT.
- Catalogo de productos con filtros por nombre, descripcion, codigo, talla y color.
- Detalle de producto.
- Carrito persistido en backend.
- Edicion de cantidades y eliminacion de productos del carrito.
- Checkout contra entrega, sin pasarela de pago.
- Panel administrador para CRUD de productos.
- Gestion de ordenes con actualizacion de estado.

## Arquitectura

El backend usa Clean Architecture para separar responsabilidades:

- **Domain**: entidades y enums del negocio.
- **Application**: casos de uso, servicios, DTOs e interfaces.
- **Infrastructure**: Entity Framework Core, SQLite, repositorios y servicios tecnicos.
- **API**: controllers, autenticacion JWT, CORS y configuracion HTTP.

El frontend usa Angular con una estructura por responsabilidades:

- **Core**: servicios HTTP, guards, interceptor y stores.
- **Shared**: modelos compartidos.
- **Features**: pantallas por modulo funcional como auth, products, cart, orders y admin.

## Tecnologias y versiones minimas

- .NET 10 o superior.
- ASP.NET Core Web API.
- Entity Framework Core.
- SQLite.
- Angular 20 o superior.
- Angular CLI 20 o superior.
- Node.js 20 o superior.
- RxJS.
- xUnit.

## Configuracion local

Clona el repositorio y ubicate en la carpeta raiz:

```powershell
cd D:\REPOS\ecommerce-net-angular-mvp
```

Restaura y compila el backend:

```powershell
dotnet restore backend\Ecommerce.sln
dotnet build backend\Ecommerce.sln
```

Instala dependencias del frontend:

```powershell
cd frontend
npm install
```

La base de datos usa SQLite. La cadena de conexion local esta configurada en:

```text
backend/src/Ecommerce.API/appsettings.json
```

## Ejecucion local

Usa dos terminales.

Terminal 1: backend

```powershell
dotnet run --project backend/src/Ecommerce.API --launch-profile http
```

El backend queda disponible en:

```text
http://localhost:5121
```

Terminal 2: frontend

```powershell
cd frontend
npm start
```

El frontend queda disponible en:

```text
http://localhost:4200
```

Si el puerto `4200` esta ocupado, ejecuta:

```powershell
npm start -- --host 127.0.0.1 --port 4201
```

Y abre:

```text
http://127.0.0.1:4201
```

## Pruebas

Backend:

```powershell
dotnet test backend\Ecommerce.sln
```

Frontend:

```powershell
cd frontend
npm test -- --watch=false --browsers=ChromeHeadless
```

## Usuarios y roles

El sistema maneja dos roles:

- **Customer**: cliente que puede comprar productos.
- **Admin**: administrador que puede gestionar productos y ordenes.

Los usuarios cliente se crean desde la pantalla de registro.

## Notas tecnicas

- La autenticacion usa JWT.
- El frontend agrega el token automaticamente con un interceptor HTTP.
- Los guards protegen rutas que requieren sesion y rol administrador.
- El carrito se persiste en backend para conservar la informacion por usuario.
- El checkout usa pago contra entrega, sin integracion con pasarela externa.
- Angular usa un proxy local para enviar las llamadas `/api` al backend.
- El backend tiene CORS configurado para desarrollo local.

## Decisiones arquitectonicas para exposicion

### Por que Clean Architecture

Se uso Clean Architecture para separar reglas de negocio, infraestructura y API. Esto permite cambiar detalles tecnicos como base de datos, autenticacion o servicios externos sin reescribir el dominio principal.

### Por que SQLite

SQLite reduce la configuracion local y permite entregar un MVP funcional rapidamente. Para una prueba tecnica es suficiente, liviano y facil de ejecutar sin instalar un servidor de base de datos.

### Por que carrito persistido en backend

Persistir el carrito en backend permite conservar la informacion por usuario, soportar multiples dispositivos y facilitar auditoria futura. Tambien evita depender solo del almacenamiento del navegador.

### Por que JWT

JWT permite autenticar peticiones HTTP de forma simple y compatible con APIs REST. El frontend guarda la sesion y envia el token en cada solicitud protegida.

### Por que proxy en Angular

El proxy simplifica el desarrollo local. Angular llama a `/api` y el servidor de desarrollo redirige esas peticiones al backend. Esto evita problemas de puertos y facilita cambiar la URL real del API por ambiente.

### Por que pago contra entrega

El pago contra entrega permite cerrar el flujo de compra sin integrar una pasarela externa. La arquitectura deja una interfaz de proveedor de pago para agregar Stripe, PayPal u otro proveedor en el futuro.
