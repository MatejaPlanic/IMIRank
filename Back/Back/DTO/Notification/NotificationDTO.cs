namespace Back.DTO.Notification
{
    public class NotificationResponse
    {
        public string Id { get; set; } = string.Empty;
        public string RecipientUserId { get; set; } = string.Empty;
        public string ActorUserId { get; set; } = string.Empty;
        public string ActorUserName { get; set; } = string.Empty;
        public string? ActorProfilePictureUrl { get; set; }
        public string ReviewId { get; set; } = string.Empty;
        public string? ReviewCommentId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class NotificationListResponse
    {
        public List<NotificationResponse> Notifications { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
