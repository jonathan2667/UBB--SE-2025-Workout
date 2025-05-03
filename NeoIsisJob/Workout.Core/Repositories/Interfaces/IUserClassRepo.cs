using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    internal interface IUserClassRepo
    {
        UserClassModel GetUserClassModelById(int userId, int classId, DateTime enrollmentDate);
        List<UserClassModel> GetAllUserClassModel();
        void AddUserClassModel(UserClassModel userClass);
        void DeleteUserClassModel(int userId, int classId, DateTime enrollmentDate);
        List<UserClassModel> GetUserClassModelByDate(DateTime date);
    }
}
