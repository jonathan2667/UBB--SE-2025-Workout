using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface ICompleteWorkoutRepository
    {
        Task<IList<CompleteWorkoutModel>> GetAllCompleteWorkoutsAsync();
        Task DeleteCompleteWorkoutsByWorkoutIdAsync(int workoutId);
        Task InsertCompleteWorkoutAsync(int workoutId, int exerciseId, int sets, int repetitionsPerSet);
    }
}
