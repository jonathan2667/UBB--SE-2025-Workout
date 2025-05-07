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
    public class ClassServiceTests
    {
        private readonly Mock<IClassRepository> classRepoMock;
        private readonly Mock<IUserClassService> userClassServiceMock;
        private readonly ClassService classService;

        public ClassServiceTests()
        {
            classRepoMock = new Mock<IClassRepository>();
            userClassServiceMock = new Mock<IUserClassService>();
            classService = new ClassService(classRepoMock.Object, userClassServiceMock.Object);
        }

        [Fact]
        public async Task GetAllClassesAsync_ReturnsListOfClasses()
        {
            // Arrange
            var expectedClasses = new List<ClassModel>
            {
                new ClassModel { CID = 1, Name = "Yoga" },
                new ClassModel { CID = 2, Name = "Cardio" }
            };

            classRepoMock.Setup(repo => repo.GetAllClassModelAsync())
                          .ReturnsAsync(expectedClasses);

            // Act
            var result = await classService.GetAllClassesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Yoga", result[0].Name);
            Assert.Equal("Cardio", result[1].Name);
        }

        [Fact]
        public async Task GetClassByIdAsync_ReturnsClassModel()
        {
            // Arrange
            var classId = 1;
            var expectedClass = new ClassModel { CID = classId, Name = "Spin" };

            classRepoMock.Setup(repo => repo.GetClassModelByIdAsync(classId))
                          .ReturnsAsync(expectedClass);

            // Act
            var result = await classService.GetClassByIdAsync(classId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Spin", result.Name);
        }

        [Fact]
        public async Task AddClassAsync_InvokesRepositoryMethod()
        {
            // Arrange
            var newClass = new ClassModel { Name = "Pilates" };

            classRepoMock.Setup(repo => repo.AddClassModelAsync(newClass))
                          .Returns(Task.CompletedTask);

            // Act
            await classService.AddClassAsync(newClass);

            // Assert
            classRepoMock.Verify(repo => repo.AddClassModelAsync(newClass), Times.Once);
        }

        [Fact]
        public async Task DeleteClassAsync_InvokesRepositoryMethod()
        {
            // Arrange
            var classId = 5;

            classRepoMock.Setup(repo => repo.DeleteClassModelAsync(classId))
                          .Returns(Task.CompletedTask);

            // Act
            await classService.DeleteClassAsync(classId);

            // Assert
            classRepoMock.Verify(repo => repo.DeleteClassModelAsync(classId), Times.Once);
        }

        [Fact]
        public async Task ConfirmRegistrationAsync_ReturnsEmptyString_WhenSuccessful()
        {
            // Arrange
            int userId = 1;
            int classId = 2;
            var date = DateTime.Today;
            var classModel = new ClassModel { CID = classId, Name = "HIIT" };

            classRepoMock.Setup(repo => repo.GetClassModelByIdAsync(classId))
                          .ReturnsAsync(classModel);

            userClassServiceMock.Setup(service => service.AddUserClassAsync(It.IsAny<UserClassModel>()))
                                 .Returns(Task.CompletedTask);

            // Act
            var result = await classService.ConfirmRegistrationAsync(userId, classId, date);

            // Assert
            Assert.Equal(string.Empty, result);
            userClassServiceMock.Verify(service => service.AddUserClassAsync(It.Is<UserClassModel>(
                uc => uc.UID == userId && uc.CID == classId && uc.Date == date)), Times.Once);
        }

        [Fact]
        public async Task ConfirmRegistrationAsync_ReturnsErrorMessage_OnException()
        {
            // Arrange
            int userId = 1;
            int classId = 2;
            var date = DateTime.Today;

            userClassServiceMock.Setup(service => service.AddUserClassAsync(It.IsAny<UserClassModel>()))
                                 .ThrowsAsync(new Exception("Test Error"));

            // Act
            var result = await classService.ConfirmRegistrationAsync(userId, classId, date);

            // Assert
            Assert.StartsWith("Registration failed:", result);
        }
    }
}
