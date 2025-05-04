using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Services.Interfaces
{
    public interface IMuscleGroupService
    {
        /// <summary>
        /// Retrieves a muscle group by its ID.
        /// </summary>
        Task<MuscleGroupModel> GetMuscleGroupByIdAsync(int muscleGroupId);
    }

}
