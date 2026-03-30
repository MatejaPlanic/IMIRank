namespace Back.Repositories.GameSuggestion
{
    public interface IGameSuggestionRepository
    {
        /// <summary>
        /// Creates a new game suggestion in the repository. The suggestion should include details such as the game title, description, and the user who submitted it.
        /// </summary>
        /// <param name="suggestion"></param>
        /// <returns></returns>
        Task CreateAsync(Models.GameSuggestion.GameSuggestion suggestion);

        /// <summary>
        /// Gets all game suggestions, sorted by submission date in descending order. Supports pagination through the page and pageSize parameters.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<List<Models.GameSuggestion.GameSuggestion>> GetAllAsync(int page, int pageSize);

        /// <summary>
        /// Counts the total number of game suggestions in the repository. This is useful for pagination to determine the total number of pages available based on the page size. The method returns the count of all game suggestions that have been submitted.
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();

        /// <summary>
        /// Marks a game suggestion as reviewed. This typically involves updating the suggestion's status to indicate that it has been reviewed by an administrator or moderator. Once marked as reviewed, the suggestion may no longer appear in the list of pending suggestions for review.   
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task MarkAsReviewedAsync(string id);
    }
}