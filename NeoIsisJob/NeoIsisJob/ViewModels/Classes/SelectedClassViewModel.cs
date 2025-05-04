using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
// using NeoIsisJob.Models;
// using NeoIsisJob.Services;
using Workout.Core.Models;
using Workout.Core.Services;

namespace NeoIsisJob.ViewModels.Classes
{
    public class SelectedClassViewModel : INotifyPropertyChanged
    {
        private readonly ClassService classService;
        private readonly UserClassService userClassService;
        private ClassModel selectedClass;
        private ObservableCollection<UserClassModel> userClasses;

        public ClassModel SelectedClass
        {
            get => selectedClass;
            set
            {
                selectedClass = value;
                // signal that the property has changed
                Debug.WriteLine($"SelectedClass set to: {selectedClass?.Name}"); // Debug message
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // gets triggered every time a property changes
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}