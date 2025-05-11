using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly WorkoutDbContext context;
        private const int FirstDayOfMonth = 1;
        private const int StartEndMonthDifference = 1;
        private const int StartEndDayDifference = -1;

        public CalendarRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<List<CalendarDayModel>> GetCalendarDaysForMonthAsync(int userId, DateTime month)
        {
            var calendarDays = new List<CalendarDayModel>();
            DateTime firstDay = new DateTime(month.Year, month.Month, FirstDayOfMonth);
            DateTime lastDay = firstDay.AddMonths(StartEndMonthDifference).AddDays(StartEndDayDifference);
            int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);

            // Get user workouts for this month
            var userWorkouts = await context.UserWorkouts
                .Where(uw => uw.UID == userId && uw.Date >= firstDay && uw.Date <= lastDay)
                .ToListAsync();

            // Get user classes for this month
            var userClasses = await context.UserClasses
                .Where(uc => uc.UID == userId && uc.Date >= firstDay && uc.Date <= lastDay)
                .ToListAsync();

            // Prepare dictionaries for quick lookup
            var workoutDays = userWorkouts
                .ToDictionary(
                    uw => uw.Date.Date,
                    uw => (HasWorkout: true, Completed: uw.Completed));

            var classDays = userClasses
                .ToDictionary(
                    uc => uc.Date.Date,
                    _ => true);

            for (int day = FirstDayOfMonth; day <= daysInMonth; day++)
            {
                var currentDate = new DateTime(month.Year, month.Month, day);
                bool hasWorkout = workoutDays.ContainsKey(currentDate);
                bool isCompleted = hasWorkout && workoutDays[currentDate].Completed;
                bool hasClass = classDays.ContainsKey(currentDate);

                calendarDays.Add(new CalendarDayModel
                {
                    DayNumber = day,
                    Date = currentDate,
                    IsCurrentDay = currentDate.Date == DateTime.Now.Date,
                    HasWorkout = hasWorkout,
                    IsWorkoutCompleted = isCompleted,
                    HasClass = hasClass
                });
            }

            return calendarDays;
        }

        public async Task<UserWorkoutModel?> GetUserWorkoutAsync(int userId, DateTime date)
        {
            return await context.UserWorkouts
                .Include(uw => uw.Workout)
                .FirstOrDefaultAsync(uw => uw.UID == userId && uw.Date.Date == date.Date);
        }

        public async Task<string?> GetUserClassAsync(int userId, DateTime date)
        {
            var userClass = await context.UserClasses
                .Include(uc => uc.Class)
                .FirstOrDefaultAsync(uc => uc.UID == userId && uc.Date.Date == date.Date);

            return userClass?.Class?.Name;
        }

        public async Task<List<WorkoutModel>> GetWorkoutsAsync()
        {
            return await context.Workouts.ToListAsync();
        }
    }
}
