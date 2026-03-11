using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Back.Models.Enums;

namespace Back.Models.User
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.RegularUser;
    }
}
