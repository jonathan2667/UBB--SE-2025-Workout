using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class MuscleGroupRepo : IMuscleGroupRepo
    {
        private readonly WorkoutDbContext context;

        public MuscleGroupRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<MuscleGroupModel?> GetMuscleGroupByIdAsync(int muscleGroupId)
        {
            try
            {
                return await context.MuscleGroups
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
                return await context.MuscleGroups.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching muscle groups: " + ex.Message);
            }
        }
    }
}
