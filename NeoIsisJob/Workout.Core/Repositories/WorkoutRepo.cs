using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;
using Workout.Core.Data;
using Workout.Core.Data.Interfaces;

namespace Workout.Core.Repositories
{
    internal class WorkoutRepo : IWorkoutRepository
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

        public WorkoutModel GetWorkoutById(int workoutId)
        {
            string query = "SELECT * FROM Workouts WHERE WID = @wid";
            SqlParameter[] parameters =
            {
                new SqlParameter("@wid", workoutId)
            };

            DataTable table = databaseHelper.ExecuteReader(query, parameters);

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

        public WorkoutModel GetWorkoutByName(string workoutName)
        {
            string query = "SELECT * FROM Workouts WHERE Name = @name";
            SqlParameter[] parameters =
            {
                new SqlParameter("@name", workoutName)
            };

            DataTable table = databaseHelper.ExecuteReader(query, parameters);

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

        public void InsertWorkout(string workoutName, int workoutTypeId)
        {
            string query = "INSERT INTO Workouts (Name, WTID) VALUES (@name, @wtid)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@name", workoutName),
                new SqlParameter("@wtid", workoutTypeId)
            };

            databaseHelper.ExecuteNonQuery(query, parameters);
        }

        public void DeleteWorkout(int workoutId)
        {
            string query = "DELETE FROM Workouts WHERE WID = @wid";
            SqlParameter[] parameters =
            {
                new SqlParameter("@wid", workoutId)
            };

            databaseHelper.ExecuteNonQuery(query, parameters);
        }

        public void UpdateWorkout(WorkoutModel workout)
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

            int duplicateCount = databaseHelper.ExecuteScalar<int>(checkQuery, checkParams);
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

            int rowsAffected = databaseHelper.ExecuteNonQuery(updateQuery, updateParams);
            if (rowsAffected == 0)
            {
                throw new Exception("No workout was updated. Ensure the workout ID exists.");
            }
        }

        public IList<WorkoutModel> GetAllWorkouts()
        {
            string query = "SELECT * FROM Workouts";

            DataTable table = databaseHelper.ExecuteReader(query, null);
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
