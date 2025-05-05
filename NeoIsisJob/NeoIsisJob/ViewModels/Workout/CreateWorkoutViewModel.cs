using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using NeoIsisJob.Commands;
using NeoIsisJob.Helpers;
using NeoIsisJob.Proxy;
using Refit;


// using NeoIsisJob.Models;
// using NeoIsisJob.Services;
using Workout.Core.Models;
using Workout.Core.Services;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.ViewModels.Workout
{
    public class CreateWorkoutViewModel : INotifyPropertyChanged
    {
        private readonly Frame frame;

        private readonly IWorkoutTypeService workoutTypeService;
        private readonly IExerciseService exerciseService;
        private readonly IMuscleGroupService muscleGroupService;
        private readonly IWorkoutService workoutService;
        private readonly ICompleteWorkoutService completeWorkoutService;
        private ObservableCollection<WorkoutTypeModel> workoutTypes;
        private ObservableCollection<ExercisesModel> exercises;

        // for the add functionality
        private string selectedWorkoutName;
        private WorkoutTypeModel selectedWorkoutType;
        private ObservableCollection<ExercisesModel> selectedExercises;
        private int selectedNumberOfSets;
        private int selectedNumberOfRepsPerSet;

        public ObservableCollection<WorkoutTypeModel> WorkoutTypes
        {
            get
            {
                return workoutTypes;
            }
            set
            {
                workoutTypes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ExercisesModel> Exercises
        {
            get
            {
                return exercises;
            }
            set
            {
                exercises = value;
                OnPropertyChanged();
            }
        }

        // property for the current selected workout type
        public WorkoutTypeModel SelectedWorkoutType
        {
            get
            {
                return selectedWorkoutType;
            }
            set
            {
                selectedWorkoutType = value;
                OnPropertyChanged();
            }
        }

        public string SelectedWorkoutName
        {
            get
            {
                return selectedWorkoutName;
            }
            set
            {
                selectedWorkoutName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ExercisesModel> SelectedExercises
        {
            get
            {
                return selectedExercises;
            }
            set
            {
                selectedExercises = value;
                OnPropertyChanged();
            }
        }

        public int SelectedNumberOfSets
        {
            get
            {
                return selectedNumberOfSets;
            }
            set
            {
                selectedNumberOfSets = value;
                OnPropertyChanged();
            }
        }

        public int SelectedNumberOfRepsPerSet
        {
            get
            {
                return selectedNumberOfRepsPerSet;
            }
            set
            {
                selectedNumberOfRepsPerSet = value;
                OnPropertyChanged();
            }
        }

        // command for add
        public ICommand CreateWorkoutAndCompleteWorkoutsCommand { get; }

        // command for cancel
        public ICommand CancelCommand { get; }

        public CreateWorkoutViewModel(Frame frame)
        {
            // pass the frame to the viewModel
            this.frame = frame;
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ServerHelpers.SERVER_BASE_URL)
            };

            this.workoutTypeService = RestService.For<IWorkoutTypeServiceProxy>(httpClient);
            this.exerciseService = RestService.For<IExerciseServiceProxy>(httpClient);
            this.muscleGroupService = RestService.For<IMuscleGroupServiceProxy>(httpClient);
            this.workoutService = RestService.For<IWorkoutServiceProxy>(httpClient);
            this.completeWorkoutService = RestService.For<ICompleteWorkoutServiceProxy>(httpClient);
            this.WorkoutTypes = new ObservableCollection<WorkoutTypeModel>();
            this.Exercises = new ObservableCollection<ExercisesModel>();
            this.SelectedExercises = new ObservableCollection<ExercisesModel>();

            // initialize the commands
            CreateWorkoutAndCompleteWorkoutsCommand = new RelayCommand(CreateWorkoutAndCompleteWorkouts);
            CancelCommand = new RelayCommand(Cancel);

            LoadWorkoutTypes();
            LoadExercises();
        }

        public async void LoadWorkoutTypes()
        {
            WorkoutTypes.Clear();

            foreach (WorkoutTypeModel workoutType in await this.workoutTypeService.GetAllWorkoutTypesAsync())
            {
                this.WorkoutTypes.Add(workoutType);
            }
        }

        public async void LoadExercises()
        {
            Exercises.Clear();

            foreach (ExercisesModel exercise in await this.exerciseService.GetAllExercisesAsync())
            {
                // add the corresponding MuscleGroup object to every one
                exercise.MuscleGroup = await this.muscleGroupService.GetMuscleGroupByIdAsync(exercise.MuscleGroupId);
                this.Exercises.Add(exercise);
            }
        }

        // function that will serve a command bound to the save button
        public async void CreateWorkoutAndCompleteWorkouts()
        {
            // save the workout and then save all entries in CompleteWorkouts

            // here add the workout
            await this.workoutService.InsertWorkoutAsync(SelectedWorkoutName, SelectedWorkoutType.Id);
            // int selectedWorkoutId = await this.workoutService.GetWorkoutByNameAsync(SelectedWorkoutName).Id;
            var workout = await this.workoutService.GetWorkoutByNameAsync(SelectedWorkoutName);
            int selectedWorkoutId = workout.Id;

            // here add all the entries in CompleteWorkouts
            foreach (ExercisesModel exercise in SelectedExercises)
            {
                await this.completeWorkoutService.InsertCompleteWorkoutAsync(selectedWorkoutId, exercise.Id, SelectedNumberOfSets, SelectedNumberOfRepsPerSet);
            }

            // now go to back to the prev page
            if (this.frame.CanGoBack)
            {
                this.frame.GoBack();
            }
        }

        public void Cancel()
        {
            if (this.frame.CanGoBack)
            {
                this.frame.GoBack();
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        // gets triggered every time a property changes
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
