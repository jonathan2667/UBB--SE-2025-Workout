using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Core.Models
{
    public class WorkoutTypeModel
    {
        private int id;
        private string name;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }

        public WorkoutTypeModel()
        {
        }

        public WorkoutTypeModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
