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
//    public class ExerciseRepoTests
//    {
//        private readonly Mock<IDatabaseHelper> _mockDatabaseHelper;
//        private readonly ExerciseRepo _repo;

//        public ExerciseRepoTests()
//        {
//            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
//            _repo = new ExerciseRepo(_mockDatabaseHelper.Object);
//        }

//        [Fact]
//        public void GetAllExercises_ShouldReturnListOfExercises()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("EID", typeof(int));
//            dataTable.Columns.Add("Name", typeof(string));
//            dataTable.Columns.Add("Description", typeof(string));
//            dataTable.Columns.Add("Difficulty", typeof(int));
//            dataTable.Columns.Add("MGID", typeof(int));

//            dataTable.Rows.Add(1, "Push-up", "Chest exercise", 2, 1);
//            dataTable.Rows.Add(2, "Squat", "Leg exercise", 3, 2);

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetAllExercises();

//            // Assert
//            result.Should().HaveCount(2);
//            result[0].Name.Should().Be("Push-up");
//            result[0].Description.Should().Be("Chest exercise");
//            result[0].Difficulty.Should().Be(2);
//            result[0].MuscleGroupId.Should().Be(1);

//            result[1].Name.Should().Be("Squat");
//            result[1].Description.Should().Be("Leg exercise");
//            result[1].Difficulty.Should().Be(3);
//            result[1].MuscleGroupId.Should().Be(2);
//        }

//        [Fact]
//        public void GetExerciseById_ShouldReturnCorrectExercise_WhenFound()
//        {
//            // Arrange
//            var exerciseId = 1;
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("EID", typeof(int));
//            dataTable.Columns.Add("Name", typeof(string));
//            dataTable.Columns.Add("Description", typeof(string));
//            dataTable.Columns.Add("Difficulty", typeof(int));
//            dataTable.Columns.Add("MGID", typeof(int));

//            dataTable.Rows.Add(1, "Push-up", "Chest exercise", 2, 1);

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetExerciseById(exerciseId);

//            // Assert
//            result.Should().NotBeNull();
//            result.Name.Should().Be("Push-up");
//            result.Description.Should().Be("Chest exercise");
//            result.Difficulty.Should().Be(2);
//            result.MuscleGroupId.Should().Be(1);
//        }

//        [Fact]
//        public void GetExerciseById_ShouldReturnNull_WhenNotFound()
//        {
//            // Arrange
//            var exerciseId = 999;
//            var emptyTable = new DataTable();
//            emptyTable.Columns.Add("EID", typeof(int));
//            emptyTable.Columns.Add("Name", typeof(string));
//            emptyTable.Columns.Add("Description", typeof(string));
//            emptyTable.Columns.Add("Difficulty", typeof(int));
//            emptyTable.Columns.Add("MGID", typeof(int));

//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(emptyTable);

//            // Act
//            var result = _repo.GetExerciseById(exerciseId);

//            // Assert
//            result.Should().BeNull();
//        }
//    }
//}
