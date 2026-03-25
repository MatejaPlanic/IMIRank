using Back.Models.Notifications;

namespace Back.Repositories.Notification
{
    public interface INotificationRepository
    {
        Task<Back.Models.Notifications.Notification> CreateAsync(Back.Models.Notifications.Notification notification);
        Task<List<Back.Models.Notifications.Notification>> GetByRecipientAsync(string recipientUserId, int page = 1, int pageSize = 50);
        Task<long> CountByRecipientAsync(string recipientUserId);
        Task<Back.Models.Notifications.Notification?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(string id, Back.Models.Notifications.Notification notification);
    }
}
