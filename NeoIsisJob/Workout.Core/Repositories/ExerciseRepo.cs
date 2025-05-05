using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;
using Workout.Server.Data;

namespace Workout.Core.Repositories
{
    public class ExerciseRepo : IExerciseRepository
    {
        private readonly WorkoutDbContext _context;

        public ExerciseRepo(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<IList<ExercisesModel>> GetAllExercisesAsync()
        {
            try
            {
                return await _context.Exercises
                    .Include(e => e.MuscleGroup)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching exercises: " + ex.Message);
            }
        }

        public async Task<ExercisesModel?> GetExerciseByIdAsync(int exerciseId)
        {
            try
            {
                return await _context.Exercises
                    .Include(e => e.MuscleGroup)
                    .FirstOrDefaultAsync(e => e.EID == exerciseId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching exercise by ID: " + ex.Message);
            }
        }
    }
}
