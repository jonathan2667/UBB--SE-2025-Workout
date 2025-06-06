using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Core.IRepositories;

namespace Workout.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        }

        public async Task<int> RegisterNewUserAsync()
        {
            // No parameters to validate here; delegate to repository
            return await _userRepo
                         .InsertUserAsync()
                         .ConfigureAwait(false);
        }

        public async Task<int> AddUserAsync(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            return await _userRepo.InsertUserAsync(username, email, password);
        }

        public async Task<UserModel> GetUserAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");
            }

            return await _userRepo
                         .GetUserByIdAsync(userId)
                         .ConfigureAwait(false);
        }

        public async Task<bool> RemoveUserAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");
            }

            return await _userRepo
                         .DeleteUserByIdAsync(userId)
                         .ConfigureAwait(false);
        }

        public async Task<IList<UserModel>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return users;
        }

        public async Task<long> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            var user = await _userRepo.GetUserByUsernameAsync(username);
            
            if (user == null)
                return -2; // User not found
            
            return user.Password == password ? user.ID : -1; // -1 for wrong password
        }
    }
}
