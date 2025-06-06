using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.IServices;

namespace Workout.Core.Services
{
    public class UserWorkoutService : IUserWorkoutService
    {
        private readonly IUserWorkoutRepository userWorkoutRepository;

        public UserWorkoutService(IUserWorkoutRepository userWorkoutRepository = null)
        {
            this.userWorkoutRepository = userWorkoutRepository
                ?? throw new ArgumentNullException(nameof(userWorkoutRepository));
        }

        public async Task<UserWorkoutModel> GetUserWorkoutForDateAsync(int userId, DateTime date)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");
            }
            if (date == default)
            {
                throw new ArgumentException("Date must be specified.", nameof(date));
            }

            var workouts = await userWorkoutRepository
                                .GetUserWorkoutModelByDateAsync(date)
                                .ConfigureAwait(false);
            return workouts.FirstOrDefault(w => w.UID == userId);
        }

        public async Task AddUserWorkoutAsync(UserWorkoutModel userWorkout)
        {
            if (userWorkout == null)
            {
                throw new ArgumentNullException(nameof(userWorkout));
            }
            if (userWorkout.UID <= 0)
            {
                throw new ArgumentException("UserId must be positive.", nameof(userWorkout));
            }
            if (userWorkout.Date == default)
            {
                throw new ArgumentException("Date must be specified.", nameof(userWorkout));
            }

            var existing = await GetUserWorkoutForDateAsync(userWorkout.UID, userWorkout.Date)
                                .ConfigureAwait(false);
            if (existing != null)
            {
                await userWorkoutRepository
                      .UpdateUserWorkoutAsync(userWorkout)
                      .ConfigureAwait(false);
            }
            else
            {
                await userWorkoutRepository
                      .AddUserWorkoutAsync(userWorkout)
                      .ConfigureAwait(false);
            }
        }

        public async Task CompleteUserWorkoutAsync(int userId, int workoutId, DateTime date)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");
            }
            if (workoutId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(workoutId), "workoutId must be positive.");
            }
            if (date == default)
            {
                throw new ArgumentException("Date must be specified.", nameof(date));
            }

            var workout = await userWorkoutRepository
                                .GetUserWorkoutModelAsync(userId, workoutId, date)
                                .ConfigureAwait(false);
            if (workout != null)
            {
                workout.Completed = true;
                await userWorkoutRepository
                      .UpdateUserWorkoutAsync(workout)
                      .ConfigureAwait(false);
            }
        }

        public async Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");
            }
            if (workoutId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(workoutId), "workoutId must be positive.");
            }
            if (date == default)
            {
                throw new ArgumentException("Date must be specified.", nameof(date));
            }

            await userWorkoutRepository
                  .DeleteUserWorkoutAsync(userId, workoutId, date)
                  .ConfigureAwait(false);
        }
    }
}
