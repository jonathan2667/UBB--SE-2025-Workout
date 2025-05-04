using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Core.Models
{
    public class WorkoutModel
    {
        private int id;
        private string name;
        private int workoutTypeId;
        private string description;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int WorkoutTypeId { get => workoutTypeId; set => workoutTypeId = value; }
        public string Description { get => description; set => description = value; }

        public WorkoutModel()
        {
        }

        public WorkoutModel(int id, string name, int workoutTypeId, string description = null)
        {
            Id = id;
            Name = name;
            WorkoutTypeId = workoutTypeId;
            Description = description;
        }
    }
}
