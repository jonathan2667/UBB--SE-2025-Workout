using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Data.Interfaces;
using Workout.Core.Models;
using Workout.Core.IServices;
using Workout.Core.Data;
using Workout.Core.Repositories;
using Workout.Core.IRepositories;

namespace Workout.Core.Services
{
    public class CompleteWorkoutService : ICompleteWorkoutService
    {
        private readonly ICompleteWorkoutRepository completeWorkoutRepository;
        public CompleteWorkoutService(ICompleteWorkoutRepository completeWorkoutRepository)
        {
            this.completeWorkoutRepository = completeWorkoutRepository
                ?? throw new ArgumentNullException(nameof(completeWorkoutRepository));
        }

        public async Task<IList<CompleteWorkoutModel>> GetAllCompleteWorkoutsAsync()
        {
            return await completeWorkoutRepository
                         .GetAllCompleteWorkoutsAsync();
        }

        public async Task<IList<CompleteWorkoutModel>> GetCompleteWorkoutsByWorkoutIdAsync(int workoutId)
        {
            if (workoutId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(workoutId), "workoutId must be positive.");
            }

            var all = await completeWorkoutRepository
                             .GetAllCompleteWorkoutsAsync();
            return all.Where(cw => cw.WID == workoutId).ToList();
        }

        public async Task DeleteCompleteWorkoutsByWorkoutIdAsync(int workoutId)
        {
            await completeWorkoutRepository
                  .DeleteCompleteWorkoutsByWorkoutIdAsync(workoutId);
        }

        public async Task InsertCompleteWorkoutAsync(int workoutId, int exerciseId, int sets, int repetitionsPerSet)
        {
            await completeWorkoutRepository
                  .InsertCompleteWorkoutAsync(workoutId, exerciseId, sets, repetitionsPerSet);
        }
    }
}
