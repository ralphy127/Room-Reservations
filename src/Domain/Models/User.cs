using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RoomRes.Domain.Models {
    [BsonIgnoreExtraElements]
    [BsonCollection("users")]
    public class User : BaseModel {
        [BsonElement("username")]
        [BsonRepresentation(BsonType.String)]
        public string? Username { get; set; }

        [BsonElement("password")]
        [BsonRepresentation(BsonType.String)]
        public string? Password { get; set; }

        public User() {}

        public User(string id, string username, string password) {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}