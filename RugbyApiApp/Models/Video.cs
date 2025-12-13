namespace RugbyApiApp.Models
{
    public class Video
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string? Url { get; set; }
        public int? LengthSeconds { get; set; }
        public bool Watched { get; set; }
        public int? Rating { get; set; } // 1-5 rating
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public Game? Game { get; set; }
    }
}
