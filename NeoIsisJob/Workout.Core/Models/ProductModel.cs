namespace Workout.Core.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Product")]
    public class ProductModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [MaxLength(50)]
        public string Size { get; set; } = "N/A";

        [MaxLength(50)]
        public string Color { get; set; } = "N/A";

        [MaxLength(255)]
        public string Description { get; set; } = "";

        [MaxLength(255)]
        public string PhotoURL { get; set; }

        [ForeignKey("CategoryID")]
        public CategoryModel Category { get; set; }

        public ICollection<CartItemModel> CartItems { get; set; }
        public ICollection<WishlistItemModel> WishlistItems { get; set; }
        public ICollection<OrderItemModel> OrderItems { get; set; }
    }
}