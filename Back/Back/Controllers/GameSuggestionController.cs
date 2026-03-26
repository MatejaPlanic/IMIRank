using Back.DTO.GameSuggestion;
using Back.Repositories.GameSuggestion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameSuggestionController : ControllerBase
    {
        private readonly IGameSuggestionRepository _repo;

        public GameSuggestionController(IGameSuggestionRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateGameSuggestionRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var userName = User.FindFirstValue(ClaimTypes.Email)!;

            var suggestion = new Models.GameSuggestion.GameSuggestion
            {
                UserId = userId,
                UserName = userName,
                Title = req.Title,
                Genre = req.Genre,
                Developer = req.Developer,
                Note = req.Note
            };

            await _repo.CreateAsync(suggestion);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var suggestions = await _repo.GetAllAsync(page, pageSize);
            var total = await _repo.CountAsync();

            return Ok(new GameSuggestionListResponse
            {
                Total = total,
                Suggestions = suggestions.Select(s => new GameSuggestionResponse
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    UserName = s.UserName,
                    Title = s.Title,
                    Genre = s.Genre,
                    Developer = s.Developer,
                    Note = s.Note,
                    IsReviewed = s.IsReviewed,
                    CreatedAt = s.CreatedAt
                }).ToList()
            });
        }

        [HttpPut("{id}/reviewed")]
        [Authorize]
        public async Task<IActionResult> MarkReviewed(string id)
        {
            await _repo.MarkAsReviewedAsync(id);
            return Ok();
        }
    }
}