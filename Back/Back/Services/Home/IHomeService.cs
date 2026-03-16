using Back.DTO.Home;

namespace Back.Services.Home
{
    public interface IHomeService
    {
        /// <summary>
        /// Gets the data needed for the home page, including top-rated games, latest games, recent reviews, and overall statistics. 
        /// <returns></returns>
        Task<HomeResponse> GetHomeDataAsync();
    }
}
