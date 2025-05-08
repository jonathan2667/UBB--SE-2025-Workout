// <copyright file="AddProductFlyout.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Windows.Media.Capture;
    using WorkoutApp.Data.Database;
    using WorkoutApp.Repository;
    using WorkoutApp.Service;
    using WorkoutApp.ViewModel;

    /// <summary>
    /// Represents the flyout control for adding a product in the WorkoutApp.
    /// </summary>
    public sealed partial class AddProductFlyout : UserControl
    {
        private readonly AddProductViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFlyout"/> class.
        /// </summary>
        /// <param name="productService">The service for managing products.</param>
        /// <param name="categoryService">The service for managing categories.</param>
        public AddProductFlyout(ProductService productService, CategoryService categoryService)
        {
            this.InitializeComponent();

            this.viewModel = new AddProductViewModel(productService);
            this.DataContext = this.viewModel;

            _ = this.LoadCategories(categoryService);
        }

        /// <summary>
        /// Loads the categories asynchronously into the view model.
        /// </summary>
        /// <param name="categoryService">The service for managing categories.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task LoadCategories(CategoryService categoryService)
        {
            await this.viewModel.LoadCategoriesAsync(categoryService);
        }

        /// <summary>
        /// Handles the Add Product button click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void OnAddProductClick(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is AddProductViewModel vm)
            {
                bool result = await this.viewModel.AddProductAsync();
                var dialog = new ContentDialog
                {
                    Title = result ? "Success" : "Error",
                    Content = result
                        ? "Product was added successfully."
                        : this.viewModel.ValidationMessage ?? "There was an error adding the product. Check debug logs for details.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot,
                };

                await dialog.ShowAsync();
                MainWindow.AppFrame?.Navigate(typeof(MainPage));
            }

            System.Diagnostics.Debug.WriteLine("[AddProductFlyout] Add Product button clicked.");
        }
    }
}
