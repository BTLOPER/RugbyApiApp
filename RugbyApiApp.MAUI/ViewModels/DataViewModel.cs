using System.Windows.Input;
using RugbyApiApp.Data;
using RugbyApiApp.Services;

namespace RugbyApiApp.MAUI.ViewModels
{
    /// <summary>
    /// ViewModel for the Data tab with browsing and fetching capabilities
    /// </summary>
    public class DataViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private RugbyApiClient? _apiClient;

        private string? _selectedDataType;
        private object? _gridData;
        private bool _isLoading;
        private string _statusMessage = "";

        // Separate data collections for each data type
        private List<CountryGridItem> _countriesData = new();
        private List<SeasonGridItem> _seasonsData = new();
        private List<LeagueGridItem> _leaguesData = new();
        private List<TeamGridItem> _teamsData = new();
        private List<GameGridItem> _gamesData = new();

        public List<CountryGridItem> CountriesData
        {
            get => _countriesData;
            set => SetProperty(ref _countriesData, value);
        }

        public List<SeasonGridItem> SeasonsData
        {
            get => _seasonsData;
            set => SetProperty(ref _seasonsData, value);
        }

        public List<LeagueGridItem> LeaguesData
        {
            get => _leaguesData;
            set => SetProperty(ref _leaguesData, value);
        }

        public List<TeamGridItem> TeamsData
        {
            get => _teamsData;
            set => SetProperty(ref _teamsData, value);
        }

        public List<GameGridItem> GamesData
        {
            get => _gamesData;
            set => SetProperty(ref _gamesData, value);
        }

        public string? SelectedDataType
        {
            get => _selectedDataType;
            set
            {
                if (SetProperty(ref _selectedDataType, value))
                {
                    UpdateVisibility();
                    _ = LoadDataByTypeAsync();
                }
            }
        }

        public object? GridData
        {
            get => _gridData;
            set => SetProperty(ref _gridData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand LoadCountriesCommand { get; }
        public ICommand LoadSeasonsCommand { get; }
        public ICommand LoadLeaguesCommand { get; }
        public ICommand LoadTeamsCommand { get; }
        public ICommand LoadGamesCommand { get; }
        public ICommand FetchCountriesCommand { get; }
        public ICommand FetchSeasonsCommand { get; }
        public ICommand FetchLeaguesCommand { get; }
        public ICommand FetchGamesCommand { get; }
        public ICommand ToggleFavoriteCommand { get; }

        private string _searchText = "";
        private bool _showFavoritesOnly;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    _ = FilterDataAsync();
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
                    _ = FilterDataAsync();
                }
            }
        }

        private bool _isCountriesVisible = true;
        private bool _isSeasonsVisible;
        private bool _isLeaguesVisible;
        private bool _isTeamsVisible;
        private bool _isGamesVisible;

        public bool IsCountriesVisible 
        { 
            get => _isCountriesVisible;
            set => SetProperty(ref _isCountriesVisible, value);
        }
        public bool IsSeasonsVisible 
        { 
            get => _isSeasonsVisible;
            set => SetProperty(ref _isSeasonsVisible, value);
        }
        public bool IsLeaguesVisible 
        { 
            get => _isLeaguesVisible;
            set => SetProperty(ref _isLeaguesVisible, value);
        }
        public bool IsTeamsVisible 
        { 
            get => _isTeamsVisible;
            set => SetProperty(ref _isTeamsVisible, value);
        }
        public bool IsGamesVisible 
        { 
            get => _isGamesVisible;
            set => SetProperty(ref _isGamesVisible, value);
        }

        private List<string> _dataTypes = new() { "Countries", "Seasons", "Leagues", "Teams", "Games" };
        public List<string> DataTypes
        {
            get => _dataTypes;
            set => SetProperty(ref _dataTypes, value);
        }

        public DataViewModel(DataService dataService, RugbyApiClient? apiClient = null)
        {
            _dataService = dataService;
            _apiClient = apiClient;

            RefreshCommand = new AsyncRelayCommand(_ => LoadDataByTypeAsync());
            LoadCountriesCommand = new AsyncRelayCommand(_ => LoadCountriesAsync());
            LoadSeasonsCommand = new AsyncRelayCommand(_ => LoadSeasonsAsync());
            LoadLeaguesCommand = new AsyncRelayCommand(_ => LoadLeaguesAsync());
            LoadTeamsCommand = new AsyncRelayCommand(_ => LoadTeamsAsync());
            LoadGamesCommand = new AsyncRelayCommand(_ => LoadGamesAsync());
            FetchCountriesCommand = new AsyncRelayCommand(_ => FetchCountriesAsync());
            FetchSeasonsCommand = new AsyncRelayCommand(_ => FetchSeasonsAsync());
            FetchLeaguesCommand = new AsyncRelayCommand(_ => FetchLeaguesAsync());
            FetchGamesCommand = new AsyncRelayCommand(_ => FetchGamesAsync());
            ToggleFavoriteCommand = new AsyncRelayCommand<int>(id => ToggleFavoriteAsync(id));
        }

        public void SetApiClient(RugbyApiClient? apiClient)
        {
            _apiClient = apiClient;
        }

        private void UpdateVisibility()
        {
            IsCountriesVisible = false;
            IsSeasonsVisible = false;
            IsLeaguesVisible = false;
            IsTeamsVisible = false;
            IsGamesVisible = false;

            switch (SelectedDataType)
            {
                case "Countries":
                    IsCountriesVisible = true;
                    break;
                case "Seasons":
                    IsSeasonsVisible = true;
                    break;
                case "Leagues":
                    IsLeaguesVisible = true;
                    break;
                case "Teams":
                    IsTeamsVisible = true;
                    break;
                case "Games":
                    IsGamesVisible = true;
                    break;
            }

            OnPropertyChanged(nameof(IsCountriesVisible));
            OnPropertyChanged(nameof(IsSeasonsVisible));
            OnPropertyChanged(nameof(IsLeaguesVisible));
            OnPropertyChanged(nameof(IsTeamsVisible));
            OnPropertyChanged(nameof(IsGamesVisible));
        }

        private async Task LoadDataByTypeAsync()
        {
            if (string.IsNullOrEmpty(SelectedDataType))
                return;

            IsLoading = true;
            try
            {
                await (SelectedDataType switch
                {
                    "Countries" => LoadCountriesAsync(),
                    "Seasons" => LoadSeasonsAsync(),
                    "Leagues" => LoadLeaguesAsync(),
                    "Teams" => LoadTeamsAsync(),
                    "Games" => LoadGamesAsync(),
                    _ => Task.CompletedTask
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadCountriesAsync()
        {
            try
            {
                var countries = await _dataService.GetCountriesAsync();
                CountriesData = countries.Select(c => new CountryGridItem
                {
                    Name = c.Name,
                    Code = c.Code,
                    Status = c.IsDataComplete ? "✓ Complete" : "⚠ Incomplete"
                }).ToList();
                StatusMessage = $"Loaded {countries.Count} countries";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        private async Task LoadSeasonsAsync()
        {
            try
            {
                var seasons = await _dataService.GetSeasonsAsync();
                SeasonsData = seasons.Select(s => new SeasonGridItem
                {
                    Year = s.Year,
                    Current = s.IsCurrent ? "Yes" : "No",
                    Status = s.IsDataComplete ? "✓ Complete" : "⚠ Incomplete"
                }).ToList();
                StatusMessage = $"Loaded {seasons.Count} seasons";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        private async Task LoadLeaguesAsync()
        {
            try
            {
                var leagues = await _dataService.GetLeaguesAsync();
                LeaguesData = leagues.Select(l => new LeagueGridItem
                {
                    Id = l.Id,
                    Name = l.Name,
                    Country = l.CountryCode,
                    Type = l.Type,
                    Favorite = l.IsFavorite
                }).ToList();
                StatusMessage = $"Loaded {leagues.Count} leagues";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        private async Task LoadTeamsAsync()
        {
            try
            {
                var teams = await _dataService.GetTeamsAsync();
                TeamsData = teams.Select(t => new TeamGridItem
                {
                    Id = t.Id,
                    Name = t.Name,
                    Code = t.Code,
                    Status = t.IsDataComplete ? "✓ Complete" : "⚠ Incomplete",
                    Favorite = t.IsFavorite
                }).ToList();
                StatusMessage = $"Loaded {teams.Count} teams";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        private async Task LoadGamesAsync()
        {
            try
            {
                var games = await _dataService.GetGamesAsync();
                GamesData = games.Select(g => new GameGridItem
                {
                    Home = g.HomeTeam?.Name ?? "Unknown",
                    Away = g.AwayTeam?.Name ?? "Unknown",
                    Date = g.Date?.ToString("yyyy-MM-dd") ?? "TBD",
                    Venue = g.Venue ?? "TBD"
                }).ToList();
                StatusMessage = $"Loaded {games.Count} games";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        private async Task FetchCountriesAsync()
        {
            if (_apiClient == null)
            {
                StatusMessage = "API client not configured";
                return;
            }

            IsLoading = true;
            try
            {
                StatusMessage = "Fetching countries from API...";
                var (countries, errorMessage) = await _apiClient.GetCountriesAsync();

                if (errorMessage != null)
                {
                    StatusMessage = $"API Error: {errorMessage}";
                    return;
                }

                if (countries != null && countries.Count > 0)
                {
                    int count = 0;
                    foreach (var country in countries)
                    {
                        if (country.Id.HasValue && country.Name != null)
                        {
                            var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(country.Flag);
                            await _dataService.UpsertCountryAsync(country.Id.Value, country.Name, country.Code, cdnFlag);
                            count++;
                        }
                    }

                    StatusMessage = $"✅ Stored {count} countries";
                    await LoadCountriesAsync();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to fetch: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task FetchSeasonsAsync()
        {
            if (_apiClient == null)
            {
                StatusMessage = "API client not configured";
                return;
            }

            IsLoading = true;
            try
            {
                StatusMessage = "Fetching seasons from API...";
                var (seasonYears, errorMessage) = await _apiClient.GetSeasonsAsync();

                if (errorMessage != null)
                {
                    StatusMessage = $"API Error: {errorMessage}";
                    return;
                }

                if (seasonYears != null && seasonYears.Count > 0)
                {
                    int count = 0;
                    foreach (var year in seasonYears)
                    {
                        await _dataService.UpsertSeasonAsync(year, year, null, null, false);
                        count++;
                    }

                    StatusMessage = $"✅ Stored {count} seasons";
                    await LoadSeasonsAsync();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to fetch: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task FetchLeaguesAsync()
        {
            if (_apiClient == null)
            {
                StatusMessage = "API client not configured";
                return;
            }

            IsLoading = true;
            try
            {
                StatusMessage = "Fetching leagues from API...";
                var (leagues, errorMessage) = await _apiClient.GetLeaguesAsync();

                if (errorMessage != null)
                {
                    StatusMessage = $"API Error: {errorMessage}";
                    return;
                }

                if (leagues != null && leagues.Count > 0)
                {
                    int count = 0;
                    foreach (var league in leagues)
                    {
                        if (league.Id.HasValue && league.Name != null)
                        {
                            var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(league.Logo);
                            var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(league.Country?.Flag);
                            await _dataService.UpsertLeagueAsync(league.Id.Value, league.Name, league.Type, cdnLogo, league.Country?.Name, league.Country?.Code, cdnFlag);
                            count++;
                        }
                    }

                    StatusMessage = $"✅ Stored {count} leagues";
                    await LoadLeaguesAsync();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to fetch: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task FetchGamesAsync()
        {
            if (_apiClient == null)
            {
                StatusMessage = "API client not configured";
                return;
            }

            IsLoading = true;
            try
            {
                var leagues = await _dataService.GetLeaguesAsync();
                if (!leagues.Any())
                {
                    StatusMessage = "No leagues in database. Fetch leagues first.";
                    return;
                }

                var seasons = await _dataService.GetSeasonsAsync();
                if (!seasons.Any())
                {
                    StatusMessage = "No seasons in database. Fetch seasons first.";
                    return;
                }

                StatusMessage = "Fetching games from API...";
                var yearList = seasons.Where(s => s.Year.HasValue).OrderByDescending(s => s.Year).Select(s => s.Year!.Value).Distinct().ToList();
                int startYear = yearList.Last();
                int endYear = yearList.First();

                int gamesCount = 0;
                int teamsCount = 0;

                foreach (var league in leagues)
                {
                    var (games, errorMessage) = await _apiClient.GetGamesByLeagueAndSeasonsAsync(league.Id, startYear, endYear);

                    if (errorMessage != null || games == null || games.Count == 0)
                        continue;

                    foreach (var game in games)
                    {
                        if (game.Id.HasValue && game.Home?.Id.HasValue == true && game.Away?.Id.HasValue == true)
                        {
                            if (!await _dataService.TeamExistsAsync(game.Home.Id.Value))
                            {
                                if (game.Home.Name != null)
                                {
                                    var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(game.Home.Logo);
                                    var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(game.Home.Flag);
                                    await _dataService.UpsertTeamAsync(game.Home.Id.Value, game.Home.Name, game.Home.Code, cdnFlag, cdnLogo);
                                    teamsCount++;
                                }
                            }

                            if (!await _dataService.TeamExistsAsync(game.Away.Id.Value))
                            {
                                if (game.Away.Name != null)
                                {
                                    var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(game.Away.Logo);
                                    var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(game.Away.Flag);
                                    await _dataService.UpsertTeamAsync(game.Away.Id.Value, game.Away.Name, game.Away.Code, cdnFlag, cdnLogo);
                                    teamsCount++;
                                }
                            }

                            await _dataService.UpsertGameAsync(game.Id.Value, game.LeagueId, game.Season, game.Home.Id.Value, game.Away.Id.Value, game.Date, game.Status?.Short, game.Venue, game.Scores?.Home, game.Scores?.Away);
                            gamesCount++;
                        }
                    }
                }

                StatusMessage = $"✅ Stored {gamesCount} games and {teamsCount} teams";
                await LoadGamesAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to fetch: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ToggleFavoriteAsync(int id)
        {
            try
            {
                if (SelectedDataType == "Teams")
                {
                    await _dataService.ToggleTeamFavoriteAsync(id);
                    
                    var item = TeamsData.FirstOrDefault(t => t.Id == id);
                    if (item != null)
                    {
                        item.Favorite = !item.Favorite;
                        
                        if (ShowFavoritesOnly && !item.Favorite)
                        {
                            TeamsData.Remove(item);
                        }
                    }
                    
                    StatusMessage = "✅ Team favorite toggled";
                }
                else if (SelectedDataType == "Leagues")
                {
                    await _dataService.ToggleLeagueFavoriteAsync(id);
                    
                    var item = LeaguesData.FirstOrDefault(l => l.Id == id);
                    if (item != null)
                    {
                        item.Favorite = !item.Favorite;
                        
                        if (ShowFavoritesOnly && !item.Favorite)
                        {
                            LeaguesData.Remove(item);
                        }
                    }
                    
                    StatusMessage = "✅ League favorite toggled";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Error: {ex.Message}";
            }
        }

        private async Task FilterDataAsync()
        {
            if (string.IsNullOrEmpty(SelectedDataType))
                return;

            IsLoading = true;
            try
            {
                if (SelectedDataType == "Countries")
                {
                    var countries = await _dataService.GetCountriesAsync();
                    CountriesData = countries
                        .Where(c => c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || c.Code.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        .Select(c => new CountryGridItem
                        {
                            Name = c.Name,
                            Code = c.Code,
                            Status = c.IsDataComplete ? "✓ Complete" : "⚠ Incomplete"
                        }).ToList();
                }
                else if (SelectedDataType == "Seasons")
                {
                    var seasons = await _dataService.GetSeasonsAsync();
                    SeasonsData = seasons
                        .Where(s => s.Year.ToString().Contains(SearchText))
                        .Select(s => new SeasonGridItem
                        {
                            Year = s.Year,
                            Current = s.IsCurrent ? "Yes" : "No",
                            Status = s.IsDataComplete ? "✓ Complete" : "⚠ Incomplete"
                        }).ToList();
                }
                else if (SelectedDataType == "Leagues")
                {
                    var leagues = await _dataService.GetLeaguesAsync();
                    LeaguesData = leagues
                        .Where(l => (l.Name?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) || 
                                    (l.CountryCode?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
                        .Where(l => !ShowFavoritesOnly || l.IsFavorite)
                        .Select(g => new LeagueGridItem
                        {
                            Id = g.Id,
                            Name = g.Name,
                            Country = g.CountryCode,
                            Type = g.Type,
                            Favorite = g.IsFavorite
                        }).ToList();
                }
                else if (SelectedDataType == "Teams")
                {
                    var teams = await _dataService.GetTeamsAsync();
                    TeamsData = teams
                        .Where(t => (t.Name?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) || 
                                    (t.Code?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
                        .Where(t => !ShowFavoritesOnly || t.IsFavorite)
                        .Select(t => new TeamGridItem
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Code = t.Code,
                            Status = t.IsDataComplete ? "✓ Complete" : "⚠ Incomplete",
                            Favorite = t.IsFavorite
                        }).ToList();
                }
                else if (SelectedDataType == "Games")
                {
                    var games = await _dataService.GetGamesAsync();
                    GamesData = games
                        .Where(g => (g.HomeTeam?.Name ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                    (g.AwayTeam?.Name ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                    (g.Venue ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        .Select(g => new GameGridItem
                        {
                            Home = g.HomeTeam?.Name ?? "Unknown",
                            Away = g.AwayTeam?.Name ?? "Unknown",
                            Date = g.Date?.ToString("yyyy-MM-dd") ?? "TBD",
                            Venue = g.Venue ?? "TBD"
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error filtering data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    /// <summary>
    /// Grid item for Countries
    /// </summary>
    public class CountryGridItem
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Status { get; set; }
    }

    /// <summary>
    /// Grid item for Seasons
    /// </summary>
    public class SeasonGridItem
    {
        public int? Year { get; set; }
        public string? Current { get; set; }
        public string? Status { get; set; }
    }

    /// <summary>
    /// Grid item for Games
    /// </summary>
    public class GameGridItem
    {
        public string? Home { get; set; }
        public string? Away { get; set; }
        public string? Date { get; set; }
        public string? Venue { get; set; }
    }
}
