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
    public class RankingsRepository : IRankingsRepository
    {
        private readonly IDatabaseHelper databaseHelper;

        public RankingsRepository()
        {
            this.databaseHelper = new DatabaseHelper();
        }

        public RankingsRepository(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public async Task<RankingModel?> GetRankingByFullIDAsync(int userId, int muscleGroupId)
        {
            string query = "SELECT * FROM Rankings WHERE UID = @UID AND MGID = @MGID";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UID", userId),
                new SqlParameter("@MGID", muscleGroupId)
            };

            try
            {
                DataTable dt = await databaseHelper.ExecuteReaderAsync(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    return new RankingModel(
                        Convert.ToInt32(row["UID"]),
                        Convert.ToInt32(row["MGID"]),
                        Convert.ToInt32(row["Rank"]));
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching ranking by full ID: " + ex.Message);
            }
        }

        public async Task<IList<RankingModel>> GetAllRankingsByUserIDAsync(int userId)
        {
            string query = "SELECT * FROM Rankings WHERE UID = @UID";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UID", userId)
            };

            try
            {
                DataTable dt = await databaseHelper.ExecuteReaderAsync(query, parameters);
                IList<RankingModel> rankings = new List<RankingModel>();

                foreach (DataRow row in dt.Rows)
                {
                    rankings.Add(new RankingModel(
                        Convert.ToInt32(row["UID"]),
                        Convert.ToInt32(row["MGID"]),
                        Convert.ToInt32(row["Rank"])));
                }

                return rankings;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching rankings by user ID: " + ex.Message);
            }
        }
    }
}
