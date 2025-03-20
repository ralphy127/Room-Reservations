using RoomRes.Domain.Models;
using RoomRes.Domain.Interfaces;


namespace RoomRes.Application.Services {
    public class RoomService : BaseService<Room>, IRoomService {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository) : base(roomRepository) {
            if(roomRepository is null) {
                throw new ArgumentNullException(nameof(roomRepository));
            }
            _roomRepository = roomRepository;
        }
    }
}