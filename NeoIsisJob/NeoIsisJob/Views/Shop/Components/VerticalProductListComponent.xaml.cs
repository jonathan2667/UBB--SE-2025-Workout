// <copyright file="VerticalProductListComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.Views.Shop.Components
{
    using System;
    using System.Collections.Generic;
    using global::Workout.Core.Models;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// A reusable component that displays a vertical list of products.
    /// </summary>
    public sealed partial class VerticalProductListComponent : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalProductListComponent"/> class.
        /// </summary>
        public VerticalProductListComponent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Occurs when a product is clicked.
        /// </summary>
        public event EventHandler<int>? ProductClicked;

        /// <summary>
        /// Gets or sets the list of products to display.
        /// </summary>
        public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();

        /// <summary>
        /// Sets the product list and refreshes the view.
        /// </summary>
        /// <param name="products">The list of products to display.</param>
        public void SetProducts(IEnumerable<ProductModel> products)
        {
            this.ProductList = products;
            this.ProductListView.ItemsSource = this.ProductList;
        }

        /// <summary>
        /// Handles click events on product items.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the item click event.</param>
        public void ProductList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ProductModel product)
            {
                this.ProductClicked?.Invoke(this, product.ID);
            }
        }
    }
}
