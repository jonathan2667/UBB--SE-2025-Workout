using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Workout.Core.Models;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.Proxy
{
    public interface ICompleteWorkoutServiceProxy : ICompleteWorkoutService
    {
        [Get("/api/completeworkout")]
        Task<IList<CompleteWorkoutModel>> GetAllCompleteWorkoutsAsync();

        [Get("/api/completeworkout/{workoutId}")]
        Task<IList<CompleteWorkoutModel>> GetCompleteWorkoutsByWorkoutIdAsync(int workoutId);

        [Delete("/api/completeworkout/{workoutId}")]
        Task DeleteCompleteWorkoutsByWorkoutIdAsync(int workoutId);

        [Post("/api/completeworkout")]
        Task InsertCompleteWorkoutAsync(
            [Body(BodySerializationMethod.UrlEncoded)]
            [AliasAs("workoutId")] int workoutId,
            [AliasAs("exerciseId")] int exerciseId,
            [AliasAs("sets")] int sets,
            [AliasAs("repetitionsPerSet")] int repetitionsPerSet
        );
    }
}
