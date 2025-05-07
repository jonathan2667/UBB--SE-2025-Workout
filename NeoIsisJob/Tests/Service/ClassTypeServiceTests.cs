using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Workout.Core.IRepositories;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Core.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Workout.Tests.Services
{
    public class ClassTypeServiceTests
    {
        private readonly Mock<IClassTypeRepository> classTypeRepoMock;
        private readonly ClassTypeService classTypeService;

        public ClassTypeServiceTests()
        {
            classTypeRepoMock = new Mock<IClassTypeRepository>();
            classTypeService = new ClassTypeService(classTypeRepoMock.Object);
        }

        [Fact]
        public async Task GetAllClassTypesAsync_ReturnsListOfClassTypes()
        {
            // Arrange
            var expectedClassTypes = new List<ClassTypeModel>
            {
                new ClassTypeModel { CTID = 1, Name = "Strength" },
                new ClassTypeModel { CTID = 2, Name = "Flexibility" }
            };

            classTypeRepoMock.Setup(repo => repo.GetAllClassTypeModelAsync())
                              .ReturnsAsync(expectedClassTypes);

            // Act
            var result = await classTypeService.GetAllClassTypesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Strength", result[0].Name);
            Assert.Equal("Flexibility", result[1].Name);
        }

        [Fact]
        public async Task GetClassTypeByIdAsync_ReturnsCorrectClassType()
        {
            // Arrange
            var classTypeId = 3;
            var expected = new ClassTypeModel { CTID = classTypeId, Name = "Endurance" };

            classTypeRepoMock.Setup(repo => repo.GetClassTypeModelByIdAsync(classTypeId))
                              .ReturnsAsync(expected);

            // Act
            var result = await classTypeService.GetClassTypeByIdAsync(classTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Endurance", result.Name);
        }

        [Fact]
        public async Task AddClassTypeAsync_CallsRepository()
        {
            // Arrange
            var newClassType = new ClassTypeModel { Name = "Mobility" };

            classTypeRepoMock.Setup(repo => repo.AddClassTypeModelAsync(newClassType))
                              .Returns(Task.CompletedTask);

            // Act
            await classTypeService.AddClassTypeAsync(newClassType);

            // Assert
            classTypeRepoMock.Verify(repo => repo.AddClassTypeModelAsync(newClassType), Times.Once);
        }

        [Fact]
        public async Task DeleteClassTypeAsync_CallsRepository()
        {
            // Arrange
            var classTypeId = 4;

            classTypeRepoMock.Setup(repo => repo.DeleteClassTypeModelAsync(classTypeId))
                              .Returns(Task.CompletedTask);

            // Act
            await classTypeService.DeleteClassTypeAsync(classTypeId);

            // Assert
            classTypeRepoMock.Verify(repo => repo.DeleteClassTypeModelAsync(classTypeId), Times.Once);
        }
    }
}
