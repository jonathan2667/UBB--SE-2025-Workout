//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using NeoIsisJob.Data.Interfaces;
//using NeoIsisJob.Models;
//using NeoIsisJob.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;

//namespace Tests.Repo.Tests
//{
//    [TestClass]
//    public class WorkoutRepoTests
//    {
//        private Mock<IDatabaseHelper> _mockDbHelper;
//        private WorkoutRepo _workoutRepo;

//        [TestInitialize]
//        public void Setup()
//        {
//            _mockDbHelper = new Mock<IDatabaseHelper>();
//            _workoutRepo = new WorkoutRepo(_mockDbHelper.Object);
//        }

//        [TestMethod]
//        public void GetWorkoutById_ShouldReturnWorkoutModel_WhenWorkoutExists()
//        {
//            // Arrange
//            var expected = new WorkoutModel(1, "Push Ups", 2);

//            _workoutRepo.InsertWorkout(expected.Name, expected.WorkoutTypeId);

//            var table = new DataTable();
//            table.Columns.Add("WID", typeof(int));
//            table.Columns.Add("Name", typeof(string));
//            table.Columns.Add("WTID", typeof(int));
//            table.Rows.Add(1, "Push Ups", 2);

            

//            _mockDbHelper
//                .Setup(db => db.ExecuteReader(
//                    It.IsAny<string>(),
//                    It.IsAny<SqlParameter[]>()))
//                .Returns(table);

//            // Act
//            var result = _workoutRepo.GetWorkoutById(1);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(expected.Id, result.Id);
//            Assert.AreEqual(expected.Name, result.Name);
//            Assert.AreEqual(expected.WorkoutTypeId, result.WorkoutTypeId);
//        }


//        [TestMethod]
//        public void GetWorkoutByName_ShouldReturnWorkoutModel_WhenWorkoutExists()
//        {
//            var expected = new WorkoutModel(1, "Squats", 3);

//            var table = new DataTable();
//            table.Columns.Add("WID", typeof(int));
//            table.Columns.Add("Name", typeof(string));
//            table.Columns.Add("WTID", typeof(int));
//            table.Rows.Add(1, "Squats", 3);

//            _mockDbHelper.Setup(db => db.ExecuteReader(
//                It.IsAny<string>(),
//                It.Is<SqlParameter[]>(p => (string)p[0].Value == "Squats")
//                )).Returns(table);


//            var result = _workoutRepo.GetWorkoutByName("Squats");

//            Assert.IsNotNull(result);
//            Assert.AreEqual(expected.Name, result.Name);
//        }

//        [TestMethod]
//        public void InsertWorkout_ShouldCallExecuteNonQuery()
//        {
//            _workoutRepo.InsertWorkout("Deadlifts", 5);

//            _mockDbHelper.Verify(db => db.ExecuteNonQuery(
//               It.IsAny<string>(),
//                It.Is<SqlParameter[]>(p =>
//                    (string)p[0].Value == "Deadlifts" &&
//                    (int)p[1].Value == 5
//                )), Times.Once);
//        }

//        [TestMethod]
//        public void DeleteWorkout_ShouldCallExecuteNonQuery()
//        {
//            _workoutRepo.DeleteWorkout(1);

//            _mockDbHelper.Setup(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(1);

//            _mockDbHelper.Setup(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(1);
//        }

//        [TestMethod]
//        public void UpdateWorkout_ShouldCallExecuteNonQuery_WhenValid()
//        {
//            var workout = new WorkoutModel(3, "Lunges", 1);

//            _mockDbHelper.Setup(db => db.ExecuteScalar<int>(
//                "SELECT COUNT(*) FROM Workouts WHERE Name = @Name AND WID != @Id",
//                It.Is<SqlParameter[]>(p => (string)p[0].Value == "Lunges" && (int)p[1].Value == 3)))
//                .Returns(0);

//            _mockDbHelper.Setup(db => db.ExecuteNonQuery(
//                "UPDATE Workouts SET Name = @Name WHERE WID = @Id",
//                It.Is<SqlParameter[]>(p => (string)p[0].Value == "Lunges" && (int)p[1].Value == 3)))
//                .Returns(1);

//            _workoutRepo.UpdateWorkout(workout);

//            _mockDbHelper.VerifyAll();
//        }

//        [TestMethod]
//        [ExpectedException(typeof(Exception), "A workout with this name already exists.")]
//        public void UpdateWorkout_ShouldThrowException_WhenDuplicateNameExists()
//        {
//            var workout = new WorkoutModel(3, "Plank", 1);

//            _mockDbHelper.Setup(db => db.ExecuteScalar<int>(
//                "SELECT COUNT(*) FROM Workouts WHERE Name = @Name AND WID != @Id",
//                It.IsAny<SqlParameter[]>()))
//                .Returns(1);

//            _workoutRepo.UpdateWorkout(workout);
//        }

//        [TestMethod]
//        public void GetAllWorkouts_ShouldReturnList()
//        {
//            var expectedList = new List<WorkoutModel>
//            {
//                new WorkoutModel(1, "Sit Ups", 1),
//                new WorkoutModel(2, "Burpees", 2)
//            };

//            var table = new DataTable();
//            table.Columns.Add("WID", typeof(int));
//            table.Columns.Add("Name", typeof(string));
//            table.Columns.Add("WTID", typeof(int));
//            table.Rows.Add(1, "Sit Ups", 1);
//            table.Rows.Add(2, "Burpees", 2);

//            _mockDbHelper.Setup(db => db.ExecuteReader(
//               "SELECT * FROM Workouts",
//               It.IsAny<SqlParameter[]>() // Corrected the second argument to match the method signature
//           )).Returns(table);

//            var result = _workoutRepo.GetAllWorkouts();

//            Assert.IsNotNull(result);
//            Assert.AreEqual(expectedList.Count, result.Count);
//            for (int i = 0; i < expectedList.Count; i++)
//            {
//                Assert.AreEqual(expectedList[i].Id, result[i].Id);
//                Assert.AreEqual(expectedList[i].Name, result[i].Name);
//                Assert.AreEqual(expectedList[i].WorkoutTypeId, result[i].WorkoutTypeId);
//            }

//        }
//    }
//}
