using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NeoIsisJob.Views.Shop.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NeoIsisJob.Views.Nutrition
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NutritionPage : Page
    {
        public NutritionPage()
        {
            this.InitializeComponent();
        }

        // Navigation methods
        public void GoToMainPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NeoIsisJob.Views.MainPage));
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
            this.Frame.Navigate(typeof(ClassPage));
        }

        public void GoToRankingPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RankingPage));
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

        public void GoToNutritionPage_Tap(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(NutritionPage));
        }
    }
}
