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
//    public class ClassTypeRepoTests
//    {
//        private readonly Mock<IDatabaseHelper> _mockDatabaseHelper;
//        private readonly ClassTypeRepository _repo;

//        public ClassTypeRepoTests()
//        {
//            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
//            _repo = new ClassTypeRepository(_mockDatabaseHelper.Object);
//        }

//        [Fact]
//        public void GetAllClassTypeModel_ShouldReturnList()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("CTID", typeof(int));
//            dataTable.Columns.Add("Name", typeof(string));

//            dataTable.Rows.Add(1, "Cardio");
//            dataTable.Rows.Add(2, "Strength");

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), null))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetAllClassTypeModel();

//            // Assert
//            result.Should().HaveCount(2);
//            result[0].Id.Should().Be(1);
//            result[0].Name.Should().Be("Cardio");
//            result[1].Name.Should().Be("Strength");
//        }

//        [Fact]
//        public void GetClassTypeModelById_ShouldReturnCorrectType()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("CTID", typeof(int));
//            dataTable.Columns.Add("Name", typeof(string));

//            dataTable.Rows.Add(1, "Cardio");

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetClassTypeModelById(1);

//            // Assert
//            result.Should().NotBeNull();
//            result!.Id.Should().Be(1);
//            result.Name.Should().Be("Cardio");
//        }

//        [Fact]
//        public void GetClassTypeModelById_ShouldReturnNull_WhenNotFound()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("CTID", typeof(int));
//            dataTable.Columns.Add("Name", typeof(string));

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetClassTypeModelById(999);

//            // Assert
//            result.Should().BeNull();
//        }

//        [Fact]
//        public void AddClassTypeModel_ShouldCallExecuteNonQueryOnce()
//        {
//            // Arrange
//            var classType = new ClassTypeModel
//            {
//                Name = "Mobility"
//            };

//            _mockDatabaseHelper.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(1);

//            // Act
//            _repo.AddClassTypeModel(classType);

//            // Assert
//            _mockDatabaseHelper.Verify(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
//        }

//        [Fact]
//        public void DeleteClassTypeModel_ShouldCallExecuteNonQueryOnce()
//        {
//            // Arrange
//            int classTypeId = 2;

//            _mockDatabaseHelper.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(1);

//            // Act
//            _repo.DeleteClassTypeModel(classTypeId);

//            // Assert
//            _mockDatabaseHelper.Verify(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
//        }
//    }
//}
