using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RugbyApiApp.MAUI.Converters
{
    /// <summary>
    /// Converts rating and star position to appropriate color
    /// Gold for stars up to rating, grey for stars beyond
    /// </summary>
    public class RatingStarColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] = Rating (int?)
            // values[1] = Star position (int)
            
            if (values.Length < 2)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3D3D3")); // Grey

            if (!int.TryParse(values[1]?.ToString() ?? "", out int starPosition))
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3D3D3"));

            int? rating = values[0] as int?;
            
            // If no rating or star is beyond rating, use grey
            if (!rating.HasValue || starPosition > rating.Value)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3D3D3")); // Grey (#D3D3D3)

            // Star is within rating, use gold
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD700")); // Gold
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
