using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.Repositories.Interfaces;
using Workout.Core.Services.Interfaces;

namespace Workout.Core.Services
{
    public class RankingsService : IRankingsService
    {
        private readonly IRankingsRepository _rankingsRepository;

        public RankingsService(IRankingsRepository rankingsRepository)
        {
            _rankingsRepository = rankingsRepository
                ?? new RankingsRepository();//throw new ArgumentNullException(nameof(rankingsRepository));
        }

        public async Task<IList<RankingModel>> GetAllRankingsByUserIDAsync(int userId)
        {
            //if (userId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");

            return await _rankingsRepository
                         .GetAllRankingsByUserIDAsync(userId);
                         //.ConfigureAwait(false);
        }

        public async Task<RankingModel> GetRankingByFullIDAsync(int userId, int muscleGroupId)
        {
            //if (userId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");
            //if (muscleGroupId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(muscleGroupId), "muscleGroupId must be positive.");

            return await _rankingsRepository
                         .GetRankingByFullIDAsync(userId, muscleGroupId);
                         //.ConfigureAwait(false);
        }

        public int CalculatePointsToNextRank(int currentPoints, IList<RankDefinition> rankDefinitions)
        {
            //if (currentPoints < 0)
            //    throw new ArgumentOutOfRangeException(nameof(currentPoints), "currentPoints cannot be negative.");
            //if (rankDefinitions == null || !rankDefinitions.Any())
            //    throw new ArgumentException("rankDefinitions cannot be null or empty.", nameof(rankDefinitions));

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
