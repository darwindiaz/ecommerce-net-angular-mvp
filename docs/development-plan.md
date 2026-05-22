# Plan de desarrollo

## Forma de trabajo

El proyecto se desarrollara por tareas pequenas y verificables.

Cada fase debe cerrar indicando:

- Que se creo.
- Que falta.
- Siguiente paso recomendado.

## Fase 1: estructura inicial

Objetivo:

- Crear estructura base del repositorio.
- Documentar arquitectura.
- Documentar modelo entidad relacion.
- Documentar contrato inicial de API.

No incluye:

- Crear solucion .NET.
- Crear aplicacion Angular.
- Implementar codigo funcional.

## Fase 2: scaffolding tecnico

Objetivo:

- Crear solucion backend.
- Crear proyectos .NET.
- Crear aplicacion Angular.
- Configurar referencias entre proyectos.

## Fase 3: dominio y persistencia

Objetivo:

- Crear entidades.
- Crear enums.
- Configurar DbContext.
- Crear migraciones.
- Crear seed data de minimo 10 productos.

## Fase 4: autenticacion

Objetivo:

- Registro.
- Login.
- JWT.
- Roles.

## Fase 5: productos

Objetivo:

- CRUD de productos.
- Filtros de catalogo.
- Validaciones de stock, precio, talla y color.

## Fase 6: carrito

Objetivo:

- Carrito persistido en backend.
- Agregar productos.
- Editar cantidades.
- Eliminar items.
- Calcular subtotales y total.

## Fase 7: ordenes

Objetivo:

- Crear orden desde carrito.
- Pago contra entrega.
- Actualizar estados.
- Listar y filtrar ordenes.

## Fase 8: Angular

Objetivo:

- Estructura Angular por `core`, `shared` y `features`.
- Pantallas cliente.
- Pantallas administrador.
- Reactive Forms.
- Signals y RxJS.

## Fase 9: integracion

Objetivo:

- Integrar frontend con backend.
- Manejo de token.
- Interceptor HTTP.
- Guards de autenticacion y roles.

## Fase 10: estabilizacion

Objetivo:

- Corregir errores.
- Refactorizar.
- Completar README (Sugerencia detalles y requisitos minimos)
- Documentar ejecucion local.
- Ejecutar tests minimos.
- Crear plan de pruebas.

## Estrategia para tiempo record

- Priorizar flujo completo antes que diseno visual avanzado.
- Mantener controllers delgados.
- Mantener servicios de aplicacion simples.
- Evitar CQRS completo mientras no aporte valor directo.
- Usar SQLite para reducir configuracion local.
- Mantener interfaces en puntos de extension reales: pagos, autenticacion y repositorios.
