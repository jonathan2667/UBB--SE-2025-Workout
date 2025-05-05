//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Tests.Repo.Mocks;
//using NeoIsisJob.Repositories;
//using NeoIsisJob.Services;
//using NeoIsisJob.Models;
//using NeoIsisJob.Repositories.Interfaces;

//namespace Tests.Service.Tests
//{
//    [TestClass]
//    public class UserWorkoutTests
//    {
//        private UserWorkoutService service;
//        private IUserWorkoutRepository repository;

//        [TestInitialize]
//        public void Setup()
//        {
//            repository = new UserWorkoutMock();
//            service = new UserWorkoutService(repository);

//        }

//        public UserWorkoutModel GetUserWorkoutForDate(int userId, DateTime date)
//        {
//            var userWorkouts = repository.GetUserWorkoutModelByDate(date);
//            return userWorkouts.FirstOrDefault(userWorkout => userWorkout.UserId == userId);
//        }

//        [TestMethod]
//        public void TestCompleteWorkout()
//        {
//            var workout = repository.GetUserWorkoutModel(1, 1, DateTime.Now);
//            service.CompleteUserWorkout(workout.UserId, workout.WorkoutId, workout.Date);

//            var updatedWorkout = repository.GetUserWorkoutModel(1, 1, DateTime.Now);

//            Assert.IsNotNull(updatedWorkout);
//            Assert.IsTrue(updatedWorkout.Completed);

//        }

//        [TestMethod]
//        public void TestAddWorkout()
//        {

//            DateTime date = new DateTime(2025, 4, 20);

//            var newWorkout = new UserWorkoutModel(1, 4, date, false);
//            service.AddUserWorkout(newWorkout);
//            var addedWorkout = repository.GetUserWorkoutModel(1, 4, date);
//            Assert.IsNotNull(addedWorkout);
//            Assert.AreEqual(addedWorkout.UserId, newWorkout.UserId);
//            Assert.AreEqual(addedWorkout.WorkoutId, newWorkout.WorkoutId);

//            var updateWorkout = new UserWorkoutModel(1, 4, date, true);

//            service.AddUserWorkout(updateWorkout);

//            var updatedWorkout = repository.GetUserWorkoutModel(1, 4, date);

//            Assert.IsNotNull(updateWorkout);
//            Assert.IsTrue(updatedWorkout.Completed);
//        }
//    }
//}
