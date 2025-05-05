using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("UserClasses")]
    public class UserClassModel
    {
        [Column("UID")]
        public int UID { get; set; }
        
        [Column("CID")]
        public int CID { get; set; }
        
        [Column("Date")]
        public DateTime Date { get; set; }

        public UserClassModel()
        {
        }

        public UserClassModel(int userId, int classId, DateTime date)
        {
            UID = userId;
            CID = classId;
            Date = date;
        }
        
        // Navigation properties
        [ForeignKey("UID")]
        public virtual UserModel User { get; set; }
        
        [ForeignKey("CID")]
        public virtual ClassModel Class { get; set; }
    }
}
