using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_JASeguroVuelas.Models
{
    /// <summary>
    /// Entidad Vuelo: catálogo de vuelos ofertados por la agencia.
    /// </summary>
    public class Vuelo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("origen")]
        public string Origen { get; set; } = string.Empty;

        [BsonElement("destino")]
        public string Destino { get; set; } = string.Empty;

        [BsonElement("fechaHora")]
        public DateTime FechaHora { get; set; }

        [BsonElement("asientosDisponibles")]
        public int AsientosDisponibles { get; set; }

        [BsonElement("precio")]
        public decimal Precio { get; set; }

        [BsonElement("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [BsonElement("fechaActualizacion")]
        public DateTime FechaActualizacion { get; set; }
    }
}
