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
//    public class ClassRepoTests
//    {
//        private readonly Mock<IDatabaseHelper> _mockDatabaseHelper;
//        private readonly ClassRepository _repo;

//        public ClassRepoTests()
//        {
//            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
//            _repo = new ClassRepository(_mockDatabaseHelper.Object);
//        }

//        [Fact]
//        public void GetAllClassModel_ShouldReturnClassList()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("CID", typeof(int));
//            dataTable.Columns.Add("Name", typeof(string));
//            dataTable.Columns.Add("Description", typeof(string));
//            dataTable.Columns.Add("CTID", typeof(int));
//            dataTable.Columns.Add("PTID", typeof(int));

//            dataTable.Rows.Add(1, "Yoga", "Stretching class", 2, 5);
//            dataTable.Rows.Add(2, "HIIT", "High-intensity training", 3, 6);

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), null))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetAllClassModel();

//            // Assert
//            result.Should().HaveCount(2);
//            result[0].Id.Should().Be(1);
//            result[0].Name.Should().Be("Yoga");
//            result[1].Name.Should().Be("HIIT");
//        }

//        [Fact]
//        public void GetClassModelById_ShouldReturnCorrectClass()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("CID", typeof(int));
//            dataTable.Columns.Add("Name", typeof(string));
//            dataTable.Columns.Add("Description", typeof(string));
//            dataTable.Columns.Add("CTID", typeof(int));
//            dataTable.Columns.Add("PTID", typeof(int));

//            dataTable.Rows.Add(1, "Yoga", "Stretching class", 2, 5);

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetClassModelById(1);

//            // Assert
//            result.Should().NotBeNull();
//            result!.Id.Should().Be(1);
//            result.Name.Should().Be("Yoga");
//            result.Description.Should().Be("Stretching class");
//        }

//        [Fact]
//        public void GetClassModelById_ShouldReturnNull_WhenNoData()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("CID", typeof(int));
//            dataTable.Columns.Add("Name", typeof(string));
//            dataTable.Columns.Add("Description", typeof(string));
//            dataTable.Columns.Add("CTID", typeof(int));
//            dataTable.Columns.Add("PTID", typeof(int));

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetClassModelById(999);

//            // Assert
//            result.Should().BeNull();
//        }

//        [Fact]
//        public void AddClassModel_ShouldCallExecuteNonQueryOnce()
//        {
//            // Arrange
//            var classModel = new ClassModel
//            {
//                Name = "Pilates",
//                Description = "Core strength training",
//                ClassTypeId = 1,
//                PersonalTrainerId = 4
//            };

//            _mockDatabaseHelper.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(1);

//            // Act
//            _repo.AddClassModel(classModel);

//            // Assert
//            _mockDatabaseHelper.Verify(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
//        }

//        [Fact]
//        public void DeleteClassModel_ShouldCallExecuteNonQueryOnce()
//        {
//            // Arrange
//            int classId = 3;

//            _mockDatabaseHelper.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(1);

//            // Act
//            _repo.DeleteClassModel(classId);

//            // Assert
//            _mockDatabaseHelper.Verify(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
//        }
//    }
//}
