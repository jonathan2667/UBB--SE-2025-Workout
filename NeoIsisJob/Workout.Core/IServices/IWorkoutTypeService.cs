using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IServices
{
    public interface IWorkoutTypeService
    {
        /// <summary>
        /// Inserts a new workout type.
        /// </summary>
        Task InsertWorkoutTypeAsync(string workoutTypeName);

        /// <summary>
        /// Deletes an existing workout type by ID.
        /// </summary>
        Task DeleteWorkoutTypeAsync(int workoutTypeId);

        /// <summary>
        /// Retrieves a workout type by its ID.
        /// </summary>
        Task<WorkoutTypeModel> GetWorkoutTypeByIdAsync(int workoutTypeId);

        /// <summary>
        /// Retrieves all workout types.
        /// </summary>
        Task<IList<WorkoutTypeModel>> GetAllWorkoutTypesAsync();
    }

}
