using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.IServices;

namespace Workout.Core.Services
{
    public class RankingsService : IRankingsService
    {
        private readonly IRankingsRepository rankingsRepository;

        public RankingsService(IRankingsRepository rankingsRepository)
        {
            this.rankingsRepository = rankingsRepository
                ?? throw new ArgumentNullException(nameof(rankingsRepository));
        }

        public async Task<IList<RankingModel>> GetAllRankingsByUserIDAsync(int userId)
        {
            return await rankingsRepository
                         .GetAllRankingsByUserIDAsync(userId);
        }

        public async Task<RankingModel> GetRankingByFullIDAsync(int userId, int muscleGroupId)
        {
            return await rankingsRepository
                         .GetRankingByFullIDAsync(userId, muscleGroupId);
        }

        public int CalculatePointsToNextRank(int currentPoints, IList<RankDefinition> rankDefinitions)
        {
            var currentRankDefinition = rankDefinitions.FirstOrDefault(r =>
               currentPoints >= r.MinPoints && currentPoints < r.MaxPoints)
               ?? rankDefinitions.Last();

            // Find the next rank (with higher minimum points)
            var nextRank = rankDefinitions.FirstOrDefault(r => r.MinPoints > currentRankDefinition.MinPoints);

            // Calculate points needed to reach next rank or return 0 if at highest rank
            return nextRank?.MinPoints - currentPoints ?? 0;
        }
    }
}
