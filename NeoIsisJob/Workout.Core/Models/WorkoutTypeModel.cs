using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("WorkoutTypes")]
    public class WorkoutTypeModel
    {
        [Key]
        [Column("WTID")]
        public int WTID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public WorkoutTypeModel()
        {
        }

        public WorkoutTypeModel(int id, string name)
        {
            WTID = id;
            Name = name;
        }
    }
}
