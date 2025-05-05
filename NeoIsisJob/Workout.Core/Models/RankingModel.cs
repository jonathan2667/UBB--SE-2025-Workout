using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("Rankings")]
    public class RankingModel
    {
        [Column("UID")]
        public int UID { get; set; }
        
        [Column("MGID")]
        public int MGID { get; set; }
        
        [Range(0, 10000)]
        [Column("Rank")]
        public int Rank { get; set; }

        public RankingModel()
        {
        }

        public RankingModel(int userId, int muscleGroupId, int rank)
        {
            UID = userId;
            MGID = muscleGroupId;
            Rank = rank;
        }
        
        // Navigation properties
        [ForeignKey("UID")]
        public virtual UserModel User { get; set; }
        
        [ForeignKey("MGID")]
        public virtual MuscleGroupModel MuscleGroup { get; set; }
    }
}
