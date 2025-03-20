using RoomRes.Domain.Models;

namespace RoomRes.Domain.Interfaces {
    public interface IUserService : IBaseService<User> {
        Task CreateAsync(string username, string password);
        Task<User?> AuthenticateAsync(string username, string password);
        Task<bool> IsUsernameTaken(string username);
    }
}