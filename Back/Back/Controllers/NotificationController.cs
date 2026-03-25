using Back.DTO.Notification;
using Back.Services.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyNotifications([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User information not found");

            var result = await _notificationService.GetNotificationsByUserAsync(userId, page, pageSize);
            return Ok(result);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkRead(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User information not found");

            var success = await _notificationService.MarkAsReadAsync(id, userId);
            if (!success)
                return NotFound("Notifikacija nije pronađena ili nemate pristup");

            return Ok();
        }
    }
}
