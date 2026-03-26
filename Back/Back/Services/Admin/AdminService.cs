using Back.DTO.Admin;
using Back.Models.Enums;
using Back.Repositories.Game;
using Back.Repositories.Review;
using Back.Repositories.User;

namespace Back.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepo;
        private readonly IReviewRepository _reviewRepo;
        private readonly IGameRepository _gameRepo;

        public AdminService(IUserRepository userRepo, IReviewRepository reviewRepo, IGameRepository gameRepo)
        {
            _userRepo = userRepo;
            _reviewRepo = reviewRepo;
            _gameRepo = gameRepo;
        }

        public async Task<AdminStatsResponse> GetAdminStatsAsync()
        {
            var totalUsers = (int)await _userRepo.CountAsync();
            var totalRegularUsers = (int)await _userRepo.CountByRoleAsync(UserRole.RegularUser);
            var totalEditors = (int)await _userRepo.CountByRoleAsync(UserRole.Editor);
            var totalAdmins = (int)await _userRepo.CountByRoleAsync(UserRole.Admin);
            var totalReviews = (int)await _reviewRepo.CountAsync();
            var totalGames = (int)await _gameRepo.CountAsync("");

            var averageReviewsPerUser = totalUsers > 0 ? (double)totalReviews / totalUsers : 0;

            var allReviews = await _reviewRepo.GetAllAsync();
            var averageRating = allReviews.Any() ? allReviews.Average(r => r.Rating) : 0;

            var reviewsByGenre = new Dictionary<string, int>();
            var games = await _gameRepo.SearchAsync("", 1, 1000); 
            foreach (var game in games)
            {
                var gameReviews = await _reviewRepo.GetByGameIdAsync(game.Id);
                if (reviewsByGenre.ContainsKey(game.Genre))
                    reviewsByGenre[game.Genre] += gameReviews.Count;
                else
                    reviewsByGenre[game.Genre] = gameReviews.Count;
            }

            var usersByMonth = new Dictionary<string, int>();
            var allUsers = await _userRepo.GetAllAsync();
            foreach (var user in allUsers)
            {
                var monthKey = user.CreatedAt.ToString("yyyy-MM");
                if (usersByMonth.ContainsKey(monthKey))
                    usersByMonth[monthKey]++;
                else
                    usersByMonth[monthKey] = 1;
            }

            var reviewsByMonth = new Dictionary<string, int>();
            foreach (var review in allReviews)
            {
                var monthKey = review.CreatedAt.ToString("yyyy-MM");
                if (reviewsByMonth.ContainsKey(monthKey))
                    reviewsByMonth[monthKey]++;
                else
                    reviewsByMonth[monthKey] = 1;
            }

            return new AdminStatsResponse
            {
                TotalUsers = totalUsers,
                TotalRegularUsers = totalRegularUsers,
                TotalEditors = totalEditors,
                TotalAdmins = totalAdmins,
                TotalReviews = totalReviews,
                TotalGames = totalGames,
                AverageReviewsPerUser = Math.Round(averageReviewsPerUser, 2),
                AverageRating = Math.Round(averageRating, 2),
                ReviewsByGenre = reviewsByGenre,
                UsersByMonth = usersByMonth,
                ReviewsByMonth = reviewsByMonth
            };
        }

        public async Task AddGameAsync(AddGameRequest request)
        {
            string coverImageUrl = "";

            if (request.CoverImage != null)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                var fileName = $"{Guid.NewGuid()}_{request.CoverImage.FileName}";
                var filePath = Path.Combine(uploadsDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.CoverImage.CopyToAsync(stream);
                }

                coverImageUrl = $"/images/{fileName}";
            }

            var game = new Models.Game.Game
            {
                Title = request.Title,
                Genre = request.Genre,
                Developer = request.Developer,
                Description = request.Description,
                ReleaseYear = request.ReleaseYear,
                CoverImageUrl = coverImageUrl
            };

            await _gameRepo.CreateAsync(game);
        }
    }
}