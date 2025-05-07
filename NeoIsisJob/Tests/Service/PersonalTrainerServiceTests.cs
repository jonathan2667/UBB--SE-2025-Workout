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
    public class PersonalTrainerServiceTests
    {
        private readonly Mock<IPersonalTrainerRepo> personalTrainerRepoMock;
        private readonly PersonalTrainerService personalTrainerService;

        public PersonalTrainerServiceTests()
        {
            personalTrainerRepoMock = new Mock<IPersonalTrainerRepo>();
            personalTrainerService = new PersonalTrainerService(personalTrainerRepoMock.Object);
        }

        [Fact]
        public async Task GetAllPersonalTrainersAsync_ReturnsListOfTrainers()
        {
            // Arrange
            var trainers = new List<PersonalTrainerModel>
            {
                new PersonalTrainerModel { PTID = 1 },
                new PersonalTrainerModel { PTID = 2 },
            };

            personalTrainerRepoMock.Setup(r => r.GetAllPersonalTrainerModelAsync())
                                    .ReturnsAsync(trainers);

            // Act
            var result = await personalTrainerService.GetAllPersonalTrainersAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].PTID);
        }

        [Fact]
        public async Task GetPersonalTrainerByIdAsync_ReturnsCorrectTrainer()
        {
            // Arrange
            var trainer = new PersonalTrainerModel { PTID = 10 };

            personalTrainerRepoMock.Setup(r => r.GetPersonalTrainerModelByIdAsync(10))
                                    .ReturnsAsync(trainer);

            // Act
            var result = await personalTrainerService.GetPersonalTrainerByIdAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.PTID);
        }

        [Fact]
        public async Task AddPersonalTrainerAsync_CallsRepository()
        {
            // Arrange
            var trainer = new PersonalTrainerModel { PTID = 3 };

            personalTrainerRepoMock.Setup(r => r.AddPersonalTrainerModelAsync(trainer))
                                    .Returns(Task.CompletedTask);

            // Act
            await personalTrainerService.AddPersonalTrainerAsync(trainer);

            // Assert
            personalTrainerRepoMock.Verify(r => r.AddPersonalTrainerModelAsync(trainer), Times.Once);
        }

        [Fact]
        public async Task DeletePersonalTrainerAsync_CallsRepository()
        {
            // Arrange
            var trainerId = 5;
            var trainer = new PersonalTrainerModel { PTID = trainerId };

            personalTrainerRepoMock.Setup(r => r.GetPersonalTrainerModelByIdAsync(trainerId))
                                    .ReturnsAsync(trainer);

            personalTrainerRepoMock.Setup(r => r.DeletePersonalTrainerModelAsync(trainerId))
                                    .Returns(Task.CompletedTask);

            // Act
            await personalTrainerService.DeletePersonalTrainerAsync(trainerId);

            // Assert
            personalTrainerRepoMock.Verify(r => r.GetPersonalTrainerModelByIdAsync(trainerId), Times.Once);
            personalTrainerRepoMock.Verify(r => r.DeletePersonalTrainerModelAsync(trainerId), Times.Once);
        }
    }
}
