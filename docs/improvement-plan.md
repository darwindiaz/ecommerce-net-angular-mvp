# Plan de mejoras

## Contexto

Durante la fase de supervision humana del MVP se identificaron incidencias y oportunidades de mejora en funcionalidad, experiencia de usuario, datos de prueba y calidad tecnica.

El objetivo de este documento es priorizar los hallazgos, definir cuales se implementan como ajustes finales y cuales quedan como mejoras futuras.

## Criterios de priorizacion

| Criterio | Descripcion |
| --- | --- |
| Impacto en demo | Que tanto afecta la presentacion o validacion del MVP. |
| Impacto funcional | Que tanto afecta un requisito principal del sistema. |
| Valor para usuario | Que tanto mejora la experiencia de cliente o administrador. |
| Esfuerzo | Tiempo estimado y complejidad para implementarlo. |
| Riesgo tecnico | Probabilidad de romper funcionalidades existentes. |

## Clasificacion

### Vitales para cierre del MVP

| ID | Mejora | Justificacion |
| --- | --- | --- |
| IMP-01 | Crear credenciales para administrador por defecto | Permite demostrar el panel administrador sin manipulaciones manuales de base de datos. |
| IMP-02 | Mostrar tallas como numeros | Evita mostrar valores tecnicos como `Seven`, `Eight`, `Nine`, `Ten` al usuario final. |
| IMP-03 | Mostrar colores en espanol | Mejora coherencia de interfaz para usuarios hispanohablantes. |
| IMP-04 | Mostrar carrito con badge visual | Mejora la lectura del estado del carrito y evita exponer una visualizacion poco clara del contador. |
| IMP-05 | Calcular edad automaticamente desde fecha de nacimiento | Evita inconsistencias entre edad y fecha ingresadas por el usuario. |

### Importantes no bloqueantes

| ID | Mejora | Justificacion |
| --- | --- | --- |
| IMP-06 | Mejorar validaciones de formularios | Formatos de celular y contrasena mejoran calidad de datos, pero el flujo principal ya funciona. |
| IMP-07 | Unificar busqueda de productos | Mejora la experiencia del catalogo, pero requiere ajustar criterios de filtrado y comportamiento visual. |
| IMP-08 | Paginacion o carga progresiva de productos | Importante para escalabilidad, aunque el MVP actualmente maneja un set pequeno de productos. |
| IMP-09 | Cambiar imagenes de productos | Mejora la percepcion visual, pero no afecta la funcionalidad principal del MVP. |

### Mejoras futuras

| ID | Mejora | Justificacion |
| --- | --- | --- |
| IMP-10 | Ampliar pruebas frontend con Jasmine/Karma y backend con xUnit | Aumenta confianza tecnica, pero ya existen pruebas minimas automatizadas. |
| IMP-11 | Evaluar Material Design o Bootstrap | Puede mejorar consistencia visual, pero introducir una libreria al final del MVP aumenta riesgo y tiempo. |

## Ajustes propuestos para Fase 11

Se propone implementar en esta fase:

1. Credenciales de administrador por defecto para demo local.
2. Visualizacion de tallas como numeros.
3. Visualizacion de colores en espanol.
4. Badge visual para carrito.
5. Calculo automatico de edad desde fecha de nacimiento.

## Mejoras que quedan documentadas para despues

1. Validaciones avanzadas de formularios.
2. Busqueda mas intuitiva.
3. Paginacion o carga progresiva.
4. Imagenes finales de productos.
5. Ampliacion de pruebas automatizadas.
6. Evaluacion de framework visual como Material Design o Bootstrap.

## Nota para exposicion

Esta fase representa la supervision humana posterior al desarrollo asistido por IA. El objetivo no fue rehacer el sistema, sino priorizar ajustes con mayor impacto en demo, experiencia de usuario y validacion del MVP.

La decision se baso en implementar mejoras de alto valor y bajo riesgo, dejando mejoras mas costosas o cosmeticas como parte de una ruta evolutiva.
