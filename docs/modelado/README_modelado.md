# Modelado de datos y dominio — JA Seguro que Vuelas

Este documento describe el modelo de datos y el modelo de dominio del proyecto **JA Seguro que Vuelas**, una API para una agencia de viajes digital.

---

## 1. Dominio: qué resuelve el sistema

**JA Seguro que Vuelas** es una agencia de viajes que ofrece vuelos. El sistema permite:

- **Usuarios (clientes)** registrarse y gestionar su perfil.
- **Solicitudes de contacto** (entidad `Contacto`) para consultas sobre origen, destino, etc., desde la web.
- **Reservas** como entidad principal de negocio: un usuario puede crear reservas que agrupan uno o varios vuelos.
- **Vuelos** como catálogo ofertado (origen, destino, fecha, precio, asientos).
- **Detalle reserva–vuelo** (tabla puente `ReservaVuelo`) para la relación N–N: una reserva puede incluir varios vuelos y un vuelo puede estar en varias reservas, con cantidad de asientos y precio por ítem.

El modelo cubre: identidad del usuario, captación de contactos/leads, y el núcleo de negocio (reservas y vuelos) con integridad y trazabilidad (timestamps).

---

## 2. Decisiones clave

### Por qué estas entidades

- **Usuario**: equivalente a Account/Client; necesario para identificar quién hace contactos y reservas y para futura autenticación.
- **Contacto**: ya existente en el proyecto; representa una solicitud de información (origen, destino, correo, teléfono). Se mantiene y se extiende con `UsuarioId` opcional y timestamps.
- **Reserva**: entidad principal de negocio; centraliza el estado del viaje (Pendiente, Confirmada, Cancelada) y la fecha de reserva.
- **Vuelo**: entidad de catálogo; evita duplicar datos de vuelos en cada reserva y permite reutilizarlos.
- **ReservaVuelo**: entidad de detalle/relación que materializa la N–N entre Reserva y Vuelo, con cantidad de asientos y precio unitario por ítem.

### Relaciones

- **1–N Usuario → Contacto**: un usuario puede tener muchas solicitudes de contacto (y el contacto puede ser anónimo si `UsuarioId` es null).
- **1–N Usuario → Reserva**: un usuario tiene muchas reservas.
- **N–N Reserva ↔ Vuelo** mediante **ReservaVuelo**: una reserva tiene varios ítems de vuelo; un vuelo puede estar en varias reservas. La tabla puente permite almacenar cantidad y precio por ítem.

### Reglas de integridad

- **Usuario**: `Email` único y obligatorio (evita cuentas duplicadas).
- **Contacto / Reserva / Vuelo / ReservaVuelo**: campos clave `NOT NULL` (nombre, correo, origen, destino, estado, fechas, etc.).
- **Timestamps**: `CreatedAt` y `UpdatedAt` en todas las entidades para auditoría y ordenación.

### Normalización

- Datos de vuelo (origen, destino, fecha, precio) viven en `Vuelo`; en `ReservaVuelo` solo se guardan referencia al vuelo, cantidad y precio unitario (permite historizar precios por reserva).
- No se duplican datos de usuario en cada contacto; se usa `UsuarioId` como referencia.

---

## 3. Supuestos (assumptions)

- **Base de datos**: El proyecto usa **MongoDB** (NoSQL). Las “claves foráneas” se modelan como referencias por `ObjectId`/string; la integridad referencial y los índices se gestionan a nivel de aplicación o con índices en MongoDB.
- **Contacto sin usuario**: Se permite `UsuarioId` opcional en `Contacto` para soportar solicitudes anónimas desde la web (solo nombre, correo, teléfono, origen, destino).
- **Estado de reserva**: Se asume un conjunto fijo de estados (ej. Pendiente, Confirmada, Cancelada) representado como enum o valor controlado en código.
- **Un vuelo, varias reservas**: Un mismo vuelo puede formar parte de varias reservas; la disponibilidad (asientos) se podría validar en lógica de negocio contra `AsientosDisponibles` y la suma de cantidades en `ReservaVuelo` para ese vuelo.
- **Exportación de diagramas**: Los diagramas están en **Mermaid** en `DER.md` y `UML_Clases.md`. Para generar **DER.png**, **UML_Clases.png** o PDF, se puede usar [Mermaid Live Editor](https://mermaid.live) u otra herramienta que exporte desde Mermaid.

---

## 4. Entregables en este directorio

| Archivo            | Descripción                                      |
|--------------------|--------------------------------------------------|
| `DER.md`           | Diagrama Entidad–Relación en Mermaid             |
| `UML_Clases.md`    | Diagrama de Clases (UML) en Mermaid              |
| `README_modelado.md` | Este documento (dominio, decisiones, supuestos) |

Para cumplir con entregables en PNG/PDF: copiar el contenido Mermaid de `DER.md` y `UML_Clases.md` en [mermaid.live](https://mermaid.live) y exportar como **DER.png** / **UML_Clases.png** (o PDF) según requiera la entrega.

---

## 5. Implementación en código

Los modelos correspondientes están implementados en la capa **Models** del proyecto:

- `Usuario.cs`
- `Contacto.cs` (existente; se añaden `UsuarioId` opcional y `UpdatedAt`)
- `Reserva.cs`
- `Vuelo.cs`
- `ReservaVuelo.cs`

Se utilizan anotaciones de **MongoDB** (`BsonId`, `BsonElement`, etc.) y convenciones **PascalCase** en C#. Las relaciones se representan mediante propiedades de referencia (IDs); en MongoDB no hay FK a nivel de base de datos, pero el dominio y las restricciones se documentan y aplican en servicio/API.
