namespace Workout.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Workout.Core.Models;

    [Table("Order")]
    public class OrderModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [ForeignKey("UserID")]
        public UserModel User { get; set; }

        public ICollection<OrderItemModel> OrderItems { get; set; }
    }
}
