using Back.DTO.Home;
using Back.Models.Enums;
using Back.Repositories.Game;
using Back.Repositories.Review;
using Back.Repositories.User;

namespace Back.Services.Home
{
    public class HomeService : IHomeService
    {
        private readonly IGameRepository _gameRepo;
        private readonly IReviewRepository _reviewRepo;
        private readonly IUserRepository _userRepo;

        public HomeService(IGameRepository gameRepo, IReviewRepository reviewRepo, IUserRepository userRepo)
        {
            _gameRepo = gameRepo;
            _reviewRepo = reviewRepo;
            _userRepo = userRepo;
        }

        public async Task<HomeResponse> GetHomeDataAsync()
        {
            var topGames = await _gameRepo.GetTopRatedAsync(6);
            var latestGames = await _gameRepo.GetLatestAsync(6);
            var recentReviews = await _reviewRepo.GetRecentAsync(5);

            var reviewDtos = new List<RecentReviewDto>();
            foreach (var review in recentReviews)
            {
                var game = await _gameRepo.GetByIdAsync(review.GameId);
                var user = await _userRepo.GetByIdAsync(review.UserId);
                reviewDtos.Add(new RecentReviewDto
                {
                    Id = review.Id,
                    GameId = review.GameId,
                    GameTitle = game?.Title ?? "Nepoznata igra",
                    GameCoverUrl = game?.CoverImageUrl ?? "",
                    UserId = review.UserId,
                    UserName = review.UserName,
                    UserProfilePictureUrl = user?.ProfilePicturePath,
                    Title = review.Title,
                    Content = review.Content,
                    Rating = review.Rating,
                    CreatedAt = review.CreatedAt
                });
            }

            var topEditors = await GetTopEditorsAsync(3);

            var totalReviews = await _reviewRepo.CountAsync();
            var totalUsers = await _userRepo.CountAsync();
            var totalEditors = await _userRepo.CountByRoleAsync(UserRole.Editor);
            var genres = await _gameRepo.GetGenresAsync();

            return new HomeResponse
            {
                TopRatedGames = topGames.Select(g => ToGameCardDto(g)).ToList(),
                LatestGames = latestGames.Select(g => ToGameCardDto(g)).ToList(),
                RecentReviews = reviewDtos,
                TopEditors = topEditors,
                Genres = genres.OrderBy(g => g).ToList(),
                Stats = new StatsDto
                {
                    TotalReviews = totalReviews,
                    TotalUsers = totalUsers,
                    TotalEditors = totalEditors
                }
            };
        }

        private async Task<List<EditorDto>> GetTopEditorsAsync(int count)
        {
            var editors = await _userRepo.GetEditorsByRoleAsync();
            var editorList = new List<EditorDto>();

            foreach (var editor in editors)
            {
                var reviews = await _reviewRepo.GetByUserIdAsync(editor.Id);
                editorList.Add(new EditorDto
                {
                    Id = editor.Id,
                    UserName = editor.UserName,
                    ProfilePictureUrl = editor.ProfilePicturePath,
                    ReviewCount = reviews.Count
                });
            }

            return editorList
                .OrderByDescending(e => e.ReviewCount)
                .Take(count)
                .ToList();
        }

        private GameCardDto ToGameCardDto(Models.Game.Game g) => new()
        {
            Id = g.Id,
            Title = g.Title,
            Genre = g.Genre,
            Developer = g.Developer,
            CoverImageUrl = g.CoverImageUrl,
            AverageRating = g.AverageRating,
            ReviewCount = g.ReviewCount,
            ReleaseYear = g.ReleaseYear
        };
    }
}
