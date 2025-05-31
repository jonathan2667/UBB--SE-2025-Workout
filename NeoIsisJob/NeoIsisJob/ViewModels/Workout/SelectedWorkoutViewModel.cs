using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Workout.Core.Models;
using Workout.Core.Services;
using Workout.Core.IServices;
using Workout.Core.IServices;
using NeoIsisJob.Helpers;
using NeoIsisJob.Proxy;
using Refit;

namespace NeoIsisJob.ViewModels.Workout
{
    public class SelectedWorkoutViewModel : INotifyPropertyChanged
    {
        private readonly WorkoutServiceProxy workoutService;
        private readonly ExerciseServiceProxy exerciseService;
        private readonly CompleteWorkoutServiceProxy completeWorkoutService;
        private WorkoutModel selectedWorkout;
        private ObservableCollection<CompleteWorkoutModel> completeWorkouts;

        public WorkoutModel SelectedWorkout
        {
            get => selectedWorkout;
        }

        public async Task SetSelectedWorkoutAsync(WorkoutModel workout)
        {
            Debug.WriteLine($"SetSelectedWorkoutAsync called with workout: {workout?.Name}");
            selectedWorkout = workout;
            OnPropertyChanged(nameof(SelectedWorkout));

            if (selectedWorkout != null)
            {
                Debug.WriteLine($"Fetching complete workouts for workout ID: {selectedWorkout.WID}");
                var completeWorkoutsRaw = await this.completeWorkoutService.GetCompleteWorkoutsByWorkoutIdAsync(selectedWorkout.WID);
                Debug.WriteLine($"Found {completeWorkoutsRaw?.Count ?? 0} complete workouts");
                
                var completeWorkoutsFilled = await FilledCompleteWorkoutsWithExercies(completeWorkoutsRaw);
                Debug.WriteLine($"Filled {completeWorkoutsFilled?.Count ?? 0} complete workouts with exercises");
                
                CompleteWorkouts = new ObservableCollection<CompleteWorkoutModel>(completeWorkoutsFilled);
                Debug.WriteLine($"Set CompleteWorkouts collection with {CompleteWorkouts.Count} items");
            }
            else
            {
                Debug.WriteLine("Selected workout is null, clearing CompleteWorkouts");
                CompleteWorkouts.Clear();
            }
        }

        public ObservableCollection<CompleteWorkoutModel> CompleteWorkouts
        {
            get => completeWorkouts;
            set
            {
                completeWorkouts = value;
                Debug.WriteLine($"CompleteWorkouts property changed, new count: {completeWorkouts?.Count ?? 0}");
                OnPropertyChanged();
            }
        }

        // Default constructor
        public SelectedWorkoutViewModel()
        {
            Debug.WriteLine("SelectedWorkoutViewModel constructor called");
            this.workoutService = new WorkoutServiceProxy();
            this.exerciseService = new ExerciseServiceProxy();
            this.completeWorkoutService = new CompleteWorkoutServiceProxy();
            this.completeWorkouts = new ObservableCollection<CompleteWorkoutModel>();
        }

        public async Task<IList<CompleteWorkoutModel>> FilledCompleteWorkoutsWithExercies(IList<CompleteWorkoutModel> complWorkouts)
        {
            Debug.WriteLine($"Filling {complWorkouts?.Count ?? 0} complete workouts with exercises");
            foreach (CompleteWorkoutModel complWorkout in complWorkouts)
            {
                complWorkout.Exercise = await this.exerciseService.GetExerciseByIdAsync(complWorkout.EID);
                Debug.WriteLine($"Filled exercise for workout: {complWorkout.Exercise?.Name}");
            }

            return complWorkouts;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        // gets triggered every time a property changes
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Debug.WriteLine($"Property changed: {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void UpdateWorkoutName(string newName)
        {
            try
            {
                if (selectedWorkout == null || string.IsNullOrWhiteSpace(newName))
                {
                    throw new InvalidOperationException("Workout cannot be null and name cannot be empty or null.");
                }

                selectedWorkout.Name = newName;
                await this.workoutService.UpdateWorkoutAsync(selectedWorkout);

                // Notify the UI about the change
                OnPropertyChanged(nameof(SelectedWorkout));

                // Reload the CompleteWorkouts collection
                IList<CompleteWorkoutModel> complWorkouts = await FilledCompleteWorkoutsWithExercies(await this.completeWorkoutService.GetCompleteWorkoutsByWorkoutIdAsync(this.selectedWorkout.WID));
                CompleteWorkouts = new ObservableCollection<CompleteWorkoutModel>(complWorkouts);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating workout name: {ex.Message}");
                throw new Exception($"An error occurred while updating the workout: {ex.Message}", ex);
            }
        }
    }
}
