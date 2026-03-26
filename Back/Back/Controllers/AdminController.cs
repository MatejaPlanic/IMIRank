using Back.DTO.Admin;
using Back.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _adminService.GetAdminStatsAsync();
            return Ok(stats);
        }

        [HttpPost("games")]
        public async Task<IActionResult> AddGame([FromForm] AddGameRequest request)
        {
            try
            {
                await _adminService.AddGameAsync(request);
                return Ok(new { message = "Igra uspešno dodana" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}