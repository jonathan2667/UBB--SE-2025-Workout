namespace Workout.Core.Utils.Filters
{
    /// <summary>
    /// Represents a filter for meals, allowing filtering by type, cooking level, cooking time range, and calorie range.
    /// </summary>
    public class MealFilter : IFilter
    {
        /// <summary>
        /// Gets or sets the type of the meal (e.g., breakfast, lunch, dinner, snack).
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the cooking level required for the meal (e.g., easy, medium, hard).
        /// </summary>
        public string? CookingLevel { get; set; }

        /// <summary>
        /// Gets or sets the cooking time range filter (quick, medium, long).
        /// </summary>
        public string? CookingTimeRange { get; set; }

        /// <summary>
        /// Gets or sets the calorie range filter (low, medium, high).
        /// </summary>
        public string? CalorieRange { get; set; }
    }
}
