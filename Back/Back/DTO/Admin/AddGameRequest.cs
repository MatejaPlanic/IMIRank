namespace Back.DTO.Admin
{
    public class AddGameRequest
    {
        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string Developer { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int ReleaseYear { get; set; }
        public IFormFile? CoverImage { get; set; }
    }
}