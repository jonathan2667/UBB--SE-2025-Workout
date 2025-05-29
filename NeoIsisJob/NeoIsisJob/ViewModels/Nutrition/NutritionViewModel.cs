using NeoIsisJob.Proxy;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Utils.Filters;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NeoIsisJob.ViewModels.Nutrition
{
    public class NutritionViewModel : INotifyPropertyChanged
    {
        private readonly MealServiceProxy mealServiceProxy;
        private MealFilter _currentFilter;

        public NutritionViewModel()
        {
            this.mealServiceProxy = new MealServiceProxy();
            this._currentFilter = new MealFilter();
        }

        public MealFilter CurrentFilter
        {
            get => _currentFilter;
            set
            {
                _currentFilter = value;
                OnPropertyChanged();
            }
        }

        // Filter options for UI binding
        public List<string> MealTypes { get; } = new List<string>
        {
            "", "Breakfast", "Lunch", "Dinner", "Snack"
        };

        public List<string> CookingLevels { get; } = new List<string>
        {
            "", "Easy", "Medium", "Hard"
        };

        public List<string> CookingTimeRanges { get; } = new List<string>
        {
            "", "Quick", "Medium", "Long"
        };

        public List<string> CalorieRanges { get; } = new List<string>
        {
            "", "Low", "Medium", "High"
        };

        public async Task<IEnumerable<MealModel>> GetAllMealsAsync()
        {
            return await this.mealServiceProxy.GetAllAsync();
        }

        public async Task<IEnumerable<MealModel>> GetFilteredMealsAsync(MealFilter filter)
        {
            if (IsFilterEmpty(filter))
            {
                return await this.mealServiceProxy.GetAllAsync();
            }
            return await this.mealServiceProxy.GetFilteredAsync(filter);
        }

        public async Task<bool> DeleteMealAsync(int id)
        {
            return await this.mealServiceProxy.DeleteAsync(id);
        }

        public async Task<MealModel> GetMealByIdAsync(int id)
        {
            return await this.mealServiceProxy.GetByIdAsync(id);
        }

        private bool IsFilterEmpty(MealFilter filter)
        {
            return filter == null ||
                   (string.IsNullOrEmpty(filter.Type) &&
                    string.IsNullOrEmpty(filter.CookingLevel) &&
                    string.IsNullOrEmpty(filter.CookingTimeRange) &&
                    string.IsNullOrEmpty(filter.CalorieRange));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
