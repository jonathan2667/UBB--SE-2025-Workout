using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NeoIsisJob.ViewModels.Workout;
// using NeoIsisJob.Models;
using Workout.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace NeoIsisJob.Views.Workout
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateWorkoutPage : Page
    {
        private CreateWorkoutViewModel viewModel;

        public CreateWorkoutViewModel ViewModel
        {
            get { return viewModel; }
            set { viewModel = value; }
        }

        public CreateWorkoutPage()
        {
            // bind the view with the view model
            this.InitializeComponent();
            // this.ViewModel = new CreateWorkoutViewModel(this.Frame);
            // this.DataContext = this.ViewModel;
        }

        // necessary so that it is sure that the page is loaded and the frame is available, so it can be passed to the viewmodel!!!
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Now that navigation has occurred, the Frame is initialized
            this.ViewModel = new CreateWorkoutViewModel(this.Frame);
            this.DataContext = this.ViewModel;
        }

        // handler for selection change in the list of exercises
        private void ExercisesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // if the context is CreateWorkoutViewModel
            if (DataContext is CreateWorkoutViewModel viewModel)
            {
                // add the currently selected exercises
                foreach (var item in e.AddedItems)
                {
                    // if it is an exercise and it is not present add
                    if (item is ExercisesModel exercise && !viewModel.SelectedExercises.Contains(exercise))
                    {
                        viewModel.SelectedExercises.Add(exercise);
                    }
                }

                // remove what is not selected anymore
                foreach (var item in e.RemovedItems)
                {
                    if (item is ExercisesModel exercise && viewModel.SelectedExercises.Contains(exercise))
                    {
                        viewModel.SelectedExercises.Remove(exercise);
                    }
                }
            }
        }
    }
}
