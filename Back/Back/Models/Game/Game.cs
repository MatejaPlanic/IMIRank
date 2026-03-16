using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Back.Models.Game
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Developer { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public double AverageRating { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;
    }
}
