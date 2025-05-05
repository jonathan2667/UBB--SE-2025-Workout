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
//    public class UserRepoTests
//    {
//        private Mock<IDatabaseHelper> _mockDatabaseHelper;
//        private UserRepo _userRepo;

//        [TestInitialize]
//        public void Setup()
//        {
//            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
//            _userRepo = new UserRepo(_mockDatabaseHelper.Object);
//        }

//        [TestMethod]
//        public void GetUserById_ReturnsUser_WhenUserExists()
//        {
//            // Arrange
//            var userId = 1;
//            var table = new DataTable();
//            table.Columns.Add("UID", typeof(int));
//            table.Rows.Add(userId);

//            _mockDatabaseHelper.Setup(d => d.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(table);

//            // Act
//            var result = _userRepo.GetUserById(userId);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(userId, result.Id);
//        }

//        [TestMethod]
//        public void GetUserById_ReturnsEmptyUser_WhenUserNotFound()
//        {
//            // Arrange
//            var table = new DataTable();
//            table.Columns.Add("UID", typeof(int));

//            _mockDatabaseHelper.Setup(d => d.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(table);

//            // Act
//            var result = _userRepo.GetUserById(999);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Id); // assuming 0 is default in your UserModel
//        }

//        [TestMethod]
//        public void InsertUser_ReturnsNewUserId()
//        {
//            // Arrange
//            int expectedId = 5;
//            _mockDatabaseHelper.Setup(d => d.ExecuteScalar<int>(It.IsAny<string>(), null))
//                .Returns(expectedId);

//            // Act
//            var result = _userRepo.InsertUser();

//            // Assert
//            Assert.AreEqual(expectedId, result);
//        }

//        [TestMethod]
//        public void DeleteUserById_ReturnsTrue_WhenUserDeleted()
//        {
//            // Arrange
//            _mockDatabaseHelper.Setup(d => d.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(1);

//            // Act
//            var result = _userRepo.DeleteUserById(1);

//            // Assert
//            Assert.IsTrue(result);
//        }

//        [TestMethod]
//        public void DeleteUserById_ReturnsFalse_WhenNoRowsAffected()
//        {
//            // Arrange
//            _mockDatabaseHelper.Setup(d => d.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(0);

//            // Act
//            var result = _userRepo.DeleteUserById(99);

//            // Assert
//            Assert.IsFalse(result);
//        }

//        [TestMethod]
//        public void GetAllUsers_ReturnsListOfUsers()
//        {
//            // Arrange
//            var table = new DataTable();
//            table.Columns.Add("UID", typeof(int));
//            table.Rows.Add(1);
//            table.Rows.Add(2);

//            _mockDatabaseHelper.Setup(d => d.ExecuteReader(It.IsAny<string>(), null))
//                .Returns(table);

//            // Act
//            var result = _userRepo.GetAllUsers();

//            // Assert
//            Assert.AreEqual(2, result.Count);
//            Assert.AreEqual(1, result[0].Id);
//            Assert.AreEqual(2, result[1].Id);
//        }
//    }
//}
