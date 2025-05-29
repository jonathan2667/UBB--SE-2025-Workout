using NeoIsisJob.Proxy;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace NeoIsisJob.ViewModels.Nutrition
{
    public class NutritionViewModel
    {
        private readonly MealServiceProxy mealServiceProxy;

        public NutritionViewModel()
        {
            this.mealServiceProxy = new MealServiceProxy();
        }

        public async Task<IEnumerable<MealModel>> GetAllMealsAsync()
        {
            return await this.mealServiceProxy.GetAllAsync();
        }

        public async Task<bool> DeleteMealAsync(int id)
        {
            return await this.mealServiceProxy.DeleteAsync(id);
        }

        public async Task<MealModel> GetMealByIdAsync(int id)
        {
            return await this.mealServiceProxy.GetByIdAsync(id);
        }
    }
}
