using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Workout.Core.Data.Interfaces;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class UserClassRepo : IUserClassRepo
    {
        private readonly IDatabaseHelper databaseHelper;

        public UserClassRepo()
        {
            databaseHelper = new DatabaseHelper();
        }

        public UserClassRepo(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public async Task<UserClassModel?> GetUserClassModelByIdAsync(int userId, int classId, DateTime enrollmentDate)
        {
            string query = "SELECT UID, CID, Date FROM UserClasses WHERE UID = @UID AND CID = @CID AND Date = @Date";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UID", userId),
                new SqlParameter("@CID", classId),
                new SqlParameter("@Date", enrollmentDate)
            };

            DataTable result = await databaseHelper.ExecuteReaderAsync(query, parameters);

            if (result.Rows.Count > 0)
            {
                var row = result.Rows[0];
                return new UserClassModel(
                    Convert.ToInt32(row["UID"]),
                    Convert.ToInt32(row["CID"]),
                    Convert.ToDateTime(row["Date"]));
            }

            return null; // returning null if no results are found
        }

        public async Task<List<UserClassModel>> GetAllUserClassModelAsync()
        {
            string query = "SELECT UID, CID, Date FROM UserClasses";

            DataTable result = await databaseHelper.ExecuteReaderAsync(query, null);
            List<UserClassModel> userClasses = new List<UserClassModel>();

            foreach (DataRow row in result.Rows)
            {
                userClasses.Add(new UserClassModel(
                    Convert.ToInt32(row["UID"]),
                    Convert.ToInt32(row["CID"]),
                    Convert.ToDateTime(row["Date"])));
            }

            return userClasses;
        }

        public async Task AddUserClassModelAsync(UserClassModel userClass)
        {
            string query = "INSERT INTO UserClasses (UID, CID, Date) VALUES (@UID, @CID, @Date)";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UID", userClass.UserId),
                new SqlParameter("@CID", userClass.ClassId),
                new SqlParameter("@Date", userClass.EnrollmentDate)
            };

            await databaseHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task DeleteUserClassModelAsync(int userId, int classId, DateTime enrollmentDate)
        {
            string query = "DELETE FROM UserClasses WHERE UID = @UID AND CID = @CID AND Date = @Date";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UID", userId),
                new SqlParameter("@CID", classId),
                new SqlParameter("@Date", enrollmentDate)
            };

            await databaseHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<List<UserClassModel>> GetUserClassModelByDateAsync(DateTime date)
        {
            string query = "SELECT UID, CID, Date FROM UserClasses WHERE Date = @Date";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Date", date)
            };

            DataTable result = await databaseHelper.ExecuteReaderAsync(query, parameters);
            List<UserClassModel> userClasses = new List<UserClassModel>();

            foreach (DataRow row in result.Rows)
            {
                userClasses.Add(new UserClassModel(
                    Convert.ToInt32(row["UID"]),
                    Convert.ToInt32(row["CID"]),
                    Convert.ToDateTime(row["Date"])));
            }

            return userClasses;
        }
    }
}
