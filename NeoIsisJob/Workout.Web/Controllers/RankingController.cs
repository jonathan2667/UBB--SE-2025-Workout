using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Web.Models;

namespace Workout.Web.Controllers
{
    public class RankingController : Controller
    {
        private readonly IRankingsService _rankingsService;
        private readonly int _currentUserId = 1; // In a real app, get this from authentication

        public RankingController(IRankingsService rankingsService)
        {
            _rankingsService = rankingsService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new RankingViewModel
            {
                RankDefinitions = GetRankDefinitions(),
                UserRankings = await _rankingsService.GetAllRankingsByUserIDAsync(_currentUserId)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> MuscleGroupDetails(int muscleGroupId)
        {
            var ranking = await _rankingsService.GetRankingByFullIDAsync(_currentUserId, muscleGroupId);
            if (ranking == null)
            {
                return NotFound();
            }

            var muscleGroupViewModel = new MuscleGroupRankingViewModel
            {
                MuscleGroupId = muscleGroupId,
                MuscleGroupName = ranking.MuscleGroup?.Name ?? $"Muscle Group {muscleGroupId}",
                CurrentRank = ranking.Rank,
                RankDefinition = GetRankDefinitionForPoints(ranking.Rank),
                PointsToNextRank = CalculatePointsToNextRank(ranking.Rank)
            };

            return View(muscleGroupViewModel);
        }

        private List<RankDefinition> GetRankDefinitions()
        {
            return new List<RankDefinition>
            {
                new RankDefinition
                {
                    Name = "Challenger",
                    MinPoints = 9500,
                    MaxPoints = 10000,
                    Color = System.Drawing.Color.Aquamarine,
                    ImagePath = "/images/ranks/Rank8.png"
                },
                new RankDefinition
                {
                    Name = "Grandmaster",
                    MinPoints = 8500,
                    MaxPoints = 9500,
                    Color = System.Drawing.Color.OrangeRed,
                    ImagePath = "/images/ranks/Rank7.png"
                },
                new RankDefinition
                {
                    Name = "Master",
                    MinPoints = 7000,
                    MaxPoints = 8500,
                    Color = System.Drawing.Color.DarkViolet,
                    ImagePath = "/images/ranks/Rank6.png"
                },
                new RankDefinition
                {
                    Name = "Elite",
                    MinPoints = 5000,
                    MaxPoints = 7000,
                    Color = System.Drawing.Color.DarkGreen,
                    ImagePath = "/images/ranks/Rank5.png"
                },
                new RankDefinition
                {
                    Name = "Gold",
                    MinPoints = 3500,
                    MaxPoints = 5000,
                    Color = System.Drawing.Color.Gold,
                    ImagePath = "/images/ranks/Rank4.png"
                },
                new RankDefinition
                {
                    Name = "Silver",
                    MinPoints = 2250,
                    MaxPoints = 3500,
                    Color = System.Drawing.Color.Silver,
                    ImagePath = "/images/ranks/Rank3.png"
                },
                new RankDefinition
                {
                    Name = "Bronze",
                    MinPoints = 1000,
                    MaxPoints = 2250,
                    Color = System.Drawing.Color.SandyBrown,
                    ImagePath = "/images/ranks/Rank2.png"
                },
                new RankDefinition
                {
                    Name = "Beginner",
                    MinPoints = 0,
                    MaxPoints = 1000,
                    Color = System.Drawing.Color.DimGray,
                    ImagePath = "/images/ranks/Rank1.png"
                }
            };
        }

        private RankDefinition GetRankDefinitionForPoints(int points)
        {
            var definitions = GetRankDefinitions();
            return definitions.Find(r => points >= r.MinPoints && points < r.MaxPoints)
                   ?? definitions[definitions.Count - 1];
        }

        private int CalculatePointsToNextRank(int currentPoints)
        {
            var definitions = GetRankDefinitions();
            return _rankingsService.CalculatePointsToNextRank(currentPoints, definitions);
        }
    }
} 