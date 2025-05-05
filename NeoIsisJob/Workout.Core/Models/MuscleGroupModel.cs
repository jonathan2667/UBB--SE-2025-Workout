using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("MuscleGroups")]
    public class MuscleGroupModel
    {
        [Key]
        [Column("MGID")]
        public int MGID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public MuscleGroupModel()
        {
        }

        public MuscleGroupModel(int id, string name)
        {
            MGID = id;
            Name = name;
        }
    }
}
