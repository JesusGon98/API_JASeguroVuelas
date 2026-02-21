using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_JASeguroVuelas.Models
{
    /// <summary>
    /// Ciudades o destinos con aeropuerto (modelo alineado con el frontend Ja_SeguroVuelas).
    /// </summary>
    public class Destino
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [BsonElement("mejorEpoca")]
        public string? MejorEpoca { get; set; }

        [BsonElement("precioMin")]
        public decimal PrecioMin { get; set; }

        [BsonElement("precioMax")]
        public decimal PrecioMax { get; set; }

        [BsonElement("etiqueta")]
        public string? Etiqueta { get; set; }

        [BsonElement("foto")]
        public string? Foto { get; set; }

        [BsonElement("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }
    }

    public class DestinoRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string? MejorEpoca { get; set; }
        public decimal PrecioMin { get; set; }
        public decimal PrecioMax { get; set; }
        public string? Etiqueta { get; set; }
        public string? Foto { get; set; }
    }
}
