using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Navigation;

namespace RugbyApiApp.MAUI.Views
{
    /// <summary>
    /// Interaction logic for WatchTabView.xaml
    /// Code-behind for the Watch tab view - handles rating star clicks
    /// </summary>
    public partial class WatchTabView : UserControl
    {
        public WatchTabView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles hyperlink navigation for YouTube video URLs
        /// </summary>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                // Open URL in default browser
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
                e.Handled = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error opening hyperlink: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles star rating clicks in the video DataGrid
        /// </summary>
        private void RatingStar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is not Button button)
                    return;

                if (!int.TryParse(button.Tag?.ToString(), out int rating))
                    return;

                // Get the parent DataGrid
                var dataGrid = FindParentDataGrid(button);
                if (dataGrid == null)
                    return;

                // Get the data context of the clicked row (the VideoItem)
                var dataContext = button.DataContext;
                if (dataContext is RugbyApiApp.MAUI.ViewModels.VideoItem video)
                {
                    // Access the ViewModel from the UserControl's DataContext
                    if (this.DataContext is RugbyApiApp.MAUI.ViewModels.WatchViewModel viewModel)
                    {
                        // Update the rating via ViewModel
                        _ = viewModel.UpdateVideoRatingAsync(video.Id, rating);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in RatingStar_Click: {ex.Message}");
            }

            e.Handled = true;
        }

        /// <summary>
        /// Helper method to find the parent DataGrid in the visual tree
        /// </summary>
        private DataGrid? FindParentDataGrid(DependencyObject? element)
        {
            if (element == null)
                return null;

            if (element is DataGrid dg)
                return dg;

            return FindParentDataGrid(VisualTreeHelper.GetParent(element));
        }
    }
}
