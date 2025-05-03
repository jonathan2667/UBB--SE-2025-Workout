using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Workout.Core.Data;
using Workout.Core.Data.Interfaces;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;

namespace Workout.Core.Repositories
{
    internal class MuscleGroupRepo : IMuscleGroupRepo
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

        public MuscleGroupModel? GetMuscleGroupById(int muscleGroupId)
        {
            string query = "SELECT MGID, Name FROM MuscleGroups WHERE MGID = @mgid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@mgid", muscleGroupId)
            };

            try
            {
                var dataTable = databaseHelper.ExecuteReader(query, parameters);

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

        public List<MuscleGroupModel> GetAllMuscleGroups()
        {
            List<MuscleGroupModel> groups = new List<MuscleGroupModel>();
            string query = "SELECT MGID, Name FROM MuscleGroups";

            try
            {
                var dataTable = databaseHelper.ExecuteReader(query, null);

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

        public void AddMuscleGroup(MuscleGroupModel muscleGroup)
        {
            string query = "INSERT INTO MuscleGroups (Name) VALUES (@name)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@name", muscleGroup.Name)
            };

            try
            {
                databaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding muscle group: " + ex.Message);
            }
        }

        public void DeleteMuscleGroup(int muscleGroupId)
        {
            string query = "DELETE FROM MuscleGroups WHERE MGID = @mgid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@mgid", muscleGroupId)
            };

            try
            {
                databaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting muscle group: " + ex.Message);
            }
        }
    }
}
