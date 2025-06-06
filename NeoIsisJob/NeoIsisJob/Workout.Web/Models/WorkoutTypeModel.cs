using System.ComponentModel.DataAnnotations;

namespace Workout.Web.Models
{
    public class WorkoutTypeModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Type Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
        
        // For direct API compatibility
        public int WTID { get => Id; set => Id = value; }
    }
} 