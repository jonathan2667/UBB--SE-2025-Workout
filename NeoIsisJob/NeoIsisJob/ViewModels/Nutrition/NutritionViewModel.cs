using NeoIsisJob.Proxy;

namespace NeoIsisJob.ViewModels.Nutrition
{
    class NutritionViewModel
    {
        private readonly MealServiceProxy mealServiceProxy;

        public NutritionViewModel()
        {
            this.mealServiceProxy = new MealServiceProxy();
        }

    }
}
