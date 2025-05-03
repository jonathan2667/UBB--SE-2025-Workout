using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Workout.Core.Models;
using Workout.Core.Data.Interfaces;
using Workout.Core.Data;
using Workout.Core.Repositories.Interfaces;
using System.Data.SqlTypes;

namespace Workout.Core.Repositories
{
    internal class CalendarRepository : ICalendarRepository
    {
        private readonly IDatabaseHelper databaseHelper;
        private const int FirstDayOfMonth = 1;
        private const int StartEndMonthDifference = 1;
        private const int StartEndDayDifference = -1;

        public CalendarRepository()
        {
            databaseHelper = new DatabaseHelper();
        }

        public CalendarRepository(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public List<CalendarDay> GetCalendarDaysForMonth(int userId, DateTime month)
        {
            var calendarDays = new List<CalendarDay>();
            DateTime firstDay = new DateTime(month.Year, month.Month, FirstDayOfMonth);
            DateTime lastDay = firstDay.AddMonths(StartEndMonthDifference).AddDays(StartEndDayDifference);
            int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);

            var workoutDays = new Dictionary<DateTime, (bool HasWorkout, bool Completed)>();
            var classDays = new Dictionary<DateTime, bool>();

            // Query workouts
            string workoutQuery = @"
                SELECT Date, Completed 
                FROM UserWorkouts 
                WHERE UID = @UserId AND Date >= @StartDate AND Date <= @EndDate";

            var workoutParams = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@StartDate", firstDay),
                new SqlParameter("@EndDate", lastDay)
            };

            var workoutTable = databaseHelper.ExecuteReader(workoutQuery, workoutParams);
            foreach (DataRow row in workoutTable.Rows)
            {
                var date = Convert.ToDateTime(row["Date"]);
                var completed = Convert.ToBoolean(row["Completed"]);
                workoutDays[date] = (true, completed);
            }

            // Query classes
            string classQuery = @"
                SELECT Date 
                FROM UserClasses 
                WHERE UID = @UserId AND Date >= @StartDate AND Date <= @EndDate";

            var classParams = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@StartDate", firstDay),
                new SqlParameter("@EndDate", lastDay)
            };

            var classTable = databaseHelper.ExecuteReader(classQuery, classParams);
            foreach (DataRow row in classTable.Rows)
            {
                var date = Convert.ToDateTime(row["Date"]);
                classDays[date] = true;
            }

            for (int day = FirstDayOfMonth; day <= daysInMonth; day++)
            {
                var currentDate = new DateTime(month.Year, month.Month, day);
                bool hasWorkout = workoutDays.ContainsKey(currentDate);
                bool isCompleted = hasWorkout && workoutDays[currentDate].Completed;
                bool hasClass = classDays.ContainsKey(currentDate);

                calendarDays.Add(new CalendarDay
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

        public UserWorkoutModel? GetUserWorkout(int userId, DateTime date)
        {
            string query = @"
                SELECT WID, Completed 
                FROM UserWorkouts 
                WHERE UID = @UserId AND Date = @Date";

            var parameters = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Date", date.Date)
            };

            var table = databaseHelper.ExecuteReader(query, parameters);
            if (table.Rows.Count == 0)
            {
                return null;
            }

            var row = table.Rows[0];
            return new UserWorkoutModel(
                userId: userId,
                workoutId: Convert.ToInt32(row["WID"]),
                date: date,
                completed: Convert.ToBoolean(row["Completed"]));
        }

        public string? GetUserClass(int userId, DateTime date)
        {
            string classIdQuery = "SELECT CID FROM UserClasses WHERE UID = @UserId AND Date = @Date";
            var parameters = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Date", date.Date)
            };

            var classIdTable = databaseHelper.ExecuteReader(classIdQuery, parameters);
            if (classIdTable.Rows.Count == 0)
            {
                return null;
            }

            int classId = Convert.ToInt32(classIdTable.Rows[0]["CID"]);

            string classNameQuery = "SELECT Name FROM Classes WHERE CID = @ClassId";
            var nameParams = new[] { new SqlParameter("@ClassId", classId) };

            var classNameTable = databaseHelper.ExecuteReader(classNameQuery, nameParams);
            return classNameTable.Rows.Count > 0 ? classNameTable.Rows[0]["Name"].ToString() : null;
        }

        public List<WorkoutModel> GetWorkouts()
        {
            string query = "SELECT WID, Name FROM Workouts";
            var table = databaseHelper.ExecuteReader(query, Array.Empty<SqlParameter>());

            var workouts = new List<WorkoutModel>();
            foreach (DataRow row in table.Rows)
            {
                workouts.Add(new WorkoutModel
                {
                    Id = Convert.ToInt32(row["WID"]),
                    Name = row["Name"].ToString()
                });
            }

            return workouts;
        }
    }
}
