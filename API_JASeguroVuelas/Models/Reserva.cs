using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_JASeguroVuelas.Models
{
    /// <summary>
    /// Estados posibles de una reserva (regla de integridad de dominio).
    /// </summary>
    public enum EstadoReserva
    {
        Pendiente = 0,
        Confirmada = 1,
        Cancelada = 2
    }

    /// <summary>
    /// Entidad principal de negocio: reserva de viaje de un usuario.
    /// </summary>
    public class Reserva
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("usuarioId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; } = string.Empty;

        [BsonElement("estado")]
        public EstadoReserva Estado { get; set; }

        [BsonElement("fechaReserva")]
        public DateTime FechaReserva { get; set; }

        [BsonElement("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [BsonElement("fechaActualizacion")]
        public DateTime FechaActualizacion { get; set; }
    }
}
