using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.Proxy
{
    public interface IRankingsServiceProxy : IRankingsService
    {
        [Get("/rankings/{userId}")]
        Task<IList<RankingModel>> GetAllRankingsByUserIDAsync(int userId);

        [Get("/rankings/{userId}/musclegroup/{muscleGroupId}")]
        Task<RankingModel> GetRankingByFullIDAsync(int userId, int muscleGroupId);

        // This method is still present for completeness but should be executed in-memory.
        int CalculatePointsToNextRank(int currentPoints, IList<RankDefinition> rankDefinitions);
    }
}
