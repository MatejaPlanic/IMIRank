using Back.DTO.Review;
using Back.Repositories.Review;
using Back.Services.Notification;
using Back.Services.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Back.Hubs;
using System.Security.Claims;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewCommentController : ControllerBase
    {
        private readonly IReviewCommentService _commentService;
        private readonly IReviewRepository _reviewRepository;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<ReviewCommentsHub> _hubContext;
        private readonly IHubContext<NotificationsHub> _notificationHubContext;

        public ReviewCommentController(
            IReviewCommentService commentService,
            IReviewRepository reviewRepository,
            INotificationService notificationService,
            IHubContext<ReviewCommentsHub> hubContext,
            IHubContext<NotificationsHub> notificationHubContext)
        {
            _commentService = commentService;
            _reviewRepository = reviewRepository;
            _notificationService = notificationService;
            _hubContext = hubContext;
            _notificationHubContext = notificationHubContext;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult<ReviewCommentResponse>> CreateComment([FromBody] CreateReviewCommentRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
                    return Unauthorized("User information not found");

                var comment = await _commentService.CreateCommentAsync(request.ReviewId, userId, userName, request);

                await _hubContext.Clients.Group($"review-{request.ReviewId}")
                    .SendAsync("ReceiveComment", comment);

                var review = await _reviewRepository.GetByIdAsync(request.ReviewId);
                if (review != null && review.UserId != userId)
                {
                    var notification = await _notificationService.CreateNotificationAsync(
                        recipientUserId: review.UserId,
                        actorUserId: userId,
                        actorUserName: userName,
                        actorProfilePictureUrl: comment.UserProfilePictureUrl,
                        reviewId: review.Id,
                        reviewCommentId: comment.Id,
                        message: $"{userName} je komentarisao vaš review"
                    );

                    await _notificationHubContext.Clients.Group($"notifications-{review.UserId}")
                        .SendAsync("NotificationReceived", notification);
                }

                return Ok(comment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{commentId}")]
        public async Task<ActionResult<ReviewCommentResponse>> GetComment(string commentId)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(commentId);
                if (comment == null)
                    return NotFound("Komentar nije pronađen");

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("review/{reviewId}")]
        public async Task<ActionResult<ReviewCommentsListResponse>> GetCommentsByReview(string reviewId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var comments = await _commentService.GetCommentsByReviewIdAsync(reviewId, page, pageSize);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{commentId}")]
        [Authorize]
        public async Task<ActionResult<bool>> UpdateComment(string commentId, [FromBody] UpdateReviewCommentRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User information not found");

                var comment = await _commentService.GetCommentByIdAsync(commentId);
                if (comment == null)
                    return NotFound("Komentar nije pronađen");

                var result = await _commentService.UpdateCommentAsync(commentId, userId, request);

                if (result)
                {
                    var updatedComment = await _commentService.GetCommentByIdAsync(commentId);
                    await _hubContext.Clients.Group($"review-{comment.ReviewId}")
                        .SendAsync("UpdateComment", updatedComment);
                }

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{commentId}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteComment(string commentId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User information not found");

                var comment = await _commentService.GetCommentByIdAsync(commentId);
                if (comment == null)
                    return NotFound("Komentar nije pronađen");

                var result = await _commentService.DeleteCommentAsync(commentId, userId);

                if (result)
                {
                    await _hubContext.Clients.Group($"review-{comment.ReviewId}")
                        .SendAsync("DeleteComment", commentId);
                }

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
