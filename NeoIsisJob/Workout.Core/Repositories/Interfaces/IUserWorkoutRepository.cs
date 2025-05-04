using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    public interface IUserWorkoutRepository
    {
        Task<List<UserWorkoutModel>> GetUserWorkoutModelByDateAsync(DateTime date);
        Task<UserWorkoutModel?> GetUserWorkoutModelAsync(int userId, int workoutId, DateTime date);
        Task AddUserWorkoutAsync(UserWorkoutModel userWorkout);
        Task UpdateUserWorkoutAsync(UserWorkoutModel userWorkout);
        Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date);
    }
}
