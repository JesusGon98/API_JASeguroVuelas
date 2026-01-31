# Diagrama de Clases (UML) — JA Seguro que Vuelas

Solo entidades del dominio (no controladores).

```mermaid
classDiagram
    class Usuario {
        +string Id
        +string Email
        +string Nombre
        +string Telefono
        +DateTime CreatedAt
        +DateTime UpdatedAt
    }

    class Contacto {
        +string Id
        +string UsuarioId
        +string Nombre
        +string Correo
        +string Telefono
        +string Origen
        +string Destino
        +DateTime CreatedAt
        +DateTime UpdatedAt
    }

    class Reserva {
        +string Id
        +string UsuarioId
        +EstadoReserva Estado
        +DateTime FechaReserva
        +DateTime CreatedAt
        +DateTime UpdatedAt
    }

    class Vuelo {
        +string Id
        +string Origen
        +string Destino
        +DateTime FechaHora
        +int AsientosDisponibles
        +decimal Precio
        +DateTime CreatedAt
        +DateTime UpdatedAt
    }

    class ReservaVuelo {
        +string Id
        +string ReservaId
        +string VueloId
        +int CantidadAsientos
        +decimal PrecioUnitario
        +DateTime CreatedAt
        +DateTime UpdatedAt
    }

    class EstadoReserva {
        <<enumeration>>
        Pendiente
        Confirmada
        Cancelada
    }

    Usuario "1" --> "*" Contacto : realiza
    Usuario "1" --> "*" Reserva : tiene
    Reserva "1" --> "*" ReservaVuelo : contiene
    Vuelo "1" --> "*" ReservaVuelo : incluido en
    Reserva --> EstadoReserva : usa
```

## Multiplicidad

- **Usuario → Contacto**: 1 – N (un usuario puede tener muchas solicitudes de contacto).  
- **Usuario → Reserva**: 1 – N (un usuario puede tener muchas reservas).  
- **Reserva → ReservaVuelo**: 1 – N (una reserva tiene varios ítems de vuelo).  
- **Vuelo → ReservaVuelo**: 1 – N (un vuelo puede estar en muchas reservas).  
- **Reserva ↔ Vuelo**: N – N a través de la clase **ReservaVuelo**.

## Value objects / Enums

- **EstadoReserva**: Pendiente, Confirmada, Cancelada (regla de integridad de dominio).
