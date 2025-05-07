using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Moq;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Workout.Tests.Services
{
    public class CalendarServiceTests
    {
        private readonly Mock<ICalendarRepository> _calendarRepoMock;
        private readonly Mock<IUserWorkoutRepository> _userWorkoutRepoMock;
        private readonly CalendarService _calendarService;

        public CalendarServiceTests()
        {
            _calendarRepoMock = new Mock<ICalendarRepository>();
            _userWorkoutRepoMock = new Mock<IUserWorkoutRepository>();
            _calendarService = new CalendarService(_calendarRepoMock.Object, _userWorkoutRepoMock.Object);
        }

        [Fact]
        public async Task GetCalendarDaysForMonthAsync_ReturnsDays()
        {
            // Arrange
            var userId = 1;
            var date = new DateTime(2025, 5, 1);
            var expectedDays = new List<CalendarDayModel>
            {
                new CalendarDayModel { Date = date, HasWorkout = true }
            };

            _calendarRepoMock.Setup(r => r.GetCalendarDaysForMonthAsync(userId, date))
                             .ReturnsAsync(expectedDays);

            // Act
            var result = await _calendarService.GetCalendarDaysForMonthAsync(userId, date);

            // Assert
            Assert.Single(result);
            Assert.True(result[0].HasWorkout);
        }

        [Fact]
        public async Task GetCalendarDaysAsync_BuildsGridCorrectly()
        {
            // Arrange
            var userId = 1;
            var currentDate = new DateTime(2025, 5, 1);
            var days = new List<CalendarDayModel>
            {
                new CalendarDayModel { Date = currentDate, HasWorkout = true }
            };

            _calendarRepoMock.Setup(r => r.GetCalendarDaysForMonthAsync(userId, currentDate))
                             .ReturnsAsync(days);

            // Act
            var result = await _calendarService.GetCalendarDaysAsync(userId, currentDate);

            // Assert
            Assert.Contains(result, d => d.HasWorkout);
        }

        [Fact]
        public async Task RemoveWorkoutAsync_DeletesWorkout_WhenExists()
        {
            // Arrange
            var userId = 1;
            var date = DateTime.Today;
            var workout = new UserWorkoutModel { WID = 10 };
            var day = new CalendarDayModel { Date = date };

            _calendarRepoMock.Setup(r => r.GetUserWorkoutAsync(userId, date))
                             .ReturnsAsync(workout);

            _userWorkoutRepoMock.Setup(r => r.DeleteUserWorkoutAsync(userId, workout.WID, date))
                                .Returns(Task.CompletedTask);

            // Act
            await _calendarService.RemoveWorkoutAsync(userId, day);

            // Assert
            _userWorkoutRepoMock.Verify(r => r.DeleteUserWorkoutAsync(userId, workout.WID, date), Times.Once);
        }

        [Fact]
        public void GetWorkoutDaysCountText_ReturnsCorrectText()
        {
            // Arrange
            var days = new ObservableCollection<CalendarDayModel>
            {
                new CalendarDayModel { HasWorkout = true },
                new CalendarDayModel { HasWorkout = false }
            };

            // Act
            var result = _calendarService.GetWorkoutDaysCountText(days);

            // Assert
            Assert.Equal("Workout Days: 1", result);
        }

        [Fact]
        public async Task AddUserWorkoutAsync_CallsRepository()
        {
            // Arrange
            var workout = new UserWorkoutModel { WID = 1 };
            _userWorkoutRepoMock.Setup(r => r.AddUserWorkoutAsync(workout))
                                .Returns(Task.CompletedTask);

            // Act
            await _calendarService.AddUserWorkoutAsync(workout);

            // Assert
            _userWorkoutRepoMock.Verify(r => r.AddUserWorkoutAsync(workout), Times.Once);
        }
    }
}
