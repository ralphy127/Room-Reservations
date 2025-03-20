using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RoomRes.Domain.Models {
    [BsonIgnoreExtraElements]
    [BsonCollection("room_reservations")]
    public class RoomReservation : BaseModel {
        [BsonElement("room_id")]
        [BsonRepresentation(BsonType.String)]
        public string? RoomId { get; set; }

        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.String)]
        public string? UserId { get; set; }

        [BsonElement("validation_time")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ValidationTime { get; set; }

        [BsonElement("starting_time")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime StartingTime { get; set; }

        [BsonElement("duration")]
        [BsonRepresentation(BsonType.String)]
        public TimeSpan Duration { get; set; }

        public RoomReservation() {}

        public RoomReservation(string id, string roomId, string userId, DateTime validationTime, DateTime startingTime, TimeSpan duration) {
            Id = id;
            RoomId = roomId;
            UserId = userId;
            ValidationTime = validationTime;
            StartingTime = startingTime;
            Duration = duration;
        }
    }
}