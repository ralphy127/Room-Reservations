using RoomRes.Domain.Models;

namespace RoomRes.Domain.Interfaces {
    public interface IUserRepository : IBaseRepository<User> {
        Task<bool> IsUsernameTaken(string username);
    }
}