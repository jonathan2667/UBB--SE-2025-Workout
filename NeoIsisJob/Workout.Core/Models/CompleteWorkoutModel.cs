using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Core.Models
{
    public class CompleteWorkoutModel
    {
        private int workoutId;
        private int exerciseId;
        private int sets;
        private int repetitionsPerSet;

        private ExercisesModel exercise;

        public int WorkoutId { get => workoutId; set => workoutId = value; }
        public int ExerciseId { get => exerciseId; set => exerciseId = value; }
        public int Sets { get => sets; set => sets = value; }
        public int RepetitionsPerSet { get => repetitionsPerSet; set => repetitionsPerSet = value; }

        // property for tracking the complete excercise
        public ExercisesModel Exercise { get => exercise; set => exercise = value; }

        private CompleteWorkoutModel()
        {
        }

        public CompleteWorkoutModel(int workoutId, int exerciseId, int sets, int repetitionsPerSet)
        {
            WorkoutId = workoutId;
            ExerciseId = exerciseId;
            Sets = sets;
            RepetitionsPerSet = repetitionsPerSet;
        }
    }
}
