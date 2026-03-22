namespace Back.DTO.Profile
{
    public class ProfileReviewDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string GameId { get; set; } = string.Empty;
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ProfileResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public int TotalReviews { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProfileReviewDto> RecentReviews { get; set; } = new();
    }

    public class UpdateUsernameRequest
    {
        public string NewUserName { get; set; } = string.Empty;
    }

    public class UpdatePasswordRequest
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}