namespace Workout.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OrderItem")]
    public class OrderItemModel
    {
        public OrderItemModel()
        {
        }

        public OrderItemModel(int orderId, int productId, int quantity)
        {
            this.OrderID = orderId;
            this.ProductID = productId;
            this.Quantity = quantity;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("OrderID")]
        public OrderModel Order { get; set; }

        [ForeignKey("ProductID")]
        public ProductModel Product { get; set; }
    }
}
