using Back.Config;
using MongoDB.Driver;

namespace Back.Repositories.Game
{
    public class GameRepository : IGameRepository
    {
        private readonly IMongoCollection<Models.Game.Game> _games;
        public GameRepository(MongoDBContext context)
        {
            _games = context.GetCollection<Models.Game.Game>("Game");
        }

        public async Task<List<Models.Game.Game>> GetTopRatedAsync(int count) =>
            await _games.Find(_ => true)
                .SortByDescending(g => g.AverageRating)
                .Limit(count)
                .ToListAsync();

        public async Task<List<Models.Game.Game>> GetLatestAsync(int count) =>
            await _games.Find(_ => true)
                .SortByDescending(g => g.ReleaseYear)
                .Limit(count)
                .ToListAsync();

        public async Task<Models.Game.Game?> GetByIdAsync(string id) =>
            await _games.Find(g => g.Id == id).FirstOrDefaultAsync();

        public async Task UpdateRatingAsync(string gameId, double newAverage, int newCount)
        {
            var update = Builders<Models.Game.Game>.Update
                .Set(g => g.AverageRating, newAverage)
                .Set(g => g.ReviewCount, newCount);
            await _games.UpdateOneAsync(g => g.Id == gameId, update);
        }
    }
}