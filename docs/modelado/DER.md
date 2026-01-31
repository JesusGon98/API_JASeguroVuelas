# Diagrama Entidad–Relación (DER) — JA Seguro que Vuelas

Dominio: agencia de viajes. Entidades: Usuario, Contacto (solicitud), Reserva, Vuelo, ReservaVuelo (tabla puente N–N).

```mermaid
erDiagram
    Usuario ||--o{ Contacto : "realiza"
    Usuario ||--o{ Reserva : "tiene"
    Reserva ||--o{ ReservaVuelo : "contiene"
    Vuelo ||--o{ ReservaVuelo : "incluido_en"

    Usuario {
        string Id PK
        string Email UK "NOT NULL"
        string Nombre "NOT NULL"
        string Telefono
        datetime CreatedAt "NOT NULL"
        datetime UpdatedAt "NOT NULL"
    }

    Contacto {
        string Id PK
        string UsuarioId FK "opcional"
        string Nombre "NOT NULL"
        string Correo "NOT NULL"
        string Telefono "NOT NULL"
        string Origen "NOT NULL"
        string Destino "NOT NULL"
        datetime CreatedAt "NOT NULL"
        datetime UpdatedAt "NOT NULL"
    }

    Reserva {
        string Id PK
        string UsuarioId FK "NOT NULL"
        string Estado "NOT NULL"
        datetime FechaReserva "NOT NULL"
        datetime CreatedAt "NOT NULL"
        datetime UpdatedAt "NOT NULL"
    }

    Vuelo {
        string Id PK
        string Origen "NOT NULL"
        string Destino "NOT NULL"
        datetime FechaHora "NOT NULL"
        int AsientosDisponibles "NOT NULL"
        decimal Precio "NOT NULL"
        datetime CreatedAt "NOT NULL"
        datetime UpdatedAt "NOT NULL"
    }

    ReservaVuelo {
        string Id PK
        string ReservaId FK "NOT NULL"
        string VueloId FK "NOT NULL"
        int CantidadAsientos "NOT NULL"
        decimal PrecioUnitario "NOT NULL"
        datetime CreatedAt "NOT NULL"
        datetime UpdatedAt "NOT NULL"
    }
```

## Leyenda

- **PK**: clave primaria  
- **FK**: clave foránea  
- **UK**: único (índice unique)  
- **NOT NULL**: obligatorio  
- **1–N**: Un usuario tiene muchos contactos y muchas reservas; una reserva tiene muchos ítems ReservaVuelo; un vuelo puede aparecer en muchos ítems ReservaVuelo.  
- **N–N**: Reserva ↔ Vuelo mediante la tabla puente **ReservaVuelo**.

## Restricciones e índices sugeridos

| Entidad     | Restricción / Índice |
|------------|-----------------------|
| Usuario    | `Email` UNIQUE, NOT NULL |
| Contacto   | `UsuarioId` opcional (contacto anónimo permitido) |
| Reserva    | `Estado` en dominio fijo (ej. Pendiente, Confirmada, Cancelada) |
| ReservaVuelo | Índice compuesto (ReservaId, VueloId) para evitar duplicados |
