using System.Text.Json.Serialization;

namespace Workout.Core.Models
{
    public class IngredientModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<MealModel> Meals { get; set; } = new();
    }
}