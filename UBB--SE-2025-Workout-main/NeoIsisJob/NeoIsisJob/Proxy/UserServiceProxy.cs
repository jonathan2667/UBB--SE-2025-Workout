using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Json;
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

        public UserModel GetUserByUsername(string username)
        {
            var response = this.httpClient.GetAsync($"user/user?username={username}").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var user = response.Content.ReadFromJsonAsync<UserModel>().Result;
                    return user;
                }
            }

            return null;
        }

        public long AddUser(string username, string password, string image)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be empty", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be empty", nameof(password));
            }

            var user = new UserModel
            {
                Username = username,
                Password = password,
            };

            var response = this.httpClient.PostAsJsonAsync(string.Empty, user).Result;
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Failed to add user. Status: {response.StatusCode}");
            }

            return -1;
        }

        public List<UserModel> GetUserFollowing(long id)
        {
            var response = this.httpClient.GetAsync($"{id}/following").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<UserModel>>().Result ?? new List<UserModel>();
            }

            Debug.WriteLine($"Failed to get following for user {id}. Status: {response.StatusCode}");
            return new List<UserModel>();
        }

        public void FollowUserById(long userId, long whoToFollowId)
        {
            var response = this.httpClient.PostAsJsonAsync($"{userId}/followers", whoToFollowId).Result;
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Failed to follow. Status: {response.StatusCode}");
            }
        }

        public void UnfollowUserById(long userId, long whoToUnfollowId)
        {
            var response = this.httpClient.DeleteAsync($"{userId}/followers/{whoToUnfollowId}").Result;
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Failed to unfollow. Status: {response.StatusCode}");
            }
        }
    }
}