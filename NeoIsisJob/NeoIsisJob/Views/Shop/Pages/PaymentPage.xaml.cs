// <copyright file="PaymentPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.Views.Shop.Pages
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob.ViewModels.Shop;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PaymentPage : Page
    {
        private readonly PaymentPageViewModel paymentPageViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentPage"/> class.
        /// </summary>
        public PaymentPage()
        {
            this.InitializeComponent();
            this.paymentPageViewModel = new PaymentPageViewModel();
        }

        private async void SendOrderButton_Click(object sender, RoutedEventArgs e)
        {
            bool successFlag = await this.paymentPageViewModel.SendOrder();

            ContentDialog dialog = new ContentDialog
            {
                Title = successFlag ? "Order Placed" : "Order Failed",
                Content = successFlag ? "Your order was successfully placed." : "Something went wrong. Please try again.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot,
            };
            await dialog.ShowAsync();

            if (successFlag)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
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
            this.Frame.Navigate(typeof(CartPage));
        }
    }
}
