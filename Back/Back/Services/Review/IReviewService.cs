using Back.DTO.Review;

namespace Back.Services.Review
{
    public interface IReviewService
    {
        /// <summary>
        /// Creates a new review for a game.After creating the review, it will also update the average rating and review count for the associated game to ensure that the game's rating information remains accurate. 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task CreateReviewAsync(CreateReviewRequest req, string userId, string userName);
    }
}
