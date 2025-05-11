using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.IRepositories;

namespace Workout.Core.Services
{
    public class MuscleGroupService : IMuscleGroupService
    {
        private readonly IMuscleGroupRepo muscleGroupRepository;

        public MuscleGroupService(IMuscleGroupRepo muscleGroupRepository)
        {
            this.muscleGroupRepository = muscleGroupRepository ?? throw new ArgumentNullException(nameof(muscleGroupRepository));
        }
        public async Task<MuscleGroupModel> GetMuscleGroupByIdAsync(int muscleGroupId)
        {
            return await muscleGroupRepository
                         .GetMuscleGroupByIdAsync(muscleGroupId);
        }
    }
}
