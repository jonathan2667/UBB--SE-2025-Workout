using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Repositories;
using Workout.Core.Models;
using Workout.Core.IRepositories;
using Workout.Core.IServices;

namespace Workout.Core.Services
{
    public class PersonalTrainerService : IPersonalTrainerService
    {
        private readonly IPersonalTrainerRepo personalTrainerRepository;

        public PersonalTrainerService(IPersonalTrainerRepo personalTrainerRepository)
        {
            this.personalTrainerRepository = personalTrainerRepository ?? throw new ArgumentNullException(nameof(personalTrainerRepository));
        }

        public async Task<List<PersonalTrainerModel>> GetAllPersonalTrainersAsync()
        {
            var trainers = await personalTrainerRepository.GetAllPersonalTrainerModelAsync();
            return trainers;
        }

        public async Task<PersonalTrainerModel?> GetPersonalTrainerByIdAsync(int personalTrainerId)
        {
            var trainer = await personalTrainerRepository.GetPersonalTrainerModelByIdAsync(personalTrainerId);
            return trainer;
        }

        public async Task AddPersonalTrainerAsync(PersonalTrainerModel personalTrainerModel)
        {
            // You can also add additional validation such as checking if the trainer already exists, etc.
            await personalTrainerRepository.AddPersonalTrainerModelAsync(personalTrainerModel);
        }

        public async Task DeletePersonalTrainerAsync(int personalTrainerId)
        {
            var trainer = await personalTrainerRepository.GetPersonalTrainerModelByIdAsync(personalTrainerId);
            await personalTrainerRepository.DeletePersonalTrainerModelAsync(personalTrainerId);
        }
    }
}
