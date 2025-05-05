using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Server.Repositories;
using Workout.Core.IServices;
using Workout.Core.IRepositories;

namespace Workout.Server.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseService(IExerciseRepository exerciseRepository = null)
        {
            _exerciseRepository = exerciseRepository
                ?? new ExerciseRepo(); //throw new ArgumentNullException(nameof(exerciseRepository));
        }

        public async Task<ExercisesModel> GetExerciseByIdAsync(int exerciseId)
        {
            //if (exerciseId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(exerciseId), "exerciseId must be positive.");

            return await _exerciseRepository
                         .GetExerciseByIdAsync(exerciseId);
                         //.ConfigureAwait(false);
        }

        public async Task<IList<ExercisesModel>> GetAllExercisesAsync()
        {
            return await _exerciseRepository
                         .GetAllExercisesAsync();
                         //.ConfigureAwait(false);
        }
    }
}
