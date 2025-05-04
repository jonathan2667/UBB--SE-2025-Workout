using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Services.Interfaces
{
    public interface IUserWorkoutService
    {
        /// <summary>
        /// Retrieves a user's workout for a specific date.
        /// </summary>
        Task<UserWorkoutModel> GetUserWorkoutForDateAsync(int userId, DateTime date);

        /// <summary>
        /// Adds or updates a user's workout.
        /// </summary>
        Task AddUserWorkoutAsync(UserWorkoutModel userWorkout);

        /// <summary>
        /// Marks a user's workout as complete.
        /// </summary>
        Task CompleteUserWorkoutAsync(int userId, int workoutId, DateTime date);

        /// <summary>
        /// Deletes a user's workout.
        /// </summary>
        Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date);
    }

}
