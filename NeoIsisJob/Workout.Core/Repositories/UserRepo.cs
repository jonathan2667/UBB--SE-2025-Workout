using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Workout.Core.Models;
using Workout.Core.Data.Interfaces;
using Workout.Core.Data;
using Workout.Core.Repositories.Interfaces;

namespace Workout.Core.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly IDatabaseHelper databaseHelper;

        public UserRepo()
        {
            databaseHelper = new DatabaseHelper();
        }

        public UserRepo(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public async Task<UserModel?> GetUserByIdAsync(int userId)
        {
            string query = "SELECT UID FROM Users WHERE UID = @Id";
            var parameters = new[]
            {
                new SqlParameter("@Id", SqlDbType.Int) { Value = userId }
            };

            DataTable result = await databaseHelper.ExecuteReaderAsync(query, parameters);

            if (result.Rows.Count > 0)
            {
                return new UserModel(Convert.ToInt32(result.Rows[0]["UID"]));
            }

            return null; // returning null if no result is found
        }

        public async Task<int> InsertUserAsync()
        {
            string query = "INSERT INTO Users DEFAULT VALUES; SELECT SCOPE_IDENTITY();";

            return await databaseHelper.ExecuteScalarAsync<int>(query);
        }

        public async Task<bool> DeleteUserByIdAsync(int userId)
        {
            string query = "DELETE FROM Users WHERE UID = @Id";
            var parameters = new[]
            {
                new SqlParameter("@Id", SqlDbType.Int) { Value = userId }
            };

            int rowsAffected = await databaseHelper.ExecuteNonQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            string query = "SELECT UID FROM Users";
            DataTable result = await databaseHelper.ExecuteReaderAsync(query, null);

            var users = new List<UserModel>();

            foreach (DataRow row in result.Rows)
            {
                users.Add(new UserModel(Convert.ToInt32(row["UID"])));
            }

            return users;
        }
    }
}
