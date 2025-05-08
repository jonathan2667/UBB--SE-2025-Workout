// <copyright file="PaymentPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View.Pages
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Controls.Primitives;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Media;
    using Microsoft.UI.Xaml.Navigation;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using WorkoutApp.ViewModel;

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
                MainWindow.AppFrame?.Navigate(typeof(MainPage));
            }
        }
    }
}
