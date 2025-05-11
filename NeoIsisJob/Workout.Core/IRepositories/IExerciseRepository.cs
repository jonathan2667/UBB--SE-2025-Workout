using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface IExerciseRepository
    {
        Task<ExercisesModel?> GetExerciseByIdAsync(int exerciseId);
        Task<IList<ExercisesModel>> GetAllExercisesAsync();
    }
}
