using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    public interface IWorkoutTypeRepository
    {
        Task<WorkoutTypeModel> GetWorkoutTypeByIdAsync(int workoutTypeId);
        Task InsertWorkoutTypeAsync(string workoutTypeName);
        Task DeleteWorkoutTypeAsync(int workoutTypeId);
        Task<IList<WorkoutTypeModel>> GetAllWorkoutTypesAsync();
    }
}
