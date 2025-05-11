using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class WorkoutTypeServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "workouttype";

        public WorkoutTypeServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        public async Task InsertWorkoutTypeAsync(string workoutTypeName)
        {
            try
            {
                var data = new { workoutTypeName };
                await PostAsync($"{EndpointName}", data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting workout type: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteWorkoutTypeAsync(int workoutTypeId)
        {
            try
            {
                await DeleteAsync($"{EndpointName}/{workoutTypeId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting workout type: {ex.Message}");
                throw;
            }
        }

        public async Task<WorkoutTypeModel> GetWorkoutTypeByIdAsync(int workoutTypeId)
        {
            try
            {
                var result = await GetAsync<WorkoutTypeModel>($"{EndpointName}/{workoutTypeId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching workout type: {ex.Message}");
                throw;
            }
        }

        public async Task<IList<WorkoutTypeModel>> GetAllWorkoutTypesAsync()
        {
            try
            {
                var results = await GetAsync<IList<WorkoutTypeModel>>($"{EndpointName}");
                return results ?? new List<WorkoutTypeModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all workout types: {ex.Message}");
                return new List<WorkoutTypeModel>();
            }
        }
    }
}