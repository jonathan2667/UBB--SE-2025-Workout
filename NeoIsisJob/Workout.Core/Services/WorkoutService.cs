using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.IServices;

namespace Workout.Core.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository = null)
        {
            this.workoutRepository = workoutRepository
                ?? throw new ArgumentNullException(nameof(workoutRepository));
        }

        public async Task<WorkoutModel> GetWorkoutAsync(int workoutId)
        {
            return await workoutRepository
                         .GetWorkoutByIdAsync(workoutId);
        }

        public async Task<WorkoutModel> GetWorkoutByNameAsync(string workoutName)
        {
            return await workoutRepository
                         .GetWorkoutByNameAsync(workoutName);
        }

        public async Task InsertWorkoutAsync(string workoutName, int workoutTypeId)
        {
            if (string.IsNullOrWhiteSpace(workoutName))
            {
                throw new ArgumentException("Workout name cannot be empty.", nameof(workoutName));
            }
            try
            {
                await workoutRepository
                      .InsertWorkoutAsync(workoutName, workoutTypeId);
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
        public async Task InsertWorkoutAsync(string workoutName, int workoutTypeId, string description)
        {
            if (string.IsNullOrWhiteSpace(workoutName))
            {
                throw new ArgumentException("Workout name cannot be empty.", nameof(workoutName));
            }

            try
            {
                await workoutRepository.InsertWorkoutAsync(workoutName, workoutTypeId, description);
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
            await workoutRepository
                  .DeleteWorkoutAsync(workoutId);
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
                await workoutRepository
                      .UpdateWorkoutAsync(workout);
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
            return await workoutRepository
                         .GetAllWorkoutsAsync();
        }
    }
}

