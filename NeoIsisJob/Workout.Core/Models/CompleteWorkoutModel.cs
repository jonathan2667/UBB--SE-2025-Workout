using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("CompleteWorkouts")]
    public class CompleteWorkoutModel
    {
        [Column("WID")]
        public int WID { get; set; }

        [Column("EID")]
        public int EID { get; set; }

        public int Sets { get; set; }

        [Column("RepsPerSet")]
        public int RepsPerSet { get; set; }

        public CompleteWorkoutModel()
        {
        }

        public CompleteWorkoutModel(int workoutId, int exerciseId, int sets, int repetitionsPerSet)
        {
            WID = workoutId;
            EID = exerciseId;
            Sets = sets;
            RepsPerSet = repetitionsPerSet;
        }

        // Navigation properties
        [ForeignKey("WID")]
        public virtual WorkoutModel Workout { get; set; }

        [ForeignKey("EID")]
        public virtual ExercisesModel Exercise { get; set; }
    }
}
