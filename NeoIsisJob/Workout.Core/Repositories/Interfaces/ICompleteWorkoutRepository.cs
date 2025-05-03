using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    internal interface ICompleteWorkoutRepository
    {
        IList<CompleteWorkoutModel> GetAllCompleteWorkouts();
        void DeleteCompleteWorkoutsByWorkoutId(int workoutId);
        void InsertCompleteWorkout(int workoutId, int exerciseId, int sets, int repetitionsPerSet);
    }
}
