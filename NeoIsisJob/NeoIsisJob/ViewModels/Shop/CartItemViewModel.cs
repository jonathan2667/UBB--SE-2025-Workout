// <copyright file="CartItemViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.ViewModel
{
    using System.ComponentModel;

    /// <summary>
    /// ViewModel for displaying cart item information in the UI.
    /// </summary>
    public class CartItemViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemViewModel"/> class.
        /// </summary>
        public CartItemViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemViewModel"/> class with details.
        /// </summary>
        /// <param name="productName">The product name.</param>
        /// <param name="imageSource">The image source path or URL.</param>
        /// <param name="price">The unit price.</param>
        /// <param name="quantity">The quantity.</param>
        public CartItemViewModel(string productName, string imageSource, double price, int quantity)
        {
            this.ProductName = productName;
            this.Price = $"${price:0.##}";
            this.ImageSource = imageSource;
            this.Quantity = quantity.ToString();
            this.TotalPrice = $"${price * quantity:0.##}";
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        required public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the price as a formatted string.
        /// </summary>
        required public string Price { get; set; }

        /// <summary>
        /// Gets or sets the image source path or URL.
        /// </summary>
        required public string ImageSource { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        required public string Quantity { get; set; }

        /// <summary>
        /// Gets or sets the total price (unit price × quantity).
        /// </summary>
        required public string TotalPrice { get; set; }

        /// <summary>
        /// Notifies UI of property change.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
