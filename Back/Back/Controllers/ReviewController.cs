using Back.DTO.Review;
using Back.Repositories.Game;
using Back.Repositories.Review;
using Back.Repositories.User;
using Back.Services.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IReviewRepository _reviewRepo;
        private readonly IGameRepository _gameRepo;
        private readonly IUserRepository _userRepo;

        public ReviewController(IReviewService reviewService, IReviewRepository reviewRepo, IGameRepository gameRepo, IUserRepository userRepo)
        {
            _reviewService = reviewService;
            _reviewRepo = reviewRepo;
            _gameRepo = gameRepo;
            _userRepo = userRepo;
        }

        [HttpGet("byGame/{gameId}")]
        public async Task<IActionResult> GetByGame(string gameId)
        {
            var reviews = await _reviewRepo.GetByGameIdAsync(gameId);
            var result = new ReviewListResponse { Total = reviews.Count };

            foreach (var r in reviews)
            {
                var game = await _gameRepo.GetByIdAsync(r.GameId);
                var user = await _userRepo.GetByIdAsync(r.UserId);
                result.Reviews.Add(new ReviewItemDto
                {
                    Id = r.Id,
                    GameId = r.GameId,
                    GameTitle = game?.Title ?? "Nepoznata igra",
                    GameCoverUrl = game?.CoverImageUrl ?? "",
                    GameGenre = game?.Genre ?? "",
                    UserId = r.UserId,
                    UserName = r.UserName,
                    UserProfilePictureUrl = user?.ProfilePicturePath,
                    Title = r.Title,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt
                });
            }

            return Ok(result);
        }

        [HttpGet("byUser/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var reviews = await _reviewRepo.GetByUserIdAsync(userId);
            var result = new ReviewListResponse { Total = reviews.Count };

            foreach (var r in reviews)
            {
                var game = await _gameRepo.GetByIdAsync(r.GameId);
                var user = await _userRepo.GetByIdAsync(r.UserId);
                result.Reviews.Add(new ReviewItemDto
                {
                    Id = r.Id,
                    GameId = r.GameId,
                    GameTitle = game?.Title ?? "Nepoznata igra",
                    GameCoverUrl = game?.CoverImageUrl ?? "",
                    GameGenre = game?.Genre ?? "",
                    UserId = r.UserId,
                    UserName = r.UserName,
                    UserProfilePictureUrl = user?.ProfilePicturePath,
                    Title = r.Title,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt
                });
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string genre = "",
            [FromQuery] double minRating = 0,
            [FromQuery] string sort = "newest",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6)
        {
            IEnumerable<string>? gameIds = null;

            if (!string.IsNullOrWhiteSpace(genre))
            {
                gameIds = await _gameRepo.GetGameIdsByGenreAsync(genre);
            }

            var reviews = await _reviewRepo.GetFilteredAsync(gameIds, minRating, sort, page, pageSize);
            var total = await _reviewRepo.CountFilteredAsync(gameIds, minRating);

            var result = new ReviewListResponse { Total = total };

            foreach (var r in reviews)
            {
                var game = await _gameRepo.GetByIdAsync(r.GameId);
                var user = await _userRepo.GetByIdAsync(r.UserId);
                result.Reviews.Add(new ReviewItemDto
                {
                    Id = r.Id,
                    GameId = r.GameId,
                    GameTitle = game?.Title ?? "Nepoznata igra",
                    GameCoverUrl = game?.CoverImageUrl ?? "",
                    GameGenre = game?.Genre ?? "",
                    UserId = r.UserId,
                    UserName = r.UserName,
                    UserProfilePictureUrl = user?.ProfilePicturePath,
                    Title = r.Title,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt
                });
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var r = await _reviewRepo.GetByIdAsync(id);
            if (r == null) return NotFound();

            var game = await _gameRepo.GetByIdAsync(r.GameId);
            var user = await _userRepo.GetByIdAsync(r.UserId);

            return Ok(new ReviewItemDto
            {
                Id = r.Id,
                GameId = r.GameId,
                GameTitle = game?.Title ?? "Nepoznata igra",
                GameCoverUrl = game?.CoverImageUrl ?? "",
                GameGenre = game?.Genre ?? "",
                UserId = r.UserId,
                UserName = r.UserName,
                UserProfilePictureUrl = user?.ProfilePicturePath,
                Title = r.Title,
                Content = r.Content,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var userName = User.FindFirstValue(ClaimTypes.Email)!;
            await _reviewService.CreateReviewAsync(req, userId, userName);
            return Ok();
        }
    }
}