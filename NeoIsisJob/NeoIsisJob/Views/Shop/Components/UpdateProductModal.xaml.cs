// <copyright file="UpdateProductModal.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View.Components
{
    using System.Diagnostics;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// Interaction logic for UpdateProductModal.xaml.
    /// This UserControl serves as the content for the update product dialog.
    /// Its DataContext is expected to be set to a ProductViewModel.
    /// </summary>
    public sealed partial class UpdateProductModal : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProductModal"/> class.
        /// </summary>
        public UpdateProductModal()
        {
            Debug.WriteLine("UpdateProductModal: Constructor called.");
            this.InitializeComponent();
            this.DataContextChanged += this.UpdateProductModal_DataContextChanged;
            this.Loaded += this.UpdateProductModal_Loaded;
        }

        /// <summary>
        /// Handles the DataContextChanged event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The event data containing the new DataContext.</param>
        private void UpdateProductModal_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            string contextType = args.NewValue?.GetType().Name ?? "null";
            Debug.WriteLine($"UpdateProductModal: DataContextChanged event fired. New DataContext is {contextType}.");

            if (args.NewValue is ViewModel.ProductViewModel vm)
            {
                Debug.WriteLine($"UpdateProductModal: DataContext set to ProductViewModel. Name: {vm.Name}, Price: {vm.Price}, Stock: {vm.Stock}");
            }
        }

        /// <summary>
        /// Handles the Loaded event of the control.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The routed event arguments.</param>
        private void UpdateProductModal_Loaded(object sender, RoutedEventArgs e)
        {
            string contextType = this.DataContext?.GetType().Name ?? "null";
            Debug.WriteLine($"UpdateProductModal: Loaded event fired. Current DataContext is {contextType}.");

            if (this.DataContext is ViewModel.ProductViewModel vm)
            {
                Debug.WriteLine($"UpdateProductModal: DataContext is ProductViewModel on Loaded. Name: {vm.Name}, Price: {vm.Price}, Stock: {vm.Stock}");
            }
        }
    }
}
