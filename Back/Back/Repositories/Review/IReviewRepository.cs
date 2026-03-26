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
        /// Gets all reviews from the database.
        /// </summary>
        /// <returns></returns>
        Task<List<Models.Review.Review>> GetAllAsync();

        /// <summary>
        /// Gets all reviews for a specific game without any sorting.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        Task<List<Models.Review.Review>> GetAllByGameIdAsync(string gameId);

        /// <summary>
        /// Gets reviews based on filtering criteria such as genre, minimum rating, sorting order, and pagination. The genre parameter allows filtering reviews by game genre, while the minRating parameter filters reviews based on a minimum rating threshold. The sort parameter specifies the sorting order (e.g., by date or rating), and the page and pageSize parameters enable pagination of results. This method returns a list of reviews that match the specified criteria.
        /// </summary>
        /// <param name="genre"></param>
        /// <param name="minRating"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<List<Models.Review.Review>> GetFilteredAsync(IEnumerable<string>? gameIds, double minRating, string sort, int page, int pageSize);

        /// <summary>
        /// Counts the number of reviews that match the specified filtering criteria, such as game IDs and minimum rating. This method is useful for determining the total number of reviews that meet the given filters, which can be used for pagination purposes when retrieving filtered reviews.
        /// </summary>
        /// <param name="gameIds"></param>
        /// <param name="minRating"></param>
        /// <returns></returns>
        Task<int> CountFilteredAsync(IEnumerable<string>? gameIds, double minRating);

        /// <summary>
        /// Retrieves a review from the database by its unique identifier (ID). Returns null if no review is found with the given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Models.Review.Review?> GetByIdAsync(string id);

        /// <summary>
        /// Gets all reviews made by a specific user, identified by their unique user ID. This method returns a list of reviews that the user has created, allowing you to see all the reviews associated with that particular user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Models.Review.Review>> GetByUserIdAsync(string userId);
    }
}
