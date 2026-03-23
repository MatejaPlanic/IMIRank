using Back.DTO.Game;
using Back.DTO.Home;
using Back.Repositories.Game;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _repo;

        public GameController(IGameRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var game = await _repo.GetByIdAsync(id);
            if (game == null) return NotFound();

            var dto = new GameCardDto
            {
                Id = game.Id,
                Title = game.Title,
                Genre = game.Genre,
                Developer = game.Developer,
                CoverImageUrl = game.CoverImageUrl,
                AverageRating = game.AverageRating,
                ReviewCount = game.ReviewCount,
                ReleaseYear = game.ReleaseYear
            };

            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string query = "",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var games = await _repo.SearchAsync(query, page, pageSize);
            var total = await _repo.CountAsync(query);

            var result = new GameSearchResponse
            {
                Total = total,
                Games = games.Select(g => new GameItemDto
                {
                    Id = g.Id,
                    Title = g.Title,
                    CoverImageUrl = g.CoverImageUrl,
                    Genre = g.Genre
                }).ToList()
            };

            return Ok(result);
        }
    }
}