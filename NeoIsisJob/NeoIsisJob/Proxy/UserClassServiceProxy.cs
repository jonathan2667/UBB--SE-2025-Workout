using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class UserClassServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "userclass";

        public UserClassServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        public async Task<List<UserClassModel>> GetAllUserClassesAsync()
        {
            try
            {
                var results = await GetAsync<List<UserClassModel>>($"{EndpointName}");
                return results ?? new List<UserClassModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all user classes: {ex.Message}");
                return new List<UserClassModel>();
            }
        }

        public async Task<UserClassModel> GetUserClassByIdAsync(int userId, int classId, DateTime date)
        {
            try
            {
                // Format date to avoid URL encoding issues
                string formattedDate = date.ToString("yyyy-MM-dd");
                var result = await GetAsync<UserClassModel>($"{EndpointName}/{userId}/{classId}/{formattedDate}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user class: {ex.Message}");
                throw;
            }
        }

        public async Task AddUserClassAsync(UserClassModel userClassModel)
        {
            try
            {
                await PostAsync($"{EndpointName}", userClassModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user class: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUserClassAsync(int userId, int classId, DateTime date)
        {
            try
            {
                string formattedDate = date.ToString("yyyy-MM-dd");
                await DeleteAsync($"{EndpointName}/{userId}/{classId}/{formattedDate}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user class: {ex.Message}");
                throw;
            }
        }
    }
}