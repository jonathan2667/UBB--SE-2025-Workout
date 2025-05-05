using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("Exercises")]
    public class ExercisesModel
    {
        [Key]
        [Column("EID")]
        public int EID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        public string Description { get; set; }

        [Range(1, 10)]
        public int Difficulty { get; set; }

        [Column("MGID")]
        public int MGID { get; set; }

        public ExercisesModel()
        {
        }

        public ExercisesModel(int id, string name, string description, int difficulty, int muscleGroupId)
        {
            EID = id;
            Name = name;
            Description = description;
            Difficulty = difficulty;
            MGID = muscleGroupId;
        }

        // Navigation property
        [ForeignKey("MGID")]
        public virtual MuscleGroupModel MuscleGroup { get; set; }
    }
}
