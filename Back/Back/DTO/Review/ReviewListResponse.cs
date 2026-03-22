namespace Back.DTO.Review
{
    public class ReviewListResponse
    {
        public List<ReviewItemDto> Reviews { get; set; } = new();
        public int Total { get; set; }
    }
    public class ReviewItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string GameId { get; set; } = string.Empty;
        public string GameTitle { get; set; } = string.Empty;
        public string GameCoverUrl { get; set; } = string.Empty;
        public string GameGenre { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? UserProfilePictureUrl { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
