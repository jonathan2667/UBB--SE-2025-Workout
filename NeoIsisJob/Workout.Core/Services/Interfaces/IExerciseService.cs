using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Services.Interfaces
{
    public interface IExerciseService
    {
        /// <summary>
        /// Retrieves a single exercise by its ID.
        /// </summary>
        Task<ExercisesModel> GetExerciseByIdAsync(int exerciseId);

        /// <summary>
        /// Retrieves all exercises.
        /// </summary>
        Task<IList<ExercisesModel>> GetAllExercisesAsync();
    }
}
