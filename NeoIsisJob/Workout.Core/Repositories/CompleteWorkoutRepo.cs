using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;
using Workout.Server.Data;

namespace Workout.Core.Repositories
{
    public class CompleteWorkoutRepo : ICompleteWorkoutRepository
    {
        private readonly WorkoutDbContext _context;

        public CompleteWorkoutRepo(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<IList<CompleteWorkoutModel>> GetAllCompleteWorkoutsAsync()
        {
            try
            {
                return await _context.CompleteWorkouts
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
                var workoutsToDelete = await _context.CompleteWorkouts
                    .Where(cw => cw.WID == workoutId)
                    .ToListAsync();

                _context.CompleteWorkouts.RemoveRange(workoutsToDelete);
                await _context.SaveChangesAsync();
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

                _context.CompleteWorkouts.Add(completeWorkout);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting complete workout: " + ex.Message);
            }
        }
    }
}
