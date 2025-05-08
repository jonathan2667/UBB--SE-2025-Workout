// <copyright file="DeleteProductButton.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View.Components
{
    using System;
    using System.Diagnostics;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WorkoutApp.ViewModel;

    /// <summary>
    /// A custom button component specifically for Delete actions, receiving its ViewModel externally.
    /// </summary>
    public sealed partial class DeleteProductButton : UserControl
    {
        /// <summary>
        /// Identifies the <see cref="ViewModel"/> DependencyProperty.
        /// This property holds the instance of the ProductViewModel for this button.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(ProductViewModel),
                typeof(DeleteProductButton),
                new PropertyMetadata(null, OnViewModelPropertyChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProductButton"/> class.
        /// </summary>
        public DeleteProductButton()
        {
            Debug.WriteLine("DeleteProductButton: Constructor called.");
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the instance of the ProductViewModel for this button.
        /// </summary>
        public ProductViewModel? ViewModel
        {
            get => (ProductViewModel?)this.GetValue(ViewModelProperty);
            set => this.SetValue(ViewModelProperty, value);
        }

        /// <summary>
        /// Handles changes to the <see cref="ViewModel"/> DependencyProperty.
        /// Updates the DataContext of the UserControl when the ViewModel is set.
        /// </summary>
        /// <param name="d">The dependency object where the property changed.</param>
        /// <param name="e">The event data for the change.</param>
        private static void OnViewModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine($"DeleteProductButton: OnViewModelPropertyChanged called. NewValue is {(e.NewValue == null ? "null" : "not null")}.");

            if (d is DeleteProductButton button && e.NewValue is ProductViewModel newViewModel)
            {
                Debug.WriteLine("DeleteProductButton: ViewModel property set. Updating DataContext.");
                button.DataContext = newViewModel;
            }
        }
    }
}
