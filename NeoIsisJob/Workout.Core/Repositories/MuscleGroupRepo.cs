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
    public class MuscleGroupRepo : IMuscleGroupRepo
    {
        private readonly IDatabaseHelper databaseHelper;

        public MuscleGroupRepo()
        {
            this.databaseHelper = new DatabaseHelper();
        }

        public MuscleGroupRepo(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public async Task<MuscleGroupModel?> GetMuscleGroupByIdAsync(int muscleGroupId)
        {
            string query = "SELECT MGID, Name FROM MuscleGroups WHERE MGID = @mgid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@mgid", muscleGroupId)
            };

            try
            {
                var dataTable = await databaseHelper.ExecuteReaderAsync(query, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    return new MuscleGroupModel
                    {
                        Id = Convert.ToInt32(row["MGID"]),
                        Name = Convert.ToString(row["Name"]) ?? string.Empty
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching muscle group by ID: " + ex.Message);
            }
        }

        public async Task<List<MuscleGroupModel>> GetAllMuscleGroupsAsync()
        {
            List<MuscleGroupModel> groups = new List<MuscleGroupModel>();
            string query = "SELECT MGID, Name FROM MuscleGroups";

            try
            {
                var dataTable = await databaseHelper.ExecuteReaderAsync(query, null);

                foreach (DataRow row in dataTable.Rows)
                {
                    groups.Add(new MuscleGroupModel
                    {
                        Id = Convert.ToInt32(row["MGID"]),
                        Name = Convert.ToString(row["Name"]) ?? string.Empty
                    });
                }

                return groups;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching muscle groups: " + ex.Message);
            }
        }
    }
}
