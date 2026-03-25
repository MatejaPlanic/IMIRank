using Back.DTO.Review;
using Back.Models.Review;
using Back.Repositories.Review;
using Back.Repositories.User;

namespace Back.Services.Review
{
    public class ReviewCommentService : IReviewCommentService
    {
        private readonly IReviewCommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public ReviewCommentService(IReviewCommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<ReviewCommentResponse> CreateCommentAsync(string reviewId, string userId, string userName, CreateReviewCommentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length < 1)
                throw new ArgumentException("Komentar ne može biti prazan");

            var comment = new ReviewComment
            {
                ReviewId = reviewId,
                UserId = userId,
                UserName = userName,
                Content = request.Content.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdComment = await _commentRepository.CreateAsync(comment);
            return await MapToResponseAsync(createdComment);
        }

        public async Task<ReviewCommentResponse?> GetCommentByIdAsync(string id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return comment != null ? await MapToResponseAsync(comment) : null;
        }

        public async Task<ReviewCommentsListResponse> GetCommentsByReviewIdAsync(string reviewId, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 50) pageSize = 10;

            var comments = await _commentRepository.GetByReviewIdAsync(reviewId, page, pageSize);
            var totalCount = await _commentRepository.GetCommentCountByReviewIdAsync(reviewId);

            var mappedComments = new List<ReviewCommentResponse>();
            foreach (var comment in comments)
            {
                mappedComments.Add(await MapToResponseAsync(comment));
            }

            return new ReviewCommentsListResponse
            {
                Comments = mappedComments,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> UpdateCommentAsync(string id, string userId, UpdateReviewCommentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length < 1)
                throw new ArgumentException("Komentar ne može biti prazan");

            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                throw new KeyNotFoundException("Komentar nije pronađen");

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("Možete editovati samo svoje komentare");

            comment.Content = request.Content.Trim();
            comment.UpdatedAt = DateTime.UtcNow;

            return await _commentRepository.UpdateAsync(id, comment);
        }

        public async Task<bool> DeleteCommentAsync(string id, string userId)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                throw new KeyNotFoundException("Komentar nije pronađen");

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("Možete brisati samo svoje komentare");

            return await _commentRepository.DeleteAsync(id);
        }

        private async Task<ReviewCommentResponse> MapToResponseAsync(ReviewComment comment)
        {
            var user = await _userRepository.GetByIdAsync(comment.UserId);
            var profilePictureUrl = user?.ProfilePicturePath != null ? $"/{user.ProfilePicturePath}" : null;

            return new ReviewCommentResponse
            {
                Id = comment.Id,
                ReviewId = comment.ReviewId,
                UserId = comment.UserId,
                UserName = comment.UserName,
                UserProfilePictureUrl = profilePictureUrl,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            };
        }
    }
}
