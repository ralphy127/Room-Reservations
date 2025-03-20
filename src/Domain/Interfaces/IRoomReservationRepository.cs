using System.Collections.Generic;
using RoomRes.Domain.Models;

namespace RoomRes.Domain.Interfaces {
    public interface IRoomReservationRepository : IBaseRepository<RoomReservation> {
        Task<RoomReservation> GetByRoomIdAsync(string roomId);
        Task<List<RoomReservation>> GetByUserIdAsync(string userId);
    }
}