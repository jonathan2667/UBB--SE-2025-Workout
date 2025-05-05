//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Tests.Repo.Mocks;
//using NeoIsisJob.Models; 

//namespace Tests.Repo.Tests
//{
//    [TestClass]
//    public class WorkoutTypeTests
//    {
//        private WorkoutTypeMock _workoutTypeRepository = new WorkoutTypeMock();
//        public WorkoutTypeTests() { }

//        [TestMethod]
//        public void TestGetAllWorkoutTypes()
//        {
//            IList<WorkoutTypeModel> res = _workoutTypeRepository.GetAllWorkoutTypes();
//            Assert.IsNotNull(res);
//            Assert.AreEqual(res.Count, 3);
//        }

//        [TestMethod]
//        public void TestInsertWorkoutTypes()
//        {
//            var newWorkoutType = new WorkoutTypeModel(4, "Workout Type D");
//            _workoutTypeRepository.InsertWorkoutType(newWorkoutType.Name);

//            var insertedWorkoutType = _workoutTypeRepository.GetWorkoutTypeById(newWorkoutType.Id);
//            Assert.IsNotNull(insertedWorkoutType);
//            Assert.AreEqual(insertedWorkoutType.Name, newWorkoutType.Name);
//        }

//        [TestMethod]
//        public void TestDeleteWorkoutType()
//        {
//            var existingWorkoutType = _workoutTypeRepository.GetWorkoutTypeById(1);
//            Assert.IsNotNull(existingWorkoutType);
//            _workoutTypeRepository.DeleteWorkoutType(existingWorkoutType.Id);

//            try
//            {
//                _workoutTypeRepository.DeleteWorkoutType(999);
//                Assert.Fail("Expected exception");
//            }
//            catch (Exception ex)
//            {
//                Assert.AreEqual("Workout type not found.", ex.Message);
//            }
//        }
//    }
//}
