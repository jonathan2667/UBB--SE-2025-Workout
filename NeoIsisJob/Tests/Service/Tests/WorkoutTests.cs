//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NeoIsisJob.Models;
//using NeoIsisJob.Repositories.Interfaces;
//using NeoIsisJob.Services;
//using NeoIsisJob.Services.Interfaces;
//using Tests.Repo.Mocks;

//namespace Tests.Service.Tests
//{
//    [TestClass]
//    public class WorkoutTests
//    {
//        private IWorkoutRepository workoutRepository;

//        private WorkoutService WorkoutService;

//        [TestInitialize]
//        public void Setup()
//        {
//            workoutRepository = new WorkoutMock();
//            WorkoutService = new WorkoutService(workoutRepository);
//        }

//        [TestMethod]
//        public void TestInsertWorkout()
//        {
//            WorkoutModel newWorkout = new WorkoutModel(4, "Workout D", 4);
//            WorkoutService.InsertWorkout(newWorkout.Name, newWorkout.WorkoutTypeId);
//            WorkoutModel insertedWorkout = workoutRepository.GetWorkoutById(newWorkout.Id);
//            Assert.IsNotNull(insertedWorkout);

//            Assert.AreEqual(insertedWorkout.Name, newWorkout.Name);

//            try
//            {
//                WorkoutService.InsertWorkout(newWorkout.Name, newWorkout.WorkoutTypeId);
//                Assert.Fail("Expected exception was not thrown.");
//            }
//            catch (Exception ex)
//            {
//                Assert.AreEqual("An error occurred while inserting workout.", ex.Message);
//            }

//            try
//            {
//                WorkoutService.InsertWorkout("", 1);
//                Assert.Fail("Expected exception wan not thrown");
//            }
//            catch (Exception ex)
//            {
//                Assert.AreEqual("Workout name cannot be empty or null.", ex.Message);
//            }
//        }

//        [TestMethod]
//        public void TestUpdateWorkout()
//        {
//            WorkoutModel existingWorkout = workoutRepository.GetWorkoutById(1);
//            existingWorkout.Name = "Updated Workout A";
//            WorkoutService.UpdateWorkout(existingWorkout);
//            WorkoutModel updatedWorkout = workoutRepository.GetWorkoutById(1);
//            Assert.IsNotNull(updatedWorkout);
//            Assert.AreEqual(updatedWorkout.Name, "Updated Workout A");
//            try
//            {
//                WorkoutService.UpdateWorkout(null);
//                Assert.Fail("Expected exception was not thrown.");
//            }
//            catch (Exception ex)
//            {
//                Assert.Contains("Workout cannot be null.", ex.Message);
//            }

//            try
//            {
//                WorkoutModel invalidWorkout = new WorkoutModel(1, "", 1);
//                WorkoutService.UpdateWorkout(invalidWorkout);
//                Assert.Fail("Expected exception was not thrown.");
//            }
//            catch (Exception ex)
//            {
//                Assert.Contains("Workout name cannot be empty or null.", ex.Message);
//            }

//            try
//            {
//                WorkoutModel duplicateWorkout = new WorkoutModel(1, "Workout B", 1);
//                WorkoutService.UpdateWorkout(duplicateWorkout);
//                Assert.Fail("Expected exception was not thrown.");
//            }
//            catch (Exception ex)
//            {
//                Assert.Contains("Workout with the same name already exists.", ex.Message);
//            }

//            try
//            {
//                WorkoutModel nonExistingWorkout = new WorkoutModel(999, "Non-existing Workout", 1);
//                WorkoutService.UpdateWorkout(nonExistingWorkout);
//                Assert.Fail("Expected exception was not thrown.");
//            }
//            catch (Exception ex)
//            {
//                Assert.Contains("Workout not found", ex.Message);

//            }

//        }
//    }
//}
