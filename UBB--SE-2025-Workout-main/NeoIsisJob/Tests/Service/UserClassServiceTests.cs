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
    public class UserClassServiceTests
    {
        private readonly Mock<IUserClassRepo> userClassRepoMock;
        private readonly UserClassService userClassService;

        public UserClassServiceTests()
        {
            userClassRepoMock = new Mock<IUserClassRepo>();
            userClassService = new UserClassService(userClassRepoMock.Object);
        }

        [Fact]
        public async Task GetAllUserClassesAsync_ReturnsListOfUserClasses()
        {
            // Arrange
            var userClasses = new List<UserClassModel>
            {
                new UserClassModel { UID = 1, CID = 2, Date = DateTime.Today },
                new UserClassModel { UID = 3, CID = 4, Date = DateTime.Today }
            };

            userClassRepoMock
                .Setup(repo => repo.GetAllUserClassModelAsync())
                .ReturnsAsync(userClasses);

            // Act
            var result = await userClassService.GetAllUserClassesAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetUserClassByIdAsync_ReturnsCorrectUserClass()
        {
            // Arrange
            int userId = 1;
            int classId = 2;
            DateTime date = DateTime.Today;

            var expected = new UserClassModel
            {
                UID = userId,
                CID = classId,
                Date = date
            };

            userClassRepoMock
                .Setup(repo => repo.GetUserClassModelByIdAsync(userId, classId, date))
                .ReturnsAsync(expected);

            // Act
            var result = await userClassService.GetUserClassByIdAsync(userId, classId, date);

            // Assert
            Assert.Equal(userId, result.UID);
            Assert.Equal(classId, result.CID);
            Assert.Equal(date, result.Date);
        }

        [Fact]
        public async Task AddUserClassAsync_CallsRepositoryOnce()
        {
            // Arrange
            var userClass = new UserClassModel
            {
                UID = 1,
                CID = 2,
                Date = DateTime.Today
            };

            userClassRepoMock
                .Setup(repo => repo.AddUserClassModelAsync(userClass))
                .Returns(Task.CompletedTask);

            // Act
            await userClassService.AddUserClassAsync(userClass);

            // Assert
            userClassRepoMock.Verify(
                repo => repo.AddUserClassModelAsync(userClass), Times.Once);
        }

        [Fact]
        public async Task DeleteUserClassAsync_CallsRepositoryOnce()
        {
            // Arrange
            int userId = 5;
            int classId = 10;
            DateTime date = DateTime.Today;

            userClassRepoMock
                .Setup(repo => repo.DeleteUserClassModelAsync(userId, classId, date))
                .Returns(Task.CompletedTask);

            // Act
            await userClassService.DeleteUserClassAsync(userId, classId, date);

            // Assert
            userClassRepoMock.Verify(
                repo => repo.DeleteUserClassModelAsync(userId, classId, date), Times.Once);
        }
    }
}
