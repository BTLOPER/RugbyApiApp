using RugbyApiApp.MAUI.ViewModels;

namespace RugbyApiApp.MAUI.Services
{
    /// <summary>
    /// Service for handling video window operations
    /// </summary>
    public interface IVideoWindowService
    {
        /// <summary>
        /// Show the Add/Edit video dialog
        /// </summary>
        bool? ShowAddEditVideoDialog(int gameId, string homeTeam, string awayTeam, DateTime gameDate, int? videoId = null);
    }

    /// <summary>
    /// Implementation of video window service
    /// </summary>
    public class VideoWindowService : IVideoWindowService
    {
        public bool? ShowAddEditVideoDialog(int gameId, string homeTeam, string awayTeam, DateTime gameDate, int? videoId = null)
        {
            var window = new Views.AddEditVideoWindow(gameId, homeTeam, awayTeam, gameDate, videoId);
            return window.ShowDialog();
        }
    }
}
