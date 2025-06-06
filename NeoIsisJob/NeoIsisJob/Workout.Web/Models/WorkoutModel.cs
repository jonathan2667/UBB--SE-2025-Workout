using System.ComponentModel.DataAnnotations;

namespace Workout.Web.Models
{
    public class WorkoutModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Workout Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Workout Type")]
        public int WorkoutTypeId { get; set; }

        [Display(Name = "Workout Type")]
        public string WorkoutTypeName { get; set; } = string.Empty;

        // For direct API compatibility - these match the Core model
        public int WID { get => Id; set => Id = value; }
        public int WTID { get => WorkoutTypeId; set => WorkoutTypeId = value; }
        
        // Required by the API - must match exactly the Core model
        public WorkoutTypeModel WorkoutType { get; set; }

        public WorkoutModel()
        {
            // Initialize WorkoutType to avoid null validation errors
            WorkoutType = new WorkoutTypeModel();
        }
    }
} 