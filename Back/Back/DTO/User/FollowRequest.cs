namespace Back.DTO.User
{
    public class FollowRequest
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class FollowResponse
    {
        public bool IsFollowing { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }
}
