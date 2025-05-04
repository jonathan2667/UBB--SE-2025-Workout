using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;
using Workout.Core.Data.Interfaces;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly IDatabaseHelper databaseHelper;

        public ClassRepository()
        {
            this.databaseHelper = new DatabaseHelper();
        }

        public ClassRepository(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public async Task<ClassModel> GetClassModelByIdAsync(int classId)
        {
            string query = "SELECT CID, Name, Description, CTID, PTID FROM Classes WHERE CID = @cid";

            using (SqlConnection connection = databaseHelper.GetConnection())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cid", classId);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
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
                    }
                }
            }

            return null;
        }

        public async Task<List<ClassModel>> GetAllClassModelAsync()
        {
            List<ClassModel> classList = new List<ClassModel>();
            string query = "SELECT CID, Name, Description, CTID, PTID FROM Classes";

            using (SqlConnection connection = databaseHelper.GetConnection())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            classList.Add(new ClassModel
                            {
                                Id = (int)reader["CID"],
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty,
                                ClassTypeId = (int)reader["CTID"],
                                PersonalTrainerId = (int)reader["PTID"]
                            });
                        }
                    }
                }
            }

            return classList;
        }

        public async Task AddClassModelAsync(ClassModel classModel)
        {
            string query = "INSERT INTO Classes (Name, Description, CTID, PTID) VALUES (@name, @description, @ctid, @ptid)";

            using (SqlConnection connection = databaseHelper.GetConnection())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", classModel.Name);
                    command.Parameters.AddWithValue("@description", classModel.Description);
                    command.Parameters.AddWithValue("@ctid", classModel.ClassTypeId);
                    command.Parameters.AddWithValue("@ptid", classModel.PersonalTrainerId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteClassModelAsync(int classId)
        {
            string query = "DELETE FROM Classes WHERE CID = @cid";

            using (SqlConnection connection = databaseHelper.GetConnection())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cid", classId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
