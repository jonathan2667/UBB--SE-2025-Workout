using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
// using NeoIsisJob.Models;
// using NeoIsisJob.Services;
using Workout.Core.Models;
using Workout.Core.Services;
using Workout.Core.IServices;
using NeoIsisJob.Proxy;

namespace NeoIsisJob.ViewModels.Classes
{
    public class SelectedClassViewModel : INotifyPropertyChanged
    {
        private readonly ClassServiceProxy classService;
        private readonly UserClassServiceProxy userClassService;
        private ClassModel selectedClass;
        private ObservableCollection<UserClassModel> userClasses;

        public SelectedClassViewModel()
        {
            this.classService = new ClassServiceProxy();
            this.userClassService = new UserClassServiceProxy();
        }

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