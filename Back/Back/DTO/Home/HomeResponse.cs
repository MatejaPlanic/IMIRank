namespace Back.DTO.Home
{
    public class HomeResponse
    {
        public List<GameCardDto> TopRatedGames { get; set; } = new();
        public List<GameCardDto> LatestGames { get; set; } = new();
        public List<RecentReviewDto> RecentReviews { get; set; } = new();
        public List<EditorDto> TopEditors { get; set; } = new();
        public List<string> Genres { get; set; } = new();
        public StatsDto Stats { get; set; } = new();
    }
    public class GameCardDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Developer { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public int ReleaseYear { get; set; }
    }
    public class RecentReviewDto
    {
        public string Id { get; set; } = string.Empty;
        public string GameId { get; set; } = string.Empty;
        public string GameTitle { get; set; } = string.Empty;
        public string GameCoverUrl { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? UserProfilePictureUrl { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class EditorDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public int ReviewCount { get; set; }
    }
    public class StatsDto
    {
        public long TotalReviews { get; set; }
        public long TotalUsers { get; set; }
        public long TotalEditors { get; set; }
    }
}
