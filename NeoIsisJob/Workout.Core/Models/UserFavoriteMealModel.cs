using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Workout.Core.Models
{
    /// <summary>
    /// Represents a user's favorite meal.
    /// </summary>
    [Table("UserFavoriteMeal")]
    public class UserFavoriteMealModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int MealID { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("MealID")]
        public MealModel? Meal { get; set; }

        [ForeignKey("UserID")]
        [JsonIgnore]
        public UserModel? User { get; set; }
    }
} 