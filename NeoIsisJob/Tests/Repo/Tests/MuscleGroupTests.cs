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
//    public class MuscleGroupRepoTests
//    {
//        private readonly Mock<IDatabaseHelper> _mockDatabaseHelper;
//        private readonly MuscleGroupRepo _repo;

//        public MuscleGroupRepoTests()
//        {
//            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
//            _repo = new MuscleGroupRepo(_mockDatabaseHelper.Object);
//        }

//        [Fact]
//        public void GetAllMuscleGroups_ShouldReturnList()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("MGID", typeof(int));
//            dataTable.Columns.Add("Name", typeof(string));

//            dataTable.Rows.Add(1, "Chest");
//            dataTable.Rows.Add(2, "Back");

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), null))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetAllMuscleGroups();

//            // Assert
//            result.Should().HaveCount(2);
//            result[0].Id.Should().Be(1);
//            result[0].Name.Should().Be("Chest");
//            result[1].Name.Should().Be("Back");
//        }

//        [Fact]
//        public void GetMuscleGroupById_ShouldReturnCorrectGroup()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("MGID", typeof(int));
//            dataTable.Columns.Add("Name", typeof(string));

//            dataTable.Rows.Add(1, "Legs");

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetMuscleGroupById(1);

//            // Assert
//            result.Should().NotBeNull();
//            result!.Id.Should().Be(1);
//            result.Name.Should().Be("Legs");
//        }

//        [Fact]
//        public void GetMuscleGroupById_ShouldReturnNull_WhenNotFound()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("MGID", typeof(int));
//            dataTable.Columns.Add("Name", typeof(string));

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetMuscleGroupById(999);

//            // Assert
//            result.Should().BeNull();
//        }

//        [Fact]
//        public void AddMuscleGroup_ShouldCallExecuteNonQueryOnce()
//        {
//            // Arrange
//            var muscleGroup = new MuscleGroupModel
//            {
//                Name = "Shoulders"
//            };

//            _mockDatabaseHelper.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(1);

//            // Act
//            _repo.AddMuscleGroup(muscleGroup);

//            // Assert
//            _mockDatabaseHelper.Verify(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
//        }

//        [Fact]
//        public void DeleteMuscleGroup_ShouldCallExecuteNonQueryOnce()
//        {
//            // Arrange
//            int muscleGroupId = 3;

//            _mockDatabaseHelper.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(1);

//            // Act
//            _repo.DeleteMuscleGroup(muscleGroupId);

//            // Assert
//            _mockDatabaseHelper.Verify(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
//        }
//    }
//}
