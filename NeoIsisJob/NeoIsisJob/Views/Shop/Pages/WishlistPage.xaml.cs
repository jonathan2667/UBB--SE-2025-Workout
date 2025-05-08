// <copyright file="WishlistPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.View.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workout.Core.Models;
    using Workout.Core.ViewModel;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// The wishlist page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WishlistPage : ContentPage
    {
        private readonly WishlistViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistPage"/> class.
        /// </summary>
        public WishlistPage()
        {
            this.InitializeComponent();
            this.viewModel = new WishlistViewModel();
            this.BindingContext = this.viewModel;
            this.LoadWishlistItems();
        }

        /// <summary>
        /// Loads the wishlist items.
        /// </summary>
        private async void LoadWishlistItems()
        {
            try
            {
                IEnumerable<WishlistItemModel> wishlistItems = await this.viewModel.GetAllProductsFromWishlistAsync();
                this.WishlistItemsListView.ItemsSource = wishlistItems;
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Error", $"Failed to load wishlist items: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Handles the remove button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnRemoveButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is WishlistItemModel item)
            {
                try
                {
                    bool success = await this.viewModel.RemoveProductFromWishlist(item.ID);
                    if (success)
                    {
                        this.LoadWishlistItems();
                    }
                    else
                    {
                        await this.DisplayAlert("Error", "Failed to remove item from wishlist", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await this.DisplayAlert("Error", $"Failed to remove item: {ex.Message}", "OK");
                }
            }
        }

        /// <summary>
        /// Handles the add to cart button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnAddToCartButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is WishlistItemModel item)
            {
                try
                {
                    var cartViewModel = new CartViewModel();
                    await cartViewModel.AddToCartAsync(item.Product, item.Quantity);
                    await this.DisplayAlert("Success", "Item added to cart", "OK");
                }
                catch (Exception ex)
                {
                    await this.DisplayAlert("Error", $"Failed to add item to cart: {ex.Message}", "OK");
                }
            }
        }
    }
}
