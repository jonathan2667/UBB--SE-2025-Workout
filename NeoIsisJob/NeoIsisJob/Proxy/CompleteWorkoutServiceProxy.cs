using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class CompleteWorkoutServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "completeworkout";

        public CompleteWorkoutServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        public async Task<IList<CompleteWorkoutModel>> GetAllCompleteWorkoutsAsync()
        {
            try
            {
                var results = await GetAsync<IList<CompleteWorkoutModel>>($"{EndpointName}");
                return results ?? new List<CompleteWorkoutModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all complete workouts: {ex.Message}");
                return new List<CompleteWorkoutModel>();
            }
        }

        public async Task<IList<CompleteWorkoutModel>> GetCompleteWorkoutsByWorkoutIdAsync(int workoutId)
        {
            try
            {
                var results = await GetAsync<IList<CompleteWorkoutModel>>($"{EndpointName}/workout/{workoutId}");
                return results ?? new List<CompleteWorkoutModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching complete workouts by workout ID: {ex.Message}");
                return new List<CompleteWorkoutModel>();
            }
        }

        public async Task DeleteCompleteWorkoutsByWorkoutIdAsync(int workoutId)
        {
            try
            {
                await DeleteAsync($"{EndpointName}/{workoutId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting complete workouts by workout ID: {ex.Message}");
                throw;
            }
        }

        public async Task InsertCompleteWorkoutAsync(int workoutId, int exerciseId, int sets, int repetitionsPerSet)
        {
            try
            {
                var completeWorkout = new
                {
                    WorkoutId = workoutId,
                    ExerciseId = exerciseId,
                    Sets = sets,
                    RepetitionsPerSet = repetitionsPerSet
                };
                await PostAsync($"{EndpointName}", completeWorkout);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting complete workout: {ex.Message}");
                throw;
            }
        }
    }
}