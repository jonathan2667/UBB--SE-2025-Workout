using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Workout.Core.Data.Interfaces;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;
using Workout.Core.Data;


namespace Workout.Core.Repositories
{
    public class CompleteWorkoutRepo : ICompleteWorkoutRepository
    {
        private readonly IDatabaseHelper databaseHelper;

        public CompleteWorkoutRepo(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public async Task<IList<CompleteWorkoutModel>> GetAllCompleteWorkoutsAsync()
        {
            IList<CompleteWorkoutModel> completeWorkouts = new List<CompleteWorkoutModel>();
            string query = "SELECT * FROM CompleteWorkouts";

            try
            {
                var dataTable = await databaseHelper.ExecuteReaderAsync(query, null);
                foreach (System.Data.DataRow row in dataTable.Rows)
                {
                    completeWorkouts.Add(new CompleteWorkoutModel(
                        Convert.ToInt32(row["WID"]),
                        Convert.ToInt32(row["EID"]),
                        Convert.ToInt32(row["Sets"]),
                        Convert.ToInt32(row["RepsPerSet"])));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching complete workouts: " + ex.Message);
            }

            return completeWorkouts;
        }

        public async Task DeleteCompleteWorkoutsByWorkoutIdAsync(int workoutId)
        {
            string deleteCommand = "DELETE FROM CompleteWorkouts WHERE WID=@wid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@wid", workoutId)
            };

            try
            {
                await databaseHelper.ExecuteNonQueryAsync(deleteCommand, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting complete workouts: " + ex.Message);
            }
        }

        public async Task InsertCompleteWorkoutAsync(int workoutId, int exerciseId, int sets, int repetitionsPerSet)
        {
            string insertCommand = "INSERT INTO CompleteWorkouts(WID, EID, [Sets], RepsPerSet) VALUES (@wid, @eid, @sets, @repsPerSet)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@wid", workoutId),
                new SqlParameter("@eid", exerciseId),
                new SqlParameter("@sets", sets),
                new SqlParameter("@repsPerSet", repetitionsPerSet)
            };

            try
            {
                await databaseHelper.ExecuteNonQueryAsync(insertCommand, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting complete workout: " + ex.Message);
            }
        }
    }
}
