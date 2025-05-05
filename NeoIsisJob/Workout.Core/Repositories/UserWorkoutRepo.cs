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
    public class UserWorkoutRepo : IUserWorkoutRepository
    {
        private readonly WorkoutDbContext _context;

        public UserWorkoutRepo(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserWorkoutModel>> GetUserWorkoutModelByDateAsync(DateTime date)
        {
            return await _context.UserWorkouts
                .Include(uw => uw.User)
                .Include(uw => uw.Workout)
                .Where(uw => uw.Date.Date == date.Date)
                .ToListAsync();
        }

        public async Task<UserWorkoutModel?> GetUserWorkoutModelAsync(int userId, int workoutId, DateTime date)
        {
            return await _context.UserWorkouts
                .Include(uw => uw.User)
                .Include(uw => uw.Workout)
                .FirstOrDefaultAsync(uw => uw.UID == userId && uw.WID == workoutId && uw.Date.Date == date.Date);
        }

        public async Task AddUserWorkoutAsync(UserWorkoutModel userWorkout)
        {
            _context.UserWorkouts.Add(userWorkout);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserWorkoutAsync(UserWorkoutModel userWorkout)
        {
            var existingUserWorkout = await _context.UserWorkouts
                .FirstOrDefaultAsync(uw => uw.UID == userWorkout.UID && uw.WID == userWorkout.WID && uw.Date.Date == userWorkout.Date.Date);

            if (existingUserWorkout != null)
            {
                existingUserWorkout.Completed = userWorkout.Completed;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date)
        {
            var userWorkout = await _context.UserWorkouts
                .FirstOrDefaultAsync(uw => uw.UID == userId && uw.WID == workoutId && uw.Date.Date == date.Date);
                
            if (userWorkout != null)
            {
                _context.UserWorkouts.Remove(userWorkout);
                await _context.SaveChangesAsync();
            }
        }
    }
}
