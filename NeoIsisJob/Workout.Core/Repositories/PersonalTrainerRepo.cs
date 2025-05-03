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
    internal class PersonalTrainerRepo : IPersonalTrainerRepo
    {
        private readonly IDatabaseHelper databaseHelper;

        public PersonalTrainerRepo()
        {
            this.databaseHelper = new DatabaseHelper();
        }

        public PersonalTrainerRepo(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public PersonalTrainerModel? GetPersonalTrainerModelById(int personalTrainerId)
        {
            string query = "SELECT PTID, LastName, FirstName, WorksSince FROM PersonalTrainers WHERE PTID = @ptid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ptid", personalTrainerId)
            };

            try
            {
                var dataTable = databaseHelper.ExecuteReader(query, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    return new PersonalTrainerModel
                    {
                        Id = Convert.ToInt32(row["PTID"]),
                        LastName = Convert.ToString(row["LastName"]) ?? string.Empty,
                        FirstName = Convert.ToString(row["FirstName"]) ?? string.Empty,
                        WorkStartDateTime = row["WorksSince"] as DateTime? ?? DateTime.MinValue
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching personal trainer by ID: " + ex.Message);
            }
        }

        public List<PersonalTrainerModel> GetAllPersonalTrainerModel()
        {
            List<PersonalTrainerModel> trainers = new List<PersonalTrainerModel>();
            string query = "SELECT PTID, LastName, FirstName, WorksSince FROM PersonalTrainers";

            try
            {
                var dataTable = databaseHelper.ExecuteReader(query, null);

                foreach (DataRow row in dataTable.Rows)
                {
                    trainers.Add(new PersonalTrainerModel
                    {
                        Id = Convert.ToInt32(row["PTID"]),
                        LastName = Convert.ToString(row["LastName"]) ?? string.Empty,
                        FirstName = Convert.ToString(row["FirstName"]) ?? string.Empty,
                        WorkStartDateTime = row["WorksSince"] as DateTime? ?? DateTime.MinValue
                    });
                }

                return trainers;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching all personal trainers: " + ex.Message);
            }
        }

        public void AddPersonalTrainerModel(PersonalTrainerModel personalTrainer)
        {
            string query = "INSERT INTO PersonalTrainers (LastName, FirstName, WorksSince) VALUES (@lastName, @firstName, @worksSince)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@lastName", personalTrainer.LastName),
                new SqlParameter("@firstName", personalTrainer.FirstName),
                new SqlParameter("@worksSince", personalTrainer.WorkStartDateTime)
            };

            try
            {
                databaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding personal trainer: " + ex.Message);
            }
        }

        public void DeletePersonalTrainerModel(int personalTrainerId)
        {
            string query = "DELETE FROM PersonalTrainers WHERE PTID = @ptid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ptid", personalTrainerId)
            };

            try
            {
                databaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting personal trainer: " + ex.Message);
            }
        }
    }
}
