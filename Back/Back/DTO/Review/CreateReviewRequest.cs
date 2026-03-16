namespace Back.DTO.Review
{
    public class CreateReviewRequest
    {
        public string GameId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public double Rating { get; set; }
    }
}
