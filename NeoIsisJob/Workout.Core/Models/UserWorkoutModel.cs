using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("UserWorkouts")]
    public class UserWorkoutModel
    {
        [Column("UID")]
        public int UID { get; set; }

        [Column("WID")]
        public int WID { get; set; }

        [Column("Date")]
        public DateTime Date { get; set; }

        [Required]
        public bool Completed { get; set; }

        public UserWorkoutModel()
        {
        }

        public UserWorkoutModel(int userId, int workoutId, DateTime date, bool completed)
        {
            UID = userId;
            WID = workoutId;
            Date = date;
            Completed = completed;
        }

        // Navigation properties
        [ForeignKey("UID")]
        public virtual UserModel User { get; set; }

        [ForeignKey("WID")]
        public virtual WorkoutModel Workout { get; set; }
    }
}
