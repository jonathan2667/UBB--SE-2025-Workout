//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;
//using Workout.Core.Models;

//namespace NeoIsisJob.Proxy
//{
//    public class CalendarServiceProxy : BaseServiceProxy
//    {
//        private const string EndpointName = "calendar";

//        public CalendarServiceProxy(IConfiguration configuration = null) 
//            : base(configuration)
//        {
//        }

//        public async Task<List<CalendarDayModel>> GetCalendarDaysForMonthAsync(int userId, DateTime date)
//        {
//            try
//            {
//                string formattedDate = date.ToString("yyyy-MM-dd");
//                var results = await GetAsync<List<CalendarDayModel>>($"{EndpointName}/month/{userId}/{formattedDate}");
//                return results ?? new List<CalendarDayModel>();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error fetching calendar days for month: {ex.Message}");
//                return new List<CalendarDayModel>();
//            }
//        }

//        public async Task<ObservableCollection<CalendarDayModel>> GetCalendarDaysAsync(int userId, DateTime currentDate)
//        {
//            try
//            {
//                string formattedDate = currentDate.ToString("yyyy-MM-dd");
//                var results = await GetAsync<List<CalendarDayModel>>($"{EndpointName}/days/{userId}/{formattedDate}");
//                return results != null ? new ObservableCollection<CalendarDayModel>(results) : new ObservableCollection<CalendarDayModel>();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error fetching calendar days: {ex.Message}");
//                return new ObservableCollection<CalendarDayModel>();
//            }
//        }

//        public async Task RemoveWorkoutAsync(int userId, CalendarDayModel day)
//        {
//            try
//            {
//                string formattedDate = day.Date.ToString("yyyy-MM-dd");
//                await DeleteAsync($"{EndpointName}/workout/{userId}/{formattedDate}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error removing workout: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task ChangeWorkoutAsync(int userId, CalendarDayModel day)
//        {
//            try
//            {
//                string formattedDate = day.Date.ToString("yyyy-MM-dd");
//                await PutAsync($"{EndpointName}/workout/{userId}/{formattedDate}", day);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error changing workout: {ex.Message}");
//                throw;
//            }
//        }

//        public string GetWorkoutDaysCountText(ObservableCollection<CalendarDayModel> calendarDays)
//        {
//            return $"Workout Days: {calendarDays.Count(d => d.HasWorkout)}";
//        }

//        public string GetDaysCountText(ObservableCollection<CalendarDayModel> calendarDays)
//        {
//            return $"Days Count: {calendarDays.Count}";
//        }

//        public async Task AddUserWorkoutAsync(UserWorkoutModel userWorkout)
//        {
//            try
//            {
//                await PostAsync($"{EndpointName}/userworkout", userWorkout);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error adding user workout: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task UpdateUserWorkoutAsync(UserWorkoutModel userWorkout)
//        {
//            try
//            {
//                await PutAsync($"{EndpointName}/userworkout", userWorkout);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error updating user workout: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date)
//        {
//            try
//            {
//                string formattedDate = date.ToString("yyyy-MM-dd");
//                await DeleteAsync($"{EndpointName}/userworkout/{userId}/{workoutId}/{formattedDate}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error deleting user workout: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task<UserWorkoutModel> GetUserWorkoutAsync(int userId, DateTime date)
//        {
//            try
//            {
//                string formattedDate = date.ToString("yyyy-MM-dd");
//                var result = await GetAsync<UserWorkoutModel>($"{EndpointName}/userworkout/{userId}/{formattedDate}");
//                return result;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error fetching user workout: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task<string> GetUserClassAsync(int userId, DateTime date)
//        {
//            try
//            {
//                string formattedDate = date.ToString("yyyy-MM-dd");
//                var result = await GetAsync<string>($"{EndpointName}/userclass/{userId}/{formattedDate}");
//                return result;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error fetching user class: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task<List<WorkoutModel>> GetWorkoutsAsync()
//        {
//            try
//            {
//                var results = await GetAsync<List<WorkoutModel>>($"{EndpointName}/workouts");
//                return results ?? new List<WorkoutModel>();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error fetching workouts: {ex.Message}");
//                return new List<WorkoutModel>();
//            }
//        }
//    }
//} 

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

        // DELETE /api/calendar/userworkout/{userId}/{workoutId}/{yyyy}/{MM}/{dd}
        public Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date)
            => DeleteAsync(
                $"{BaseRoute}/userworkout/{userId}/{workoutId}/{date:yyyy}/{date:MM}/{date:dd}");

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

        /// <summary>
        /// Alias for your old RemoveWorkoutAsync signature:
        /// </summary>
        public Task RemoveWorkoutAsync(int userId, CalendarDayModel day)
            => DeleteAsync(
                $"{BaseRoute}/workout/{userId}/{day.Date:yyyy}/{day.Date:MM}/{day.Date:dd}");
    }
}
