using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface IUserClassRepo
    {
        Task<UserClassModel?> GetUserClassModelByIdAsync(int userId, int classId, DateTime enrollmentDate);
        Task<List<UserClassModel>> GetAllUserClassModelAsync();
        Task AddUserClassModelAsync(UserClassModel userClass);
        Task DeleteUserClassModelAsync(int userId, int classId, DateTime enrollmentDate);
        Task<List<UserClassModel>> GetUserClassModelByDateAsync(DateTime date);
    }
}
