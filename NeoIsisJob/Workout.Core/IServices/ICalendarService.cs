using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;


namespace Workout.Core.IServices
{
    public interface ICalendarService
    {
        Task<List<CalendarDayModel>> GetCalendarDaysForMonthAsync(int userId, DateTime date);

        Task<ObservableCollection<CalendarDayModel>> GetCalendarDaysAsync(int userId, DateTime currentDate);

        Task AddUserWorkoutAsync(UserWorkoutModel userWorkout);

        Task UpdateUserWorkoutAsync(UserWorkoutModel userWorkout);

        Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date);

        Task<UserWorkoutModel> GetUserWorkoutAsync(int userId, DateTime date);

        /// <summary>
        /// Removes a workout for the specified day (if one exists).
        /// </summary>
        Task RemoveWorkoutAsync(int userId, CalendarDayModel day);

        /// <summary>
        /// Changes (replaces) a workout for the specified day.
        /// </summary>
        Task ChangeWorkoutAsync(int userId, CalendarDayModel day);

        /// <summary>
        /// Pure in-memory count; no need for async.
        /// </summary>
        string GetWorkoutDaysCountText(ObservableCollection<CalendarDayModel> calendarDays);

        /// <summary>
        /// Pure in-memory count; no need for async.
        /// </summary>
        string GetDaysCountText(ObservableCollection<CalendarDayModel> calendarDays);

        Task<string> GetUserClassAsync(int userId, DateTime date);

        Task<List<WorkoutModel>> GetWorkoutsAsync();
    }
}

