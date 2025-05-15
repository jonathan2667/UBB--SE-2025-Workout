using System.ComponentModel.DataAnnotations;
using Workout.Core.Models;

namespace Workout.Web.ViewModels.Shop
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(255, ErrorMessage = "Description cannot be longer than 255 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "Size is required")]
        public string Size { get; set; } = string.Empty;

        [Required(ErrorMessage = "Photo URL is required")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string PhotoURL { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive number")]
        public int Stock { get; set; }

        public IEnumerable<CategoryModel> Categories { get; set; } = [];
    }
} 