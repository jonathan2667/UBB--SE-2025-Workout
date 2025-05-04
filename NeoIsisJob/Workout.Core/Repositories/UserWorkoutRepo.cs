using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Workout.Core.Data.Interfaces;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;

namespace Workout.Core.Repositories
{
    public class UserWorkoutRepo : IUserWorkoutRepository
    {
        private readonly IDatabaseHelper databaseHelper;

        public UserWorkoutRepo(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public async Task<List<UserWorkoutModel>> GetUserWorkoutModelByDateAsync(DateTime date)
        {
            string query = "SELECT UID, WID, Date, Completed FROM UserWorkouts WHERE Date = @Date";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Date", date)
            };

            DataTable table = await databaseHelper.ExecuteReaderAsync(query, parameters);
            var userWorkouts = new List<UserWorkoutModel>();

            foreach (DataRow row in table.Rows)
            {
                userWorkouts.Add(new UserWorkoutModel(
                    Convert.ToInt32(row["UID"]),
                    Convert.ToInt32(row["WID"]),
                    Convert.ToDateTime(row["Date"]),
                    Convert.ToBoolean(row["Completed"])));
            }

            return userWorkouts;
        }

        public async Task<UserWorkoutModel?> GetUserWorkoutModelAsync(int userId, int workoutId, DateTime date)
        {
            string query = "SELECT UID, WID, Date, Completed FROM UserWorkouts WHERE UID = @UID AND WID = @WID AND Date = @Date";
            SqlParameter[] parameters =
            {
                new SqlParameter("@UID", userId),
                new SqlParameter("@WID", workoutId),
                new SqlParameter("@Date", date)
            };

            DataTable table = await databaseHelper.ExecuteReaderAsync(query, parameters);

            if (table.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = table.Rows[0];

            return new UserWorkoutModel(
                Convert.ToInt32(row["UID"]),
                Convert.ToInt32(row["WID"]),
                Convert.ToDateTime(row["Date"]),
                Convert.ToBoolean(row["Completed"]));
        }

        public async Task AddUserWorkoutAsync(UserWorkoutModel userWorkout)
        {
            string query = "INSERT INTO UserWorkouts (UID, WID, Date, Completed) VALUES (@UID, @WID, @Date, @Completed)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@UID", userWorkout.UserId),
                new SqlParameter("@WID", userWorkout.WorkoutId),
                new SqlParameter("@Date", userWorkout.Date),
                new SqlParameter("@Completed", userWorkout.Completed)
            };

            await databaseHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task UpdateUserWorkoutAsync(UserWorkoutModel userWorkout)
        {
            string query = "UPDATE UserWorkouts SET Completed = @Completed WHERE UID = @UID AND WID = @WID AND Date = @Date";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Completed", userWorkout.Completed),
                new SqlParameter("@UID", userWorkout.UserId),
                new SqlParameter("@WID", userWorkout.WorkoutId),
                new SqlParameter("@Date", userWorkout.Date)
            };

            await databaseHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date)
        {
            string query = "DELETE FROM UserWorkouts WHERE UID = @UID AND WID = @WID AND Date = @Date";
            SqlParameter[] parameters =
            {
                new SqlParameter("@UID", userId),
                new SqlParameter("@WID", workoutId),
                new SqlParameter("@Date", date)
            };

            await databaseHelper.ExecuteNonQueryAsync(query, parameters);
        }
    }
}
