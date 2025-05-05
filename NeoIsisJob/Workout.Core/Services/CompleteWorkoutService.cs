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
        private readonly ICompleteWorkoutRepository _completeWorkoutRepository;

        //public CompleteWorkoutService(IDatabaseHelper databaseHelper)
        //{
        //    //if (databaseHelper == null) throw new ArgumentNullException(nameof(databaseHelper));
        //    _completeWorkoutRepository = new CompleteWorkoutRepo(databaseHelper);
        //}

        public CompleteWorkoutService(ICompleteWorkoutRepository completeWorkoutRepository)
        {
            _completeWorkoutRepository = completeWorkoutRepository
                ?? throw new ArgumentNullException(nameof(completeWorkoutRepository));
        }

        //public CompleteWorkoutService()
        //    : this(new CompleteWorkoutRepo(new DatabaseHelper()))
        //{
        //}

        public async Task<IList<CompleteWorkoutModel>> GetAllCompleteWorkoutsAsync()
        {
            return await _completeWorkoutRepository
                         .GetAllCompleteWorkoutsAsync();
                         //.ConfigureAwait(false);
        }

        public async Task<IList<CompleteWorkoutModel>> GetCompleteWorkoutsByWorkoutIdAsync(int workoutId)
        {
            if (workoutId <= 0)
                throw new ArgumentOutOfRangeException(nameof(workoutId), "workoutId must be positive.");

            var all = await _completeWorkoutRepository
                             .GetAllCompleteWorkoutsAsync();
                             //.ConfigureAwait(false);
            return all.Where(cw => cw.WID == workoutId).ToList();
        }

        public async Task DeleteCompleteWorkoutsByWorkoutIdAsync(int workoutId)
        {
            //if (workoutId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(workoutId), "workoutId must be positive.");

            await _completeWorkoutRepository
                  .DeleteCompleteWorkoutsByWorkoutIdAsync(workoutId);
                  //.ConfigureAwait(false);
        }

        public async Task InsertCompleteWorkoutAsync(int workoutId, int exerciseId, int sets, int repetitionsPerSet)
        {
            //if (workoutId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(workoutId), "workoutId must be positive.");
            //if (exerciseId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(exerciseId), "exerciseId must be positive.");
            //if (sets <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(sets), "sets must be positive.");
            //if (repetitionsPerSet <= 0)
            //throw new ArgumentOutOfRangeException(nameof(repetitionsPerSet), "repetitionsPerSet must be positive.");

            await _completeWorkoutRepository
                  .InsertCompleteWorkoutAsync(workoutId, exerciseId, sets, repetitionsPerSet);
                  //.ConfigureAwait(false);
        }
    }
}
