using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
// using NeoIsisJob.Models;
// using NeoIsisJob.Services;
// using NeoIsisJob.Services.Interfaces;
using NeoIsisJob.Commands;
using Workout.Core.Models;
using System.Net.Http;
using System;
using NeoIsisJob.Helpers;
using NeoIsisJob.Proxy;

namespace NeoIsisJob.ViewModels.Workout
{
    public class WorkoutViewModel : INotifyPropertyChanged
    {
        // Use concrete proxy implementations
        private readonly WorkoutServiceProxy workoutService;
        private readonly WorkoutTypeServiceProxy workoutTypeService;
        private readonly CompleteWorkoutServiceProxy completeWorkoutService;
        private readonly ExerciseServiceProxy exerciseService;
        private ObservableCollection<WorkoutModel> workouts;
        private ObservableCollection<WorkoutTypeModel> workoutTypes;
        private WorkoutTypeModel selectedWorkoutType;

        // Add SelectedWorkoutViewModel as a property
        public SelectedWorkoutViewModel SelectedWorkoutViewModel { get; }

        // Add commands for deleting, updating, and closing the edit popup
        public ICommand DeleteWorkoutCommand { get; }
        public ICommand UpdateWorkoutCommand { get; }
        public ICommand CloseEditPopupCommand { get; }

        // Default constructor
        public WorkoutViewModel()
        {
            this.workoutTypeService = new WorkoutTypeServiceProxy();
            this.exerciseService = new ExerciseServiceProxy();
            this.workoutService = new WorkoutServiceProxy();
            this.completeWorkoutService = new CompleteWorkoutServiceProxy();

            Workouts = new ObservableCollection<WorkoutModel>();
            WorkoutTypes = new ObservableCollection<WorkoutTypeModel>();

            // Initialize SelectedWorkoutViewModel with dependencies
            SelectedWorkoutViewModel = new SelectedWorkoutViewModel();

            // Initialize commands
            DeleteWorkoutCommand = new RelayCommand<int>(DeleteWorkout);
            UpdateWorkoutCommand = new RelayCommand<string>(UpdateWorkout);
            CloseEditPopupCommand = new RelayCommand(CloseEditPopup);

            // Load workouts and workout types
            LoadWorkouts();
            LoadWorkoutTypes();
        }

        // Add properties for Workouts, WorkoutTypes, and SelectedWorkoutType
        public ObservableCollection<WorkoutModel> Workouts
        {
            get => workouts;
            set
            {
                workouts = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<WorkoutTypeModel> WorkoutTypes
        {
            get => workoutTypes;
            set
            {
                workoutTypes = value;
                OnPropertyChanged();
            }
        }

        public WorkoutTypeModel SelectedWorkoutType
        {
            get => selectedWorkoutType;
            set
            {
                selectedWorkoutType = value;
                OnPropertyChanged();
                ApplyWorkoutFilter();
            }
        }

        // Expose SelectedWorkout from SelectedWorkoutViewModel
        public WorkoutModel SelectedWorkout
        {
            get => SelectedWorkoutViewModel.SelectedWorkout;
        }

        private bool isEditPopupOpen;
        public bool IsEditPopupOpen
        {
            get => isEditPopupOpen;
            set
            {
                isEditPopupOpen = value;
                OnPropertyChanged();
            }
        }

        private async void LoadWorkouts()
        {
            Workouts.Clear();

            foreach (var workout in await this.workoutService.GetAllWorkoutsAsync())
            {
                Workouts.Add(workout);
            }
        }

        private async void LoadWorkoutTypes()
        {
            WorkoutTypes.Clear();
            foreach (var workoutType in await this.workoutTypeService.GetAllWorkoutTypesAsync())
            {
                WorkoutTypes.Add(workoutType);
            }
        }

        private async void ApplyWorkoutFilter()
        {
            Workouts.Clear();
            IList<WorkoutModel> allWorkouts = await this.workoutService.GetAllWorkoutsAsync();

            if (SelectedWorkoutType != null)
            {
                foreach (WorkoutModel workout in allWorkouts.Where(w => w.WTID == SelectedWorkoutType.WTID))
                {
                    Workouts.Add(workout);
                }
            }
            else
            {
                foreach (WorkoutModel workout in allWorkouts)
                {
                    Workouts.Add(workout);
                }
            }
        }

        public void ApplyWorkoutTypeFilter(WorkoutTypeModel selectedType, bool isChecked)
        {
            if (isChecked)
            {
                SelectedWorkoutType = selectedType;
            }
            else
            {
                SelectedWorkoutType = null;
            }
        }

        public async void DeleteWorkout(int workoutId)
        {
            // Delete the selected workout and its complete workouts
            await this.completeWorkoutService.DeleteCompleteWorkoutsByWorkoutIdAsync(workoutId);
            await this.workoutService.DeleteWorkoutAsync(workoutId);

            // Loading workouts again
            LoadWorkouts();
        }

        public async void UpdateWorkout(string newName)
        {
            // Update the selected workout's name
            if (SelectedWorkout != null && !string.IsNullOrWhiteSpace(newName))
            {
                SelectedWorkout.Name = newName;
                await workoutService.UpdateWorkoutAsync(SelectedWorkout);
                LoadWorkouts();
                IsEditPopupOpen = false;
            }
        }

        private void CloseEditPopup()
        {
            // Close the edit popup
            IsEditPopupOpen = false;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}