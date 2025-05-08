// <copyright file="VerticalWishlistItemListComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.Views.Shop.Components
{
    using System;
    using System.Collections.Generic;
    using global::Workout.Core.Models;
    using Microsoft.UI.Xaml.Controls;

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
        public IEnumerable<WishlistItemModel> WishlistItemList { get; set; } = new List<WishlistItemModel>();

        /// <summary>
        /// Sets the product list and refreshes the view.
        /// </summary>
        /// <param name="wishlistItems">The list of wishlist items to display.</param>
        public void SetProducts(IEnumerable<WishlistItemModel> wishlistItems)
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
            if (e.ClickedItem is WishlistItemModel wishlistItem)
            {
                this.WishlistItemClicked?.Invoke(this, wishlistItem.ProductID);
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
