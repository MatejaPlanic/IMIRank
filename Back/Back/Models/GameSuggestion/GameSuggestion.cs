using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Back.Models.GameSuggestion
{
    public class GameSuggestion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Developer { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public bool IsReviewed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}