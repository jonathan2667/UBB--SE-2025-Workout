// <copyright file="CategoryFilter.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.Views.Shop.Components
{
    using System;
    using System.Configuration;
    using global::Workout.Core.Models;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob.Proxy;
    using NeoIsisJob.ViewModels.Shop;
    using WorkoutApp.ViewModel;

    /// <summary>
    /// A filter component for selecting a product category.
    /// </summary>
    public sealed partial class CategoryFilter : UserControl
    {
        private readonly CategoryFilterViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryFilter"/> class.
        /// </summary>
        public CategoryFilter()
        {
            this.InitializeComponent();

            this.viewModel = new CategoryFilterViewModel();
            this.DataContext = this.viewModel;

            this.Loaded += (_, __) =>
            {
                this.DispatcherQueue.TryEnqueue(async () =>
                {
                    await this.viewModel.LoadCategoriesAsync();
                });
            };
        }

        /// <summary>
        /// Occurs when the selected category changes.
        /// </summary>
        public event EventHandler<int>? CategoryChanged;

        /// <summary>
        /// Resets the selected category filter.
        /// </summary>
        public void ResetFilter()
        {
            this.CategoryComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Handles selection change in the category combo box.
        /// </summary>
        private void CategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is CategoryModel selectedCategory && selectedCategory.ID.HasValue)
            {
                this.CategoryChanged?.Invoke(this, selectedCategory.ID.Value);
            }
        }
    }
}
