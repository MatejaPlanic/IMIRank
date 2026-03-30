using Back.DTO.User;
using Back.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers.User
{
    [ApiController]
    [Route("api/follow")]
    [Authorize]
    public class FollowController : ControllerBase
    {
        private readonly IFollowService _followService;

        public FollowController(IFollowService followService)
        {
            _followService = followService;
        }

        private string GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return userIdClaim?.Value ?? throw new Exception("User not authenticated");
        }

        [HttpPost("follow/{userId}")]
        public async Task<IActionResult> Follow(string userId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var result = await _followService.FollowAsync(currentUserId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("unfollow/{userId}")]
        public async Task<IActionResult> Unfollow(string userId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var result = await _followService.UnfollowAsync(currentUserId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("status/{userId}")]
        public async Task<IActionResult> GetFollowStatus(string userId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var result = await _followService.GetFollowStatusAsync(currentUserId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
