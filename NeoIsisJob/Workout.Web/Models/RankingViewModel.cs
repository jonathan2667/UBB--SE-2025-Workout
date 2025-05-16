using System.Collections.Generic;
using Workout.Core.Models;

namespace Workout.Web.Models
{
    public class RankingViewModel
    {
        public List<RankDefinition> RankDefinitions { get; set; } = new List<RankDefinition>();
        public IList<RankingModel> UserRankings { get; set; } = new List<RankingModel>();

        // Helper method to get the rank definition for a given point value
        public RankDefinition GetRankDefinitionForPoints(int points)
        {
            return RankDefinitions.Find(r => points >= r.MinPoints && points < r.MaxPoints) 
                   ?? RankDefinitions[RankDefinitions.Count - 1];
        }
    }
} 