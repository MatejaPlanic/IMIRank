namespace Back.DTO.Admin
{
    public class AdminStatsResponse
    {
        public int TotalUsers { get; set; }
        public int TotalRegularUsers { get; set; }
        public int TotalEditors { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalReviews { get; set; }
        public int TotalGames { get; set; }
        public double AverageReviewsPerUser { get; set; }
        public double AverageRating { get; set; }
        public Dictionary<string, int> ReviewsByGenre { get; set; } = new();
        public Dictionary<string, int> UsersByMonth { get; set; } = new();
        public Dictionary<string, int> ReviewsByMonth { get; set; } = new();
    }
}