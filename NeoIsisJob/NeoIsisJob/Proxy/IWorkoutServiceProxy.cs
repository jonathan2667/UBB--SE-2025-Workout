using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Workout.Core.Models;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.Proxy
{
    public interface IWorkoutServiceProxy : IWorkoutService
    {
        [Get("/api/workout/{workoutId}")]
        Task<WorkoutModel> GetWorkoutAsync(int workoutId);

        [Get("/api/workout/name/{workoutName}")]
        Task<WorkoutModel> GetWorkoutByNameAsync(string workoutName);

        [Post("/api/workout")]
        Task InsertWorkoutAsync([Query] string workoutName, [Query] int workoutTypeId);

        [Delete("/api/workout/{workoutId}")]
        Task DeleteWorkoutAsync(int workoutId);

        [Put("/api/workout")]
        Task UpdateWorkoutAsync([Body] WorkoutModel workout);

        [Get("/api/workout")]
        Task<IList<WorkoutModel>> GetAllWorkoutsAsync();
    }
}
