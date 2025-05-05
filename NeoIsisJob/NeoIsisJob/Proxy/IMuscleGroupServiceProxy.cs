using System.Threading.Tasks;
using Refit;
using Workout.Core.Models;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.Proxy
{
    public interface IMuscleGroupServiceProxy : IMuscleGroupService
    {
        [Get("/api/musclegroup/{muscleGroupId}")]
        Task<MuscleGroupModel> GetMuscleGroupByIdAsync(int muscleGroupId);
    }
}
