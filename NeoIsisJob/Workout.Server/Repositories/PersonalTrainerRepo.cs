using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Server.Data;

namespace Workout.Server.Repositories
{
    public class PersonalTrainerRepo : IPersonalTrainerRepo
    {
        private readonly WorkoutDbContext _context;

        public PersonalTrainerRepo(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<PersonalTrainerModel?> GetPersonalTrainerModelByIdAsync(int personalTrainerId)
        {
            try
            {
                return await _context.PersonalTrainers
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
                return await _context.PersonalTrainers.ToListAsync();
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
                _context.PersonalTrainers.Add(personalTrainer);
                await _context.SaveChangesAsync();
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
                var trainer = await _context.PersonalTrainers.FindAsync(personalTrainerId);
                if (trainer != null)
                {
                    _context.PersonalTrainers.Remove(trainer);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting personal trainer: " + ex.Message);
            }
        }
    }
}
