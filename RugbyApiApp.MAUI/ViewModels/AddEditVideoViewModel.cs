using RugbyApiApp.Data;
using RugbyApiApp.Models;
using RugbyApiApp.Services;

namespace RugbyApiApp.MAUI.ViewModels
{
    /// <summary>
    /// ViewModel for adding/editing videos in the Watch tab
    /// </summary>
    public class AddEditVideoViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private readonly int _gameId;
        private readonly int? _videoId;
        private Video? _currentVideo;

        public AddEditVideoViewModel(int gameId, int? videoId = null)
        {
            _gameId = gameId;
            _videoId = videoId;
            _dataService = new DataService(new RugbyDbContext());
        }

        public async Task LoadVideoAsync(int videoId)
        {
            try
            {
                _currentVideo = await _dataService.GetVideoAsync(videoId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading video: {ex.Message}");
            }
        }

        public async Task<bool> SaveVideoAsync(
            string title,
            string url,
            string description,
            string lengthString,
            string dateString,
            string ratingString,
            bool isWatched,
            bool isFavorite)
        {
            try
            {
                // Parse optional fields
                int? lengthSeconds = null;
                if (!string.IsNullOrWhiteSpace(lengthString) && int.TryParse(lengthString, out var length))
                {
                    lengthSeconds = length;
                }

                DateTime? videoDate = null;
                if (!string.IsNullOrWhiteSpace(dateString) && DateTime.TryParse(dateString, out var date))
                {
                    videoDate = date;
                }

                int? rating = null;
                if (!string.IsNullOrWhiteSpace(ratingString) && int.TryParse(ratingString, out var rate))
                {
                    rating = Math.Clamp(rate, 1, 10);
                }

                if (_videoId.HasValue)
                {
                    // Update existing video
                    if (_currentVideo == null)
                    {
                        _currentVideo = await _dataService.GetVideoAsync(_videoId.Value);
                    }

                    if (_currentVideo != null)
                    {
                        _currentVideo.Title = title;
                        _currentVideo.Url = url;
                        _currentVideo.Description = description;
                        _currentVideo.LengthSeconds = lengthSeconds;
                        _currentVideo.Date = videoDate;
                        _currentVideo.Rating = rating;
                        _currentVideo.Watched = isWatched;
                        _currentVideo.IsFavorite = isFavorite;

                        await _dataService.UpdateVideoAsync(_currentVideo);
                    }
                }
                else
                {
                    // Create new video
                    var newVideo = new Video
                    {
                        GameId = _gameId,
                        Title = title,
                        Url = url,
                        Description = description,
                        LengthSeconds = lengthSeconds,
                        Date = videoDate ?? DateTime.Now,
                        Rating = rating,
                        Watched = isWatched,
                        IsFavorite = isFavorite
                    };

                    await _dataService.AddVideoAsync(newVideo);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving video: {ex.Message}");
                return false;
            }
        }
    }
}
