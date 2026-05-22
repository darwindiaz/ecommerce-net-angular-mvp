# Plan de pruebas

## Objetivo

Validar que el MVP de e-commerce funciona de punta a punta y cumple los flujos principales definidos para la prueba tecnica.

El plan se enfoca en:

- Autenticacion y roles.
- Catalogo y detalle de productos.
- Carrito persistido en backend.
- Checkout contra entrega.
- Ordenes de cliente.
- Administracion de productos y ordenes.
- Integracion frontend-backend.
- Pruebas automatizadas minimas.

## Alcance

Incluye pruebas funcionales manuales, pruebas de integracion local y ejecucion de pruebas automatizadas existentes.

No incluye:

- Pruebas de carga.
- Pruebas de seguridad avanzadas.
- Pruebas de pasarela de pago real.
- Automatizacion E2E completa.
- Matriz extensa de navegadores.

## Ambiente de pruebas

Backend:

```text
http://localhost:5121
```

Frontend:

```text
http://localhost:4200
```

Base de datos:

```text
SQLite local
```

Navegador recomendado:

```text
Chrome o Edge
```

Comando para ejecutar backend:

```powershell
dotnet run --project backend/src/Ecommerce.API --launch-profile http
```

Comando para ejecutar frontend:

```powershell
cd frontend
npm start
```

## Pruebas automatizadas

### Backend

Comando:

```powershell
dotnet test backend\Ecommerce.sln
```

Resultado esperado:

- Todas las pruebas de xUnit deben pasar.
- No deben existir errores de compilacion.

### Frontend

Comando:

```powershell
cd frontend
npm test -- --watch=false --browsers=ChromeHeadless
```

Resultado esperado:

- Las pruebas Angular deben pasar.
- No deben existir errores de compilacion del frontend.

## Casos de prueba manuales

| ID | Modulo | Caso | Pasos | Resultado esperado | Estado |
| --- | --- | --- | --- | --- | --- |
| AUTH-01 | Autenticacion | Registrar cliente | Abrir registro, completar datos validos y enviar formulario. | Usuario creado, sesion iniciada y redireccion al catalogo. | Pendiente |
| AUTH-02 | Autenticacion | Login valido | Ingresar email y contrasena validos. | Usuario autenticado y redireccion al catalogo. | Pendiente |
| AUTH-03 | Autenticacion | Login invalido | Ingresar credenciales incorrectas. | Se muestra mensaje de error y no se inicia sesion. | Pendiente |
| CAT-01 | Catalogo | Listar productos | Abrir pantalla de catalogo. | Se muestran productos con imagen, nombre, disponibilidad y precio. | Pendiente |
| CAT-02 | Catalogo | Filtrar productos | Aplicar filtros por nombre, descripcion, codigo, talla o color. | El listado muestra productos que coinciden con los filtros. | Pendiente |
| PROD-01 | Producto | Ver detalle | Abrir un producto desde el catalogo. | Se muestra codigo, imagen, nombre, descripcion, talla, color, stock y precio. | Pendiente |
| CART-01 | Carrito | Agregar producto | Seleccionar cantidad y agregar producto al carrito. | Producto agregado y contador del carrito actualizado. | Pendiente |
| CART-02 | Carrito | Actualizar cantidad | Cambiar cantidad de un item en carrito. | Subtotal y total se actualizan correctamente. | Pendiente |
| CART-03 | Carrito | Eliminar producto | Eliminar un item del carrito. | Producto eliminado y total actualizado. | Pendiente |
| ORDER-01 | Ordenes | Finalizar compra | Con productos en carrito, ejecutar checkout. | Se crea una orden en estado `InProcess` y el carrito queda limpio. | Pendiente |
| ORDER-02 | Ordenes | Listar ordenes cliente | Abrir pantalla de ordenes con usuario cliente. | Se muestran las ordenes del usuario autenticado. | Pendiente |
| ADMIN-01 | Administracion | Bloquear acceso sin rol | Intentar entrar al panel admin sin rol administrador. | El sistema redirige a catalogo o impide el acceso. | Pendiente |
| ADMIN-02 | Administracion | Crear producto | Ingresar como admin, completar formulario de producto y guardar. | Producto creado y visible en el listado. | Pendiente |
| ADMIN-03 | Administracion | Editar producto | Seleccionar producto, modificar datos y guardar. | Producto actualizado correctamente. | Pendiente |
| ADMIN-04 | Administracion | Eliminar producto | Eliminar un producto desde admin. | Producto eliminado del listado. | Pendiente |
| ADMIN-05 | Administracion | Actualizar estado de orden | Cambiar estado de una orden desde admin. | Orden actualizada con el nuevo estado. | Pendiente |

## Pruebas de integracion frontend-backend

Validar los siguientes puntos durante la navegacion local:

- Angular consume el backend usando rutas relativas `/api`.
- El proxy de Angular redirige correctamente al backend local.
- El token JWT se envia en endpoints protegidos.
- Las rutas protegidas redirigen si no hay sesion activa.
- El rol `Admin` permite acceder a funcionalidades administrativas.
- El carrito persiste en backend por usuario autenticado.
- El checkout crea una orden y descuenta stock.
- Las ordenes se consultan desde SQLite local.

## Criterios de aceptacion

La fase se considera aprobada si:

- Backend compila correctamente.
- Frontend compila correctamente.
- Pruebas automatizadas backend pasan.
- Pruebas automatizadas frontend pasan.
- Un cliente puede registrarse, iniciar sesion, agregar productos al carrito y finalizar una compra.
- Un cliente puede consultar sus ordenes.
- Un administrador puede crear, editar y eliminar productos.
- Un administrador puede actualizar estados de orden.
- No se observan errores visibles en consola durante los flujos principales.
- El README permite levantar el proyecto localmente sin pasos adicionales no documentados.

## Riesgos y limitaciones

- No hay pasarela de pago real; el pago es contra entrega.
- No se incluyen pruebas de carga.
- No se incluyen pruebas de seguridad avanzadas.
- La UI es funcional para MVP, no representa un diseno final de produccion.
- SQLite es adecuado para ejecucion local y MVP, pero no para escenarios productivos de alta concurrencia.
- El flujo administrador depende de contar con un usuario con rol `Admin`.
