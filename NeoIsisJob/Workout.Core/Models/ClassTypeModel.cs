using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("ClassTypes")]
    public class ClassTypeModel
    {
        [Key]
        [Column("CTID")]
        public int CTID { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ClassTypeModel()
        {
        }

        public ClassTypeModel(int id, string name)
        {
            CTID = id;
            Name = name;
        }
    }
}
