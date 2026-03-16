namespace Back.Repositories.Game
{
    public interface IGameRepository
    {
        /// <summary>
        /// Gets the top-rated games, sorted by average rating in descending order.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<Models.Game.Game>> GetTopRatedAsync(int count);

        /// <summary>
        /// Gets the latest games, sorted by release year in descending order.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<Models.Game.Game>> GetLatestAsync(int count);

        /// <summary>
        /// Gets a game by its unique identifier. Returns null if no game is found with the given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Models.Game.Game?> GetByIdAsync(string id);

        /// <summary>
        /// Updates the average rating and review count for a game. This method is typically called after a new review is added or an existing review is updated/deleted to ensure the game's rating information remains accurate.
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="newAverage"></param>
        /// <param name="newCount"></param>
        /// <returns></returns>
        Task UpdateRatingAsync(string gameId, double newAverage, int newCount);
    }
}