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
    public class PersonalTrainerRepo : IPersonalTrainerRepo
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

        public async Task<PersonalTrainerModel?> GetPersonalTrainerModelByIdAsync(int personalTrainerId)
        {
            string query = "SELECT PTID, LastName, FirstName, WorksSince FROM PersonalTrainers WHERE PTID = @ptid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ptid", personalTrainerId)
            };

            try
            {
                var dataTable = await databaseHelper.ExecuteReaderAsync(query, parameters);

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

        public async Task<List<PersonalTrainerModel>> GetAllPersonalTrainerModelAsync()
        {
            List<PersonalTrainerModel> trainers = new List<PersonalTrainerModel>();
            string query = "SELECT PTID, LastName, FirstName, WorksSince FROM PersonalTrainers";

            try
            {
                var dataTable = await databaseHelper.ExecuteReaderAsync(query, null);

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

        public async Task AddPersonalTrainerModelAsync(PersonalTrainerModel personalTrainer)
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
                await databaseHelper.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding personal trainer: " + ex.Message);
            }
        }

        public async Task DeletePersonalTrainerModelAsync(int personalTrainerId)
        {
            string query = "DELETE FROM PersonalTrainers WHERE PTID = @ptid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ptid", personalTrainerId)
            };

            try
            {
                await databaseHelper.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting personal trainer: " + ex.Message);
            }
        }
    }
}
