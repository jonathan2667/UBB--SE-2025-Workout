namespace Workout.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Workout.Core.Models;

    [Table("CartItem")]
    public class CartItemModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("ProductID")]
        public ProductModel Product { get; set; }

        [ForeignKey("UserID")]
        public UserModel User { get; set; }
    }
}
