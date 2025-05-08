// <copyright file="WishlistPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View.Pages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using WorkoutApp.Models;
    using WorkoutApp.ViewModel;

    /// <summary>
    /// A page that displays the user's wishlist.
    /// </summary>
    public sealed partial class WishlistPage : Page
    {
        private readonly WishlistViewModel wishlistViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistPage"/> class.
        /// </summary>
        public WishlistPage()
        {
            this.InitializeComponent();
            this.wishlistViewModel = new WishlistViewModel();
        }

        /// <summary>
        /// Called when the page is navigated to.
        /// </summary>
        /// <param name="e">Event data that provides information about the navigation.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                await this.LoadProducts();
            }
            catch (System.Exception ex)
            {
                // Handle exceptions, e.g., show a message to the user
                System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the products in the wishlist.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task LoadProducts()
        {
            IEnumerable<WishlistItem> products = await this.wishlistViewModel.GetAllProductsFromWishlistAsync();
            this.WishlistItemListControl.SetProducts(products);
        }

        private void VerticalWishlistItemListControl_WishlistItemClicked(object sender, int productID)
        {
            MainWindow.AppFrame.Navigate(typeof(ProductDetailPage), productID);
        }

        private async void VerticalWishlistItemListControl_WishlistItemRemoved(object sender, int wishlistItemID)
        {
            await this.wishlistViewModel.RemoveProductFromWishlist(wishlistItemID);
            await this.LoadProducts();
        }
    }
}
