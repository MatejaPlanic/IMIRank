namespace Back.Repositories.GameSuggestion
{
    public interface IGameSuggestionRepository
    {
        Task CreateAsync(Models.GameSuggestion.GameSuggestion suggestion);
        Task<List<Models.GameSuggestion.GameSuggestion>> GetAllAsync(int page, int pageSize);
        Task<int> CountAsync();
        Task MarkAsReviewedAsync(string id);
    }
}