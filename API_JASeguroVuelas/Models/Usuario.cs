using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_JASeguroVuelas.Models
{
    /// <summary>
    /// Entidad Usuario (cliente de la agencia). Equivalente a Account/Client.
    /// </summary>
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("telefono")]
        public string? Telefono { get; set; }

        [BsonElement("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [BsonElement("fechaActualizacion")]
        public DateTime FechaActualizacion { get; set; }
    }
}
