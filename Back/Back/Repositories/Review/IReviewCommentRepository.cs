using Back.Models.Review;

namespace Back.Repositories.Review
{
    public interface IReviewCommentRepository
    {
        Task<ReviewComment> CreateAsync(ReviewComment comment);
        Task<ReviewComment?> GetByIdAsync(string id);
        Task<List<ReviewComment>> GetByReviewIdAsync(string reviewId, int page = 1, int pageSize = 10);
        Task<int> GetCommentCountByReviewIdAsync(string reviewId);
        Task<bool> UpdateAsync(string id, ReviewComment comment);
        Task<bool> DeleteAsync(string id);
    }
}
