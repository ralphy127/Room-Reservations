using RoomRes.Domain.Models;
using System.Collections.Generic;

namespace RoomRes.Presentation.ViewModels {
    public class RoomReservationListViewModel {
        public required IEnumerable<RoomReservation> RoomReservations { get; set; }
    }

    public class RoomReservationAddViewModel {
        public required RoomReservation RoomReservation { get; set; }
    }
}