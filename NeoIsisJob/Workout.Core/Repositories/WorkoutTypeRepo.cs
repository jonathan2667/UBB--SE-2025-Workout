using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Workout.Core.Data;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;
using Workout.Core.Data.Interfaces;
using System.Data.SqlTypes;

namespace Workout.Core.Repositories
{
    internal class WorkoutTypeRepo : IWorkoutTypeRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public WorkoutTypeRepo()
        {
            this.databaseHelper = new DatabaseHelper();
        }

        public WorkoutTypeModel GetWorkoutTypeById(int workoutTypeId)
        {
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                // open the connection
                connection.Open();

                // create the query
                string query = "SELECT * FROM WorkoutTypes WHERE WTID=@wtid";

                // create the command now
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@wtid", workoutTypeId);
                SqlDataReader reader = command.ExecuteReader();

                // now check if the type exists -> if yes return it
                if (reader.Read())
                {
                    return new WorkoutTypeModel(Convert.ToInt32(reader["WTID"]), Convert.ToString(reader["Name"]));
                }
            }

            // otherwise return empty instance
            return new WorkoutTypeModel();
        }

        public void InsertWorkoutType(string workoutTypeName)
        {
            // use the setup connection
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                // open the connection
                connection.Open();

                // insert statement to insert the workout type
                string insertStatement = "INSERT INTO WorkoutTypes([Name]) VALUES (@name)";

                // now create the command and set the parameters
                SqlCommand command = new SqlCommand(insertStatement, connection);
                command.Parameters.AddWithValue("@name", workoutTypeName);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteWorkoutType(int workoutTypeId)
        {
            // use the setup connection
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                // open the connection
                connection.Open();

                // delete statement
                string deleteStatement = "DELETE FROM WorkoutTypes WHERE WTID=@wtid";

                // now create the command and set the parameters
                SqlCommand command = new SqlCommand(deleteStatement, connection);
                command.Parameters.AddWithValue("@wtid", workoutTypeId);

                command.ExecuteNonQuery();
            }
        }

        public IList<WorkoutTypeModel> GetAllWorkoutTypes()
        {
            List<WorkoutTypeModel> workoutTypes = new List<WorkoutTypeModel>();

            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                // open the connection
                connection.Open();

                // create the query
                string query = "SELECT * FROM WorkoutTypes";

                // create the command now
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                // now check if the type exists -> if yes return it
                while (reader.Read())
                {
                    // Ensure data is not null before accessing it
                    string name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : "Unknown";
                    int workoutTypeId = Convert.ToInt32(reader["WTID"]);

                    // Create WorkoutTypeModel and add to the list
                    workoutTypes.Add(new WorkoutTypeModel(workoutTypeId, name));
                }
            }

            // if no entry -> return empty list annyways
            return workoutTypes;
        }

        // eventually edit if it is needed
    }
}
