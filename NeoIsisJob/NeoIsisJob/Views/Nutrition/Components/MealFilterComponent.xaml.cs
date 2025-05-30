using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Workout.Core.Utils.Filters;

namespace NeoIsisJob.Views.Nutrition.Components
{
    public sealed partial class MealFilterComponent : UserControl
    {
        public event EventHandler<MealFilter> FilterChanged;

        private MealFilter _currentFilter;

        public MealFilterComponent()
        {
            this.InitializeComponent();
            _currentFilter = new MealFilter();
        }

        public MealFilter CurrentFilter
        {
            get => _currentFilter;
            set
            {
                _currentFilter = value ?? new MealFilter();
                UpdateUIFromFilter();
            }
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var value = selectedItem.Tag?.ToString();
                
                if (comboBox == MealTypeComboBox)
                {
                    _currentFilter.Type = string.IsNullOrEmpty(value) ? null : value;
                }
                else if (comboBox == CookingLevelComboBox)
                {
                    _currentFilter.CookingLevel = string.IsNullOrEmpty(value) ? null : value;
                }
                else if (comboBox == CookingTimeRangeComboBox)
                {
                    _currentFilter.CookingTimeRange = string.IsNullOrEmpty(value) ? null : value;
                }
                else if (comboBox == CalorieRangeComboBox)
                {
                    _currentFilter.CalorieRange = string.IsNullOrEmpty(value) ? null : value;
                }

                UpdateActiveFiltersDisplay();
            }
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            FilterChanged?.Invoke(this, _currentFilter);
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            _currentFilter = new MealFilter();
            UpdateUIFromFilter();
            UpdateActiveFiltersDisplay();
            FilterChanged?.Invoke(this, _currentFilter);
        }

        private void UpdateUIFromFilter()
        {
            // Update ComboBoxes
            SetComboBoxSelection(MealTypeComboBox, _currentFilter.Type);
            SetComboBoxSelection(CookingLevelComboBox, _currentFilter.CookingLevel);
            SetComboBoxSelection(CookingTimeRangeComboBox, _currentFilter.CookingTimeRange);
            SetComboBoxSelection(CalorieRangeComboBox, _currentFilter.CalorieRange);
        }

        private void SetComboBoxSelection(ComboBox comboBox, string value)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Tag?.ToString() == (value ?? string.Empty))
                {
                    comboBox.SelectedItem = item;
                    return;
                }
            }
            comboBox.SelectedIndex = 0; // Default to first item (usually "All")
        }

        private void UpdateActiveFiltersDisplay()
        {
            ActiveFiltersContainer.Children.Clear();

            var hasActiveFilters = false;

            if (!string.IsNullOrEmpty(_currentFilter.Type))
            {
                AddFilterChip($"Type: {_currentFilter.Type}");
                hasActiveFilters = true;
            }

            if (!string.IsNullOrEmpty(_currentFilter.CookingLevel))
            {
                AddFilterChip($"Level: {_currentFilter.CookingLevel}");
                hasActiveFilters = true;
            }

            if (!string.IsNullOrEmpty(_currentFilter.CookingTimeRange))
            {
                AddFilterChip($"Time: {_currentFilter.CookingTimeRange}");
                hasActiveFilters = true;
            }

            if (!string.IsNullOrEmpty(_currentFilter.CalorieRange))
            {
                AddFilterChip($"Calories: {_currentFilter.CalorieRange}");
                hasActiveFilters = true;
            }

            ActiveFiltersPanel.Visibility = hasActiveFilters ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AddFilterChip(string text)
        {
            var chip = new Border
            {
                Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.LightBlue),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(8, 4, 8, 4),
                Child = new TextBlock
                {
                    Text = text,
                    FontSize = 12,
                    Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.DarkBlue)
                }
            };

            ActiveFiltersContainer.Children.Add(chip);
        }
    }
} 