using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using NeoIsisJob.ViewModels.Shop;
using Workout.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeoIsisJob.Views.Nutrition;

namespace NeoIsisJob.Views.Shop.Pages
{
    public sealed partial class FavouriteMealsPage : Page
    {
        private readonly FavouriteMealsViewModel favouriteMealsViewModel;

        public FavouriteMealsPage()
        {
            this.InitializeComponent();
            this.favouriteMealsViewModel = new FavouriteMealsViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await this.LoadFavouriteMeals();
        }

        private async Task LoadFavouriteMeals()
        {
            IEnumerable<UserFavoriteMealModel> meals = await this.favouriteMealsViewModel.GetAllFavouriteMealsAsync();
            this.FavoriteMealListControl.SetMeals(meals);
        }

        private void FavoriteMealListControl_FavoriteMealItemClicked(object sender, int mealID)
        {
            // TODO: Navigate to meal detail page if needed
        }

        private async void FavoriteMealListControl_FavoriteMealItemRemoved(object sender, int mealID)
        {
            await this.favouriteMealsViewModel.RemoveMealFromFavourites(mealID);
            await this.LoadFavouriteMeals();
        }

        // Navigation methods
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
            this.Frame.Navigate(typeof(ClassPage));
        }

        public void GoToRankingPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RankingPage));
        }

        public void GoToShopHomePage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage)); // Shop MainPage
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
            this.Frame.Navigate(typeof(NutritionPage));
        }

        public void GoToFavouriteMealsPage_Tap(object sender, RoutedEventArgs e)
        {
            // Already on this page
        }
    }
}