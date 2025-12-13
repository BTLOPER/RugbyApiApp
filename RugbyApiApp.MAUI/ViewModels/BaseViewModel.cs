using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RugbyApiApp.MAUI.ViewModels
{
    /// <summary>
    /// Base class for all ViewModels implementing INotifyPropertyChanged
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises PropertyChanged event for a property
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets a property value and raises PropertyChanged if value changed
        /// </summary>
        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Gets a property value with lazy initialization
        /// </summary>
        protected T GetProperty<T>(ref T? backingField, Func<T> defaultValueFactory, [CallerMemberName] string? propertyName = null) where T : class
        {
            if (backingField == null)
            {
                backingField = defaultValueFactory();
                OnPropertyChanged(propertyName);
            }
            return backingField;
        }
    }
}
