using Back.DTO.Admin;

namespace Back.Services.Admin
{
    public interface IAdminService
    {
        /// <summary>
        /// Retrieves various statistics for the admin dashboard, including total user count, total game count, total review count, and average rating across all games. This method aggregates data from the database to provide insights into the overall usage and engagement on the platform. The returned AdminStatsResponse object contains these statistics, which can be used to monitor the health and growth of the platform.
        /// </summary>
        /// <returns></returns>
        Task<AdminStatsResponse> GetAdminStatsAsync();

        /// <summary>
        /// Adds a new game to the platform. This method takes an AddGameRequest object that contains the necessary information about the game, such as its title, genre, release date, and other relevant details. The method will validate the input data and create a new game entry in the database. If the game is successfully added, it will return without any issues. If there are validation errors or if a game with the same title already exists, an exception will be thrown indicating the specific issue that occurred during the game addition process.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddGameAsync(AddGameRequest request);
    }
}