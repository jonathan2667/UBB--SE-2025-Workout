// <copyright file="AddMealFlyout.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.Views.Nutrition.Components
{
    using System;
    using System.Linq;
    using global::Workout.Core.Models;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob.ViewModels.Nutrition;

    /// <summary>
    /// A flyout UI component for creating and adding a new meal.
    /// </summary>
    public sealed partial class AddMealFlyout : UserControl
    {
        /// <summary>
        /// Event that is raised when a meal is successfully added.
        /// </summary>
        public event RoutedEventHandler MealAdded;

        /// <summary>
        /// The view model that manages state and logic for adding a meal.
        /// </summary>
        private readonly AddMealViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddMealFlyout"/> class.
        /// </summary>
        public AddMealFlyout()
        {
            this.InitializeComponent();
            this.viewModel = new AddMealViewModel();
            this.DataContext = this.viewModel;
            this.LoadIngredients();
        }

        /// <summary>
        /// Loads the list of available ingredients into the view model.
        /// </summary>
        private void LoadIngredients()
        {
            this.viewModel.LoadIngredients();
        }

        /// <summary>
        /// Handles the click event for adding a meal. Validates input and shows a success/error dialog.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private async void OnAddMealClick(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is AddMealViewModel vm)
            {
                this.viewModel.SelectedIngredients = this.IngredientsListBox.SelectedItems
                    .Cast<IngredientModel>()
                    .ToList();

                bool result = await this.viewModel.AddMealAsync();

                var dialog = new ContentDialog
                {
                    Title = result ? "Success" : "Error",
                    Content = result
                        ? "Meal was added successfully."
                        : this.viewModel.ValidationMessage ?? "There was an error adding the meal. Check debug logs for details.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot,
                };

                await dialog.ShowAsync();

                if (result)
                {
                    MealAdded?.Invoke(this, new RoutedEventArgs());
                }

                // Don't navigate - let the parent handle any UI updates
                System.Diagnostics.Debug.WriteLine("[AddMealFlyout] Add Meal button clicked.");
            }
        }
    }
}
