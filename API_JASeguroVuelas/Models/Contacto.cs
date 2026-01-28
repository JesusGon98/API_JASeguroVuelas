using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_JASeguroVuelas.Models
{
    public class Contacto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("correo")]
        public string Correo { get; set; } = string.Empty;

        [BsonElement("telefono")]
        public string Telefono { get; set; } = string.Empty;

        [BsonElement("origen")]
        public string Origen { get; set; } = string.Empty;

        [BsonElement("destino")]
        public string Destino { get; set; } = string.Empty;

        [BsonElement("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }
    }

    public class ContactoRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
    }
}
