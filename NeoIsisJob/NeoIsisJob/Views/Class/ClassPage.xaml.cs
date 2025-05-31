using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
// using NeoIsisJob.Models;
using NeoIsisJob.ViewModels.Classes;
using NeoIsisJob.Views.Shop.Pages;
using NeoIsisJob.Views.Nutrition;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace NeoIsisJob.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClassPage : Page
    {
        private ClassesViewModel classesViewModel;
        public ClassesViewModel ViewModel { get; }

        public ClassPage()
        {
            this.InitializeComponent();
            ViewModel = new ClassesViewModel();
            this.DataContext = ViewModel;
        }

        public void GoToMainPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        public void GoToWorkoutPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WorkoutPage));
        }

        public void GoToCalendarPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CalendarPage));
        }

        public void GoToClassPage_Tap(object sender, RoutedEventArgs e)
        {
            // this.Frame.Navigate(typeof(ClassPage));
        }

        public void GoToShopHomePage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NeoIsisJob.Views.Shop.Pages.MainPage));
        }

        public void GoToWishlistPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WishlistPage));
        }

        public void GoToCartPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CartPage));
        }

        public void GoToRankingPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RankingPage));
        }

        public void GoToNutritionPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NutritionPage));
        }

    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isVisible = (bool)value;
            bool invert = parameter?.ToString()?.ToLower() == "inverse";
            if (invert)
            {
                isVisible = !isVisible;
            }
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
