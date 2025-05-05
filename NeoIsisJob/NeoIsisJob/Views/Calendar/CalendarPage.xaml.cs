using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
// using NeoIsisJob.Data;
// using NeoIsisJob.Models;
using NeoIsisJob.ViewModels.Calendar;
// using NeoIsisJob.Services;
// using NeoIsisJob.Services.Interfaces;
using Workout.Core.Models;
using Workout.Core.Data;
using Workout.Core.Repositories;
using Workout.Core.Services;
using Workout.Core.IServices;
using NeoIsisJob.Proxy;
namespace NeoIsisJob.Views
{
    public sealed partial class CalendarPage : Page
    {
        public CalendarViewModel ViewModel { get; private set; }
        private readonly CalendarServiceProxy calendarService;

        public CalendarPage()
        {
            this.InitializeComponent();
            
            // Initialize services
            calendarService = new CalendarServiceProxy();
            
            // Assuming you have a way to get the UserId, e.g., from app state or navigation
            int userId = 1; // Replace with actual user ID source
            ViewModel = new CalendarViewModel(userId);
            this.DataContext = ViewModel;
            Loaded += CalendarPage_Loaded;
        }

        private void CalendarPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCalendarDisplay();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Property changed: {e.PropertyName}");
            if (e.PropertyName == nameof(ViewModel.CalendarDays))
            {
                UpdateCalendarDisplay();
            }
        }

        private void UpdateCalendarDisplay()
        {
            if (CalendarDaysGrid == null)
            {
                System.Diagnostics.Debug.WriteLine("CalendarDaysGrid is null in UpdateCalendarDisplay!");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Updating calendar with {ViewModel.CalendarDays.Count} days");
            CalendarDaysGrid.Children.Clear();
            CalendarDaysGrid.Background = new SolidColorBrush(Microsoft.UI.Colors.LightGray);
            CalendarDaysGrid.IsHitTestVisible = true;

            foreach (var day in ViewModel.CalendarDays)
            {
                FrameworkElement dayElement;
                var button = new Button
                {
                    Content = day.DayNumber.ToString(),
                    FontSize = 24,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(2),
                    IsEnabled = day.IsEnabled,
                    Visibility = day.IsEnabled ? Visibility.Visible : Visibility.Collapsed,
                    Tag = day
                };
                button.IsHitTestVisible = true;
                button.IsEnabled = true;
                button.Focus(FocusState.Programmatic);
                System.Diagnostics.Debug.WriteLine($"Added button for day {day.DayNumber}, Row: {day.GridRow}, Col: {day.GridColumn}, Enabled: {day.IsEnabled}, Class?: {day.HasClass}");
                System.Diagnostics.Debug.WriteLine($"Day {day.DayNumber} button Tag: {button.Tag}");
                // Set background color based on requirements
                if (day.HasWorkout)
                {
                    if (day.Date < DateTime.Now.Date) // Past days
                    {
                        button.Click += DayButton_Click_Past;
                        button.Background = new SolidColorBrush(
                            day.IsWorkoutCompleted ? Windows.UI.Color.FromArgb(255, 11, 224, 68) : Windows.UI.Color.FromArgb(255, 212, 17, 6));
                    }
                    else // Present and future days
                    {
                        button.Click += DayButton_Click_Future;
                        button.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 243, 247, 2));
                    }
                }
                else if (day.HasClass && day.Date >= DateTime.Now.Date) // Class days
                {
                    button.Click += DayButton_Click_Future;
                    button.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 204, 2, 164));
                }
                else if (day.HasClass && day.Date < DateTime.Now.Date)
                {
                    button.Click += DayButton_Click_Past;
                    button.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 204, 2, 164));
                }
                else // No workout or class
                {
                    button.Click += DayButton_Click_NoWorkout;
                    button.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
                }

                // Apply special styling for current day
                if (day.IsCurrentDay)
                {
                    dayElement = new Border
                    {
                        BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Blue),
                        BorderThickness = new Thickness(2),
                        CornerRadius = new CornerRadius(20),
                        Margin = new Thickness(2),
                        Child = button,
                        IsHitTestVisible = true // Ensure border doesn't block clicks
                    };
                }
                else
                {
                    dayElement = button;
                }

                button.IsHitTestVisible = true;
                button.Focus(FocusState.Programmatic);
                Grid.SetRow(dayElement, day.GridRow);
                Grid.SetColumn(dayElement, day.GridColumn);
                CalendarDaysGrid.Children.Add(dayElement);
            }
        }

        private async void DayButton_Click_Past(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DayButton_Click_Past triggered");
            if (sender is Button button && button.Tag is CalendarDayModel day)
            {
                System.Diagnostics.Debug.WriteLine($"Clicked day: {day.DayNumber}, HasWorkout: {day.HasWorkout}");

                ContentDialog dialog;

                if (day.HasWorkout || day.HasClass)
                {
                    var userWorkout = await calendarService.GetUserWorkoutAsync(ViewModel.UserId, day.Date);
                    string userClass = day.HasClass ? await calendarService.GetUserClassAsync(ViewModel.UserId, day.Date) : null;

                    if (userWorkout != null || userClass != null)
                    {
                        string workoutName = userWorkout != null ? await GetWorkoutName(userWorkout.WID) : null;
                        string message = $"Date: {day.Date:yyyy-MM-dd}\n";

                        if (userWorkout != null)
                        {
                            message += $"Workout Name: {workoutName ?? "Unknown Workout"}\n";
                            message += $"Completed: {(userWorkout.Completed ? "Yes" : "No")}\n";
                        }
                        if (userClass != null)
                        {
                            message += $"Class Name: {userClass ?? "Unknown Class"}\n";
                        }

                        dialog = new ContentDialog
                        {
                            Title = $"Details - Day {day.DayNumber}",
                            Content = message.Trim(),
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        };
                    }
                    else
                    {
                        dialog = new ContentDialog
                        {
                            Title = $"Day {day.DayNumber}",
                            Content = "Data not found.",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        };
                    }
                }
                else
                {
                    dialog = new ContentDialog
                    {
                        Title = $"Day {day.DayNumber}",
                        Content = "No workout or class scheduled for this day.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                }

                await dialog.ShowAsync();
            }
        }

        private async void DayButton_Click_Future(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DayButton_Click_Future triggered");
            if (sender is Button button && button.Tag is CalendarDayModel day)
            {
                System.Diagnostics.Debug.WriteLine($"Clicked day: {day.DayNumber}, HasWorkout: {day.HasWorkout}");
                ContentDialog dialog;

                if (day.HasWorkout || day.HasClass)
                {
                    var userWorkout = await calendarService.GetUserWorkoutAsync(ViewModel.UserId, day.Date);
                    string userClass = day.HasClass ? await calendarService.GetUserClassAsync(ViewModel.UserId, day.Date) : null;

                    if (userWorkout != null || userClass != null)
                    {
                        string workoutName = userWorkout != null ? await GetWorkoutName(userWorkout.WID) : null;
                        string message = $"Date: {day.Date:yyyy-MM-dd}\n";

                        if (userWorkout != null)
                        {
                            message += $"Workout Name: {workoutName ?? "Unknown Workout"}\n";
                        }
                        if (userClass != null)
                        {
                            message += $"Class Name: {userClass ?? "Unknown Class"}\n";
                        }

                        dialog = new ContentDialog
                        {
                            Title = $"Details - Day {day.DayNumber}",
                            Content = message.Trim(),
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot,
                            PrimaryButtonText = day.HasWorkout && day.Date >= DateTime.Now.Date ? "Change Workout" : null,
                            SecondaryButtonText = day.HasWorkout && day.Date >= DateTime.Now.Date ? "Remove Workout" : null
                        };

                        dialog.PrimaryButtonClick += async (s, args) =>
                        {
                            if (day.HasWorkout && day.Date >= DateTime.Now.Date)
                            {
                                args.Cancel = true; // Keep dialog open

                                // Fetch workout list
                                List<WorkoutModel> workouts = await calendarService.GetWorkoutsAsync();
                                System.Diagnostics.Debug.WriteLine("Workout List:");
                                if (workouts.Count > 0)
                                {
                                    foreach (var workout in workouts)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"ID: {workout.WID}, Name: {workout.Name}");
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine("No workouts found.");
                                }

                                // Create workout list UI
                                var stackPanel = new StackPanel();
                                foreach (var workout in workouts)
                                {
                                    var workoutButton = new Button
                                    {
                                        Content = workout.Name,
                                        Margin = new Thickness(0, 5, 0, 0),
                                        Tag = workout.WID // Store workout ID in Tag
                                    };
                                    workoutButton.Click += async (btnSender, btnArgs) =>
                                    {
                                        if (btnSender is Button clickedButton && clickedButton.Tag is int workoutId)
                                        {
                                            // Remove existing workout first
                                            var existingWorkout = await calendarService.GetUserWorkoutAsync(ViewModel.UserId, day.Date);
                                            if (existingWorkout != null)
                                            {
                                                await calendarService.DeleteUserWorkoutAsync(ViewModel.UserId, existingWorkout.WID, day.Date);
                                            }

                                            // Add new workout
                                            var newWorkout = new UserWorkoutModel(
                                                userId: ViewModel.UserId,
                                                workoutId: workoutId,
                                                date: day.Date,
                                                completed: false);
                                            ViewModel.AddUserWorkout(newWorkout);
                                            dialog.Content = $"Workout changed to '{clickedButton.Content}' successfully.";
                                            dialog.PrimaryButtonText = "Change Workout";
                                            dialog.SecondaryButtonText = "Remove Workout";
                                        }
                                    };
                                    stackPanel.Children.Add(workoutButton);
                                }

                                // Add a "Back" button to revert to initial view23
                                var backButton = new Button
                                {
                                    Content = "Back",
                                    Margin = new Thickness(0, 10, 0, 0)
                                };
                                backButton.Click += (btnSender, btnArgs) =>
                                {
                                    dialog.Content = message;
                                    dialog.PrimaryButtonText = "Change Workout";
                                    dialog.SecondaryButtonText = "Remove Workout";
                                };
                                stackPanel.Children.Add(backButton);

                                // Update dialog content to show workout list
                                dialog.Content = stackPanel;
                                dialog.PrimaryButtonText = null; // Hide "Change Workout" while showing list
                                dialog.SecondaryButtonText = null; // Hide "Remove Workout" while showing list
                            }
                        };

                        dialog.SecondaryButtonClick += async (s, args) =>
                        {
                            if (day.HasWorkout && day.Date >= DateTime.Now.Date)
                            {
                                args.Cancel = true; // Keep dialog open
                                await calendarService.RemoveWorkoutAsync(ViewModel.UserId, day);
                                ViewModel.UpdateCalendar(); // Force calendar update
                                dialog.Hide(); // Close the dialog
                            }
                        };
                    }
                    else
                    {
                        dialog = new ContentDialog
                        {
                            Title = $"Day {day.DayNumber}",
                            Content = "Data not found.",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        };
                    }
                }
                else
                {
                    dialog = new ContentDialog
                    {
                        Title = $"Day {day.DayNumber}",
                        Content = "No workout or class scheduled for this day.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                }

                await dialog.ShowAsync();
            }
        }

        private async void DayButton_Click_NoWorkout(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DayButton_Click_Future triggered");
            if (sender is Button button && button.Tag is CalendarDayModel day)
            {
                System.Diagnostics.Debug.WriteLine($"Clicked day: {day.DayNumber}, HasWorkout: {day.HasWorkout}");
                string message = string.Empty;
                ContentDialog dialog;

                dialog = new ContentDialog
                {
                    Title = $"Day {day.DayNumber}",
                    Content = "No workout scheduled for this day.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot,
                    PrimaryButtonText = day.Date >= DateTime.Now.Date ? "Add Workout" : null
                };

                dialog.PrimaryButtonClick += async (s, args) =>
                {
                    if (day.Date >= DateTime.Now.Date)
                    {
                        // Prevent dialog from closing immediately
                        args.Cancel = true; // Key fix: Keeps dialog open

                        // Fetch workout list
                        List<WorkoutModel> workouts = await calendarService.GetWorkoutsAsync(); // Ensure this method exists
                        System.Diagnostics.Debug.WriteLine("Workout List:");
                        if (workouts.Count > 0)
                        {
                            foreach (var workout in workouts)
                            {
                                System.Diagnostics.Debug.WriteLine($"ID: {workout.WID}, Name: {workout.Name}");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("No workouts found.");
                        }

                        // Create workout list UI
                        var stackPanel = new StackPanel();
                        foreach (var workout in workouts)
                        {
                            var button = new Button
                            {
                                Content = workout.Name,
                                Margin = new Thickness(0, 5, 0, 0),
                                Tag = workout.WID // Store workout ID in Tag
                            };
                            button.Click += async (btnSender, btnArgs) =>
                            {
                                if (btnSender is Button clickedButton && clickedButton.Tag is int workoutId)
                                {
                                    // Check if workout already exists for this date
                                    var existingWorkout = await calendarService.GetUserWorkoutAsync(ViewModel.UserId, day.Date);
                                    if (existingWorkout != null)
                                    {
                                        // If workout exists, update it instead of adding a new one
                                        await calendarService.DeleteUserWorkoutAsync(ViewModel.UserId, existingWorkout.WID, day.Date);
                                    }

                                    // Add new workout
                                    var newWorkout = new UserWorkoutModel(
                                        userId: ViewModel.UserId,
                                        workoutId: workoutId,
                                        date: day.Date,
                                        completed: false);
                                    await calendarService.AddUserWorkoutAsync(newWorkout);
                                    ViewModel.UpdateCalendar(); // Force calendar update
                                    dialog.Content = $"Workout '{clickedButton.Content}' added successfully.";
                                    dialog.Hide(); // Close the dialog
                                }
                            };
                            stackPanel.Children.Add(button);
                        }

                        // Add a "Back" button to revert to initial view
                        var backButton = new Button
                        {
                            Content = "Back",
                            Margin = new Thickness(0, 10, 0, 0)
                        };
                        backButton.Click += (btnSender, btnArgs) =>
                        {
                            dialog.Content = message;
                            dialog.PrimaryButtonText = "Change Workout";
                            dialog.SecondaryButtonText = "Remove Workout";
                        };
                        stackPanel.Children.Add(backButton);

                        // Update dialog content to show workout list
                        dialog.Content = stackPanel;
                        dialog.PrimaryButtonText = null; // Hide "Change Workout" while showing list
                        dialog.SecondaryButtonText = null; // Hide "Remove Workout" while showing list
                    }
                };

                await dialog.ShowAsync();
            }
        }

        private async Task<string> GetWorkoutName(int workoutId)
        {
            using (var conn = new DatabaseHelper().GetConnection())
            {
                await conn.OpenAsync();
                string query = "SELECT Name FROM Workouts WHERE Wid = @WorkoutId";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkoutId", workoutId);
                    var result = await cmd.ExecuteScalarAsync();
                    return result?.ToString();
                }
            }
        }

        public void GoToMainPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        public void GoToWorkoutPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WorkoutPage));
        }

        // Already on CalendarPage, no Action needed
        public void GoToCalendarPage_Tap(object sender, RoutedEventArgs e)
        {
        }

        public void GoToClassPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ClassPage));
        }

        public void GoToRankingPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RankingPage));
        }
    }
}