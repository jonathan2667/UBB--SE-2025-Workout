using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IServices
{
    public interface IWorkoutService
    {
        Task<WorkoutModel> GetWorkoutAsync(int workoutId);
        Task<WorkoutModel> GetWorkoutByNameAsync(string workoutName);
        Task InsertWorkoutAsync(string workoutName, int workoutTypeId);
        Task DeleteWorkoutAsync(int workoutId);
        Task UpdateWorkoutAsync(WorkoutModel workout);
        Task<IList<WorkoutModel>> GetAllWorkoutsAsync();
    }

}
