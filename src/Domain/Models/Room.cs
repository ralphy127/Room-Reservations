using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RoomRes.Domain.Models {
    [BsonIgnoreExtraElements]
    [BsonCollection("rooms")]
    public class Room : BaseModel {
        [BsonElement("capacity")]
        [BsonRepresentation(BsonType.Int32)]
        public int Capacity { get; set; }

        public Room() {}

        public Room(string id, int capacity) {
            Id = id;
            Capacity = capacity;
        }
    }
}
