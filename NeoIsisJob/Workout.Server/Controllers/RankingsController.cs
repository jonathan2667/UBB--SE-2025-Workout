// Workout.Server/Controllers/RankingsController.cs
using System.Collections.Generic;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]// ⇒ /api/rankings
    public class RankingsController : ControllerBase
    {
        private readonly IRankingsService rankingsService;

        public RankingsController(IRankingsService rankingsService)
            => this.rankingsService = rankingsService;

        // GET /api/rankings/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<RankingModel>>> GetAllByUser(int userId)
        {
            var rankings = await rankingsService.GetAllRankingsByUserIDAsync(userId);
            return Ok(rankings);
        }

        // GET /api/rankings/{userId}/{muscleGroupId}
        [HttpGet("{userId}/{muscleGroupId}")]
        public async Task<ActionResult<RankingModel>> GetByUserAndGroup(int userId, int muscleGroupId)
        {
            var ranking = await rankingsService.GetRankingByFullIDAsync(userId, muscleGroupId);
            if (ranking == null)
            {
                return NotFound();
            }
            return Ok(ranking);
        }

        // GET /api/rankings/calculate/{points}
        [HttpGet("calculate/{points}")]
        public ActionResult<object> Calculate(int points)
        {
            // Define your rank bands:
            var rankDefinitions = new List<RankDefinition>
            {
                new RankDefinition { Name = "Challenger",  MinPoints = 9500, MaxPoints = 10000, Color = Color.Aquamarine,   ImagePath = "/Assets/Ranks/Rank8.png" },
                new RankDefinition { Name = "Grandmaster", MinPoints = 8500, MaxPoints = 9500,  Color = Color.OrangeRed,   ImagePath = "/Assets/Ranks/Rank7.png" },
                new RankDefinition { Name = "Master",      MinPoints = 7000, MaxPoints = 8500,  Color = Color.DarkViolet,   ImagePath = "/Assets/Ranks/Rank6.png" },
                new RankDefinition { Name = "Elite",       MinPoints = 5000, MaxPoints = 7000,  Color = Color.DarkGreen,    ImagePath = "/Assets/Ranks/Rank5.png" },
                new RankDefinition { Name = "Gold",        MinPoints = 3500, MaxPoints = 5000,  Color = Color.Gold,         ImagePath = "/Assets/Ranks/Rank4.png" },
                new RankDefinition { Name = "Silver",      MinPoints = 2250, MaxPoints = 3500,  Color = Color.Silver,       ImagePath = "/Assets/Ranks/Rank3.png" },
                new RankDefinition { Name = "Bronze",      MinPoints = 1000, MaxPoints = 2250,  Color = Color.SandyBrown,   ImagePath = "/Assets/Ranks/Rank2.png" },
                new RankDefinition { Name = "Beginner",    MinPoints = 0,    MaxPoints = 1000,  Color = Color.DimGray,      ImagePath = "/Assets/Ranks/Rank1.png" }
            };

            // Call your service (returns whatever type it returns—string, a DTO, etc.)
            var result = rankingsService.CalculatePointsToNextRank(points, rankDefinitions);

            // Return as-is:
            return Ok(result);
        }
    }
}
