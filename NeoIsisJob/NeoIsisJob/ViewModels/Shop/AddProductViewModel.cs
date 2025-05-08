// <copyright file="AddProductViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using WorkoutApp.Models;
    using WorkoutApp.Service;

    /// <summary>
    /// ViewModel for adding a new product.
    /// </summary>
    public class AddProductViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The service used for product operations.
        /// </summary>
        private readonly ProductService productService;

        /// <summary>
        /// Backing field for validation message.
        /// </summary>
        private string validationMessage = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductViewModel"/> class.
        /// </summary>
        /// <param name="productService">The product service used to add the product.</param>
        public AddProductViewModel(ProductService productService)
        {
            this.productService = productService;
            this.Categories = new ObservableCollection<Category>();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the list of available categories.
        /// </summary>
        public ObservableCollection<Category> Categories { get; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product price as a string.
        /// </summary>
        public string Price { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stock quantity as a string.
        /// </summary>
        public string Stock { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product size.
        /// </summary>
        public string Size { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product color.
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product photo URL.
        /// </summary>
        public string PhotoURL { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the selected category.
        /// </summary>
        public Category? SelectedCategory { get; set; }

        /// <summary>
        /// Gets or sets the validation message for the UI.
        /// </summary>
        public string ValidationMessage
        {
            get => this.validationMessage;
            set => this.SetProperty(ref this.validationMessage, value);
        }

        /// <summary>
        /// Loads product categories asynchronously.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadCategoriesAsync(CategoryService categoryService)
        {
            var categories = await categoryService.GetAllAsync();
            this.Categories.Clear();
            foreach (var category in categories)
            {
                this.Categories.Add(category);
            }
        }

        /// <summary>
        /// Attempts to create a new product.
        /// </summary>
        /// <returns><c>true</c> if successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> AddProductAsync()
        {
            if (!this.IsValid(out string? validationMessage))
            {
                this.ValidationMessage = validationMessage ?? "Invalid input.";
                return false;
            }

            this.ValidationMessage = string.Empty;

            if (!decimal.TryParse(this.Price, out decimal parsedPrice))
            {
                this.ValidationMessage = "Price must be a valid decimal number.";
                return false;
            }

            if (!int.TryParse(this.Stock, out int parsedStock))
            {
                this.ValidationMessage = "Stock must be a valid integer.";
                return false;
            }

            var newProduct = new Product(
                id: null,
                name: this.Name,
                price: parsedPrice,
                stock: parsedStock,
                category: this.SelectedCategory!,
                size: this.Size,
                color: this.Color,
                description: this.Description,
                photoURL: this.PhotoURL);

            try
            {
                var createdProduct = await this.productService.CreateAsync(newProduct);
                System.Diagnostics.Debug.WriteLine($"[AddProductViewModel] Product created with ID: {createdProduct.ID}");
                return true;
            }
            catch (Exception ex)
            {
                this.ValidationMessage = $"Error creating product: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Notifies listeners that a property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the property and notifies listeners if the value changes.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="storage">The backing field.</param>
        /// <param name="value">The new value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns><c>true</c> if changed; otherwise, <c>false</c>.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Validates the input form data.
        /// </summary>
        /// <param name="error">The output error message if invalid.</param>
        /// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
        private bool IsValid(out string? error)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                error = "Name is required.";
                return false;
            }

            if (!decimal.TryParse(this.Price, out decimal parsedPrice) || parsedPrice < 0)
            {
                error = "Price must be a valid positive number.";
                return false;
            }

            if (!int.TryParse(this.Stock, out int parsedStock) || parsedStock < 0)
            {
                error = "Stock must be a valid positive integer.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.Size))
            {
                error = "Size is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.Color))
            {
                error = "Color is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.Description))
            {
                error = "Description is required.";
                return false;
            }

            if (this.SelectedCategory is null)
            {
                error = "Please select a category.";
                return false;
            }

            error = null;
            return true;
        }
    }
}
