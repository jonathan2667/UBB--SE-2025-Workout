using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Workout.Core.Models;
using NeoIsisJob.Proxy;

namespace NeoIsisJob.Views
{
    public sealed partial class MainPage : Page
    {
        // Services
        private readonly WorkoutServiceProxy workoutService;
        private readonly ExerciseServiceProxy exerciseService;
        private readonly CompleteWorkoutServiceProxy completeWorkoutService;
        private readonly CalendarServiceProxy calendarService;

        // Current workout data
        private WorkoutModel currentWorkout;
        private List<ExerciseWithDetails> currentWorkoutExercises;

        // Test user ID (for testing purposes only)
        private readonly int currentUserId = 1;

        // Helper class to display exercise details
        private class ExerciseWithDetails
        {
            public string Name { get; set; }
            public string Details { get; set; }

            public override string ToString()
            {
                return $"{Name}: {Details}";
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            // Initialize services
            workoutService = new WorkoutServiceProxy();
            exerciseService = new ExerciseServiceProxy();
            completeWorkoutService = new CompleteWorkoutServiceProxy();
            calendarService = new CalendarServiceProxy();
            // Set current date
            CurrentDateTextBlock.Text = DateTime.Now.ToString("dddd, MMMM d, yyyy");

            // Configure button events
            AddWorkoutButton.Click += AddWorkoutButton_Click;
            CompleteWorkoutButton.Click += CompleteWorkoutButton_Click;
            DeleteWorkoutButton.Click += DeleteWorkoutButton_Click;

            // Load today's workout - for testing, we'll check if there's an active workout
            LoadTodaysWorkout();
        }

        private void LoadTodaysWorkout()
        {
            try
            {
                // For testing, check if we already have a workout assigned for today in the session storage
                // In a real implementation, this would come from the UserWorkouts table
                var todaysWorkoutId = (int?)Windows.Storage.ApplicationData.Current.LocalSettings.Values["TodaysWorkoutId"];

                if (todaysWorkoutId.HasValue)
                {
                    // Get the workout details
                    currentWorkout = workoutService.GetWorkoutAsync(todaysWorkoutId.Value).Result;

                    if (currentWorkout != null)
                    {
                        // Get the exercises for this workout
                        var completeWorkouts = completeWorkoutService.GetCompleteWorkoutsByWorkoutIdAsync(currentWorkout.WID).Result;
                        currentWorkoutExercises = new List<ExerciseWithDetails>();

                        foreach (var completeWorkout in completeWorkouts)
                        {
                            var exercise = exerciseService.GetExerciseByIdAsync(completeWorkout.EID).Result;
                            if (exercise != null)
                            {
                                currentWorkoutExercises.Add(new ExerciseWithDetails
                                {
                                    Name = exercise.Name,
                                    Details = $"{completeWorkout.Sets} sets × {completeWorkout.RepsPerSet} reps"
                                });
                            }
                        }

                        // Update UI to show the workout
                        DisplayWorkout();
                        return;
                    }
                }

                // No workout found, show the no workout state
                DisplayNoWorkout();
            }
            catch (Exception ex)
            {
                // For debugging
                System.Diagnostics.Debug.WriteLine($"Error loading workout: {ex.Message}");
                DisplayNoWorkout();
            }
        }

        private void DisplayWorkout()
        {
            // Show workout details
            WorkoutTitleTextBlock.Text = currentWorkout.Name;
            WorkoutDescriptionTextBlock.Text = "Today's workout plan";

            // Populate exercises list
            ExercisesList.ItemsSource = currentWorkoutExercises;

            // Show the exercise panel, hide the no workout message
            NoWorkoutTextBlock.Visibility = Visibility.Collapsed;
            WorkoutExercisesPanel.Visibility = Visibility.Visible;

            // Show Complete/Delete buttons, hide Add button
            AddWorkoutButton.Visibility = Visibility.Collapsed;
            CompleteWorkoutButton.Visibility = Visibility.Visible;
            DeleteWorkoutButton.Visibility = Visibility.Visible;
        }

        private void DisplayNoWorkout()
        {
            // Reset workout title
            WorkoutTitleTextBlock.Text = "No Active Workout";

            // Show the no workout message
            NoWorkoutTextBlock.Visibility = Visibility.Visible;
            WorkoutExercisesPanel.Visibility = Visibility.Collapsed;

            // Show Add button, hide Complete/Delete buttons
            AddWorkoutButton.Visibility = Visibility.Visible;
            CompleteWorkoutButton.Visibility = Visibility.Collapsed;
            DeleteWorkoutButton.Visibility = Visibility.Collapsed;

            // Clear current workout data
            currentWorkout = null;
            currentWorkoutExercises = null;
        }
        private async void AddWorkoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1) Fetch list of workouts asynchronously
                var availableWorkouts = await workoutService.GetAllWorkoutsAsync();

                if (availableWorkouts.Count == 0)
                {
                    var noWorkoutsDialog = new ContentDialog
                    {
                        Title = "No Workouts Available",
                        Content = "There are no workouts available to add.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await noWorkoutsDialog.ShowAsync();
                    return;
                }

                // 2) Show selection dialog
                var selectWorkoutDialog = new ContentDialog
                {
                    Title = "Select a Workout",
                    CloseButtonText = "Cancel",
                    PrimaryButtonText = "Add",
                    XamlRoot = this.XamlRoot
                };

                var workoutListView = new ListView
                {
                    SelectionMode = ListViewSelectionMode.Single,
                    ItemsSource = availableWorkouts,
                    DisplayMemberPath = "Name",
                    Height = 300,
                    Margin = new Thickness(0, 10, 0, 10)
                };

                selectWorkoutDialog.Content = workoutListView;
                selectWorkoutDialog.IsPrimaryButtonEnabled = false;

                workoutListView.SelectionChanged += (_, __) =>
                    selectWorkoutDialog.IsPrimaryButtonEnabled = workoutListView.SelectedItem != null;

                var result = await selectWorkoutDialog.ShowAsync();
                if (result != ContentDialogResult.Primary || workoutListView.SelectedItem is not WorkoutModel selected)
                {
                    return;
                }

                // 3) POST to calendar/userworkout
                var today = DateTime.Now.Date;
                var uw = new UserWorkoutModel(
                    userId: currentUserId,
                    workoutId: selected.WID,
                    date: today,
                    completed: false);

                await calendarService.AddUserWorkoutAsync(uw);

                // 4) Refresh UI exactly like your calendar page does
                //    or store to LocalSettings if you still want that fallback
                LoadTodaysWorkout();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding workout: {ex}");
                var dlg = new ContentDialog
                {
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await dlg.ShowAsync();
            }
        }
        private void CompleteWorkoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentWorkout != null)
            {
                // For testing, just remove the workout from local settings
                Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove("TodaysWorkoutId");

                // Reload to show the "no active workout" state
                LoadTodaysWorkout();
            }
        }

        private void DeleteWorkoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentWorkout != null)
            {
                // For testing, just remove the workout from local settings
                Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove("TodaysWorkoutId");

                // Reload to show the "no active workout" state
                LoadTodaysWorkout();
            }
        }

        // Navigation methods - you already have these implemented
        public void GoToMainPage_Tap(object sender, RoutedEventArgs e)
        {
            // this.Frame.Navigate(typeof(MainPage));
        }

        public void GoToWorkoutPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WorkoutPage));
        }

        public void GoToCalendarPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CalendarPage));
        }

        public void GoToClassPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ClassPage));
        }

        public void GoToRankingPage_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RankingPage));
        }

        public void GoToShopHomePage_Tap(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(RankingPage));
        }

        public void GoToWishlistPage_Tap(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(RankingPage));
        }

        public void GoToCartPage_Tap(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(RankingPage));
        }
    }
}