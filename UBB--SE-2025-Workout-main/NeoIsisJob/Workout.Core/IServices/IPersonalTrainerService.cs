using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IServices
{
    public interface IPersonalTrainerService
    {
        Task<List<PersonalTrainerModel>> GetAllPersonalTrainersAsync();
        Task<PersonalTrainerModel?> GetPersonalTrainerByIdAsync(int personalTrainerId);
        Task AddPersonalTrainerAsync(PersonalTrainerModel personalTrainerModel);
        Task DeletePersonalTrainerAsync(int personalTrainerId);
    }
}
