using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Workout.Core.Models;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.Proxy
{
    public interface IExerciseServiceProxy : IExerciseService
    {
        [Get("/api/exercise/{exerciseId}")]
        Task<ExercisesModel> GetExerciseByIdAsync(int exerciseId);

        [Get("/api/exercise")]
        Task<IList<ExercisesModel>> GetAllExercisesAsync();
    }
}
