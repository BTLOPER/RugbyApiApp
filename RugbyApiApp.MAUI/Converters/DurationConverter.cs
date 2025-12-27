using System;
using System.Globalization;
using System.Windows.Data;

namespace RugbyApiApp.MAUI.Converters
{
    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan duration)
            {
                if (duration.TotalHours >= 1)
                {
                    return duration.ToString(@"hh\:mm");
                }
                else
                {
                    return duration.ToString(@"mm\:ss");
                }
            }

            if (value is int totalSeconds)
            {
                var durationSpan = TimeSpan.FromSeconds(totalSeconds);
                if (durationSpan.TotalHours >= 1)
                {
                    return durationSpan.ToString(@"hh\:mm");
                }
                else
                {
                    return durationSpan.ToString(@"mm\:ss");
                }
            }

            return "--";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
