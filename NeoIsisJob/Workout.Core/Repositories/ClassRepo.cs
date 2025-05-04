using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Workout.Core.Data.Interfaces; // <-- Make sure this is correct
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class ClassRepo
    {
        private readonly IDatabaseHelper databaseHelper;

        public ClassRepo()
        {
            this.databaseHelper = new DatabaseHelper(); // still works
        }

        public ClassRepo(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper; // DI-friendly constructor
        }

        public ClassModel GetClassModelById(int classId)
        {
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT CID, Name, Description, CTID, PTID FROM Classes WHERE Cid = @cid";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@cid", classId);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new ClassModel
                    {
                        Id = (int)reader["CID"],
                        Name = reader["Name"].ToString() ?? string.Empty,
                        Description = reader["Description"].ToString() ?? string.Empty,
                        ClassTypeId = (int)reader["CTID"],
                        PersonalTrainerId = (int)reader["PTID"]
                    };
                }

                return new ClassModel();
            }
        }

        public List<ClassModel> GetAllClassModel()
        {
            List<ClassModel> classes = new List<ClassModel>();
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT CID, Name, Description, CTID, PTID FROM Classes";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

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

                return classes;
            }
        }

        public void AddClassModel(ClassModel classModel)
        {
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Classes (Name, Description, CTID, PTID) VALUES (@name, @description, @ctid, @ptid)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@name", classModel.Name);
                command.Parameters.AddWithValue("@description", classModel.Description);
                command.Parameters.AddWithValue("@ctid", classModel.ClassTypeId);
                command.Parameters.AddWithValue("@ptid", classModel.PersonalTrainerId);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteClassModel(int classId)
        {
            using (SqlConnection connection = this.databaseHelper.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Class WHERE Cid = @cid";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@cid", classId);
                command.ExecuteNonQuery();
            }
        }
    }
}
