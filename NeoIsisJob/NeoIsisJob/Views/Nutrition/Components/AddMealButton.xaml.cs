// <copyright file="AddMealButton.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.Views.Nutrition.Components
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Controls.Primitives;

    /// <summary>
    /// A user control that displays a button for adding a meal, which opens a flyout.
    /// </summary>
    public sealed partial class AddMealButton : UserControl
    {
        /// <summary>
        /// Event that is raised when a meal is successfully added.
        /// </summary>
        public event RoutedEventHandler AddMealCompleted;

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
            var flyoutContent = new AddMealFlyout();
            flyoutContent.MealAdded += OnMealAdded;

            var flyout = new Flyout
            {
                Content = flyoutContent,
                Placement = FlyoutPlacementMode.BottomEdgeAlignedRight,
                ShowMode = FlyoutShowMode.Standard,
            };
            flyout.ShowAt(this.AddButton);
        }

        /// <summary>
        /// Handles the event when a meal is successfully added through the flyout.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMealAdded(object sender, RoutedEventArgs e)
        {
            AddMealCompleted?.Invoke(this, new RoutedEventArgs());
        }
    }
}
