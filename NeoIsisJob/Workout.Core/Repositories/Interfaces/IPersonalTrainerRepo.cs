using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    internal interface IPersonalTrainerRepo
    {
        PersonalTrainerModel GetPersonalTrainerModelById(int personalTrainerId);
        List<PersonalTrainerModel> GetAllPersonalTrainerModel();
        void AddPersonalTrainerModel(PersonalTrainerModel personalTrainer);
        void DeletePersonalTrainerModel(int personalTrainerId);
    }
}
