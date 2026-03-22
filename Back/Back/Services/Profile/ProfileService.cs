using Back.DTO.Profile;
using Back.Repositories.Review;
using Back.Repositories.User;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Back.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _userRepo;
        private readonly IReviewRepository _reviewRepo;

        public ProfileService(IUserRepository userRepo, IReviewRepository reviewRepo)
        {
            _userRepo = userRepo;
            _reviewRepo = reviewRepo;
        }

        public async Task<ProfileResponse> GetProfileAsync(string userId)
        {
            var user = await _userRepo.GetByIdAsync(userId)
                ?? throw new Exception("Korisnik nije pronađen");

            var allReviews = await _reviewRepo.GetByUserIdAsync(userId);

            var recentReviews = allReviews
                .OrderByDescending(r => r.CreatedAt)
                .Take(3)
                .Select(r => new ProfileReviewDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    GameId = r.GameId,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt
                })
                .ToList();

            return new ProfileResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString(),
                TotalReviews = allReviews.Count,
                ProfilePictureUrl = user.ProfilePicturePath != null
                    ? $"/{user.ProfilePicturePath}"
                    : null,
                CreatedAt = user.CreatedAt,
                RecentReviews = recentReviews
            };
        }

        public async Task UpdateUsernameAsync(string userId, UpdateUsernameRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.NewUserName))
                throw new Exception("Korisničko ime ne može biti prazno");
            await _userRepo.UpdateUsernameAsync(userId, req.NewUserName);
        }

        public async Task UpdatePasswordAsync(string userId, UpdatePasswordRequest req)
        {
            if (req.NewPassword != req.ConfirmPassword)
                throw new Exception("Lozinke se ne podudaraju");

            var user = await _userRepo.GetByIdAsync(userId)
                ?? throw new Exception("Korisnik nije pronađen");

            if (!BCrypt.Net.BCrypt.Verify(req.OldPassword, user.Password))
                throw new Exception("Stara lozinka nije ispravna");

            await _userRepo.UpdatePasswordAsync(userId, BCrypt.Net.BCrypt.HashPassword(req.NewPassword));
        }
    }
}
