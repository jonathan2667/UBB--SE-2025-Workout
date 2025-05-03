using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Core.Models
{
    internal class UserClassModel
    {
        private int userId;
        private int classId;
        private DateTime enrollmentDate;

        public int UserId { get => userId; set => userId = value; }
        public int ClassId { get => classId; set => classId = value; }
        public DateTime EnrollmentDate { get => enrollmentDate; set => enrollmentDate = value; }

        public UserClassModel()
        {
        }

        public UserClassModel(int userId, int classId, DateTime enrollmentDate)
        {
            UserId = userId;
            ClassId = classId;
            EnrollmentDate = enrollmentDate;
        }
    }
}
