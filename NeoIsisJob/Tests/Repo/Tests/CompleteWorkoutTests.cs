//using System;
//using System.Collections.Generic;
//using System.Data;
//using Moq;
//using FluentAssertions;
//using NeoIsisJob.Data.Interfaces; // Import the IDatabaseHelper interface
//using NeoIsisJob.Models;
//using NeoIsisJob.Repositories;
//using Xunit;
//using System.Data.SqlClient;

//namespace Tests.Repo.Tests
//{
//    public class CompleteWorkoutRepoTests
//    {
//        private readonly Mock<IDatabaseHelper> _mockDatabaseHelper; // Mocking the IDatabaseHelper interface
//        private readonly CompleteWorkoutRepo _repo;

//        public CompleteWorkoutRepoTests()
//        {
//            _mockDatabaseHelper = new Mock<IDatabaseHelper>(); // Mock IDatabaseHelper
//            _repo = new CompleteWorkoutRepo(_mockDatabaseHelper.Object); // Inject the mock IDatabaseHelper into the repo
//        }

//        [Fact]
//        public void GetAllCompleteWorkouts_ShouldReturnCompleteWorkoutList()
//        {
//            // Arrange
//            var dataTable = new DataTable();
//            dataTable.Columns.Add("WID", typeof(int));
//            dataTable.Columns.Add("EID", typeof(int));
//            dataTable.Columns.Add("Sets", typeof(int));
//            dataTable.Columns.Add("RepsPerSet", typeof(int));

//            // Add sample data to the DataTable
//            dataTable.Rows.Add(1, 1, 3, 10);
//            dataTable.Rows.Add(2, 2, 4, 12);

//            // Mock the ExecuteReader method to return the DataTable
//            _mockDatabaseHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(dataTable);

//            // Act
//            var result = _repo.GetAllCompleteWorkouts();

//            // Assert
//            result.Should().HaveCount(2); // Ensures we get 2 records
//            result[0].WorkoutId.Should().Be(1);  // Changed to WorkoutId
//            result[0].ExerciseId.Should().Be(1); // Changed to ExerciseId
//            result[0].Sets.Should().Be(3);
//            result[0].RepetitionsPerSet.Should().Be(10);
//        }

//        [Fact]
//        public void DeleteCompleteWorkoutsByWorkoutId_ShouldCallExecuteNonQuery()
//        {
//            // Arrange
//            var workoutId = 1;
//            var sqlParameters = new SqlParameter[]
//            {
//                new SqlParameter("@wid", workoutId)
//            };

//            // Mock the ExecuteNonQuery method to return a result
//            _mockDatabaseHelper.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(1);

//            // Act
//            _repo.DeleteCompleteWorkoutsByWorkoutId(workoutId);

//            // Assert
//            _mockDatabaseHelper.Verify(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
//        }

//        [Fact]
//        public void InsertCompleteWorkout_ShouldCallExecuteNonQuery()
//        {
//            // Arrange
//            var workoutId = 1;
//            var exerciseId = 1;
//            var sets = 3;
//            var repsPerSet = 10;

//            var sqlParameters = new SqlParameter[]
//            {
//                new SqlParameter("@wid", workoutId),
//                new SqlParameter("@eid", exerciseId),
//                new SqlParameter("@sets", sets),
//                new SqlParameter("@repsPerSet", repsPerSet)
//            };

//            // Mock the ExecuteNonQuery method to return a result
//            _mockDatabaseHelper.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                               .Returns(1);

//            // Act
//            _repo.InsertCompleteWorkout(workoutId, exerciseId, sets, repsPerSet);

//            // Assert
//            _mockDatabaseHelper.Verify(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
//        }
//    }
//}
