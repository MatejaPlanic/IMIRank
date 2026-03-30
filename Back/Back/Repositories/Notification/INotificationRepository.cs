using Back.Models.Notifications;

namespace Back.Repositories.Notification
{
    public interface INotificationRepository
    {
        /// <summary>
        /// Creates a new notification in the repository. The notification should include details such as the recipient user ID, actor user ID, message, and any relevant references (e.g., review ID, comment ID). The method returns the created notification with its assigned unique identifier.
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        Task<Back.Models.Notifications.Notification> CreateAsync(Back.Models.Notifications.Notification notification);

        /// <summary>
        /// Gets a list of notifications for a specific recipient user ID. The notifications are sorted by creation date in descending order (most recent first). Supports pagination through the page and pageSize parameters to efficiently retrieve large sets of notifications.
        /// </summary>
        /// <param name="recipientUserId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<List<Back.Models.Notifications.Notification>> GetByRecipientAsync(string recipientUserId, int page = 1, int pageSize = 50);

        /// <summary>
        /// Counts the total number of notifications for a specific recipient user ID. This is useful for pagination to determine the total number of pages available based on the page size. The method returns the count of notifications that match the recipient user ID.
        /// </summary>
        /// <param name="recipientUserId"></param>
        /// <returns></returns>
        Task<long> CountByRecipientAsync(string recipientUserId);

        /// <summary>
        /// Gets a notification by its unique identifier. Returns null if no notification is found with the given ID.
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Back.Models.Notifications.Notification?> GetByIdAsync(string id);

        /// <summary>
        /// Updates an existing notification identified by its unique identifier. The method allows updating the notification's details such as the message, read status, or any relevant references. It returns a boolean indicating whether the update was successful (true if the notification was found and updated, false otherwise).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notification"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(string id, Back.Models.Notifications.Notification notification);
    }
}
