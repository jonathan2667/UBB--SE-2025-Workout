using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    internal interface IWorkoutTypeRepository
    {
        WorkoutTypeModel GetWorkoutTypeById(int workoutTypeId);
        void InsertWorkoutType(string workoutTypeName);
        void DeleteWorkoutType(int workoutTypeId);
        IList<WorkoutTypeModel> GetAllWorkoutTypes();
    }
}
