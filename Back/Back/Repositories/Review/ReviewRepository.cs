using Back.Config;
using MongoDB.Driver;

namespace Back.Repositories.Review
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly IMongoCollection<Models.Review.Review> _reviews;

        public ReviewRepository(MongoDBContext context)
        {
            _reviews = context.GetCollection<Models.Review.Review>("Review");
        }

        public async Task<List<Models.Review.Review>> GetRecentAsync(int count) =>
            await _reviews.Find(_ => true)
                .SortByDescending(r => r.CreatedAt)
                .Limit(count)
                .ToListAsync();

        public async Task<List<Models.Review.Review>> GetByGameIdAsync(string gameId) =>
            await _reviews.Find(r => r.GameId == gameId)
                .SortByDescending(r => r.CreatedAt)
                .ToListAsync();

        public async Task<List<Models.Review.Review>> GetAllByGameIdAsync(string gameId) =>
            await _reviews.Find(r => r.GameId == gameId).ToListAsync();

        public async Task CreateAsync(Models.Review.Review review) =>
            await _reviews.InsertOneAsync(review);

        public async Task<long> CountAsync() =>
            await _reviews.CountDocumentsAsync(_ => true);
    }
}
