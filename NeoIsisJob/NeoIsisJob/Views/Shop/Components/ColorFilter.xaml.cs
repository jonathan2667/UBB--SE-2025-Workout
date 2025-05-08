// <copyright file="ColorFilter.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View.Components
{
    using System;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// A user control that allows filtering by color using a ComboBox.
    /// </summary>
    public sealed partial class ColorFilter : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorFilter"/> class.
        /// </summary>
        public ColorFilter()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Occurs when the user selects a new color.
        /// </summary>
        public event EventHandler<string>? ColorChanged;

        /// <summary>
        /// Resets the color filter selection.
        /// </summary>
        public void ResetFilter()
        {
            this.ColorComboBox.SelectedItem = null;
        }

        /// <summary>
        /// Handles the ComboBox selection changed event to notify listeners.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedColor = (this.ColorComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (!string.IsNullOrEmpty(selectedColor))
            {
                this.ColorChanged?.Invoke(this, selectedColor);
            }
        }
    }
}
