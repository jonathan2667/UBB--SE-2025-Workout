using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class UserWorkoutServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "userworkout";

        public UserWorkoutServiceProxy(IConfiguration configuration = null) 
            : base(configuration)
        {
        }

        public async Task<UserWorkoutModel> GetUserWorkoutForDateAsync(int userId, DateTime date)
        {
            try
            {
                // Format date to avoid URL encoding issues
                string formattedDate = date.ToString("yyyy-MM-dd");
                var result = await GetAsync<UserWorkoutModel>($"{EndpointName}/date/{userId}/{formattedDate}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user workout for date: {ex.Message}");
                throw;
            }
        }

        public async Task AddUserWorkoutAsync(UserWorkoutModel userWorkout)
        {
            try
            {
                await PostAsync($"{EndpointName}", userWorkout);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user workout: {ex.Message}");
                throw;
            }
        }

        public async Task CompleteUserWorkoutAsync(int userId, int workoutId, DateTime date)
        {
            try
            {
                string formattedDate = date.ToString("yyyy-MM-dd");
                await PostAsync($"{EndpointName}/complete/{userId}/{workoutId}/{formattedDate}", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error completing user workout: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUserWorkoutAsync(int userId, int workoutId, DateTime date)
        {
            try
            {
                string formattedDate = date.ToString("yyyy-MM-dd");
                await DeleteAsync($"{EndpointName}/{userId}/{workoutId}/{formattedDate}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user workout: {ex.Message}");
                throw;
            }
        }
    }
} 