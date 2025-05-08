// <copyright file="PaymentPage.xaml.cs" company="PlaceholderCompany">
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
    /// The payment page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        private readonly PaymentPageViewModel viewModel;
        private readonly CartViewModel cartViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentPage"/> class.
        /// </summary>
        public PaymentPage()
        {
            this.InitializeComponent();
            this.viewModel = new PaymentPageViewModel();
            this.cartViewModel = new CartViewModel();
            this.BindingContext = this.viewModel;
            this.LoadCartItems();
        }

        /// <summary>
        /// Loads the cart items.
        /// </summary>
        private async void LoadCartItems()
        {
            try
            {
                IEnumerable<CartItemModel> cartItems = await this.cartViewModel.GetAllCartItemsAsync();
                this.CartItemsListView.ItemsSource = cartItems;
                decimal total = await this.cartViewModel.CalculateTotalAsync();
                this.TotalLabel.Text = $"Total: ${total:F2}";
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Error", $"Failed to load cart items: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Handles the process payment button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnProcessPaymentButtonClicked(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<CartItemModel> cartItems = await this.cartViewModel.GetAllCartItemsAsync();
                decimal total = await this.cartViewModel.CalculateTotalAsync();

                // Create the order
                OrderModel order = await this.viewModel.CreateOrderAsync(cartItems, total);

                // Clear the cart
                await this.viewModel.ClearCartAsync();

                await this.DisplayAlert("Success", "Payment processed successfully", "OK");
                await this.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Error", $"Failed to process payment: {ex.Message}", "OK");
            }
        }
    }
}
