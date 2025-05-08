// <copyright file="VerticalWishlistItemListComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.UI.Xaml.Controls;
    using WorkoutApp.Models;

    /// <summary>
    /// Component that displays a vertical list of wishlist items.
    /// </summary>
    public sealed partial class VerticalWishlistItemListComponent : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalWishlistItemListComponent"/> class.
        /// </summary>
        public VerticalWishlistItemListComponent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Occurs when a wishlist item is clicked.
        /// </summary>
        public event EventHandler<int>? WishlistItemClicked;

        /// <summary>
        /// Occurs when a wishlist item is requested to be removed.
        /// </summary>
        public event EventHandler<int>? WishlistItemRemoved;

        /// <summary>
        /// Gets or sets the list of wishlist items to display.
        /// </summary>
        public IEnumerable<WishlistItem> WishlistItemList { get; set; } = new List<WishlistItem>();

        /// <summary>
        /// Sets the product list and refreshes the view.
        /// </summary>
        /// <param name="wishlistItems">The list of wishlist items to display.</param>
        public void SetProducts(IEnumerable<WishlistItem> wishlistItems)
        {
            this.WishlistItemList = wishlistItems;
            this.ProductListView.ItemsSource = this.WishlistItemList;
        }

        /// <summary>
        /// Handles item click events in the product list.
        /// </summary>
        /// <param name="sender">The source of the event (the control that was clicked).</param>
        /// <param name="e">The event data associated with the item click.</param>
        public void ProductList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is WishlistItem wishlistItem && wishlistItem.Product.ID.HasValue)
            {
                this.WishlistItemClicked?.Invoke(this, wishlistItem.Product.ID.Value);
            }
        }

        /// <summary>
        /// Handles removal button click and prompts confirmation dialog.
        /// </summary>
        private async void RemoveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Confirm Removal",
                Content = "Are you sure you want to remove this item from your wishlist?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot,
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (sender is Button button && button.Tag is int wishlistItemId)
                {
                    this.WishlistItemRemoved?.Invoke(this, wishlistItemId);
                }
            }
        }
    }
}
