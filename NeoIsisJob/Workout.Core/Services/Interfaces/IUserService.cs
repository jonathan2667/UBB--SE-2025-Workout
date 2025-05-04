using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user and returns the new user ID.
        /// </summary>
        Task<int> RegisterNewUserAsync();

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        Task<UserModel> GetUserAsync(int userId);

        /// <summary>
        /// Removes a user by their ID. Returns true if deletion was successful.
        /// </summary>
        Task<bool> RemoveUserAsync(int userId);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        Task<IList<UserModel>> GetAllUsersAsync();
    }

}
