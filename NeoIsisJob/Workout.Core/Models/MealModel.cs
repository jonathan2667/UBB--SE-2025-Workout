using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Core.Models
{
    public class MealModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public string CookingLevel { get; set; }
        public int CookingTimeMins { get; set; }
        public string Directions { get; set; }
        public List<IngredientModel> Ingredients { get; set; }
    }
}
