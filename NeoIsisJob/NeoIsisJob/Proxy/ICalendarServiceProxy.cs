using System.Collections.Generic;
using System.Threading.Tasks;
using NeoIsisJob.Models;
using Refit;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.Proxy
{
    public interface ICalendarServiceProxy : ICalendarService
    {
        [Get("/api/calendar/{userId}/{year}/{month}")]
        Task<List<CalendarDay>> GetCalendarDaysForMonthAsync(int userId, int year, int month);

        [Get("/api/calendar/userworkout/{userId}/{date}")]
        Task<UserWorkoutModel> GetUserWorkoutAsync(int userId, string date); // Format: yyyy-MM-dd

        [Delete("/api/calendar/userworkout/{userId}/{workoutId}/{date}")]
        Task DeleteUserWorkoutAsync(int userId, int workoutId, string date);

        [Post("/api/calendar/userworkout")]
        Task AddUserWorkoutAsync([Body] UserWorkoutModel userWorkout);

        [Put("/api/calendar/userworkout")]
        Task UpdateUserWorkoutAsync([Body] UserWorkoutModel userWorkout);

        [Get("/api/calendar/userclass/{userId}/{date}")]
        Task<string> GetUserClassAsync(int userId, string date); // Format: yyyy-MM-dd

        [Get("/api/calendar/workouts")]
        Task<List<WorkoutModel>> GetWorkoutsAsync();
    }
}
