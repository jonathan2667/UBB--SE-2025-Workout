using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Core.Models
{
    public class UserModel
    {
        private int id;

        public int Id { get => id; set => id = value; }

        public UserModel()
        {
        }

        public UserModel(int id)
        {
            Id = id;
        }
    }
}
