using Back.Config;
using Back.Models.Review;
using MongoDB.Driver;

namespace Back.Repositories.Review
{
    public class ReviewCommentRepository : IReviewCommentRepository
    {
        private readonly IMongoCollection<ReviewComment> _collection;

        public ReviewCommentRepository(MongoDBContext context)
        {
            _collection = context.GetCollection<ReviewComment>("review_comments");
        }

        public async Task<ReviewComment> CreateAsync(ReviewComment comment)
        {
            await _collection.InsertOneAsync(comment);
            return comment;
        }

        public async Task<ReviewComment?> GetByIdAsync(string id)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(id);
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<ReviewComment>> GetByReviewIdAsync(string reviewId, int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;
            return await _collection
                .Find(x => x.ReviewId == reviewId)
                .SortByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCommentCountByReviewIdAsync(string reviewId)
        {
            return (int)await _collection.CountDocumentsAsync(x => x.ReviewId == reviewId);
        }

        public async Task<bool> UpdateAsync(string id, ReviewComment comment)
        {
            var result = await _collection.ReplaceOneAsync(x => x.Id == id, comment);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
