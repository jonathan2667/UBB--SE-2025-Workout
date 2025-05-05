using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface IMuscleGroupRepo
    {
        Task<MuscleGroupModel?> GetMuscleGroupByIdAsync(int muscleGroupId);
        Task<List<MuscleGroupModel>> GetAllMuscleGroupsAsync();
    }
}
