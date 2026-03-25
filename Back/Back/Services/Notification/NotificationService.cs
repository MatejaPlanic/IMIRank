using Back.DTO.Notification;
using Back.Models.Notifications;
using Back.Repositories.Notification;

namespace Back.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<NotificationResponse> CreateNotificationAsync(string recipientUserId, string actorUserId, string actorUserName, string? actorProfilePictureUrl, string reviewId, string? reviewCommentId, string message)
        {
            var notification = new Back.Models.Notifications.Notification
            {
                RecipientUserId = recipientUserId,
                ActorUserId = actorUserId,
                ActorUserName = actorUserName,
                ActorProfilePictureUrl = actorProfilePictureUrl,
                ReviewId = reviewId,
                ReviewCommentId = reviewCommentId,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _notificationRepository.CreateAsync(notification);
            return MapToDto(created);
        }

        public async Task<NotificationListResponse> GetNotificationsByUserAsync(string recipientUserId, int page = 1, int pageSize = 50)
        {
            var notifications = await _notificationRepository.GetByRecipientAsync(recipientUserId, page, pageSize);
            var count = await _notificationRepository.CountByRecipientAsync(recipientUserId);

            return new NotificationListResponse
            {
                Notifications = notifications.Select(MapToDto).ToList(),
                TotalCount = (int)count
            };
        }

        public async Task<bool> MarkAsReadAsync(string notificationId, string recipientUserId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null || notification.RecipientUserId != recipientUserId)
                return false;

            if (notification.IsRead) return true;

            notification.IsRead = true;
            return await _notificationRepository.UpdateAsync(notificationId, notification);
        }

        private NotificationResponse MapToDto(Back.Models.Notifications.Notification notification)
        {
            return new NotificationResponse
            {
                Id = notification.Id,
                RecipientUserId = notification.RecipientUserId,
                ActorUserId = notification.ActorUserId,
                ActorUserName = notification.ActorUserName,
                ActorProfilePictureUrl = notification.ActorProfilePictureUrl,
                ReviewId = notification.ReviewId,
                ReviewCommentId = notification.ReviewCommentId,
                Message = notification.Message,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }
    }
}
