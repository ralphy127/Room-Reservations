using RoomRes.Domain.Models;
using System.Collections.Generic;

namespace RoomRes.Presentation.ViewModels {
    public class RoomListViewModel {
        public required IEnumerable<Room> Rooms { get; set; }
    }

    public class RoomAddViewModel {
        public required Room Room { get; set; }
    }
}