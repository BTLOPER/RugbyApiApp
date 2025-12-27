using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RugbyApiApp.MAUI.ViewModels
{
    /// <summary>
    /// Grid item for Leagues with minimal property change notification
    /// </summary>
    public class LeagueGridItem : INotifyPropertyChanged
    {
        private bool _favorite;

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Type { get; set; }

        public bool Favorite
        {
            get => _favorite;
            set => SetProperty(ref _favorite, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// Grid item for Teams with minimal property change notification
    /// </summary>
    public class TeamGridItem : INotifyPropertyChanged
    {
        private bool _favorite;

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Status { get; set; }

        public bool Favorite
        {
            get => _favorite;
            set => SetProperty(ref _favorite, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
