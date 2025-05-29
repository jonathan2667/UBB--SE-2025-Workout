namespace Workout.Core.Utils.Filters
{
    /// <summary>
    /// Represents a filter for meals, allowing filtering by type, cooking level, search term, and maximum cooking time.
    /// </summary>
    public class MealFilter : IFilter
    {
        /// <summary>
        /// Gets or sets the type of the meal (e.g., breakfast, lunch, dinner).
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the cooking level required for the meal (e.g., beginner, intermediate, expert).
        /// </summary>
        public string? CookingLevel { get; set; }

        /// <summary>
        /// Gets or sets the search term to filter meals by name or description.
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the maximum cooking time (in minutes) for the meal.
        /// </summary>
        public int? MaxCookingTime { get; set; }
    }
}
