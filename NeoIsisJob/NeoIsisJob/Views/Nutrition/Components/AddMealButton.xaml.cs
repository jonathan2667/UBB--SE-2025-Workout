// <copyright file="AddMealButton.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.Views.Nutrition.Components
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// A user control that displays a button for adding a meal, which opens a flyout.
    /// </summary>
    public sealed partial class AddMealButton : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddMealButton"/> class.
        /// </summary>
        public AddMealButton()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event for the Add button, showing a flyout containing the AddMealFlyout.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void AddMealButton_Click(object sender, RoutedEventArgs e)
        {
            var flyout = new Flyout
            {
                Content = new AddMealFlyout(),
            };

            flyout.ShowAt(this.AddButton);
        }
    }
}
