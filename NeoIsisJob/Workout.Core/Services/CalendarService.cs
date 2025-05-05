using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Data;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.IServices;

namespace Workout.Core.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepository calendarRepository;
        private readonly IUserWorkoutRepository userWorkoutRepo;

        public CalendarService(
            ICalendarRepository calendarRepository,
            IUserWorkoutRepository userWorkoutRepo)
        {
            this.calendarRepository = calendarRepository
                ?? throw new ArgumentNullException(nameof(calendarRepository));
            this.userWorkoutRepo = userWorkoutRepo
                ?? throw new ArgumentNullException(nameof(userWorkoutRepo));
        }

        public async Task<List<CalendarDayModel>> GetCalendarDaysForMonthAsync(int userId, DateTime date)
        {
            return await calendarRepository
                .GetCalendarDaysForMonthAsync(userId, date);
        }

        public async Task<ObservableCollection<CalendarDayModel>> GetCalendarDaysAsync(int userId, DateTime currentDate)
        {
            var calendarDays = new ObservableCollection<CalendarDayModel>();
            var monthDays = await GetCalendarDaysForMonthAsync(userId, currentDate);

            // build grid
            DateTime firstDay = new DateTime(currentDate.Year, currentDate.Month, 1);
            int startDow = (int)firstDay.DayOfWeek;
            int row = 0, col = 0;
            for (int i = 0; i < startDow; i++)
            {
                calendarDays.Add(new CalendarDayModel { IsEnabled = false, GridRow = row, GridColumn = col });
                col++;
                if (col > 6) { col = 0; row++; }
            }

            DateTime today = DateTime.Now.Date;
            foreach (var day in monthDays)
            {
                day.GridRow = row;
                day.GridColumn = col;

                if (day.HasWorkout && day.Date >= today)
                {
                    day.IsEnabled = true;
                }
                calendarDays.Add(day);
                col++;
                if (col > 6)
                {
                    col = 0;
                    row++;
                }
            }

            return calendarDays;
        }

        public async Task RemoveWorkoutAsync(int userId, CalendarDayModel day)
        {
            var w = await GetUserWorkoutAsync(userId, day.Date);
            if (w != null)
                await DeleteUserWorkoutAsync(userId, w.WID, day.Date);
        }

        public async Task ChangeWorkoutAsync(int userId, CalendarDayModel day)
        {
            // same logic as Remove; you can extend to Add afterward
            var w = await GetUserWorkoutAsync(userId, day.Date);
            if (w != null)
                await DeleteUserWorkoutAsync(userId, w.WID, day.Date);
        }

        public string GetWorkoutDaysCountText(ObservableCollection<CalendarDayModel> calendarDays)
            => $"Workout Days: {calendarDays.Count(d => d.HasWorkout)}";

        public string GetDaysCountText(ObservableCollection<CalendarDayModel> calendarDays)
            => $"Days Count: {calendarDays.Count}";

        public async Task AddUserWorkoutAsync(UserWorkoutModel userWorkout)
            => await userWorkoutRepo.AddUserWorkoutAsync(userWorkout);

        public async Task UpdateUserWorkoutAsync(UserWorkoutModel userWorkout)
            => await userWorkoutRepo.UpdateUserWorkoutAsync(userWorkout);

        public async Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date)
            => await userWorkoutRepo.DeleteUserWorkoutAsync(userId, workoutId, date);

        public async Task<UserWorkoutModel> GetUserWorkoutAsync(int userId, DateTime date)
            => await calendarRepository.GetUserWorkoutAsync(userId, date);

        public async Task<string> GetUserClassAsync(int userId, DateTime date)
            => await calendarRepository.GetUserClassAsync(userId, date);

        public async Task<List<WorkoutModel>> GetWorkoutsAsync()
            => await calendarRepository.GetWorkoutsAsync();
    }
}
