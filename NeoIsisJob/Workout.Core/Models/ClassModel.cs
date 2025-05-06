using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("Classes")]
    public class ClassModel
    {
        [Key]
        [Column("CID")]
        public int CID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string Description { get; set; }
        [Column("CTID")]
        public int CTID { get; set; }
        [Column("PTID")]
        public int PTID { get; set; }
        [NotMapped]
        public string TrainerFullName => PersonalTrainer != null ? $"{PersonalTrainer.LastName} {PersonalTrainer.FirstName}" : "No Trainer Assigned";
        public ClassModel()
        {
            UserClasses = new List<UserClassModel>();
        }

        public ClassModel(int id, string name, string description, int classTypeId, int personalTrainerId)
        {
            CID = id;
            Name = name;
            Description = description;
            CTID = classTypeId;
            PTID = personalTrainerId;
            UserClasses = new List<UserClassModel>();
        }
        // Navigation properties
        [ForeignKey("CTID")]
        public virtual ClassTypeModel ClassType { get; set; }
        [ForeignKey("PTID")]
        public virtual PersonalTrainerModel PersonalTrainer { get; set; }
        public virtual ICollection<UserClassModel> UserClasses { get; set; }
    }
}
