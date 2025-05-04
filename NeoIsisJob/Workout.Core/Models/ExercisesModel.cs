using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Core.Models
{
    public class ExercisesModel
    {
        private int id;
        private string name;
        private string description;
        private int difficulty;
        private int muscleGroupId;

        private MuscleGroupModel muscleGroup;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public int Difficulty { get => difficulty; set => difficulty = value; }
        public int MuscleGroupId { get => muscleGroupId; set => muscleGroupId = value; }

        // property for the referenced muscle group
        public MuscleGroupModel MuscleGroup { get => muscleGroup; set => muscleGroup = value; }

        public ExercisesModel()
        {
        }

        public ExercisesModel(int id, string name, string description, int difficulty, int muscleGroupId)
        {
            Id = id;
            Name = name;
            Description = description;
            Difficulty = difficulty;
            MuscleGroupId = muscleGroupId;
        }
    }
}
