using Back.DTO.Review;
using Back.Repositories.Game;
using Back.Repositories.Review;
using Back.Services.User;
using Back.Services.Notification;
using Back.Repositories.User;
using Microsoft.AspNetCore.SignalR;
using Back.Hubs;

namespace Back.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IGameRepository _gameRepo;
        private readonly IFollowService _followService;
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public ReviewService(IReviewRepository reviewRepo, IGameRepository gameRepo, IFollowService followService, INotificationService notificationService, IUserRepository userRepository, IHubContext<NotificationsHub> hubContext)
        {
            _reviewRepo = reviewRepo;
            _gameRepo = gameRepo;
            _followService = followService;
            _notificationService = notificationService;
            _userRepository = userRepository;
            _hubContext = hubContext;
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

            var followers = await _followService.GetFollowersAsync(userId);


            foreach (var followerId in followers)
            {
                // Osiguraj da imaš najnovije korisnikove podatke sa slikom
                var currentEditor = await _userRepository.GetByIdAsync(userId);
                
                // Formatiraj profilePictureUrl sa / prefix kao UserController
                var profilePictureUrl = currentEditor?.ProfilePicturePath != null 
                    ? $"/{currentEditor.ProfilePicturePath}" 
                    : null;
                
                var notificationResponse = await _notificationService.CreateNotificationAsync(
                    recipientUserId: followerId,
                    actorUserId: userId,
                    actorUserName: userName,
                    actorProfilePictureUrl: profilePictureUrl,
                    reviewId: review.Id,
                    reviewCommentId: null,
                    message: $"{userName} je objavio novu recenziju: \"{req.Title}\""
                );


                await _hubContext.Clients
                    .Group($"notifications-{followerId}")
                    .SendAsync("NotificationReceived", notificationResponse);
            }
        }
    }
}
