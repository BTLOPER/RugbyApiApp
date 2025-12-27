namespace RugbyApiApp.Models
{
    /// <summary>
    /// Stores YouTube video search results for a game
    /// Used to cache search results and allow users to review before adding to Video table
    /// </summary>
    public class YouTubeVideoSearchResult
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string VideoId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string ThumbnailUrl { get; set; } = "";
        public string ChannelTitle { get; set; } = "";
        public string ChannelId { get; set; } = "";
        
        // Content Details
        public string? Duration { get; set; } // ISO 8601 format (e.g., PT12M34S)
        public string? Definition { get; set; } // "sd" or "hd"
        public string? Dimension { get; set; } // "2d" or "3d"
        public bool LicensedContent { get; set; }
        
        // Statistics
        public long ViewCount { get; set; }
        public long LikeCount { get; set; }
        public long CommentCount { get; set; }
        
        // Dates
        public DateTime PublishedAt { get; set; }
        public DateTime SearchedAt { get; set; } // When we retrieved this search result
        
        // User Control
        public bool IsIgnored { get; set; } = false; // User marked as not useful
        
        // Metadata
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Game? Game { get; set; }
    }
}
