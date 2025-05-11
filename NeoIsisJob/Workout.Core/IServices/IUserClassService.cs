using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IServices
{
    public interface IUserClassService
    {
        Task<List<UserClassModel>> GetAllUserClassesAsync();
        Task<UserClassModel> GetUserClassByIdAsync(int userId, int classId, DateTime date);
        Task AddUserClassAsync(UserClassModel userClassModel);
        Task DeleteUserClassAsync(int userId, int classId, DateTime date);
    }
}
