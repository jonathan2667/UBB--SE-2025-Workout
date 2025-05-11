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
    public class UserWorkoutRepo : IUserWorkoutRepository
    {
        private readonly WorkoutDbContext context;

        public UserWorkoutRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<List<UserWorkoutModel>> GetUserWorkoutModelByDateAsync(DateTime date)
        {
            return await context.UserWorkouts
                .Include(uw => uw.User)
                .Include(uw => uw.Workout)
                .Where(uw => uw.Date.Date == date.Date)
                .ToListAsync();
        }

        public async Task<UserWorkoutModel?> GetUserWorkoutModelAsync(int userId, int workoutId, DateTime date)
        {
            return await context.UserWorkouts
                .Include(uw => uw.User)
                .Include(uw => uw.Workout)
                .FirstOrDefaultAsync(uw => uw.UID == userId && uw.WID == workoutId && uw.Date.Date == date.Date);
        }

        public async Task AddUserWorkoutAsync(UserWorkoutModel userWorkout)
        {
            context.UserWorkouts.Add(userWorkout);
            await context.SaveChangesAsync();
        }

        public async Task UpdateUserWorkoutAsync(UserWorkoutModel userWorkout)
        {
            var existingUserWorkout = await context.UserWorkouts
                .FirstOrDefaultAsync(uw => uw.UID == userWorkout.UID && uw.WID == userWorkout.WID && uw.Date.Date == userWorkout.Date.Date);

            if (existingUserWorkout != null)
            {
                existingUserWorkout.Completed = userWorkout.Completed;
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date)
        {
            var userWorkout = await context.UserWorkouts
                .FirstOrDefaultAsync(uw => uw.UID == userId && uw.WID == workoutId && uw.Date.Date == date.Date);
            if (userWorkout != null)
            {
                context.UserWorkouts.Remove(userWorkout);
                await context.SaveChangesAsync();
            }
        }
    }
}
