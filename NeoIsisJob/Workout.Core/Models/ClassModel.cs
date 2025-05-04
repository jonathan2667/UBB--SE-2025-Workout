using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Core.Models
{
    public class ClassModel
    {
        private int id;
        private string name;
        private string description;
        private int classTypeId;
        private int personalTrainerId;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public int ClassTypeId { get => classTypeId; set => classTypeId = value; }
        public int PersonalTrainerId { get => personalTrainerId; set => personalTrainerId = value; }
        public PersonalTrainerModel PersonalTrainer { get; set; }

        public string TrainerFullName => PersonalTrainer != null ? $"{PersonalTrainer.LastName} {PersonalTrainer.FirstName}" : "No Trainer Assigned";
        public ClassModel()
        {
        }

        public ClassModel(int id, string name, string description, int classTypeId, int personalTrainerId)
        {
            Id = id;
            Name = name;
            Description = description;
            ClassTypeId = classTypeId;
            PersonalTrainerId = personalTrainerId;
        }
    }
}
