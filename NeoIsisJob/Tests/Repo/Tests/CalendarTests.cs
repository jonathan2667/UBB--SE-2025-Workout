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
//    public class CalendarRepositoryTests
//    {
//        private readonly Mock<IDatabaseHelper> _mockDbHelper;
//        private readonly CalendarRepository _repo;

//        public CalendarRepositoryTests()
//        {
//            _mockDbHelper = new Mock<IDatabaseHelper>();
//            _repo = new CalendarRepository(_mockDbHelper.Object);
//        }

//        [Fact]
//        public void GetCalendarDaysForMonth_ShouldReturnDaysWithWorkoutsAndClasses()
//        {
//            // Arrange
//            int userId = 1;
//            var targetMonth = new DateTime(2024, 4, 1);
//            var workoutTable = new DataTable();
//            workoutTable.Columns.Add("Date", typeof(DateTime));
//            workoutTable.Columns.Add("Completed", typeof(bool));
//            workoutTable.Rows.Add(new DateTime(2024, 4, 10), true);
//            workoutTable.Rows.Add(new DateTime(2024, 4, 15), false);

//            var classTable = new DataTable();
//            classTable.Columns.Add("Date", typeof(DateTime));
//            classTable.Rows.Add(new DateTime(2024, 4, 15));

//            _mockDbHelper.SetupSequence(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(workoutTable)
//                         .Returns(classTable);

//            // Act
//            var result = _repo.GetCalendarDaysForMonth(userId, targetMonth);

//            // Assert
//            result.Should().HaveCount(30);
//            var day10 = result.Find(d => d.DayNumber == 10);
//            var day15 = result.Find(d => d.DayNumber == 15);

//            day10!.HasWorkout.Should().BeTrue();
//            day10.IsWorkoutCompleted.Should().BeTrue();
//            day10.HasClass.Should().BeFalse();

//            day15!.HasWorkout.Should().BeTrue();
//            day15.IsWorkoutCompleted.Should().BeFalse();
//            day15.HasClass.Should().BeTrue();
//        }

//        [Fact]
//        public void GetUserWorkout_ShouldReturnWorkout_WhenExists()
//        {
//            // Arrange
//            int userId = 1;
//            DateTime date = new DateTime(2024, 4, 10);

//            var workoutTable = new DataTable();
//            workoutTable.Columns.Add("WID", typeof(int));
//            workoutTable.Columns.Add("Completed", typeof(bool));
//            workoutTable.Rows.Add(5, true);

//            _mockDbHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(workoutTable);

//            // Act
//            var result = _repo.GetUserWorkout(userId, date);

//            // Assert
//            result.Should().NotBeNull();
//            result!.WorkoutId.Should().Be(5);
//            result.Completed.Should().BeTrue();
//        }

//        [Fact]
//        public void GetUserWorkout_ShouldReturnNull_WhenNoWorkoutExists()
//        {
//            // Arrange
//            var emptyTable = new DataTable();
//            emptyTable.Columns.Add("WID", typeof(int));
//            emptyTable.Columns.Add("Completed", typeof(bool));

//            _mockDbHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(emptyTable);

//            // Act
//            var result = _repo.GetUserWorkout(1, DateTime.Today);

//            // Assert
//            result.Should().BeNull();
//        }

//        [Fact]
//        public void GetUserClass_ShouldReturnClassName_WhenExists()
//        {
//            // Arrange
//            var classIdTable = new DataTable();
//            classIdTable.Columns.Add("CID", typeof(int));
//            classIdTable.Rows.Add(3);

//            var classNameTable = new DataTable();
//            classNameTable.Columns.Add("Name", typeof(string));
//            classNameTable.Rows.Add("Yoga");

//            _mockDbHelper.SetupSequence(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(classIdTable)
//                         .Returns(classNameTable);

//            // Act
//            var result = _repo.GetUserClass(1, DateTime.Today);

//            // Assert
//            result.Should().Be("Yoga");
//        }

//        [Fact]
//        public void GetUserClass_ShouldReturnNull_WhenNoClassExists()
//        {
//            // Arrange
//            var emptyTable = new DataTable();
//            emptyTable.Columns.Add("CID", typeof(int));

//            _mockDbHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(emptyTable);

//            // Act
//            var result = _repo.GetUserClass(1, DateTime.Today);

//            // Assert
//            result.Should().BeNull();
//        }

//        [Fact]
//        public void GetWorkouts_ShouldReturnAllWorkouts()
//        {
//            // Arrange
//            var workoutTable = new DataTable();
//            workoutTable.Columns.Add("WID", typeof(int));
//            workoutTable.Columns.Add("Name", typeof(string));
//            workoutTable.Rows.Add(1, "Push-ups");
//            workoutTable.Rows.Add(2, "Squats");

//            _mockDbHelper.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
//                         .Returns(workoutTable);

//            // Act
//            var result = _repo.GetWorkouts();

//            // Assert
//            result.Should().HaveCount(2);
//            result[0].Id.Should().Be(1);
//            result[0].Name.Should().Be("Push-ups");
//            result[1].Id.Should().Be(2);
//            result[1].Name.Should().Be("Squats");
//        }
//    }
//}
