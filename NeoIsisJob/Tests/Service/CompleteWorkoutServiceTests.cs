using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Workout.Tests.Services
{
    public class CompleteWorkoutServiceTests
    {
        private readonly Mock<ICompleteWorkoutRepository> completeWorkoutRepoMock;
        private readonly CompleteWorkoutService completeWorkoutService;

        public CompleteWorkoutServiceTests()
        {
            completeWorkoutRepoMock = new Mock<ICompleteWorkoutRepository>();
            completeWorkoutService = new CompleteWorkoutService(completeWorkoutRepoMock.Object);
        }

        [Fact]
        public async Task GetAllCompleteWorkoutsAsync_ReturnsAllWorkouts()
        {
            // Arrange
            var completeWorkouts = new List<CompleteWorkoutModel>
            {
                new CompleteWorkoutModel { WID = 1, EID = 2 },
                new CompleteWorkoutModel { WID = 2, EID = 3 }
            };

            completeWorkoutRepoMock.Setup(repo => repo.GetAllCompleteWorkoutsAsync())
                                    .ReturnsAsync(completeWorkouts);

            // Act
            var result = await completeWorkoutService.GetAllCompleteWorkoutsAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, w => w.WID == 1 && w.EID == 2);
        }

        [Fact]
        public async Task GetCompleteWorkoutsByWorkoutIdAsync_ReturnsFilteredWorkouts()
        {
            // Arrange
            var workoutId = 5;
            var completeWorkouts = new List<CompleteWorkoutModel>
            {
                new CompleteWorkoutModel { WID = 5, EID = 1 },
                new CompleteWorkoutModel { WID = 5, EID = 2 },
                new CompleteWorkoutModel { WID = 3, EID = 4 }
            };

            completeWorkoutRepoMock.Setup(repo => repo.GetAllCompleteWorkoutsAsync())
                                    .ReturnsAsync(completeWorkouts);

            // Act
            var result = await completeWorkoutService.GetCompleteWorkoutsByWorkoutIdAsync(workoutId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, w => Assert.Equal(workoutId, w.WID));
        }

        [Fact]
        public async Task GetCompleteWorkoutsByWorkoutIdAsync_Throws_WhenWorkoutIdInvalid()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                completeWorkoutService.GetCompleteWorkoutsByWorkoutIdAsync(0));
        }

        [Fact]
        public async Task DeleteCompleteWorkoutsByWorkoutIdAsync_CallsRepository()
        {
            // Arrange
            var workoutId = 3;

            completeWorkoutRepoMock.Setup(repo => repo.DeleteCompleteWorkoutsByWorkoutIdAsync(workoutId))
                                    .Returns(Task.CompletedTask);

            // Act
            await completeWorkoutService.DeleteCompleteWorkoutsByWorkoutIdAsync(workoutId);

            // Assert
            completeWorkoutRepoMock.Verify(repo => repo.DeleteCompleteWorkoutsByWorkoutIdAsync(workoutId), Times.Once);
        }

        [Fact]
        public async Task InsertCompleteWorkoutAsync_CallsRepository()
        {
            // Arrange
            int workoutId = 1;
            int exerciseId = 2;
            int sets = 3;
            int reps = 12;

            completeWorkoutRepoMock.Setup(repo =>
                    repo.InsertCompleteWorkoutAsync(workoutId, exerciseId, sets, reps))
                .Returns(Task.CompletedTask);

            // Act
            await completeWorkoutService.InsertCompleteWorkoutAsync(workoutId, exerciseId, sets, reps);

            // Assert
            completeWorkoutRepoMock.Verify(repo =>
                repo.InsertCompleteWorkoutAsync(workoutId, exerciseId, sets, reps), Times.Once);
        }
    }
}
