using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Back.Models.User
{
    public class Follow
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string FollowerId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string FollowingId { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
