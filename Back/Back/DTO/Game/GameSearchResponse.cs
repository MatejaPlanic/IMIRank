namespace Back.DTO.Game
{
    public class GameSearchResponse
    {
        public List<GameItemDto> Games { get; set; } = new();
        public int Total { get; set; }
    }
    public class GameItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
    }
}
