// <copyright file="UpdateProductButton.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View.Components // Your specified namespace
{
    using System;
    using System.Diagnostics; // Required for Debug.WriteLine
    using Microsoft.UI.Xaml; // Required for DependencyProperty, DependencyPropertyChangedEventArgs
    using Microsoft.UI.Xaml.Controls; // Required for UserControl, Button
    using WorkoutApp.ViewModel; // Assuming ProductViewModel is here

    /// <summary>
    /// A custom button component specifically for Update actions, receiving its ViewModel externally.
    /// Clicking this button will trigger the display of the update modal.
    /// </summary>
    public sealed partial class UpdateProductButton : UserControl // Inherit from UserControl
    {
        /// <summary>
        /// Identifies the <see cref="ViewModel"/> DependencyProperty.
        /// This property holds the instance of the ProductViewModel for this button.
        /// The ViewModel is provided by the parent View.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(ProductViewModel),
                typeof(UpdateProductButton),
                new PropertyMetadata(null, OnViewModelPropertyChanged)); // Default value null, call OnViewModelPropertyChanged when changed

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProductButton"/> class.
        /// </summary>
        public UpdateProductButton()
        {
            Debug.WriteLine("UpdateProductButton: Constructor called."); // Added logging
            this.InitializeComponent(); // Initialize the XAML defined in UpdateProductButton.xaml
        }

        /// <summary>
        /// Gets or sets the instance of the ProductViewModel for this button.
        /// This is a Dependency Property. Setting this property will update the DataContext.
        /// </summary>
        public ProductViewModel ViewModel
        {
            get => (ProductViewModel)this.GetValue(ViewModelProperty);
            set => this.SetValue(ViewModelProperty, value);
        }

        /// <summary>
        /// Handles changes to the <see cref="ViewModel"/> DependencyProperty.
        /// Updates the DataContext of the UserControl when the ViewModel is set.
        /// </summary>
        private static void OnViewModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine($"UpdateProductButton: OnViewModelPropertyChanged called. NewValue is {(e.NewValue == null ? "null" : "not null")}."); // Added logging
            if (d is UpdateProductButton button && e.NewValue is ProductViewModel newViewModel)
            {
                Debug.WriteLine($"UpdateProductButton: ViewModel property set. Updating DataContext."); // Added logging
                button.DataContext = newViewModel;
            }
        }

        // The Click event handler is handled by x:Bind in UpdateProductButton.xaml:
        // Click="{x:Bind ViewModel.ExecuteEnterEditModeAsync}"
        // This directly calls the ExecuteEnterEditModeAsync method on the ViewModel instance
        // that is set as the DataContext of the UserControl via the ViewModel DP.
        // No separate Click event handler is needed in the code-behind for this pattern.
    }
}
