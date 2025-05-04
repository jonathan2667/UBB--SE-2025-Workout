using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;


namespace Workout.Core.Services.Interfaces
{
    public interface ICalendarService
    {
        Task<List<CalendarDay>> GetCalendarDaysForMonthAsync(int userId, DateTime date);

        Task<ObservableCollection<CalendarDay>> GetCalendarDaysAsync(int userId, DateTime currentDate);

        Task AddUserWorkoutAsync(UserWorkoutModel userWorkout);

        Task UpdateUserWorkoutAsync(UserWorkoutModel userWorkout);

        Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date);

        Task<UserWorkoutModel> GetUserWorkoutAsync(int userId, DateTime date);

        /// <summary>
        /// Removes a workout for the specified day (if one exists).
        /// </summary>
        Task RemoveWorkoutAsync(int userId, CalendarDay day);

        /// <summary>
        /// Changes (replaces) a workout for the specified day.
        /// </summary>
        Task ChangeWorkoutAsync(int userId, CalendarDay day);

        /// <summary>
        /// Pure in-memory count; no need for async.
        /// </summary>
        string GetWorkoutDaysCountText(ObservableCollection<CalendarDay> calendarDays);

        /// <summary>
        /// Pure in-memory count; no need for async.
        /// </summary>
        string GetDaysCountText(ObservableCollection<CalendarDay> calendarDays);

        Task<string> GetUserClassAsync(int userId, DateTime date);

        Task<List<WorkoutModel>> GetWorkoutsAsync();
    }
}

