using System.Text;
using RoomRes.Domain.Models;
using RoomRes.Domain.Interfaces;


namespace RoomRes.Application.Services {
    public class UserService : BaseService<User>, IUserService {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) : base(userRepository) {
            if(userRepository is null) {
                throw new ArgumentNullException(nameof(userRepository));
            }

            _userRepository = userRepository;
        }

        public async Task CreateAsync(string username, string password) {
            string nextId = await _userRepository.GetNextIdAsync();
            User user = new User(nextId, username, password);
            await _userRepository.AddAsync(user);
        }

        public async Task<User?> AuthenticateAsync(string username, string password) {
            User selectedUser = await _userRepository.GetByFieldAsync("username", username);
            if(selectedUser is null) {
                return null;
            }

            if(selectedUser.Password == password) {
                return selectedUser;
            } else {
                return null;
            }
        }

        public async Task<bool> IsUsernameTaken(string username) {
            return await _userRepository.IsUsernameTaken(username);
        }
    }
}
