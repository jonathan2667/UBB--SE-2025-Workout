using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Workout.Tests.Services
{
    public class RankingsServiceTests
    {
        private readonly Mock<IRankingsRepository> rankingsRepositoryMock;
        private readonly RankingsService rankingsService;

        public RankingsServiceTests()
        {
            rankingsRepositoryMock = new Mock<IRankingsRepository>();
            rankingsService = new RankingsService(rankingsRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllRankingsByUserIDAsync_ReturnsCorrectList()
        {
            // Arrange
            int userId = 1;
            var expectedRankings = new List<RankingModel>
            {
                new RankingModel { UID = userId, MGID = 1, Rank = 120 },
                new RankingModel { UID = userId, MGID = 2, Rank = 200 },
            };

            rankingsRepositoryMock
                .Setup(r => r.GetAllRankingsByUserIDAsync(userId))
                .ReturnsAsync(expectedRankings);

            // Act
            var result = await rankingsService.GetAllRankingsByUserIDAsync(userId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].MGID);
        }

        [Fact]
        public async Task GetRankingByFullIDAsync_ReturnsCorrectRanking()
        {
            // Arrange
            int userId = 2;
            int mgId = 3;
            var expected = new RankingModel { UID = userId, MGID = mgId, Rank = 250 };

            rankingsRepositoryMock
                .Setup(r => r.GetRankingByFullIDAsync(userId, mgId))
                .ReturnsAsync(expected);

            // Act
            var result = await rankingsService.GetRankingByFullIDAsync(userId, mgId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(250, result.Rank);
        }

        [Fact]
        public void CalculatePointsToNextRank_ReturnsCorrectPoints_WhenInMiddleRank()
        {
            // Arrange
            var rankDefinitions = new List<RankDefinition>
            {
                new RankDefinition { MinPoints = 0, MaxPoints = 100 },
                new RankDefinition { MinPoints = 100, MaxPoints = 200 },
                new RankDefinition { MinPoints = 200, MaxPoints = 300 },
            };

            int currentPoints = 150;

            // Act
            var pointsToNext = rankingsService.CalculatePointsToNextRank(currentPoints, rankDefinitions);

            // Assert
            Assert.Equal(50, pointsToNext);
        }

        [Fact]
        public void CalculatePointsToNextRank_ReturnsZero_WhenAtTopRank()
        {
            // Arrange
            var rankDefinitions = new List<RankDefinition>
            {
                new RankDefinition { MinPoints = 0, MaxPoints = 100 },
                new RankDefinition { MinPoints = 100, MaxPoints = 200 },
                new RankDefinition { MinPoints = 200, MaxPoints = 300 },
            };

            int currentPoints = 250;

            // Act
            var pointsToNext = rankingsService.CalculatePointsToNextRank(currentPoints, rankDefinitions);

            // Assert
            Assert.Equal(0, pointsToNext);
        }

        [Fact]
        public void CalculatePointsToNextRank_ReturnsNextMinPoints_WhenBelowFirstRank()
        {
            // Arrange
            var rankDefinitions = new List<RankDefinition>
            {
                new RankDefinition { MinPoints = 10, MaxPoints = 50 },
                new RankDefinition { MinPoints = 50, MaxPoints = 100 },
            };

            int currentPoints = 15;

            // Act
            var pointsToNext = rankingsService.CalculatePointsToNextRank(currentPoints, rankDefinitions);

            // Assert
            Assert.Equal(35, pointsToNext);
        }
    }
}
