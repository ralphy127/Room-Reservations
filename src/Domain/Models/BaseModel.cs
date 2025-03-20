using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RoomRes.Domain.Models {
    public abstract class BaseModel {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string? Id { get; set; }
    }
}