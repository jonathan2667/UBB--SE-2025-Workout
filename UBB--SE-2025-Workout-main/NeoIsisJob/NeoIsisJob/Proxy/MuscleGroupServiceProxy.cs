using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class MuscleGroupServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "musclegroup";

        public MuscleGroupServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        public async Task<MuscleGroupModel> GetMuscleGroupByIdAsync(int muscleGroupId)
        {
            try
            {
                var result = await GetAsync<MuscleGroupModel>($"{EndpointName}/{muscleGroupId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching muscle group: {ex.Message}");
                throw;
            }
        }
    }
}