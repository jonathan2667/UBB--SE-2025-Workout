using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Workout.Tests.Services
{
    public class WorkoutServiceTests
    {
        private readonly Mock<IWorkoutRepository> workoutRepoMock;
        private readonly WorkoutService workoutService;

        public WorkoutServiceTests()
        {
            workoutRepoMock = new Mock<IWorkoutRepository>();
            workoutService = new WorkoutService(workoutRepoMock.Object);
        }

        [Fact]
        public async Task GetWorkoutAsync_ReturnsWorkout_WhenIdIsValid()
        {
            // Arrange
            int workoutId = 1;
            var expectedWorkout = new WorkoutModel { WID = workoutId };
            workoutRepoMock
                .Setup(repo => repo.GetWorkoutByIdAsync(workoutId))
                .ReturnsAsync(expectedWorkout);

            // Act
            var result = await workoutService.GetWorkoutAsync(workoutId);

            // Assert
            Assert.Equal(workoutId, result.WID);
        }

        [Fact]
        public async Task GetWorkoutByNameAsync_ReturnsWorkout()
        {
            // Arrange
            string workoutName = "Leg Day";
            var expectedWorkout = new WorkoutModel { Name = workoutName };
            workoutRepoMock
                .Setup(repo => repo.GetWorkoutByNameAsync(workoutName))
                .ReturnsAsync(expectedWorkout);

            // Act
            var result = await workoutService.GetWorkoutByNameAsync(workoutName);

            // Assert
            Assert.Equal(workoutName, result.Name);
        }

        [Fact]
        public async Task InsertWorkoutAsync_ThrowsArgumentException_WhenNameIsEmpty()
        {
            // Arrange
            string invalidName = " ";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                workoutService.InsertWorkoutAsync(invalidName, 1));
        }

        [Fact]
        public async Task InsertWorkoutAsync_ThrowsCustomException_OnDuplicateName()
        {
            // Arrange
            string workoutName = "Push";
            workoutRepoMock
                .Setup(repo => repo.InsertWorkoutAsync(workoutName, 1))
                .ThrowsAsync(new Exception("Violation of UNIQUE KEY"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                workoutService.InsertWorkoutAsync(workoutName, 1));

            Assert.Contains("An error occurred while inserting workout", ex.Message);
        }

        [Fact]
        public async Task InsertWorkout_WithDescription_ThrowsOnDuplicateName()
        {
            // Arrange
            workoutRepoMock
                .Setup(r => r.InsertWorkoutAsync("Push", 1, "desc"))
                .ThrowsAsync(new Exception("Violation of UNIQUE KEY"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                workoutService.InsertWorkoutAsync("Push", 1, "desc"));

            Assert.Contains("An error occurred while inserting workout", ex.Message);
        }

        [Fact]
        public async Task DeleteWorkoutAsync_CallsRepository()
        {
            // Arrange
            int workoutId = 5;

            // Act
            await workoutService.DeleteWorkoutAsync(workoutId);

            // Assert
            workoutRepoMock.Verify(r => r.DeleteWorkoutAsync(workoutId), Times.Once);
        }

        [Fact]
        public async Task UpdateWorkoutAsync_ThrowsIfWorkoutIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                workoutService.UpdateWorkoutAsync(null));
        }

        [Fact]
        public async Task UpdateWorkoutAsync_ThrowsIfWorkoutNameIsEmpty()
        {
            // Arrange
            var invalidWorkout = new WorkoutModel { Name = " " };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                workoutService.UpdateWorkoutAsync(invalidWorkout));
        }

        [Fact]
        public async Task UpdateWorkoutAsync_ThrowsIfDuplicateName()
        {
            // Arrange
            var model = new WorkoutModel { Name = "Duplicate" };
            workoutRepoMock
                .Setup(repo => repo.UpdateWorkoutAsync(model))
                .ThrowsAsync(new Exception("A workout with this name already exists"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                workoutService.UpdateWorkoutAsync(model));

            Assert.Contains("already exists", ex.Message);
        }

        [Fact]
        public async Task GetAllWorkoutsAsync_ReturnsList()
        {
            // Arrange
            var expectedList = new List<WorkoutModel>
            {
                new WorkoutModel { WID = 1 },
                new WorkoutModel { WID = 2 },
            };

            workoutRepoMock
                .Setup(repo => repo.GetAllWorkoutsAsync())
                .ReturnsAsync(expectedList);

            // Act
            var result = await workoutService.GetAllWorkoutsAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}
