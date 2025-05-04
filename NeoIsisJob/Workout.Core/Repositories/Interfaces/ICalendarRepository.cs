using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    public interface ICalendarRepository
    {
        Task<List<CalendarDay>> GetCalendarDaysForMonthAsync(int userId, DateTime month);
        Task<UserWorkoutModel?> GetUserWorkoutAsync(int userId, DateTime date);
        Task<List<WorkoutModel>> GetWorkoutsAsync();
        Task<string?> GetUserClassAsync(int userId, DateTime date);
    }
}
