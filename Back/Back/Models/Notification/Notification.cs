using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Back.Models.Notifications
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string RecipientUserId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string ActorUserId { get; set; } = string.Empty;

        public string ActorUserName { get; set; } = string.Empty;

        public string? ActorProfilePictureUrl { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ReviewId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ReviewCommentId { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
