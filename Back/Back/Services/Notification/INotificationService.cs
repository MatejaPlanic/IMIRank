using Back.DTO.Notification;

namespace Back.Services.Notification
{
    public interface INotificationService
    {
        /// <summary>
        /// Creates a new notification for a user. This method takes the recipientUserId to identify the user who will receive the notification, the actorUserId and actorUserName to identify the user who triggered the notification, an optional actorProfilePictureUrl for the actor's profile picture, the reviewId and an optional reviewCommentId to link the notification to a specific review or comment, and a message that describes the notification. The method will create a new notification entry in the database with this information and return a NotificationResponse object containing the details of the created notification.
        /// </summary>
        /// <param name="recipientUserId"></param>
        /// <param name="actorUserId"></param>
        /// <param name="actorUserName"></param>
        /// <param name="actorProfilePictureUrl"></param>
        /// <param name="reviewId"></param>
        /// <param name="reviewCommentId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<NotificationResponse> CreateNotificationAsync(string recipientUserId, string actorUserId, string actorUserName, string? actorProfilePictureUrl, string reviewId, string? reviewCommentId, string message);

        /// <summary>
        /// Retrieves a paginated list of notifications for a specific user identified by their unique identifier (recipientUserId). The method supports pagination through the page and pageSize parameters, allowing efficient retrieval of notifications in batches. It returns a NotificationListResponse object that contains a list of NotificationResponse objects for the specified page, along with pagination metadata such as total count and total pages. If no notifications are found for the given recipientUserId, an empty list will be returned in the NotificationListResponse.
        /// </summary>
        /// <param name="recipientUserId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<NotificationListResponse> GetNotificationsByUserAsync(string recipientUserId, int page = 1, int pageSize = 50);

        /// <summary>
        /// Marks a specific notification as read for a user. This method takes the notificationId to identify the notification that should be marked as read and the recipientUserId to ensure that the notification belongs to the correct user. The method will update the read status of the specified notification in the database and return a boolean indicating whether the operation was successful (true if the notification was found and marked as read, false otherwise). If the notification does not exist or does not belong to the specified user, it will return false, indicating that the operation was not successful.
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="recipientUserId"></param>
        /// <returns></returns>
        Task<bool> MarkAsReadAsync(string notificationId, string recipientUserId);
    }
}
