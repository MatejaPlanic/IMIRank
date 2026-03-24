namespace Back.DTO.Review
{
    public class CreateReviewCommentRequest
    {
        public string ReviewId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    public class UpdateReviewCommentRequest
    {
        public string Content { get; set; } = string.Empty;
    }

    public class ReviewCommentResponse
    {
        public string Id { get; set; } = string.Empty;
        public string ReviewId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ReviewCommentsListResponse
    {
        public List<ReviewCommentResponse> Comments { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    }
}
