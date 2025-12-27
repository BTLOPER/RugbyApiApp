using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;

namespace RugbyApiApp.YouTubeApi
{
    /// <summary>
    /// Service for searching YouTube videos related to rugby matches
    /// </summary>
    public class YoutubeVideoService
    {
        private readonly IConfiguration _configuration;
        private readonly YouTubeService _youtubeService;

        public YoutubeVideoService(IConfiguration configuration)
        {
            _configuration = configuration;
            var apiKey = _configuration["YouTubeApiKey"] ?? "";
            
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("YouTubeApiKey not found in configuration. Please set it in user secrets.");
            }

            _youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "RugbyApiApp"
            });
        }

        /// <summary>
        /// Searches YouTube for rugby videos and retrieves detailed information
        /// </summary>
        /// <param name="homeTeam">Home team name</param>
        /// <param name="awayTeam">Away team name</param>
        /// <param name="season">Season/year (e.g., 2024)</param>
        /// <param name="maxResults">Maximum number of results to return (default 25, max 50)</param>
        /// <param name="definition">Video definition filter: "any", "standard", "high"</param>
        /// <param name="minDurationSeconds">Minimum video duration in seconds (optional)</param>
        /// <param name="maxDurationSeconds">Maximum video duration in seconds (optional)</param>
        /// <returns>List of YouTube video search results with detailed statistics</returns>
        public async Task<List<YoutubeSearchResult>> SearchVideoAsync(
            string homeTeam, 
            string awayTeam, 
            int season, 
            int maxResults = 25,
            string definition = "any",
            int? minDurationSeconds = null,
            int? maxDurationSeconds = null)
        {
            try
            {
                // Check if either team name contains "W" (Women's matches)
                bool isWomensMatch = homeTeam.Contains(" W ", StringComparison.OrdinalIgnoreCase) || 
                                     awayTeam.Contains(" W ", StringComparison.OrdinalIgnoreCase);

                // Build search query with team names and season
                var searchQuery = isWomensMatch 
                    ? $"{homeTeam} vs {awayTeam} {season} Women rugby"
                    : $"{homeTeam} vs {awayTeam} {season} rugby";

                // First, search for videos
                var searchRequest = _youtubeService.Search.List("snippet");
                searchRequest.Q = searchQuery;
                searchRequest.Type = "video";
                searchRequest.MaxResults = Math.Min(maxResults, 50);
                searchRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;

                var searchResponse = await searchRequest.ExecuteAsync();

                var results = new List<YoutubeSearchResult>();

                if (searchResponse.Items == null || searchResponse.Items.Count == 0)
                    return results;

                // Collect video IDs for batch retrieval of detailed statistics
                var videoIds = new List<string>();
                foreach (var item in searchResponse.Items)
                {
                    if (item.Id?.VideoId != null)
                    {
                        videoIds.Add(item.Id.VideoId);
                    }
                }

                if (videoIds.Count == 0)
                    return results;

                // Fetch detailed video information (statistics, contentDetails, etc.)
                var videosRequest = _youtubeService.Videos.List("snippet,contentDetails,statistics");
                videosRequest.Id = string.Join(",", videoIds);

                var videosResponse = await videosRequest.ExecuteAsync();

                if (videosResponse.Items == null || videosResponse.Items.Count == 0)
                    return results;

                // Map detailed video information to our results
                foreach (var video in videosResponse.Items)
                {
                    // Apply duration filters if specified
                    if (video.ContentDetails?.Duration != null)
                    {
                        var timespan = System.Xml.XmlConvert.ToTimeSpan(video.ContentDetails.Duration);
                        int durationSeconds = (int)timespan.TotalSeconds;

                        if (minDurationSeconds.HasValue && durationSeconds < minDurationSeconds.Value)
                            continue;
                        if (maxDurationSeconds.HasValue && durationSeconds > maxDurationSeconds.Value)
                            continue;
                    }

                    // Apply definition filter if specified
                    if (!definition.Equals("any", StringComparison.OrdinalIgnoreCase))
                    {
                        var videoDef = video.ContentDetails?.Definition?.ToLowerInvariant() ?? "unknown";
                        if (!videoDef.StartsWith(definition.ToLowerInvariant()))
                            continue;
                    }

                    results.Add(new YoutubeSearchResult
                    {
                        VideoId = video.Id,
                        Title = video.Snippet?.Title ?? "Untitled",
                        Description = video.Snippet?.Description ?? "",
                        ThumbnailUrl = video.Snippet?.Thumbnails?.Medium?.Url ?? "",
                        PublishedAt = video.Snippet?.PublishedAt,
                        ChannelTitle = video.Snippet?.ChannelTitle ?? "Unknown Channel",
                        ChannelId = video.Snippet?.ChannelId ?? "",
                        
                        // Content Details
                        Duration = video.ContentDetails?.Duration ?? "",
                        Definition = video.ContentDetails?.Definition ?? "",
                        Dimension = video.ContentDetails?.Dimension ?? "2d",
                        LicensedContent = video.ContentDetails?.LicensedContent ?? false,
                        
                        // Statistics (Convert ulong to long)
                        ViewCount = video.Statistics?.ViewCount.HasValue == true ? (long)(video.Statistics.ViewCount.Value) : 0,
                        LikeCount = video.Statistics?.LikeCount.HasValue == true ? (long)(video.Statistics.LikeCount.Value) : 0,
                        CommentCount = video.Statistics?.CommentCount.HasValue == true ? (long)(video.Statistics.CommentCount.Value) : 0
                    });
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error searching YouTube: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the full YouTube URL for a video
        /// </summary>
        public static string GetVideoUrl(string videoId) => $"https://www.youtube.com/watch?v={videoId}";

        /// <summary>
        /// Gets the embed URL for a video
        /// </summary>
        public static string GetEmbedUrl(string videoId) => $"https://www.youtube.com/embed/{videoId}";
    }

    /// <summary>
    /// Represents a YouTube video search result with detailed information
    /// </summary>
    public class YoutubeSearchResult
    {
        public string VideoId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string ThumbnailUrl { get; set; } = "";
        public DateTime? PublishedAt { get; set; }
        public string ChannelTitle { get; set; } = "";
        public string ChannelId { get; set; } = "";
        
        // Content Details
        public string Duration { get; set; } = ""; // ISO 8601 format (e.g., PT12M34S)
        public string Definition { get; set; } = ""; // "sd" or "hd"
        public string Dimension { get; set; } = "2d"; // "2d" or "3d"
        public bool LicensedContent { get; set; }
        
        // Statistics
        public long ViewCount { get; set; }
        public long LikeCount { get; set; }
        public long CommentCount { get; set; }

        /// <summary>
        /// Gets the full YouTube URL for this video
        /// </summary>
        public string VideoUrl => YoutubeVideoService.GetVideoUrl(VideoId);

        /// <summary>
        /// Formats view count as a readable number without decimals (e.g., "1.2M", "500K")
        /// </summary>
        public string FormattedViewCount => FormatLargeNumber(ViewCount);

        /// <summary>
        /// Formats like count as a readable number without decimals (e.g., "1.2M", "500K")
        /// </summary>
        public string FormattedLikeCount => FormatLargeNumber(LikeCount);

        /// <summary>
        /// Formats comment count as a readable number without decimals (e.g., "1.2M", "500K")
        /// </summary>
        public string FormattedCommentCount => FormatLargeNumber(CommentCount);

        /// <summary>
        /// Gets the definition in uppercase (e.g., "HD", "SD")
        /// </summary>
        public string FormattedDefinition => Definition.ToUpperInvariant();

        /// <summary>
        /// Converts ISO 8601 duration to human-readable format (e.g., "12:34" or "1:23:45")
        /// </summary>
        public string FormattedDuration => ConvertIso8601Duration(Duration);

        /// <summary>
        /// Converts large numbers to readable format (e.g., 1500000 -> "1.5M", 50000 -> "50K")
        /// </summary>
        private static string FormatLargeNumber(long number)
        {
            if (number == 0)
                return "0";

            if (number >= 1_000_000)
                return $"{(number / 1_000_000.0):F1}M".TrimEnd('0').TrimEnd('.');
            
            if (number >= 1_000)
                return $"{(number / 1_000.0):F1}K".TrimEnd('0').TrimEnd('.');
            
            return number.ToString();
        }

        /// <summary>
        /// Converts ISO 8601 duration format (e.g., "PT12M34S", "PT1H23M45S") to human-readable format
        /// </summary>
        private static string ConvertIso8601Duration(string duration)
        {
            if (string.IsNullOrEmpty(duration))
                return "Unknown";

            try
            {
                // Parse ISO 8601 duration using TimeSpan
                var timespan = System.Xml.XmlConvert.ToTimeSpan(duration);
                
                if (timespan.Hours > 0)
                    return $"{timespan.Hours}:{timespan.Minutes:D2}:{timespan.Seconds:D2}";
                
                return $"{timespan.Minutes}:{timespan.Seconds:D2}";
            }
            catch
            {
                return "Invalid";
            }
        }
    }
}
