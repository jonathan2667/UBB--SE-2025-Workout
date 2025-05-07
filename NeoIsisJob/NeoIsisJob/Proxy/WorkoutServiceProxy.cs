using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class WorkoutServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "workout";

        public WorkoutServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        public async Task<WorkoutModel> GetWorkoutAsync(int workoutId)
        {
            try
            {
                var result = await GetAsync<WorkoutModel>($"{EndpointName}/{workoutId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching workout: {ex.Message}");
                throw;
            }
        }

        public async Task<WorkoutModel> GetWorkoutByNameAsync(string workoutName)
        {
            try
            {
                var result = await GetAsync<WorkoutModel>($"{EndpointName}/name/{workoutName}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching workout by name: {ex.Message}");
                throw;
            }
        }

        public async Task InsertWorkoutAsync(string workoutName, int workoutTypeId)
        {
            try
            {
                var escaped = Uri.EscapeDataString(workoutName);
                await PostAsync($"{EndpointName}/{escaped}/{workoutTypeId}", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting workout: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            try
            {
                await DeleteAsync($"{EndpointName}/{workoutId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting workout: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateWorkoutAsync(WorkoutModel workout)
        {
            try
            {
                await PutAsync($"{EndpointName}", workout);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating workout: {ex.Message}");
                throw;
            }
        }

        public async Task<IList<WorkoutModel>> GetAllWorkoutsAsync()
        {
            try
            {
                var results = await GetAsync<IList<WorkoutModel>>($"{EndpointName}");
                return results ?? new List<WorkoutModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all workouts: {ex.Message}");
                return new List<WorkoutModel>();
            }
        }
    }
}