// <copyright file="ProductDetailPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View // Using the 'View' namespace as in your provided code
{
    using System;
    using System.Configuration;
    using System.Diagnostics; // Required for Debug.WriteLine
    using System.Threading.Tasks;
    using Microsoft.UI.Dispatching; // Required for DispatcherQueue
    using Microsoft.UI.Xaml; // Required for RoutedEventArgs, FrameworkElement
    using Microsoft.UI.Xaml.Controls; // For WinUI Page, ContentDialog
    using Microsoft.UI.Xaml.Navigation; // For NavigationEventArgs
    using WorkoutApp.Data.Database; // Assuming DbConnectionFactory and DbService are here
    using WorkoutApp.Models;
    using WorkoutApp.Repository; // Assuming ProductRepository and IRepository are here
    using WorkoutApp.Service; // Assuming ProductService and IService are here
    using WorkoutApp.ViewModel; // Corrected: Using the singular 'ViewModel' namespace for ProductViewModel

    /// <summary>
    /// Code-behind for the ProductDetailPage.xaml.
    /// </summary>
    public sealed partial class ProductDetailPage : Page // Inherit from Page
    {
        /// <summary>
        /// Gets The ViewModel for the ProductDetailPage.
        /// </summary>
        public ProductViewModel ViewModel { get; }

        private readonly CartViewModel cartViewModel;

        private readonly WishlistViewModel wishlistViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDetailPage"/> class.
        /// </summary>
        public ProductDetailPage()
        {
            Debug.WriteLine("ProductDetailPage: Constructor called."); // Added logging
            this.InitializeComponent();

            // Initialize dependencies for the ProductService.
            // This should ideally be done via Dependency Injection in a real app,
            // but we'll new them up here for simplicity based on your structure.
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var connectionFactory = new DbConnectionFactory(connectionString);
            var dbService = new DbService(connectionFactory);
            var productRepository = new ProductRepository(dbService);
            var productService = new ProductService(productRepository);

            // Initialize the ViewModel with the necessary service
            this.ViewModel = new ProductViewModel(productService);
            Debug.WriteLine($"ProductDetailPage: ViewModel created. Initial ViewModel.ID: {this.ViewModel.ID}"); // Added logging

            // Set the DataContext of the page to the ViewModel
            // You can add other initialization logic here if needed,
            // similar to how you set the RemoveButtonText in DrinkDetailPage.
            // For example, logic based on user roles or product status.
            this.cartViewModel = new CartViewModel();
            this.wishlistViewModel = new WishlistViewModel();
            this.DataContext = this.ViewModel;

            // Subscribe to the ViewModel's events
            // ViewModel.PropertyChanged += ViewModel_PropertyChanged; // Removed as it's no longer needed for modal logic
            this.ViewModel.RequestShowUpdateModal += this.ViewModel_RequestShowUpdateModal; // Subscribe to the event that signals modal display
            Debug.WriteLine("ProductDetailPage: Subscribed to RequestShowUpdateModal."); // Updated logging
        }

        // Removed the ViewModel_PropertyChanged handler as it's no longer needed for modal logic.
        /*
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
             Debug.WriteLine($"ProductDetailPage: ViewModel.PropertyChanged called for: {e.PropertyName}"); // Added logging
             // The logic to show the modal based on IsUpdateModalOpen is moved to ViewModel_RequestShowUpdateModal
        }
        */
        /// <summary>
        /// Handles the RequestShowUpdateModal event from the ViewModel.
        /// This method is responsible for showing the ContentDialog.
        /// </summary>
        private void ViewModel_RequestShowUpdateModal(object? sender, EventArgs e)
        {
            Debug.WriteLine("ProductDetailPage: ViewModel_RequestShowUpdateModal event received."); // Added logging

            // Use DispatcherQueue.TryEnqueue to schedule the ShowAsync call
            // back onto the UI thread's dispatcher queue. This is necessary
            // to avoid potential re-entrancy issues.
            this.DispatcherQueue.TryEnqueue(async () =>
            {
                Debug.WriteLine("ProductDetailPage: DispatcherQueue Enqueue callback executing."); // Added logging inside enqueue

                // Explicitly set the DataContext of the ContentDialog's content (the UpdateProductModal)
                // This ensures the modal has the correct ViewModel instance for binding.
                // Cast Content to FrameworkElement to access DataContext
                if (this.UpdateProductContentDialog.Content is FrameworkElement modalContent) // Corrected: Cast to FrameworkElement
                {
                    modalContent.DataContext = this.ViewModel; // Corrected: Access DataContext on the casted object
                    Debug.WriteLine($"ProductDetailPage: Explicitly set DataContext of ContentDialog.Content to ViewModel."); // Added logging
                }
                else
                {
                    Debug.WriteLine("ProductDetailPage: ContentDialog.Content is null or not a FrameworkElement. Cannot set DataContext."); // Added logging
                }


                // Show the dialog
                // Use the XamlRoot from the page to show the dialog correctly
                // Ensure XamlRoot is available (page is loaded and in the visual tree)
                if (this.XamlRoot != null)
                {
                    this.UpdateProductContentDialog.XamlRoot = this.XamlRoot;
                    Debug.WriteLine("ProductDetailPage: Calling ShowAsync for UpdateProductContentDialog."); // Added logging before ShowAsync
                    try
                    {
                        await this.UpdateProductContentDialog.ShowAsync();
                        Debug.WriteLine("ProductDetailPage: UpdateProductContentDialog closed."); // Added logging after ShowAsync
                                                                                                  // When the dialog is closed (by clicking Save or Cancel),
                                                                                                  // the IsUpdateModalOpen property in the ViewModel should be set back to false
                                                                                                  // by the respective ExecuteSaveAsync or ExecuteCancelEditAsync methods.
                                                                                                  // This ensures the ViewModel state is consistent with the UI.
                    }
                    catch (Exception ex)
                    {
                        // Catch potential exceptions if ShowAsync is called while already open
                        // or other UI thread issues.
                        Debug.WriteLine($"ProductDetailPage: Error showing ContentDialog: {ex.Message}");
                    }
                }
                else
                {
                    Debug.WriteLine("ProductDetailPage: XamlRoot is null. Cannot show ContentDialog.");
                }
            });
        }


        /// <summary>
        /// Handles the OnNavigatedTo event to load product data when the page is navigated to.
        /// </summary>
        /// <param name="e">The navigation event arguments.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Debug.WriteLine($"ProductDetailPage: OnNavigatedTo called. Parameter type: {e.Parameter?.GetType().Name}, Parameter value: {e.Parameter}"); // Added logging

            // Check if the navigation parameter is an integer (the product ID)
            if (e.Parameter is int productId)
            {
                Debug.WriteLine($"ProductDetailPage: Navigation parameter is Product ID: {productId}. Calling LoadProductAsync."); // Added logging

                // Load the product data using the ViewModel
                await this.ViewModel.LoadProductAsync(productId); // Await the LoadProductAsync call
                await this.CheckProductExistanceInWishlist(); // Check if the product exists in the wishlist
            }
            else
            {
                Debug.WriteLine($"ProductDetailPage: Navigation parameter is NOT an integer Product ID. Parameter: {e.Parameter}"); // Added logging

                // You might want to handle cases where the parameter is not an int or is missing
                // For example, navigate back or show an error message.
            }
        }

        /// <summary>
        /// Handles the Click event for the "View" button on a related product item.
        /// Loads the related product details into the current page's ViewModel.
        /// </summary>
        /// <param name="sender">The source of the event (the clicked Button).</param>
        /// <param name="e">The event arguments.</param>
        private async void SeeRelatedProductButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("ProductDetailPage: SeeRelatedProductButton_Click called."); // Added logging

            // Get the clicked Button
            if (sender is Button clickedButton)
            {
                // Get the product ID from the Tag property
                if (clickedButton.Tag is int relatedProductId)
                {
                    Debug.WriteLine($"ProductDetailPage: Related Product Button clicked. Loading Product ID: {relatedProductId} into current page."); // Added logging

                    // Load the data for the related product into the *current* ViewModel
                    // This will update the UI bindings on the current page.
                    await this.ViewModel.LoadProductAsync(relatedProductId);
                }
                else
                {
                    Debug.WriteLine($"ProductDetailPage: Related Product Button clicked, but Tag is not an int Product ID. Tag value: {clickedButton.Tag}"); // Added logging
                }
            }
            else
            {
                Debug.WriteLine($"ProductDetailPage: SeeRelatedProductButton_Click called, but sender is not a Button. Sender type: {sender?.GetType().Name}"); // Added logging
            }
        }

        private async void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                Product selectedProduct = this.ViewModel.GetSelectedProduct();
                if (selectedProduct != null)
                {
                    CartItem addedItem = await this.cartViewModel.AddProductToCart(selectedProduct);

                    if (addedItem != null)
                    {
                        // Success feedback
                        await new ContentDialog
                        {
                            Title = "Success",
                            Content = "Product added to cart.",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        }.ShowAsync();
                    }
                    else
                    {
                        // Failure feedback
                        await new ContentDialog
                        {
                            Title = "Error",
                            Content = "Failed to add product to cart.",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        }.ShowAsync();
                    }
                }
            }
        }

        private async void AddToWishlistButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                Product selectedProduct = this.ViewModel.GetSelectedProduct();
                if (selectedProduct != null && selectedProduct.ID.HasValue)
                {
                    // Check if the product is already in the wishlist
                    WishlistItem item = await this.wishlistViewModel.GetProductFromWishlist(selectedProduct.ID.Value);
                    if (item != null && item.ID.HasValue)
                    {
                        // Remove from wishlist
                        bool removed = await this.wishlistViewModel.RemoveProductFromWishlist(item.ID.Value);
                        if (removed)
                        {
                            this.AddToWishlistButton.Content = "Add to Wishlist";
                            await new ContentDialog
                            {
                                Title = "Success",
                                Content = "Product removed from wishlist.",
                                CloseButtonText = "OK",
                                XamlRoot = this.XamlRoot
                            }.ShowAsync();
                        }
                        else
                        {
                            // Failure feedback
                            await new ContentDialog
                            {
                                Title = "Error",
                                Content = "Failed to remove product from wishlist.",
                                CloseButtonText = "OK",
                                XamlRoot = this.XamlRoot
                            }.ShowAsync();
                        }
                    }
                    else
                    {
                        // Add to wishlist
                        WishlistItem addedItem = await this.wishlistViewModel.AddProductToWishlist(selectedProduct);
                        if (addedItem != null)
                        {
                            this.AddToWishlistButton.Content = "Remove from Wishlist";
                            
                            // Success feedback
                            await new ContentDialog
                            {
                                Title = "Success",
                                Content = "Product added to wishlist.",
                                CloseButtonText = "OK",
                                XamlRoot = this.XamlRoot
                            }.ShowAsync();
                        }
                        else
                        {
                            // Failure feedback
                            await new ContentDialog
                            {
                                Title = "Error",
                                Content = "Failed to add product to wishlist.",
                                CloseButtonText = "OK",
                                XamlRoot = this.XamlRoot
                            }.ShowAsync();
                        }
                    }
                    /*WishlistItem addedItem = await this.wishlistViewModel.AddProductToWishlist(selectedProduct);
                    if (addedItem != null)
                    {
                        // Success feedback
                        await new ContentDialog
                        {
                            Title = "Success",
                            Content = "Product added to wishlist.",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot,
                        }.ShowAsync();
                    }
                    else
                    {
                        // Failure feedback
                        await new ContentDialog
                        {
                            Title = "Error",
                            Content = "Failed to add product to cart.",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        }.ShowAsync();
                    }*/
                }
            }
        }

        private async Task CheckProductExistanceInWishlist()
        {
            Product selectedProduct = this.ViewModel.GetSelectedProduct();
            if (selectedProduct != null && selectedProduct.ID.HasValue)
            {
                WishlistItem item = await this.wishlistViewModel.GetProductFromWishlist(selectedProduct.ID.Value);
                if (item != null)
                {
                    this.AddToWishlistButton.Content = "Remove from Wishlist";
                }
                else
                {
                    this.AddToWishlistButton.Content = "Add to Wishlist";
                }
            }
        }
    }
}