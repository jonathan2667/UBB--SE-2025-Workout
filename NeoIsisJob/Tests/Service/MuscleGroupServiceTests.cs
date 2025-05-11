using System;
using System.Threading.Tasks;
using Moq;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Workout.Tests.Services
{
    public class MuscleGroupServiceTests
    {
        private readonly Mock<IMuscleGroupRepo> muscleGroupRepoMock;
        private readonly MuscleGroupService muscleGroupService;

        public MuscleGroupServiceTests()
        {
            muscleGroupRepoMock = new Mock<IMuscleGroupRepo>();
            muscleGroupService = new MuscleGroupService(muscleGroupRepoMock.Object);
        }

        [Fact]
        public async Task GetMuscleGroupByIdAsync_ReturnsCorrectMuscleGroup()
        {
            // Arrange
            int id = 3;
            var expected = new MuscleGroupModel { MGID = id, Name = "Chest" };

            muscleGroupRepoMock.Setup(r => r.GetMuscleGroupByIdAsync(id))
                                .ReturnsAsync(expected);

            // Act
            var result = await muscleGroupService.GetMuscleGroupByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.MGID);
            Assert.Equal("Chest", result.Name);
        }
    }
}
