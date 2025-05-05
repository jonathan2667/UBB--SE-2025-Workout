using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class RankingsServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "rankings";

        public RankingsServiceProxy(IConfiguration configuration = null) 
            : base(configuration)
        {
        }

        public async Task<IList<RankingModel>> GetAllRankingsByUserIDAsync(int userId)
        {
            try
            {
                var results = await GetAsync<IList<RankingModel>>($"{EndpointName}/user/{userId}");
                return results ?? new List<RankingModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user rankings: {ex.Message}");
                return new List<RankingModel>();
            }
        }

        public async Task<RankingModel> GetRankingByFullIDAsync(int userId, int muscleGroupId)
        {
            try
            {
                var result = await GetAsync<RankingModel>($"{EndpointName}/{userId}/{muscleGroupId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching ranking: {ex.Message}");
                throw;
            }
        }

        public int CalculatePointsToNextRank(int currentPoints, IList<RankDefinition> rankDefinitions)
        {
            if (rankDefinitions == null || rankDefinitions.Count == 0)
            {
                return 0;
            }

            // Find the next rank definition based on current points
            var nextRank = rankDefinitions
                .OrderBy(r => r.RequiredPoints)
                .FirstOrDefault(r => r.RequiredPoints > currentPoints);

            if (nextRank == null)
            {
                // User is already at max rank
                return 0;
            }

            return nextRank.RequiredPoints - currentPoints;
        }
    }
} 