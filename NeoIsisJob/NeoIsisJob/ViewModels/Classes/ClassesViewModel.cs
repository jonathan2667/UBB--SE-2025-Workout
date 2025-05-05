using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using System;
using System.Diagnostics;
// using NeoIsisJob.Models;
// using NeoIsisJob.Services;
using NeoIsisJob.Commands;

using Workout.Core.Models;
using Workout.Core.Services;
using Workout.Core.Services.Interfaces;
using NeoIsisJob.Helpers;
using System.Net.Http;
using Refit;
using NeoIsisJob.Proxy;

namespace NeoIsisJob.ViewModels.Classes
{
    public class ClassesViewModel : INotifyPropertyChanged
    {
        private readonly IClassService classService;
        private readonly IClassTypeService classTypeService;
        private readonly IPersonalTrainerService personalTrainerService;
        private readonly IUserClassService userClassService;
        private ObservableCollection<ClassModel> classes;
        private ObservableCollection<ClassTypeModel> classTypes;
        private ObservableCollection<PersonalTrainerModel> personalTrainers;
        private DateTimeOffset selectedDate = DateTimeOffset.Now;
        private ClassTypeModel selectedClassType;
        public bool HasClasses => Classes?.Count > 0;
        public SelectedClassViewModel SelectedClassViewModel { get; }
        public ICommand CloseRegisterPopupCommand { get; }
        public ICommand OpenRegisterPopupCommand { get; }
        public ICommand ConfirmRegistrationCommand { get; }
        public ClassesViewModel()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ServerHelpers.SERVER_BASE_URL)
            };

            this.classService = RestService.For<IClassServiceProxy>(httpClient);
            this.classTypeService = RestService.For<IClassTypeServiceProxy>(httpClient);
            this.personalTrainerService = RestService.For<IPersonalTrainerServiceProxy>(httpClient);
            this.userClassService = RestService.For<IUserClassServiceProxy>(httpClient);
            Classes = new ObservableCollection<ClassModel>();
            ClassTypes = new ObservableCollection<ClassTypeModel>();
            PersonalTrainers = new ObservableCollection<PersonalTrainerModel>();

            ConfirmRegistrationCommand = new RelayCommand(ConfirmRegistration);
            CloseRegisterPopupCommand = new RelayCommand(CloseRegisterPopup);
            OpenRegisterPopupCommand = new RelayCommand<ClassModel>(OpenRegisterPopup);
            _ = LoadClasses();
            _ = LoadClassTypes();
        }

        public ObservableCollection<ClassModel> Classes
        {
            get => classes;
            set
            {
                classes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasClasses));
            }
        }

        public ObservableCollection<ClassTypeModel> ClassTypes
        {
            get => classTypes;
            set
            {
                classTypes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PersonalTrainerModel> PersonalTrainers
        {
            get => personalTrainers;
            set
            {
                personalTrainers = value;
                OnPropertyChanged();
            }
        }

        private ClassModel selectedClass;
        public ClassModel SelectedClass
        {
            get => selectedClass;
            set
            {
                selectedClass = value;
                OnPropertyChanged();
            }
        }

        private bool isRegisterPopupOpen;
        public bool IsRegisterPopupOpen
        {
            get => isRegisterPopupOpen;
            set
            {
                isRegisterPopupOpen = value;
                OnPropertyChanged();
            }
        }
        public DateTimeOffset SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                OnPropertyChanged();
            }
        }

        private void OpenRegisterPopup(ClassModel classModel)
        {
            SelectedClass = classModel;
            IsRegisterPopupOpen = true;
        }
        private async Task LoadClasses()
        {
            Classes.Clear();

            var trainersDict = (await personalTrainerService.GetAllPersonalTrainersAsync()).ToDictionary(personalTrainer => personalTrainer.PTID);

            foreach (var classItem in await classService.GetAllClassesAsync())
            {
                // Assign Personal Trainer to Class
                if (trainersDict.TryGetValue(classItem.PTID, out var trainer))
                {
                    classItem.PersonalTrainer = trainer;
                }

                Classes.Add(classItem);
            }
            OnPropertyChanged(nameof(HasClasses));
        }

        private async Task LoadClassTypes()
        {
            ClassTypes.Clear();
            foreach (var classType in await this.classTypeService.GetAllClassTypesAsync())
            {
                ClassTypes.Add(classType);
            }
        }
        private string dateError;
        public string DateError
        {
            get => dateError;
            set
            {
                dateError = value;
                OnPropertyChanged();
            }
        }

        private async void ConfirmRegistration()
        {
            if (SelectedClass == null)
            {
                return;
            }

            // Validate date is not in the past
            if (SelectedDate.Date < DateTime.Today)
            {
                DateError = "Please choose a valid date (today or future)";
                return;
            }

            try
            {
                int currentUserId = GetCurrentUserId();
                var userClass = new UserClassModel
                {
                    UID = currentUserId,
                    CID = SelectedClass.CID,
                    Date = SelectedDate.Date
                };

                await userClassService.AddUserClassAsync(userClass);
                DateError = string.Empty; // Clear error if successful
                Debug.WriteLine($"Successfully registered for class {SelectedClass.Name}");
                IsRegisterPopupOpen = false;
            }
            catch (Exception ex)
            {
                DateError = $"Registration failed: {ex.Message}";
                Debug.WriteLine(DateError);
            }
        }

        private int currentUserId = 1;

        public int CurrentUserId
        {
            get => currentUserId;
            set
            {
                currentUserId = value;
                OnPropertyChanged();
            }
        }
        private int GetCurrentUserId()
        {
            if (currentUserId <= 0)
            {
                throw new InvalidOperationException("No valid user is set");
            }
            return currentUserId;
        }
        private void CloseRegisterPopup()
        {
            // Close the edit popup
            IsRegisterPopupOpen = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}