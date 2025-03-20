using MongoDB.Driver;
using RoomRes.Domain.Models;
using RoomRes.Domain.Interfaces;

namespace RoomRes.Infrastructure.Repositories {
    public class RoomRepository : BaseRepository<Room>, IRoomRepository {
        private readonly IMongoCollection<Room> _rooms;
    
        public RoomRepository(IMongoDatabase database) : base(database, "rooms") {
            _rooms = database.GetCollection<Room>("rooms");
        }
    }
}