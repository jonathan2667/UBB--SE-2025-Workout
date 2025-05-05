using System.Threading.Tasks;
using System.Collections.Generic;
using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface IRankingsRepository
    {
        Task<RankingModel?> GetRankingByFullIDAsync(int userId, int muscleGroupId);
        Task<IList<RankingModel>> GetAllRankingsByUserIDAsync(int userId);
    }
}
