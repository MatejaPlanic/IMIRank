using Back.DTO.User;
using Back.Repositories.User;

namespace Back.Services.User
{
    public interface IFollowService
    {
        Task<FollowResponse> FollowAsync(string currentUserId, string targetUserId);
        Task<FollowResponse> UnfollowAsync(string currentUserId, string targetUserId);
        Task<FollowResponse> GetFollowStatusAsync(string currentUserId, string targetUserId);
        Task<List<string>> GetFollowersAsync(string userId);
    }

    public class FollowService : IFollowService
    {
        private readonly IFollowRepository _followRepo;
        private readonly IUserRepository _userRepo;

        public FollowService(IFollowRepository followRepo, IUserRepository userRepo)
        {
            _followRepo = followRepo;
            _userRepo = userRepo;
        }

        public async Task<FollowResponse> FollowAsync(string currentUserId, string targetUserId)
        {
            if (currentUserId == targetUserId)
                throw new Exception("Ne možete pratiti samog sebe");

            await _followRepo.FollowAsync(currentUserId, targetUserId);

            var followersCount = await _followRepo.GetFollowersCountAsync(targetUserId);
            var followingCount = await _followRepo.GetFollowingCountAsync(targetUserId);
            return new FollowResponse
            {
                IsFollowing = true,
                FollowersCount = followersCount,
                FollowingCount = followingCount
            };
        }

        public async Task<FollowResponse> UnfollowAsync(string currentUserId, string targetUserId)
        {
            if (currentUserId == targetUserId)
                throw new Exception("Ne možete prestati pratiti samog sebe");

            await _followRepo.UnfollowAsync(currentUserId, targetUserId);

            var followersCount = await _followRepo.GetFollowersCountAsync(targetUserId);
            var followingCount = await _followRepo.GetFollowingCountAsync(targetUserId);
            return new FollowResponse
            {
                IsFollowing = false,
                FollowersCount = followersCount,
                FollowingCount = followingCount
            };
        }

        public async Task<FollowResponse> GetFollowStatusAsync(string currentUserId, string targetUserId)
        {
            var isFollowing = await _followRepo.IsFollowingAsync(currentUserId, targetUserId);
            var followersCount = await _followRepo.GetFollowersCountAsync(targetUserId);
            var followingCount = await _followRepo.GetFollowingCountAsync(targetUserId);

            return new FollowResponse
            {
                IsFollowing = isFollowing,
                FollowersCount = followersCount,
                FollowingCount = followingCount
            };
        }

        public async Task<List<string>> GetFollowersAsync(string userId)
        {
            return await _followRepo.GetFollowersAsync(userId);
        }
    }
}
