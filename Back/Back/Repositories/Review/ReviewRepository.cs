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

        public async Task<Models.Review.Review?> GetByIdAsync(string id) => await _reviews.Find(r => r.Id == id).FirstOrDefaultAsync();

        public async Task<int> CountFilteredAsync(string genre, double minRating)
        {
            var filter = BuildFilter(genre, minRating);
            return (int)await _reviews.CountDocumentsAsync(filter);
        }

        public async Task<List<Models.Review.Review>> GetFilteredAsync(string genre, double minRating, string sort, int page, int pageSize)
        {
            var filter = BuildFilter(genre, minRating);
            var query = _reviews.Find(filter);

            query = sort switch
            {
                "rating" => query.SortByDescending(r => r.Rating),
                "oldest" => query.SortBy(r => r.CreatedAt),
                _ => query.SortByDescending(r => r.CreatedAt)
            };

            return await query.Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        private FilterDefinition<Models.Review.Review> BuildFilter(string genre, double minRating)
        {
            var builder = Builders<Models.Review.Review>.Filter;
            var filter = builder.Gte(r => r.Rating, minRating);
            return filter;
        }
    }
}
