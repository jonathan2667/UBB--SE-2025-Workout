using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Microsoft.UI.Xaml.Media;
// using Windows.UI;
using Workout.Core.Models;
using Workout.Core.IServices;
using NeoIsisJob.Proxy;
using NeoIsisJob.Helpers;
using Refit;
using Workout.Core.IServices;

namespace NeoIsisJob.ViewModels.Rankings
{
    /// <summary>
    /// RankingsViewModel - Now uses hardcoded data instead of API calls to avoid 404 errors.
    /// This ensures the Rankings tab works while API issues are being resolved.
    /// </summary>
    public class RankingsViewModel
    {
        private readonly RankingsServiceProxy rankingsService;
        private readonly int userId = 1; // !!!!!!!!!!!!!!! HARDCODED USER VALUE !!!!!!! CHANGE THIS FOR PROD !!!!!!!!
        private readonly List<RankDefinition> rankDefinitions;

        public RankingsViewModel()
        {
            this.rankingsService = new RankingsServiceProxy();
            this.rankDefinitions = InitializeRankDefinitions();
        }

        private List<RankDefinition> InitializeRankDefinitions()
        {
            return new List<RankDefinition>
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
        }

        public IList<RankDefinition> GetRankDefinitions()
        {
            return rankDefinitions;
        }

        public RankDefinition GetRankDefinitionForPoints(int points)
        {
            return rankDefinitions.FirstOrDefault(r => points >= r.MinPoints && points < r.MaxPoints)
                   ?? rankDefinitions.Last();
        }

        public int GetNextRankPoints(int currentRank)
        {
            // Calculate locally instead of using API
            var currentRankDefinition = rankDefinitions.FirstOrDefault(r =>
               currentRank >= r.MinPoints && currentRank < r.MaxPoints)
               ?? rankDefinitions.Last();

            // Find the next rank (with higher minimum points)
            var nextRank = rankDefinitions.FirstOrDefault(r => r.MinPoints > currentRankDefinition.MinPoints);

            // Calculate points needed to reach next rank or return 0 if at highest rank
            return nextRank?.MinPoints - currentRank ?? 0;
        }

        public async Task<RankingModel> GetRankingByMGID(int muscleGroupid)
        {
            // Hardcoded rankings data to avoid API issues
            var hardcodedRankings = new Dictionary<int, RankingModel>
            {
                { 1, new RankingModel(1, 1, 2000) }, // Chest: 2000 points
                { 2, new RankingModel(1, 2, 7800) }, // Legs: 7800 points  
                { 3, new RankingModel(1, 3, 6700) }, // Arms: 6700 points
                { 4, new RankingModel(1, 4, 9600) }, // Abs: 9600 points
                { 5, new RankingModel(1, 5, 3700) }  // Back: 3700 points
            };

            // Return hardcoded data immediately
            if (hardcodedRankings.ContainsKey(muscleGroupid))
            {
                return await Task.FromResult(hardcodedRankings[muscleGroupid]);
            }

            // Fallback to default if muscle group not found
            return await Task.FromResult(new RankingModel(1, muscleGroupid, 1000)); // Default: 1000 points
        }

        public SolidColorBrush GetRankColor(int rank)
        {
            var def = GetRankDefinitionForPoints(rank);
            System.Drawing.Color sd = def.Color;  // your GDI+ color

            // Convert to the UWP/WinUI Color struct
            Windows.UI.Color uiColor =
                Windows.UI.Color.FromArgb(sd.A, sd.R, sd.G, sd.B);

            return new SolidColorBrush(uiColor);
        }

        public string GetRankIcon(int rank)
        {
            var rankDefinition = GetRankDefinitionForPoints(rank);
            return rankDefinition.ImagePath;
        }

        public int GetRankLowerBound(int rank)
        {
            var rankDefinition = GetRankDefinitionForPoints(rank);
            return rankDefinition.MinPoints;
        }

        public int GetRankUpperBound(int rank)
        {
            var rankDefinition = GetRankDefinitionForPoints(rank);
            return rankDefinition.MaxPoints;
        }
    }
}
