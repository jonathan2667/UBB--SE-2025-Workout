using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    internal interface IUserWorkoutRepository
    {
        List<UserWorkoutModel> GetUserWorkoutModelByDate(DateTime date);
        UserWorkoutModel GetUserWorkoutModel(int userId, int workoutId, DateTime date);
        void AddUserWorkout(UserWorkoutModel userWorkout);
        void UpdateUserWorkout(UserWorkoutModel userWorkout);
        void DeleteUserWorkout(int userId, int workoutId, DateTime date);
    }
}
