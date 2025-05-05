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
//    public class UserWorkoutRepoTests
//    {
//        private Mock<IDatabaseHelper> _mockDbHelper;
//        private UserWorkoutRepo _repo;

//        [TestInitialize]
//        public void Setup()
//        {
//            _mockDbHelper = new Mock<IDatabaseHelper>();
//            _repo = new UserWorkoutRepo(_mockDbHelper.Object);
//        }

//        [TestMethod]
//        public void GetUserWorkoutModelByDate_ReturnsUserWorkouts()
//        {
//            // Arrange
//            var date = new DateTime(2025, 4, 10);
//            var table = new DataTable();
//            table.Columns.Add("UID", typeof(int));
//            table.Columns.Add("WID", typeof(int));
//            table.Columns.Add("Date", typeof(DateTime));
//            table.Columns.Add("Completed", typeof(bool));

//            table.Rows.Add(1, 101, date, true);
//            table.Rows.Add(2, 102, date, false);

//            _mockDbHelper.Setup(x => x.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(table);

//            // Act
//            var result = _repo.GetUserWorkoutModelByDate(date);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//            Assert.AreEqual(1, result[0].UserId);
//            Assert.AreEqual(101, result[0].WorkoutId);
//        }

//        [TestMethod]
//        public void GetUserWorkoutModel_ReturnsCorrectWorkout()
//        {
//            // Arrange
//            var date = new DateTime(2025, 4, 10);
//            var table = new DataTable();
//            table.Columns.Add("UID", typeof(int));
//            table.Columns.Add("WID", typeof(int));
//            table.Columns.Add("Date", typeof(DateTime));
//            table.Columns.Add("Completed", typeof(bool));

//            table.Rows.Add(1, 101, date, true);

//            _mockDbHelper.Setup(x => x.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(table);

//            // Act
//            var result = _repo.GetUserWorkoutModel(1, 101, date);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.UserId);
//            Assert.IsTrue(result.Completed);
//        }

//        [TestMethod]
//        public void AddUserWorkout_ExecutesWithoutError()
//        {
//            // Arrange
//            var workout = new UserWorkoutModel(1, 101, DateTime.Today, true);
//            _mockDbHelper.Setup(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(1);

//            // Act + Assert
//            _repo.AddUserWorkout(workout);
//        }

//        [TestMethod]
//        public void UpdateUserWorkout_ExecutesSuccessfully()
//        {
//            // Arrange
//            var workout = new UserWorkoutModel(1, 101, DateTime.Today, false);
//            _mockDbHelper.Setup(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(1);

//            // Act
//            _repo.UpdateUserWorkout(workout);
//        }

//        [TestMethod]
//        public void DeleteUserWorkout_ExecutesSuccessfully()
//        {
//            // Arrange
//            var date = DateTime.Today;
//            _mockDbHelper.Setup(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(1);

//            // Act
//            _repo.DeleteUserWorkout(1, 101, date);

//            // Assert
//            _mockDbHelper.Verify(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);


//        }
//    }
//}
