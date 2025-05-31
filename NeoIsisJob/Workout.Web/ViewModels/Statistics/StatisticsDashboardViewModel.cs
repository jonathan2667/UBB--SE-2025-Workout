using Workout.Core.Models;

namespace Workout.Web.ViewModels.Statistics
{
    /// <summary>
    /// View model for the statistics dashboard containing nutrition and water tracking data.
    /// </summary>
    public class StatisticsDashboardViewModel
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the current date for the dashboard.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets today's nutrition summary.
        /// </summary>
        public UserDailyNutritionModel TodayNutrition { get; set; }

        /// <summary>
        /// Gets or sets the weekly nutrition average.
        /// </summary>
        public UserDailyNutritionModel WeeklyAverage { get; set; }

        /// <summary>
        /// Gets or sets the monthly nutrition average.
        /// </summary>
        public UserDailyNutritionModel MonthlyAverage { get; set; }

        /// <summary>
        /// Gets or sets the top meal types consumed by the user.
        /// </summary>
        public Dictionary<string, int> TopMealTypes { get; set; }

        /// <summary>
        /// Gets or sets today's water intake in milliliters.
        /// </summary>
        public int TodayWaterIntake { get; set; }

        /// <summary>
        /// Gets or sets the user's daily water goal in milliliters.
        /// </summary>
        public int WaterGoal { get; set; }

        /// <summary>
        /// Gets or sets the water intake progress percentage.
        /// </summary>
        public double WaterProgress { get; set; }

        /// <summary>
        /// Gets or sets the water intake history for the last 7 days.
        /// </summary>
        public Dictionary<DateTime, int> WaterHistory { get; set; }

        /// <summary>
        /// Gets or sets today's meal log entries.
        /// </summary>
        public IEnumerable<UserMealLogModel> TodayMealLogs { get; set; }

        /// <summary>
        /// Gets or sets the nutrition trends for the last 7 days.
        /// </summary>
        public IEnumerable<UserDailyNutritionModel> NutritionTrends { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsDashboardViewModel"/> class.
        /// </summary>
        public StatisticsDashboardViewModel()
        {
            Date = DateTime.Today;
            TodayNutrition = new UserDailyNutritionModel();
            WeeklyAverage = new UserDailyNutritionModel();
            MonthlyAverage = new UserDailyNutritionModel();
            TopMealTypes = new Dictionary<string, int>();
            WaterHistory = new Dictionary<DateTime, int>();
            TodayMealLogs = new List<UserMealLogModel>();
            NutritionTrends = new List<UserDailyNutritionModel>();
        }

        /// <summary>
        /// Gets the protein percentage of total calories for today.
        /// </summary>
        public double TodayProteinPercentage
        {
            get
            {
                if (TodayNutrition?.TotalCalories > 0)
                {
                    // 1 gram of protein = 4 calories
                    return (TodayNutrition.TotalProteins * 4 / TodayNutrition.TotalCalories) * 100;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the carbohydrate percentage of total calories for today.
        /// </summary>
        public double TodayCarbohydratePercentage
        {
            get
            {
                if (TodayNutrition?.TotalCalories > 0)
                {
                    // 1 gram of carbohydrate = 4 calories
                    return (TodayNutrition.TotalCarbohydrates * 4 / TodayNutrition.TotalCalories) * 100;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the fat percentage of total calories for today.
        /// </summary>
        public double TodayFatPercentage
        {
            get
            {
                if (TodayNutrition?.TotalCalories > 0)
                {
                    // 1 gram of fat = 9 calories
                    return (TodayNutrition.TotalFats * 9 / TodayNutrition.TotalCalories) * 100;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user is on track with their nutrition goals.
        /// </summary>
        public bool IsOnTrackNutrition
        {
            get
            {
                // Simple heuristic: balanced macros and reasonable calorie intake
                var proteinPct = TodayProteinPercentage;
                var carbPct = TodayCarbohydratePercentage;
                var fatPct = TodayFatPercentage;
                
                return proteinPct >= 15 && proteinPct <= 35 &&
                       carbPct >= 45 && carbPct <= 65 &&
                       fatPct >= 20 && fatPct <= 35 &&
                       TodayNutrition?.TotalCalories >= 1200 &&
                       TodayNutrition?.TotalCalories <= 3000;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user is on track with their water intake.
        /// </summary>
        public bool IsOnTrackWater => WaterProgress >= 80;
    }
} 