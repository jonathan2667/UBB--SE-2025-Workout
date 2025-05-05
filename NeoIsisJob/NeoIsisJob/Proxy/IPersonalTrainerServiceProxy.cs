using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Workout.Core.Models;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.Proxy
{
    public interface IPersonalTrainerServiceProxy : IPersonalTrainerService
    {
        [Get("/api/personaltrainer")]
        Task<List<PersonalTrainerModel>> GetAllPersonalTrainersAsync();

        [Get("/api/personaltrainer/{personalTrainerId}")]
        Task<PersonalTrainerModel?> GetPersonalTrainerByIdAsync(int personalTrainerId);

        [Post("/api/personaltrainer")]
        Task AddPersonalTrainerAsync([Body] PersonalTrainerModel personalTrainerModel);

        [Delete("/api/personaltrainer/{personalTrainerId}")]
        Task DeletePersonalTrainerAsync(int personalTrainerId);
    }
}
