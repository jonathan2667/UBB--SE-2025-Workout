using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NeoIsisJob.Views.Shop.Pages;
using Workout.Core.Models;
using NeoIsisJob.ViewModels.Shop;
using System;

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

        private void MealList_MealClicked(object sender, MealModel meal)
        {
            // Handle meal click - could navigate to detail view
        }

        private void MealList_MealDeleted(object sender, MealModel meal)
        {
            // Meal was already removed from the list in the component
        }

        private void AddMealButton_AddMealCompleted(object sender, RoutedEventArgs e)
        {
            // Refresh the meal list when a new meal is added
            MealList.RefreshMeals();
        }

        private async void MealList_MealLiked(object sender, MealModel meal)
        {
            var vm = new FavouriteMealsViewModel();
            await vm.AddMealToFavourites(meal);
            var dialog = new ContentDialog
            {
                Title = "Favourited!",
                Content = $"{meal.Name} was added to your favourite meals.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
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

        public void GoToFavouriteMealsPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NeoIsisJob.Views.Shop.Pages.FavouriteMealsPage));
        }
    }
}
