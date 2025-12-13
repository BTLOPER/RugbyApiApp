using System.Windows.Input;
using RugbyApiApp.Data;
using RugbyApiApp.Services;

namespace RugbyApiApp.MAUI.ViewModels
{
    /// <summary>
    /// ViewModel for the Home tab with real-time dashboard statistics
    /// </summary>
    public class HomeViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private System.Windows.Threading.DispatcherTimer? _refreshTimer;

        private int _countriesCount;
        private double _countriesProgress;
        private string _countriesPercent = "0% Complete";

        private int _seasonsCount;
        private double _seasonsProgress;
        private string _seasonsPercent = "0% Complete";

        private int _leaguesCount;
        private double _leaguesProgress;
        private string _leaguesPercent = "0% Complete";

        private int _teamsCount;
        private double _teamsProgress;
        private string _teamsPercent = "0% Complete";

        private int _gamesCount;
        private double _gamesProgress;
        private string _gamesPercent = "0% Complete";

        public int CountriesCount
        {
            get => _countriesCount;
            set => SetProperty(ref _countriesCount, value);
        }

        public double CountriesProgress
        {
            get => _countriesProgress;
            set => SetProperty(ref _countriesProgress, value);
        }

        public string CountriesPercent
        {
            get => _countriesPercent;
            set => SetProperty(ref _countriesPercent, value);
        }

        public int SeasonsCount
        {
            get => _seasonsCount;
            set => SetProperty(ref _seasonsCount, value);
        }

        public double SeasonsProgress
        {
            get => _seasonsProgress;
            set => SetProperty(ref _seasonsProgress, value);
        }

        public string SeasonsPercent
        {
            get => _seasonsPercent;
            set => SetProperty(ref _seasonsPercent, value);
        }

        public int LeaguesCount
        {
            get => _leaguesCount;
            set => SetProperty(ref _leaguesCount, value);
        }

        public double LeaguesProgress
        {
            get => _leaguesProgress;
            set => SetProperty(ref _leaguesProgress, value);
        }

        public string LeaguesPercent
        {
            get => _leaguesPercent;
            set => SetProperty(ref _leaguesPercent, value);
        }

        public int TeamsCount
        {
            get => _teamsCount;
            set => SetProperty(ref _teamsCount, value);
        }

        public double TeamsProgress
        {
            get => _teamsProgress;
            set => SetProperty(ref _teamsProgress, value);
        }

        public string TeamsPercent
        {
            get => _teamsPercent;
            set => SetProperty(ref _teamsPercent, value);
        }

        public int GamesCount
        {
            get => _gamesCount;
            set => SetProperty(ref _gamesCount, value);
        }

        public double GamesProgress
        {
            get => _gamesProgress;
            set => SetProperty(ref _gamesProgress, value);
        }

        public string GamesPercent
        {
            get => _gamesPercent;
            set => SetProperty(ref _gamesPercent, value);
        }

        public ICommand RefreshStatsCommand { get; }

        public HomeViewModel(DataService dataService)
        {
            _dataService = dataService;
            RefreshStatsCommand = new AsyncRelayCommand(_ => RefreshStatsAsync());

            // Initialize dashboard and start refresh timer
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await RefreshStatsAsync();
            StartRefreshTimer();
        }

        private async Task RefreshStatsAsync()
        {
            try
            {
                var stats = await _dataService.GetCompletionStatsAsync();

                CountriesCount = stats.CompleteCountries;
                CountriesProgress = stats.CountryCompletionPercent;
                CountriesPercent = $"{stats.CountryCompletionPercent:F1}% Complete ({stats.CompleteCountries} / {stats.TotalCountries})";

                SeasonsCount = stats.CompleteSeasons;
                SeasonsProgress = stats.SeasonCompletionPercent;
                SeasonsPercent = $"{stats.SeasonCompletionPercent:F1}% Complete ({stats.CompleteSeasons} / {stats.TotalSeasons})";

                LeaguesCount = stats.CompleteLeagues;
                LeaguesProgress = stats.LeagueCompletionPercent;
                LeaguesPercent = $"{stats.LeagueCompletionPercent:F1}% Complete ({stats.CompleteLeagues} / {stats.TotalLeagues})";

                TeamsCount = stats.CompleteTeams;
                TeamsProgress = stats.TeamCompletionPercent;
                TeamsPercent = $"{stats.TeamCompletionPercent:F1}% Complete ({stats.CompleteTeams} / {stats.TotalTeams})";

                GamesCount = stats.CompleteGames;
                GamesProgress = stats.GameCompletionPercent;
                GamesPercent = $"{stats.GameCompletionPercent:F1}% Complete ({stats.CompleteGames} / {stats.TotalGames})";
            }
            catch (Exception ex)
            {
                // Silent fail to prevent UI interruption
            }
        }

        private void StartRefreshTimer()
        {
            _refreshTimer = new System.Windows.Threading.DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromSeconds(5);
            _refreshTimer.Tick += async (s, e) => await RefreshStatsAsync();
            _refreshTimer.Start();
        }

        public void Cleanup()
        {
            _refreshTimer?.Stop();
            _refreshTimer = null;
        }
    }
}
