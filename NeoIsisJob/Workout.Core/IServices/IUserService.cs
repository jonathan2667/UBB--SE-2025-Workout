﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IServices
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user and returns the new user ID.
        /// </summary>
        Task<int> RegisterNewUserAsync();

        /// <summary>
        /// Adds a new user with credentials and returns the new user ID.
        /// </summary>
        Task<int> AddUserAsync(string username, string email, string password);

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

        /// <summary>
        /// Authenticates a user and returns user ID if successful.
        /// </summary>
        Task<long> LoginAsync(string username, string password);
    }
}
