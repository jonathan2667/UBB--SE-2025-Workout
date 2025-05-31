using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NeoIsisJob.ViewModels.Workout;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Extensions.DependencyInjection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace NeoIsisJob.Views.Workout
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectedWorkoutPage : Page
    {
        private SelectedWorkoutViewModel selectedWorkoutViewModel;

        public SelectedWorkoutViewModel ViewModel { get; }

        public SelectedWorkoutPage()
        {
            this.InitializeComponent();
            // take the view model from the app(it is registered as singleton)
            this.ViewModel = App.Services.GetService<SelectedWorkoutViewModel>();
            // set is as data context for the page
            this.DataContext = this.ViewModel;
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // if CanGoBack is true -> navigate back
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}
