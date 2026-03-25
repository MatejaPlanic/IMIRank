using Back.DTO.Notification;

namespace Back.Services.Notification
{
    public interface INotificationService
    {
        Task<NotificationResponse> CreateNotificationAsync(string recipientUserId, string actorUserId, string actorUserName, string? actorProfilePictureUrl, string reviewId, string? reviewCommentId, string message);
        Task<NotificationListResponse> GetNotificationsByUserAsync(string recipientUserId, int page = 1, int pageSize = 50);
        Task<bool> MarkAsReadAsync(string notificationId, string recipientUserId);
    }
}
