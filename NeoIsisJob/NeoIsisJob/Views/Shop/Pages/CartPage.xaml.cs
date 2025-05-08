// <copyright file="CartPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Workout.Core.Models;
using NeoIsisJob.ViewModels.Shop;

namespace NeoIsisJob.Views.Shop.Pages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;


    /// <summary>
    /// A page that displays current items in the cart.
    /// </summary>
    public sealed partial class CartPage : Page
    {
        private readonly CartViewModel cartViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartPage"/> class.
        /// </summary>
        public CartPage()
        {
            this.InitializeComponent();
            this.cartViewModel = new CartViewModel();
        }

        /// <summary>
        /// Called when the page is navigated to.
        /// </summary>
        /// <param name="e">Event data that provides information about the navigation.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await this.LoadProducts();
        }

        private void VerticalProductListControl_ProductClicked(object sender, int productID)
        {
            this.Frame.Navigate(typeof(ProductDetailPage), productID);
        }

        private async void VerticalProductListControl_CartItemRemoved(object sender, int cartItemID)
        {
            await this.cartViewModel.RemoveProductFromCart(cartItemID);
            await this.LoadProducts();
        }

        private void CheckoutButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PaymentPage));
        }

        private async Task LoadProducts()
        {
            IEnumerable<CartItemModel> products = await this.cartViewModel.GetAllProductsFromCartAsync();
            this.ProductListViewControl.SetProducts(products);
            this.TotalPriceTextBlock.Text = this.cartViewModel.TotalPrice.ToString("C2");
        }

        // Navigation methods - you already have these implemented
        public void GoToMainPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NeoIsisJob.Views.MainPage));
        }

        public void GoToWorkoutPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WorkoutPage));
        }

        public void GoToCalendarPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CalendarPage));
        }

        public void GoToClassPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ClassPage));
        }

        public void GoToRankingPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RankingPage));
        }

        public void GoToShopHomePage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NeoIsisJob.Views.Shop.Pages.MainPage));
        }

        public void GoToWishlistPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WishlistPage));
        }

        public void GoToCartPage_Tap(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(CartPage));
        }
    }
}
