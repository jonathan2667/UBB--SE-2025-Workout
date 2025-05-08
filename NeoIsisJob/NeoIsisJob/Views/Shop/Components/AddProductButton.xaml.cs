// <copyright file="AddProductButton.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Workout.Core.IServices;

namespace NeoIsisJob.Views.Shop.Components
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob.Proxy;

    /// <summary>
    /// A custom button that opens a flyout for adding a new product.
    /// </summary>
    public sealed partial class AddProductButton : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductButton"/> class.
        /// </summary>
        public AddProductButton()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the click event to show the add product flyout.
        /// </summary>
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var flyout = new Flyout
            {
                Content = new AddProductFlyout(new ProductServiceProxy(), new CategoryServiceProxy()),
            };

            flyout.ShowAt(this.AddButton); // Ensure AddButton is defined in XAML
        }
    }
}