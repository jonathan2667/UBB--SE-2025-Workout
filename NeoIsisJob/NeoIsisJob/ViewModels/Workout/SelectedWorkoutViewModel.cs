using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Diagnostics;
// using NeoIsisJob.Models;
// using NeoIsisJob.Services;
// using NeoIsisJob.Services.Interfaces;
using Workout.Core.Models;
using Workout.Core.Services;
using Workout.Core.IServices;
using Workout.Core.IServices;
using NeoIsisJob.Helpers;
using System.Net.Http;
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
            selectedWorkout = workout;
            Debug.WriteLine($"SelectedWorkout set to: {selectedWorkout?.Name}");
            OnPropertyChanged(nameof(SelectedWorkout));

            if (selectedWorkout != null)
            {
                var completeWorkoutsRaw = await this.completeWorkoutService.GetCompleteWorkoutsByWorkoutIdAsync(selectedWorkout.WID);
                var completeWorkoutsFilled = await FilledCompleteWorkoutsWithExercies(completeWorkoutsRaw);
                CompleteWorkouts = new ObservableCollection<CompleteWorkoutModel>(completeWorkoutsFilled);
            }
            else
            {
                CompleteWorkouts.Clear();
            }
        }

        public ObservableCollection<CompleteWorkoutModel> CompleteWorkouts
        {
            get => completeWorkouts;
            set
            {
                completeWorkouts = value;
                OnPropertyChanged();
            }
        }

        // Default constructor
        public SelectedWorkoutViewModel()
        {
            this.workoutService = new WorkoutServiceProxy();
            this.exerciseService = new ExerciseServiceProxy();
            this.completeWorkoutService = new CompleteWorkoutServiceProxy();
            this.completeWorkouts = new ObservableCollection<CompleteWorkoutModel>();
        }

        public async Task<IList<CompleteWorkoutModel>> FilledCompleteWorkoutsWithExercies(IList<CompleteWorkoutModel> complWorkouts)
        {
            foreach (CompleteWorkoutModel complWorkout in complWorkouts)
            {
                complWorkout.Exercise = await this.exerciseService.GetExerciseByIdAsync(complWorkout.EID);
            }

            return complWorkouts;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        // gets triggered every time a property changes
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
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
                throw new Exception($"An error occurred while updating the workout: {ex.Message}", ex);
            }
        }
    }
}
