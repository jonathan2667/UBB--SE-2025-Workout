using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class ExerciseRepo : IExerciseRepository
    {
        private readonly WorkoutDbContext context;

        public ExerciseRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<IList<ExercisesModel>> GetAllExercisesAsync()
        {
            try
            {
                return await context.Exercises
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
                return await context.Exercises
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
