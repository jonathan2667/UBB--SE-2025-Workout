using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class CompleteWorkoutRepo : ICompleteWorkoutRepository
    {
        private readonly WorkoutDbContext context;

        public CompleteWorkoutRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<IList<CompleteWorkoutModel>> GetAllCompleteWorkoutsAsync()
        {
            try
            {
                return await context.CompleteWorkouts
                    .Include(cw => cw.Workout)
                    .Include(cw => cw.Exercise)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching complete workouts: " + ex.Message);
            }
        }

        public async Task DeleteCompleteWorkoutsByWorkoutIdAsync(int workoutId)
        {
            try
            {
                var workoutsToDelete = await context.CompleteWorkouts
                    .Where(cw => cw.WID == workoutId)
                    .ToListAsync();

                context.CompleteWorkouts.RemoveRange(workoutsToDelete);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting complete workouts: " + ex.Message);
            }
        }

        public async Task InsertCompleteWorkoutAsync(int workoutId, int exerciseId, int sets, int repetitionsPerSet)
        {
            try
            {
                var completeWorkout = new CompleteWorkoutModel
                {
                    WID = workoutId,
                    EID = exerciseId,
                    Sets = sets,
                    RepsPerSet = repetitionsPerSet
                };

                context.CompleteWorkouts.Add(completeWorkout);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting complete workout: " + ex.Message);
            }
        }
    }
}
