using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Workout.Core.Models;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace NeoIsisJob.Views.Nutrition.Components
{
    public sealed partial class MealListComponent : UserControl
    {
        private readonly ViewModels.Nutrition.NutritionViewModel viewModel;
        public ObservableCollection<MealModel> Meals { get; private set; }

        public event EventHandler<MealModel> MealClicked;
        public event EventHandler<MealModel> MealDeleted;
        public event EventHandler<MealModel> MealLiked;

        public MealListComponent()
        {
            this.InitializeComponent();
            this.viewModel = new ViewModels.Nutrition.NutritionViewModel();
            this.Meals = new ObservableCollection<MealModel>();
            this.MealListView.ItemsSource = this.Meals;
            this.LoadMeals();
        }

        private async void LoadMeals()
        {
            try
            {
                var meals = await this.viewModel.GetAllMealsAsync();
                this.Meals.Clear();
                foreach (var meal in meals)
                {
                    this.Meals.Add(meal);
                }
            }
            catch (Exception ex)
            {
                // Handle error appropriately
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Failed to load meals. Please try again later.",
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
            }
        }

        private void MealListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is MealModel meal)
            {
                MealClicked?.Invoke(this, meal);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is MealModel meal)
            {
                try
                {
                    bool success = await this.viewModel.DeleteMealAsync(meal.Id);
                    if (success)
                    {
                        this.Meals.Remove(meal);
                        MealDeleted?.Invoke(this, meal);
                    }
                }
                catch (Exception ex)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Failed to delete meal. Please try again later.",
                        CloseButtonText = "OK"
                    };
                    await dialog.ShowAsync();
                }
            }
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is MealModel meal)
            {
                MealLiked?.Invoke(this, meal);
            }
        }

        public void RefreshMeals()
        {
            this.LoadMeals();
        }
    }
} 