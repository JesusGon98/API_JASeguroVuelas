using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_JASeguroVuelas.Models
{
    /// <summary>
    /// Tabla/entidad puente N–N entre Reserva y Vuelo.
    /// Una reserva puede incluir varios vuelos; un vuelo puede estar en varias reservas.
    /// </summary>
    public class ReservaVuelo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("reservaId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReservaId { get; set; } = string.Empty;

        [BsonElement("vueloId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VueloId { get; set; } = string.Empty;

        [BsonElement("cantidadAsientos")]
        public int CantidadAsientos { get; set; }

        [BsonElement("precioUnitario")]
        public decimal PrecioUnitario { get; set; }

        [BsonElement("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [BsonElement("fechaActualizacion")]
        public DateTime FechaActualizacion { get; set; }
    }
}
