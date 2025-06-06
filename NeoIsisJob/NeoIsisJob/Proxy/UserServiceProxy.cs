using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class UserServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "user";

        public UserServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        public async Task<int> RegisterNewUserAsync()
        {
            try
            {
                var result = await PostAsync<int>($"{EndpointName}/register", null);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering new user: {ex.Message}");
                throw;
            }
        }

        public async Task<UserModel> GetUserAsync(int userId)
        {
            try
            {
                var result = await GetAsync<UserModel>($"{EndpointName}/{userId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> RemoveUserAsync(int userId)
        {
            try
            {
                var result = await DeleteAsync<bool>($"{EndpointName}/{userId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing user: {ex.Message}");
                throw;
            }
        }

        public async Task<IList<UserModel>> GetAllUsersAsync()
        {
            try
            {
                var results = await GetAsync<IList<UserModel>>($"{EndpointName}");
                return results ?? new List<UserModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all users: {ex.Message}");
                return new List<UserModel>();
            }
        }
    }
}