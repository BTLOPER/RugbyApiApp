using Microsoft.EntityFrameworkCore;
using RugbyApiApp.Data;
using RugbyApiApp.Models;

namespace RugbyApiApp.Services
{
    public partial class DataService
    {
        /// <summary>
        /// Add or update a YouTube video search result
        /// </summary>
        public async Task<YouTubeVideoSearchResult> UpsertYouTubeSearchResultAsync(int gameId, string videoId, string title, string description, string thumbnailUrl, string channelTitle, string channelId, string? duration, string? definition, string? dimension, bool licensedContent, long viewCount, long likeCount, long commentCount, DateTime publishedAt)
        {
            var existing = await _context.YouTubeVideoSearchResults
                .FirstOrDefaultAsync(y => y.GameId == gameId && y.VideoId == videoId);

            if (existing != null)
            {
                existing.Title = title;
                existing.Description = description;
                existing.ThumbnailUrl = thumbnailUrl;
                existing.ChannelTitle = channelTitle;
                existing.ChannelId = channelId;
                existing.Duration = duration;
                existing.Definition = definition;
                existing.Dimension = dimension;
                existing.LicensedContent = licensedContent;
                existing.ViewCount = viewCount;
                existing.LikeCount = likeCount;
                existing.CommentCount = commentCount;
                existing.PublishedAt = publishedAt;
                existing.SearchedAt = DateTime.UtcNow;
                existing.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                existing = new YouTubeVideoSearchResult
                {
                    GameId = gameId,
                    VideoId = videoId,
                    Title = title,
                    Description = description,
                    ThumbnailUrl = thumbnailUrl,
                    ChannelTitle = channelTitle,
                    ChannelId = channelId,
                    Duration = duration,
                    Definition = definition,
                    Dimension = dimension,
                    LicensedContent = licensedContent,
                    ViewCount = viewCount,
                    LikeCount = likeCount,
                    CommentCount = commentCount,
                    PublishedAt = publishedAt,
                    SearchedAt = DateTime.UtcNow,
                    IsIgnored = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.YouTubeVideoSearchResults.Add(existing);
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        /// <summary>
        /// Get YouTube search results for a game (excluding ignored videos by default)
        /// </summary>
        public async Task<List<YouTubeVideoSearchResult>> GetYouTubeSearchResultsAsync(int gameId, bool includeIgnored = false)
        {
            var query = _context.YouTubeVideoSearchResults
                .Where(y => y.GameId == gameId);

            if (!includeIgnored)
            {
                query = query.Where(y => !y.IsIgnored);
            }

            return await query
                .OrderByDescending(y => y.ViewCount)
                .ThenByDescending(y => y.SearchedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Mark a YouTube search result as ignored or un-ignored
        /// </summary>
        public async Task<bool> SetYouTubeSearchResultIgnoredAsync(int resultId, bool isIgnored)
        {
            var result = await _context.YouTubeVideoSearchResults.FirstOrDefaultAsync(y => y.Id == resultId);
            if (result == null)
                return false;

            result.IsIgnored = isIgnored;
            result.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Delete YouTube search results for a game
        /// </summary>
        public async Task<bool> ClearYouTubeSearchResultsAsync(int gameId)
        {
            var results = await _context.YouTubeVideoSearchResults
                .Where(y => y.GameId == gameId)
                .ToListAsync();

            if (results.Count == 0)
                return true;

            _context.YouTubeVideoSearchResults.RemoveRange(results);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Get the date when YouTube search results were last retrieved for a game
        /// </summary>
        public async Task<DateTime?> GetLastYouTubeSearchDateAsync(int gameId)
        {
            return await _context.YouTubeVideoSearchResults
                .Where(y => y.GameId == gameId)
                .MinAsync(y => (DateTime?)y.SearchedAt);
        }
    }
}
