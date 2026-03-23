using Back.Config;
using Back.Models.Enums;
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

        public async Task<long> CountAsync() => await _users.CountDocumentsAsync(_ => true);

        public async Task<long> CountByRoleAsync(UserRole role) => await _users.CountDocumentsAsync(u => u.Role == role);

        public async Task<Models.User.User?> GetByIdAsync(string id) => await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task UpdateUsernameAsync(string id, string newUsername)
        {
            var update = Builders<Models.User.User>.Update.Set(u => u.UserName, newUsername);
            await _users.UpdateOneAsync(u => u.Id == id, update);
        }

        public async Task UpdatePasswordAsync(string id, string newHashedPassword)
        {
            var update = Builders<Models.User.User>.Update.Set(u => u.Password, newHashedPassword);
            await _users.UpdateOneAsync(u => u.Id == id, update);
        }

        public async Task UpdateProfilePictureAsync(string id, string picturePath)
        {
            var update = Builders<Models.User.User>.Update.Set(u => u.ProfilePicturePath, picturePath);
            await _users.UpdateOneAsync(u => u.Id == id, update);
        }

        public async Task<List<Models.User.User>> SearchByUserNameAsync(string query, int page, int pageSize)
        {
            var filter = string.IsNullOrWhiteSpace(query)
                ? Builders<Models.User.User>.Filter.Empty
                : Builders<Models.User.User>.Filter.Regex(u => u.UserName, new MongoDB.Bson.BsonRegularExpression(query, "i"));

            return await _users.Find(filter)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<long> CountByUserNameAsync(string query)
        {
            var filter = string.IsNullOrWhiteSpace(query)
                ? Builders<Models.User.User>.Filter.Empty
                : Builders<Models.User.User>.Filter.Regex(u => u.UserName, new MongoDB.Bson.BsonRegularExpression(query, "i"));

            return await _users.CountDocumentsAsync(filter);
        }

        public async Task<List<Models.User.User>> GetEditorsByRoleAsync() =>
            await _users.Find(u => u.Role == UserRole.Editor).ToListAsync();
    }
}