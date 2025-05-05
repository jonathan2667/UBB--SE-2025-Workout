//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using NeoIsisJob.Repositories;
//using NeoIsisJob.Models;
//using NeoIsisJob.Repositories.Interfaces;

//namespace Tests.Repo.Mocks
//{
//    class UserWorkoutMock : IUserWorkoutRepository
//    {
//        private readonly List<UserWorkoutModel> userWorkouts;

//        public UserWorkoutMock()
//        {
//            userWorkouts = new List<UserWorkoutModel>
//            {
//                new UserWorkoutModel(1, 1, DateTime.Now, false),
//                new UserWorkoutModel(1, 2, DateTime.Now, false),
//                new UserWorkoutModel(1, 3, DateTime.Now, false)
//            };
//        }

//        public List<UserWorkoutModel> GetUserWorkoutModelByDate(DateTime date)
//        {
//            return userWorkouts.Where(uw => uw.Date.Date == date.Date).ToList();
//        }

//        public UserWorkoutModel GetUserWorkoutModel(int userId, int workoutId, DateTime date)
//        {
//            return userWorkouts.FirstOrDefault(uw => uw.UserId == userId && uw.WorkoutId == workoutId);
//        }

//        public void AddUserWorkout(UserWorkoutModel userWorkout)
//        {
//            if (userWorkout == null)
//                throw new ArgumentNullException(nameof(userWorkout));
//            userWorkouts.Add(userWorkout);
//        }

//        public void UpdateUserWorkout(UserWorkoutModel userWorkout)
//        {
//            if (userWorkout == null)
//                throw new ArgumentNullException(nameof(userWorkout));
//            var existingWorkout = GetUserWorkoutModel(userWorkout.UserId, userWorkout.WorkoutId, userWorkout.Date);
//            if (existingWorkout != null)
//            {
//                existingWorkout.Completed = userWorkout.Completed;
//            }
//            else
//            {
//                throw new KeyNotFoundException("User workout not found.");
//            }
//        }

//        public void DeleteUserWorkout(int userId, int workoutId, DateTime date)
//        {
//            var userWorkout = GetUserWorkoutModel(userId, workoutId, date);
//            if (userWorkout != null)
//            {
//                userWorkouts.Remove(userWorkout);
//            }
//            else
//            {
//                throw new KeyNotFoundException("User workout not found.");
//            }
//        }
//    }
//}
