// <copyright file="VerticalProductListComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.View.Components
{
    using System;
    using System.Collections.Generic;
    using Workout.Core.Models;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// The vertical product list component.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerticalProductListComponent : ContentView
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
            if (e.ClickedItem is ProductModel product && product.ID.HasValue)
            {
                this.ProductClicked?.Invoke(this, product.ID.Value);
            }
        }
    }
}
