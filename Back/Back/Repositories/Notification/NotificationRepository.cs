using Back.Config;
using Back.Models.Notifications;
using MongoDB.Driver;

namespace Back.Repositories.Notification
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IMongoCollection<global::Back.Models.Notifications.Notification> _notifications;

        public NotificationRepository(MongoDBContext context)
        {
            _notifications = context.GetCollection<global::Back.Models.Notifications.Notification>("notifications");
        }

        public async Task<Back.Models.Notifications.Notification> CreateAsync(Back.Models.Notifications.Notification notification)
        {
            await _notifications.InsertOneAsync(notification);
            return notification;
        }

        public async Task<List<Back.Models.Notifications.Notification>> GetByRecipientAsync(string recipientUserId, int page = 1, int pageSize = 50)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 50;

            return await _notifications.Find(n => n.RecipientUserId == recipientUserId)
                .SortByDescending(n => n.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<long> CountByRecipientAsync(string recipientUserId)
        {
            return await _notifications.CountDocumentsAsync(n => n.RecipientUserId == recipientUserId);
        }

        public async Task<Back.Models.Notifications.Notification?> GetByIdAsync(string id)
        {
            return await _notifications.Find(n => n.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(string id, Back.Models.Notifications.Notification notification)
        {
            var update = Builders<Back.Models.Notifications.Notification>.Update
                .Set(x => x.IsRead, notification.IsRead);

            var result = await _notifications.UpdateOneAsync(n => n.Id == id, update);
            return result.ModifiedCount > 0;
        }
    }
}
