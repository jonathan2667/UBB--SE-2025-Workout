using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class PersonalTrainerServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "personaltrainer";

        public PersonalTrainerServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        public async Task<List<PersonalTrainerModel>> GetAllPersonalTrainersAsync()
        {
            try
            {
                var results = await GetAsync<List<PersonalTrainerModel>>($"{EndpointName}");
                return results ?? new List<PersonalTrainerModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all personal trainers: {ex.Message}");
                return new List<PersonalTrainerModel>();
            }
        }

        public async Task<PersonalTrainerModel?> GetPersonalTrainerByIdAsync(int personalTrainerId)
        {
            try
            {
                var result = await GetAsync<PersonalTrainerModel>($"{EndpointName}/{personalTrainerId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching personal trainer: {ex.Message}");
                throw;
            }
        }

        public async Task AddPersonalTrainerAsync(PersonalTrainerModel personalTrainerModel)
        {
            try
            {
                await PostAsync($"{EndpointName}", personalTrainerModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding personal trainer: {ex.Message}");
                throw;
            }
        }

        public async Task DeletePersonalTrainerAsync(int personalTrainerId)
        {
            try
            {
                await DeleteAsync($"{EndpointName}/{personalTrainerId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting personal trainer: {ex.Message}");
                throw;
            }
        }
    }
}