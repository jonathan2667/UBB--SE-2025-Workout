using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
// using NeoIsisJob.Models;
// using NeoIsisJob.Data;
// using NeoIsisJob.Repositories;
// using NeoIsisJob.Services;
// using NeoIsisJob.Services.Interfaces;
using Workout.Core.Models;
using Workout.Core.Data;
using Workout.Core.Services;
using Workout.Core.IServices;
using NeoIsisJob.Proxy;
using NeoIsisJob.Helpers;

namespace NeoIsisJob.ViewModels.Calendar
{
    public class CalendarViewModel : INotifyPropertyChanged
    {
        private DateTime currentDate;
        private string yearText;
        private string monthText;
        private ObservableCollection<CalendarDayModel> calendarDays;
        private bool isLoading;
        private string errorMessage;

        private readonly CalendarServiceProxy calendarService;

        public readonly int UserId;
        public event PropertyChangedEventHandler PropertyChanged;

        public CalendarViewModel(int userId)
        {
            Debug.WriteLine($"[CalendarViewModel] Initializing with userId: {userId}");
            this.UserId = userId;
            currentDate = DateTime.Now;

            this.calendarService = new CalendarServiceProxy();
            CalendarDays = new ObservableCollection<CalendarDayModel>();
            PreviousMonthCommand = new RelayCommand(PreviousMonth);
            NextMonthCommand = new RelayCommand(NextMonth);
            RefreshCommand = new RelayCommand(RefreshCalendar);
            InitializeCalendarAsync().ConfigureAwait(false);
        }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public string YearText
        {
            get => yearText;
            set
            {
                yearText = value;
                OnPropertyChanged(nameof(YearText));
            }
        }

        public string MonthText
        {
            get => monthText;
            set
            {
                monthText = value;
                OnPropertyChanged(nameof(MonthText));
            }
        }

        public ObservableCollection<CalendarDayModel> CalendarDays
        {
            get => calendarDays;
            set
            {
                calendarDays = value;
                OnPropertyChanged(nameof(CalendarDays));
                OnPropertyChanged(nameof(WorkoutDaysCountText));
                OnPropertyChanged(nameof(DaysCountText));
            }
        }

        public string WorkoutDaysCountText => calendarService.GetWorkoutDaysCountText(CalendarDays);
        public string DaysCountText => calendarService.GetDaysCountText(CalendarDays);
        public ICommand PreviousMonthCommand { get; }
        public ICommand NextMonthCommand { get; }
        public ICommand RefreshCommand { get; }

        private async Task InitializeCalendarAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                
                Debug.WriteLine($"[CalendarViewModel] Initializing calendar for {currentDate:yyyy-MM}");
                
                await UpdateCalendarInternalAsync();
                
                Debug.WriteLine("[CalendarViewModel] Calendar initialized successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CalendarViewModel] Error initializing calendar: {ex.Message}");
                ErrorMessage = $"Failed to load calendar: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void UpdateCalendar()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                
                Debug.WriteLine($"[CalendarViewModel] Updating calendar for {currentDate:yyyy-MM}");
                
                await UpdateCalendarInternalAsync();
                
                Debug.WriteLine("[CalendarViewModel] Calendar updated successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CalendarViewModel] Error updating calendar: {ex.Message}");
                ErrorMessage = $"Failed to update calendar: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        //private async Task UpdateCalendarInternalAsync()
        //{
        //    YearText = currentDate.Year.ToString();
        //    MonthText = currentDate.ToString("MMMM");

        //    Debug.WriteLine($"[CalendarViewModel] Fetching calendar days for {UserId} on {currentDate:yyyy-MM-dd}");
        //    var days = await calendarService.GetCalendarDaysAsync(UserId, currentDate);
        //    Debug.WriteLine($"[CalendarViewModel] Received {days.Count} calendar days");

        //    CalendarDays = days;
        //}
        private async Task UpdateCalendarInternalAsync()
        {
            YearText = currentDate.Year.ToString();
            MonthText = currentDate.ToString("MMMM");

            Debug.WriteLine($"[CalendarViewModel] Fetching calendar days for {UserId} on {currentDate:yyyy-MM-dd}");
            var days = await calendarService.GetCalendarDaysAsync(UserId, currentDate);
            Debug.WriteLine($"[CalendarViewModel] Received {days.Count} calendar days");

            // ────────────────────────────────────────────────────────────────────────
            // 1) Figure out which column the 1st of the month lands on:
            var firstOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            int firstColumn = (int)firstOfMonth.DayOfWeek;   // Sunday=0, Monday=1, … Saturday=6

            // 2) Assign a grid position to each day:
            for (int i = 0; i < days.Count; i++)
            {
                var day = days[i];
                int slot = firstColumn + i;            // zero-based slot in the calendar
                day.GridRow = slot / 7;             // integer division → which row (0..5)
                day.GridColumn = slot % 7;             // mod 7 → which column (0..6)
            }
            // ────────────────────────────────────────────────────────────────────────

            // Finally, publish to the UI
            CalendarDays = new ObservableCollection<CalendarDayModel>(days);
        }


        private void RefreshCalendar()
        {
            Debug.WriteLine("[CalendarViewModel] Manual refresh requested");
            UpdateCalendar();
        }

        private void PreviousMonth()
        {
            Debug.WriteLine("[CalendarViewModel] Navigating to previous month");
            currentDate = currentDate.AddMonths(-1);
            UpdateCalendar();
        }

        private void NextMonth()
        {
            Debug.WriteLine("[CalendarViewModel] Navigating to next month");
            currentDate = currentDate.AddMonths(1);
            UpdateCalendar();
        }

        public async void AddUserWorkout(UserWorkoutModel userWorkout)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                
                Debug.WriteLine($"[CalendarViewModel] Adding workout {userWorkout.WID} for user {userWorkout.UID} on {userWorkout.Date:yyyy-MM-dd}");
                await calendarService.AddUserWorkoutAsync(userWorkout);
                Debug.WriteLine("[CalendarViewModel] Workout added successfully");
                
                UpdateCalendar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CalendarViewModel] Error adding workout: {ex.Message}");
                ErrorMessage = $"Failed to add workout: {ex.Message}";
                IsLoading = false;
            }
        }

        public async void UpdateUserWorkout(UserWorkoutModel userWorkout)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                
                Debug.WriteLine($"[CalendarViewModel] Updating workout {userWorkout.WID} for user {userWorkout.UID} on {userWorkout.Date:yyyy-MM-dd}");
                await calendarService.UpdateUserWorkoutAsync(userWorkout);
                Debug.WriteLine("[CalendarViewModel] Workout updated successfully");
                
                UpdateCalendar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CalendarViewModel] Error updating workout: {ex.Message}");
                ErrorMessage = $"Failed to update workout: {ex.Message}";
                IsLoading = false;
            }
        }

        public async void DeleteUserWorkout(int workoutId, DateTime date)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                
                Debug.WriteLine($"[CalendarViewModel] Deleting workout {workoutId} for user {UserId} on {date:yyyy-MM-dd}");
                await calendarService.DeleteUserWorkoutAsync(UserId, workoutId, date);
                Debug.WriteLine("[CalendarViewModel] Workout deleted successfully");
                
                UpdateCalendar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CalendarViewModel] Error deleting workout: {ex.Message}");
                ErrorMessage = $"Failed to delete workout: {ex.Message}";
                IsLoading = false;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => canExecute == null || canExecute();

        public void Execute(object parameter) => execute();
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => canExecute == null || canExecute((T)parameter);

        public void Execute(object parameter) => execute((T)parameter);
    }
}