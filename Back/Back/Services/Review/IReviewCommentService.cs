using Back.DTO.Review;
using Back.Models.Review;

namespace Back.Services.Review
{
    public interface IReviewCommentService
    {
        Task<ReviewCommentResponse> CreateCommentAsync(string reviewId, string userId, string userName, CreateReviewCommentRequest request);
        Task<ReviewCommentResponse?> GetCommentByIdAsync(string id);
        Task<ReviewCommentsListResponse> GetCommentsByReviewIdAsync(string reviewId, int page = 1, int pageSize = 10);
        Task<bool> UpdateCommentAsync(string id, string userId, UpdateReviewCommentRequest request);
        Task<bool> DeleteCommentAsync(string id, string userId);
    }
}
