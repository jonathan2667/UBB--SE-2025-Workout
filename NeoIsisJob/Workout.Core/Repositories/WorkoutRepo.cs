using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class WorkoutRepo : IWorkoutRepository
    {
        private readonly WorkoutDbContext _context;

        public WorkoutRepo(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<WorkoutModel> GetWorkoutByIdAsync(int workoutId)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutType)
                .FirstOrDefaultAsync(w => w.WID == workoutId);
                
            return workout ?? new WorkoutModel();
        }

        public async Task<WorkoutModel> GetWorkoutByNameAsync(string workoutName)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutType)
                .FirstOrDefaultAsync(w => w.Name == workoutName);
                
            return workout ?? new WorkoutModel();
        }

        public async Task InsertWorkoutAsync(string workoutName, int workoutTypeId)
        {
            var workout = new WorkoutModel
            {
                Name = workoutName,
                WTID = workoutTypeId
            };
            
            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            var workout = await _context.Workouts.FindAsync(workoutId);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateWorkoutAsync(WorkoutModel workout)
        {
            if (workout == null)
            {
                throw new ArgumentNullException(nameof(workout), "Workout cannot be null.");
            }

            // Check for duplicates
            bool duplicateExists = await _context.Workouts
                .AnyAsync(w => w.Name == workout.Name && w.WID != workout.WID);
                
            if (duplicateExists)
            {
                throw new Exception("A workout with this name already exists.");
            }

            // Perform the update
            var existingWorkout = await _context.Workouts.FindAsync(workout.WID);
            if (existingWorkout == null)
            {
                throw new Exception("No workout was updated. Ensure the workout ID exists.");
            }
            
            existingWorkout.Name = workout.Name;
            existingWorkout.WTID = workout.WTID;
            
            await _context.SaveChangesAsync();
        }

        public async Task<IList<WorkoutModel>> GetAllWorkoutsAsync()
        {
            return await _context.Workouts
                .Include(w => w.WorkoutType)
                .ToListAsync();
        }
    }
}
