using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    public interface IUserRepo
    {
        Task<UserModel?> GetUserByIdAsync(int userId);
        Task<int> InsertUserAsync();
        Task<bool> DeleteUserByIdAsync(int userId);
        Task<List<UserModel>> GetAllUsersAsync();
    }
}
