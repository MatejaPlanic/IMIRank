using Back.DTO.Profile;
using Back.Repositories.Review;
using Back.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IReviewRepository _reviewRepo;

        public UserController(IUserRepository userRepo, IReviewRepository reviewRepo)
        {
            _userRepo = userRepo;
            _reviewRepo = reviewRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var users = await _userRepo.SearchByUserNameAsync(query, page, pageSize);
            var total = await _userRepo.CountByUserNameAsync(query);

            var resultTasks = users.Select(async user => new PublicProfileDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role.ToString(),
                ProfilePictureUrl = user.ProfilePicturePath != null ? $"/{user.ProfilePicturePath}" : null,
                TotalReviews = (await _reviewRepo.GetByUserIdAsync(user.Id)).Count
            });

            var result = (await Task.WhenAll(resultTasks)).ToList();

            return Ok(new { total, users = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();

            var reviews = await _reviewRepo.GetByUserIdAsync(id);
            var response = new PublicProfileDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role.ToString(),
                ProfilePictureUrl = user.ProfilePicturePath != null ? $"/{user.ProfilePicturePath}" : null,
                TotalReviews = reviews.Count,
                RecentReviews = reviews
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(10)
                    .Select(r => new PublicProfileReviewDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        GameId = r.GameId,
                        Rating = r.Rating,
                        CreatedAt = r.CreatedAt
                    })
                    .ToList()
            };

            return Ok(response);
        }
    }
}
