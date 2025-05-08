// <copyright file="Header.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View.Components
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
    using WorkoutApp.View.Pages;

    /// <summary>
    /// A user control that represents the header of the application.
    /// </summary>
    public sealed partial class Header : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        public Header()
        {
            this.InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AppFrame?.Navigate(typeof(MainPage));
        }

        private void WishlistButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AppFrame?.Navigate(typeof(WishlistPage));
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AppFrame?.Navigate(typeof(CartPage));
        }
    }
}
