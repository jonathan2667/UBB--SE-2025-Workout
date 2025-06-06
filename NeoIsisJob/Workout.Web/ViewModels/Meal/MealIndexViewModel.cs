using Workout.Core.Models;
using Workout.Core.Utils.Filters;

namespace Workout.Web.ViewModels.Meal
{
    public class MealIndexViewModel
    {
        public IEnumerable<MealModel> Meals { get; set; } = new List<MealModel>();
        public MealFilter Filter { get; set; } = new MealFilter();

        // Filter options for dropdowns
        public List<string> MealTypes { get; set; } = new List<string>
        {
            "Breakfast", "Lunch", "Dinner", "Snack"
        };

        public List<string> CookingLevels { get; set; } = new List<string>
        {
            "Easy", "Medium", "Hard"
        };

        public List<string> CookingTimeRanges { get; set; } = new List<string>
        {
            "Quick", "Medium", "Long"
        };

        public List<string> CalorieRanges { get; set; } = new List<string>
        {
            "Low", "Medium", "High"
        };
    }
} 