using Back.DTO.Review;
using Back.Repositories.Game;
using Back.Repositories.Review;
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

        public ReviewController(IReviewService reviewService, IReviewRepository reviewRepo, IGameRepository gameRepo)
        {
            _reviewService = reviewService;
            _reviewRepo = reviewRepo;
            _gameRepo = gameRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string genre = "",
            [FromQuery] double minRating = 0,
            [FromQuery] string sort = "newest",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6)
        {
            var reviews = await _reviewRepo.GetFilteredAsync(genre, minRating, sort, page, pageSize);
            var total = await _reviewRepo.CountFilteredAsync(genre, minRating);

            var result = new ReviewListResponse { Total = total };

            foreach (var r in reviews)
            {
                var game = await _gameRepo.GetByIdAsync(r.GameId);
                result.Reviews.Add(new ReviewItemDto
                {
                    Id = r.Id,
                    GameId = r.GameId,
                    GameTitle = game?.Title ?? "Nepoznata igra",
                    GameCoverUrl = game?.CoverImageUrl ?? "",
                    GameGenre = game?.Genre ?? "",
                    UserName = r.UserName,
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

            return Ok(new ReviewItemDto
            {
                Id = r.Id,
                GameId = r.GameId,
                GameTitle = game?.Title ?? "Nepoznata igra",
                GameCoverUrl = game?.CoverImageUrl ?? "",
                GameGenre = game?.Genre ?? "",
                UserName = r.UserName,
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