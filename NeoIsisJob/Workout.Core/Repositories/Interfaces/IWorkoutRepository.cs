using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    internal interface IWorkoutRepository
    {
        WorkoutModel GetWorkoutById(int workoutId);
        WorkoutModel GetWorkoutByName(string workoutName);
        void InsertWorkout(string workoutName, int workoutTypeId);
        void DeleteWorkout(int workoutId);
        void UpdateWorkout(WorkoutModel workout);
        IList<WorkoutModel> GetAllWorkouts();
    }
}
