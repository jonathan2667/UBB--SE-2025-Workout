using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface IUserRepo
    {
        Task<UserModel?> GetUserByIdAsync(int userId);
        Task<UserModel?> GetUserByUsernameAsync(string username);
        Task<int> InsertUserAsync();
        Task<int> InsertUserAsync(string username, string email, string password);
        Task<bool> DeleteUserByIdAsync(int userId);
        Task<List<UserModel>> GetAllUsersAsync();
    }
}
