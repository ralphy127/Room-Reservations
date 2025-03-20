using RoomRes.Domain.Models;
using System.Collections.Generic;

namespace RoomRes.Domain.Interfaces {
    public interface IRoomReservationService : IBaseService<RoomReservation> {
        Task<bool> IsRoomFree(string roomId, DateTime startingTime, TimeSpan duration);
        Task<List<RoomReservation>> GetByUserIdAsync(string userId);
    }
}