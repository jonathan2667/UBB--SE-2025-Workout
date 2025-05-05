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
        private readonly IUserRepo _userRepository;

        public UserService(IUserRepo userRepo)
        {
            _userRepository = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        }

        public async Task<int> RegisterNewUserAsync()
        {
            // No parameters to validate here; delegate to repository
            return await _userRepository
                         .InsertUserAsync()
                         .ConfigureAwait(false);
        }

        public async Task<UserModel> GetUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");

            return await _userRepository
                         .GetUserByIdAsync(userId)
                         .ConfigureAwait(false);
        }

        public async Task<bool> RemoveUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");

            return await _userRepository
                         .DeleteUserByIdAsync(userId)
                         .ConfigureAwait(false);
        }

        public async Task<IList<UserModel>> GetAllUsersAsync()
        {
            return await _userRepository
                         .GetAllUsersAsync()
                         .ConfigureAwait(false);
        }
    }
}
