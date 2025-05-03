using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Workout.Core.Data.Interfaces;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;
using System.Data.SqlTypes;

namespace Workout.Core.Repositories
{
    internal class CompleteWorkoutRepo : ICompleteWorkoutRepository
    {
        private readonly IDatabaseHelper databaseHelper;

        public CompleteWorkoutRepo(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public IList<CompleteWorkoutModel> GetAllCompleteWorkouts()
        {
            IList<CompleteWorkoutModel> completeWorkouts = new List<CompleteWorkoutModel>();
            string query = "SELECT * FROM CompleteWorkouts";

            try
            {
                var dataTable = databaseHelper.ExecuteReader(query, null);
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

        public void DeleteCompleteWorkoutsByWorkoutId(int workoutId)
        {
            string deleteCommand = "DELETE FROM CompleteWorkouts WHERE WID=@wid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@wid", workoutId)
            };

            try
            {
                databaseHelper.ExecuteNonQuery(deleteCommand, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting complete workouts: " + ex.Message);
            }
        }

        public void InsertCompleteWorkout(int workoutId, int exerciseId, int sets, int repetitionsPerSet)
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
                databaseHelper.ExecuteNonQuery(insertCommand, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting complete workout: " + ex.Message);
            }
        }
    }
}
