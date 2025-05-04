using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;
using Workout.Core.Data;
using Workout.Core.Data.Interfaces;

namespace Workout.Core.Repositories
{
    public class WorkoutRepo : IWorkoutRepository
    {
        private readonly IDatabaseHelper databaseHelper;

        public WorkoutRepo()
        {
            databaseHelper = new DatabaseHelper();
        }

        public WorkoutRepo(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public async Task<WorkoutModel> GetWorkoutByIdAsync(int workoutId)
        {
            string query = "SELECT * FROM Workouts WHERE WID = @wid";
            SqlParameter[] parameters =
            {
                new SqlParameter("@wid", workoutId)
            };

            DataTable table = await databaseHelper.ExecuteReaderAsync(query, parameters);

            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                return new WorkoutModel(
                    Convert.ToInt32(row["WID"]),
                    row["Name"].ToString(),
                    Convert.ToInt32(row["WTID"]));
            }

            return new WorkoutModel(); // return empty object if not found
        }

        public async Task<WorkoutModel> GetWorkoutByNameAsync(string workoutName)
        {
            string query = "SELECT * FROM Workouts WHERE Name = @name";
            SqlParameter[] parameters =
            {
                new SqlParameter("@name", workoutName)
            };

            DataTable table = await databaseHelper.ExecuteReaderAsync(query, parameters);

            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                return new WorkoutModel(
                    Convert.ToInt32(row["WID"]),
                    row["Name"].ToString(),
                    Convert.ToInt32(row["WTID"]));
            }

            return new WorkoutModel();
        }

        public async Task InsertWorkoutAsync(string workoutName, int workoutTypeId)
        {
            string query = "INSERT INTO Workouts (Name, WTID) VALUES (@name, @wtid)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@name", workoutName),
                new SqlParameter("@wtid", workoutTypeId)
            };

            await databaseHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            string query = "DELETE FROM Workouts WHERE WID = @wid";
            SqlParameter[] parameters =
            {
                new SqlParameter("@wid", workoutId)
            };

            await databaseHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task UpdateWorkoutAsync(WorkoutModel workout)
        {
            if (workout == null)
            {
                throw new ArgumentNullException(nameof(workout), "Workout cannot be null.");
            }

            // Check for duplicates
            string checkQuery = "SELECT COUNT(*) FROM Workouts WHERE Name = @Name AND WID != @Id";
            SqlParameter[] checkParams =
            {
                new SqlParameter("@Name", workout.Name),
                new SqlParameter("@Id", workout.Id)
            };

            int duplicateCount = await databaseHelper.ExecuteScalarAsync<int>(checkQuery, checkParams);
            if (duplicateCount > 0)
            {
                throw new Exception("A workout with this name already exists.");
            }

            // Perform the update
            string updateQuery = "UPDATE Workouts SET Name = @Name WHERE WID = @Id";
            SqlParameter[] updateParams =
            {
                new SqlParameter("@Name", workout.Name),
                new SqlParameter("@Id", workout.Id)
            };

            int rowsAffected = await databaseHelper.ExecuteNonQueryAsync(updateQuery, updateParams);
            if (rowsAffected == 0)
            {
                throw new Exception("No workout was updated. Ensure the workout ID exists.");
            }
        }

        public async Task<IList<WorkoutModel>> GetAllWorkoutsAsync()
        {
            string query = "SELECT * FROM Workouts";

            DataTable table = await databaseHelper.ExecuteReaderAsync(query, null);
            var workouts = new List<WorkoutModel>();

            foreach (DataRow row in table.Rows)
            {
                workouts.Add(new WorkoutModel(
                    Convert.ToInt32(row["WID"]),
                    row["Name"].ToString(),
                    Convert.ToInt32(row["WTID"])));
            }

            return workouts;
        }
    }
}
