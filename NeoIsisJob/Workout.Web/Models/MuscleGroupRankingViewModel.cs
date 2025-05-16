using Workout.Core.Models;

namespace Workout.Web.Models
{
    public class MuscleGroupRankingViewModel
    {
        public int MuscleGroupId { get; set; }
        public string MuscleGroupName { get; set; }
        public int CurrentRank { get; set; }
        public RankDefinition RankDefinition { get; set; }
        public int PointsToNextRank { get; set; }
    }
} 