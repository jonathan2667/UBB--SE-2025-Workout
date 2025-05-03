using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Workout.Core.Data;
using Workout.Core.Models;
using System.Data.SqlTypes;

namespace Workout.Core.Repositories
{
    internal class ClassRepo
    {
        private readonly DatabaseHelper databaseHelper;

        public ClassRepo()
        {
            this.databaseHelper = new DatabaseHelper();
        }

        public ClassModel GetClassModelById(int classId)
        {
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                // Open the connection
                connection.Open();

                // Create a query
                string query = "SELECT CID, Name, Description, CTID, PTID FROM Classes WHERE Cid = @cid";

                // Create a command
                SqlCommand command = new SqlCommand(query, connection);

                // Add the parameter
                command.Parameters.AddWithValue("@cid", classId);

                // Execute the command
                SqlDataReader reader = command.ExecuteReader();

                // Read the data
                if (reader.Read())
                {
                    // Return the class model
                    return new ClassModel
                    {
                        Id = (int)reader["CID"],
                        Name = reader["Name"].ToString() ?? string.Empty,
                        Description = reader["Description"].ToString() ?? string.Empty,
                        ClassTypeId = (int)reader["CTID"],
                        PersonalTrainerId = (int)reader["PTID"]
                    };
                }

                // Return an empty instance
                return new ClassModel();
            }
        }

        public List<ClassModel> GetAllClassModel()
        {
            List<ClassModel> classes = new List<ClassModel>();
            try
            {
                using (SqlConnection connection = this.databaseHelper.GetConnection())
                {
                    // Open the connection
                    connection.Open();

                    // Create a query
                    string query = "SELECT CID, Name, Description, CTID, PTID FROM Classes";

                    // Create a command
                    SqlCommand command = new SqlCommand(query, connection);

                    // Execute the command
                    SqlDataReader reader = command.ExecuteReader();

                    // Read the data
                    while (reader.Read())
                    {
                        classes.Add(new ClassModel
                        {
                            Id = (int)reader["CID"],
                            Name = reader["Name"].ToString() ?? string.Empty,
                            Description = reader["Description"].ToString() ?? string.Empty,
                            ClassTypeId = (int)reader["CTID"],
                            PersonalTrainerId = (int)reader["PTID"]
                        });
                    }

                    // Return the classes
                    return classes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddClassModel(ClassModel classModel)
        {
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                // Open the connection
                connection.Open();

                // Create a query
                string query = "INSERT INTO Classes (Name, Description, CTID, PTID) VALUES (@name, @description, @ctid, @ptid)";

                // Create a command
                SqlCommand command = new SqlCommand(query, connection);

                // Add the parameters
                command.Parameters.AddWithValue("@name", classModel.Name);
                command.Parameters.AddWithValue("@description", classModel.Description);
                command.Parameters.AddWithValue("@ctid", classModel.ClassTypeId);
                command.Parameters.AddWithValue("@ptid", classModel.PersonalTrainerId);

                // Execute the command
                command.ExecuteNonQuery();
            }
        }

        public void DeleteClassModel(int classId)
        {
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                // Open the connection
                connection.Open();

                // Create a query
                string query = "DELETE FROM Class WHERE Cid = @cid";

                // Create a command
                SqlCommand command = new SqlCommand(query, connection);

                // Add the parameter
                command.Parameters.AddWithValue("@cid", classId);

                // Execute the command
                command.ExecuteNonQuery();
            }
        }
    }
}
