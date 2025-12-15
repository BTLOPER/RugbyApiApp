using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RugbyApiApp.MAUI.Controls
{
    /// <summary>
    /// A custom star rating control that displays 5 stars for rating videos (1-5)
    /// </summary>
    public partial class StarRatingControl : UserControl
    {
        private int _currentRating = 0;
        private int _hoverRating = 0;

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register(
                nameof(Rating),
                typeof(int),
                typeof(StarRatingControl),
                new PropertyMetadata(0, OnRatingChanged));

        public int Rating
        {
            get => (int)GetValue(RatingProperty);
            set => SetValue(RatingProperty, Math.Max(0, Math.Min(5, value)));
        }

        public static readonly RoutedEvent RatingChangedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(RatingChanged),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(StarRatingControl));

        public event RoutedEventHandler RatingChanged
        {
            add => AddHandler(RatingChangedEvent, value);
            remove => RemoveHandler(RatingChangedEvent, value);
        }

        public StarRatingControl()
        {
            InitializeComponent();
            UpdateStarDisplay(0);
            
            // Add mouse move/leave handlers to all stars
            foreach (var star in new[] { Star1, Star2, Star3, Star4, Star5 })
            {
                star.MouseEnter += (s, e) =>
                {
                    if (int.TryParse(((Button)s).Tag?.ToString() ?? "0", out int rating))
                    {
                        _hoverRating = rating;
                        UpdateStarDisplay(rating);
                    }
                };

                star.MouseLeave += (s, e) =>
                {
                    _hoverRating = 0;
                    UpdateStarDisplay(_currentRating);
                };
            }
        }

        private static void OnRatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StarRatingControl control)
            {
                int newRating = (int)e.NewValue;
                control._currentRating = newRating;
                control.UpdateStarDisplay(newRating);
                control.RaiseEvent(new RoutedEventArgs(RatingChangedEvent));
            }
        }

        private void OnStarClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag?.ToString() ?? "0", out int rating))
            {
                Rating = rating;
                _currentRating = rating;
            }
        }

        private void UpdateStarDisplay(int filledStars)
        {
            var stars = new[] { Star1, Star2, Star3, Star4, Star5 };

            for (int i = 0; i < stars.Length; i++)
            {
                // Filled star (yellow)
                if (i < filledStars)
                {
                    stars[i].Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 184, 0)); // #FFB800
                    stars[i].Content = "★";
                }
                // Empty star (light gray)
                else
                {
                    stars[i].Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 200, 200)); // #C8C8C8
                    stars[i].Content = "★";
                }
            }
        }
    }
}
