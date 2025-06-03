using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("Workouts")]
    public class WorkoutModel
    {
        [Key]
        [Column("WID")]
        public int WID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Column("WTID")]
        public int WTID { get; set; }

        public string Description { get; set; }

        public WorkoutModel()
        {
            CompleteWorkouts = new List<CompleteWorkoutModel>();
            UserWorkouts = new List<UserWorkoutModel>();
        }

        public WorkoutModel(int id, string name, int workoutTypeId, string description = null)
        {
            WID = id;
            Name = name;
            WTID = workoutTypeId;
            Description = description;
            CompleteWorkouts = new List<CompleteWorkoutModel>();
            UserWorkouts = new List<UserWorkoutModel>();
        }

        // Navigation properties
        [ForeignKey("WTID")]
        public virtual WorkoutTypeModel WorkoutType { get; set; }
        public virtual ICollection<CompleteWorkoutModel> CompleteWorkouts { get; set; }
        public virtual ICollection<UserWorkoutModel> UserWorkouts { get; set; }
    }
}
