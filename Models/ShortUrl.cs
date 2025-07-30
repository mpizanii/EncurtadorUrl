using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EncurtadorUrl.Models
{
    public class ShortUrl
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("codigo")]
        public string Codigo { get; set; }

        [BsonElement("urlOriginal")]
        public string UrlOriginal { get; set; }

        [BsonElement("dataCriacao")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}