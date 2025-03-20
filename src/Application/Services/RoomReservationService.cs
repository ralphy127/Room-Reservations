using System.Collections.Generic;
using RoomRes.Domain.Models;
using RoomRes.Domain.Interfaces;

namespace RoomRes.Application.Services {
    public class RoomReservationService : BaseService<RoomReservation>, IRoomReservationService {
        private readonly IRoomReservationRepository _roomReservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;

        public RoomReservationService(IRoomReservationRepository roomReservationRepository,
                                      IRoomRepository roomRepository,
                                      IUserRepository userRepository) : base(roomReservationRepository) {
            if(roomReservationRepository is null) {
                throw new ArgumentNullException(nameof(roomReservationRepository));
            }
            if(roomRepository is null) {
                throw new ArgumentNullException(nameof(roomRepository));
            }
            if(userRepository is null) {
                throw new ArgumentNullException(nameof(userRepository));
            }
            
            _roomReservationRepository = roomReservationRepository;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> IsRoomFree(string roomId, DateTime startingTime, TimeSpan duration) {
            List<RoomReservation> roomReservations = await _roomReservationRepository.GetAllAsync();

            foreach(RoomReservation reservation in roomReservations) {
                if(reservation.RoomId == roomId && reservation.StartingTime < startingTime.Add(duration) &&
                   reservation.StartingTime.Add(reservation.Duration) > startingTime) {
                        return false;
                }
            }

            return true;
        }   

        public async Task<List<RoomReservation>> GetByUserIdAsync(string userId) {
            return await _roomReservationRepository.GetByUserIdAsync(userId);
        }
    }
}