using System.Windows;
using Microsoft.Extensions.Configuration;
using RugbyApiApp.Data;
using RugbyApiApp.MAUI.ViewModels;
using RugbyApiApp.Services;

namespace RugbyApiApp.MAUI;

public partial class MainWindow : Window
{
    private MainViewModel? _viewModel;
    private List<GridDataItem>? _originalGridData; // Store original unfiltered data

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
            
            // Update DataGrid visibility based on selection
            UpdateDataGridVisibility(selectedItem);
        }
    }

    private void UpdateDataGridVisibility(string? selectedDataType)
    {
        // Reset original data when switching data types
        _originalGridData = null;

        // Hide all DataGrids first
        CountriesDataGrid.Visibility = System.Windows.Visibility.Collapsed;
        SeasonsDataGrid.Visibility = System.Windows.Visibility.Collapsed;
        LeaguesDataGrid.Visibility = System.Windows.Visibility.Collapsed;
        TeamsDataGrid.Visibility = System.Windows.Visibility.Collapsed;
        GamesDataGrid.Visibility = System.Windows.Visibility.Collapsed;

        // Show/hide search and filter controls based on data type
        bool showSearchAndFilter = selectedDataType == "Leagues" || selectedDataType == "Teams";
        SearchBox.Visibility = showSearchAndFilter ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        FavoritesCheckBox.Visibility = showSearchAndFilter ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

        // Show the selected DataGrid
        switch (selectedDataType)
        {
            case "Countries":
                CountriesDataGrid.Visibility = System.Windows.Visibility.Visible;
                break;
            case "Seasons":
                SeasonsDataGrid.Visibility = System.Windows.Visibility.Visible;
                break;
            case "Leagues":
                LeaguesDataGrid.Visibility = System.Windows.Visibility.Visible;
                break;
            case "Teams":
                TeamsDataGrid.Visibility = System.Windows.Visibility.Visible;
                break;
            case "Games":
                GamesDataGrid.Visibility = System.Windows.Visibility.Visible;
                break;
        }

        // Clear search and filter when switching data types
        SearchBox.Text = string.Empty;
        FavoritesCheckBox.IsChecked = false;
    }

    private void OnRefreshClicked(object sender, RoutedEventArgs e)
    {
        if (DataTypeCombo.SelectedIndex >= 0 && _viewModel?.DataViewModel != null)
        {
            _viewModel.DataViewModel.RefreshCommand.Execute(null);
            // Reset original data when refreshing
            _originalGridData = null;
        }
    }

    private void DataGrid_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
    {
        // Handle favorite checkbox changes
        if (e.Column is System.Windows.Controls.DataGridCheckBoxColumn && e.Column.Header.ToString() == "⭐ Favorite")
        {
            try
            {
                var dataGrid = sender as System.Windows.Controls.DataGrid;
                if (dataGrid?.SelectedItem != null)
                {
                    var rowData = dataGrid.SelectedItem;
                    var idProperty = rowData.GetType().GetProperty("Id");
                    if (idProperty != null)
                    {
                        var id = (int)idProperty.GetValue(rowData);
                        _viewModel?.DataViewModel.ToggleFavoriteCommand.Execute(id);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error toggling favorite: {ex.Message}");
            }
        }
    }

    private void OnSearchTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void OnFavoritesFilterChanged(object sender, RoutedEventArgs e)
    {
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        if (_viewModel?.DataViewModel == null)
            return;

        string searchText = SearchBox.Text.ToLower();
        bool favoritesOnly = FavoritesCheckBox.IsChecked ?? false;
        var selectedDataType = (DataTypeCombo.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content as string;

        // Get the current grid data from the ViewModel
        if (_viewModel.DataViewModel.GridData is List<GridDataItem> currentData)
        {
            // Store original data on first filter application
            if (_originalGridData == null)
            {
                _originalGridData = new List<GridDataItem>(currentData);
            }

            // Start with the original unfiltered data
            var filteredData = new List<GridDataItem>(_originalGridData);

            // Filter based on search text and favorites
            filteredData = filteredData
                .Where(item =>
                {
                    // Apply favorites filter
                    if (favoritesOnly && !item.Favorite)
                        return false;

                    // Apply search filter - search in Name and other relevant fields
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        bool matchesSearch = false;

                        if (!string.IsNullOrEmpty(item.Name) && item.Name.ToLower().Contains(searchText))
                            matchesSearch = true;
                        else if (!string.IsNullOrEmpty(item.Code) && item.Code.ToLower().Contains(searchText))
                            matchesSearch = true;
                        else if (!string.IsNullOrEmpty(item.Country) && item.Country.ToLower().Contains(searchText))
                            matchesSearch = true;
                        else if (!string.IsNullOrEmpty(item.Type) && item.Type.ToLower().Contains(searchText))
                            matchesSearch = true;

                        if (!matchesSearch)
                            return false;
                    }

                    return true;
                })
                .ToList();

            // Update the GridData with filtered results
            _viewModel.DataViewModel.GridData = filteredData;
        }
    }
}
