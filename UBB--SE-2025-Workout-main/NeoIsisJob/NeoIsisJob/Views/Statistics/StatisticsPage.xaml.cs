using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NeoIsisJob.Proxy;
using NeoIsisJob.ViewModels;
using Workout.Core.Models;

namespace NeoIsisJob.Views.Statistics
{
    /// <summary>
    /// Statistics page for displaying nutrition and water tracking data.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        /// <summary>
        /// Gets the ViewModel for this page.
        /// </summary>
        public StatisticsViewModel ViewModel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsPage"/> class.
        /// </summary>
        public StatisticsPage()
        {
            this.InitializeComponent();

            // Get the ViewModel from dependency injection
            this.ViewModel = App.Services.GetRequiredService<StatisticsViewModel>();
            this.DataContext = this.ViewModel;
        }

        /// <summary>
        /// Handles the page loaded event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await this.ViewModel.LoadDataAsync();
        }

        /// <summary>
        /// Handles the add custom water button click event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private async void AddCustomWater_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (StatisticsViewModel)this.DataContext;
            var waterAmount = this.CustomWaterAmount.Value;
            if (viewModel != null && waterAmount > 0)
            {
                await viewModel.AddWaterAsync((int)waterAmount);
                this.CustomWaterAmount.Value = 0;
            }
        }

        /// <summary>
        /// Handles the log meal button click event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private async void LogMeal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get meal service to load available meals
                var mealService = new MealAPIServiceProxy();
                var availableMeals = await mealService.GetAllAsync();

                if (!availableMeals.Any())
                {
                    var noMealsDialog = new ContentDialog
                    {
                        Title = "No Meals Available",
                        Content = "There are no meals available to log. Please add some meals first.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await noMealsDialog.ShowAsync();
                    return;
                }

                // Create meal selection dialog
                var mealSelectionDialog = new ContentDialog
                {
                    Title = "Log Meal",
                    CloseButtonText = "Cancel",
                    PrimaryButtonText = "Log Meal",
                    XamlRoot = this.XamlRoot
                };

                // Create content panel wrapped in ScrollViewer for scrolling
                var scrollViewer = new ScrollViewer
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                    ZoomMode = ZoomMode.Disabled,
                    MaxHeight = 500, // Limit height to ensure dialog fits on screen
                    Padding = new Thickness(0, 0, 10, 0) // Add some padding for scrollbar
                };

                var contentPanel = new StackPanel
                {
                    Spacing = 15,
                    Margin = new Thickness(0, 10, 0, 10)
                };

                // Meal selection
                var mealLabel = new TextBlock { Text = "Select a meal:", FontWeight = Microsoft.UI.Text.FontWeights.SemiBold };
                contentPanel.Children.Add(mealLabel);

                var mealListView = new ListView
                {
                    SelectionMode = ListViewSelectionMode.Single,
                    ItemsSource = availableMeals,
                    Height = 150,
                    BorderBrush = this.Resources["SystemControlForegroundBaseMediumLowBrush"] as Microsoft.UI.Xaml.Media.Brush,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(4),
                    DisplayMemberPath = "Name" // Simple display of meal names
                };

                // Add event to show meal details when selection changes
                mealListView.SelectionChanged += (_, selectionArgs) =>
                {
                    if (selectionArgs.AddedItems.FirstOrDefault() is MealModel selectedMeal)
                    {
                        // You could add a TextBlock to show meal details here if needed
                        // For now, we'll just enable the primary button
                        mealSelectionDialog.IsPrimaryButtonEnabled = true;
                    }
                    else
                    {
                        mealSelectionDialog.IsPrimaryButtonEnabled = false;
                    }
                };

                contentPanel.Children.Add(mealListView);

                // Add meal details panel
                var detailsPanel = new StackPanel { Spacing = 5 };
                var detailsLabel = new TextBlock
                {
                    Text = "Meal Details:",
                    FontWeight = Microsoft.UI.Text.FontWeights.SemiBold
                };
                detailsPanel.Children.Add(detailsLabel);

                var detailsTextBlock = new TextBlock
                {
                    Text = "Select a meal to see details",
                    FontSize = 12,
                    Foreground = this.Resources["SystemControlForegroundBaseMediumBrush"] as Microsoft.UI.Xaml.Media.Brush,
                    TextWrapping = TextWrapping.Wrap
                };
                detailsPanel.Children.Add(detailsTextBlock);

                // Update details when meal selection changes
                mealListView.SelectionChanged += (_, selectionArgs) =>
                {
                    if (selectionArgs.AddedItems.FirstOrDefault() is MealModel selectedMeal)
                    {
                        detailsTextBlock.Text = $"{selectedMeal.Type} • {selectedMeal.Calories} cal • " +
                                              $"P: {selectedMeal.Proteins:F1}g • C: {selectedMeal.Carbohydrates:F1}g • F: {selectedMeal.Fats:F1}g\n" +
                                              $"Cooking: {selectedMeal.CookingLevel} ({selectedMeal.CookingTimeMins} min)";
                    }
                    else
                    {
                        detailsTextBlock.Text = "Select a meal to see details";
                    }
                };

                contentPanel.Children.Add(detailsPanel);

                // Portion size selection
                var portionLabel = new TextBlock { Text = "Portion size:", FontWeight = Microsoft.UI.Text.FontWeights.SemiBold };
                contentPanel.Children.Add(portionLabel);

                var portionPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };

                var portionBox = new NumberBox
                {
                    Value = 1.0,
                    Minimum = 0.1,
                    Maximum = 5.0,
                    SmallChange = 0.1,
                    LargeChange = 0.5,
                    Width = 100,
                    SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact
                };

                var portionLabel2 = new TextBlock { Text = "x (1.0 = normal portion)", VerticalAlignment = VerticalAlignment.Center };

                portionPanel.Children.Add(portionBox);
                portionPanel.Children.Add(portionLabel2);
                contentPanel.Children.Add(portionPanel);

                // Notes section
                var notesLabel = new TextBlock { Text = "Notes (optional):", FontWeight = Microsoft.UI.Text.FontWeights.SemiBold };
                contentPanel.Children.Add(notesLabel);

                var notesBox = new TextBox
                {
                    PlaceholderText = "Add any notes about this meal...",
                    Height = 60,
                    AcceptsReturn = true,
                    TextWrapping = TextWrapping.Wrap
                };
                contentPanel.Children.Add(notesBox);

                // Add contentPanel to scrollViewer
                scrollViewer.Content = contentPanel;
                mealSelectionDialog.Content = scrollViewer;
                mealSelectionDialog.IsPrimaryButtonEnabled = false;

                var result = await mealSelectionDialog.ShowAsync();

                if (result == ContentDialogResult.Primary && mealListView.SelectedItem is MealModel selectedMeal)
                {
                    // Log the meal
                    var portionSize = portionBox.Value;
                    var notes = notesBox.Text;

                    await this.ViewModel.LogMealAsync(selectedMeal, portionSize, notes);

                    // Show success message
                    var successDialog = new ContentDialog
                    {
                        Title = "Meal Logged!",
                        Content = $"Successfully logged {selectedMeal.Name} ({portionSize:F1}x portion).",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await successDialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"Failed to log meal: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = (this as FrameworkElement)?.XamlRoot ?? throw new InvalidOperationException("XamlRoot is null"),
                };
                await errorDialog.ShowAsync();
            }
        }
    }
}