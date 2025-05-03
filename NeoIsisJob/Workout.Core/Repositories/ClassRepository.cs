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
using System.Data.SqlTypes;

namespace Workout.Core.Repositories
{
    internal class ClassRepository : IClassRepository
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

        public ClassModel GetClassModelById(int classId)
        {
            string query = "SELECT CID, Name, Description, CTID, PTID FROM Classes WHERE CID = @cid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@cid", classId)
            };

            try
            {
                var dataTable = databaseHelper.ExecuteReader(query, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    return new ClassModel
                    {
                        Id = Convert.ToInt32(row["CID"]),
                        Name = Convert.ToString(row["Name"]) ?? string.Empty,
                        Description = Convert.ToString(row["Description"]) ?? string.Empty,
                        ClassTypeId = Convert.ToInt32(row["CTID"]),
                        PersonalTrainerId = Convert.ToInt32(row["PTID"])
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching class by ID: " + ex.Message);
            }
        }

        public List<ClassModel> GetAllClassModel()
        {
            List<ClassModel> classList = new List<ClassModel>();
            string query = "SELECT CID, Name, Description, CTID, PTID FROM Classes";

            try
            {
                var dataTable = databaseHelper.ExecuteReader(query, null);

                foreach (DataRow row in dataTable.Rows)
                {
                    classList.Add(new ClassModel
                    {
                        Id = Convert.ToInt32(row["CID"]),
                        Name = Convert.ToString(row["Name"]) ?? string.Empty,
                        Description = Convert.ToString(row["Description"]) ?? string.Empty,
                        ClassTypeId = Convert.ToInt32(row["CTID"]),
                        PersonalTrainerId = Convert.ToInt32(row["PTID"])
                    });
                }

                return classList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching classes: " + ex.Message);
            }
        }

        public void AddClassModel(ClassModel classModel)
        {
            string query = "INSERT INTO Classes (Name, Description, CTID, PTID) VALUES (@name, @description, @ctid, @ptid)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@name", classModel.Name),
                new SqlParameter("@description", classModel.Description),
                new SqlParameter("@ctid", classModel.ClassTypeId),
                new SqlParameter("@ptid", classModel.PersonalTrainerId)
            };

            try
            {
                databaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding class: " + ex.Message);
            }
        }

        public void DeleteClassModel(int classId)
        {
            string query = "DELETE FROM Classes WHERE CID = @cid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@cid", classId)
            };

            try
            {
                databaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting class: " + ex.Message);
            }
        }
    }
}
