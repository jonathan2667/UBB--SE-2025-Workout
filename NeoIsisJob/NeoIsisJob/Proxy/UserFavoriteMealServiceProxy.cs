using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    /// <summary>
    /// Service proxy for user favorite meal operations that communicates with the API.
    /// </summary>
    public class UserFavoriteMealServiceProxy : BaseServiceProxy
    {
        private readonly string apiEndpoint = "UserFavoriteMeal";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFavoriteMealServiceProxy"/> class.
        /// </summary>
        public UserFavoriteMealServiceProxy() : base()
        {
        }

        /// <summary>
        /// Gets all favorite meals for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A list of user's favorite meals.</returns>
        public async Task<IEnumerable<UserFavoriteMealModel>> GetUserFavoritesAsync(int userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var favorites = JsonSerializer.Deserialize<IEnumerable<UserFavoriteMealModel>>(jsonString, jsonOptions);
                    return favorites ?? new List<UserFavoriteMealModel>();
                }
                
                return new List<UserFavoriteMealModel>();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting user favorites: {ex.Message}");
                return new List<UserFavoriteMealModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting user favorites: {ex.Message}");
                return new List<UserFavoriteMealModel>();
            }
        }

        /// <summary>
        /// Adds a meal to user's favorites.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mealId">The meal identifier.</param>
        /// <returns>The created favorite meal entry.</returns>
        public async Task<UserFavoriteMealModel> AddToFavoritesAsync(int userId, int mealId)
        {
            try
            {
                var url = $"{apiEndpoint}/{userId}/{mealId}";
                System.Diagnostics.Debug.WriteLine($"[UserFavoriteMealServiceProxy] POST URL: {url}");
                System.Diagnostics.Debug.WriteLine($"[UserFavoriteMealServiceProxy] Full URL: {httpClient.BaseAddress}{url}");
                
                var response = await httpClient.PostAsync(url, null);
                
                System.Diagnostics.Debug.WriteLine($"[UserFavoriteMealServiceProxy] Response Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"[UserFavoriteMealServiceProxy] Response Content: {jsonString}");
                    var favorite = JsonSerializer.Deserialize<UserFavoriteMealModel>(jsonString, jsonOptions);
                    return favorite;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"[UserFavoriteMealServiceProxy] Error Content: {errorContent}");
                }
                
                throw new InvalidOperationException($"Failed to add meal to favorites. Status: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error adding to favorites: {ex.Message}");
                throw new InvalidOperationException("Failed to add meal to favorites due to network error.");
            }
        }

        /// <summary>
        /// Removes a meal from user's favorites.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mealId">The meal identifier.</param>
        /// <returns>True if removed successfully, false otherwise.</returns>
        public async Task<bool> RemoveFromFavoritesAsync(int userId, int mealId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{apiEndpoint}/{userId}/{mealId}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error removing from favorites: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing from favorites: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Checks if a meal is in user's favorites.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mealId">The meal identifier.</param>
        /// <returns>True if meal is favorite, false otherwise.</returns>
        public async Task<bool> IsMealFavoriteAsync(int userId, int mealId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/{mealId}/isfavorite");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var isFavorite = JsonSerializer.Deserialize<bool>(jsonString, jsonOptions);
                    return isFavorite;
                }
                
                return false;
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error checking favorite status: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking favorite status: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets all favorite meals for the currently logged-in user.
        /// </summary>
        /// <returns>A list of user's favorite meals.</returns>
        public async Task<IEnumerable<UserFavoriteMealModel>> GetAllAsync()
        {
            // For backward compatibility - use hardcoded user ID for now
            // In a real application, this should come from authentication/session
            return await GetUserFavoritesAsync(1);
        }

        /// <summary>
        /// Creates a new favorite meal entry.
        /// </summary>
        /// <param name="favoriteMeal">The favorite meal to create.</param>
        /// <returns>The created favorite meal entry.</returns>
        public async Task<UserFavoriteMealModel> CreateAsync(UserFavoriteMealModel favoriteMeal)
        {
            return await AddToFavoritesAsync(favoriteMeal.UserID, favoriteMeal.MealID);
        }

        /// <summary>
        /// Deletes a favorite meal entry.
        /// </summary>
        /// <param name="mealId">The meal identifier to remove from favorites.</param>
        /// <returns>True if removed successfully, false otherwise.</returns>
        public async Task<bool> DeleteAsync(int mealId)
        {
            // For backward compatibility - use hardcoded user ID for now
            // In a real application, this should come from authentication/session
            return await RemoveFromFavoritesAsync(1, mealId);
        }
    }
} 