namespace Back.DTO.GameSuggestion
{
    public class CreateGameSuggestionRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Developer { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }

    public class GameSuggestionResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Developer { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public bool IsReviewed { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GameSuggestionListResponse
    {
        public List<GameSuggestionResponse> Suggestions { get; set; } = new();
        public int Total { get; set; }
    }
}