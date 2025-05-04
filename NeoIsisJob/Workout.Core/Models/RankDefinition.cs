using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Workout.Core.Models
{
    public class RankDefinition
    {
        public string Name { get; set; }
        public int MinPoints { get; set; }
        public int MaxPoints { get; set; }
        public Color Color { get; set; }
        public string ImagePath { get; set; }
    }
}
