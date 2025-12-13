using System.Windows;
using Microsoft.Extensions.Configuration;
using RugbyApiApp.Data;
using RugbyApiApp.Services;

namespace RugbyApiApp.MAUI;

public partial class MainWindow : Window
{
    private readonly DataService _dataService;
    private readonly SecretsService _secretsService;
    private RugbyApiClient? _apiClient;

    public MainWindow()
    {
        InitializeComponent();
        
        // Configure User Secrets
        var config = new ConfigurationBuilder()
            .AddUserSecrets<MainWindow>()
            .Build();

        _secretsService = new SecretsService(config);

        // Initialize API client with key from User Secrets or environment variable
        var apiKey = _secretsService.GetApiKey();
        if (!string.IsNullOrEmpty(apiKey))
        {
            _apiClient = new RugbyApiClient(apiKey);
        }

        _dataService = new DataService(new RugbyDbContext());

        // Load initial dashboard data
        _ = LoadDashboardAsync();

        // Set up timer to refresh dashboard every 5 seconds
        var timer = new System.Windows.Threading.DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(5);
        timer.Tick += async (s, e) => await LoadDashboardAsync();
        timer.Start();
    }

    private async Task LoadDashboardAsync()
    {
        try
        {
            var stats = await _dataService.GetCompletionStatsAsync();

            // Update Countries
            CountriesCount.Text = $"{stats.CompleteCountries} / {stats.TotalCountries}";
            CountriesProgress.Value = stats.CountryCompletionPercent;
            CountriesPercent.Text = $"{stats.CountryCompletionPercent:F1}% Complete";

            // Update Seasons
            SeasonsCount.Text = $"{stats.CompleteSeasons} / {stats.TotalSeasons}";
            SeasonsProgress.Value = stats.SeasonCompletionPercent;
            SeasonsPercent.Text = $"{stats.SeasonCompletionPercent:F1}% Complete";

            // Update Leagues
            LeaguesCount.Text = $"{stats.CompleteLeagues} / {stats.TotalLeagues}";
            LeaguesProgress.Value = stats.LeagueCompletionPercent;
            LeaguesPercent.Text = $"{stats.LeagueCompletionPercent:F1}% Complete";

            // Update Teams
            TeamsCount.Text = $"{stats.CompleteTeams} / {stats.TotalTeams}";
            TeamsProgress.Value = stats.TeamCompletionPercent;
            TeamsPercent.Text = $"{stats.TeamCompletionPercent:F1}% Complete";

            // Update Games
            GamesCount.Text = $"{stats.CompleteGames} / {stats.TotalGames}";
            GamesProgress.Value = stats.GameCompletionPercent;
            GamesPercent.Text = $"{stats.GameCompletionPercent:F1}% Complete";
        }
        catch (Exception ex)
        {
            // Silent fail on dashboard update to prevent UI interruptions
        }
    }

    private async void OnViewStatisticsClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            var stats = await _dataService.GetCompletionStatsAsync();
            MessageBox.Show(stats.ToString(), "📊 Data Statistics", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load statistics: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnBrowseDataClicked(object sender, RoutedEventArgs e)
    {
        if (DataTypeCombo.SelectedIndex >= 0)
        {
            OnDataTypeChanged(DataTypeCombo, null);
        }
    }

    private void OnSettingsClicked(object sender, RoutedEventArgs e)
    {
        // Settings tab is already available via TabControl
        MessageBox.Show("Please click the 'Settings' tab above", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async void OnDataTypeChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs? e)
    {
        try
        {
            if (DataTypeCombo.SelectedIndex < 0) return;

            var selectedType = (DataTypeCombo.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content as string;
            switch (selectedType)
            {
                case "Countries":
                    await LoadCountries();
                    break;
                case "Seasons":
                    await LoadSeasons();
                    break;
                case "Leagues":
                    await LoadLeagues();
                    break;
                case "Teams":
                    await LoadTeams();
                    break;
                case "Games":
                    await LoadGames();
                    break;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task LoadCountries()
    {
        var countries = await _dataService.GetCountriesAsync();
        DataGrid.ItemsSource = countries.Select(c => new
        {
            Name = c.Name,
            Code = c.Code,
            Status = c.IsDataComplete ? "✓ Complete" : "⚠ Incomplete"
        }).ToList();
    }

    private async Task LoadSeasons()
    {
        var seasons = await _dataService.GetSeasonsAsync();
        DataGrid.ItemsSource = seasons.Select(s => new
        {
            Year = s.Year,
            Current = s.IsCurrent ? "Yes" : "No",
            Status = s.IsDataComplete ? "✓ Complete" : "⚠ Incomplete"
        }).ToList();
    }

    private async Task LoadLeagues()
    {
        var leagues = await _dataService.GetLeaguesAsync();
        DataGrid.ItemsSource = leagues.Select(l => new
        {
            Name = l.Name,
            Country = l.CountryCode,
            Type = l.Type
        }).ToList();
    }

    private async Task LoadTeams()
    {
        var teams = await _dataService.GetTeamsAsync();
        DataGrid.ItemsSource = teams.Select(t => new
        {
            Name = t.Name,
            Code = t.Code,
            Status = t.IsDataComplete ? "✓ Complete" : "⚠ Incomplete"
        }).ToList();
    }

    private async Task LoadGames()
    {
        var games = await _dataService.GetGamesAsync();
        DataGrid.ItemsSource = games.Select(g => new
        {
            Home = g.HomeTeam?.Name ?? "Unknown",
            Away = g.AwayTeam?.Name ?? "Unknown",
            Date = g.Date?.ToString("yyyy-MM-dd") ?? "TBD",
            Venue = g.Venue ?? "TBD"
        }).ToList();
    }

    private void OnRefreshClicked(object sender, RoutedEventArgs e)
    {
        if (DataTypeCombo.SelectedIndex < 0)
        {
            MessageBox.Show("Please select a data type first", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var selectedType = (DataTypeCombo.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content as string;
        switch (selectedType)
        {
            case "Countries":
                FetchCountriesAsync();
                break;
            case "Seasons":
                FetchSeasonsAsync();
                break;
            case "Leagues":
                FetchLeaguesAsync();
                break;
            case "Teams":
                FetchTeamsAsync();
                break;
            case "Games":
                FetchGamesAsync();
                break;
        }
    }

    private async void FetchCountriesAsync()
    {
        try
        {
            if (_apiClient == null)
            {
                MessageBox.Show("Please save an API key first", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Fetch all countries from API?",
                "Fetch Countries",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            await Task.Run(async () =>
            {
                try
                {
                    Dispatcher.Invoke(() => MessageBox.Show("Fetching countries...", "Progress", MessageBoxButton.OK, MessageBoxImage.Information));

                    var (countries, errorMessage) = await _apiClient.GetCountriesAsync();

                    if (errorMessage != null)
                    {
                        Dispatcher.Invoke(() => MessageBox.Show($"API Error: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
                        return;
                    }

                    if (countries != null && countries.Count > 0)
                    {
                        int count = 0;
                        foreach (var countryResponse in countries)
                        {
                            if (countryResponse.Id.HasValue && countryResponse.Name != null)
                            {
                                var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(countryResponse.Flag);

                                await _dataService.UpsertCountryAsync(
                                    countryResponse.Id.Value,
                                    countryResponse.Name,
                                    countryResponse.Code,
                                    cdnFlag
                                );
                                count++;
                            }
                        }

                        Dispatcher.Invoke(async () =>
                        {
                            MessageBox.Show($"✅ Stored {count} countries", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            await LoadCountries();
                        });
                    }
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Failed to fetch countries: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void FetchSeasonsAsync()
    {
        try
        {
            if (_apiClient == null)
            {
                MessageBox.Show("Please save an API key first", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Fetch all seasons from API?",
                "Fetch Seasons",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            await Task.Run(async () =>
            {
                try
                {
                    Dispatcher.Invoke(() => MessageBox.Show("Fetching seasons...", "Progress", MessageBoxButton.OK, MessageBoxImage.Information));

                    var (seasonYears, errorMessage) = await _apiClient.GetSeasonsAsync();

                    if (errorMessage != null)
                    {
                        Dispatcher.Invoke(() => MessageBox.Show($"API Error: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
                        return;
                    }

                    if (seasonYears != null && seasonYears.Count > 0)
                    {
                        int count = 0;
                        foreach (var year in seasonYears)
                        {
                            await _dataService.UpsertSeasonAsync(
                                seasonId: year,
                                year: year,
                                startDate: null,
                                endDate: null,
                                isCurrent: false
                            );
                            count++;
                        }

                        Dispatcher.Invoke(async () =>
                        {
                            MessageBox.Show($"✅ Stored {count} seasons", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            await LoadSeasons();
                        });
                    }
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Failed to fetch seasons: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void FetchLeaguesAsync()
    {
        try
        {
            if (_apiClient == null)
            {
                MessageBox.Show("Please save an API key first", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Fetch all leagues from API?",
                "Fetch Leagues",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            await Task.Run(async () =>
            {
                try
                {
                    Dispatcher.Invoke(() => MessageBox.Show("Fetching leagues...", "Progress", MessageBoxButton.OK, MessageBoxImage.Information));

                    var (leagues, errorMessage) = await _apiClient.GetLeaguesAsync();

                    if (errorMessage != null)
                    {
                        Dispatcher.Invoke(() => MessageBox.Show($"API Error: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
                        return;
                    }

                    if (leagues != null && leagues.Count > 0)
                    {
                        int count = 0;
                        foreach (var leagueResponse in leagues)
                        {
                            if (leagueResponse.Id.HasValue && leagueResponse.Name != null)
                            {
                                var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(leagueResponse.Logo);
                                var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(leagueResponse.Country?.Flag);
                                await _dataService.UpsertLeagueAsync(leagueResponse.Id.Value, leagueResponse.Name, leagueResponse.Type, cdnLogo, leagueResponse.Country?.Name, leagueResponse.Country?.Code, cdnFlag);
                                count++;
                            }
                        }

                        Dispatcher.Invoke(async () =>
                        {
                            MessageBox.Show($"✅ Stored {count} leagues", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            await LoadLeagues();
                        });
                    }
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Failed to fetch leagues: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void FetchTeamsAsync()
    {
        MessageBox.Show("Teams are auto-created when fetching games.\n\nPlease use the 'Games' fetch to add teams.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async void FetchGamesAsync()
    {
        try
        {
            if (_apiClient == null)
            {
                MessageBox.Show("Please save an API key first", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var leagues = await _dataService.GetLeaguesAsync();
            if (!leagues.Any())
            {
                MessageBox.Show("No leagues in database. Fetch leagues first.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var seasons = await _dataService.GetSeasonsAsync();
            if (!seasons.Any())
            {
                MessageBox.Show("No seasons in database. Fetch seasons first.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Fetch games for all leagues and all seasons?\n\nThis will take several minutes.",
                "Fetch Games",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            await Task.Run(async () =>
            {
                try
                {
                    Dispatcher.Invoke(() => MessageBox.Show("Fetching games...\n\nThis may take a few minutes.", "Progress", MessageBoxButton.OK, MessageBoxImage.Information));

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

                    Dispatcher.Invoke(async () =>
                    {
                        MessageBox.Show($"✅ Stored {gamesCount} games and {teamsCount} teams", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadGames();
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Failed to fetch games: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void OnSaveApiKeyClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            var apiKey = ApiKeyBox.Password;
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                MessageBox.Show("Please enter an API key", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _secretsService.SetApiKey(apiKey);
            _apiClient = new RugbyApiClient(apiKey);
            
            MessageBox.Show("API key saved successfully\n\n" + _secretsService.GetStorageInfo(), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ApiKeyBox.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save API key: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnClearApiKeyClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            var result = MessageBox.Show(
                "Remove stored API key?\n\n" +
                "Note: To also clear from User Secrets, run:\n" +
                "dotnet user-secrets remove RugbyApiKey",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                _secretsService.ClearApiKey();
                _apiClient = null;
                ApiKeyBox.Clear();
                MessageBox.Show("API key cleared from environment variable\n\n" + _secretsService.GetStorageInfo(), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to clear API key: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnShowDatabasePathClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            var dbPath = RugbyDbContext.GetDatabasePath();
            MessageBox.Show(dbPath, "Database Path", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to get database path: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void OnClearDataClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            var result = MessageBox.Show(
                "This will delete all data from the database. Are you sure?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await _dataService.ClearAllDataAsync();
                MessageBox.Show("All data cleared successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to clear data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void OnTestApiKeyClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_apiClient == null)
            {
                MessageBox.Show("Please save an API key first", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Show loading message
            MessageBox.Show("Testing API connection...", "Testing", MessageBoxButton.OK, MessageBoxImage.Information);

            var (status, errorMessage) = await _apiClient.GetStatusAsync();

            if (errorMessage != null)
            {
                MessageBox.Show($"API Error: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

                var message = $"✅ API Connection Successful\n\n" +
                    $"Account: {accountName}\n" +
                    $"Email: {status.Response.Account?.Email}\n" +
                    $"Plan: {plan}\n\n" +
                    $"📊 Daily API Requests:\n" +
                    $"Current: {current:N0} / {limit:N0}\n" +
                    $"Remaining: {remaining:N0}\n" +
                    $"Usage: {percentUsed:F1}%";

                MessageBox.Show(message, "✅ API Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No status information returned from API", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to test API key: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void OnAutoFetchAllDataClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_apiClient == null)
            {
                MessageBox.Show("Please save an API key first", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "This will fetch all countries, seasons, leagues, teams, and games from the API.\n\n" +
                "This may take several minutes depending on your internet connection.\n\n" +
                "Do you want to continue?",
                "Auto-Fetch All Data",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            // Disable button during fetch
            var button = sender as System.Windows.Controls.Button;
            if (button != null) button.IsEnabled = false;

            // Run fetch on background thread to keep UI responsive
            await Task.Run(async () =>
            {
                try
                {
                    await AutoFetchAllIncompleteAsync();
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Auto-fetch failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                    return;
                }

                // Show completion message on UI thread
                Dispatcher.Invoke(async () =>
                {
                    MessageBox.Show("✅ Auto-fetch completed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Refresh statistics
                    var stats = await _dataService.GetCompletionStatsAsync();
                    MessageBox.Show(stats.ToString(), "📊 Data Statistics", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Re-enable button
                    if (button != null) button.IsEnabled = true;
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Auto-fetch failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task AutoFetchAllIncompleteAsync()
    {
        try
        {
            // Fetch Countries
            Dispatcher.Invoke(() => 
                MessageBox.Show("Fetching Countries...", "Progress", MessageBoxButton.OK, MessageBoxImage.Information));
            await FetchAndStoreCountriesAsync();

            // Fetch Seasons
            Dispatcher.Invoke(() =>
                MessageBox.Show("Fetching Seasons...", "Progress", MessageBoxButton.OK, MessageBoxImage.Information));
            await FetchAndStoreSeasonAsync();

            // Fetch Leagues
            Dispatcher.Invoke(() =>
                MessageBox.Show("Fetching Leagues...", "Progress", MessageBoxButton.OK, MessageBoxImage.Information));
            await FetchAndStoreLeaguesAsync();

            // Fetch Games and Teams
            Dispatcher.Invoke(() =>
                MessageBox.Show("Fetching Games and Teams...\n\nThis may take a few minutes.", "Progress", MessageBoxButton.OK, MessageBoxImage.Information));
            await FetchAllGamesAndTeamsAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error during auto-fetch: {ex.Message}", ex);
        }
    }

    private async Task FetchAndStoreCountriesAsync()
    {
        try
        {
            var incompleteCountries = await _dataService.GetIncompleteCountriesAsync();
            if (incompleteCountries.Count > 0)
            {
                // Countries already fetched
                return;
            }

            var (countries, errorMessage) = await _apiClient.GetCountriesAsync();

            if (errorMessage != null)
            {
                throw new Exception($"API Error: {errorMessage}");
            }

            if (countries != null && countries.Count > 0)
            {
                foreach (var countryResponse in countries)
                {
                    if (countryResponse.Id.HasValue && countryResponse.Name != null)
                    {
                        var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(countryResponse.Flag);

                        await _dataService.UpsertCountryAsync(
                            countryResponse.Id.Value,
                            countryResponse.Name,
                            countryResponse.Code,
                            cdnFlag
                        );
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to fetch countries: {ex.Message}", ex);
        }
    }

    private async Task FetchAndStoreSeasonAsync()
    {
        try
        {
            var incompleteSeasons = await _dataService.GetIncompleteSeasonAsync();
            if (incompleteSeasons.Count > 0)
            {
                // Seasons already fetched
                return;
            }

            var (seasonYears, errorMessage) = await _apiClient.GetSeasonsAsync();

            if (errorMessage != null)
            {
                throw new Exception($"API Error: {errorMessage}");
            }

            if (seasonYears != null && seasonYears.Count > 0)
            {
                foreach (var year in seasonYears)
                {
                    await _dataService.UpsertSeasonAsync(
                        seasonId: year,
                        year: year,
                        startDate: null,
                        endDate: null,
                        isCurrent: false
                    );
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to fetch seasons: {ex.Message}", ex);
        }
    }

    private async Task FetchAndStoreLeaguesAsync()
    {
        try
        {
            var incompleteLeagues = await _dataService.GetIncompleteLeaguesAsync();
            if (incompleteLeagues.Count > 0)
            {
                // Leagues already fetched
                return;
            }

            var (leagues, errorMessage) = await _apiClient.GetLeaguesAsync();

            if (errorMessage != null)
            {
                throw new Exception($"API Error: {errorMessage}");
            }

            if (leagues != null && leagues.Count > 0)
            {
                foreach (var leagueResponse in leagues)
                {
                    if (leagueResponse.Id.HasValue && leagueResponse.Name != null)
                    {
                        var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(leagueResponse.Logo);
                        var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(leagueResponse.Country?.Flag);

                        await _dataService.UpsertLeagueAsync(
                            leagueResponse.Id.Value,
                            leagueResponse.Name,
                            leagueResponse.Type,
                            cdnLogo,
                            leagueResponse.Country?.Name,
                            leagueResponse.Country?.Code,
                            cdnFlag
                        );
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to fetch leagues: {ex.Message}", ex);
        }
    }

    private async Task FetchAllGamesAndTeamsAsync()
    {
        try
        {
            var leagues = await _dataService.GetLeaguesAsync();
            if (!leagues.Any())
            {
                throw new Exception("No leagues in database. Fetch leagues first.");
            }

            var seasons = await _dataService.GetSeasonsAsync();
            if (!seasons.Any())
            {
                throw new Exception("No seasons in database. Fetch seasons first.");
            }

            var yearList = seasons
                .Where(s => s.Year.HasValue)
                .OrderByDescending(s => s.Year)
                .Select(s => s.Year!.Value)
                .Distinct()
                .ToList();

            if (!yearList.Any())
            {
                throw new Exception("No valid seasons found.");
            }

            int startYear = yearList.Last();
            int endYear = yearList.First();

            int totalLeaguesProcessed = 0;
            int totalGames = 0;
            int totalTeams = 0;

            foreach (var league in leagues)
            {
                try
                {
                    var (games, errorMessage) = await _apiClient.GetGamesByLeagueAndSeasonsAsync(league.Id, startYear, endYear);

                    if (errorMessage != null)
                    {
                        // Log error but continue with other leagues
                        continue;
                    }

                    if (games != null && games.Count > 0)
                    {
                        int leagueGames = 0;
                        int leagueTeams = 0;

                        foreach (var gameResponse in games)
                        {
                            if (gameResponse.Id.HasValue &&
                                gameResponse.Home?.Id.HasValue == true &&
                                gameResponse.Away?.Id.HasValue == true)
                            {
                                // Store home team if new
                                if (!await _dataService.TeamExistsAsync(gameResponse.Home.Id.Value))
                                {
                                    if (gameResponse.Home.Name != null)
                                    {
                                        var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Home.Logo);
                                        var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Home.Flag);
                                        await _dataService.UpsertTeamAsync(
                                            gameResponse.Home.Id.Value,
                                            gameResponse.Home.Name,
                                            gameResponse.Home.Code,
                                            cdnFlag,
                                            cdnLogo
                                        );
                                        leagueTeams++;
                                    }
                                }

                                // Store away team if new
                                if (!await _dataService.TeamExistsAsync(gameResponse.Away.Id.Value))
                                {
                                    if (gameResponse.Away.Name != null)
                                    {
                                        var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Away.Logo);
                                        var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Away.Flag);
                                        await _dataService.UpsertTeamAsync(
                                            gameResponse.Away.Id.Value,
                                            gameResponse.Away.Name,
                                            gameResponse.Away.Code,
                                            cdnFlag,
                                            cdnLogo
                                        );
                                        leagueTeams++;
                                    }
                                }

                                // Store game
                                await _dataService.UpsertGameAsync(
                                    gameResponse.Id.Value,
                                    gameResponse.LeagueId,
                                    gameResponse.Season,
                                    gameResponse.Home.Id.Value,
                                    gameResponse.Away.Id.Value,
                                    gameResponse.Date,
                                    gameResponse.Status?.Short,
                                    gameResponse.Venue,
                                    gameResponse.Scores?.Home,
                                    gameResponse.Scores?.Away
                                );
                                leagueGames++;
                            }
                        }

                        if (leagueGames > 0)
                        {
                            totalGames += leagueGames;
                            totalTeams += leagueTeams;
                            totalLeaguesProcessed++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error but continue with other leagues
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to fetch games and teams: {ex.Message}", ex);
        }
    }
}
