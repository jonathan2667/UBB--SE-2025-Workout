using System;
using System.Collections.Generic;
using Workout.Core.Models;
using Microsoft.UI.Xaml.Controls;

namespace NeoIsisJob.Views.Shop.Components
{
    public sealed partial class VerticalFavoriteMealItemListComponent : UserControl
    {
        public VerticalFavoriteMealItemListComponent()
        {
            this.InitializeComponent();
        }

        public event EventHandler<int>? FavoriteMealItemClicked;
        public event EventHandler<int>? FavoriteMealItemRemoved;

        public IEnumerable<UserFavoriteMealModel> FavoriteMealList { get; set; } = new List<UserFavoriteMealModel>();

        public void SetMeals(IEnumerable<UserFavoriteMealModel> favoriteMeals)
        {
            this.FavoriteMealList = favoriteMeals;
            this.MealListView.ItemsSource = this.FavoriteMealList;
        }

        private void MealList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is UserFavoriteMealModel meal)
            {
                FavoriteMealItemClicked?.Invoke(this, meal.MealID);
            }
        }

        private void RemoveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int mealId)
            {
                FavoriteMealItemRemoved?.Invoke(this, mealId);
            }
        }
    }
} 