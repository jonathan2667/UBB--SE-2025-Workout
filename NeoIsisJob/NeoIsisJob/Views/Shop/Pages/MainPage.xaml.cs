// <copyright file="MainPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using WorkoutApp.Models;
    using WorkoutApp.ViewModel;

    /// <summary>
    /// A page that displays the main list of products and allows category filtering.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel mainPageViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.mainPageViewModel = new MainPageViewModel();
        }

        /// <inheritdoc/>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await this.LoadProducts();
        }

        private async Task LoadProducts()
        {
            IEnumerable<Product> products = await this.mainPageViewModel.GetAllProductsAsync();
            this.ProductListViewControl.SetProducts(products);
        }

        private void VerticalProductListControl_ProductClicked(object sender, int productID)
        {
            if (MainWindow.AppFrame != null)
            {
                MainWindow.AppFrame.Navigate(typeof(ProductDetailPage), productID);
            }
            else
            {
                // Handle the case where AppFrame is null, e.g., log an error or show a message.
                throw new InvalidOperationException("Navigation frame is not initialized.");
            }
        }

        private async void CategorySelector_SelectionChanged(object sender, int selectedCategoryID)
        {
            this.mainPageViewModel.SetSelectedCategoryID(selectedCategoryID);
            await this.LoadProducts();
        }

        private async void ColorSelector_SelectionChanged(object sender, string selectedColor)
        {
            this.mainPageViewModel.SetSelectedColor(selectedColor);
            await this.LoadProducts();
        }

        private async void SizeSelector_SelectionChanged(object sender, string selectedSize)
        {
            this.mainPageViewModel.SetSelectedSize(selectedSize);
            await this.LoadProducts();
        }

        private async void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            this.ResetFilters();
            this.mainPageViewModel.ResetFilters();
            await this.LoadProducts();
        }

        private void ResetFilters()
        {
            this.CategoryFilterControl.ResetFilter();
            this.ColorFilterControl.ResetFilter();
            this.SizeFilterControl.ResetFilter();
        }

        private void BuildCategoryFilters(int categoryId)
        {
            /*
            while (FilterOptionsPanel.Children.Count > 2)
            {
                FilterOptionsPanel.Children.RemoveAt(2);
            }

            switch (categoryId)
            {
                case 1:
                    AddFilterCombo("Size", new List<string> { "S", "M", "L", "XL" }, categoryId);
                    AddFilterCombo("Color", new List<string> { "Black", "Red", "Blue", "White" }, categoryId);
                    break;

                case 2:
                    AddFilterCombo("Size", new List<string> { "20g", "100g", "250g", "500g", "1kg" }, categoryId);
                    break;

                case 3:
                    // Future use.
                    break;
            }

            var clearButton = new Button
            {
                Content = "Clear Filters",
                Margin = new Thickness(0, 20, 0, 0),
                Width = 140,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            clearButton.Click += (s, e) =>
            {
                ProductsGridView.ItemsSource = allProducts;
                FilterOptionsPanel.Visibility = Visibility.Collapsed;
            };

            FilterOptionsPanel.Children.Add(clearButton);
            */
        }

        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string searchTerm = this.SearchBoxControl.Text;
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    this.mainPageViewModel.SetSearchTerm(searchTerm);
                    await this.LoadProducts();
                }
            }
        }
    }
}
