using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using RugbyApiApp.Data;
using RugbyApiApp.Services;

namespace RugbyApiApp.MAUI.ViewModels
{
    /// <summary>
    /// ViewModel for the Settings tab with API key and database management
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private readonly SecretsService _secretsService;
        private readonly IConfiguration _configuration;
        private RugbyApiClient? _apiClient;

        private string _apiKey = "";
        private string _statusMessage = "";
        private bool _isLoading;
        private string _databasePath = "";
        private string _storageInfo = "";

        public string ApiKey
        {
            get => _apiKey;
            set => SetProperty(ref _apiKey, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string DatabasePath
        {
            get => _databasePath;
            set => SetProperty(ref _databasePath, value);
        }

        public string StorageInfo
        {
            get => _storageInfo;
            set => SetProperty(ref _storageInfo, value);
        }

        public ICommand SaveApiKeyCommand { get; }
        public ICommand ClearApiKeyCommand { get; }
        public ICommand TestApiKeyCommand { get; }
        public ICommand AutoFetchAllDataCommand { get; }
        public ICommand ShowDatabasePathCommand { get; }
        public ICommand ClearAllDataCommand { get; }

        public SettingsViewModel(DataService dataService, SecretsService secretsService, IConfiguration configuration)
        {
            _dataService = dataService;
            _secretsService = secretsService;
            _configuration = configuration;

            SaveApiKeyCommand = new AsyncRelayCommand(_ => SaveApiKeyAsync());
            ClearApiKeyCommand = new AsyncRelayCommand(_ => ClearApiKeyAsync());
            TestApiKeyCommand = new AsyncRelayCommand(_ => TestApiKeyAsync());
            AutoFetchAllDataCommand = new AsyncRelayCommand(_ => AutoFetchAllDataAsync());
            ShowDatabasePathCommand = new AsyncRelayCommand(_ => ShowDatabasePathAsync());
            ClearAllDataCommand = new AsyncRelayCommand(_ => ClearAllDataAsync());

            UpdateStorageInfo();
        }

        public void SetApiClient(RugbyApiClient? apiClient)
        {
            _apiClient = apiClient;
        }

        private async Task SaveApiKeyAsync()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                StatusMessage = "Please enter an API key";
                return;
            }

            try
            {
                _secretsService.SetApiKey(ApiKey);
                _apiClient = new RugbyApiClient(ApiKey);
                StatusMessage = $"✅ API key saved successfully\n{_secretsService.GetStorageInfo()}";
                UpdateStorageInfo();
                ApiKey = "";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to save API key: {ex.Message}";
            }
        }

        private async Task ClearApiKeyAsync()
        {
            try
            {
                _secretsService.ClearApiKey();
                _apiClient = null;
                StatusMessage = $"✅ API key cleared\nNote: To remove from User Secrets, run:\ndotnet user-secrets remove RugbyApiKey\n\n{_secretsService.GetStorageInfo()}";
                UpdateStorageInfo();
                ApiKey = "";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to clear API key: {ex.Message}";
            }
        }

        private async Task TestApiKeyAsync()
        {
            if (_apiClient == null)
            {
                StatusMessage = "Please save an API key first";
                return;
            }

            IsLoading = true;
            try
            {
                StatusMessage = "Testing API connection...";
                var (status, errorMessage) = await _apiClient.GetStatusAsync();

                if (errorMessage != null)
                {
                    StatusMessage = $"API Error: {errorMessage}";
                    return;
                }

                if (status?.Response != null)
                {
                    var accountName = $"{status.Response.Account?.FirstName} {status.Response.Account?.LastName}".Trim();
                    var plan = status.Response.Subscription?.Plan ?? "Unknown";
                    var current = status.Response.Requests?.Current ?? 0;
                    var limit = status.Response.Requests?.LimitDay ?? 0;
                    var remaining = limit - current;
                    var percentUsed = limit > 0 ? (current * 100.0 / limit) : 0;

                    StatusMessage = $@"✅ API Connection Successful

Account: {accountName}
Email: {status.Response.Account?.Email}
Plan: {plan}

📊 Daily API Requests:
Current: {current:N0} / {limit:N0}
Remaining: {remaining:N0}
Usage: {percentUsed:F1}%";
                }
                else
                {
                    StatusMessage = "No status information returned from API";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to test API key: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AutoFetchAllDataAsync()
        {
            if (_apiClient == null)
            {
                StatusMessage = "Please save an API key first";
                return;
            }

            IsLoading = true;
            try
            {
                StatusMessage = "🔄 Auto-fetching all data...\nThis may take several minutes.";
                await AutoFetchAllIncompleteAsync();
                StatusMessage = "✅ Auto-fetch completed successfully!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Auto-fetch failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AutoFetchAllIncompleteAsync()
        {
            // Fetch Countries
            StatusMessage = "📍 Fetching Countries...";
            await FetchAndStoreCountriesAsync();

            // Fetch Seasons
            StatusMessage = "📅 Fetching Seasons...";
            await FetchAndStoreSeasonAsync();

            // Fetch Leagues
            StatusMessage = "🏆 Fetching Leagues...";
            await FetchAndStoreLeaguesAsync();

            // Fetch Games and Teams
            StatusMessage = "🎮 Fetching Games and Teams...\nThis may take a few minutes.";
            await FetchAllGamesAndTeamsAsync();
        }

        private async Task FetchAndStoreCountriesAsync()
        {
            var incompleteCountries = await _dataService.GetIncompleteCountriesAsync();
            if (incompleteCountries.Count > 0)
                return;

            var (countries, errorMessage) = await _apiClient!.GetCountriesAsync();
            if (errorMessage != null)
                throw new Exception($"API Error: {errorMessage}");

            if (countries != null && countries.Count > 0)
            {
                foreach (var country in countries)
                {
                    if (country.Id.HasValue && country.Name != null)
                    {
                        var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(country.Flag);
                        await _dataService.UpsertCountryAsync(country.Id.Value, country.Name, country.Code, cdnFlag);
                    }
                }
            }
        }

        private async Task FetchAndStoreSeasonAsync()
        {
            var incompleteSeasons = await _dataService.GetIncompleteSeasonAsync();
            if (incompleteSeasons.Count > 0)
                return;

            var (seasonYears, errorMessage) = await _apiClient!.GetSeasonsAsync();
            if (errorMessage != null)
                throw new Exception($"API Error: {errorMessage}");

            if (seasonYears != null && seasonYears.Count > 0)
            {
                foreach (var year in seasonYears)
                {
                    await _dataService.UpsertSeasonAsync(year, year, null, null, false);
                }
            }
        }

        private async Task FetchAndStoreLeaguesAsync()
        {
            var incompleteLeagues = await _dataService.GetIncompleteLeaguesAsync();
            if (incompleteLeagues.Count > 0)
                return;

            var (leagues, errorMessage) = await _apiClient!.GetLeaguesAsync();
            if (errorMessage != null)
                throw new Exception($"API Error: {errorMessage}");

            if (leagues != null && leagues.Count > 0)
            {
                foreach (var league in leagues)
                {
                    if (league.Id.HasValue && league.Name != null)
                    {
                        var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(league.Logo);
                        var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(league.Country?.Flag);
                        await _dataService.UpsertLeagueAsync(league.Id.Value, league.Name, league.Type, cdnLogo, league.Country?.Name, league.Country?.Code, cdnFlag);
                    }
                }
            }
        }

        private async Task FetchAllGamesAndTeamsAsync()
        {
            var leagues = await _dataService.GetLeaguesAsync();
            if (!leagues.Any())
                throw new Exception("No leagues in database. Fetch leagues first.");

            var seasons = await _dataService.GetSeasonsAsync();
            if (!seasons.Any())
                throw new Exception("No seasons in database. Fetch seasons first.");

            var yearList = seasons.Where(s => s.Year.HasValue).OrderByDescending(s => s.Year).Select(s => s.Year!.Value).Distinct().ToList();
            int startYear = yearList.Last();
            int endYear = yearList.First();

            foreach (var league in leagues)
            {
                var (games, errorMessage) = await _apiClient!.GetGamesByLeagueAndSeasonsAsync(league.Id, startYear, endYear);

                if (errorMessage != null || games == null || games.Count == 0)
                    continue;

                foreach (var game in games)
                {
                    if (game.Id.HasValue && game.Home?.Id.HasValue == true && game.Away?.Id.HasValue == true)
                    {
                        if (!await _dataService.TeamExistsAsync(game.Home.Id.Value) && game.Home.Name != null)
                        {
                            var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(game.Home.Logo);
                            var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(game.Home.Flag);
                            await _dataService.UpsertTeamAsync(game.Home.Id.Value, game.Home.Name, game.Home.Code, cdnFlag, cdnLogo);
                        }

                        if (!await _dataService.TeamExistsAsync(game.Away.Id.Value) && game.Away.Name != null)
                        {
                            var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(game.Away.Logo);
                            var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(game.Away.Flag);
                            await _dataService.UpsertTeamAsync(game.Away.Id.Value, game.Away.Name, game.Away.Code, cdnFlag, cdnLogo);
                        }

                        await _dataService.UpsertGameAsync(game.Id.Value, game.LeagueId, game.Season, game.Home.Id.Value, game.Away.Id.Value, game.Date, game.Status?.Short, game.Venue, game.Scores?.Home, game.Scores?.Away);
                    }
                }
            }
        }

        private async Task ShowDatabasePathAsync()
        {
            try
            {
                DatabasePath = RugbyDbContext.GetDatabasePath();
                StatusMessage = $"Database path copied to clipboard:\n{DatabasePath}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to get database path: {ex.Message}";
            }
        }

        private async Task ClearAllDataAsync()
        {
            try
            {
                await _dataService.ClearAllDataAsync();
                StatusMessage = "✅ All data cleared successfully";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to clear data: {ex.Message}";
            }
        }

        private void UpdateStorageInfo()
        {
            StorageInfo = _secretsService.GetStorageInfo();
        }
    }
}
