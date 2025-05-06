using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class ExerciseServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "exercise";

        public ExerciseServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        public async Task<ExercisesModel> GetExerciseByIdAsync(int exerciseId)
        {
            try
            {
                var result = await GetAsync<ExercisesModel>($"{EndpointName}/{exerciseId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching exercise: {ex.Message}");
                throw;
            }
        }

        public async Task<IList<ExercisesModel>> GetAllExercisesAsync()
        {
            try
            {
                var results = await GetAsync<IList<ExercisesModel>>($"{EndpointName}");
                return results ?? new List<ExercisesModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all exercises: {ex.Message}");
                return new List<ExercisesModel>();
            }
        }
    }
}