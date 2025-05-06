// NeoIsisJob/Proxy/CalendarServiceProxy.cs
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class CalendarServiceProxy : BaseServiceProxy
    {
        private const string BaseRoute = "calendar";

        public CalendarServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        // —— Core async HTTP methods —— //

        // GET /api/calendar/{userId}/{year}/{month}
        public Task<List<CalendarDayModel>> GetCalendarDaysForMonthAsync(int userId, DateTime date)
            => GetAsync<List<CalendarDayModel>>(
                $"{BaseRoute}/{userId}/{date:yyyy}/{date:MM}");

        // GET /api/calendar/{userId}/{year}/{month}/{day}
        public Task<UserWorkoutModel> GetUserWorkoutAsync(int userId, DateTime date)
            => GetAsync<UserWorkoutModel>(
                $"{BaseRoute}/{userId}/{date:yyyy}/{date:MM}/{date:dd}");

        // GET /api/calendar/{userId}/{year}/{month}/{day}/class
        public Task<string> GetUserClassAsync(int userId, DateTime date)
            => GetAsync<string>(
                $"{BaseRoute}/{userId}/{date:yyyy}/{date:MM}/{date:dd}/class");

        // GET /api/calendar/workouts
        public Task<List<WorkoutModel>> GetWorkoutsAsync()
            => GetAsync<List<WorkoutModel>>($"{BaseRoute}/workouts");

        // POST /api/calendar/userworkout
        public Task AddUserWorkoutAsync(UserWorkoutModel uw)
            => PostAsync($"{BaseRoute}/userworkout", uw);

        // PUT /api/calendar/userworkout
        public Task UpdateUserWorkoutAsync(UserWorkoutModel uw)
            => PutAsync($"{BaseRoute}/userworkout", uw);
        // PUT /api/calendar/workout/{userId}/{yyyy}/{MM}/{dd}
        public Task ChangeWorkoutAsync(int userId, CalendarDayModel day)
            => PutAsync(
                $"{BaseRoute}/workout/{userId}/{day.Date:yyyy}/{day.Date:MM}/{day.Date:dd}",
                day);

        // POST /api/calendar/workoutdayscount
        public Task<string> GetWorkoutDaysCountTextAsync(IEnumerable<CalendarDayModel> days)
            => PostAsync<string>(
                $"{BaseRoute}/workoutdayscount",
                days.ToList());

        // POST /api/calendar/dayscount
        public Task<string> GetDaysCountTextAsync(IEnumerable<CalendarDayModel> days)
            => PostAsync<string>(
                $"{BaseRoute}/dayscount",
                days.ToList());

        // —— Synchronous wrappers for your ViewModel —— //

        /// <summary>
        /// Your ViewModel calls this:
        ///     string WorkoutDaysCountText => calendarService.GetWorkoutDaysCountText(CalendarDays);
        /// </summary>
        public string GetWorkoutDaysCountText(ObservableCollection<CalendarDayModel> days)
            => GetWorkoutDaysCountTextAsync(days).GetAwaiter().GetResult();

        /// <summary>
        /// Your ViewModel calls this:
        ///     string DaysCountText => calendarService.GetDaysCountText(CalendarDays);
        /// </summary>
        public string GetDaysCountText(ObservableCollection<CalendarDayModel> days)
            => GetDaysCountTextAsync(days).GetAwaiter().GetResult();

        /// <summary>
        /// Alias for your old GetCalendarDaysAsync signature:
        ///     Task<ObservableCollection<CalendarDayModel>> GetCalendarDaysAsync(int,DateTime)
        /// </summary>
        public async Task<ObservableCollection<CalendarDayModel>> GetCalendarDaysAsync(int userId, DateTime date)
        {
            var list = await GetCalendarDaysForMonthAsync(userId, date);
            return new ObservableCollection<CalendarDayModel>(list);
        }

        ///// <summary>
        ///// Alias for your old RemoveWorkoutAsync signature:
        ///// </summary>
        // public Task RemoveWorkoutAsync(int userId, CalendarDayModel day)
        //    => DeleteAsync(
        //        $"{BaseRoute}/workout/{userId}/{day.Date:yyyy}/{day.Date:MM}/{day.Date:dd}");
        // not sure if its ok yet
    //    public Task RemoveWorkoutAsync(int userId, CalendarDayModel day)
    // => DeleteAsync($"{BaseRoute}/workout/{userId}/{day.Date:yyyy}/{day.Date:MM}/{day.Date:dd}");
        // <summary>
        // DELETE /api/calendar/userworkout/{userId}/{workoutId}/{yyyy}/{MM}/{dd}
        // </summary>
        public Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date)
            => DeleteAsync(
                $"{BaseRoute}/userworkout/{userId}/{workoutId}/{date:yyyy}/{date:MM}/{date:dd}");
    }
}
