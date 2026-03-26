using Back.Config;
using MongoDB.Driver;

namespace Back.Repositories.GameSuggestion
{
    public class GameSuggestionRepository : IGameSuggestionRepository
    {
        private readonly IMongoCollection<Models.GameSuggestion.GameSuggestion> _col;

        public GameSuggestionRepository(MongoDBContext context)
        {
            _col = context.GetCollection<Models.GameSuggestion.GameSuggestion>("gameSuggestions");
        }

        public async Task CreateAsync(Models.GameSuggestion.GameSuggestion suggestion) =>
            await _col.InsertOneAsync(suggestion);

        public async Task<List<Models.GameSuggestion.GameSuggestion>> GetAllAsync(int page, int pageSize) =>
            await _col.Find(_ => true)
                .SortByDescending(s => s.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

        public async Task<int> CountAsync() =>
            (int)await _col.CountDocumentsAsync(_ => true);

        public async Task MarkAsReviewedAsync(string id)
        {
            var update = Builders<Models.GameSuggestion.GameSuggestion>.Update.Set(s => s.IsReviewed, true);
            await _col.UpdateOneAsync(s => s.Id == id, update);
        }
    }
}