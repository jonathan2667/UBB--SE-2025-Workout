// <copyright file="AddProductButton.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WorkoutApp.Data.Database;
    using WorkoutApp.Repository;
    using WorkoutApp.Service;

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
            string? connectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["DefaultConnection"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The 'DefaultConnection' connection string is missing.");
            }

            var connectionFactory = new DbConnectionFactory(connectionString);
            var dbService = new DbService(connectionFactory);
            var categoryRepo = new CategoryRepository(dbService);
            var productRepo = new ProductRepository(dbService);
            var productService = new ProductService(productRepo);
            var categoryService = new CategoryService(categoryRepo);

            var flyout = new Flyout
            {
                Content = new AddProductFlyout(productService, categoryService),
            };

            flyout.ShowAt(this.AddButton); // Ensure AddButton is defined in XAML
        }
    }
}