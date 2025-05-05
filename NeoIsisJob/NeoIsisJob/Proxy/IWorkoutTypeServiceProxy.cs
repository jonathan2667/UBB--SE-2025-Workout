using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Services.Interfaces
{
    public interface IWorkoutTypeServiceProxy : IWorkoutTypeService
    {
        [Post("/workouttypes")]
        Task InsertWorkoutTypeAsync([Body] string workoutTypeName);

        [Delete("/workouttypes/{workoutTypeId}")]
        Task DeleteWorkoutTypeAsync(int workoutTypeId);

        [Get("/workouttypes/{workoutTypeId}")]
        Task<WorkoutTypeModel> GetWorkoutTypeByIdAsync(int workoutTypeId);

        [Get("/workouttypes")]
        Task<IList<WorkoutTypeModel>> GetAllWorkoutTypesAsync();
    }
}
