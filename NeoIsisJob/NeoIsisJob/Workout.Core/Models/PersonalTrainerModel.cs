using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("PersonalTrainers")]
    public class PersonalTrainerModel
    {
        [Key]
        [Column("PTID")]
        public int PTID { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Column("WorksSince")]
        public DateTime WorksSince { get; set; }
        [NotMapped]
        public string Name => $"{FirstName} {LastName}";

        public PersonalTrainerModel()
        {
        }

        public PersonalTrainerModel(int id, string firstName, string lastName, DateTime worksSince)
        {
            PTID = id;
            FirstName = firstName;
            LastName = lastName;
            WorksSince = worksSince;
        }
    }
}
