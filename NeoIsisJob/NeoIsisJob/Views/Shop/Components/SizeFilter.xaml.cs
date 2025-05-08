// <copyright file="SizeFilter.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View.Components
{
    using System;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// A component for selecting a product size filter from a ComboBox.
    /// </summary>
    public sealed partial class SizeFilter : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SizeFilter"/> class.
        /// </summary>
        public SizeFilter()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Occurs when the size selection changes.
        /// </summary>
        public event EventHandler<string>? SizeFilterChanged;

        /// <summary>
        /// Resets the size selection filter.
        /// </summary>
        public void ResetFilter()
        {
            this.SizeComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Handles selection changes in the size ComboBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void SizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedSize = (this.SizeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (!string.IsNullOrEmpty(selectedSize))
            {
                this.SizeFilterChanged?.Invoke(this, selectedSize);
            }
        }
    }
}
