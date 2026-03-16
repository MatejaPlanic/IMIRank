using Back.DTO.Review;
using Back.Repositories.Game;
using Back.Repositories.Review;

namespace Back.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IGameRepository _gameRepo;

        public ReviewService(IReviewRepository reviewRepo, IGameRepository gameRepo)
        {
            _reviewRepo = reviewRepo;
            _gameRepo = gameRepo;
        }

        public async Task CreateReviewAsync(CreateReviewRequest req, string userId, string userName)
        {
            var review = new Models.Review.Review
            {
                GameId = req.GameId,
                UserId = userId,
                UserName = userName,
                Title = req.Title,
                Content = req.Content,
                Rating = req.Rating,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepo.CreateAsync(review);

            var allReviews = await _reviewRepo.GetAllByGameIdAsync(req.GameId);
            var newAverage = allReviews.Average(r => r.Rating);
            await _gameRepo.UpdateRatingAsync(req.GameId, Math.Round(newAverage, 1), allReviews.Count);
        }
    }
}
