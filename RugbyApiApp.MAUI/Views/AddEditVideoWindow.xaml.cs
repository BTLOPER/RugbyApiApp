using System.Windows;
using RugbyApiApp.MAUI.ViewModels;

namespace RugbyApiApp.MAUI.Views
{
    public partial class AddEditVideoWindow : Window
    {
        private AddEditVideoViewModel? _viewModel;
        private bool _isEditMode = false;

        public AddEditVideoWindow(int gameId, string homeTeam, string awayTeam, DateTime gameDate, int? videoId = null)
        {
            InitializeComponent();

            _isEditMode = videoId.HasValue;

            // Set window title based on mode
            WindowTitle.Text = _isEditMode ? "Edit Video" : "Add Video";
            SaveButton.Content = _isEditMode ? "💾 Update" : "💾 Save";

            // Display game info
            HomeTeamBlock.Text = homeTeam;
            AwayTeamBlock.Text = awayTeam;
            DateBlock.Text = gameDate.ToString("yyyy-MM-dd");

            // Create view model
            _viewModel = new AddEditVideoViewModel(gameId, videoId);
            DataContext = _viewModel;

            // Load video data if editing
            if (_isEditMode && videoId.HasValue)
            {
                LoadVideoData(videoId.Value);
            }
        }

        private void LoadVideoData(int videoId)
        {
            if (_viewModel != null)
            {
                _viewModel.LoadVideoAsync(videoId).ConfigureAwait(false);
            }
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_viewModel != null)
            {
                var result = _viewModel.SaveVideoAsync(
                    TitleTextBox.Text,
                    UrlTextBox.Text,
                    DescriptionTextBox.Text,
                    LengthTextBox.Text,
                    VideoDateTextBox.Text,
                    RatingTextBox.Text,
                    IsWatchedCheckBox.IsChecked ?? false,
                    IsFavoriteCheckBox.IsChecked ?? false
                ).Result;

                if (result)
                {
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to save video. Please check your input.", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            // Title is required
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
                return false;

            // URL is required
            if (string.IsNullOrWhiteSpace(UrlTextBox.Text))
                return false;

            return true;
        }
    }
}
