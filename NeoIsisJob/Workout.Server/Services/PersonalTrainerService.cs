using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Server.Repositories;
using Workout.Core.Models;

namespace Workout.Server.Services
{
    public class PersonalTrainerService
    {
        private readonly PersonalTrainerRepo personalTrainerRepository;

        public PersonalTrainerService()
        {
            this.personalTrainerRepository = new PersonalTrainerRepo();
        }

        public async Task<List<PersonalTrainerModel>> GetAllPersonalTrainersAsync()
        {
            var trainers = await personalTrainerRepository.GetAllPersonalTrainerModelAsync();
            return trainers;
        }

        public async Task<PersonalTrainerModel?> GetPersonalTrainerByIdAsync(int personalTrainerId)
        {
            //if (personalTrainerId <= 0)
            //    throw new ArgumentException("Invalid personal trainer ID.");

            var trainer = await personalTrainerRepository.GetPersonalTrainerModelByIdAsync(personalTrainerId);
            //if (trainer == null)
            //{
            //    throw new ArgumentException("Personal trainer not found.");
            //}
            return trainer;
        }

        public async Task AddPersonalTrainerAsync(PersonalTrainerModel personalTrainerModel)
        {
            //if (personalTrainerModel == null)
            //    throw new ArgumentNullException(nameof(personalTrainerModel));

            // Validate FirstName and LastName
            //if (string.IsNullOrWhiteSpace(personalTrainerModel.FirstName) || string.IsNullOrWhiteSpace(personalTrainerModel.LastName))
            //    throw new ArgumentException("Trainer's full name (First Name and Last Name) cannot be empty.");

            // You can also add additional validation such as checking if the trainer already exists, etc.
            await personalTrainerRepository.AddPersonalTrainerModelAsync(personalTrainerModel);
        }

        public async Task DeletePersonalTrainerAsync(int personalTrainerId)
        {
            //if (personalTrainerId <= 0)
            //    throw new ArgumentException("Invalid personal trainer ID.");

            var trainer = await personalTrainerRepository.GetPersonalTrainerModelByIdAsync(personalTrainerId);
            //if (trainer == null)
            //{
            //    throw new ArgumentException("Personal trainer not found.");
            //}

            await personalTrainerRepository.DeletePersonalTrainerModelAsync(personalTrainerId);
        }

    }
}
