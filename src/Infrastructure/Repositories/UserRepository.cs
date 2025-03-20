using RoomRes.Domain.Models;
using RoomRes.Domain.Interfaces;
using MongoDB.Driver;

namespace RoomRes.Infrastructure.Repositories {
    public class UserRepository : BaseRepository<User>, IUserRepository {
        private readonly IMongoCollection<User> _users;
    
        public UserRepository(IMongoDatabase database) : base(database, "users") {
            _users = database.GetCollection<User>("users");
        }

        public async Task<bool> IsUsernameTaken(string username) {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq("username", username);
            User user = await _users.Find(filter).FirstOrDefaultAsync();

            return user is not null;
        }
    }
}