using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Workout.Core.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using Workout.Core.Utils.Filters;

namespace NeoIsisJob.Views.Nutrition.Components
{
    public sealed partial class MealListComponent : UserControl
    {
        private readonly ViewModels.Nutrition.NutritionViewModel viewModel;
        public ObservableCollection<MealModel> Meals { get; private set; }
        private MealFilter _currentFilter;

        public event EventHandler<MealModel> MealClicked;
        public event EventHandler<MealModel> MealDeleted;
        public event EventHandler<MealModel> MealLiked;

        public MealListComponent()
        {
            this.InitializeComponent();
            this.viewModel = new ViewModels.Nutrition.NutritionViewModel();
            this.Meals = new ObservableCollection<MealModel>();
            this.MealListView.ItemsSource = this.Meals;
            this._currentFilter = new MealFilter();
            this.LoadMeals();
        }

        private async void LoadMeals()
        {
            await LoadMealsWithFilter(_currentFilter);
        }

        private async Task LoadMealsWithFilter(MealFilter filter)
        {
            try
            {
                var meals = await this.viewModel.GetFilteredMealsAsync(filter);
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

        public async void ApplyFilter(MealFilter filter)
        {
            _currentFilter = filter ?? new MealFilter();
            await LoadMealsWithFilter(_currentFilter);
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