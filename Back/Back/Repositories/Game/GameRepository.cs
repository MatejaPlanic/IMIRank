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

        public async Task<List<Models.Game.Game>> SearchAsync(string query, int page, int pageSize)
        {
            var filter = string.IsNullOrEmpty(query)
                ? Builders<Models.Game.Game>.Filter.Empty
                : Builders<Models.Game.Game>.Filter.Regex(g => g.Title, new MongoDB.Bson.BsonRegularExpression(query, "i"));

            return await _games.Find(filter)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync(string query)
        {
            var filter = string.IsNullOrEmpty(query)
                ? Builders<Models.Game.Game>.Filter.Empty
                : Builders<Models.Game.Game>.Filter.Regex(g => g.Title, new MongoDB.Bson.BsonRegularExpression(query, "i"));

            return (int)await _games.CountDocumentsAsync(filter);
        }

        public async Task<List<string>> GetGenresAsync()
        {
            var genres = await _games.DistinctAsync<string>("Genre", Builders<Models.Game.Game>.Filter.Empty);
            return await genres.ToListAsync();
        }

        public async Task<List<string>> GetGameIdsByGenreAsync(string genre)
        {
            var filter = Builders<Models.Game.Game>.Filter.Eq(g => g.Genre, genre);
            return await _games.Find(filter)
                .Project(g => g.Id)
                .ToListAsync();
        }

        public async Task CreateAsync(Models.Game.Game game)
        {
            await _games.InsertOneAsync(game);
        }
    }
}