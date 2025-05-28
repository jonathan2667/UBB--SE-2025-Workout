namespace Workout.Core.Models
{
    public class IngredientModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MealModel> Meals { get; set; }
    }
}