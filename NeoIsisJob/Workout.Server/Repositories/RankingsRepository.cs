using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Server.Data;

namespace Workout.Server.Repositories
{
    public class RankingsRepository : IRankingsRepository
    {
        private readonly WorkoutDbContext _context;

        public RankingsRepository(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<RankingModel?> GetRankingByFullIDAsync(int userId, int muscleGroupId)
        {
            try
            {
                return await _context.Rankings
                    .Include(r => r.MuscleGroup)
                    .FirstOrDefaultAsync(r => r.UID == userId && r.MGID == muscleGroupId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching ranking by full ID: " + ex.Message);
            }
        }

        public async Task<IList<RankingModel>> GetAllRankingsByUserIDAsync(int userId)
        {
            try
            {
                return await _context.Rankings
                    .Include(r => r.MuscleGroup)
                    .Where(r => r.UID == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching rankings by user ID: " + ex.Message);
            }
        }
    }
}
