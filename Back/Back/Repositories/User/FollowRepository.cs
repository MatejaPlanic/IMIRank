using Back.Config;
using Back.Models.User;
using MongoDB.Driver;

namespace Back.Repositories.User
{
    public class FollowRepository : IFollowRepository
    {
        private readonly IMongoCollection<Follow> _follows;
        private readonly IMongoCollection<Models.User.User> _users;

        public FollowRepository(MongoDBContext context)
        {
            _follows = context.GetCollection<Follow>("Follow");
            _users = context.GetCollection<Models.User.User>("User");
        }

        public async Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            var result = await _follows.Find(f => f.FollowerId == followerId && f.FollowingId == followingId).FirstOrDefaultAsync();
            return result != null;
        }

        public async Task FollowAsync(string followerId, string followingId)
        {
            if (await IsFollowingAsync(followerId, followingId))
                return;

            var follow = new Follow { FollowerId = followerId, FollowingId = followingId };
            await _follows.InsertOneAsync(follow);
        }

        public async Task UnfollowAsync(string followerId, string followingId)
        {
            await _follows.DeleteOneAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
        }

        public async Task<List<string>> GetFollowersAsync(string userId)
        {
            var follows = await _follows.Find(f => f.FollowingId == userId).ToListAsync();
            return follows.Select(f => f.FollowerId).ToList();
        }

        public async Task<List<string>> GetFollowingAsync(string userId)
        {
            var follows = await _follows.Find(f => f.FollowerId == userId).ToListAsync();
            return follows.Select(f => f.FollowingId).ToList();
        }

        public async Task<int> GetFollowersCountAsync(string userId)
        {
            return (int)await _follows.CountDocumentsAsync(f => f.FollowingId == userId);
        }

        public async Task<int> GetFollowingCountAsync(string userId)
        {
            return (int)await _follows.CountDocumentsAsync(f => f.FollowerId == userId);
        }
    }
}
