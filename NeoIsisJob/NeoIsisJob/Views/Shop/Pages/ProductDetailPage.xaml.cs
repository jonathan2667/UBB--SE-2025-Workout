// <copyright file="ProductDetailPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.View.Pages
{
    using System;
    using System.Configuration;
    using System.Diagnostics; // Required for Debug.WriteLine
    using System.Threading.Tasks;
    using Microsoft.UI.Dispatching; // Required for DispatcherQueue
    using Microsoft.UI.Xaml; // Required for RoutedEventArgs, FrameworkElement
    using Microsoft.UI.Xaml.Controls; // For WinUI Page, ContentDialog
    using Microsoft.UI.Xaml.Navigation; // For NavigationEventArgs
    using Workout.Core.Data.Database; // Assuming DbConnectionFactory and DbService are here
    using Workout.Core.Models;
    using Workout.Core.Repository; // Assuming ProductRepository and IRepository are here
    using Workout.Core.Service; // Assuming ProductService and IService are here
    using Workout.Core.ViewModel; // Corrected: Using the singular 'ViewModel' namespace for ProductViewModel
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// The product detail page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailPage : ContentPage
    {
        private readonly ProductViewModel viewModel;
        private ProductModel? product;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDetailPage"/> class.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        public ProductDetailPage(int productId)
        {
            this.InitializeComponent();
            this.viewModel = new ProductViewModel();
            this.BindingContext = this.viewModel;
            this.LoadProduct(productId);
        }

        /// <summary>
        /// Loads the product.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        private async void LoadProduct(int productId)
        {
            try
            {
                this.product = await this.viewModel.GetProductByIdAsync(productId);
                if (this.product != null)
                {
                    this.BindingContext = this.product;
                }
                else
                {
                    await this.DisplayAlert("Error", "Product not found", "OK");
                    await this.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Error", $"Failed to load product: {ex.Message}", "OK");
                await this.Navigation.PopAsync();
            }
        }

        /// <summary>
        /// Handles the add to cart button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnAddToCartButtonClicked(object sender, EventArgs e)
        {
            if (this.product != null)
            {
                try
                {
                    var cartViewModel = new CartViewModel();
                    await cartViewModel.AddToCartAsync(this.product, 1);
                    await this.DisplayAlert("Success", "Product added to cart", "OK");
                }
                catch (Exception ex)
                {
                    await this.DisplayAlert("Error", $"Failed to add product to cart: {ex.Message}", "OK");
                }
            }
        }

        /// <summary>
        /// Handles the add to wishlist button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnAddToWishlistButtonClicked(object sender, EventArgs e)
        {
            if (this.product != null)
            {
                try
                {
                    var wishlistViewModel = new WishlistViewModel();
                    await wishlistViewModel.AddProductToWishlist(this.product);
                    await this.DisplayAlert("Success", "Product added to wishlist", "OK");
                }
                catch (Exception ex)
                {
                    await this.DisplayAlert("Error", $"Failed to add product to wishlist: {ex.Message}", "OK");
                }
            }
        }
    }
}