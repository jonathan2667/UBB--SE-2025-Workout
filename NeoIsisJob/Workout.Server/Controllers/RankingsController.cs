using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RankingsController : ControllerBase
    {
        private readonly IRankingsService _rankingsService;

        public RankingsController(IRankingsService rankingsService)
        {
            _rankingsService = rankingsService;
        }
        [HttpGet("api/rankings/{userId}")]
        public async Task<IActionResult> GetAllRankingsByUserID(int userId)
        {
            try
            {
                var rankings = await _rankingsService.GetAllRankingsByUserIDAsync(userId);
                return Ok(rankings);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching rankings: {ex.Message}");
            }
        }
        [HttpGet("api/rankings/{userId}/{muscleGroupId}")]
        public async Task<IActionResult> GetRankingByUserIDAndMuscleGroupID(int userId, int muscleGroupId)
        {
            try
            {
                var ranking = await _rankingsService.GetRankingByFullIDAsync(userId, muscleGroupId);
                return Ok(ranking);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching ranking: {ex.Message}");
            }
        }

        [HttpGet("api/rankings/calculate/{points}")]
        public async Task<IActionResult> CalculatePoints(int points)
        {
            List<RankDefinition> rankDefinitions = new List<RankDefinition>
            {
                new RankDefinition
                {
                    Name = "Challenger",
                    MinPoints = 9500,
                    MaxPoints = 10000,
                    Color = Color.Aquamarine,
                    ImagePath = "/Assets/Ranks/Rank8.png"
                },
                new RankDefinition
                {
                    Name = "Grandmaster",
                    MinPoints = 8500,
                    MaxPoints = 9500,
                    Color = Color.OrangeRed,
                    ImagePath = "/Assets/Ranks/Rank7.png"
                },
                new RankDefinition
                {
                    Name = "Master",
                    MinPoints = 7000,
                    MaxPoints = 8500,
                    Color = Color.DarkViolet,
                    ImagePath = "/Assets/Ranks/Rank6.png"
                },
                new RankDefinition
                {
                    Name = "Elite",
                    MinPoints = 5000,
                    MaxPoints = 7000,
                    Color = Color.DarkGreen,
                    ImagePath = "/Assets/Ranks/Rank5.png"
                },
                new RankDefinition
                {
                    Name = "Gold",
                    MinPoints = 3500,
                    MaxPoints = 5000,
                    Color = Color.Gold,
                    ImagePath = "/Assets/Ranks/Rank4.png"
                },
                new RankDefinition
                {
                    Name = "Silver",
                    MinPoints = 2250,
                    MaxPoints = 3500,
                    Color = Color.Silver,
                    ImagePath = "/Assets/Ranks/Rank3.png"
                },
                new RankDefinition
                {
                    Name = "Bronze",
                    MinPoints = 1000,
                    MaxPoints = 2250,
                    Color = Color.SandyBrown,
                    ImagePath = "/Assets/Ranks/Rank2.png"
                },
                new RankDefinition
                {
                    Name = "Beginner",
                    MinPoints = 0,
                    MaxPoints = 1000,
                    Color = Color.DimGray,
                    ImagePath = "/Assets/Ranks/Rank1.png"
                }
            };

            try
            {
                var calculated = _rankingsService.CalculatePointsToNextRank(points, rankDefinitions);
                return Ok(calculated);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error calculating points: {ex.Message}");
            }
        }


    }
}
