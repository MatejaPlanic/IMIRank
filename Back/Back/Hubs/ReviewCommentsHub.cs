using Microsoft.AspNetCore.SignalR;

namespace Back.Hubs
{
    public class ReviewCommentsHub : Hub
    {
        public async Task JoinReviewGroup(string reviewId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"review-{reviewId}");
        }

        public async Task LeaveReviewGroup(string reviewId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"review-{reviewId}");
        }

        public async Task BroadcastNewComment(string reviewId, object comment)
        {
            await Clients.Group($"review-{reviewId}").SendAsync("ReceiveComment", comment);
        }

        public async Task BroadcastUpdateComment(string reviewId, object comment)
        {
            await Clients.Group($"review-{reviewId}").SendAsync("UpdateComment", comment);
        }

        public async Task BroadcastDeleteComment(string reviewId, string commentId)
        {
            await Clients.Group($"review-{reviewId}").SendAsync("DeleteComment", commentId);
        }
    }
}
