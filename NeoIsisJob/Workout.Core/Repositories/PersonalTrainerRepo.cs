using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class PersonalTrainerRepo : IPersonalTrainerRepo
    {
        private readonly WorkoutDbContext context;

        public PersonalTrainerRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<PersonalTrainerModel?> GetPersonalTrainerModelByIdAsync(int personalTrainerId)
        {
            try
            {
                return await context.PersonalTrainers
                    .FirstOrDefaultAsync(pt => pt.PTID == personalTrainerId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching personal trainer by ID: " + ex.Message);
            }
        }

        public async Task<List<PersonalTrainerModel>> GetAllPersonalTrainerModelAsync()
        {
            try
            {
                return await context.PersonalTrainers.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching all personal trainers: " + ex.Message);
            }
        }

        public async Task AddPersonalTrainerModelAsync(PersonalTrainerModel personalTrainer)
        {
            try
            {
                context.PersonalTrainers.Add(personalTrainer);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding personal trainer: " + ex.Message);
            }
        }

        public async Task DeletePersonalTrainerModelAsync(int personalTrainerId)
        {
            try
            {
                var trainer = await context.PersonalTrainers.FindAsync(personalTrainerId);
                if (trainer != null)
                {
                    context.PersonalTrainers.Remove(trainer);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting personal trainer: " + ex.Message);
            }
        }
    }
}
