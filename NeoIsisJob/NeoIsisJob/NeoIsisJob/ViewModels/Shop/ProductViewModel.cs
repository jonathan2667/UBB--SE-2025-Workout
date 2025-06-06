// <copyright file="ProductViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.ViewModels.Shop // Using the singular 'ViewModel' namespace as per your structure
{
    using global::Workout.Core.Models;
    using global::Workout.Core.Utils.Filters;
    using NeoIsisJob.Proxy;
    using System;
    using System.Collections.Generic; // Required for EqualityComparer
    using System.Collections.ObjectModel; // Required for ObservableCollection
    using System.ComponentModel; // Required for INotifyPropertyChanged
    using System.Diagnostics; // Required for Debug.WriteLine
    using System.Globalization; // Required for CultureInfo
    using System.Runtime.CompilerServices; // Required for CallerMemberName
    using System.Threading.Tasks;
    using System.Windows.Input; // Required for ICommand


    /// <summary>
    /// ViewModel for a single product, designed for UI data binding.
    /// Supports loading, displaying, updating, and deleting a product using Commands.
    /// </summary>
    public class ProductViewModel : INotifyPropertyChanged // Implement INotifyPropertyChanged for UI updates
    {
        // The type is ProductService because GetFilteredAsync is not in IService<Product>
        private readonly ProductServiceProxy productService;
        private int productId; // Store the product ID internally once loaded
        private ProductModel? product; // Hold the underlying Product model

        // Properties to expose Product data for binding
        private string name = "Loading...";
        private decimal price = 0.00m;
        private int stock = 0;
        private int categoryId = 0; // ViewModel property to hold Category ID
        private string categoryName = "Loading..."; // ViewModel property to hold Category Name
        private string size = "N/A";
        private string color = "N/A";
        private string description = "Loading description...";
        private string? photoUrl = null; // Use string? for nullable PhotoURL

        // New property for related products
        private ObservableCollection<ProductModel> relatedProducts = new ObservableCollection<ProductModel>();

        /// <summary>
        /// Gets the command for saving product changes.
        /// </summary>
        public ICommand? SaveCommand { get; }

        /// <summary>
        /// Gets the command for cancelling product editing.
        /// </summary>
        public ICommand? CancelEditCommand { get; }

        /// <summary>
        /// Gets the command for deleting the product.
        /// </summary>
        public ICommand? DeleteCommand { get; }

        // Property to track if the update modal is currently open (useful for ViewModel state)
        private bool isUpdateModalOpen = false;

        /// <summary>
        /// Gets or sets a value indicating whether the update modal is open.
        /// </summary>
        public bool IsUpdateModalOpen
        {
            get => this.isUpdateModalOpen;
            set => this.SetProperty(ref this.isUpdateModalOpen, value); // Use SetProperty to notify UI when modal state changes
        }

        // New event to signal the View to show the update modal

        /// <summary>
        /// Event that is raised when the update modal should be shown.
        /// </summary>
        public event EventHandler? RequestShowUpdateModal;

        // Event required by INotifyPropertyChanged

        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductViewModel"/> class.
        /// This is the parameterless constructor required for XAML object element usage.
        /// It initializes dependencies with default/placeholder values.
        /// The ViewModel should be properly initialized with data using LoadProductAsync later.
        /// </summary>
        public ProductViewModel()
        {
            Debug.WriteLine("ProductViewModel parameterless constructor called.");

            // Initialize dependencies with default/placeholder values.
            this.productService = new ProductServiceProxy();

            // Initialize Commands
            this.SaveCommand = new RelayCommand(async _ => await this.ExecuteSaveAsync());
            this.CancelEditCommand = new RelayCommand(async _ => await this.ExecuteCancelEditAsync());
            this.DeleteCommand = new RelayCommand(async _ => await this.ExecuteDeleteAsync());
        }


        /// <summary>
        /// Gets the unique identifier of the product.
        /// </summary>
        public int ID => this.product?.ID ?? this.productId; // Return loaded ID or initial ID if not loaded

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value); // Use SetProperty to notify UI on change
        }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public decimal Price
        {
            get => this.price;
            set
            {
                if (this.SetProperty(ref this.price, value))
                {
                    this.OnPropertyChanged(nameof(this.FormattedPrice)); // Notify UI when Price changes
                }
            }
        }

        /// <summary>
        /// Gets the formatted price as a currency string.
        /// This property is used for UI binding since StringFormat is not supported in WinUI XAML.
        /// </summary>
        public string FormattedPrice => this.Price.ToString("C2", CultureInfo.CurrentCulture);

        /// <summary>
        /// Gets or sets the stock quantity of the product.
        /// </summary>
        public int Stock
        {
            get => this.stock;
            set => this.SetProperty(ref this.stock, value);
        }

        /// <summary>
        /// Gets or sets the category ID of the product.
        /// </summary>
        public int CategoryID
        {
            get => this.categoryId;
            set => this.SetProperty(ref this.categoryId, value);
        }

        /// <summary>
        /// Gets or sets the category name of the product.
        /// </summary>
        public string CategoryName
        {
            get => this.categoryName;
            set => this.SetProperty(ref this.categoryName, value);
        }

        /// <summary>
        /// Gets or sets the size of the product.
        /// </summary>
        public string Size
        {
            get => this.size;
            set => this.SetProperty(ref this.size, value);
        }

        /// <summary>
        /// Gets or sets the color of the product.
        /// </summary>
        public string Color
        {
            get => this.color;
            set => this.SetProperty(ref this.color, value);
        }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        public string Description
        {
            get => this.description;
            set => this.SetProperty(ref this.description, value);
        }

        /// <summary>
        /// Gets or sets the URL of the product photo.
        /// </summary>
        public string? PhotoURL
        {
            get => this.photoUrl;
            set => this.SetProperty(ref this.photoUrl, value);
        }

        private CategoryModel category;

        /// <summary>
        /// Gets the collection of related products for UI binding.
        /// </summary>
        public ObservableCollection<ProductModel> RelatedProducts
        {
            get => this.relatedProducts;
            private set => this.SetProperty(ref this.relatedProducts, value); // Use SetProperty to notify UI
        }

        // --- Methods for UI Interaction (Executed by Commands or x:Bind) ---

        /// <summary>
        /// Loads a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to load.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task LoadProductAsync(int id)
        {
            Debug.WriteLine($"[ProductViewModel] LoadProductAsync called with ID: {id}");
            try
            {
                this.productId = id;
                var loadedProduct = await this.productService.GetByIdAsync(id);
                Debug.WriteLine($"[ProductViewModel] Loaded product: {loadedProduct?.ID} - {loadedProduct?.Name}");
                if (loadedProduct == null)
                {
                    Debug.WriteLine($"[ProductViewModel] No product found with ID: {id}");
                    throw new InvalidOperationException($"No product found with ID: {id}");
                }
                this.product = loadedProduct;
                this.Name = loadedProduct.Name;
                this.Price = loadedProduct.Price;
                this.Stock = loadedProduct.Stock;
                this.CategoryID = loadedProduct.CategoryID;
                this.CategoryName = loadedProduct.Category?.Name ?? "Unknown Category";
                this.Size = loadedProduct.Size;
                this.Color = loadedProduct.Color;
                this.Description = loadedProduct.Description;
                this.PhotoURL = loadedProduct.PhotoURL;
                Debug.WriteLine($"[ProductViewModel] ViewModel properties updated: ID={this.product.ID}, Name={this.Name}");
                await this.LoadRelatedProductsAsync(loadedProduct.CategoryID, loadedProduct.ID, 4);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ProductViewModel] Error loading product: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Loads related products asynchronously based on category and exclusion.
        /// </summary>
        /// <param name="categoryId">The category ID to filter by.</param>
        /// <param name="excludeProductId">The ID of the product to exclude.</param>
        /// <param name="count">The number of related products to fetch.</param>
        private async Task LoadRelatedProductsAsync(int categoryId, int excludeProductId, int count)
        {
            Debug.WriteLine($"ProductViewModel: LoadRelatedProductsAsync called for Category ID: {categoryId}, Exclude ID: {excludeProductId}, Count: {count}"); // Added logging
            try
            {
                // Create a ProductFilter instance with the desired criteria
                var filter = new ProductFilter(categoryId, excludeProductId, count, null, null, null);

                // Call the new generic service method to get filtered products
                var related = await this.productService.GetFilteredAsync(filter);

                // Clear existing related products and add the new ones
                this.RelatedProducts.Clear();
                foreach (var p in related)
                {
                    this.RelatedProducts.Add(p);
                }
                Debug.WriteLine($"ProductViewModel: Loaded {this.RelatedProducts.Count} related products."); // Added logging
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ProductViewModel: Error loading related products for category {categoryId}: {ex}"); // Added logging
                this.RelatedProducts.Clear(); // Clear related products on error
            }
        }

        /// <summary>
        /// Executes the method to prepare for editing and signals the View to show the modal.
        /// Made public to be accessible from XAML x:Bind.
        /// </summary>
        public async Task ExecuteEnterEditModeAsync() // Made public and async for x:Bind
        {
            Debug.WriteLine("ProductViewModel: ExecuteEnterEditModeAsync called."); // Added logging
            // Ensure the product is loaded before attempting to edit
            if (this.product == null && this.ID > 0)
            {
                Debug.WriteLine("ProductViewModel: Product is null, attempting to load product before entering edit mode.");
                await this.LoadProductAsync(this.ID); // Load if not already loaded
            }

            if (this.product != null)
            {
                // Ensure ViewModel properties are up-to-date with the 'product' model before showing the modal
                // Although LoadProductAsync does this, explicitly doing it here again ensures the latest data
                // is in the properties right before the modal is requested.
                this.Name = this.product.Name;
                this.Price = this.product.Price;
                this.Stock = this.product.Stock;
                this.CategoryID = this.product.Category?.ID ?? 0;
                this.CategoryName = this.product.Category?.Name ?? "Unknown Category";
                this.Size = this.product.Size;
                this.Color = this.product.Color;
                this.Description = this.product.Description;
                this.PhotoURL = this.product.PhotoURL;
                Debug.WriteLine($"ProductViewModel: Properties confirmed before opening modal: Name={this.Name}, Price={this.Price}, Stock={this.Stock}, CategoryID={this.CategoryID}, CategoryName={this.CategoryName}"); // Added logging

                this.IsUpdateModalOpen = true; // Set ViewModel state
                this.Name = this.product.Name;
                this.Price = this.product.Price;
                this.Stock = this.product.Stock;
                this.CategoryID = this.product.Category?.ID ?? 0;
                this.CategoryName = this.product.Category?.Name ?? "Unknown Category";
                this.Size = this.product.Size;
                this.Color = this.product.Color;
                this.Description = this.product.Description;
                this.PhotoURL = this.product.PhotoURL;
                Debug.WriteLine($"ProductViewModel: Properties confirmed before opening modal: Name={this.Name}, Price={this.Price}, Stock={this.Stock}, CategoryID={this.CategoryID}, CategoryName={this.CategoryName}"); // Added logging

                this.IsUpdateModalOpen = true; // Set ViewModel state
                Debug.WriteLine("ProductViewModel: IsUpdateModalOpen set to true. Raising RequestShowUpdateModal event.");
                this.RequestShowUpdateModal?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Debug.WriteLine("ProductViewModel: Cannot enter edit mode, product is null after load attempt.");
            }
        }

        /// <summary>
        /// Executes the command to save product changes.
        /// Made public to be accessible from XAML x:Bind.
        /// </summary>
        /// <returns>A Task representing the asynchronous save operation.</returns>
        public async Task ExecuteSaveAsync() // Made public
        {
            Debug.WriteLine("ProductViewModel: ExecuteSaveAsync called."); // Added logging

            if (this.product == null)
            {
                Debug.WriteLine("ProductViewModel: ExecuteSaveAsync - No product is selected."); // Added logging
                throw new InvalidOperationException("No product is selected.");
            }

            try
            {
                // Create a new ProductModel with the current ViewModel state
                var updatedProduct = new ProductModel
                {
                    ID = this.product.ID,
                    Name = this.Name,
                    Price = this.Price,
                    Stock = this.Stock,
                    CategoryID = this.CategoryID,
                    Category = new CategoryModel { ID = this.CategoryID, Name = this.CategoryName },
                    Size = this.Size,
                    Color = this.Color,
                    Description = this.Description,
                    PhotoURL = this.PhotoURL
                };

                Debug.WriteLine($"ProductViewModel: ExecuteSaveAsync - Updating product with ID: {updatedProduct.ID}"); // Added logging

                // Call the service to update the product
                var result = await this.productService.UpdateAsync(updatedProduct);
                Debug.WriteLine($"ProductViewModel: ExecuteSaveAsync - Update completed"); // Added logging

                if (result != null)
                {
                    // Update the product field with the new state
                    this.product = result;
                    // Update ViewModel properties from the result
                    this.Name = result.Name;
                    this.Price = result.Price;
                    this.Stock = result.Stock;
                    this.CategoryID = result.CategoryID;
                    this.CategoryName = result.Category?.Name ?? "Unknown Category";
                    this.Size = result.Size;
                    this.Color = result.Color;
                    this.Description = result.Description;
                    this.PhotoURL = result.PhotoURL;
                    Debug.WriteLine($"ProductViewModel: ExecuteSaveAsync - Product updated successfully: ID={this.product.ID}, Name={this.product.Name}"); // Added logging
                }
                else
                {
                    Debug.WriteLine("ProductViewModel: ExecuteSaveAsync - Failed to update product."); // Added logging
                    throw new InvalidOperationException("Failed to update product.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ProductViewModel: ExecuteSaveAsync - Error updating product: {ex}"); // Added logging
                throw; // Re-throw to let the caller handle the error
            }
        }

        /// <summary>
        /// Executes the command/method to cancel the current editing session and reverts ViewModel properties.
        /// Called from the modal's Cancel button.
        /// Made public to be accessible from XAML x:Bind.
        /// </summary>
        /// <returns>A Task representing the asynchronous cancel operation.</returns>
        public async Task ExecuteCancelEditAsync() // Made public
        {
            Debug.WriteLine("ProductViewModel: ExecuteCancelEditAsync called. Reverting changes."); // Added logging

            // Re-load the product data from the service to discard changes
            if (this.product != null)
            {
                await this.LoadProductAsync(this.product.ID); // This will reset all ViewModel properties
                Debug.WriteLine($"ProductViewModel: Properties reverted after cancel: Name={this.Name}, Price={this.Price}, Stock={this.Stock}, CategoryID={this.CategoryID}, CategoryName={this.CategoryName}"); // Added logging
            }
            else
            {
                // If product was never loaded successfully, just exit editing
                // Reset ViewModel properties to default state
                this.Name = "Product Not Loaded";
                this.Price = 0;
                this.Stock = 0;
                this.CategoryID = 0;
                this.CategoryName = "N/A";
                this.Size = "N/A";
                this.Color = "N/A";
                this.Description = string.Empty;
                this.PhotoURL = null;
                this.RelatedProducts.Clear();
                Debug.WriteLine("ProductViewModel: Properties reset to default after cancel (product was null)."); // Added logging
            }

            this.IsUpdateModalOpen = false; // Close the modal
        }

        /// <summary>
        /// Executes the command to delete the product.
        /// Made public to be accessible from XAML x:Bind.
        /// </summary>
        /// <returns>A Task representing the asynchronous delete operation.</returns>
        public async Task ExecuteDeleteAsync() // Made public
        {
            Debug.WriteLine("ProductViewModel: ExecuteDeleteAsync called."); // Added logging at the very beginning
            if (this.product == null || this.product.ID == null)
            {
                Debug.WriteLine("ProductViewModel: Attempted to delete a product that was not loaded correctly (product or ID is null)."); // Added logging
                return; // Command finished, no action taken
            }

            // --- Placeholder for Confirmation Dialog ---
            // In a real app, you would show a confirmation dialog here.
            // For this example, we'll assume the user confirmed.
            Debug.WriteLine($"ProductViewModel: Attempting to delete product ID {this.product.ID}. (Assuming user confirmation)");


            // bool confirmed = await ShowConfirmationDialogAsync($"Are you sure you want to delete {Name}?");
            // if (!confirmed) return;

            // --- End Placeholder ---

            try
            {
                // Call the service to delete the product
                Debug.WriteLine($"ProductViewModel: Calling productService.DeleteAsync({this.product.ID})..."); // Added logging
                bool success = await this.productService.DeleteAsync(this.product.ID);
                Debug.WriteLine($"ProductViewModel: productService.DeleteAsync returned: {success}"); // Added logging

                if (success)
                {
                    Debug.WriteLine($"ProductViewModel: Product ID {this.product.ID} deleted successfully."); // Added logging

                    // --- Update UI State After Successful Deletion ---
                    // Clear ViewModel properties to visually indicate deletion
                    this.Name = "Product Deleted";
                    this.Price = 0;
                    this.Stock = 0;
                    this.CategoryID = 0;
                    this.CategoryName = string.Empty;
                    this.Size = string.Empty;
                    this.Color = string.Empty;
                    this.Description = "This product has been deleted.";
                    this.PhotoURL = null; // Clear the image
                    this.RelatedProducts.Clear(); // Clear related products
                    this.IsUpdateModalOpen = false; // Ensure modal is closed if open

                    Debug.WriteLine("ProductViewModel: ViewModel properties updated after successful deletion."); // Added logging

                    // In a real application, you would typically raise an event here
                    // (e.g., OnProductDeleted event) that the View (ProductDetailPage.xaml.cs)
                    // listens to and then handles navigation away from the page.
                    // For example: OnProductDeleted?.Invoke(this, EventArgs.Empty);
                    // --- End UI Update ---
                }
                else
                {
                    Debug.WriteLine($"ProductViewModel: Failed to delete product ID {this.product.ID}. Service reported failure."); // Added logging

                    // Handle the error (e.g., show an error message in the UI)
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ProductViewModel: Error deleting product ID {this.product.ID}: {ex}"); // Added logging

                // Handle the error (e.g., show an error message in the UI)
            }
            Debug.WriteLine("ProductViewModel: ExecuteDeleteAsync finished."); // Added logging
        }

        /// <summary>
        /// Helper method to set property value and raise PropertyChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="field">The reference to the backing field.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="propertyName">The name of the property (automatically inferred).</param>
        /// <returns>True if the value was changed, false otherwise.</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                // Debug.WriteLine($"ProductViewModel: SetProperty for {propertyName} - Value unchanged."); // Optional: Log if value is the same
                return false; // Value hasn't changed
            }

            // Debug.WriteLine($"ProductViewModel: SetProperty for {propertyName} - Value changing from '{field}' to '{value}'."); // Added logging for property changes
            field = value; // Update the backing field
            this.OnPropertyChanged(propertyName); // Notify the UI
            return true; // Value was changed
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            Debug.WriteLine($"ProductViewModel: OnPropertyChanged called for: {propertyName}"); // Added logging
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets the selected product.
        /// </summary>
        /// <returns>The selected product.</returns>
        public ProductModel GetSelectedProduct()
        {
            Debug.WriteLine($"[ProductViewModel] GetSelectedProduct called. product?.ID={this.product?.ID}, Name={this.Name}");
            if (this.product == null)
            {
                Debug.WriteLine("[ProductViewModel] GetSelectedProduct - No product is selected.");
                throw new InvalidOperationException("No product is selected.");
            }
            var currentProduct = new ProductModel
            {
                ID = this.product.ID,
                Name = this.Name,
                Price = this.Price,
                Stock = this.Stock,
                CategoryID = this.CategoryID,
                Category = new CategoryModel { ID = this.CategoryID, Name = this.CategoryName },
                Size = this.Size,
                Color = this.Color,
                Description = this.Description,
                PhotoURL = this.PhotoURL
            };
            Debug.WriteLine($"[ProductViewModel] GetSelectedProduct - Returning product with ID: {currentProduct.ID}, Name: {currentProduct.Name}");
            return currentProduct;
        }

        // --- Basic ICommand Implementation (RelayCommand) ---
        private class RelayCommand : ICommand
        {
            private readonly Action<object?> _execute;
            private readonly Func<object?, bool>? _canExecute;

            /// <summary>
            /// Occurs when changes occur that affect whether or not the command should execute.
            /// </summary>
            public event EventHandler? CanExecuteChanged;

            /// <summary>
            /// Initializes a new instance of the <see cref="RelayCommand"/> class.
            /// </summary>
            /// <param name="execute">The execution logic.</param>
            /// <param name="canExecute">The execution status logic.</param>
            public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
            {
                this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
                this._canExecute = canExecute;
            }

            /// <summary>
            /// Defines the method that determines whether the command can execute in its current state.
            /// </summary>
            /// <param name="parameter">Data used by the command.</param>
            /// <returns>True if this command can be executed; otherwise, false.</returns>
            public bool CanExecute(object? parameter)
            {
                return this._canExecute == null || this._canExecute(parameter);
            }

            /// <summary>
            /// Defines the method to be called when the command is invoked.
            /// </summary>
            /// <param name="parameter">Data used by the command.</param>
            public void Execute(object? parameter)
            {
                this._execute(parameter);
            }

            /// <summary>
            /// Raises the CanExecuteChanged event.
            /// </summary>
            public void RaiseCanExecuteChanged()
            {
                this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
