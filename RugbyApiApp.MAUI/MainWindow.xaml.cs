using System.Windows;
using Microsoft.Extensions.Configuration;
using RugbyApiApp.Data;
using RugbyApiApp.MAUI.ViewModels;
using RugbyApiApp.Services;

namespace RugbyApiApp.MAUI;

public partial class MainWindow : Window
{
    private MainViewModel? _viewModel;

    public MainWindow()
    {
        InitializeComponent();

        // Configure User Secrets
        var config = new ConfigurationBuilder()
            .AddUserSecrets<MainWindow>()
            .Build();

        // Create services
        var secretsService = new SecretsService(config);
        var dataService = new DataService(new RugbyDbContext());

        // Create and set MainViewModel
        _viewModel = new MainViewModel(dataService, secretsService, config);
        DataContext = _viewModel;

        // Set up data contexts for individual tabs
        HomeTab.DataContext = _viewModel.HomeViewModel;
        DataTab.DataContext = _viewModel.DataViewModel;
        SettingsTab.DataContext = _viewModel.SettingsViewModel;
    }

    protected override void OnClosed(EventArgs e)
    {
        _viewModel?.Cleanup();
        base.OnClosed(e);
    }

    // Navigation handlers
    private void OnViewStatisticsClicked(object sender, RoutedEventArgs e)
    {
        var stats = _viewModel?.HomeViewModel;
        if (stats != null)
        {
            MessageBox.Show(
                $"Countries: {stats.CountriesPercent}\n" +
                $"Seasons: {stats.SeasonsPercent}\n" +
                $"Leagues: {stats.LeaguesPercent}\n" +
                $"Teams: {stats.TeamsPercent}\n" +
                $"Games: {stats.GamesPercent}",
                "📊 Data Statistics",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    private void OnBrowseDataClicked(object sender, RoutedEventArgs e)
    {
        TabControl.SelectedIndex = 1; // Switch to Data tab
    }

    private void OnSettingsClicked(object sender, RoutedEventArgs e)
    {
        TabControl.SelectedIndex = 2; // Switch to Settings tab
    }

    private void OnAutoFetchAllDataClicked(object sender, RoutedEventArgs e)
    {
        if (_viewModel?.SettingsViewModel != null)
        {
            var result = MessageBox.Show(
                "This will fetch all countries, seasons, leagues, teams, and games from the API.\n\n" +
                "This may take several minutes depending on your internet connection.\n\n" +
                "Do you want to continue?",
                "Auto-Fetch All Data",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _viewModel.SettingsViewModel.AutoFetchAllDataCommand.Execute(null);
            }
        }
    }

    private void OnClearDataClicked(object sender, RoutedEventArgs e)
    {
        if (_viewModel?.SettingsViewModel != null)
        {
            var result = MessageBox.Show(
                "This will delete all data from the database. Are you sure?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _viewModel.SettingsViewModel.ClearAllDataCommand.Execute(null);
            }
        }
    }

    // Data tab handlers
    private void OnDataTypeChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs? e)
    {
        if (DataTypeCombo.SelectedIndex >= 0 && _viewModel?.DataViewModel != null)
        {
            var selectedItem = (DataTypeCombo.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content as string;
            _viewModel.DataViewModel.SelectedDataType = selectedItem;
        }
    }

    private void OnRefreshClicked(object sender, RoutedEventArgs e)
    {
        if (DataTypeCombo.SelectedIndex >= 0 && _viewModel?.DataViewModel != null)
        {
            _viewModel.DataViewModel.RefreshCommand.Execute(null);
        }
    }
}
