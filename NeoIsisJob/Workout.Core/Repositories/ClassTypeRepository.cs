using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Workout.Core.Data;
using Workout.Core.Models;
using Workout.Core.Data.Interfaces;
using Workout.Core.Repositories.Interfaces;

namespace Workout.Core.Repositories
{
    internal class ClassTypeRepository : IClassTypeRepository
    {
        private readonly IDatabaseHelper databaseHelper;

        public ClassTypeRepository()
        {
            this.databaseHelper = new DatabaseHelper();
        }

        public ClassTypeRepository(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public ClassTypeModel? GetClassTypeModelById(int classTypeId)
        {
            string query = "SELECT CTID, Name FROM ClassTypes WHERE CTID = @ctid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ctid", classTypeId)
            };

            try
            {
                var dataTable = databaseHelper.ExecuteReader(query, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    return new ClassTypeModel
                    {
                        Id = Convert.ToInt32(row["CTID"]),
                        Name = Convert.ToString(row["Name"]) ?? string.Empty
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching class type by ID: " + ex.Message);
            }
        }

        public List<ClassTypeModel> GetAllClassTypeModel()
        {
            List<ClassTypeModel> classTypes = new List<ClassTypeModel>();
            string query = "SELECT CTID, Name FROM ClassTypes";

            try
            {
                var dataTable = databaseHelper.ExecuteReader(query, null);

                foreach (DataRow row in dataTable.Rows)
                {
                    classTypes.Add(new ClassTypeModel
                    {
                        Id = Convert.ToInt32(row["CTID"]),
                        Name = Convert.ToString(row["Name"]) ?? string.Empty
                    });
                }

                return classTypes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching class types: " + ex.Message);
            }
        }

        public void AddClassTypeModel(ClassTypeModel classType)
        {
            string query = "INSERT INTO ClassTypes (Name) VALUES (@name)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@name", classType.Name)
            };

            try
            {
                databaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding class type: " + ex.Message);
            }
        }

        public void DeleteClassTypeModel(int classTypeId)
        {
            string query = "DELETE FROM ClassTypes WHERE CTID = @ctid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ctid", classTypeId)
            };

            try
            {
                databaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting class type: " + ex.Message);
            }
        }
    }
}
