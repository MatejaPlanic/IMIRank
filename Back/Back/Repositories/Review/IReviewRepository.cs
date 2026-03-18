namespace Back.Repositories.Review
{
    public interface IReviewRepository
    {
        /// <summary>
        /// Gets the most recent reviews, sorted by creation date in descending order.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<Models.Review.Review>> GetRecentAsync(int count);

        /// <summary>
        /// Gets all reviews for a specific game, sorted by creation date in descending order.
        /// <param name="gameId"></param>
        /// <returns></returns>
        Task<List<Models.Review.Review>> GetByGameIdAsync(string gameId);

        /// <summary>
        /// Creates a new review in the database. The review object should contain all necessary information (e.g., game ID, user ID, rating, comment) before calling this method.
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        Task CreateAsync(Models.Review.Review review);

        /// <summary>
        /// Counts the total number of reviews in the database.
        /// </summary>
        /// <returns></returns>
        Task<long> CountAsync();

        /// <summary>
        /// Gets all reviews for a specific game without any sorting.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        Task<List<Models.Review.Review>> GetAllByGameIdAsync(string gameId);

        Task<List<Models.Review.Review>> GetFilteredAsync(string genre, double minRating, string sort, int page, int pageSize);
        Task<int> CountFilteredAsync(string genre, double minRating);
        Task<Models.Review.Review?> GetByIdAsync(string id);
    }
}
