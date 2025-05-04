using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Services.Interfaces
{
    public interface IRankingsService
    {
        /// <summary>
        /// Retrieves all rankings for a given user.
        /// </summary>
        Task<IList<RankingModel>> GetAllRankingsByUserIDAsync(int userId);

        /// <summary>
        /// Retrieves a specific ranking by user and muscle group.
        /// </summary>
        Task<RankingModel> GetRankingByFullIDAsync(int userId, int muscleGroupId);

        /// <summary>
        /// Calculates points needed to reach the next rank (pure in-memory logic).
        /// </summary>
        int CalculatePointsToNextRank(int currentPoints, IList<RankDefinition> rankDefinitions);
    }

}
