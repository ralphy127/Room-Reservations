using RoomRes.Domain.Models;
using System.Collections.Generic;

namespace RoomRes.Presentation.ViewModels {
    public class UserListViewModel {
        public required IEnumerable<User>? Users { get; set; }
    }

    public class UserAddViewModel {
        public required User User { get; set; }
    }
}