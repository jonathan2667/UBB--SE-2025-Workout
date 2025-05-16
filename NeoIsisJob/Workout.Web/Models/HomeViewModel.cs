using System;
using System.Collections.Generic;
using Workout.Core.Models;

namespace Workout.Web.Models
{
    public class HomeViewModel
    {
        public string CurrentDate { get; set; }
        public Workout.Core.Models.WorkoutModel CurrentWorkout { get; set; }
        public List<ExerciseWithDetails> WorkoutExercises { get; set; }
        public bool HasActiveWorkout => CurrentWorkout != null;
        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }

        public class ExerciseWithDetails
        {
            public string Name { get; set; }
            public string Details { get; set; }

            public override string ToString()
            {
                return $"{Name}: {Details}";
            }
        }
    }
} 