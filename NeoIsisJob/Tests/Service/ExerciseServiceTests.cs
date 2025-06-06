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
    public class ExerciseServiceTests
    {
        private readonly Mock<IExerciseRepository> exerciseRepoMock;
        private readonly ExerciseService exerciseService;

        public ExerciseServiceTests()
        {
            exerciseRepoMock = new Mock<IExerciseRepository>();
            exerciseService = new ExerciseService(exerciseRepoMock.Object);
        }

        [Fact]
        public async Task GetExerciseByIdAsync_ReturnsCorrectExercise()
        {
            // Arrange
            int exerciseId = 10;
            var expected = new ExercisesModel { EID = exerciseId, Name = "Bench Press" };

            exerciseRepoMock.Setup(repo => repo.GetExerciseByIdAsync(exerciseId))
                             .ReturnsAsync(expected);

            // Act
            var result = await exerciseService.GetExerciseByIdAsync(exerciseId);

            // Assert
            Assert.Equal("Bench Press", result.Name);
            Assert.Equal(exerciseId, result.EID);
        }

        [Fact]
        public async Task GetAllExercisesAsync_ReturnsAllExercises()
        {
            // Arrange
            var exercises = new List<ExercisesModel>
            {
                new ExercisesModel { EID = 1, Name = "Squat" },
                new ExercisesModel { EID = 2, Name = "Deadlift" }
            };

            exerciseRepoMock.Setup(repo => repo.GetAllExercisesAsync())
                             .ReturnsAsync(exercises);

            // Act
            var result = await exerciseService.GetAllExercisesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Name == "Squat");
        }
    }
}
