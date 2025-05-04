using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Services.Interfaces
{
    public interface ICompleteWorkoutService
    {
        /// <summary>
        /// Retrieves all complete workout entries.
        /// </summary>
        Task<IList<CompleteWorkoutModel>> GetAllCompleteWorkoutsAsync();

        /// <summary>
        /// Retrieves completed workouts filtered by workout ID.
        /// </summary>
        Task<IList<CompleteWorkoutModel>> GetCompleteWorkoutsByWorkoutIdAsync(int workoutId);

        /// <summary>
        /// Deletes all completed workouts for the specified workout ID.
        /// </summary>
        Task DeleteCompleteWorkoutsByWorkoutIdAsync(int workoutId);

        /// <summary>
        /// Inserts a completed workout record.
        /// </summary>
        Task InsertCompleteWorkoutAsync(int workoutId, int exerciseId, int sets, int repetitionsPerSet);
    }

}
