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
//    public class RankingsTests
//    {
//        private Mock<IDatabaseHelper> _mockDatabaseHelper;
//        private RankingsRepository _rankingRepository;

//        [TestInitialize]
//        public void Setup()
//        {
//            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
//            _rankingRepository = new RankingsRepository(_mockDatabaseHelper.Object);
//        }

//        [TestMethod]
//        public void GetRankingByFullID_ShouldReturnRankingModel_WhenDataExists()
//        {
//            // Arrange
//            int userId = 1;
//            int mgid = 2;

//            var mockTable = new DataTable();
//            mockTable.Columns.Add("UID", typeof(int));
//            mockTable.Columns.Add("MGID", typeof(int));
//            mockTable.Columns.Add("Rank", typeof(int));
//            mockTable.Rows.Add(userId, mgid, 5);

//            _mockDatabaseHelper
//                .Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(mockTable);

//            // Act
//            var result = _rankingRepository.GetRankingByFullID(userId, mgid);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(userId, result.UserId);
//            Assert.AreEqual(mgid, result.MuscleGroupId);
//            Assert.AreEqual(5, result.Rank);
//        }

//        [TestMethod]
//        public void GetRankingByFullID_ShouldReturnNull_WhenNoDataFound()
//        {
//            // Arrange
//            var emptyTable = new DataTable();
//            emptyTable.Columns.Add("UID", typeof(int));
//            emptyTable.Columns.Add("MGID", typeof(int));
//            emptyTable.Columns.Add("Rank", typeof(int));

//            _mockDatabaseHelper
//                .Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(emptyTable);

//            // Act
//            var result = _rankingRepository.GetRankingByFullID(1, 2);

//            // Assert
//            Assert.IsNull(result);
//        }

//        [TestMethod]
//        public void GetAllRankingsByUserID_ShouldReturnListOfRankings()
//        {
//            // Arrange
//            var mockTable = new DataTable();
//            mockTable.Columns.Add("UID", typeof(int));
//            mockTable.Columns.Add("MGID", typeof(int));
//            mockTable.Columns.Add("Rank", typeof(int));
//            mockTable.Rows.Add(1, 1, 10);
//            mockTable.Rows.Add(1, 2, 20);

//            _mockDatabaseHelper
//                .Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                .Returns(mockTable);

//            // Act
//            var results = _rankingRepository.GetAllRankingsByUserID(1);

//            // Assert
//            Assert.AreEqual(2, results.Count);
//            Assert.AreEqual(10, results[0].Rank);
//            Assert.AreEqual(20, results[1].Rank);
//        }
//    }
//}
