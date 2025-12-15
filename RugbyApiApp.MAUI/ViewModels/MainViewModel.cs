using Microsoft.Extensions.Configuration;
using RugbyApiApp.Data;
using RugbyApiApp.Services;

namespace RugbyApiApp.MAUI.ViewModels
{
    /// <summary>
    /// Main ViewModel that coordinates all child ViewModels and services
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private readonly SecretsService _secretsService;
        private readonly IConfiguration _configuration;
        private RugbyApiClient? _apiClient;

        public HomeViewModel HomeViewModel { get; }
        public DataViewModel DataViewModel { get; }
        public SettingsViewModel SettingsViewModel { get; }
        public WatchViewModel WatchViewModel { get; }

        public MainViewModel(DataService dataService, SecretsService secretsService, IConfiguration configuration)
        {
            _dataService = dataService;
            _secretsService = secretsService;
            _configuration = configuration;

            // Initialize API client
            var apiKey = secretsService.GetApiKey();
            if (!string.IsNullOrEmpty(apiKey))
            {
                _apiClient = new RugbyApiClient(apiKey);
            }

            // Create child ViewModels
            HomeViewModel = new HomeViewModel(dataService);
            DataViewModel = new DataViewModel(dataService, _apiClient);
            SettingsViewModel = new SettingsViewModel(dataService, secretsService, configuration);
            WatchViewModel = new WatchViewModel(dataService);
            SettingsViewModel.SetApiClient(_apiClient);
        }

        public void UpdateApiClient(RugbyApiClient? apiClient)
        {
            _apiClient = apiClient;
            DataViewModel.SetApiClient(apiClient);
            SettingsViewModel.SetApiClient(apiClient);
        }

        public void Cleanup()
        {
            HomeViewModel.Cleanup();
        }
    }
}
