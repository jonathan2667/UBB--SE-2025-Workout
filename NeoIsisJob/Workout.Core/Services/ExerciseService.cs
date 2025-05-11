using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.IServices;
using Workout.Core.IRepositories;

namespace Workout.Core.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository exerciseRepository;

        public ExerciseService(IExerciseRepository exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
        }

        public async Task<ExercisesModel> GetExerciseByIdAsync(int exerciseId)
        {
            return await exerciseRepository
                         .GetExerciseByIdAsync(exerciseId);
        }

        public async Task<IList<ExercisesModel>> GetAllExercisesAsync()
        {
            return await exerciseRepository
                         .GetAllExercisesAsync();
        }
    }
}
