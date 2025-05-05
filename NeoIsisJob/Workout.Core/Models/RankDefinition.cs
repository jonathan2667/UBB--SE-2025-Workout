using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Workout.Core.Models
{
    [Table("RankDefinitions")]
    public class RankDefinition
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        public int MinPoints { get; set; }
        
        public int MaxPoints { get; set; }
        
        [NotMapped]
        public Color Color { get; set; }
        
        [Column("ColorValue")]
        public int ColorArgb
        {
            get => Color.ToArgb();
            set => Color = Color.FromArgb(value);
        }
        
        [MaxLength(255)]
        public string ImagePath { get; set; }
    }
}
