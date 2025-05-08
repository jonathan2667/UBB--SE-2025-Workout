// <copyright file="VerticalCartItemListComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.UI.Xaml.Controls;
    using WorkoutApp.Models;

    /// <summary>
    /// A reusable component that displays a vertical list of products.
    /// </summary>
    public sealed partial class VerticalRemovableProductListComponent : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalRemovableProductListComponent"/> class.
        /// </summary>
        public VerticalRemovableProductListComponent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Occurs when a product is clicked.
        /// </summary>
        public event EventHandler<int>? CartItemClicked;

        /// <summary>
        /// Occurs when a product is requested to be removed.
        /// </summary>
        public event EventHandler<int>? CartItemRemoved;

        /// <summary>
        /// Gets or sets the list of products to display.
        /// </summary>
        public IEnumerable<CartItem> CartItemList { get; set; } = new List<CartItem>();

        /// <summary>
        /// Sets the product list and refreshes the view.
        /// </summary>
        /// <param name="cartItems">The list of products to display.</param>
        public void SetProducts(IEnumerable<CartItem> cartItems)
        {
            this.CartItemList = cartItems;
            this.ProductListView.ItemsSource = this.CartItemList;
        }

        /// <summary>
        /// Handles click events on product items.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the item click event.</param>
        public void ProductList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is CartItem cartItem && cartItem.Product.ID.HasValue)
            {
                this.CartItemClicked?.Invoke(this, cartItem.Product.ID.Value);
            }
        }

        /// <summary>
        /// Handles the removal confirmation and triggers removal if confirmed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void RemoveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Confirm Removal",
                Content = "Are you sure you want to remove this item from your cart?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot,
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (sender is Button button && button.Tag is int cartItemId)
                {
                    this.CartItemRemoved?.Invoke(this, cartItemId);
                }
            }
        }
    }
}
