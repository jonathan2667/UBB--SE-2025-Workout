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
    public class UserServiceTests
    {
        private readonly Mock<IUserRepo> userRepoMock;
        private readonly UserService userService;

        public UserServiceTests()
        {
            userRepoMock = new Mock<IUserRepo>();
            userService = new UserService(userRepoMock.Object);
        }

        [Fact]
        public async Task RegisterNewUserAsync_ReturnsNewUserId()
        {
            // Arrange
            int expectedUserId = 42;
            userRepoMock
                .Setup(repo => repo.InsertUserAsync())
                .ReturnsAsync(expectedUserId);

            // Act
            int result = await userService.RegisterNewUserAsync();

            // Assert
            Assert.Equal(expectedUserId, result);
        }

        [Fact]
        public async Task GetUserAsync_ReturnsUserModel_WhenValidId()
        {
            // Arrange
            int userId = 10;
            var expected = new UserModel { ID = userId };

            userRepoMock
                .Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(expected);

            // Act
            var result = await userService.GetUserAsync(userId);

            // Assert
            Assert.Equal(userId, result.ID);
        }

        [Fact]
        public async Task GetUserAsync_ThrowsException_WhenInvalidId()
        {
            // Arrange
            int invalidUserId = -1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                userService.GetUserAsync(invalidUserId));
        }

        [Fact]
        public async Task RemoveUserAsync_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            int userId = 5;
            userRepoMock
                .Setup(repo => repo.DeleteUserByIdAsync(userId))
                .ReturnsAsync(true);

            // Act
            bool result = await userService.RemoveUserAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveUserAsync_ThrowsException_WhenInvalidId()
        {
            // Arrange
            int invalidUserId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                userService.RemoveUserAsync(invalidUserId));
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsUserList()
        {
            // Arrange
            var users = new List<UserModel>
            {
                new UserModel { ID = 1 },
                new UserModel { ID = 2 }
            };

            userRepoMock
                .Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await userService.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}
