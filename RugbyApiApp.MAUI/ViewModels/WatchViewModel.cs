using System.Windows.Input;
using RugbyApiApp.Data;
using RugbyApiApp.Models;
using RugbyApiApp.Services;
using RugbyApiApp.MAUI.Services;
using RugbyApiApp.MAUI.Views;
using RugbyApiApp.YouTubeApi;
using Microsoft.Extensions.Configuration;

namespace RugbyApiApp.MAUI.ViewModels
{
    /// <summary>
    /// ViewModel for the Watch tab - manages game filtering and video viewing
    /// </summary>
    public class WatchViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private readonly IVideoWindowService _videoWindowService;
        private readonly YoutubeVideoService? _youtubeService;
        private List<int> _pageSizeOptions = new() { 10, 20, 50 };
        
        private List<Game> _allGames = new();
        private List<GameVideoItem> _filteredGames = new();
        private List<League> _allLeagues = new();
        private List<League> _filteredLeagues = new();
        private List<int> _seasons = new();
        private List<Team> _allTeams = new();
        private List<Team> _filteredTeams = new();
        private List<int> _pageNumbers = new();
        
        private int? _selectedLeagueId;
        private int? _selectedSeasonId;
        private int? _selectedTeamId;
        private bool _showFavoritesOnly;
        private GameVideoItem? _selectedGame;
        private List<VideoItem> _selectedGameVideos = new();
        private List<YoutubeSearchResult> _youtubeSearchResults = new();
        private bool _isSearching;
        private string _youtubeStatusMessage = "";
        private CancellationTokenSource? _searchCancellationTokenSource;
        
        private int _pageSize = 20;
        private int _currentPage = 1;
        private int _totalGames = 0;
        private int _totalPages = 0;
        private string _statusMessage = "";

        // YouTube Search Filters
        private string _youTubeMinDuration = "";
        private string _youTubeMaxDuration = "";
        private string _youTubeSelectedDefinition = "any";
        private string _youTubeSelectedLeague = "any";

        public List<int> PageSizeOptions
        {
            get => _pageSizeOptions;
            set => SetProperty(ref _pageSizeOptions, value);
        }

        public List<int> PageNumbers
        {
            get => _pageNumbers;
            set => SetProperty(ref _pageNumbers, value);
        }

        public List<GameVideoItem> FilteredGames
        {
            get => _filteredGames;
            set => SetProperty(ref _filteredGames, value);
        }

        public List<League> Leagues
        {
            get => _filteredLeagues;
            set => SetProperty(ref _filteredLeagues, value);
        }

        public List<int> Seasons
        {
            get => _seasons;
            set => SetProperty(ref _seasons, value);
        }

        public List<Team> Teams
        {
            get => _filteredTeams;
            set => SetProperty(ref _filteredTeams, value);
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (SetProperty(ref _pageSize, value))
                {
                    _currentPage = 1;
                    ApplyFiltersAsync().ConfigureAwait(false);
                }
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (value >= 1 && value <= _totalPages)
                {
                    if (SetProperty(ref _currentPage, value))
                    {
                        LoadPageAsync(_currentPage).ConfigureAwait(false);
                    }
                }
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            set => SetProperty(ref _totalPages, value);
        }

        public int TotalGames
        {
            get => _totalGames;
            set => SetProperty(ref _totalGames, value);
        }

        public int? SelectedLeagueId
        {
            get => _selectedLeagueId;
            set
            {
                if (SetProperty(ref _selectedLeagueId, value))
                {
                    _currentPage = 1;
                    ApplyFiltersAsync().ConfigureAwait(false);
                }
            }
        }

        public int? SelectedSeasonId
        {
            get => _selectedSeasonId;
            set
            {
                if (SetProperty(ref _selectedSeasonId, value))
                {
                    _currentPage = 1;
                    ApplyFiltersAsync().ConfigureAwait(false);
                }
            }
        }

        public int? SelectedTeamId
        {
            get => _selectedTeamId;
            set
            {
                if (SetProperty(ref _selectedTeamId, value))
                {
                    _currentPage = 1;
                    ApplyFiltersAsync().ConfigureAwait(false);
                }
            }
        }

        public bool ShowFavoritesOnly
        {
            get => _showFavoritesOnly;
            set
            {
                if (SetProperty(ref _showFavoritesOnly, value))
                {
                    _currentPage = 1;
                    UpdateFilteredCollections();
                    ApplyFiltersAsync().ConfigureAwait(false);
                }
            }
        }

        public GameVideoItem? SelectedGame
        {
            get => _selectedGame;
            set
            {
                if (SetProperty(ref _selectedGame, value))
                {
                    if (value != null)
                    {
                        LoadGameVideosAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        public List<VideoItem> SelectedGameVideos
        {
            get => _selectedGameVideos;
            set => SetProperty(ref _selectedGameVideos, value);
        }

        public List<YoutubeSearchResult> YouTubeSearchResults
        {
            get => _youtubeSearchResults;
            set => SetProperty(ref _youtubeSearchResults, value);
        }

        public bool IsSearching
        {
            get => _isSearching;
            set => SetProperty(ref _isSearching, value);
        }

        public string YouTubeStatusMessage
        {
            get => _youtubeStatusMessage;
            set => SetProperty(ref _youtubeStatusMessage, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        // YouTube Search Filters
        public string YouTubeMinDuration
        {
            get => _youTubeMinDuration;
            set => SetProperty(ref _youTubeMinDuration, value);
        }

        public string YouTubeMaxDuration
        {
            get => _youTubeMaxDuration;
            set => SetProperty(ref _youTubeMaxDuration, value);
        }

        public string YouTubeSelectedDefinition
        {
            get => _youTubeSelectedDefinition;
            set => SetProperty(ref _youTubeSelectedDefinition, value);
        }

        public string YouTubeSelectedLeague
        {
            get => _youTubeSelectedLeague;
            set => SetProperty(ref _youTubeSelectedLeague, value);
        }

        public List<string> YouTubeDefinitionOptions { get; } = new()
        {
            "any",
            "standard",
            "high"
        };

        public List<string> YouTubeLeagueOptions
        {
            get => _filteredLeagues.Select(l => l.Name).Prepend("any").Distinct().ToList();
        }

        public ICommand LoadDataCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }
        public ICommand AddVideoCommand { get; }
        public ICommand EditVideoCommand { get; }
        public ICommand DeleteVideoCommand { get; }
        public ICommand ToggleFavoriteCommand { get; }
        public ICommand ToggleWatchedCommand { get; }
        public ICommand SetRatingCommand { get; }
        public ICommand SearchYouTubeCommand { get; }
        public ICommand CancelSearchCommand { get; }
        public ICommand AddYouTubeVideoCommand { get; }

        public WatchViewModel(DataService dataService)
            : this(dataService, new VideoWindowService(), null as YoutubeVideoService)
        {
        }

        public WatchViewModel(DataService dataService, IVideoWindowService videoWindowService, YoutubeVideoService? youtubeService)
        {
            _dataService = dataService;
            _videoWindowService = videoWindowService;
            _youtubeService = youtubeService;
            
            LoadDataCommand = new AsyncRelayCommand(async _ => await LoadInitialDataAsync());
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());
            FirstPageCommand = new RelayCommand(_ => CurrentPage = 1, _ => _currentPage > 1);
            PreviousPageCommand = new RelayCommand(_ => CurrentPage = _currentPage - 1, _ => _currentPage > 1);
            NextPageCommand = new RelayCommand(_ => CurrentPage = _currentPage + 1, _ => _currentPage < _totalPages);
            LastPageCommand = new RelayCommand(_ => CurrentPage = _totalPages, _ => _currentPage < _totalPages);
            AddVideoCommand = new RelayCommand(_ => OnAddVideo(), _ => SelectedGame != null);
            EditVideoCommand = new RelayCommand(_ => OnEditVideo(), _ => SelectedGameVideos?.Count > 0);
            DeleteVideoCommand = new RelayCommand(_ => OnDeleteVideo(), _ => SelectedGameVideos?.Count > 0);
            ToggleFavoriteCommand = new RelayCommand<GameVideoItem>(async gameVideo => await ToggleFavoriteAsync(gameVideo), gameVideo => gameVideo != null);
            ToggleWatchedCommand = new RelayCommand<VideoItem>(async video => await ToggleWatchedAsync(video), video => video != null);
            SetRatingCommand = new RelayCommand<int>(async rating => await SetRatingAsync(rating), rating => SelectedGame != null);
            
            // SearchYouTubeCommand - Always enabled for execution, checks conditions in the method
            SearchYouTubeCommand = new AsyncRelayCommand(async _ => await SearchYouTubeAsync());
            
            CancelSearchCommand = new RelayCommand(_ => CancelSearch(), _ => IsSearching);
            AddYouTubeVideoCommand = new AsyncRelayCommand<YoutubeSearchResult>(async result => await AddYouTubeVideoAsync(result), result => result != null && SelectedGame != null);
        }

        public async Task LoadInitialDataAsync()
        {
            try
            {
                StatusMessage = "Loading data...";

                // Load all games
                _allGames = await _dataService.GetGamesAsync();

                // Load leagues
                _allLeagues = await _dataService.GetLeaguesAsync();

                // Load teams
                _allTeams = await _dataService.GetTeamsAsync();

                // Extract unique seasons from games
                var seasonsList = _allGames
                    .Where(g => g.Season.HasValue)
                    .Select(g => g.Season!.Value)
                    .Distinct()
                    .OrderByDescending(s => s)
                    .ToList();
                Seasons = seasonsList;

                // Update filtered collections
                UpdateFilteredCollections();

                // Reset pagination
                _currentPage = 1;

                // Apply initial filters
                await ApplyFiltersAsync();
                
                if (_allGames.Count == 0)
                {
                    StatusMessage = "No games found in database. Load data from the Data tab first.";
                }
                else
                {
                    UpdateStatusMessage();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading data: {ex.Message}";
            }
        }

        private void UpdateFilteredCollections()
        {
            // Filter leagues based on favorites setting
            if (ShowFavoritesOnly)
            {
                Leagues = _allLeagues.Where(l => l.IsFavorite).ToList();
                Teams = _allTeams.Where(t => t.IsFavorite).ToList();
            }
            else
            {
                Leagues = new List<League>(_allLeagues);
                Teams = new List<Team>(_allTeams);
            }
        }

        private async Task ApplyFiltersAsync()
        {
            try
            {
                var filtered = _allGames.AsEnumerable();

                // Apply league filter
                if (SelectedLeagueId.HasValue)
                {
                    filtered = filtered.Where(g => g.LeagueId == SelectedLeagueId);
                }

                // Apply season filter
                if (SelectedSeasonId.HasValue)
                {
                    filtered = filtered.Where(g => g.Season == SelectedSeasonId);
                }

                // Apply team filter (check if team is home or away)
                if (SelectedTeamId.HasValue)
                {
                    filtered = filtered.Where(g => g.HomeTeamId == SelectedTeamId || g.AwayTeamId == SelectedTeamId);
                }

                // Order by date descending
                var orderedGames = filtered.OrderByDescending(g => g.Date).ToList();
                TotalGames = orderedGames.Count;
                TotalPages = (int)Math.Ceiling((double)TotalGames / PageSize);

                // Generate page numbers
                PageNumbers = Enumerable.Range(1, TotalPages).ToList();

                // Load first page
                await LoadPageAsync(1);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error applying filters: {ex.Message}";
            }
        }

        private async Task LoadPageAsync(int pageNumber)
        {
            try
            {
                if (pageNumber < 1 || pageNumber > TotalPages)
                    return;

                _currentPage = pageNumber;

                var filtered = _allGames.AsEnumerable();

                // Apply filters
                if (SelectedLeagueId.HasValue)
                    filtered = filtered.Where(g => g.LeagueId == SelectedLeagueId);

                if (SelectedSeasonId.HasValue)
                    filtered = filtered.Where(g => g.Season == SelectedSeasonId);

                if (SelectedTeamId.HasValue)
                    filtered = filtered.Where(g => g.HomeTeamId == SelectedTeamId || g.AwayTeamId == SelectedTeamId);

                var orderedGames = filtered.OrderByDescending(g => g.Date).ToList();
                var pageGames = orderedGames.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

                // Convert to GameVideoItem with video counts
                var gameVideoItems = new List<GameVideoItem>();
                foreach (var game in pageGames)
                {
                    var videos = await _dataService.GetVideosByGameIdAsync(game.Id);
                    var watchedCount = videos.Count(v => v.Watched);
                    
                    gameVideoItems.Add(new GameVideoItem
                    {
                        GameId = game.Id,
                        LeagueName = (await _dataService.GetLeagueAsync(game.LeagueId ?? 0))?.Name ?? "Unknown",
                        HomeTeamName = game.HomeTeam?.Name ?? "Unknown",
                        AwayTeamName = game.AwayTeam?.Name ?? "Unknown",
                        Date = game.Date,
                        TotalVideos = videos.Count,
                        WatchedVideos = watchedCount,
                        IsGameFavorite = false
                    });
                }

                OnPropertyChanged(nameof(CurrentPage));
                FilteredGames = gameVideoItems;
                UpdateStatusMessage();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading page: {ex.Message}";
            }
        }

        private void UpdateStatusMessage()
        {
            int startRecord = (CurrentPage - 1) * PageSize + 1;
            int endRecord = Math.Min(CurrentPage * PageSize, TotalGames);
            StatusMessage = $"Showing records {startRecord}-{endRecord} of {TotalGames} | Page {CurrentPage} of {TotalPages}";
        }

        private async Task LoadGameVideosAsync()
        {
            if (SelectedGame == null)
            {
                SelectedGameVideos = new();
                return;
            }

            try
            {
                var videos = await _dataService.GetVideosByGameIdAsync(SelectedGame.GameId);
                SelectedGameVideos = videos
                    .Select(v => new VideoItem
                    {
                        Id = v.Id,
                        Title = v.Title ?? "Untitled",
                        Url = v.Url ?? "",
                        Description = v.Description ?? "",
                        LengthSeconds = v.LengthSeconds,
                        Date = v.Date,
                        IsFavorite = v.IsFavorite,
                        Rating = v.Rating,
                        IsWatched = v.Watched
                    })
                    .OrderByDescending(v => v.Date)
                    .ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading videos: {ex.Message}";
            }
        }

        private async Task SearchYouTubeAsync()
        {
            // Check preconditions
            if (_youtubeService is null)
            {
                YouTubeStatusMessage = "❌ YouTube API key not configured. Please configure it in Settings.";
                return;
            }

            if (SelectedGame == null)
            {
                YouTubeStatusMessage = "⚠️ No game selected. Please select a game first.";
                return;
            }

            if (IsSearching)
            {
                YouTubeStatusMessage = "⏳ Search already in progress...";
                return;
            }

            try
            {
                IsSearching = true;
                YouTubeStatusMessage = "🔍 Searching YouTube...";
                YouTubeSearchResults = new();

                // Create a new cancellation token for this search
                _searchCancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = _searchCancellationTokenSource.Token;

                var season = SelectedSeasonId ?? DateTime.Now.Year;

                // Parse duration filters
                int? minDurationSeconds = null;
                int? maxDurationSeconds = null;

                if (!string.IsNullOrEmpty(YouTubeMinDuration) && int.TryParse(YouTubeMinDuration, out int minDuration))
                {
                    minDurationSeconds = minDuration * 60; // Convert minutes to seconds
                }

                if (!string.IsNullOrEmpty(YouTubeMaxDuration) && int.TryParse(YouTubeMaxDuration, out int maxDuration))
                {
                    maxDurationSeconds = maxDuration * 60; // Convert minutes to seconds
                }

                var results = await _youtubeService.SearchVideoAsync(
                    SelectedGame.HomeTeamName ?? "Unknown",
                    SelectedGame.AwayTeamName ?? "Unknown",
                    season,
                    maxResults: 25,
                    definition: YouTubeSelectedDefinition,
                    minDurationSeconds: minDurationSeconds,
                    maxDurationSeconds: maxDurationSeconds
                );

                // Check if cancelled during the search
                if (cancellationToken.IsCancellationRequested)
                {
                    YouTubeStatusMessage = "🚫 Search cancelled.";
                    YouTubeSearchResults = new();
                    return;
                }

                YouTubeSearchResults = results;
                if (results.Count == 0)
                {
                    YouTubeStatusMessage = "📭 No videos found. Try different filters or check the game name.";
                }
                else
                {
                    YouTubeStatusMessage = $"✅ Found {results.Count} videos. Click 'Add' to add them to the database.";
                }
            }
            catch (OperationCanceledException)
            {
                YouTubeStatusMessage = "🚫 Search was cancelled.";
                YouTubeSearchResults = new();
            }
            catch (Exception ex)
            {
                YouTubeStatusMessage = $"❌ Error searching YouTube: {ex.Message}";
            }
            finally
            {
                IsSearching = false;
                _searchCancellationTokenSource?.Dispose();
                _searchCancellationTokenSource = null;
            }
        }

        private void CancelSearch()
        {
            if (_searchCancellationTokenSource != null)
            {
                _searchCancellationTokenSource.Cancel();
                YouTubeStatusMessage = "Cancelling search...";
            }
        }

        private async Task AddYouTubeVideoAsync(YoutubeSearchResult? result)
        {
            if (result is null || SelectedGame == null)
            {
                YouTubeStatusMessage = "No video selected or game selected.";
                return;
            }

            try
            {
                var video = new Video
                {
                    GameId = SelectedGame.GameId,
                    Title = result.Title,
                    Url = result.VideoUrl,
                    Description = result.Description,
                    Date = result.PublishedAt,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _dataService.AddVideoAsync(video);
                YouTubeStatusMessage = $"✓ Added '{result.Title}' to the database";
                
                // Refresh the videos list for the current game
                if (SelectedGame != null)
                {
                    var currentGame = SelectedGame;
                    SelectedGame = null;
                    SelectedGame = currentGame;
                }
            }
            catch (Exception ex)
            {
                YouTubeStatusMessage = $"Error adding video: {ex.Message}";
            }
        }

        private void ClearFilters()
        {
            SelectedLeagueId = null;
            SelectedSeasonId = null;
            SelectedTeamId = null;
            ShowFavoritesOnly = false;
            SelectedGame = null;
            _currentPage = 1;
            PageSize = 20;
            YouTubeSearchResults = new();
            YouTubeStatusMessage = "";
            UpdateFilteredCollections();
        }

        private void OnAddVideo()
        {
            if (SelectedGame == null)
                return;

            var result = _videoWindowService.ShowAddEditVideoDialog(
                SelectedGame.GameId,
                SelectedGame.HomeTeamName,
                SelectedGame.AwayTeamName,
                SelectedGame.Date ?? DateTime.Now
            );

            if (result == true)
            {
                // Refresh the videos list
                var currentGame = SelectedGame;
                SelectedGame = null;
                SelectedGame = currentGame;
            }
        }

        private void OnEditVideo()
        {
            // Handle edit video - would require selecting a video from the grid
        }

        private async void OnDeleteVideo()
        {
            if (SelectedGameVideos == null || SelectedGameVideos.Count == 0)
            {
                StatusMessage = "Please select a video to delete.";
                return;
            }

            // Get the first selected video (for DataGrid single selection)
            var videoToDelete = SelectedGameVideos.FirstOrDefault();
            if (videoToDelete == null)
                return;

            try
            {
                // Delete the video from the database
                var success = await _dataService.DeleteVideoByIdAsync(videoToDelete.Id);
                
                if (success)
                {
                    StatusMessage = $"Video '{videoToDelete.Title}' deleted successfully.";
                    // Refresh the videos list
                    if (SelectedGame != null)
                    {
                        var currentGame = SelectedGame;
                        SelectedGame = null;
                        SelectedGame = currentGame;
                    }
                }
                else
                {
                    StatusMessage = "Failed to delete video.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error deleting video: {ex.Message}";
            }
        }

        private async Task ToggleFavoriteAsync(GameVideoItem gameVideo)
        {
            if (gameVideo == null) return;

            try
            {
                gameVideo.IsGameFavorite = !gameVideo.IsGameFavorite;
                // Update the favorite status in the data service
                await _dataService.SetGameFavoriteAsync(gameVideo.GameId, gameVideo.IsGameFavorite);

                // Optionally, refresh the games list or current game details
                await LoadInitialDataAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating favorite: {ex.Message}";
            }
        }

        private async Task ToggleWatchedAsync(VideoItem video)
        {
            if (video == null) return;

            try
            {
                video.IsWatched = !video.IsWatched;
                // Update the watched status in the data service
                await _dataService.SetVideoWatchedAsync(video.Id, video.IsWatched);

                // Optionally, refresh the videos list
                await LoadGameVideosAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating watched status: {ex.Message}";
            }
        }

        private async Task SetRatingAsync(int rating)
        {
            if (SelectedGameVideos == null || SelectedGameVideos.Count == 0)
                return;

            try
            {
                // Since the DataGrid doesn't support selecting a specific video for rating,
                // we'll need to track which video is being rated
                // For now, we'll update all videos - this needs to be improved with proper selection
                StatusMessage = $"Rating set to {rating} stars";
                await LoadGameVideosAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error setting rating: {ex.Message}";
            }
        }

        public async Task UpdateVideoRatingAsync(int videoId, int rating)
        {
            try
            {
                // Update the video rating in the database
                await _dataService.SetVideoRatingAsync(videoId, rating);
                
                // Force re-bind by clearing and re-loading the videos list
                var tempGame = SelectedGame;
                SelectedGame = null;  // Clears the list
                SelectedGame = tempGame;  // Re-loads the list with fresh data
                
                StatusMessage = $"✅ Rating updated to {rating} stars";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error setting rating: {ex.Message}";
            }
        }
    }

    /// <summary>
    /// Represents a game with video information
    /// </summary>
    public class GameVideoItem
    {
        public int GameId { get; set; }
        public string? LeagueName { get; set; }
        public string? HomeTeamName { get; set; }
        public string? AwayTeamName { get; set; }
        public DateTime? Date { get; set; }
        public int TotalVideos { get; set; }
        public int WatchedVideos { get; set; }
        public bool IsGameFavorite { get; set; }

        public string DisplayText => $"{LeagueName} - {HomeTeamName} vs {AwayTeamName} ({Date:yyyy-MM-dd})";
        public string VideoStatus => $"{WatchedVideos}/{TotalVideos} watched";
    }

    /// <summary>
    /// Represents a video for display
    /// </summary>
    public class VideoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public int? LengthSeconds { get; set; }
        public DateTime? Date { get; set; }
        public bool IsFavorite { get; set; }
        public int? Rating { get; set; }
        public bool IsWatched { get; set; }
    }
}
