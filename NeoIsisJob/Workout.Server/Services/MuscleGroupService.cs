using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Server.Repositories;
using Workout.Core.IRepositories;

namespace Workout.Server.Services
{
    public class MuscleGroupService : IMuscleGroupService
    {
        private readonly IMuscleGroupRepo _muscleGroupRepository;

        public MuscleGroupService(IMuscleGroupRepo muscleGroupRepository = null)
        {
            _muscleGroupRepository = muscleGroupRepository
                ?? new MuscleGroupRepo(); //throw new ArgumentNullException(nameof(muscleGroupRepository));
        }

        public async Task<MuscleGroupModel> GetMuscleGroupByIdAsync(int muscleGroupId)
        {
            //if (muscleGroupId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(muscleGroupId), "muscleGroupId must be positive.");

            return await _muscleGroupRepository
                         .GetMuscleGroupByIdAsync(muscleGroupId);
                         //.ConfigureAwait(false);
        }
    }
}
