using System.Threading.Tasks;
using System.Collections.Generic;
using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface IPersonalTrainerRepo
    {
        Task<PersonalTrainerModel?> GetPersonalTrainerModelByIdAsync(int personalTrainerId);
        Task<List<PersonalTrainerModel>> GetAllPersonalTrainerModelAsync();
        Task AddPersonalTrainerModelAsync(PersonalTrainerModel personalTrainer);
        Task DeletePersonalTrainerModelAsync(int personalTrainerId);
    }
}
