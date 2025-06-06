using System.ComponentModel.DataAnnotations;

namespace Workout.Web.ViewModels.Meal
{
    public class CreateMealViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cooking level is required")]
        public string CookingLevel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cooking time is required")]
        [Range(1, 480, ErrorMessage = "Cooking time must be between 1 and 480 minutes")]
        public int CookingTimeMins { get; set; }

        [Required(ErrorMessage = "Directions are required")]
        public string Directions { get; set; } = string.Empty;

        [Required(ErrorMessage = "Calories are required")]
        [Range(0, 5000, ErrorMessage = "Calories must be between 0 and 5000")]
        public int Calories { get; set; }

        [Required(ErrorMessage = "Proteins are required")]
        [Range(0, 500, ErrorMessage = "Proteins must be between 0 and 500 grams")]
        public double Proteins { get; set; }

        [Required(ErrorMessage = "Carbohydrates are required")]
        [Range(0, 500, ErrorMessage = "Carbohydrates must be between 0 and 500 grams")]
        public double Carbohydrates { get; set; }

        [Required(ErrorMessage = "Fats are required")]
        [Range(0, 500, ErrorMessage = "Fats must be between 0 and 500 grams")]
        public double Fats { get; set; }
    }
} 