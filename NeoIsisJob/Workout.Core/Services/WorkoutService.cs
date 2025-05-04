using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.Repositories.Interfaces;
using Workout.Core.Services.Interfaces;

namespace Workout.Core.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository = null)
        {
            _workoutRepository = workoutRepository
                ?? new WorkoutRepo();//throw new ArgumentNullException(nameof(workoutRepository));
        }

        public async Task<WorkoutModel> GetWorkoutAsync(int workoutId)
        {
            //if (workoutId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(workoutId), "workoutId must be positive.");

            return await _workoutRepository
                         .GetWorkoutByIdAsync(workoutId);
                         //.ConfigureAwait(false);
        }

        public async Task<WorkoutModel> GetWorkoutByNameAsync(string workoutName)
        {
            //if (string.IsNullOrWhiteSpace(workoutName))
            //    throw new ArgumentException("Workout name cannot be empty.", nameof(workoutName));

            return await _workoutRepository
                         .GetWorkoutByNameAsync(workoutName);
                         //.ConfigureAwait(false);
        }

        public async Task InsertWorkoutAsync(string workoutName, int workoutTypeId)
        {
            if (string.IsNullOrWhiteSpace(workoutName))
                throw new ArgumentException("Workout name cannot be empty.", nameof(workoutName));
            //if (workoutTypeId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(workoutTypeId), "workoutTypeId must be positive.");

            try
            {
                await _workoutRepository
                      .InsertWorkoutAsync(workoutName, workoutTypeId);
                      //.ConfigureAwait(false);
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                throw new Exception("A workout with this name already exists.");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting workout.", ex);
            }
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            //if (workoutId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(workoutId), "workoutId must be positive.");

            await _workoutRepository
                  .DeleteWorkoutAsync(workoutId);
                  //.ConfigureAwait(false);
        }

        public async Task UpdateWorkoutAsync(WorkoutModel workout)
        {

            if (workout == null)
            {
                throw new ArgumentNullException(nameof(workout), "Workout cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(workout.Name))
            {
                throw new ArgumentException("Workout name cannot be empty or null.", nameof(workout.Name));
            }

            try
            {
                await _workoutRepository
                      .UpdateWorkoutAsync(workout);
                      //.ConfigureAwait(false);
            }
            catch (Exception ex) when (ex.Message.Contains("already exists"))
            {
                throw new Exception("A workout with this name already exists. Please choose a different name.");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the workout: {ex.Message}", ex);
            }
        }

        public async Task<IList<WorkoutModel>> GetAllWorkoutsAsync()
        {
            return await _workoutRepository
                         .GetAllWorkoutsAsync();
                         //.ConfigureAwait(false);
        }
    }
}

