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
using Workout.Core.IServices;
using NeoIsisJob.Helpers;
using System.Net.Http;
using Refit;
using NeoIsisJob.Proxy;

namespace NeoIsisJob.ViewModels.Classes
{
    public class ClassesViewModel : INotifyPropertyChanged
    {
        private readonly ClassServiceProxy classService;
        private readonly ClassTypeServiceProxy classTypeService;
        private readonly PersonalTrainerServiceProxy personalTrainerService;
        private readonly UserClassServiceProxy userClassService;
        private ObservableCollection<ClassModel> classes;
        private ObservableCollection<ClassTypeModel> classTypes;
        private ObservableCollection<PersonalTrainerModel> personalTrainers;
        private DateTimeOffset selectedDate = DateTimeOffset.Now;
        private ClassTypeModel selectedClassType;
        private bool isLoading;
        private string errorMessage;

        public bool HasClasses => Classes?.Count > 0;
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        public SelectedClassViewModel SelectedClassViewModel { get; }
        public ICommand CloseRegisterPopupCommand { get; }
        public ICommand OpenRegisterPopupCommand { get; }
        public ICommand ConfirmRegistrationCommand { get; }
        public ICommand RefreshCommand { get; }

        public ClassesViewModel()
        {
            Debug.WriteLine("[ClassesViewModel] Initializing");
            
            // Initialize proxies
            this.classService = new ClassServiceProxy();
            this.classTypeService = new ClassTypeServiceProxy();
            this.personalTrainerService = new PersonalTrainerServiceProxy();
            this.userClassService = new UserClassServiceProxy();
            
            // Initialize collections
            Classes = new ObservableCollection<ClassModel>();
            ClassTypes = new ObservableCollection<ClassTypeModel>();
            PersonalTrainers = new ObservableCollection<PersonalTrainerModel>();

            // Initialize commands
            ConfirmRegistrationCommand = new RelayCommand(ConfirmRegistration);
            CloseRegisterPopupCommand = new RelayCommand(CloseRegisterPopup);
            OpenRegisterPopupCommand = new RelayCommand<ClassModel>(OpenRegisterPopup);
            RefreshCommand = new RelayCommand(async () => await InitializeDataAsync());
            
            // Load data
            InitializeDataAsync().ConfigureAwait(false);
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
        
        private async Task InitializeDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                
                Debug.WriteLine("[ClassesViewModel] Loading data...");
                
                // Load classes and class types in parallel
                await Task.WhenAll(
                    LoadClassesAsync(),
                    LoadClassTypesAsync()
                );
                
                Debug.WriteLine("[ClassesViewModel] Data loaded successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ClassesViewModel] Error loading data: {ex.Message}");
                ErrorMessage = $"Error loading data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
        
        private async Task LoadClassesAsync()
        {
            try
            {
                Debug.WriteLine("[ClassesViewModel] Loading classes...");
                Classes.Clear();

                // Get all personal trainers first and create a lookup dictionary
                var trainers = await personalTrainerService.GetAllPersonalTrainersAsync();
                Debug.WriteLine($"[ClassesViewModel] Loaded {trainers.Count} trainers");
                var trainersDict = trainers.ToDictionary(trainer => trainer.PTID);

                // Load all classes
                var allClasses = await classService.GetAllClassesAsync();
                Debug.WriteLine($"[ClassesViewModel] Loaded {allClasses.Count} classes");

                // Associate trainers with classes and add to the collection
                foreach (var classItem in allClasses)
                {
                    // Assign Personal Trainer to Class
                    if (trainersDict.TryGetValue(classItem.PTID, out var trainer))
                    {
                        classItem.PersonalTrainer = trainer;
                    }

                    Classes.Add(classItem);
                }
                
                // Update HasClasses property
                OnPropertyChanged(nameof(HasClasses));
                Debug.WriteLine("[ClassesViewModel] Classes loaded successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ClassesViewModel] Error loading classes: {ex.Message}");
                throw; // Let the parent method handle the exception
            }
        }

        private async Task LoadClassTypesAsync()
        {
            try
            {
                Debug.WriteLine("[ClassesViewModel] Loading class types...");
                ClassTypes.Clear();
                
                var classTypes = await this.classTypeService.GetAllClassTypesAsync();
                Debug.WriteLine($"[ClassesViewModel] Loaded {classTypes.Count} class types");
                
                foreach (var classType in classTypes)
                {
                    ClassTypes.Add(classType);
                }
                
                Debug.WriteLine("[ClassesViewModel] Class types loaded successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ClassesViewModel] Error loading class types: {ex.Message}");
                throw; // Let the parent method handle the exception
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
                DateError = "No class selected.";
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
                Debug.WriteLine($"[ClassesViewModel] Registering user {currentUserId} for class {SelectedClass.CID} on {SelectedDate.Date}");
                
                var userClass = new UserClassModel
                {
                    UID = currentUserId,
                    CID = SelectedClass.CID,
                    Date = SelectedDate.Date
                };

                await userClassService.AddUserClassAsync(userClass);
                DateError = string.Empty; // Clear error if successful
                Debug.WriteLine($"[ClassesViewModel] Successfully registered for class {SelectedClass.Name}");
                IsRegisterPopupOpen = false;
            }
            catch (Exception ex)
            {
                DateError = $"Registration failed: {ex.Message}";
                Debug.WriteLine($"[ClassesViewModel] Registration failed: {ex.Message}");
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