using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    /// <summary>
    /// Legacy proxy for favorite meals. This is now redirected to use UserFavoriteMealServiceProxy for proper API communication.
    /// </summary>
    public class FavoriteMealServiceProxy
    {
        private readonly UserFavoriteMealServiceProxy _userFavoriteMealProxy;

        public FavoriteMealServiceProxy()
        {
            _userFavoriteMealProxy = new UserFavoriteMealServiceProxy();
        }

        public async Task<IEnumerable<UserFavoriteMealModel>> GetAllAsync()
        {
            return await _userFavoriteMealProxy.GetAllAsync();
        }

        public async Task<UserFavoriteMealModel> CreateAsync(UserFavoriteMealModel favoriteMeal)
        {
            return await _userFavoriteMealProxy.CreateAsync(favoriteMeal);
        }

        public async Task<bool> DeleteAsync(int mealId)
        {
            return await _userFavoriteMealProxy.DeleteAsync(mealId);
        }
    }
} 