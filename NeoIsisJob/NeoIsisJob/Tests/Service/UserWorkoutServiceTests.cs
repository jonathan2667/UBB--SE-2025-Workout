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
    public class UserWorkoutServiceTests
    {
        private readonly Mock<IUserWorkoutRepository> repoMock;
        private readonly UserWorkoutService service;

        public UserWorkoutServiceTests()
        {
            repoMock = new Mock<IUserWorkoutRepository>();
            service = new UserWorkoutService(repoMock.Object);
        }

        [Fact]
        public async Task GetUserWorkoutForDateAsync_ReturnsWorkout_WhenExists()
        {
            // Arrange
            int userId = 1;
            var date = new DateTime(2024, 1, 1);
            var workoutList = new List<UserWorkoutModel>
            {
                new UserWorkoutModel { UID = userId, Date = date },
                new UserWorkoutModel { UID = 2, Date = date }
            };
            repoMock.Setup(r => r.GetUserWorkoutModelByDateAsync(date))
                     .ReturnsAsync(workoutList);

            // Act
            var result = await service.GetUserWorkoutForDateAsync(userId, date);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UID);
        }

        [Fact]
        public async Task GetUserWorkoutForDateAsync_ThrowsOnInvalidUser()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                service.GetUserWorkoutForDateAsync(0, DateTime.Now));
        }

        [Fact]
        public async Task AddUserWorkoutAsync_CallsUpdate_WhenWorkoutExists()
        {
            var model = new UserWorkoutModel { UID = 1, Date = new DateTime(2024, 1, 1) };
            repoMock.Setup(r => r.GetUserWorkoutModelByDateAsync(model.Date))
                     .ReturnsAsync(new List<UserWorkoutModel> { model });

            // Act
            await service.AddUserWorkoutAsync(model);

            // Assert
            repoMock.Verify(r => r.UpdateUserWorkoutAsync(model), Times.Once);
        }

        [Fact]
        public async Task AddUserWorkoutAsync_CallsAdd_WhenWorkoutDoesNotExist()
        {
            var model = new UserWorkoutModel { UID = 1, Date = DateTime.Today };
            repoMock.Setup(r => r.GetUserWorkoutModelByDateAsync(model.Date))
                     .ReturnsAsync(new List<UserWorkoutModel>());

            // Act
            await service.AddUserWorkoutAsync(model);

            // Assert
            repoMock.Verify(r => r.AddUserWorkoutAsync(model), Times.Once);
        }

        [Fact]
        public async Task CompleteUserWorkoutAsync_MarksCompleted_AndUpdates()
        {
            int userId = 1, workoutId = 2;
            var date = DateTime.Today;
            var model = new UserWorkoutModel { UID = userId, WID = workoutId, Date = date };

            repoMock.Setup(r => r.GetUserWorkoutModelAsync(userId, workoutId, date))
                     .ReturnsAsync(model);

            // Act
            await service.CompleteUserWorkoutAsync(userId, workoutId, date);

            // Assert
            Assert.True(model.Completed);
            repoMock.Verify(r => r.UpdateUserWorkoutAsync(model), Times.Once);
        }

        [Fact]
        public async Task CompleteUserWorkoutAsync_DoesNothing_IfWorkoutNotFound()
        {
            repoMock.Setup(r => r.GetUserWorkoutModelAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                     .ReturnsAsync((UserWorkoutModel)null);

            await service.CompleteUserWorkoutAsync(1, 1, DateTime.Today);

            repoMock.Verify(r => r.UpdateUserWorkoutAsync(It.IsAny<UserWorkoutModel>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUserWorkoutAsync_ValidatesAndCallsRepo()
        {
            int userId = 1, workoutId = 2;
            var date = DateTime.Today;

            await service.DeleteUserWorkoutAsync(userId, workoutId, date);

            repoMock.Verify(r => r.DeleteUserWorkoutAsync(userId, workoutId, date), Times.Once);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public async Task DeleteUserWorkoutAsync_ThrowsIfIdsInvalid(int userId, int workoutId)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                service.DeleteUserWorkoutAsync(userId, workoutId, DateTime.Today));
        }

        [Fact]
        public async Task DeleteUserWorkoutAsync_ThrowsIfDateInvalid()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.DeleteUserWorkoutAsync(1, 1, default));
        }
    }
}
