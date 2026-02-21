using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_JASeguroVuelas.Models
{
    /// <summary>
    /// Reservaciones de vuelo (modelo alineado con el frontend Ja_SeguroVuelas).
    /// </summary>
    public class Reservacion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("codigo")]
        public string Codigo { get; set; } = string.Empty;

        [BsonElement("origen")]
        public string Origen { get; set; } = string.Empty;

        [BsonElement("destino")]
        public string Destino { get; set; } = string.Empty;

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; }

        [BsonElement("pasajeros")]
        public int Pasajeros { get; set; }

        [BsonElement("estado")]
        public string Estado { get; set; } = string.Empty;

        [BsonElement("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [BsonElement("vueloId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? VueloId { get; set; }

        [BsonElement("usuarioId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UsuarioId { get; set; }
    }

    public class ReservacionRequest
    {
        public string Codigo { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int Pasajeros { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? VueloId { get; set; }
        public string? UsuarioId { get; set; }
    }
}
