namespace Workout.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OrderItem")]
    public class OrderItemModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemModel"/> class.
        /// </summary>
        public OrderItemModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemModel"/> class with specified order, product, and quantity.
        /// </summary>
        /// <param name="orderID">The ID of the order.</param>
        /// <param name="productID">The ID of the product.</param>
        /// <param name="quantity">The quantity of the product.</param>
        public OrderItemModel(int orderID, int productID, int quantity)
        {
            this.OrderID = orderID;
            this.ProductID = productID;
            this.Quantity = quantity;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the order item.
        /// </summary>
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
