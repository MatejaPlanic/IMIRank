namespace Back.Repositories.User
{
    public interface IFollowRepository
    {
        Task<bool> IsFollowingAsync(string followerId, string followingId);
        Task FollowAsync(string followerId, string followingId);
        Task UnfollowAsync(string followerId, string followingId);
        Task<List<string>> GetFollowersAsync(string userId);
        Task<List<string>> GetFollowingAsync(string userId);
        Task<int> GetFollowersCountAsync(string userId);
        Task<int> GetFollowingCountAsync(string userId);
    }
}
