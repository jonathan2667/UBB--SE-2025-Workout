using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Core.Models
{
    internal class UserWorkoutModel
    {
        private int userId;
        private int workoutId;
        private DateTime date;
        private bool completed;

        public int UserId { get => userId; set => userId = value; }
        public int WorkoutId { get => workoutId; set => workoutId = value; }
        public DateTime Date { get => date; set => date = value; }
        public bool Completed { get => completed; set => completed = value; }
        public int UserWorkoutId { get; set; }

        private UserWorkoutModel()
        {
        }

        public UserWorkoutModel(int userId, int workoutId, DateTime date, bool completed)
        {
            UserId = userId;
            WorkoutId = workoutId;
            Date = date;
            Completed = completed;
        }
    }
}
