using Back.Config;
using MongoDB.Driver;

namespace Back.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<Back.Models.User.User> _users;
        public UserRepository(MongoDBContext context)
        {
            _users = context.GetCollection<Back.Models.User.User>("User");
        }

        public async Task<Back.Models.User.User?> FindByEmailAsync(string email) => await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task CreateAsync(Back.Models.User.User user) => await _users.InsertOneAsync(user);
    }
}