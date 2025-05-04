using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;
using Workout.Core.Data;
using Workout.Core.Data.Interfaces;

namespace Workout.Core.Repositories
{
    public class WorkoutTypeRepo : IWorkoutTypeRepository
    {
        private readonly DatabaseHelper databaseHelper;

        public WorkoutTypeRepo()
        {
            this.databaseHelper = new DatabaseHelper();
        }

        public async Task<WorkoutTypeModel> GetWorkoutTypeByIdAsync(int workoutTypeId)
        {
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                await connection.OpenAsync();

                // create the query
                string query = "SELECT * FROM WorkoutTypes WHERE WTID=@wtid";

                // create the command now
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@wtid", workoutTypeId);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                // now check if the type exists -> if yes return it
                if (await reader.ReadAsync())
                {
                    return new WorkoutTypeModel(Convert.ToInt32(reader["WTID"]), Convert.ToString(reader["Name"]));
                }
            }

            // otherwise return empty instance
            return new WorkoutTypeModel();
        }

        public async Task InsertWorkoutTypeAsync(string workoutTypeName)
        {
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                await connection.OpenAsync();

                // insert statement to insert the workout type
                string insertStatement = "INSERT INTO WorkoutTypes([Name]) VALUES (@name)";

                // now create the command and set the parameters
                SqlCommand command = new SqlCommand(insertStatement, connection);
                command.Parameters.AddWithValue("@name", workoutTypeName);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteWorkoutTypeAsync(int workoutTypeId)
        {
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                await connection.OpenAsync();

                // delete statement
                string deleteStatement = "DELETE FROM WorkoutTypes WHERE WTID=@wtid";

                // now create the command and set the parameters
                SqlCommand command = new SqlCommand(deleteStatement, connection);
                command.Parameters.AddWithValue("@wtid", workoutTypeId);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IList<WorkoutTypeModel>> GetAllWorkoutTypesAsync()
        {
            List<WorkoutTypeModel> workoutTypes = new List<WorkoutTypeModel>();

            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                await connection.OpenAsync();

                // create the query
                string query = "SELECT * FROM WorkoutTypes";

                // create the command now
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                // now check if the type exists -> if yes return it
                while (await reader.ReadAsync())
                {
                    // Ensure data is not null before accessing it
                    string name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : "Unknown";
                    int workoutTypeId = Convert.ToInt32(reader["WTID"]);

                    // Create WorkoutTypeModel and add to the list
                    workoutTypes.Add(new WorkoutTypeModel(workoutTypeId, name));
                }
            }

            return workoutTypes;
        }
    }
}
