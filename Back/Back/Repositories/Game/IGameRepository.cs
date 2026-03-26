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

        /// <summary>
        /// Searches for games based on a query string that matches the game title. The search is case-insensitive and supports pagination through the page and pageSize parameters. If the query is empty or null, it returns all games paginated.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<List<Models.Game.Game>> SearchAsync(string query, int page, int pageSize);

        /// <summary>
        /// Counts the total number of games that match the search query. This is useful for pagination to determine the total number of pages available based on the page size. If the query is empty or null, it counts all games.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<int> CountAsync(string query);

        /// <summary>
        /// Creates a new game in the repository.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        Task CreateAsync(Models.Game.Game game);

        /// <summary>
        /// Gets list of available genres for games.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetGenresAsync();

        /// <summary>
        /// Gets IDs of games for a specific genre.
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        Task<List<string>> GetGameIdsByGenreAsync(string genre);
    }
}