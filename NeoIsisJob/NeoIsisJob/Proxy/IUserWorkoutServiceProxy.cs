using System.Threading.Tasks;
using Refit;
using Workout.Core.Models;

namespace Workout.Core.ServiceProxies
{
    public interface IUserWorkoutServiceProxy
    {
        [Get("/api/userworkout/{userId}/{date}")]
        Task<UserWorkoutModel> GetUserWorkoutForDateAsync(int userId, string date); // Format: yyyy-MM-dd

        [Post("/api/userworkout")]
        Task AddUserWorkoutAsync([Body] UserWorkoutModel userWorkout);

        [Post("/api/userworkout/complete")]
        Task CompleteUserWorkoutAsync([Query] int userId, [Query] int workoutId, [Query] string date); // Format: yyyy-MM-dd

        [Delete("/api/userworkout")]
        Task DeleteUserWorkoutAsync([Query] int userId, [Query] int workoutId, [Query] string date); // Format: yyyy-MM-dd
    }
}
