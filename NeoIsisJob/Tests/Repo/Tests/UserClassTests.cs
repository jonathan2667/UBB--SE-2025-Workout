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
//    public class UserClassRepoTests
//    {
//        private Mock<IDatabaseHelper> _mockDatabaseHelper;
//        private UserClassRepo _userClassRepo;

//        [TestInitialize]
//        public void Setup()
//        {
//            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
//            _userClassRepo = new UserClassRepo(_mockDatabaseHelper.Object);
//        }

//        [TestMethod]
//        public void GetUserClassModelById_ReturnsModel_WhenMatchFound()
//        {
//            // Arrange
//            var mockTable = new DataTable();
//            mockTable.Columns.Add("UID", typeof(int));
//            mockTable.Columns.Add("CID", typeof(int));
//            mockTable.Columns.Add("Date", typeof(DateTime));
//            mockTable.Rows.Add(1, 101, new DateTime(2024, 1, 1));

//            _mockDatabaseHelper
//                .Setup(d => d.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(mockTable);

//            // Act
//            var result = _userClassRepo.GetUserClassModelById(1, 101, new DateTime(2024, 1, 1));

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.UserId);
//            Assert.AreEqual(101, result.ClassId);
//            Assert.AreEqual(new DateTime(2024, 1, 1), result.EnrollmentDate);
//        }

//        [TestMethod]
//        public void GetUserClassModelById_ReturnsEmptyModel_WhenNoMatch()
//        {
//            var table = new DataTable();
//            table.Columns.Add("UID", typeof(int));
//            table.Columns.Add("CID", typeof(int));
//            table.Columns.Add("Date", typeof(DateTime));

//            _mockDatabaseHelper
//                .Setup(d => d.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(table);

//            var result = _userClassRepo.GetUserClassModelById(1, 2, DateTime.Today);

//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.UserId); // default int value
//        }

//        [TestMethod]
//        public void GetAllUserClassModel_ReturnsListOfUserClasses()
//        {
//            var table = new DataTable();
//            table.Columns.Add("UID", typeof(int));
//            table.Columns.Add("CID", typeof(int));
//            table.Columns.Add("Date", typeof(DateTime));
//            table.Rows.Add(1, 201, DateTime.Today);
//            table.Rows.Add(2, 202, DateTime.Today.AddDays(-1));

//            _mockDatabaseHelper
//                .Setup(d => d.ExecuteReader("SELECT UID, CID, Date FROM UserClasses", null))
//                .Returns(table);

//            var result = _userClassRepo.GetAllUserClassModel();

//            Assert.AreEqual(2, result.Count);
//            Assert.AreEqual(1, result[0].UserId);
//        }

//        [TestMethod]
//        public void AddUserClassModel_ExecutesInsertQuery()
//        {
//            var userClass = new UserClassModel(1, 10, DateTime.Today);

//            _mockDatabaseHelper
//                .Setup(d => d.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(1);

//            _userClassRepo.AddUserClassModel(userClass);

//            _mockDatabaseHelper.Verify(d => d.ExecuteNonQuery(It.Is<string>(s => s.Contains("INSERT INTO UserClasses")), It.IsAny<SqlParameter[]>()), Times.Once);
//        }

//        [TestMethod]
//        public void DeleteUserClassModel_ExecutesDeleteQuery()
//        {
//            _mockDatabaseHelper
//                .Setup(d => d.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(1);

//            _userClassRepo.DeleteUserClassModel(1, 10, DateTime.Today);

//            _mockDatabaseHelper.Verify(d => d.ExecuteNonQuery(It.Is<string>(s => s.Contains("DELETE FROM UserClasses")), It.IsAny<SqlParameter[]>()), Times.Once);
//        }

//        [TestMethod]
//        public void GetUserClassModelByDate_ReturnsFilteredList()
//        {
//            var table = new DataTable();
//            table.Columns.Add("UID", typeof(int));
//            table.Columns.Add("CID", typeof(int));
//            table.Columns.Add("Date", typeof(DateTime));
//            table.Rows.Add(1, 201, DateTime.Today);

//            _mockDatabaseHelper
//                .Setup(d => d.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(table);

//            var result = _userClassRepo.GetUserClassModelByDate(DateTime.Today);

//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual(201, result[0].ClassId);
//        }
//    }
//}
