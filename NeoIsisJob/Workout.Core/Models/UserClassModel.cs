using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;    // ← add this

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

        // navigation props must be ignored & nullable:
        [JsonIgnore]
        [ForeignKey("UID")]
        public virtual UserModel? User { get; set; }

        [JsonIgnore]
        [ForeignKey("CID")]
        public virtual ClassModel? Class { get; set; }
    }
}
