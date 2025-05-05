//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using FluentAssertions;
//using Moq;
//using NeoIsisJob.Data.Interfaces;
//using NeoIsisJob.Models;
//using NeoIsisJob.Repositories;
//using Xunit;

//namespace Tests.Repo.Tests
//{
//    public class PersonalTrainerRepoTests
//    {
//        private readonly Mock<IDatabaseHelper> _mockDatabaseHelper;
//        private readonly PersonalTrainerRepo _repo;

//        public PersonalTrainerRepoTests()
//        {
//            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
//            _repo = new PersonalTrainerRepo(_mockDatabaseHelper.Object);
//        }

//        [Fact]
//        public void GetAllPersonalTrainerModel_ShouldReturnList()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("PTID", typeof(int));
//            dataTable.Columns.Add("LastName", typeof(string));
//            dataTable.Columns.Add("FirstName", typeof(string));
//            dataTable.Columns.Add("WorksSince", typeof(DateTime));

//            dataTable.Rows.Add(1, "Smith", "John", new DateTime(2020, 1, 1));
//            dataTable.Rows.Add(2, "Doe", "Jane", new DateTime(2019, 5, 10));

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), null))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetAllPersonalTrainerModel();

//            // Assert
//            result.Should().HaveCount(2);
//            result[0].Id.Should().Be(1);
//            result[0].LastName.Should().Be("Smith");
//            result[0].FirstName.Should().Be("John");
//            result[1].LastName.Should().Be("Doe");
//        }

//        [Fact]
//        public void GetPersonalTrainerModelById_ShouldReturnTrainer()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("PTID", typeof(int));
//            dataTable.Columns.Add("LastName", typeof(string));
//            dataTable.Columns.Add("FirstName", typeof(string));
//            dataTable.Columns.Add("WorksSince", typeof(DateTime));

//            dataTable.Rows.Add(5, "Miller", "Alice", new DateTime(2021, 6, 1));

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetPersonalTrainerModelById(5);

//            // Assert
//            result.Should().NotBeNull();
//            result!.Id.Should().Be(5);
//            result.LastName.Should().Be("Miller");
//            result.FirstName.Should().Be("Alice");
//        }

//        [Fact]
//        public void GetPersonalTrainerModelById_ShouldReturnNull_WhenNotFound()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("PTID", typeof(int));
//            dataTable.Columns.Add("LastName", typeof(string));
//            dataTable.Columns.Add("FirstName", typeof(string));
//            dataTable.Columns.Add("WorksSince", typeof(DateTime));

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetPersonalTrainerModelById(999);

//            // Assert
//            result.Should().BeNull();
//        }

//        [Fact]
//        public void AddPersonalTrainerModel_ShouldCallExecuteNonQueryOnce()
//        {
//            // Arrange
//            var trainer = new PersonalTrainerModel
//            {
//                LastName = "Brown",
//                FirstName = "Charlie",
//                WorkStartDateTime = new DateTime(2022, 3, 15)
//            };

//            _mockDatabaseHelper.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(1);

//            // Act
//            _repo.AddPersonalTrainerModel(trainer);

//            // Assert
//            _mockDatabaseHelper.Verify(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
//        }

//        [Fact]
//        public void DeletePersonalTrainerModel_ShouldCallExecuteNonQueryOnce()
//        {
//            // Arrange
//            int trainerId = 7;

//            _mockDatabaseHelper.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(1);

//            // Act
//            _repo.DeletePersonalTrainerModel(trainerId);

//            // Assert
//            _mockDatabaseHelper.Verify(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
//        }
//    }
//}
