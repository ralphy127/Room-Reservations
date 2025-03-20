using System.Collections.Generic;
using MongoDB.Driver;
using RoomRes.Domain.Models;
using RoomRes.Domain.Interfaces;

namespace RoomRes.Infrastructure.Repositories {
    public class RoomReservationRepository : BaseRepository<RoomReservation>, IRoomReservationRepository {
        private readonly IMongoCollection<RoomReservation> _roomReservations;
    
        public RoomReservationRepository(IMongoDatabase database) : base(database, "room_reservations") {
            _roomReservations = database.GetCollection<RoomReservation>("room_reservations");
        }

        public async Task<RoomReservation> GetByRoomIdAsync(string roomId) {
            FilterDefinition<RoomReservation> filter = Builders<RoomReservation>.Filter.Eq("roomId", roomId);
            return await _roomReservations.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<RoomReservation>> GetByUserIdAsync(string userId) {
            FilterDefinition<RoomReservation> filter = Builders<RoomReservation>.Filter.Eq(rr => rr.UserId, userId);
            return await _roomReservations.Find(filter).ToListAsync();
        }
    }
}