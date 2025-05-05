using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Server.Data;

namespace Workout.Server.Repositories
{
    public class MuscleGroupRepo : IMuscleGroupRepo
    {
        private readonly WorkoutDbContext _context;

        public MuscleGroupRepo(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<MuscleGroupModel?> GetMuscleGroupByIdAsync(int muscleGroupId)
        {
            try
            {
                return await _context.MuscleGroups
                    .FirstOrDefaultAsync(mg => mg.MGID == muscleGroupId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching muscle group by ID: " + ex.Message);
            }
        }

        public async Task<List<MuscleGroupModel>> GetAllMuscleGroupsAsync()
        {
            try
            {
                return await _context.MuscleGroups.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching muscle groups: " + ex.Message);
            }
        }
    }
}
