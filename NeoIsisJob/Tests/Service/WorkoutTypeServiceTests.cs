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
    public class WorkoutTypeServiceTests
    {
        private readonly Mock<IWorkoutTypeRepository> repoMock;
        private readonly WorkoutTypeService service;

        public WorkoutTypeServiceTests()
        {
            repoMock = new Mock<IWorkoutTypeRepository>();
            service = new WorkoutTypeService(repoMock.Object);
        }

        [Fact]
        public async Task InsertWorkoutTypeAsync_Throws_WhenNameIsNullOrWhitespace()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.InsertWorkoutTypeAsync(null));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.InsertWorkoutTypeAsync(string.Empty));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.InsertWorkoutTypeAsync("   "));
        }

        [Fact]
        public async Task InsertWorkoutTypeAsync_CallsRepository()
        {
            string name = "Cardio";
            await service.InsertWorkoutTypeAsync(name);
            repoMock.Verify(r => r.InsertWorkoutTypeAsync(name), Times.Once);
        }

        [Fact]
        public async Task InsertWorkoutTypeAsync_ThrowsGeneric_WhenRepositoryFails()
        {
            repoMock.Setup(r => r.InsertWorkoutTypeAsync(It.IsAny<string>()))
                     .ThrowsAsync(new Exception("Some DB error"));

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                service.InsertWorkoutTypeAsync("Strength"));

            Assert.Contains("An error occurred", ex.Message);
        }

        [Fact]
        public async Task DeleteWorkoutTypeAsync_CallsRepository()
        {
            int id = 1;
            await service.DeleteWorkoutTypeAsync(id);
            repoMock.Verify(r => r.DeleteWorkoutTypeAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetWorkoutTypeByIdAsync_ReturnsType()
        {
            var expected = new WorkoutTypeModel { WTID = 1, Name = "Yoga" };
            repoMock.Setup(r => r.GetWorkoutTypeByIdAsync(1))
                     .ReturnsAsync(expected);

            var result = await service.GetWorkoutTypeByIdAsync(1);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllWorkoutTypesAsync_ReturnsList()
        {
            var expected = new List<WorkoutTypeModel>
            {
                new WorkoutTypeModel { WTID = 1, Name = "Cardio" },
                new WorkoutTypeModel { WTID = 2, Name = "Strength" }
            };
            repoMock.Setup(r => r.GetAllWorkoutTypesAsync())
                     .ReturnsAsync(expected);

            var result = await service.GetAllWorkoutTypesAsync();

            Assert.Equal(2, result.Count);
        }
    }
}
