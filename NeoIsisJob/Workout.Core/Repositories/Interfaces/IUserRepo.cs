using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    internal interface IUserRepo
    {
        UserModel GetUserById(int userId);
        int InsertUser();
        bool DeleteUserById(int userId);
        List<UserModel> GetAllUsers();
    }
}
